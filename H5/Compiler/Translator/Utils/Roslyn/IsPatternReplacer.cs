using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;

namespace H5.Translator
{
    public class IsPatternReplacer : ICSharpReplacer
    {
        Dictionary<string, bool> typesInfo = new Dictionary<string, bool>();

        public SyntaxNode Replace(SyntaxNode root, SemanticModel model, SharpSixRewriter rewriter)
        {
            root = InsertVariables(root, model);
            return ReplacePatterns(root, model);
        }

        public SyntaxNode InsertVariables(SyntaxNode root, SemanticModel model)
        {
            var patterns = root
                .DescendantNodes()
                .OfType<IsPatternExpressionSyntax>();

            var updatedStatements = new Dictionary<SyntaxNode, List<LocalDeclarationStatementSyntax>>();

            foreach (var pattern in patterns)
            {
                try
                {
                    SyntaxNode lambdaExpr = pattern.Ancestors().OfType<LambdaExpressionSyntax>().FirstOrDefault();
                    SyntaxNode beforeStatement = pattern.Ancestors().OfType<StatementSyntax>().FirstOrDefault();

                    if (pattern.Pattern is DeclarationPatternSyntax declarationPattern)
                    {
                        if (lambdaExpr != null && !SyntaxHelper.IsChildOf(beforeStatement, lambdaExpr))
                        {
                            if (lambdaExpr is ParenthesizedLambdaExpressionSyntax pl && !(pl.Body is BlockSyntax))
                            {
                                beforeStatement = lambdaExpr;
                            }
                            else if (lambdaExpr is SimpleLambdaExpressionSyntax sl && !(sl.Body is BlockSyntax))
                            {
                                beforeStatement = lambdaExpr;
                            }
                        }

                        if (beforeStatement != null)
                        {
                            if (declarationPattern.Designation is SingleVariableDesignationSyntax designation)
                            {
                                if (!typesInfo.Keys.Contains(declarationPattern.Type.ToString()))
                                {
                                    var ti = model.GetTypeInfo(declarationPattern.Type);
                                    var isValueType = ti.Type != null ? ti.Type.IsValueType : false;
                                    typesInfo[declarationPattern.Type.ToString()] = isValueType;
                                }

                                var locals = updatedStatements.ContainsKey(beforeStatement) ? updatedStatements[beforeStatement] : new List<LocalDeclarationStatementSyntax>();

                                var varDecl = SyntaxFactory.VariableDeclaration(declarationPattern.Type).WithVariables(SyntaxFactory.SingletonSeparatedList(
                                    SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier(designation.Identifier.ValueText))
                                ));

                                locals.Add(SyntaxFactory.LocalDeclarationStatement(varDecl).WithTrailingTrivia(SyntaxFactory.Whitespace("\n")).NormalizeWhitespace());

                                updatedStatements[beforeStatement] = locals;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new ReplacerException(pattern, e);
                }
            }

            var annotated = new Dictionary<SyntaxAnnotation, Tuple<bool, List<LocalDeclarationStatementSyntax>>>();

            root = root.ReplaceNodes(updatedStatements.Keys, (n1, n2) =>
            {
                var annotation = new SyntaxAnnotation();
                var locals = updatedStatements[n1];
                annotated[annotation] = new Tuple<bool, List<LocalDeclarationStatementSyntax>>(SyntaxHelper.RequireReturnStatement(model, n1), locals);

                n2 = n2.WithAdditionalAnnotations(annotation);
                return n2;
            });

            foreach (var annotation in annotated.Keys)
            {
                var annotatedNode = root.GetAnnotatedNodes(annotation).First();
                var tuple = annotated[annotation];
                var requireReturn = tuple.Item1;
                var varStatements = tuple.Item2;

                if (annotatedNode is LambdaExpressionSyntax)
                {
                    ExpressionSyntax bodyExpr = null;
                    if (annotatedNode.IsKind(SyntaxKind.ParenthesizedLambdaExpression))
                    {
                        bodyExpr = ((ParenthesizedLambdaExpressionSyntax)annotatedNode).Body as ExpressionSyntax;
                    }
                    else
                    {
                        bodyExpr = ((SimpleLambdaExpressionSyntax)annotatedNode).Body as ExpressionSyntax;
                    }
                    if (bodyExpr == null)
                    {
                        continue;
                    }

                    var list = new List<StatementSyntax>(varStatements);
                    list.Add(requireReturn ? (StatementSyntax)SyntaxFactory.ReturnStatement(bodyExpr) : SyntaxFactory.ExpressionStatement(bodyExpr));

                    LambdaExpressionSyntax lambdaExpression = null;
                    if (annotatedNode is ParenthesizedLambdaExpressionSyntax pl && !(pl.Body is BlockSyntax))
                    {
                        lambdaExpression = pl.WithBody(SyntaxFactory.Block(list)).NormalizeWhitespace();
                    }
                    else if (annotatedNode is SimpleLambdaExpressionSyntax sl && !(sl.Body is BlockSyntax))
                    {
                        lambdaExpression = sl.WithBody(SyntaxFactory.Block(list)).NormalizeWhitespace();
                    }

                    if (lambdaExpression != null)
                    {
                        root = root.ReplaceNode(annotatedNode, lambdaExpression);
                        continue;
                    }
                }

                if(annotatedNode.Parent is BlockSyntax || !(annotatedNode is StatementSyntax syntax))
                {
                    root = root.InsertNodesBefore(annotatedNode, varStatements);
                }
                else
                {
                    var list = new List<StatementSyntax>(varStatements);
                    list.Add(syntax);
                    root = root.ReplaceNode(annotatedNode, SyntaxFactory.Block(list).NormalizeWhitespace());
                }
            }

            return root;
        }

        public SyntaxNode ReplacePatterns(SyntaxNode root, SemanticModel model)
        {
            var patterns = root.DescendantNodes().OfType<IsPatternExpressionSyntax>();
            var updatedPatterns = new Dictionary<IsPatternExpressionSyntax, ExpressionSyntax>();

            foreach (var pattern in patterns)
            {
                try
                {
                    var block = pattern.Ancestors().OfType<BlockSyntax>().FirstOrDefault();

                    if (block != null)
                    {
                        // Handle Declaration Pattern with Assignment
                        if (pattern.Pattern is DeclarationPatternSyntax declarationPattern &&
                            declarationPattern.Designation is SingleVariableDesignationSyntax designation)
                        {
                            var beforeStatement = pattern.Ancestors().OfType<StatementSyntax>().FirstOrDefault(ss => ss.Parent == block);

                            var key = declarationPattern.Type.ToString();
                            BinaryExpressionSyntax newExpr;
                            if (typesInfo.Keys.Contains(key) && typesInfo[key])
                            {
                                newExpr = SyntaxFactory.BinaryExpression(SyntaxKind.NotEqualsExpression, SyntaxFactory.ParenthesizedExpression(SyntaxFactory.AssignmentExpression(
                                    SyntaxKind.SimpleAssignmentExpression,
                                    SyntaxFactory.IdentifierName(designation.Identifier.ValueText),
                                    SyntaxFactory.ConditionalExpression(SyntaxFactory.BinaryExpression(SyntaxKind.IsExpression, pattern.Expression, declarationPattern.Type),
                                        SyntaxFactory.CastExpression(declarationPattern.Type, pattern.Expression),
                                        SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName("H5.Script"),
                                                SyntaxFactory.GenericName(
                                                    SyntaxFactory.Identifier("Write"))
                                                .WithTypeArgumentList(
                                                    SyntaxFactory.TypeArgumentList(
                                                        SyntaxFactory.SingletonSeparatedList(declarationPattern.Type)))))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.LiteralExpression(
                                                            SyntaxKind.StringLiteralExpression,
                                                            SyntaxFactory.Literal("null"))))))
                                    )
                                )), SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression));
                            }
                            else
                            {
                                newExpr = SyntaxFactory.BinaryExpression(SyntaxKind.NotEqualsExpression, SyntaxFactory.ParenthesizedExpression(SyntaxFactory.AssignmentExpression(
                                    SyntaxKind.SimpleAssignmentExpression,
                                    SyntaxFactory.IdentifierName(designation.Identifier.ValueText),
                                    SyntaxFactory.BinaryExpression(SyntaxKind.AsExpression, pattern.Expression, declarationPattern.Type)
                                )), SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression));
                            }

