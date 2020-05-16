using H5.Contract;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.Semantics;

namespace H5.Translator
{
    public class PrimitiveExpressionBlock : ConversionBlock
    {
        public PrimitiveExpressionBlock(IEmitter emitter, PrimitiveExpression primitiveExpression)
            : base(emitter, primitiveExpression)
        {
            Emitter = emitter;
            PrimitiveExpression = primitiveExpression;
        }

        public PrimitiveExpression PrimitiveExpression { get; set; }

        protected override Expression GetExpression()
        {
            return PrimitiveExpression;
        }

        protected override void EmitConversionExpression()
        {
            if (PrimitiveExpression.IsNull)
            {
                return;
            }

            var isTplRaw = Emitter.TemplateModifier == "raw";
            if (PrimitiveExpression.Value is RawValue || isTplRaw)
            {
                Write(AbstractEmitterBlock.UpdateIndentsInString(PrimitiveExpression.Value.ToString(), 0));
            }
            else
            {
                object value = PrimitiveExpression.Value;

                WriteScript(H5.Translator.Emitter.ConvertConstant(value, PrimitiveExpression, Emitter));
            }
        }
    }
}