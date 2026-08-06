using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using Mono.Cecil;
using Object.Net.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.Utils;
using TopologicalSorting;
using AssemblyDefinition = Mono.Cecil.AssemblyDefinition;
using Microsoft.Extensions.Logging;
using ZLogger;
using Mosaik.Core;
using System.Threading;
using NUglify.JavaScript;

namespace H5.Translator
{
    public partial class Translator : ITranslator
    {
        public const string H5_ASSEMBLY                        = CS.NS.H5;
        public const string H5_ASSEMBLY_DOT                    = H5_ASSEMBLY + ".";
        public const string H5ResourcesPlusSeparatedFormatList = "H5.Resources.list";
        public const string H5ResourcesJsonFormatList          = "H5.Resources.json";
        public const string H5ResourcesCombinedPrefix          = "H5.Resources.Parts.";
        public const string LocalesPrefix                      = "H5.Translator.Resources.Locales.";
        public const string DefaultLocalesOutputName           = "H5.Locales.js";
        public const string H5ConsoleName                      = "h5.console.js";
        public const string SupportedProjectType               = "Library";
        public const string DefaultRootNamespace               = "ClassLibrary";
        public const string SystemAssemblyName                 = "mscorlib";

        public static readonly  Encoding OutputEncoding                        = new UTF8Encoding(false);
        private static readonly string[] MinifierCodeSettingsInternalFileNames = new string[] { "h5.js", "h5.min.js", "h5.collections.js", "h5.collections.min.js" };

        private char[] invalidPathChars;
        public char[] InvalidPathChars
        {
            get
            {
                if (invalidPathChars == null)
                {
                    var l = new List<char>(Path.GetInvalidPathChars());
                    l.AddRange(new char[] { '<', '>', ':', '"', '|', '?', '*' });
                    invalidPathChars = l.Distinct().ToArray();
                }

                return invalidPathChars;
            }
        }

        public FileHelper FileHelper
        {
            get;
            private set;
        }

        public Dictionary<string, int> Stats { get; set; } = new Dictionary<string, int>();

        private static readonly CodeSettings MinifierCodeSettingsSafe = new CodeSettings
        {
            EvalTreatment        = EvalTreatment.MakeAllSafe,
            LocalRenaming        = LocalRenaming.KeepAll,
            TermSemicolons       = true,
            StrictMode           = false,
            RemoveUnneededCode   = false,
            AlwaysEscapeNonAscii = true
        };

        private static readonly CodeSettings MinifierCodeSettingsSafeCrunchLocal = new CodeSettings
        {
            EvalTreatment        = EvalTreatment.MakeAllSafe,
            LocalRenaming        = LocalRenaming.CrunchAll,
            TermSemicolons       = true,
            StrictMode           = false,
            RemoveUnneededCode   = false,
            AlwaysEscapeNonAscii = true
        };

        private static readonly CodeSettings MinifierCodeSettingsInternal = new CodeSettings
        {
            TermSemicolons       = true,
            StrictMode           = false,
            RemoveUnneededCode   = false,
            AlwaysEscapeNonAscii = true
        };

        private static readonly CodeSettings MinifierCodeSettingsLocales = new CodeSettings
        {
            TermSemicolons       = true,
            RemoveUnneededCode   = false,
            AlwaysEscapeNonAscii = true
        };

        public Translator(string location)
        {
            Location          = location;
            Validator         = CreateValidator();
            DefineConstants   = new List<string>() { "H5" };
            ProjectProperties = new ProjectProperties();
            FileHelper        = new FileHelper();
            Outputs           = new TranslatorOutput();
        }

        public void PostBuildStreamCacheCleanup()
        {
            bool ShouldCacheAssembly(string path)
            {
                return path.Contains(".nuget");
            }

            lock (_loadedAssembliesLock)
            {
                foreach (var path in _loadedAssemblies.Keys.ToArray())
                {
                    if (!ShouldCacheAssembly(path))
                    {
                        if (_loadedAssemblies.Remove(path, out var oldAssembly))
                        {
                            oldAssembly.assembly.Dispose();
                        }
                    }
                }

                foreach (var path in _loadedAssemblieStreams.Keys.ToArray())
                {
                    if (!ShouldCacheAssembly(path))
                    {
                        if (_loadedAssemblieStreams.Remove(path, out var oldStream))
                        {
                            oldStream.Close();
                            oldStream.Dispose();
                        }
                    }
                }
            }
        }

