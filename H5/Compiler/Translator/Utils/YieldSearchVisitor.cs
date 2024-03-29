using ICSharpCode.NRefactory.CSharp;

namespace H5.Translator
{
    public class YieldSearchVisitor : DepthFirstAstVisitor
    {
        public YieldSearchVisitor()
        {
        }

        public bool Found { get; set; }

        public override void VisitLambdaExpression(LambdaExpression lambdaExpression)
        {
        }

        public override void VisitAnonymousMethodExpression(AnonymousMethodExpression anonymousMethodExpression)
        {
        }

        public override void VisitYieldBreakStatement(YieldBreakStatement yieldBreakStatement)
        {
            Found = true;
        }

        public override void VisitYieldReturnStatement(YieldReturnStatement yieldReturnStatement)
        {
            Found = true;
        }
    }
}