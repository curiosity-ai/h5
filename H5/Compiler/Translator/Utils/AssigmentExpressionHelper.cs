using ICSharpCode.NRefactory.CSharp;

namespace H5.Translator.Utils
{
    public class AssigmentExpressionHelper
    {
        public static bool CheckIsRightAssigmentExpression(AssignmentExpression expression)
        {
            return CheckIsExpression(expression.Right);
        }

        public static bool CheckIsExpression(Expression expression)
        {
            return (expression is ParenthesizedExpression ||
                    expression is IdentifierExpression ||
                    expression is MemberReferenceExpression ||
                    expression is PrimitiveExpression ||
                    expression is IndexerExpression ||
                    expression is LambdaExpression ||
                    expression is AnonymousMethodExpression ||
                    expression is ObjectCreateExpression);
        }
    }
}