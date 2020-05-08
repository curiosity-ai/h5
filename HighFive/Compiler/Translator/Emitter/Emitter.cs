using H5.Contract;
using ICSharpCode.NRefactory.TypeSystem;
using Mono.Cecil;
using System.Collections.Generic;

namespace H5.Translator
{
    public partial class Emitter : Visitor, IEmitter
    {
        public Emitter(IDictionary<string,
            TypeDefinition> typeDefinitions,
            H5Types h5Types,
            List<ITypeInfo> types,
            IValidator validator,
            IMemberResolver resolver,
            Dictionary<string, ITypeInfo> typeInfoDefinitions,
            ILogger logger)
        {
            this.Log = logger;

            this.Resolver = resolver;
            this.TypeDefinitions = typeDefinitions;
            this.TypeInfoDefinitions = typeInfoDefinitions;
            this.Types = types;
            this.H5Types = h5Types;

            this.H5Types.InitItems(this);

            logger.Trace("Sorting types infos by name...");
            this.Types.Sort(this.CompareTypeInfosByName);
            logger.Trace("Sorting types infos by name done");

            this.SortTypesByInheritance();

            this.Validator = validator;
            this.AssignmentType = ICSharpCode.NRefactory.CSharp.AssignmentOperatorType.Any;
            this.UnaryOperatorType = ICSharpCode.NRefactory.CSharp.UnaryOperatorType.Any;
            this.JsDoc = new JsDoc();
            this.AnonymousTypes = new Dictionary<AnonymousType, IAnonymousTypeConfig>();
            this.AutoStartupMethods = new List<string>();
            this.Cache = new EmitterCache();
            this.AssemblyNameRuleCache = new Dictionary<IAssembly, NameRule[]>();
            this.ClassNameRuleCache = new Dictionary<ITypeDefinition, NameRule[]>();
            this.AssemblyCompilerRuleCache = new Dictionary<IAssembly, CompilerRule[]>();
            this.ClassCompilerRuleCache = new Dictionary<ITypeDefinition, CompilerRule[]>();
        }

        public virtual List<TranslatorOutputItem> Emit()
        {
            this.Log.Info("Emitting...");

            var blocks = this.GetBlocks();
            foreach (var block in blocks)
            {
                this.JsDoc.Init();

                this.Log.Trace("Emitting block " + block.GetType());

                block.Emit();
            }

            if (this.AutoStartupMethods.Count > 1)
            {
                var autoMethods = string.Join(", ", this.AutoStartupMethods);

                throw (TranslatorException)H5.Translator.TranslatorException.Create("Program has more than one entry point defined - {0}", autoMethods);
            }

            var output = this.TransformOutputs();

            this.Log.Info("Emitting done");

            return output;
        }

        private IEnumerable<IAbstractEmitterBlock> GetBlocks()
        {
            yield return new H5.Translator.EmitBlock(this);

            if (this.AssemblyInfo.GenerateTypeScript)
            {
                yield return new H5.Translator.TypeScript.EmitBlock(this);
            }
        }
    }
}