        public void Translate(CancellationToken cancellationToken)
        {
            using (new Measure(Logger, "Translating assembly"))
            {
                var config = AssemblyInfo;

                Dictionary<string, string> discoveredAssemblyPaths = null;

                if (Rebuild)
                {
                    Logger.ZLogInformation("Rebuilding assembly on path {0}", AssemblyLocation);
                    discoveredAssemblyPaths = BuildAssembly(cancellationToken);
                }
                else if (!File.Exists(AssemblyLocation))
                {
                    Logger.ZLogInformation("Building assembly on path {0}", AssemblyLocation);
                    discoveredAssemblyPaths = BuildAssembly(cancellationToken);
                }

                var references = InspectReferences(discoveredAssemblyPaths);
                References = references;

                LogProductInfo();

                if (!string.IsNullOrWhiteSpace(config.BeforeBuild))
                {
                    try
                    {
                        using (new Measure(Logger, $"Running BeforeBuild event {config.BeforeBuild}"))
                        {
                            RunEvent(config.BeforeBuild);
                        }
                    }
                    catch (Exception exc)
                    {
                        Logger.ZLogError(exc, "Error running BeforeBuild event {0}", config.BeforeBuild);
                        throw new TranslatorException($"Error: Unable to run beforeBuild event command: {exc.Message}\nStack trace:\n{exc.StackTrace}");
                    }
                }

                BuildSyntaxTree(cancellationToken);

                var resolver = new MemberResolver(ParsedSourceFiles, Emitter.ToAssemblyReferences(references), AssemblyDefinition);
                resolver = Preconvert(resolver, config, cancellationToken);

                InspectTypes(resolver, config);

                resolver.CanFreeze = true;
                var emitter = CreateEmitter(resolver, cancellationToken);

                if (!AssemblyInfo.OverflowMode.HasValue)
                {
                    AssemblyInfo.OverflowMode = OverflowMode;
                }

                emitter.Translator   = this;
                emitter.AssemblyInfo = AssemblyInfo;
                emitter.References   = references;
                emitter.SourceFiles  = SourceFiles;
                emitter.InitialLevel = 1;

                if (AssemblyInfo.Module != null)
                {
                    AssemblyInfo.Module.Emitter = emitter;
                }

                foreach (var td in TypeInfoDefinitions)
                {
                    if (td.Value.Module != null)
                    {
                        td.Value.Module.Emitter = emitter;
                    }
                }

                SortReferences();

                AddMainOutputs(emitter.Emit());
                EmitterOutputs = emitter.Outputs;
            }
        }

        protected virtual MemberResolver Preconvert(MemberResolver resolver, IH5DotJson_AssemblySettings config, CancellationToken cancellationToken)
        {
            bool needRecompile = false;

            foreach (var sourceFile in ParsedSourceFiles)
            {
                cancellationToken.ThrowIfCancellationRequested();

                Logger.ZLogTrace("Preconvert {0}", sourceFile.ParsedFile.FileName);
                var syntaxTree  = sourceFile.SyntaxTree;
                var tempEmitter = new TempEmitter { AssemblyInfo = config };
                var detecter    = new PreconverterDetecter(resolver, tempEmitter);
                syntaxTree.AcceptVisitor(detecter);

                if (detecter.Found)
                {
                    var fixer   = new PreconverterFixer(resolver, tempEmitter);
                    var astNode = syntaxTree.AcceptVisitor(fixer);
                    syntaxTree            = astNode != null ? (SyntaxTree)astNode : syntaxTree;
                    sourceFile.SyntaxTree = syntaxTree;
                    needRecompile         = true;
                }
            }

            if (needRecompile)
            {
                return new MemberResolver(ParsedSourceFiles, resolver.Assemblies, AssemblyDefinition);
            }

            return resolver;
        }

