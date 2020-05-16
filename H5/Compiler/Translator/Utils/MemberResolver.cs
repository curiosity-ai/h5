using System.Collections.Concurrent;
using H5.Contract;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.CSharp.TypeSystem;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.TypeSystem.Implementation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mono.Cecil;
using Mosaik.Core;
using Microsoft.Extensions.Logging;

namespace H5.Translator
{
    public class MemberResolver : IMemberResolver
    {
        private static ILogger Logger = ApplicationLogging.CreateLogger<MemberResolver>();

        private string lastFileName;
        private IList<ParsedSourceFile> sourceFiles;
        private ICompilation compilation;
        private CSharpAstResolver resolver;
        private IProjectContent project;
        private readonly ConcurrentDictionary<SyntaxTree, CSharpUnresolvedFile> typeSystemCache;

        public bool CanFreeze { get; set; }

        public CSharpAstResolver Resolver
        {
            get
            {
                return resolver;
            }
        }

        public ICompilation Compilation
        {
            get
            {
                return compilation;
            }
        }

        public IEnumerable<IAssemblyReference> Assemblies { get; private set; }

        public MemberResolver(IList<ParsedSourceFile> sourceFiles, IEnumerable<IAssemblyReference> assemblies, AssemblyDefinition assemblyDefinition)
        {
            project = null;
            lastFileName = null;
            this.sourceFiles = sourceFiles;
            Assemblies = assemblies;
            MainAssembly = assemblyDefinition;
            typeSystemCache = new ConcurrentDictionary<SyntaxTree, CSharpUnresolvedFile>();

            project = new CSharpProjectContent();
            project = project.AddAssemblyReferences(assemblies);
            project = project.SetAssemblyName(assemblyDefinition.FullName);
            AddOrUpdateFiles();
        }

        public AssemblyDefinition MainAssembly
        {
            get; set;
        }

        private void AddOrUpdateFiles()
        {
            typeSystemCache.Clear();
            var unresolvedFiles = new IUnresolvedFile[sourceFiles.Count];

            Parallel.For(0, unresolvedFiles.Length, i =>
            {
                var syntaxTree = sourceFiles[i].SyntaxTree;
                unresolvedFiles[i] = GetTypeSystem(syntaxTree);
            });

            project = project.AddOrUpdateFiles(unresolvedFiles);
            compilation = project.CreateCompilation();
        }

        private void InitResolver(SyntaxTree syntaxTree)
        {
            if (lastFileName != syntaxTree.FileName || string.IsNullOrEmpty(syntaxTree.FileName))
            {
                lastFileName = syntaxTree.FileName;
                var typeSystem = GetTypeSystem(syntaxTree);
                resolver = new CSharpAstResolver(compilation, syntaxTree, typeSystem);
            }
        }

        private CSharpUnresolvedFile GetTypeSystem(SyntaxTree syntaxTree)
        {
            CSharpUnresolvedFile existingTypeSystem;
            if (typeSystemCache.TryGetValue(syntaxTree, out existingTypeSystem))
            {
                return existingTypeSystem;
            }
            CSharpUnresolvedFile unresolvedFile = null;
            if (!string.IsNullOrEmpty(syntaxTree.FileName))
            {
                unresolvedFile = syntaxTree.ToTypeSystem();
            }
            typeSystemCache[syntaxTree] = unresolvedFile;
            return unresolvedFile;
        }

        public ResolveResult ResolveNode(AstNode node)
        {
            var syntaxTree = node.GetParent<SyntaxTree>();
            InitResolver(syntaxTree);

            var result = resolver.Resolve(node);

            if (result is MethodGroupResolveResult resolveResult && node.Parent != null)
            {
                var methodGroupResolveResult = resolveResult;
                var parentResolveResult = ResolveNode(node.Parent);
                var parentInvocation = parentResolveResult as InvocationResolveResult;
                IParameterizedMember method = methodGroupResolveResult.Methods.LastOrDefault();
                bool isInvocation = node.Parent is InvocationExpression invocationExp && (invocationExp.Target == node);

                if (node is Expression expression)
                {
                    var conversion = Resolver.GetConversion(expression);
                    if (conversion != null && conversion.IsMethodGroupConversion)
                    {
                        return new MemberResolveResult(new TypeResolveResult(conversion.Method.DeclaringType), conversion.Method);
                    }
                }

                if (isInvocation && parentInvocation != null)
                {
                    var or = methodGroupResolveResult.PerformOverloadResolution(compilation, parentInvocation.GetArgumentsForCall().ToArray());
                    if (or.FoundApplicableCandidate)
                    {
                        method = or.BestCandidate;
                        return new MemberResolveResult(new TypeResolveResult(method.DeclaringType), method);
                    }
                }

                if (parentInvocation != null && method == null)
                {
                    if (methodGroupResolveResult.TargetType is DefaultResolvedTypeDefinition typeDef)
                    {
                        var methods = typeDef.Methods.Where(m => m.Name == methodGroupResolveResult.MethodName);
                        method = methods.FirstOrDefault();
                    }
                }

                if (method == null)
                {
                    var extMethods = methodGroupResolveResult.GetEligibleExtensionMethods(false);

                    if (!extMethods.Any())
                    {
                        extMethods = methodGroupResolveResult.GetExtensionMethods();
                    }

                    if (!extMethods.Any() || !extMethods.First().Any())
                    {
                        throw new EmitterException(node, "Cannot find method defintion");
                    }

                    method = extMethods.First().First();
                }

                if (parentInvocation == null || method.FullName != parentInvocation.Member.FullName)
                {
                    MemberResolveResult memberResolveResult = new MemberResolveResult(new TypeResolveResult(method.DeclaringType), method);
                    return memberResolveResult;
                }

                return parentResolveResult;
            }

            if ((result == null || result.IsError))
            {
                if (result is CSharpInvocationResolveResult invocationResult && invocationResult.OverloadResolutionErrors != OverloadResolutionErrors.None)
                {
                    return result;
                }

                if (result.IsError)
                {
                    Logger.LogWarning("Node resolving has failed {0}: {1}", node.StartLocation, node.ToString());
                }
            }

            return result;
        }
    }
}