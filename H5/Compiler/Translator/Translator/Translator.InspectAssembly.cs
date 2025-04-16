#undef PARALLEL
using H5.Contract;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.TypeSystem;
using Mono.Cecil;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
using Mosaik.Core;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace H5.Translator
{
    public partial class Translator
    {
        private static readonly Dictionary<string, (AssemblyDefinition assembly, DateTime timestamp, long size)> _loadedAssemblies       = new Dictionary<string, (AssemblyDefinition assembly, DateTime timestamp, long size)>();
        private static readonly Dictionary<string, Stream>                                                       _loadedAssemblieStreams = new Dictionary<string, Stream>();
        private static readonly object                                                                           _loadedAssembliesLock   = new object();

        private static ILogger Logger = ApplicationLogging.CreateLogger<Translator>();

        public Stack<string> CurrentAssemblyLocationInspected { get; set; } = new Stack<string>();

        private class CecilAssemblyResolver : DefaultAssemblyResolver
        {
            private static ILogger Logger = ApplicationLogging.CreateLogger<CecilAssemblyResolver>();

            public CecilAssemblyResolver(string location)
            {
                ResolveFailure += CecilAssemblyResolver_ResolveFailure;
                AddSearchDirectory(Path.GetDirectoryName(location));
            }

            private AssemblyDefinition CecilAssemblyResolver_ResolveFailure(object sender, AssemblyNameReference reference)
            {
                string fullName = reference != null ? reference.FullName : "";
                Logger.ZLogTrace("CecilAssemblyResolver: ResolveFailure {0}", (fullName ?? ""));

                return null;
            }

            public override AssemblyDefinition Resolve(AssemblyNameReference name)
            {
                string fullName = name != null ? name.FullName : "";

                Logger.ZLogTrace("CecilAssemblyResolver: Resolve(AssemblyNameReference) {0}", (fullName ?? ""));

                return base.Resolve(name);
            }

            public override AssemblyDefinition Resolve(AssemblyNameReference name, ReaderParameters parameters)
            {
                string fullName = name != null ? name.FullName : "";

                Logger.ZLogTrace("CecilAssemblyResolver: Resolve(AssemblyNameReference, ReaderParameters) {0}, {1}", (fullName ?? ""), (parameters != null ? parameters.ReadingMode.ToString() : ""));

                return base.Resolve(name, parameters);
            }
        }

        protected virtual void LoadReferenceAssemblies(List<AssemblyDefinition> references)
        {
            var locations = GetProjectReferenceAssemblies().Distinct();

            foreach (var path in locations)
            {
                var assemblyDefinition = LoadOrGetFromCache(path);

                if (assemblyDefinition is object && !references.Any(a => a.Name.Name == assemblyDefinition.Name.Name))
                {
                    references.Add(assemblyDefinition);
                }
            }
        }

        private AssemblyDefinition LoadOrGetFromCache(string path)
        {
            AssemblyDefinition assemblyDefinition;
            var                fileInfo = new FileInfo(path);

            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException(path);
            }

            lock (_loadedAssembliesLock)
            {
                if (!_loadedAssemblies.TryGetValue(path, out var assemblyData) || (assemblyData.size != fileInfo.Length || assemblyData.timestamp != fileInfo.LastWriteTimeUtc))
                {
                    if (_loadedAssemblieStreams.Remove(path, out var oldStream))
                    {
                        oldStream.Close();
                        oldStream.Dispose();
                    }

                    assemblyDefinition = AssemblyDefinition.ReadAssembly(LoadAssemblyAsFileStream(path),
                        new ReaderParameters()
                        {
                            ReadingMode      = ReadingMode.Deferred,
                            AssemblyResolver = new CecilAssemblyResolver(AssemblyLocation)
                        }
                    );


                    _loadedAssemblies[path] = (assemblyDefinition, fileInfo.LastWriteTimeUtc, fileInfo.Length);
                }
                else
                {
                    assemblyDefinition = assemblyData.assembly;
                }
            }

            return assemblyDefinition;
        }

        protected virtual AssemblyDefinition LoadAssembly(string location, List<AssemblyDefinition> references, Dictionary<string, string> discoveredAssemblyPaths)
        {
            Logger.ZLogTrace("Assembly definition loading {0} ...", (location ?? ""));

            if (CurrentAssemblyLocationInspected.Contains(location))
            {
                var message = string.Format("There is a circular reference found for assembly location {0}. To avoid the error, rename your project's assembly to be different from that location.", location);
                Logger.ZLogError(message);
                throw new InvalidOperationException(message);
            }

            CurrentAssemblyLocationInspected.Push(location);

            var assemblyDefinition = LoadOrGetFromCache(location);

            foreach (AssemblyNameReference r in assemblyDefinition.MainModule.AssemblyReferences)
            {
                var name = r.Name;

                if (name == SystemAssemblyName || name == "System.Core")
                {
                    continue;
                }

                var fullName = r.FullName;

                if (references.Any(a => a.Name.FullName == fullName))
                {
                    continue;
                }

                var path = Path.Combine(Path.GetDirectoryName(location), name) + ".dll";

                if (!File.Exists(path) && discoveredAssemblyPaths is object && discoveredAssemblyPaths.TryGetValue(name, out var discoveredPath))
                {
                    path = discoveredPath;
                }

                var updateH5Location = name.ToLowerInvariant() == "h5" && (string.IsNullOrWhiteSpace(H5Location) || !File.Exists(H5Location));

                if (updateH5Location)
                {
                    H5Location = path;
                }

                var reference = LoadAssembly(path, references, discoveredAssemblyPaths);

                if (reference != null && !references.Any(a => a.Name.FullName == reference.Name.FullName))
                {
                    references.Add(reference);
                }
            }

            Logger.ZLogTrace("Assembly definition loading {0} done", (location ?? ""));

            var cl = CurrentAssemblyLocationInspected.Pop();

            if (cl != location)
            {
                throw new InvalidOperationException(string.Format("Current location {0} is not the current location in stack {1}", location, cl));
            }

            return assemblyDefinition;
        }

        private static Stream LoadAssemblyAsFileStream(string location)
        {
            Stream stream = null;

            for (int i = 0; i < 5; i++)
            {
                try
                {
                    //Must be a FileStream, as it needs the path info attached
                    stream = File.Open(location, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
                    break;
                }
                catch (Exception E)
                {
                    Logger.ZLogWarning(E, "Failed to open file {0}, {1}", location, i < 4 ? " will try again in a few milliseconds" : "throwing...");
                }
                Thread.Sleep(100 + i * 250);
            }

            if (stream is object)
            {
                //Always add the stream to the Cache, so it gets disposed after
                _loadedAssemblieStreams.Add(location, stream); //Should throw in case it's already there - but should never happen as this method is only called from within a locked section

                return stream;
            }
            else
            {
                throw new IOException($"Access to the path {location} is denied.");
            }
        }

        protected virtual void ReadTypes(AssemblyDefinition assembly)
        {
            var name = (assembly != null && assembly.Name != null && assembly.Name.Name != null ? assembly.Name.Name : "");
            Logger.ZLogTrace("Reading types for assembly {0} ...", name);

            AddNestedTypes(assembly.MainModule.Types);

            Logger.ZLogTrace("Reading types for assembly {0} done", name);
        }

        protected virtual void AddNestedTypes(IEnumerable<TypeDefinition> types)
        {
            bool skip_key;

            foreach (TypeDefinition type in types)
            {
                if (type.FullName.Contains("<"))
                {
                    continue;
                }
                if (type.FullName.Equals("Microsoft.CodeAnalysis.EmbeddedAttribute"))
                {
                    continue;
                }
                if (type.FullName.Equals("System.Runtime.CompilerServices.NullableAttribute"))
                {
                    continue;
                }

                Validator.CheckType(type, this);

                string key = H5Types.GetTypeDefinitionKey(type);

                H5Type duplicateH5Type = null;

                skip_key = false;

                if (H5Types.TryGetValue(key, out duplicateH5Type))
                {
                    var duplicate = duplicateH5Type.TypeDefinition;

                    var message = string.Format(
                        Constants.Messages.Exceptions.DUPLICATE_H5_TYPE,
                        type.Module.Assembly.FullName,
                        type.FullName,
                        duplicate.Module.Assembly.FullName,
                        duplicate.FullName);

                    if (!AssemblyInfo.IgnoreDuplicateTypes)
                    {
                        Logger.LogError(message);
                        throw new InvalidOperationException(message);
                    }
                    else
                    {
                        Logger.LogWarning(message);
                    }
                    skip_key = true;
                }

                if (!skip_key)
                {
                    TypeDefinitions.Add(key, type);

                    H5Types.Add(key, new H5Type(key)
                    {
                        TypeDefinition = type
                    });

                    if (type.HasNestedTypes)
                    {
                        InheritAttributes(type);
                        AddNestedTypes(type.NestedTypes);
                    }
                }
            }
        }

        /// <summary>
        /// Makes any existing nested types (classes?) inherit the FileName attribute of the specified type.
        /// Does not override a nested type's FileName attribute if present.
        /// </summary>
        /// <param name="type"></param>
        protected static void InheritAttributes(TypeDefinition type)
        {
            // List of attribute names that are meant to be inherited by sub-classes.
            var attrList = new List<string>
            {
                "FileNameAttribute",
                "ModuleAttribute",
                "NamespaceAttribute"
            };

            foreach (var attribute in attrList)
            {
                if (type.CustomAttributes.Any(ca => ca.AttributeType.Name == attribute))
                {
                    var FAt = type.CustomAttributes.First(ca => ca.AttributeType.Name == attribute);

                    foreach (var nestedType in type.NestedTypes)
                    {
                        if (!nestedType.CustomAttributes.Any(ca => ca.AttributeType.Name == attribute))
                        {
                            nestedType.CustomAttributes.Add(FAt);
                        }
                    }
                }
            }
        }

        protected virtual List<AssemblyDefinition> InspectReferences(Dictionary<string, string> discoveredAssemblyPaths)
        {
            using (new Measure(Logger, "Inspecting references"))
            {
                TypeInfoDefinitions = new Dictionary<string, ITypeInfo>();

                var references = new List<AssemblyDefinition>();
                var assembly   = LoadAssembly(AssemblyLocation, references, discoveredAssemblyPaths);
                LoadReferenceAssemblies(references);
                TypeDefinitions    = new Dictionary<string, TypeDefinition>();
                H5Types            = new H5Types();
                AssemblyDefinition = assembly;

                if (assembly.Name.Name != H5_ASSEMBLY || AssemblyInfo.Assembly != null && AssemblyInfo.Assembly.EnableReservedNamespaces)
                {
                    ReadTypes(assembly);
                }

                foreach (var item in references)
                {
                    ReadTypes(item);
                }

                var prefix = Path.GetDirectoryName(Location);

                for (int i = 0; i < SourceFiles.Count; i++)
                {
                    SourceFiles[i] = Path.Combine(prefix, SourceFiles[i]);
                }


                return references;
            }
        }

        protected virtual void InspectTypes(MemberResolver resolver, IH5DotJson_AssemblySettings config)
        {
            using (new Measure(Logger, "Inspecting types"))
            {
                var inspector = CreateInspector(config);
                inspector.AssemblyInfo = config;
                inspector.Resolver     = resolver;

                for (int i = 0; i < ParsedSourceFiles.Length; i++)
                {
                    var sourceFile = ParsedSourceFiles[i];
                    Logger.ZLogTrace("Visiting syntax tree {0}", (sourceFile != null && sourceFile.ParsedFile != null && sourceFile.ParsedFile.FileName != null ? sourceFile.ParsedFile.FileName : ""));

                    inspector.VisitSyntaxTree(sourceFile.SyntaxTree);
                }

                AssemblyInfo = inspector.AssemblyInfo;
                Types        = inspector.Types;
            }
        }

        protected virtual Inspector CreateInspector(IH5DotJson_AssemblySettings config = null)
        {
            return new Inspector(config);
        }

        private string[] Rewrite()
        {
            using (var m = new Measure(Logger, $"Rewritting code for {SourceFiles.Count} files"))
            {
                var rewriter = new SharpSixRewriter(this);
                var result   = new string[SourceFiles.Count];

                var queue      = new ConcurrentQueue<int>(Enumerable.Range(0, SourceFiles.Count));
                var exceptions = new ConcurrentStack<Exception>();

                var threads = Enumerable.Range(0, Environment.ProcessorCount).Select(i =>
                {
                    var t = new Thread(() =>
                    {
                        while (queue.TryDequeue(out var index))
                        {
                            Logger.LogTrace("Rewriting {0} using thread {1}", SourceFiles[index], Thread.CurrentThread.ManagedThreadId);

                            try
                            {
                                result[index] = rewriter.Clone().Rewrite(index);
                            }
                            catch (Exception ex)
                            {
                                exceptions.Push(ex);
                                return;
                            }
                        }
                    });
                    t.IsBackground = true;
                    t.Start();
                    return t;
                }).ToArray();

                Array.ForEach(threads, t => t.Join());

                if (exceptions.Any())
                {
                    throw new AggregateException("Compilation failed", exceptions);
                }

                rewriter.CommitCache();

                m.SetOperations(result.Length);

                return result;
            }
        }

        protected void BuildSyntaxTree(CancellationToken cancellationToken)
        {
            var rewriten = Rewrite();

            using (var m = new Measure(Logger, "Building syntax tree"))
            {
                var queue = new ConcurrentQueue<int>(Enumerable.Range(0, rewriten.Length));

                var exceptions = new ConcurrentStack<Exception>();

                var threads = Enumerable.Range(0, Environment.ProcessorCount).Select(i =>
                {
                    var t = new Thread(() =>
                    {
                        while (queue.TryDequeue(out var index) && !cancellationToken.IsCancellationRequested)
                        {
                            try
                            {
                                BuildSyntaxTreeForFile(index, ref rewriten);
                            }
                            catch (Exception ex)
                            {
                                exceptions.Push(ex);
                                return;
                            }
                        }
                    });
                    t.IsBackground = true;
                    t.Start();
                    return t;
                }).ToArray();

                Array.ForEach(threads, t => t.Join());

                cancellationToken.ThrowIfCancellationRequested();

                if (exceptions.Any())
                {
                    throw new AggregateException("Compilation failed", exceptions);
                }

                m.SetOperations(rewriten.Length);
            }
        }

        private void BuildSyntaxTreeForFile(int index, ref string[] rewriten)
        {
            var fileName = SourceFiles[index];

            var swAll = ValueStopwatch.StartNew();
            var sw    = ValueStopwatch.StartNew();

            Logger.ZLogTrace("Building syntax tree for file '{0}' ...", (fileName ?? ""));

            var parser = new CSharpParser();

            if (DefineConstants != null && DefineConstants.Count > 0)
            {
                foreach (var defineConstant in DefineConstants)
                {
                    parser.CompilerSettings.ConditionalSymbols.Add(defineConstant);
                }
            }

            //RFO: Temp fix for invalid handling of A?.Method() being converted to A != null ? A.Method() : ()null
            if (rewriten[index].Contains("()null"))
            {
                rewriten[index] = rewriten[index].Replace("()null", "null");
            }

            var syntaxTree = parser.Parse(rewriten[index], fileName);
            syntaxTree.FileName = fileName;

            Logger.ZLogTrace("Building syntax tree for file '{0}' finished in {1:n1} ms", (fileName ?? ""), sw.GetElapsedTime().TotalMilliseconds);

            sw = ValueStopwatch.StartNew(); //Reset the stopwatch

            if (parser.HasErrors)
            {
                var errors = new List<string>();

                foreach (var error in parser.Errors)
                {
                    errors.Add(fileName + ":" + error.Region.BeginLine + "," + error.Region.BeginColumn + ": " + error.Message);
                }

                throw new EmitterException(syntaxTree, "Error parsing code." + Environment.NewLine + String.Join(Environment.NewLine, errors));
            }

            var expandResult = new QueryExpressionExpander().ExpandQueryExpressions(syntaxTree);

            Logger.ZLogTrace("Expanding query expressions for file '{0}' finished in {1:n1} ms", (fileName ?? ""), sw.GetElapsedTime().TotalMilliseconds);

            sw = ValueStopwatch.StartNew(); //Reset the stopwatch

            syntaxTree = (expandResult != null ? (SyntaxTree)expandResult.AstNode : syntaxTree);

            var emptyLambdaDetecter = new EmptyLambdaDetecter();
            syntaxTree.AcceptVisitor(emptyLambdaDetecter);

            Logger.ZLogTrace("Empty lambda detector on file '{0}' finished in {1:n1} ms", (fileName ?? ""), sw.GetElapsedTime().TotalMilliseconds);
            sw = ValueStopwatch.StartNew(); //Reset the stopwatch


            if (emptyLambdaDetecter.Found)
            {
                var fixer   = new EmptyLambdaFixer();
                var astNode = syntaxTree.AcceptVisitor(fixer);
                Logger.ZLogTrace("Empty lambda fixer on file '{0}' finished in {1:n1} ms", (fileName ?? ""), sw.GetElapsedTime().TotalMilliseconds);
                sw                  = ValueStopwatch.StartNew(); //Reset the stopwatch
                syntaxTree          = (astNode != null ? (SyntaxTree)astNode : syntaxTree);
                syntaxTree.FileName = fileName;
            }

            var f = new ParsedSourceFile(syntaxTree, new CSharpUnresolvedFile { FileName = fileName });

            ParsedSourceFiles[index] = f;

            var tcv = new TypeSystemConvertVisitor(f.ParsedFile);

            f.SyntaxTree.AcceptVisitor(tcv);

            Logger.ZLogTrace("Type system converter on file '{0}' finished in {1:n1} ms", (fileName ?? ""), sw.GetElapsedTime().TotalMilliseconds);

            Logger.ZLogTrace("Processing syntax tree for file '{0}' finished in {1:n1} ms", (fileName ?? ""), swAll.GetElapsedTime().TotalMilliseconds);
        }
    }
}