        protected virtual void SortReferences()
        {
            var graph = new DependencyGraph();

            foreach (var t in References)
            {
                var tProcess = graph.Processes.FirstOrDefault(p => p.Name == t.Name.Name);

                if (tProcess == null)
                {
                    tProcess = new OrderedProcess(graph, t.Name.Name);
                }

                foreach (var xref in t.MainModule.AssemblyReferences)
                {
                    var dProcess = graph.Processes.FirstOrDefault(p => p.Name == xref.Name);

                    if (dProcess == null)
                    {
                        dProcess = new OrderedProcess(graph, xref.Name);
                    }

                    tProcess.After(dProcess);
                }
            }

            if (graph.ProcessCount > 0)
            {
                AssemblyDefinition asmDef = null;

                try
                {
                    var list = new List<AssemblyDefinition>(References.Count());

                    Logger.ZLogTrace("Sorting references...");

                    Logger.ZLogTrace("\t\tCalculate sorting references...");
                    //IEnumerable<IEnumerable<OrderedProcess>> sorted = graph.CalculateSort();
                    TopologicalSort sorted = graph.CalculateSort();
                    Logger.ZLogTrace("\t\tCalculate sorting references done");

                    // The fix required for Mono 5.0.0.94
                    // It does not "understand" TopologicalSort's Enumerator in foreach
                    // foreach (var processes in sorted)
                    // The code is modified to get it "directly" and "typed"
                    var sortedISetEnumerable = sorted as IEnumerable<ISet<OrderedProcess>>;
                    Logger.ZLogTrace("\t\tGot Enumerable<ISet<OrderedProcess>>");

                    var sortedISetEnumerator = sortedISetEnumerable.GetEnumerator();
                    Logger.ZLogTrace("\t\tGot Enumerator<ISet<OrderedProcess>>");

                    //foreach (var processes in sorted)
                    while (sortedISetEnumerator.MoveNext())
                    {
                        var processes = sortedISetEnumerator.Current;

                        foreach (var process in processes)
                        {
                            Logger.ZLogTrace("\tHandling {0}", process.Name);

                            asmDef = References.FirstOrDefault(r => r.Name.Name == process.Name);

                            if (asmDef != null && list.All(r => r.Name.Name != asmDef.Name.Name))
                            {
                                list.Add(asmDef);
                            }
                        }
                    }

                    References = list;

                    Logger.ZLogTrace("Sorting references done:");

                    for (int i = 0; i < list.Count; i++)
                    {
                        Logger.ZLogTrace("\t {0}", list[i].Name);
                    }
                }
                catch (Exception ex)
                {
                    Logger.ZLogWarning("Topological sort failed {0} with error {1}", asmDef != null ? $"at reference {asmDef.FullName}" : string.Empty, ex);
                }
            }
        }

        private static void NewLine(StringBuilder sb, string line = null)
        {
            if (line != null)
            {
                sb.Append(line);
            }

            sb.Append(Emitter.NEW_LINE);
        }

        private static void NewLine(MemoryStream sb, string line = null)
        {
            if (line != null)
            {
                var b = OutputEncoding.GetBytes(line);
                sb.Write(b, 0, b.Length);
            }

            var nl = OutputEncoding.GetBytes(Emitter.NEW_LINE);
            sb.Write(nl, 0, nl.Length);
        }

        public bool CheckIfRequiresSourceMap(TranslatorOutputItem output)
        {
            return !output.IsEmpty
             && output.OutputType == TranslatorOutputType.JavaScript
             && output.OutputKind.HasFlag(TranslatorOutputKind.ProjectOutput)
             && !output.OutputKind.HasFlag(TranslatorOutputKind.Locale)
             && !output.OutputKind.HasFlag(TranslatorOutputKind.PluginOutput)
             && !output.OutputKind.HasFlag(TranslatorOutputKind.Reference)
             && !output.OutputKind.HasFlag(TranslatorOutputKind.Resource)
             && !output.OutputKind.HasFlag(TranslatorOutputKind.Metadata);
        }

