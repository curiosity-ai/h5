using H5.Contract;
using ICSharpCode.NRefactory.CSharp;

namespace H5.Translator
{
    public class NullReferenceBlock : ConversionBlock
    {
        public NullReferenceBlock(IEmitter emitter, AstNode nullNode)
            : base(emitter, nullNode)
        {
            Emitter = emitter;
            NullNode = nullNode;
        }

        public AstNode NullNode { get; set; }

        protected override Expression GetExpression()
        {
            var expr = NullNode as Expression;
            return expr;
        }

        protected override void EmitConversionExpression()
        {
            VisitNull();
        }

        protected void VisitNull()
        {
            Write("null");
        }
    }
}