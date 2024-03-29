using ICSharpCode.NRefactory.CSharp;

namespace H5.Translator
{
    public class GotoSearchVisitor : DepthFirstAstVisitor
    {
        public bool Found { get; set; }

        public override void VisitLambdaExpression(LambdaExpression lambdaExpression)
        {
        }

        public override void VisitAnonymousMethodExpression(AnonymousMethodExpression anonymousMethodExpression)
        {
        }

        public override void VisitGotoStatement(GotoStatement gotoStatement)
        {
            Found = true;
        }

        public override void VisitGotoCaseStatement(GotoCaseStatement gotoCaseStatement)
        {
            Found = true;
        }

        public override void VisitGotoDefaultStatement(GotoDefaultStatement gotoDefaultStatement)
        {
            Found = true;
        }
    }
}