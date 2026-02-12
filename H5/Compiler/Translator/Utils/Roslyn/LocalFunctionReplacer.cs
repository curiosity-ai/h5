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
    public class LocalFunctionReplacer : ICSharpReplacer
    {
        private static SyntaxNode GetFirstUsageLocalFunc(SemanticModel model, LocalFunctionStatementSyntax node, SyntaxNode root, List<SyntaxNode> ignore = null)
        {
            var symbol = model.GetSymbolInfo(node).Symbol ?? model.GetDeclaredSymbol(node);

            return root.DescendantNodes(x => x != node).OfType<IdentifierNameSyntax>().Where(x =>
            {
                if (x.Identifier.ValueText != node.Identifier.ValueText)
                {
                    return false;
                }

                var info = model.GetSymbolInfo(x);
                var xSymbol = info.Symbol;

                if (xSymbol == null && info.CandidateSymbols != null && info.CandidateSymbols.Length > 0)
                {
                    xSymbol = info.CandidateSymbols[0];
                }

                if (xSymbol == null || !SymbolEqualityComparer.Default.Equals(symbol, xSymbol))
                {
                    return false;
                }

                if (ignore != null && ignore.Contains(x))
                {
                    return false;
                }

                return true;
            }).FirstOrDefault();
        }

        public SyntaxNode Replace(SyntaxNode root, SemanticModel model, SharpSixRewriter rewriter)
        {
            // Sort by depth descending (innermost first) to handle nesting,
            // then by start position (top to bottom) to handle siblings in order.
            var localFns = root.DescendantNodes()
                               .OfType<LocalFunctionStatementSyntax>()
                               .Select(node => new { Node = node, Depth = node.Ancestors().Count() })
                               .OrderByDescending(x => x.Depth)
                               .ThenBy(x => x.Node.SpanStart)
                               .Select(x => x.Node);

            var updatedBlocks = new Dictionary<SyntaxNode, List<StatementSyntax>>();
            var updatedClasses = new Dictionary<TypeDeclarationSyntax, List<DelegateDeclarationSyntax>>();

            // Capture init vars with their original position to ensure declaration order is preserved.
            var tempInits = new Dictionary<SyntaxNode, List<(StatementSyntax Init, int Position)>>();

            foreach (var fn in localFns)
            {
                try
                {
                    var parentNode = fn.Parent;
                    var usage = GetFirstUsageLocalFunc(model, fn, parentNode);
                    var beforeStatement = usage?.Ancestors().OfType<StatementSyntax>().FirstOrDefault(ss => ss.Parent == parentNode);

                    if (beforeStatement is LocalFunctionStatementSyntax beforeFn)
                    {
                        List<SyntaxNode> ignore = new List<SyntaxNode>();
                        var usageFn = usage;
                        var beforeStatementFn = beforeStatement;
                        while (beforeStatementFn != null && beforeStatementFn is LocalFunctionStatementSyntax)
                        {
                            ignore.Add(usageFn);
                            usageFn = GetFirstUsageLocalFunc(model, fn, parentNode, ignore);
                            beforeStatementFn = usageFn?.Ancestors().OfType<StatementSyntax>().FirstOrDefault(ss => ss.Parent == parentNode);
                        }

                        usage = GetFirstUsageLocalFunc(model, beforeFn, parentNode);
                        beforeStatement = usage?.Ancestors().OfType<StatementSyntax>().FirstOrDefault(ss => ss.Parent == parentNode);

                        if (beforeStatementFn != null && (beforeStatement == null || beforeStatementFn.SpanStart < beforeStatement.SpanStart))
                        {
                            beforeStatement = beforeStatementFn;
                        }
                    }

                    var customDelegate = false;

                    if (fn.TypeParameterList != null && fn.TypeParameterList.Parameters.Count > 0)
                    {
                        customDelegate = true;
                    }
                    else
                    {
                        foreach (var prm in fn.ParameterList.Parameters)
                        {
                            if (prm.Default != null)
                            {
                                customDelegate = true;
                                break;
                            }

                            foreach (var modifier in prm.Modifiers)
                            {
                                var kind = modifier.Kind();
                                if (kind == SyntaxKind.RefKeyword ||
                                    kind == SyntaxKind.OutKeyword ||
                                    kind == SyntaxKind.ParamsKeyword)
                                {
                                    customDelegate = true;
                                    break;
                                }
                            }

                            if (customDelegate)
                            {
                                break;
                            }
                        }
                    }

                    var returnType = fn.ReturnType.WithoutLeadingTrivia().WithoutTrailingTrivia();
                    var isVoid = returnType is PredefinedTypeSyntax ptsInstance && ptsInstance.Keyword.IsKind(SyntaxKind.VoidKeyword);

                    TypeSyntax varType;

                    if (customDelegate)
                    {
                        var typeDecl = parentNode.Ancestors().OfType<TypeDeclarationSyntax>().FirstOrDefault();
                        var delegates = updatedClasses.ContainsKey(typeDecl) ? updatedClasses[typeDecl] : new List<DelegateDeclarationSyntax>();
                        var name = $"___{fn.Identifier.ValueText}_Delegate_{delegates.Count}";
                        var delDecl = SyntaxFactory.DelegateDeclaration(returnType, SyntaxFactory.Identifier(name))
                            .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PrivateKeyword)))
                            .WithParameterList(fn.ParameterList).NormalizeWhitespace();
                        delegates.Add(delDecl);
                        updatedClasses[typeDecl] = delegates;

                        varType = SyntaxFactory.IdentifierName(name);
                    }
                    else if (isVoid)
                    {
                        if (fn.ParameterList.Parameters.Count == 0)
                        {
                            varType = SyntaxFactory.QualifiedName(SyntaxFactory.IdentifierName("System"), SyntaxFactory.IdentifierName("Action"));
                        }
                        else
                        {
                            varType = SyntaxFactory.QualifiedName
                            (
                                SyntaxFactory.IdentifierName("System"),
                                SyntaxFactory.GenericName("Action").WithTypeArgumentList
                                (
                                    SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(fn.ParameterList.Parameters.Select(p => p.Type)))
                                )
                            );
                        }
                    }
                    else
                    {
                        if (fn.ParameterList.Parameters.Count == 0)
                        {
                            varType = SyntaxFactory.QualifiedName(
                                SyntaxFactory.IdentifierName("System"),
                                SyntaxFactory.GenericName("Func").WithTypeArgumentList(
                                    SyntaxFactory.TypeArgumentList(SyntaxFactory.SingletonSeparatedList(returnType))
                                )
                            );
                        }
                        else
                        {
                            varType = SyntaxFactory.QualifiedName
                            (
                                SyntaxFactory.IdentifierName("System"),
                                SyntaxFactory.GenericName("Func").WithTypeArgumentList
                                (
                                    SyntaxFactory.TypeArgumentList(
                                        SyntaxFactory.SeparatedList(
                                            fn.ParameterList.Parameters.Select(p => p.Type).Concat(
                                                new TypeSyntax[] { returnType }
                                            )
                                        )
                                    )
                                )
                            );
                        }
                    }

                    List<ParameterSyntax> prms = new List<ParameterSyntax>();

                    if (customDelegate)
                    {
                        foreach (var prm in fn.ParameterList.Parameters)
                        {
                            var newPrm = prm.WithDefault(null);
                            var idx = newPrm.Modifiers.IndexOf(SyntaxKind.ParamsKeyword);

                            if (idx > -1)
                            {
                                newPrm = newPrm.WithModifiers(newPrm.Modifiers.RemoveAt(idx));
                            }

                            prms.Add(newPrm);
                        }
                    }
                    else
                    {
                        foreach (var prm in fn.ParameterList.Parameters)
                        {
                            prms.Add(SyntaxFactory.Parameter(prm.Identifier));
                        }
                    }

                    var initVar = SyntaxFactory.LocalDeclarationStatement(SyntaxFactory.VariableDeclaration(varType).WithVariables(
                        SyntaxFactory.SingletonSeparatedList(
                            SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier(fn.Identifier.ValueText)).WithInitializer
                            (
                                SyntaxFactory.EqualsValueClause(SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression))
                            )
                        )
                    )).NormalizeWhitespace().WithTrailingTrivia(SyntaxFactory.Whitespace(Emitter.NEW_LINE));

                    BlockSyntax lambdaBodyBlock = fn.Body;
                    if (fn.Body != null)
                    {
                        var descendantBlocks = updatedBlocks.Keys.Where(k => k.Ancestors().Contains(fn.Body)).ToList();

                        if (descendantBlocks.Count > 0)
                        {
                            lambdaBodyBlock = lambdaBodyBlock.ReplaceNodes(descendantBlocks, (b1, b2) => {
                                var stmts = updatedBlocks[b1];
                                if (tempInits.ContainsKey(b1)) {
                                    var sortedInits = tempInits[b1].OrderBy(x => x.Position).Select(x => x.Init);
                                    stmts = sortedInits.Concat(stmts).ToList();
                                    tempInits.Remove(b1);
                                }

                                if (b2 is BlockSyntax) return ((BlockSyntax)b2).WithStatements(SyntaxFactory.List(stmts));
                                if (b2 is SwitchSectionSyntax) return ((SwitchSectionSyntax)b2).WithStatements(SyntaxFactory.List(stmts));
                                return b2;
                            });

                            foreach(var k in descendantBlocks) updatedBlocks.Remove(k);
                        }

                        if (updatedBlocks.ContainsKey(fn.Body) || tempInits.ContainsKey(fn.Body))
                        {
                            var stmts = updatedBlocks.ContainsKey(fn.Body) ? updatedBlocks[fn.Body] : lambdaBodyBlock.Statements.ToList();
                            if (tempInits.ContainsKey(fn.Body))
                            {
                                var sortedInits = tempInits[fn.Body].OrderBy(x => x.Position).Select(x => x.Init);
                                stmts.InsertRange(0, sortedInits);
                                tempInits.Remove(fn.Body);
                            }
                            updatedBlocks.Remove(fn.Body);
                            lambdaBodyBlock = lambdaBodyBlock.WithStatements(SyntaxFactory.List(stmts));
                        }
                    }

                    var lambda = SyntaxFactory.ParenthesizedLambdaExpression(lambdaBodyBlock ?? (CSharpSyntaxNode)fn.ExpressionBody.Expression).WithParameterList(
                            SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList(prms))
                        );

                    if (fn.Modifiers.Any(SyntaxKind.AsyncKeyword))
                    {
                        lambda = lambda.WithAsyncKeyword(SyntaxFactory.Token(SyntaxKind.AsyncKeyword));
                    }

                    var assignment = SyntaxFactory.ExpressionStatement(SyntaxFactory.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                        SyntaxFactory.IdentifierName(fn.Identifier.ValueText),
                        lambda
                    )).NormalizeWhitespace().WithTrailingTrivia(SyntaxFactory.Whitespace(Emitter.NEW_LINE));

                    List<StatementSyntax> statements = null;

                    if (updatedBlocks.ContainsKey(parentNode))
                    {
                        statements = updatedBlocks[parentNode];
                    }
                    else
                    {
                        if (parentNode is BlockSyntax bs)
                        {
                            statements = bs.Statements.ToList();
                        }
                        else if (parentNode is SwitchSectionSyntax sss)
                        {
                            statements = sss.Statements.ToList();
                        }
                    }

                    var fnIdx = statements.IndexOf(fn);
                    statements.Insert(beforeStatement != null ? statements.IndexOf(beforeStatement) : Math.Max(0, fnIdx), assignment);
                    updatedBlocks[parentNode] = statements;

                    // Collect inits with position
                    if (!tempInits.ContainsKey(parentNode))
                    {
                        tempInits[parentNode] = new List<(StatementSyntax, int)>();
                    }
                    tempInits[parentNode].Add((initVar, fn.SpanStart));
                }
                catch (Exception e)
                {
                    throw new ReplacerException(fn, e);
                }
            }

            // Populate remaining inits
            foreach (var key in tempInits.Keys)
            {
                var sortedInits = tempInits[key].OrderBy(x => x.Position).Select(x => x.Init).ToList();
                if (updatedBlocks.ContainsKey(key))
                {
                    updatedBlocks[key] = sortedInits.Concat(updatedBlocks[key]).ToList();
                }
                else
                {
                    // This case should theoretically not be hit if updatedBlocks always has parentNode from assignments
                    // but for safety, if we have inits for a block not otherwise updated, we should handle it
                    // although technically every fn adds an assignment to updatedBlocks.
                }
            }

            if (updatedClasses.Count > 0)
            {
                root = root.ReplaceNodes(updatedClasses.Keys, (t1, t2) =>
                {
                    var members = updatedClasses[t1].ToArray();

                    t1 = t1.ReplaceNodes(updatedBlocks.Keys, (b1, b2) => {
                        SyntaxNode result = b1 is SwitchSectionSyntax sss ? sss.WithStatements(SyntaxFactory.List(updatedBlocks[b1])) : (SyntaxNode)(((BlockSyntax)b1).WithStatements(SyntaxFactory.List(updatedBlocks[b1])));
                        return result;
                    });

                    if (t1 is ClassDeclarationSyntax cls)
                    {
                        return cls.AddMembers(members);
                    }

                    if (t2 is StructDeclarationSyntax structDecl)
                    {
                        return structDecl.AddMembers(members);
                    }

                    return t1;
                });
            }
            else if (updatedBlocks.Count > 0)
            {
                root = root.ReplaceNodes(updatedBlocks.Keys, (b1, b2) =>
                {
                    SyntaxNode result = b2 is SwitchSectionSyntax sss ? sss.WithStatements(SyntaxFactory.List(updatedBlocks[b1])) : (SyntaxNode)(((BlockSyntax)b2).WithStatements(SyntaxFactory.List(updatedBlocks[b1])));
                    return result;
                });
            }

            root = root.RemoveNodes(root.DescendantNodes().OfType<LocalFunctionStatementSyntax>(), SyntaxRemoveOptions.KeepTrailingTrivia | SyntaxRemoveOptions.KeepLeadingTrivia);

            return root;
        }
    }
}