using H5.Contract;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.TypeSystem;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mono.Cecil;
using Mosaik.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLogger;
using System.Collections.Concurrent;
using System.Threading;

namespace H5.Translator
{
    public partial class Emitter : Visitor
    {
        public string Tag { get; set; }

        public EmitterCache Cache { get; private set; }

        public IValidator Validator { get; private set; }

        public List<ITypeInfo> Types { get; set; }

        public bool IsAssignment { get; set; }

        public AssignmentOperatorType AssignmentType { get; set; }

        public UnaryOperatorType UnaryOperatorType { get; set; }

        public bool IsUnaryAccessor { get; set; }

        public Dictionary<string, AstType> Locals { get; set; }

        public Dictionary<IVariable, string> LocalsMap { get; set; }

        public Dictionary<string, string> LocalsNamesMap { get; set; }

        public Stack<Dictionary<string, AstType>> LocalsStack { get; set; }

        public int Level { get; set; }

        public int initialLevel;
        public int InitialLevel
        {
            get
            {
                return initialLevel;
            }
            set
            {
                initialLevel = value;
                ResetLevel();
            }
        }

        public int ResetLevel(int? level = null)
        {
            if (!level.HasValue)
            {
                level = InitialLevel;
            }

            if (level < InitialLevel && !InitPosition.HasValue )
            {
                level = InitialLevel;
            }

            if (level < 0)
            {
                level = 0;
            }

            Level = level.Value;

            return Level;
        }

        public InitPosition? InitPosition { get; set; }

        public bool IsNewLine { get; set; }

        public bool EnableSemicolon { get; set; }

        public int IteratorCount { get; set; }

        public int ThisRefCounter { get; set; }

        public IDictionary<string, TypeDefinition> TypeDefinitions
        {
            get;
            protected set;
        }

        public ITypeInfo TypeInfo { get; set; }

        public StringBuilder Output { get; set; }

        public Stack<IWriter> Writers { get; set; }

        public bool Comma { get; set; }

        private HashSet<string> namespaces;

        protected HashSet<string> Namespaces
        {
            get
            {
                if (namespaces == null)
                {
                    namespaces = CreateNamespaces();
                }
                return namespaces;
            }
        }

        public IReadOnlyList<AssemblyDefinition> References { get; set; }

        public IList<string> SourceFiles { get; set; }

        private List<IAssemblyReference> list;

        protected IReadOnlyList<IAssemblyReference> AssemblyReferences
        {
            get
            {
                if (list != null)
                {
                    return list;
                }

                list = ToAssemblyReferences(References);

                return list;
            }
        }

        internal static List<IAssemblyReference> ToAssemblyReferences(IReadOnlyList<AssemblyDefinition> references)
        {
            var stack = new ConcurrentStack<IAssemblyReference>();

            if (references is object)
            {
                using (new Measure(Logger, "Loading assembly definitions", references.Count))
                {
                    Task.WaitAll(references.Select(reference => Task.Run(() =>
                    {
                        Logger.ZLogTrace("\tLoading AssemblyDefinition {0} ...", (reference != null && reference.Name != null && reference.Name.Name != null ? reference.Name.Name : ""));
                        var loader = new CecilLoader() { IncludeInternalMembers = true };
                        stack.Push(loader.LoadAssembly(reference));
                        Logger.ZLogTrace("\tLoading AssemblyDefinition done");
                    })).ToArray());
                }
            }

            return stack.ToList();
        }

        public IMemberResolver Resolver { get; set; }

        public IH5DotJson_AssemblySettings AssemblyInfo { get; set; }

        public Dictionary<string, ITypeInfo> TypeInfoDefinitions { get; set; }

        public List<IModuleDependency> CurrentDependencies { get; set; }

        public IEmitterOutputs Outputs { get; set; }

        public IEmitterOutput EmitterOutput { get; set; }

        public bool SkipSemiColon { get; set; }

        public IEnumerable<MethodDefinition> MethodsGroup { get; set; }

        public Dictionary<int, StringBuilder> MethodsGroupBuilder { get; set; }

        public bool IsAsync { get; set; }

        public bool IsYield { get; set; }

        public List<string> AsyncVariables { get; set; }

        public IAsyncBlock AsyncBlock { get; set; }

        public bool ReplaceAwaiterByVar { get; set; }

        public bool AsyncExpressionHandling { get; set; }

        public AstNode IgnoreBlock { get; set; }

        public AstNode NoBraceBlock { get; set; }

        public Action BeforeBlock { get; set; }

        public IWriterInfo LastSavedWriter { get; set; }

        public List<IJumpInfo> JumpStatements { get; set; }

        public SwitchStatement AsyncSwitch { get; set; }

        public Dictionary<string, bool> TempVariables { get; set; }

        public Dictionary<string, string> NamedTempVariables { get; set; }

        public Dictionary<string, bool> ParentTempVariables { get; set; }

        public H5Types H5Types { get; set; }
        
        public CancellationToken CancellationToken { get; }

        public ITranslator Translator { get; set; }

        public IJsDoc JsDoc { get; set; }

        public IType ReturnType { get; set; }

        public bool ReplaceJump { get; set; }

        public string CatchBlockVariable { get; set; }

        public bool StaticBlock { get; set; }

        public Dictionary<string, string> NamedFunctions { get; set; }

        public Dictionary<IType, Dictionary<string, string>> NamedBoxedFunctions { get; set; }

        public bool IsJavaScriptOverflowMode
        {
            get
            {
                return AssemblyInfo.OverflowMode.HasValue && AssemblyInfo.OverflowMode == OverflowMode.Javascript;
            }
        }

        public bool IsRefArg { get; set; }

        public Dictionary<AnonymousType, IAnonymousTypeConfig> AnonymousTypes { get; set; }

        public List<string> AutoStartupMethods { get; set; }

        public bool IsAnonymousReflectable { get; set; }

        public string MetaDataOutputName { get; set; }

        public IType[] ReflectableTypes { get; set; }

        public Dictionary<string, int> NamespacesCache { get; set; }

        private bool AssemblyJsDocWritten { get; set; }

        public bool ForbidLifting { get; set; }

        public bool DisableDependencyTracking { get; set; }

        public Dictionary<IAssembly, NameRule[]> AssemblyNameRuleCache { get; }

        public Dictionary<ITypeDefinition, NameRule[]> ClassNameRuleCache { get; }

        public Dictionary<IAssembly, CompilerRule[]> AssemblyCompilerRuleCache { get; }

        public Dictionary<ITypeDefinition, CompilerRule[]> ClassCompilerRuleCache { get; }

        public string SourceFileName { get; set; }

        public int SourceFileNameIndex { get; set; }

        public string LastSequencePoint { get; set; }

        public bool InConstructor { get; set; }

        public CompilerRule Rules { get; set; }

        public bool HasModules { get; set; }

        public string TemplateModifier { get; set; }

        public int WrapRestCounter { get; set; }
    }
}