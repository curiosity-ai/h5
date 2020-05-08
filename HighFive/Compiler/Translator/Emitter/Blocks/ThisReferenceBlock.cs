using HighFive.Contract;
using ICSharpCode.NRefactory.CSharp;

namespace HighFive.Translator
{
    public class ThisReferenceBlock : ConversionBlock
    {
        public ThisReferenceBlock(IEmitter emitter, ThisReferenceExpression thisReferenceExpression)
            : base(emitter, thisReferenceExpression)
        {
            this.Emitter = emitter;
            this.ThisReferenceExpression = thisReferenceExpression;
        }

        public ThisReferenceExpression ThisReferenceExpression
        {
            get;
            set;
        }

        protected override Expression GetExpression()
        {
            return this.ThisReferenceExpression;
        }

        protected override void EmitConversionExpression()
        {
            this.WriteThis();
        }
    }
}