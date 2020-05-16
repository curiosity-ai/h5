using H5.Contract;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.Semantics;

namespace H5.Translator
{
    public class ParenthesizedBlock : ConversionBlock
    {
        public ParenthesizedBlock(IEmitter emitter, ParenthesizedExpression parenthesizedExpression)
            : base(emitter, parenthesizedExpression)
        {
            Emitter = emitter;
            ParenthesizedExpression = parenthesizedExpression;
        }

        public ParenthesizedExpression ParenthesizedExpression { get; set; }

        protected override Expression GetExpression()
        {
            return ParenthesizedExpression;
        }

        protected override void EmitConversionExpression()
        {
            var ignoreParentheses = IgnoreParentheses(ParenthesizedExpression.Expression);

            if (!ignoreParentheses)
            {
                WriteOpenParentheses();
            }

            int startPos = Emitter.Output.Length;

            ParenthesizedExpression.Expression.AcceptVisitor(Emitter);

            if (!ignoreParentheses)
            {
                WriteCloseParentheses();
            }
        }

        protected bool IgnoreParentheses(Expression expression)
        {
            if (ParenthesizedExpression.Parent is CastExpression)
            {
                var conversion = Emitter.Resolver.Resolver.GetConversion(ParenthesizedExpression);
                bool isOperator = ParenthesizedExpression.Parent.Parent is BinaryOperatorExpression ||
                                  ParenthesizedExpression.Parent.Parent is UnaryOperatorExpression;
                if (!isOperator && (conversion.IsNumericConversion || conversion.IsEnumerationConversion || conversion.IsIdentityConversion))
                {
                    return true;
                }
            }

            if (expression is CastExpression castExpr)
            {
                if (Emitter.Resolver.ResolveNode(castExpr.Expression) is OperatorResolveResult orr)
                {
                    return false;
                }

                var rr = Emitter.Resolver.ResolveNode(expression);
                if (rr is ConstantResolveResult)
                {
                    return false;
                }

                if (rr.Type.Kind == ICSharpCode.NRefactory.TypeSystem.TypeKind.Unknown || rr.Type.Kind == ICSharpCode.NRefactory.TypeSystem.TypeKind.Delegate)
                {
                    return false;
                }

                return true;
            }
            return false;
        }
    }
}