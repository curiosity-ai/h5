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
            var oldRoot = root;
            root = InsertVariables(root, model, rewriter);

            if (root != oldRoot)
            {
                var syntaxTree = root.SyntaxTree ?? SyntaxFactory.SyntaxTree(root, model.SyntaxTree.Options as CSharpParseOptions);
                var compilation = model.Compilation.ReplaceSyntaxTree(model.SyntaxTree, syntaxTree);
                model = compilation.GetSemanticModel(syntaxTree);
            }

            return ReplacePatterns(root, model);
        }

        private void GetVariables(PatternSyntax pattern, SemanticModel model, SharpSixRewriter rewriter, List<(SingleVariableDesignationSyntax Designation, TypeSyntax Type, bool IsValueType)> variables, ITypeSymbol expressionType)
        {
            if (pattern is DeclarationPatternSyntax decl)
            {
                if (decl.Designation is SingleVariableDesignationSyntax single)
                {
                    TypeSyntax typeSyntax = null;
                    bool isValueType = false;

                    var symbol = model.GetDeclaredSymbol(single) as ILocalSymbol;
                    if (symbol != null)
                    {
                        if (symbol.Type.TypeKind != TypeKind.Error)
                        {
                            typeSyntax = SyntaxHelper.GenerateTypeSyntax(symbol.Type, model, single.SpanStart, rewriter);
                            isValueType = symbol.Type.IsValueType;
                        }
                    }

                    if (typeSyntax == null || typeSyntax.IsMissing)
                    {
                        if ((decl.Type.IsVar || decl.Type.ToString() == "var") && expressionType != null)
                        {
                            if (expressionType.TypeKind != TypeKind.Error)
                            {
                                typeSyntax = SyntaxHelper.GenerateTypeSyntax(expressionType, model, decl.SpanStart, rewriter);
                                isValueType = expressionType.IsValueType;
                            }
                        }
                        else if (!decl.Type.IsVar && decl.Type.ToString() != "var")
                        {
                            var typeInfo = model.GetTypeInfo(decl.Type);
                            typeSyntax = decl.Type;
                            isValueType = typeInfo.Type?.IsValueType ?? false;
                        }
                    }

                    if (typeSyntax == null || typeSyntax.IsMissing || typeSyntax.ToString() == "var" || typeSyntax.ToString().Trim() == "?")
                    {
                         // Fallback to dynamic if type is unknown or 'var' (which is invalid without initializer)
                         typeSyntax = SyntaxFactory.ParseTypeName("dynamic");
                         isValueType = false;
                    }

                    variables.Add((single, typeSyntax, isValueType));
                }
            }
            else if (pattern is VarPatternSyntax varPattern)
            {
                if (varPattern.Designation is SingleVariableDesignationSyntax single)
                {
                    TypeSyntax typeSyntax = null;
                    bool isValueType = false;

                    var symbol = model.GetDeclaredSymbol(single) as ILocalSymbol;
                    if (symbol != null)
                    {
                        if (symbol.Type.TypeKind != TypeKind.Error)
                        {
                            typeSyntax = SyntaxHelper.GenerateTypeSyntax(symbol.Type, model, single.SpanStart, rewriter);
                            isValueType = symbol.Type.IsValueType;
                        }
                    }

                    if ((typeSyntax == null || typeSyntax.IsMissing) && expressionType != null)
                    {
                        if (expressionType.TypeKind != TypeKind.Error)
                        {
                            typeSyntax = SyntaxHelper.GenerateTypeSyntax(expressionType, model, varPattern.SpanStart, rewriter);
                            isValueType = expressionType.IsValueType;
                        }
                    }

                    if (typeSyntax == null || typeSyntax.IsMissing || typeSyntax.ToString() == "var" || typeSyntax.ToString().Trim() == "?")
                    {
                         typeSyntax = SyntaxFactory.ParseTypeName("dynamic");
                         isValueType = false;
                    }

                    variables.Add((single, typeSyntax, isValueType));
                }
            }
            else if (pattern is RecursivePatternSyntax recursive)
            {
                if (recursive.Type != null)
                {
                    var ti = model.GetTypeInfo(recursive.Type);
                    if (ti.Type != null)
                    {
                        expressionType = ti.Type;
                    }
                }

                if (recursive.PropertyPatternClause != null)
                {
                    foreach (var sub in recursive.PropertyPatternClause.Subpatterns)
                    {
                        ITypeSymbol subType = null;
                        if (expressionType != null)
                        {
                            var memberExpr = GetSubPatternExpression(sub);
                            if (memberExpr != null)
                            {
                                subType = GetMemberType(expressionType, memberExpr);
                            }
                        }
                        GetVariables(sub.Pattern, model, rewriter, variables, subType);
                    }
                }
                if (recursive.PositionalPatternClause != null)
                {
                    var type = expressionType;
                    if (type != null && (type.IsTupleType || type.Name == "ValueTuple" || type.Name == "System.ValueTuple"))
                    {
                        int index = 0;
                        foreach (var sub in recursive.PositionalPatternClause.Subpatterns)
                        {
                            ITypeSymbol subType = null;
                            if (type is INamedTypeSymbol nts && nts.IsTupleType && index < nts.TupleElements.Length)
                            {
                                subType = nts.TupleElements[index].Type;
                            }
                            else
                            {
                                var fieldName = $"Item{index + 1}";
                                var field = type.GetMembers(fieldName).FirstOrDefault() as IFieldSymbol;
                                if (field != null) subType = field.Type;
                            }
                            GetVariables(sub.Pattern, model, rewriter, variables, subType);
                            index++;
                        }
                    }
                    else
                    {
                        foreach (var sub in recursive.PositionalPatternClause.Subpatterns)
                        {
                            GetVariables(sub.Pattern, model, rewriter, variables, null);
                        }
                    }
                }
            }
            else if (pattern is ListPatternSyntax listPattern)
            {
                ITypeSymbol elementType = null;
                if (expressionType is IArrayTypeSymbol ats)
                {
                    elementType = ats.ElementType;
                }
                else if (expressionType is INamedTypeSymbol nts)
                {
                    var indexer = nts.GetMembers().OfType<IPropertySymbol>().FirstOrDefault(p => p.IsIndexer);
                    if (indexer != null)
                    {
                        elementType = indexer.Type;
                    }
                }

                foreach (var p in listPattern.Patterns)
                {
                    if (p is SlicePatternSyntax)
                    {
                        GetVariables(p, model, rewriter, variables, expressionType);
                    }
                    else
                    {
                        GetVariables(p, model, rewriter, variables, elementType);
                    }
                }
            }
            else if (pattern is SlicePatternSyntax slicePattern)
            {
                if (slicePattern.Pattern != null)
                {
                    GetVariables(slicePattern.Pattern, model, rewriter, variables, expressionType);
                }
            }
            else if (pattern is ParenthesizedPatternSyntax paren)
            {
                GetVariables(paren.Pattern, model, rewriter, variables, expressionType);
            }
            else if (pattern is BinaryPatternSyntax binary)
            {
                GetVariables(binary.Left, model, rewriter, variables, expressionType);
                GetVariables(binary.Right, model, rewriter, variables, expressionType);
            }
            else if (pattern is UnaryPatternSyntax unary)
            {
                GetVariables(unary.Pattern, model, rewriter, variables, expressionType);
            }
        }

        public SyntaxNode InsertVariables(SyntaxNode root, SemanticModel model, SharpSixRewriter rewriter)
        {
            var patterns = root
                .DescendantNodes()
                .OfType<IsPatternExpressionSyntax>();

            var updatedStatements = new Dictionary<SyntaxNode, List<LocalDeclarationStatementSyntax>>();
            var declaredVariables = new Dictionary<SyntaxNode, HashSet<string>>();

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

                        var typeInfo = model.GetTypeInfo(pattern.Expression);
                        var exprType = typeInfo.Type ?? typeInfo.ConvertedType;

                        GetVariables(pattern.Pattern, model, rewriter, vars, exprType);

                        if (vars.Count > 0)
                        {
                            var locals = updatedStatements.ContainsKey(beforeStatement) ? updatedStatements[beforeStatement] : new List<LocalDeclarationStatementSyntax>();

                            if (!declaredVariables.ContainsKey(beforeStatement))
                            {
                                declaredVariables[beforeStatement] = new HashSet<string>();
                            }
                            var scopeVars = declaredVariables[beforeStatement];

                            foreach (var v in vars)
                            {
                                var varName = v.Designation.Identifier.ValueText;
                                if (scopeVars.Contains(varName))
                                {
                                    continue;
                                }
                                scopeVars.Add(varName);

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
                                                SyntaxFactory.ParseName("global::H5.Script"),
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
                             updatedPatterns[pattern] = SyntaxFactory.ParenthesizedExpression(MakeCheck(pattern.Expression, pattern.Pattern, model, null)).NormalizeWhitespace();
                        }
                        // Handle Recursive Pattern
                        else if (pattern.Pattern is RecursivePatternSyntax recursivePattern)
                        {
                            updatedPatterns[pattern] = SyntaxFactory.ParenthesizedExpression(MakeCheck(pattern.Expression, recursivePattern, model, null)).NormalizeWhitespace();
                        }
                        else
                        {
                             updatedPatterns[pattern] = SyntaxFactory.ParenthesizedExpression(MakeCheck(pattern.Expression, pattern.Pattern, model, null)).NormalizeWhitespace();
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

        private ExpressionSyntax MakeCheck(ExpressionSyntax expression, PatternSyntax pattern, SemanticModel model, ITypeSymbol expressionType = null)
        {
            if (expressionType == null)
            {
                try
                {
                    var ti = model.GetTypeInfo(expression);
                    expressionType = ti.Type ?? ti.ConvertedType;
                }
                catch
                {
                    // Ignore, expression might be synthesized
                }
            }

            if (pattern is ConstantPatternSyntax constPattern)
            {
                if (constPattern.Expression.IsKind(SyntaxKind.NullLiteralExpression))
                {
                    return SyntaxFactory.BinaryExpression(SyntaxKind.EqualsExpression, expression, constPattern.Expression);
                }

                if (expressionType is INamedTypeSymbol namedType &&
                    (namedType.Name == "ReadOnlySpan" || namedType.Name == "Span") &&
                    namedType.ContainingNamespace?.ToDisplayString() == "System" &&
                    namedType.TypeArguments.Length == 1 &&
                    namedType.TypeArguments[0].SpecialType == SpecialType.System_Char)
                {
                    return SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.ParseName("global::H5.Script"),
                            SyntaxFactory.GenericName("Write").AddTypeArgumentListArguments(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword)))
                        ),
                        SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new[] {
                            SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal("H5.equals({0}, {1})"))),
                            SyntaxFactory.Argument(expression),
                            SyntaxFactory.Argument(constPattern.Expression)
                        }))
                    );
                }

                return SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, expression, SyntaxFactory.IdentifierName("Equals")),
                    SyntaxFactory.ArgumentList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Argument(constPattern.Expression)))
                );
            }
            else if (pattern is RecursivePatternSyntax recursivePattern)
            {
                var condition = (ExpressionSyntax)SyntaxFactory.BinaryExpression(SyntaxKind.NotEqualsExpression, expression, SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression));

                ExpressionSyntax typedExpression = expression;

                if (recursivePattern.Type != null)
                {
                    var typeCheck = SyntaxFactory.BinaryExpression(SyntaxKind.IsExpression, expression, recursivePattern.Type);
                    condition = SyntaxFactory.BinaryExpression(SyntaxKind.LogicalAndExpression, condition, typeCheck);

                    typedExpression = SyntaxFactory.ParenthesizedExpression(SyntaxFactory.CastExpression(recursivePattern.Type, expression));

                    var ti = model.GetTypeInfo(recursivePattern.Type);
                    if (ti.Type != null)
                    {
                        expressionType = ti.Type;
                    }
                }

                if (recursivePattern.PropertyPatternClause != null)
                {
                    foreach (var sub in recursivePattern.PropertyPatternClause.Subpatterns)
                    {
                        var memberExpr = GetSubPatternExpression(sub);
                        var propAccess = GetMemberAccess(typedExpression, memberExpr);

                        ITypeSymbol subType = null;
                        if (expressionType != null && memberExpr != null)
                        {
                            subType = GetMemberType(expressionType, memberExpr);
                        }

                        var subCheck = MakeCheck(propAccess, sub.Pattern, model, subType);
                        condition = SyntaxFactory.BinaryExpression(SyntaxKind.LogicalAndExpression, condition, SyntaxFactory.ParenthesizedExpression(subCheck));
                    }
                }

                if (recursivePattern.PositionalPatternClause != null)
                {
                    var type = expressionType;

                    if (type != null && (type.IsTupleType || type.Name == "ValueTuple" || type.Name == "System.ValueTuple"))
                    {
                        int index = 0;
                        foreach (var subPattern in recursivePattern.PositionalPatternClause.Subpatterns)
                        {
                            var fieldName = $"Item{index + 1}";
                            var memberAccess = SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                typedExpression,
                                SyntaxFactory.IdentifierName(fieldName)
                            );

                            ITypeSymbol subType = null;
                            if (type is INamedTypeSymbol nts && nts.IsTupleType && index < nts.TupleElements.Length)
                            {
                                subType = nts.TupleElements[index].Type;
                            }
                            else
                            {
                                var field = type.GetMembers(fieldName).FirstOrDefault() as IFieldSymbol;
                                if (field != null) subType = field.Type;
                            }

                            var subCheck = MakeCheck(memberAccess, subPattern.Pattern, model, subType);
                            condition = SyntaxFactory.BinaryExpression(SyntaxKind.LogicalAndExpression, condition, SyntaxFactory.ParenthesizedExpression(subCheck));
                            index++;
                        }
                    }
                    else
                    {
                        int index = 0;
                        foreach (var subPattern in recursivePattern.PositionalPatternClause.Subpatterns)
                        {
                            var fieldName = $"Item{index + 1}";
                            var memberAccess = SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                typedExpression,
                                SyntaxFactory.IdentifierName(fieldName)
                            );

                            var subCheck = MakeCheck(memberAccess, subPattern.Pattern, model, null);
                            condition = SyntaxFactory.BinaryExpression(SyntaxKind.LogicalAndExpression, condition, SyntaxFactory.ParenthesizedExpression(subCheck));
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
                        SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.ParseName("global::H5.Script"), SyntaxFactory.GenericName("Write").AddTypeArgumentListArguments(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword)))),
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
                        SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.ParseName("global::H5.Script"), SyntaxFactory.GenericName("Write").AddTypeArgumentListArguments(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword)))),
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

                ITypeSymbol elementType = null;
                if (expressionType is IArrayTypeSymbol ats)
                {
                    elementType = ats.ElementType;
                }
                else if (expressionType is INamedTypeSymbol nts)
                {
                    var indexer = nts.GetMembers().OfType<IPropertySymbol>().FirstOrDefault(p => p.IsIndexer);
                    if (indexer != null)
                    {
                        elementType = indexer.Type;
                    }
                }

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
                                        SyntaxFactory.ParseName("global::H5.Script"),
                                        SyntaxFactory.GenericName("Write").AddTypeArgumentListArguments(SyntaxFactory.ParseTypeName("dynamic"))))
                                    .WithArgumentList(SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(args)));

                                var subCheck = MakeCheck(getSubArray, slice.Pattern, model, expressionType);
                                condition = SyntaxFactory.BinaryExpression(SyntaxKind.LogicalAndExpression, condition, SyntaxFactory.ParenthesizedExpression(subCheck));
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

                        var elementCheck = MakeCheck(elementAccess, p, model, elementType);
                        condition = SyntaxFactory.BinaryExpression(SyntaxKind.LogicalAndExpression, condition, SyntaxFactory.ParenthesizedExpression(elementCheck));
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

                    return SyntaxFactory.PrefixUnaryExpression(SyntaxKind.LogicalNotExpression, SyntaxFactory.ParenthesizedExpression(MakeCheck(expression, unaryPattern.Pattern, model, expressionType)));
                }
            }
            else if (pattern is BinaryPatternSyntax binaryPattern)
            {
                // pattern is left and right
                // pattern is left or right
                var leftCheck = MakeCheck(expression, binaryPattern.Left, model, expressionType);
                var rightCheck = MakeCheck(expression, binaryPattern.Right, model, expressionType);

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
                return MakeCheck(expression, parenPattern.Pattern, model, expressionType);
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

        private static ExpressionSyntax GetSubPatternExpression(SubpatternSyntax sub)
        {
            if (sub.NameColon != null)
            {
                return sub.NameColon.Name;
            }
            else if (sub.ExpressionColon != null)
            {
                return sub.ExpressionColon.Expression;
            }
            return null;
        }

        private ITypeSymbol GetMemberType(ITypeSymbol parentType, ExpressionSyntax memberExpression)
        {
            if (parentType == null)
            {
                return null;
            }

            if (memberExpression is IdentifierNameSyntax id)
            {
                var members = parentType.GetMembers(id.Identifier.ValueText);
                var member = members.FirstOrDefault();
                if (member is IPropertySymbol ps) return ps.Type;
                if (member is IFieldSymbol fs) return fs.Type;
            }
            else if (memberExpression is MemberAccessExpressionSyntax memberAccess)
            {
                var expressionType = GetMemberType(parentType, memberAccess.Expression);
                return GetMemberType(expressionType, memberAccess.Name);
            }

            return null;
        }

        private ExpressionSyntax GetMemberAccess(ExpressionSyntax parentExpression, ExpressionSyntax memberExpression)
        {
            if (memberExpression is IdentifierNameSyntax id)
            {
                return SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, parentExpression, id);
            }
            else if (memberExpression is MemberAccessExpressionSyntax memberAccess)
            {
                var expression = GetMemberAccess(parentExpression, memberAccess.Expression);
                return SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, expression, memberAccess.Name);
            }
            return parentExpression;
        }
    }
}
