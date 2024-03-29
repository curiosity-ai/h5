﻿using System;
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
                        if (pattern.Pattern is DeclarationPatternSyntax declarationPattern)
                        {
                            var beforeStatement = pattern.Ancestors().OfType<StatementSyntax>().FirstOrDefault(ss => ss.Parent == block);

                            if (declarationPattern.Designation is SingleVariableDesignationSyntax designation)
                            {
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
                        }
                        else if (pattern.Pattern is ConstantPatternSyntax cps)
                        {
                            ExpressionSyntax newExpr;

                            if (cps.Expression.Kind() == SyntaxKind.NullLiteralExpression)
                            {
                                newExpr = SyntaxFactory.BinaryExpression(SyntaxKind.EqualsExpression, pattern.Expression, cps.Expression);
                            }
                            else
                            {
                                newExpr = SyntaxFactory.InvocationExpression(SyntaxFactory.MemberAccessExpression(
                                       SyntaxKind.SimpleMemberAccessExpression,
                                       pattern.Expression,
                                       SyntaxFactory.IdentifierName("Equals")), SyntaxFactory.ArgumentList(
                                       SyntaxFactory.SingletonSeparatedList(
                                           SyntaxFactory.Argument(
                                               cps.Expression))));
                            }

                            updatedPatterns[pattern] = newExpr.NormalizeWhitespace();
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
    }
}