using H5.Contract;
using ICSharpCode.NRefactory.CSharp;

namespace H5.Translator
{
    public class ThisReferenceBlock : ConversionBlock
    {
        public ThisReferenceBlock(IEmitter emitter, ThisReferenceExpression thisReferenceExpression)
            : base(emitter, thisReferenceExpression)
        {
            Emitter = emitter;
            ThisReferenceExpression = thisReferenceExpression;
        }

        public ThisReferenceExpression ThisReferenceExpression { get; set; }

        protected override Expression GetExpression()
        {
            return ThisReferenceExpression;
        }

        protected override void EmitConversionExpression()
        {
            WriteThis();
        }
    }
}