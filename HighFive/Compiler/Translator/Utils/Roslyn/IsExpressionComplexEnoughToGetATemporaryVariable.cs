using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HighFive.Translator
{
    public static class IsExpressionComplexEnoughToGetATemporaryVariable
    {
        private class Analyzer : CSharpSyntaxWalker
        {
            private SemanticModel _semanticModel;

            public bool IsComplex
            {
                get;
                private set;
            }

            public Analyzer(SemanticModel semanticModel)
            {
                _semanticModel = semanticModel;
            }

            public void Analyze(SyntaxNode node)
            {
                Visit(node);
            }

            public override void VisitArrayCreationExpression(ArrayCreationExpressionSyntax node)
            {
                IsComplex = true;
                base.VisitArrayCreationExpression(node);
            }

            public override void VisitImplicitArrayCreationExpression(ImplicitArrayCreationExpressionSyntax node)
            {
                IsComplex = true;
                base.VisitImplicitArrayCreationExpression(node);
            }

            public override void VisitBinaryExpression(BinaryExpressionSyntax node)
            {
                IsComplex = true;
                base.VisitBinaryExpression(node);
            }

            public override void VisitInvocationExpression(InvocationExpressionSyntax node)
            {
                IsComplex = true;
                base.VisitInvocationExpression(node);
            }

            public override void VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
            {
                IsComplex = true;
                base.VisitObjectCreationExpression(node);
            }

            public override void VisitAnonymousObjectCreationExpression(AnonymousObjectCreationExpressionSyntax node)
            {
                IsComplex = true;
                base.VisitAnonymousObjectCreationExpression(node);
            }

            public override void VisitPostfixUnaryExpression(PostfixUnaryExpressionSyntax node)
            {
                IsComplex = true;
                base.VisitPostfixUnaryExpression(node);
            }

            public override void VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax node)
            {
                IsComplex = true;
                base.VisitPrefixUnaryExpression(node);
            }

            public override void VisitConditionalExpression(ConditionalExpressionSyntax node)
            {
                IsComplex = true;
                base.VisitConditionalExpression(node);
            }

            public override void VisitConditionalAccessExpression(ConditionalAccessExpressionSyntax node)
            {
                IsComplex = true;
                base.VisitConditionalAccessExpression(node);
            }

            public override void VisitAnonymousMethodExpression(AnonymousMethodExpressionSyntax node)
            {
                IsComplex = true;
                base.VisitAnonymousMethodExpression(node);
            }

            public override void VisitIdentifierName(IdentifierNameSyntax node)
            {
                var symbol = _semanticModel.GetSymbolInfo(node).Symbol;

                if (symbol is IPropertySymbol)
                {
                    IsComplex = true;
                }

                base.VisitIdentifierName(node);
            }

            public override void VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
            {
                var symbol = _semanticModel.GetSymbolInfo(node).Symbol;

                if (symbol is IPropertySymbol)
                {
                    IsComplex = true;
                }

                base.VisitMemberAccessExpression(node);
            }

            public override void VisitElementAccessExpression(ElementAccessExpressionSyntax node)
            {
                IsComplex = true;
                base.VisitElementAccessExpression(node);
            }

            public override void VisitCastExpression(CastExpressionSyntax node)
            {
                IsComplex = true;
                base.VisitCastExpression(node);
            }

            public override void VisitAssignmentExpression(AssignmentExpressionSyntax node)
            {
                IsComplex = true;
                base.VisitAssignmentExpression(node);
            }
        }

        public static bool IsComplex(SemanticModel semanticModel, SyntaxNode node)
        {
            var analyzer = new Analyzer(semanticModel);
            analyzer.Analyze(node);
            return analyzer.IsComplex;
        }
    }
}