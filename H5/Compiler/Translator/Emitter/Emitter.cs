using H5.Contract;
using ICSharpCode.NRefactory.TypeSystem;
using Microsoft.Extensions.Logging;
using Mono.Cecil;
using Mosaik.Core;
using System.Collections.Generic;

namespace H5.Translator
{
    public partial class Emitter : Visitor, IEmitter
    {
        private static ILogger Logger = ApplicationLogging.CreateLogger<Emitter>();

        public Emitter(IDictionary<string,TypeDefinition> typeDefinitions, H5Types h5Types, List<ITypeInfo> types, IValidator validator, IMemberResolver resolver, Dictionary<string, ITypeInfo> typeInfoDefinitions)
        {
            Resolver = resolver;
            TypeDefinitions = typeDefinitions;
            TypeInfoDefinitions = typeInfoDefinitions;
            Types = types;
            H5Types = h5Types;

            H5Types.InitItems(this);

            Logger.LogTrace("Sorting types infos by name...");
            Types.Sort(CompareTypeInfosByName);
            Logger.LogTrace("Sorting types infos by name done");

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

        public virtual List<TranslatorOutputItem> Emit()
        {
            Logger.LogInformation("Emitting...");

            var blocks = GetBlocks();
            foreach (var block in blocks)
            {
                JsDoc.Init();

                Logger.LogTrace("Emitting block " + block.GetType());

                block.Emit();
            }

            if (AutoStartupMethods.Count > 1)
            {
                var autoMethods = string.Join(", ", AutoStartupMethods);

                throw (TranslatorException)H5.Translator.TranslatorException.Create("Program has more than one entry point defined - {0}", autoMethods);
            }

            var output = TransformOutputs();

            Logger.LogInformation("Emitting done");

            return output;
        }

        private IEnumerable<IAbstractEmitterBlock> GetBlocks()
        {
            yield return new H5.Translator.EmitBlock(this);

            if (AssemblyInfo.GenerateTypeScript)
            {
                yield return new H5.Translator.TypeScript.EmitBlock(this);
            }
        }
    }
}