using H5.Contract;
using ICSharpCode.NRefactory.TypeSystem;
using Microsoft.Extensions.Logging;
using Mono.Cecil;
using Mosaik.Core;
using System.Collections.Generic;
using System.Threading;
using ZLogger;

namespace H5.Translator
{
    public partial class Emitter : Visitor, IEmitter
    {
        private static ILogger Logger = ApplicationLogging.CreateLogger<Emitter>();

        public Emitter(IDictionary<string,TypeDefinition> typeDefinitions, H5Types h5Types, List<ITypeInfo> types, IValidator validator, IMemberResolver resolver, Dictionary<string, ITypeInfo> typeInfoDefinitions, CancellationToken cancellationToken)
        {
            Resolver = resolver;
            TypeDefinitions = typeDefinitions;
            TypeInfoDefinitions = typeInfoDefinitions;
            Types = types;
            H5Types = h5Types;
            CancellationToken = cancellationToken;

            H5Types.InitItems(this);

            using (new Measure(Logger, "Sorting types by name", logOnlyDuration: true))
            {
                Types.Sort(CompareTypeInfosByName);
            }
            
            SortTypesByInheritance();

            Validator = validator;
            AssignmentType = ICSharpCode.NRefactory.CSharp.AssignmentOperatorType.Any;
            UnaryOperatorType = ICSharpCode.NRefactory.CSharp.UnaryOperatorType.Any;
            JsDoc = new JsDoc();
            AnonymousTypes = new Dictionary<AnonymousType, IAnonymousTypeConfig>();
            AutoStartupMethods = new List<string>();
            Cache = new EmitterCache();
            AssemblyNameRuleCache = new Dictionary<IAssembly, NameRule[]>();
            ClassNameRuleCache = new Dictionary<ITypeDefinition, NameRule[]>();
            AssemblyCompilerRuleCache = new Dictionary<IAssembly, CompilerRule[]>();
            ClassCompilerRuleCache = new Dictionary<ITypeDefinition, CompilerRule[]>();
        }

        public List<TranslatorOutputItem> Emit()
        {
            using (new Measure(Logger, "Emitting JavaScript code"))
            {
                var blocks = GetBlocks();
                foreach (var block in blocks)
                {
                    CancellationToken.ThrowIfCancellationRequested();

                    JsDoc.Init();

                    Logger.ZLogTrace("Emitting block {0}", block.GetType());

                    block.Emit();
                }

                if (AutoStartupMethods.Count > 1)
                {
                    var autoMethods = string.Join(", ", AutoStartupMethods);

                    Logger.LogError("Program has more than one entry point defined - {0}", autoMethods);

                    throw (TranslatorException)TranslatorException.Create("Program has more than one entry point defined - {0}", autoMethods);
                }

                return TransformOutputs();
            }

        }

        private IEnumerable<IAbstractEmitterBlock> GetBlocks()
        {
            yield return new EmitBlock(this);

            if (AssemblyInfo.GenerateTypeScript)
            {
                yield return new TypeScript.EmitBlock(this);
            }
        }
    }
}