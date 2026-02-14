using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace H5.Translator
{
    public class NameofReplacer : CSharpSyntaxRewriter
    {
        private readonly SemanticModel semanticModel;

        public NameofReplacer(SemanticModel semanticModel)
        {
            this.semanticModel = semanticModel;
        }

        public override SyntaxNode VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            // Check if it's "nameof"
            if (node.Expression is IdentifierNameSyntax syntax && syntax.Identifier.Text == "nameof")
            {
                // Check if it's the `nameof` keyword or a method
                var symbolInfo = semanticModel.GetSymbolInfo(node.Expression);
                if (symbolInfo.Symbol == null && node.ArgumentList.Arguments.Count == 1)
                {
                    // It is likely the nameof operator.
                    // Try to get constant value first (e.g. from Roslyn if supported)
                    var constantValue = semanticModel.GetConstantValue(node);
                    if (constantValue.HasValue && constantValue.Value is string s)
                    {
                        return SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(s))
                            .WithLeadingTrivia(node.GetLeadingTrivia())
                            .WithTrailingTrivia(node.GetTrailingTrivia());
                    }

                    // Fallback: extract identifier name directly from the argument expression.
                    // nameof(x) -> "x"
                    // nameof(A.B) -> "B"
                    var argExpr = node.ArgumentList.Arguments[0].Expression;
                    string name = GetNameFromExpression(argExpr);

                    if (name != null)
                    {
                        return SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(name))
                            .WithLeadingTrivia(node.GetLeadingTrivia())
                            .WithTrailingTrivia(node.GetTrailingTrivia());
                    }
                }
            }

            return base.VisitInvocationExpression(node);
        }

        private string GetNameFromExpression(ExpressionSyntax expr)
        {
            if (expr is IdentifierNameSyntax id) return id.Identifier.ValueText;
            if (expr is MemberAccessExpressionSyntax ma) return ma.Name.Identifier.ValueText;
            if (expr is AliasQualifiedNameSyntax aq) return aq.Name.Identifier.ValueText;
            if (expr is QualifiedNameSyntax qn) return qn.Right.Identifier.ValueText;
            if (expr is GenericNameSyntax gn) return gn.Identifier.ValueText;

            // Handle other cases if needed? e.g. nameof(List<int>) -> List
            // GenericNameSyntax handles it.

            return null;
        }
    }
}