        public bool CheckIfRequiresSourceMap(H5ResourceInfoPart resourcePart)
        {
            var fileHelper = new FileHelper();

            return resourcePart != null
             && resourcePart.Assembly == null // i.e. this assembly output
             && fileHelper.IsJS(resourcePart.Name);
        }

        public TranslatorOutputItem FindTranslatorOutputItem(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return null;
            }

            foreach (var output in Outputs.GetOutputs())
            {
                if (output.FullPath.LocalPath == filePath)
                {
                    return output;
                }
            }

            return null;
        }

        public string GenerateSourceMap(string fileName, string content, Action<SourceMapBuilder> before = null)
        {
            if (AssemblyInfo.SourceMap.Enabled)
            {
                var projectPath = Path.GetDirectoryName(Location);
                var sourceRoot  = projectPath;

                SourceMapGenerator.Generate(fileName, sourceRoot, projectPath, ref content,
                    before,
                    (sourceRelativePath) =>
                    {
                        string           path       = null;
                        ParsedSourceFile sourceFile = null;

                        try
                        {
                            path       = Path.Combine(projectPath, sourceRelativePath);
                            sourceFile = ParsedSourceFiles.First(pf => pf.ParsedFile.FileName == path);

                            return sourceFile.SyntaxTree.TextSource ?? sourceFile.SyntaxTree.ToString(GetFormatter());
                        }
                        catch (Exception ex)
                        {
                            throw (TranslatorException)TranslatorException.Create(
                                "Could not get ParsedSourceFile for SourceMap. Exception: {0}; projectPath: {1}; sourceRelativePath: {2}; path: {3}.",
                                ex.ToString(), projectPath, sourceRelativePath, path);
                        }

                    },
                    new string[0], SourceFiles, AssemblyInfo.SourceMap.Eol
                );
            }

            return content;
        }

        private static CSharpFormattingOptions GetFormatter()
        {
            var formatter = FormattingOptionsFactory.CreateSharpDevelop();
            formatter.AnonymousMethodBraceStyle               = BraceStyle.NextLine;
            formatter.MethodBraceStyle                        = BraceStyle.NextLine;
            formatter.StatementBraceStyle                     = BraceStyle.NextLine;
            formatter.PropertyBraceStyle                      = BraceStyle.NextLine;
            formatter.ConstructorBraceStyle                   = BraceStyle.NextLine;
            formatter.NewLineAfterConstructorInitializerColon = NewLinePlacement.NewLine;
            formatter.NewLineAferMethodCallOpenParentheses    = NewLinePlacement.NewLine;
            formatter.ClassBraceStyle                         = BraceStyle.NextLine;
            formatter.ArrayInitializerBraceStyle              = BraceStyle.NextLine;
            formatter.IndentPreprocessorDirectives            = false;

            return formatter;
        }

        public void RunAfterBuild()
        {
            using (new Measure(Logger, "Running after build"))
            {
                if (!string.IsNullOrWhiteSpace(AssemblyInfo.AfterBuild))
                {
                    try
                    {
                        Logger.ZLogTrace("Run AfterBuild event");
                        RunEvent(AssemblyInfo.AfterBuild);
                    }
                    catch (Exception ex)
                    {
                        var message = $"Error: Unable to run afterBuild event command: {ex}";

                        Logger.ZLogError(message);
                        throw new TranslatorException(message);
                    }
                }
                else
                {
                    Logger.ZLogTrace("No AfterBuild event specified");
                }
            }
        }

        protected virtual Emitter CreateEmitter(IMemberResolver resolver, CancellationToken cancellationToken)
        {
            using (new Measure(Logger, "Creating Emitter"))
            {
                return new Emitter(TypeDefinitions, H5Types, Types, Validator, resolver, TypeInfoDefinitions, cancellationToken);
            }
        }

        protected virtual Validator CreateValidator()
        {
            return new Validator();
        }

        public EmitterException CreateExceptionFromLastNode()
        {
            return EmitNode != null ? new EmitterException(EmitNode) : null;
        }
    }
}