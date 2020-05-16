using H5.Contract;
using ICSharpCode.NRefactory.CSharp;
using Object.Net.Utilities;
using System.Linq;
using ICSharpCode.NRefactory.Semantics;

namespace H5.Translator.TypeScript
{
    public class EnumBlock : TypeScriptBlock
    {
        public EnumBlock(IEmitter emitter, ITypeInfo typeInfo, string ns)
            : base(emitter, typeInfo.TypeDeclaration)
        {
            TypeInfo = typeInfo;
            Namespace = ns;
        }

        public ITypeInfo TypeInfo { get; set; }

        public string Namespace { get; set; }

        protected override void DoEmit()
        {
            var typeDef = Emitter.GetTypeDefinition();
            string name = Emitter.Validator.GetCustomTypeName(typeDef, Emitter, true, false);

            if (name.IsEmpty())
            {
                name = H5Types.ToTypeScriptName(TypeInfo.Type, Emitter, false, true);
            }

            Write("enum ");
            Write(name);

            WriteSpace();
            BeginBlock();

            if (TypeInfo.StaticConfig.Fields.Count > 0)
            {
                var lastField = TypeInfo.StaticConfig.Fields.Last();
                foreach (var field in TypeInfo.StaticConfig.Fields)
                {

                    Write(GetEnumItemName(Emitter, field));

                    var initializer = field.Initializer;
                    if (initializer != null && initializer is PrimitiveExpression)
                    {
                        Write(" = ");
                        if (Helpers.IsStringNameEnum(TypeInfo.Type))
                        {
                            WriteScript(((PrimitiveExpression)initializer).Value);
                        }
                        else
                        {
                            Write(((PrimitiveExpression)initializer).Value);
                        }

                    }

                    if (field != lastField)
                    {
                        Write(",");
                    }

                    WriteNewLine();
                }
            }

            EndBlock();
        }

        public static string GetEnumItemName(IEmitter emitter, TypeConfigItem field)
        {
            var memeber_rr = (MemberResolveResult)emitter.Resolver.ResolveNode(field.Entity);
            var mname = emitter.GetEntityName(memeber_rr.Member);
            return mname;
        }
    }
}