                            updatedPatterns[pattern] = newExpr.NormalizeWhitespace();
                        }
                        // Handle Constant Pattern (Top-level optimization/legacy handling)
                        else if (pattern.Pattern is ConstantPatternSyntax cps)
                        {
                             // Use MakeCheck logic directly or keep existing special handling if strictly needed.
                             // Existing logic for top-level constant pattern seems identical to what MakeCheck should do,
                             // except MakeCheck is cleaner. Let's reuse MakeCheck logic here too for consistency,
                             // unless there is a reason not to.
                             // Actually, the existing logic handles null specifically. MakeCheck also does.
                             // So we can fallback to MakeCheck for everything else.

                             updatedPatterns[pattern] = MakeCheck(pattern.Expression, pattern.Pattern).NormalizeWhitespace();
                        }
                        // Handle Recursive Pattern
                        else if (pattern.Pattern is RecursivePatternSyntax recursivePattern)
                        {
                            updatedPatterns[pattern] = MakeCheck(pattern.Expression, recursivePattern).NormalizeWhitespace();
                        }
                        else
                        {
                             // Fallback for other patterns (Relational, Binary, Unary, Type, Parenthesized)
                             // which are handled by MakeCheck but were missing here.
                             updatedPatterns[pattern] = MakeCheck(pattern.Expression, pattern.Pattern).NormalizeWhitespace();
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new ReplacerException(pattern, e);
                }
            }

            if (updatedPatterns.Count > 0)
            {
                root = root.ReplaceNodes(updatedPatterns.Keys, (b1, b2) => updatedPatterns[b1]);
            }

            return root;
        }

        private ExpressionSyntax MakeCheck(ExpressionSyntax expression, PatternSyntax pattern)
        {
            if (pattern is ConstantPatternSyntax constPattern)
            {
                if (constPattern.Expression.IsKind(SyntaxKind.NullLiteralExpression))
                {
                    return SyntaxFactory.BinaryExpression(SyntaxKind.EqualsExpression, expression, constPattern.Expression);
                }

                return SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, expression, SyntaxFactory.IdentifierName("Equals")),
                    SyntaxFactory.ArgumentList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Argument(constPattern.Expression)))
                );
            }
            else if (pattern is RecursivePatternSyntax recursivePattern)
            {
                var condition = (ExpressionSyntax)SyntaxFactory.BinaryExpression(SyntaxKind.NotEqualsExpression, expression, SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression));

                if (recursivePattern.Type != null)
                {
                    var typeCheck = SyntaxFactory.BinaryExpression(SyntaxKind.IsExpression, expression, recursivePattern.Type);
                    condition = SyntaxFactory.BinaryExpression(SyntaxKind.LogicalAndExpression, condition, typeCheck);
                }

                if (recursivePattern.PropertyPatternClause != null)
                {
                    foreach (var sub in recursivePattern.PropertyPatternClause.Subpatterns)
                    {
                        var propAccess = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, expression, sub.NameColon.Name);
                        var subCheck = MakeCheck(propAccess, sub.Pattern);
                        condition = SyntaxFactory.BinaryExpression(SyntaxKind.LogicalAndExpression, condition, subCheck);
                    }
                }

                return condition;
            }
            else if (pattern is DeclarationPatternSyntax declPattern)
            {
                // Partial support: Type check only (variables ignored for now, handled by InsertVariables if top-level)
                return SyntaxFactory.BinaryExpression(SyntaxKind.IsExpression, expression, declPattern.Type);
            }
            else if (pattern is UnaryPatternSyntax unaryPattern)
            {
                // pattern is not pattern
                if (unaryPattern.OperatorToken.IsKind(SyntaxKind.NotKeyword))
                {
                    // Special case for 'not null' -> '!= null'
                    if (unaryPattern.Pattern is ConstantPatternSyntax innerConst && innerConst.Expression.IsKind(SyntaxKind.NullLiteralExpression))
                    {
                         return SyntaxFactory.BinaryExpression(SyntaxKind.NotEqualsExpression, expression, innerConst.Expression);
                    }

                    return SyntaxFactory.PrefixUnaryExpression(SyntaxKind.LogicalNotExpression, SyntaxFactory.ParenthesizedExpression(MakeCheck(expression, unaryPattern.Pattern)));
                }
            }
            else if (pattern is BinaryPatternSyntax binaryPattern)
            {
                // pattern is left and right
                // pattern is left or right
                var leftCheck = MakeCheck(expression, binaryPattern.Left);
                var rightCheck = MakeCheck(expression, binaryPattern.Right);

                if (binaryPattern.OperatorToken.IsKind(SyntaxKind.AndKeyword))
                {
                    return SyntaxFactory.BinaryExpression(SyntaxKind.LogicalAndExpression, leftCheck, rightCheck);
                }
                else if (binaryPattern.OperatorToken.IsKind(SyntaxKind.OrKeyword))
                {
                    return SyntaxFactory.BinaryExpression(SyntaxKind.LogicalOrExpression, leftCheck, rightCheck);
                }
            }
            else if (pattern is ParenthesizedPatternSyntax parenPattern)
            {
                return MakeCheck(expression, parenPattern.Pattern);
            }
            else if (pattern is RelationalPatternSyntax relPattern)
            {
                // pattern is < 10
                SyntaxKind opKind = SyntaxKind.None;
                switch (relPattern.OperatorToken.Kind())
                {
                    case SyntaxKind.LessThanToken: opKind = SyntaxKind.LessThanExpression; break;
                    case SyntaxKind.LessThanEqualsToken: opKind = SyntaxKind.LessThanOrEqualExpression; break;
                    case SyntaxKind.GreaterThanToken: opKind = SyntaxKind.GreaterThanExpression; break;
                    case SyntaxKind.GreaterThanEqualsToken: opKind = SyntaxKind.GreaterThanOrEqualExpression; break;
                }

                if (opKind != SyntaxKind.None)
                {
                    return SyntaxFactory.BinaryExpression(opKind, expression, relPattern.Expression);
                }
            }
            else if (pattern is TypePatternSyntax typePattern)
            {
                return SyntaxFactory.BinaryExpression(SyntaxKind.IsExpression, expression, typePattern.Type);
            }
            else if (pattern is DiscardPatternSyntax || pattern is VarPatternSyntax)
            {
                 return SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression);
            }

            // Fallback: Discard or Var matches everything (checked non-null by parent RecursivePattern usually, but here we assume true)
            // Or unknown pattern type
            return SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression);
        }
    }
}
