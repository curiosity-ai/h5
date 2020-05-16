using H5.Contract;
using ICSharpCode.NRefactory.CSharp;

namespace H5.Translator
{
    public class TypeOfExpressionBlock : ConversionBlock
    {
        public TypeOfExpressionBlock(IEmitter emitter, TypeOfExpression typeOfExpression)
            : base(emitter, typeOfExpression)
        {
            Emitter = emitter;
            TypeOfExpression = typeOfExpression;
        }

        public TypeOfExpression TypeOfExpression { get; set; }

        protected override Expression GetExpression()
        {
            return TypeOfExpression;
        }

        protected override void EmitConversionExpression()
        {
            TypeOfExpression.Type.AcceptVisitor(Emitter);
        }
    }
}