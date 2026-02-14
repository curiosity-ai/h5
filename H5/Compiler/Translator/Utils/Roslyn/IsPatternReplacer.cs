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

        private void GetVariables(PatternSyntax pattern, SemanticModel model, List<(SingleVariableDesignationSyntax Designation, TypeSyntax Type, bool IsValueType)> variables)
        {
            if (pattern is DeclarationPatternSyntax decl)
            {
                if (decl.Designation is SingleVariableDesignationSyntax single)
                {
                    var type = decl.Type;
                    var typeInfo = model.GetTypeInfo(type);
                    bool isValueType = typeInfo.Type?.IsValueType ?? false;
                    variables.Add((single, type, isValueType));
                }
            }
            else if (pattern is VarPatternSyntax varPattern)
            {
                if (varPattern.Designation is SingleVariableDesignationSyntax single)
                {
                    var symbol = model.GetDeclaredSymbol(single) as ILocalSymbol;
                    if (symbol != null)
                    {
                        var typeSyntax = SyntaxFactory.ParseTypeName(symbol.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
                        bool isValueType = symbol.Type.IsValueType;
                        variables.Add((single, typeSyntax, isValueType));
                    }
                }
            }
            else if (pattern is RecursivePatternSyntax recursive)
            {
                if (recursive.PropertyPatternClause != null)
                {
                    foreach (var sub in recursive.PropertyPatternClause.Subpatterns)
                    {
                        GetVariables(sub.Pattern, model, variables);
                    }
                }
                if (recursive.PositionalPatternClause != null)
                {
                    foreach (var sub in recursive.PositionalPatternClause.Subpatterns)
                    {
                        GetVariables(sub.Pattern, model, variables);
                    }
                }
            }
            else if (pattern is ListPatternSyntax listPattern)
            {
                foreach (var p in listPattern.Patterns)
                {
                    GetVariables(p, model, variables);
                }
            }
            else if (pattern is SlicePatternSyntax slicePattern)
            {
                if (slicePattern.Pattern != null)
                {
                    GetVariables(slicePattern.Pattern, model, variables);
                }
            }
            else if (pattern is ParenthesizedPatternSyntax paren)
            {
                GetVariables(paren.Pattern, model, variables);
            }
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
                        var vars = new List<(SingleVariableDesignationSyntax Designation, TypeSyntax Type, bool IsValueType)>();
                        GetVariables(pattern.Pattern, model, vars);

                        if (vars.Count > 0)
                        {
                            var locals = updatedStatements.ContainsKey(beforeStatement) ? updatedStatements[beforeStatement] : new List<LocalDeclarationStatementSyntax>();

                            foreach (var v in vars)
                            {
                                if (!typesInfo.ContainsKey(v.Type.ToString()))
                                {
                                    typesInfo[v.Type.ToString()] = v.IsValueType;
                                }

                                var varDecl = SyntaxFactory.VariableDeclaration(v.Type).WithVariables(SyntaxFactory.SingletonSeparatedList(
                                    SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier(v.Designation.Identifier.ValueText))
                                ));

                                locals.Add(SyntaxFactory.LocalDeclarationStatement(varDecl).WithTrailingTrivia(SyntaxFactory.Whitespace("\n")).NormalizeWhitespace());
                            }

                            updatedStatements[beforeStatement] = locals;
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

                             updatedPatterns[pattern] = MakeCheck(pattern.Expression, pattern.Pattern, model).NormalizeWhitespace();
                        }
                        // Handle Recursive Pattern
                        else if (pattern.Pattern is RecursivePatternSyntax recursivePattern)
                        {
                            updatedPatterns[pattern] = MakeCheck(pattern.Expression, recursivePattern, model).NormalizeWhitespace();
                        }
                        else
                        {
                             // Fallback for other patterns (Relational, Binary, Unary, Type, Parenthesized)
                             // which are handled by MakeCheck but were missing here.
                             updatedPatterns[pattern] = MakeCheck(pattern.Expression, pattern.Pattern, model).NormalizeWhitespace();
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

        private ExpressionSyntax MakeCheck(ExpressionSyntax expression, PatternSyntax pattern, SemanticModel model)
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
                        var subCheck = MakeCheck(propAccess, sub.Pattern, model);
                        condition = SyntaxFactory.BinaryExpression(SyntaxKind.LogicalAndExpression, condition, subCheck);
                    }
                }

                if (recursivePattern.PositionalPatternClause != null)
                {
                    var typeInfo = model.GetTypeInfo(expression);
                    var type = typeInfo.Type ?? typeInfo.ConvertedType;

                    if (type != null && (type.IsTupleType || type.Name == "ValueTuple" || type.Name == "System.ValueTuple"))
                    {
                        int index = 0;
                        foreach (var subPattern in recursivePattern.PositionalPatternClause.Subpatterns)
                        {
                            var fieldName = $"Item{index + 1}";
                            var memberAccess = SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                expression,
                                SyntaxFactory.IdentifierName(fieldName)
                            );

                            var subCheck = MakeCheck(memberAccess, subPattern.Pattern, model);
                            condition = SyntaxFactory.BinaryExpression(SyntaxKind.LogicalAndExpression, condition, subCheck);
                            index++;
                        }
                    }
                }

                return condition;
            }
            else if (pattern is DeclarationPatternSyntax declPattern)
            {
                var isCheck = SyntaxFactory.BinaryExpression(SyntaxKind.IsExpression, expression, declPattern.Type);

                if (declPattern.Designation is SingleVariableDesignationSyntax designation)
                {
                    var varName = SyntaxFactory.IdentifierName(designation.Identifier.ValueText);
                    var castExpr = SyntaxFactory.CastExpression(declPattern.Type, expression);

                    var assignmentHelper = SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.IdentifierName("H5.Script"), SyntaxFactory.GenericName("Write").AddTypeArgumentListArguments(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword)))),
                        SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new[] {
                            SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal("({0} = {1}, true)"))),
                            SyntaxFactory.Argument(varName),
                            SyntaxFactory.Argument(castExpr)
                        }))
                    );

                    return SyntaxFactory.BinaryExpression(SyntaxKind.LogicalAndExpression, isCheck, assignmentHelper);
                }

                return isCheck;
            }
            else if (pattern is VarPatternSyntax varPattern)
            {
                if (varPattern.Designation is SingleVariableDesignationSyntax designation)
                {
                    var varName = SyntaxFactory.IdentifierName(designation.Identifier.ValueText);

                    var assignmentHelper = SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.IdentifierName("H5.Script"), SyntaxFactory.GenericName("Write").AddTypeArgumentListArguments(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword)))),
                        SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new[] {
                            SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal("({0} = {1}, true)"))),
                            SyntaxFactory.Argument(varName),
                            SyntaxFactory.Argument(expression)
                        }))
                    );
                    return assignmentHelper;
                }
                return SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression);
            }
            else if (pattern is ListPatternSyntax listPattern)
            {
                var condition = (ExpressionSyntax)SyntaxFactory.BinaryExpression(SyntaxKind.NotEqualsExpression, expression, SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression));

                int patternCount = listPattern.Patterns.Count;
                int sliceIndex = -1;
                for (int i = 0; i < patternCount; i++)
                {
                    if (listPattern.Patterns[i] is SlicePatternSyntax)
                    {
                        sliceIndex = i;
                        break;
                    }
                }

                ExpressionSyntax lengthCheck;
                var lengthAccess = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, expression, SyntaxFactory.IdentifierName("Length"));

                if (sliceIndex == -1)
                {
                    lengthCheck = SyntaxFactory.BinaryExpression(SyntaxKind.EqualsExpression, lengthAccess, SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(patternCount)));
                }
                else
                {
                    lengthCheck = SyntaxFactory.BinaryExpression(SyntaxKind.GreaterThanOrEqualExpression, lengthAccess, SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(patternCount - 1)));
                }
                condition = SyntaxFactory.BinaryExpression(SyntaxKind.LogicalAndExpression, condition, lengthCheck);

                for (int i = 0; i < patternCount; i++)
                {
                    var p = listPattern.Patterns[i];
                    if (i == sliceIndex)
                    {
                        if (p is SlicePatternSyntax slice)
                        {
                            if (slice.Pattern != null)
                            {
                                var start = SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(i));
                                var endFromEnd = SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(patternCount - 1 - i));

                                int endVal = patternCount - 1 - i;
                                string format;
                                var args = new List<ArgumentSyntax>();

                                args.Add(null);
                                args.Add(SyntaxFactory.Argument(expression));
                                args.Add(SyntaxFactory.Argument(start));

                                if (endVal == 0)
                                {
                                    format = "{0}.slice({1})";
                                }
                                else
                                {
                                    format = "{0}.slice({1}, -{2})";
                                    args.Add(SyntaxFactory.Argument(endFromEnd));
                                }

                                args[0] = SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(format)));

                                var getSubArray = SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.IdentifierName("H5.Script"),
                                        SyntaxFactory.GenericName("Write").AddTypeArgumentListArguments(SyntaxFactory.ParseTypeName("dynamic"))))
                                    .WithArgumentList(SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(args)));

                                var subCheck = MakeCheck(getSubArray, slice.Pattern, model);
                                condition = SyntaxFactory.BinaryExpression(SyntaxKind.LogicalAndExpression, condition, subCheck);
                            }
                        }
                    }
                    else
                    {
                        ExpressionSyntax elementAccess;
                        if (sliceIndex == -1 || i < sliceIndex)
                        {
                            elementAccess = SyntaxFactory.ElementAccessExpression(expression)
                               .WithArgumentList(SyntaxFactory.BracketedArgumentList(SyntaxFactory.SingletonSeparatedList(
                                   SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(i)))
                               )));
                        }
                        else
                        {
                            var offset = patternCount - i;
                            var indexCalc = SyntaxFactory.BinaryExpression(SyntaxKind.SubtractExpression, lengthAccess, SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(offset)));

                            elementAccess = SyntaxFactory.ElementAccessExpression(expression)
                               .WithArgumentList(SyntaxFactory.BracketedArgumentList(SyntaxFactory.SingletonSeparatedList(
                                   SyntaxFactory.Argument(indexCalc)
                               )));
                        }

                        var elementCheck = MakeCheck(elementAccess, p, model);
                        condition = SyntaxFactory.BinaryExpression(SyntaxKind.LogicalAndExpression, condition, elementCheck);
                    }
                }

                return condition;
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

                    return SyntaxFactory.PrefixUnaryExpression(SyntaxKind.LogicalNotExpression, SyntaxFactory.ParenthesizedExpression(MakeCheck(expression, unaryPattern.Pattern, model)));
                }
            }
            else if (pattern is BinaryPatternSyntax binaryPattern)
            {
                // pattern is left and right
                // pattern is left or right
                var leftCheck = MakeCheck(expression, binaryPattern.Left, model);
                var rightCheck = MakeCheck(expression, binaryPattern.Right, model);

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
                return MakeCheck(expression, parenPattern.Pattern, model);
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
