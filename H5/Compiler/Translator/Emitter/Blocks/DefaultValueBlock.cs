using H5.Contract;
using H5.Contract.Constants;

using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;

namespace H5.Translator
{
    public class DefaultValueBlock : AbstractEmitterBlock
    {
        public DefaultValueBlock(IEmitter emitter, DefaultValueExpression defaultValueExpression)
            : base(emitter, defaultValueExpression)
        {
            this.Emitter = emitter;
            this.DefaultValueExpression = defaultValueExpression;
        }

        public DefaultValueExpression DefaultValueExpression { get; set; }

        protected override void DoEmit()
        {
            var resolveResult = this.Emitter.Resolver.ResolveNode(this.DefaultValueExpression.Type, this.Emitter);
            var value = DefaultValueBlock.DefaultValue(resolveResult, this.Emitter, DefaultValueExpression.Type);
            this.Write(value);
        }

        public static string DefaultValue(ResolveResult resolveResult, IEmitter emitter, AstType astType = null)
        {
            if ((!resolveResult.IsError && resolveResult.Type.IsReferenceType.HasValue && resolveResult.Type.IsReferenceType.Value) || resolveResult.Type.Kind == TypeKind.Dynamic || resolveResult.Type.IsKnownType(KnownTypeCode.NullableOfT))
            {
                return "null";
            }

            if (resolveResult.Type.Kind == TypeKind.Enum)
            {
                var enumMode = Helpers.EnumEmitMode(resolveResult.Type);
                var isString = enumMode >= 3 && enumMode <= 6;
                return isString ? "null" : "0";
            }

            return JS.Funcs.H5_GETDEFAULTVALUE + "(" + (astType != null ? H5Types.ToJsName(astType, emitter) : H5Types.ToJsName(resolveResult.Type, emitter)) + ")";
        }
    }
}