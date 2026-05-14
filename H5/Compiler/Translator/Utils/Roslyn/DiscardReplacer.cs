using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System.Threading;

namespace H5.Translator
{
    public class DiscardReplacer
    {
        private const string DISCARD_IDENTIFIER = "_";
        private const string DISCARD_VARIABLE = "_discard";
        public SyntaxNode Replace(SyntaxNode root, SemanticModel model, Func<SyntaxNode, Tuple<SyntaxTree, SemanticModel>> updater, SharpSixRewriter rewriter)
        {
            var discards = root
               .DescendantNodes()
               .OfType<DiscardDesignationSyntax>();

            var outVars = root
                .DescendantNodes()
                .OfType<ArgumentSyntax>()
                .Where(arg => arg.Expression is DeclarationExpressionSyntax && arg.RefOrOutKeyword.IsKind(SyntaxKind.OutKeyword));

            var outDiscardVars = root
                .DescendantNodes()
                .OfType<ArgumentSyntax>()
                .Where(arg => {
                    if (arg.Expression is IdentifierNameSyntax ins && ins.Identifier.ValueText == DISCARD_IDENTIFIER)
                    {
                        var si = model.GetSymbolInfo(arg.Expression);
                        return si.Symbol == null || si.Symbol is IDiscardSymbol;
                    }

                    return false;
                 });

            var discardAssigments = root
                .DescendantNodes()
                .OfType<AssignmentExpressionSyntax>()
                .Where(assignment => {
                    if (assignment.Left is IdentifierNameSyntax ins && ins.Identifier.ValueText == DISCARD_IDENTIFIER)
                    {
                        var si = model.GetSymbolInfo(assignment.Left);
                        return si.Symbol == null || si.Symbol is IDiscardSymbol;
                    }

                    return false;
                });

            var updatedMembers = new Dictionary<MemberAccessExpressionSyntax, string>();
            foreach (var memberAccess in root.DescendantNodes().OfType<MemberAccessExpressionSyntax>())
            {
                try
                {
                    var symbol = model.GetSymbolInfo(memberAccess).Symbol;
                    
                    if (symbol != null && symbol is IFieldSymbol && symbol.ContainingType.IsTupleType)
                    {
                        var field = symbol as IFieldSymbol;
                        var tupleField = field.CorrespondingTupleField;
                        updatedMembers[memberAccess] = tupleField.Name;
                    }
                }
                catch(Exception E)
                {
                    Console.WriteLine(E);
                }

     
            }

            var updatedStatements = new Dictionary<StatementSyntax, List<LocalDeclarationStatementSyntax>>();
            var updatedDiscards = new Dictionary<DiscardDesignationSyntax, string>();
            var updatedDiscardVars = new Dictionary<ArgumentSyntax, string>();

            var tempIndex = 0;

            foreach (var discard in discards)
            {
                try
                {
                    var noLocal = false;
                    var parentTuple = discard.GetParent<TupleExpressionSyntax>();
                    if (parentTuple != null && parentTuple.Parent is AssignmentExpressionSyntax ae && ae.Left == parentTuple)
                    {
                        noLocal = true;
                    }

                    var typeInfo = model.GetTypeInfo(discard.Parent);
                    var beforeStatement = discard.Ancestors().OfType<StatementSyntax>().FirstOrDefault();

                    if (beforeStatement != null)
                    {
                        if (typeInfo.Type != null)
                        {
                            string instance = DISCARD_VARIABLE + ++tempIndex;
                            if (beforeStatement.Parent != null)
                            {
                                var info = LocalUsageGatherer.GatherInfo(model, beforeStatement.Parent);

                                while (info.DirectlyOrIndirectlyUsedLocals.Any(s => s.Name == instance) || info.Names.Contains(instance))
                                {
                                    instance = DISCARD_VARIABLE + ++tempIndex;
                                }
                            }

                            if (!noLocal)
                            {
                                var locals = updatedStatements.ContainsKey(beforeStatement) ? updatedStatements[beforeStatement] : new List<LocalDeclarationStatementSyntax>();
                                var varDecl = SyntaxFactory.VariableDeclaration(SyntaxHelper.GenerateTypeSyntax(typeInfo.Type, model, discard.Parent.GetLocation().SourceSpan.Start, rewriter)).WithVariables(SyntaxFactory.SingletonSeparatedList(
                                    SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier(instance))
                                ));

                                var local = SyntaxFactory.LocalDeclarationStatement(varDecl).NormalizeWhitespace().WithTrailingTrivia(SyntaxFactory.Whitespace("\n"));
                                locals.Add(local);

                                updatedStatements[beforeStatement] = locals;
                            }

                            updatedDiscards[discard] = instance;
                        }
                        else if (discard.Parent is DeclarationPatternSyntax && !(discard.Parent.Parent is IsPatternExpressionSyntax))
                        {
                            string instance = DISCARD_VARIABLE + ++tempIndex;
                            if (beforeStatement.Parent != null)
                            {
                                var info = LocalUsageGatherer.GatherInfo(model, beforeStatement.Parent);

                                while (info.DirectlyOrIndirectlyUsedLocals.Any(s => s.Name == instance) || info.Names.Contains(instance))
                                {
                                    instance = DISCARD_VARIABLE + ++tempIndex;
                                }
                            }

                            updatedDiscards[discard] = instance;
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new ReplacerException(discard, e);
                }
            }

            foreach (var discardVar in outDiscardVars)
            {
                try
                {
                    var typeInfo = model.GetTypeInfo(discardVar.Expression);

                    if (typeInfo.Type != null)
                    {
                        var beforeStatement = discardVar.Ancestors().OfType<StatementSyntax>().FirstOrDefault();
                        if (beforeStatement != null)
                        {
                            string instance = DISCARD_VARIABLE + ++tempIndex;
                            if (beforeStatement.Parent != null)
                            {
                                var info = LocalUsageGatherer.GatherInfo(model, beforeStatement.Parent);

                                while (info.DirectlyOrIndirectlyUsedLocals.Any(s => s.Name == instance) || info.Names.Contains(instance))
                                {
                                    instance = DISCARD_VARIABLE + ++tempIndex;
                                }
                            }

                            var locals = updatedStatements.ContainsKey(beforeStatement) ? updatedStatements[beforeStatement] : new List<LocalDeclarationStatementSyntax>();
                            var varDecl = SyntaxFactory.VariableDeclaration(SyntaxHelper.GenerateTypeSyntax(typeInfo.Type, model, discardVar.Expression.GetLocation().SourceSpan.Start, rewriter)).WithVariables(SyntaxFactory.SingletonSeparatedList(
                                SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier(instance))
                            ));

                            var local = SyntaxFactory.LocalDeclarationStatement(varDecl).NormalizeWhitespace().WithTrailingTrivia(SyntaxFactory.Whitespace("\n"));
                            locals.Add(local);

                            updatedStatements[beforeStatement] = locals;
                            updatedDiscardVars[discardVar] = instance;
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new ReplacerException(discardVar, e);
                }
            }

            // Track names already claimed in each hoist-target scope so that two `out var X`
            // declarations hoisted into the same block don't produce a duplicate-name
            // declaration. When a collision is detected, the second occurrence is renamed
            // (e.g. `t` -> `t1`) and any references that resolve to the renamed symbol are
            // rewritten to match.
            var scopeClaimedNames = new Dictionary<SyntaxNode, HashSet<string>>();
            var outVarFinalNames = new Dictionary<ArgumentSyntax, string>();
            var referencesToRename = new Dictionary<IdentifierNameSyntax, string>();

            foreach (var outVar in outVars)
            {
                try
                {
                    if (((DeclarationExpressionSyntax)outVar.Expression).Designation.Kind() == SyntaxKind.DiscardDesignation)
                    {
                        continue;
                    }

                    var typeInfo = model.GetTypeInfo(outVar.Expression);

                    if (typeInfo.Type != null)
                    {
                        var beforeStatement = outVar.Ancestors().OfType<StatementSyntax>().FirstOrDefault();
                        if (beforeStatement != null)
                        {
                            if (outVar.Expression is DeclarationExpressionSyntax de)
                            {
                                if (de.Designation is SingleVariableDesignationSyntax designation)
                                {
                                    var scope = (SyntaxNode)beforeStatement.Parent ?? beforeStatement;
                                    if (!scopeClaimedNames.TryGetValue(scope, out var claimed))
                                    {
                                        claimed = new HashSet<string>();
                                        var info = LocalUsageGatherer.GatherInfo(model, scope);
                                        foreach (var n in info.Names)
                                        {
                                            claimed.Add(n);
                                        }
                                        scopeClaimedNames[scope] = claimed;
                                    }

                                    var originalName = designation.Identifier.ValueText;
                                    var finalName = originalName;

                                    if (claimed.Contains(finalName))
                                    {
                                        int i = 1;
                                        while (claimed.Contains(originalName + i))
                                        {
                                            i++;
                                        }
                                        finalName = originalName + i;

                                        // The C# semantic model can produce unreliable symbol info for
                                        // duplicate-name locals (it tends to fold subsequent declarations
                                        // onto the first symbol). Use a syntactic rewrite that renames any
                                        // matching identifier within the statement that declares the
                                        // out-var. That covers the typical case (`if (TryGet(out var t))
                                        // ... use t ...`) without depending on the semantic model.
                                        foreach (var idName in beforeStatement.DescendantNodes().OfType<IdentifierNameSyntax>())
                                        {
                                            if (idName.Identifier.ValueText != originalName)
                                            {
                                                continue;
                                            }

                                            if (idName == outVar.Expression)
                                            {
                                                continue;
                                            }

                                            referencesToRename[idName] = finalName;
                                        }
                                    }

                                    claimed.Add(finalName);
                                    outVarFinalNames[outVar] = finalName;

                                    var locals = updatedStatements.ContainsKey(beforeStatement) ? updatedStatements[beforeStatement] : new List<LocalDeclarationStatementSyntax>();
                                    var varDecl = SyntaxFactory.VariableDeclaration(SyntaxHelper.GenerateTypeSyntax(typeInfo.Type, model, outVar.Expression.GetLocation().SourceSpan.Start, rewriter)).WithVariables(SyntaxFactory.SingletonSeparatedList(
                                        SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier(finalName))
                                    ));

                                    locals.Add(SyntaxFactory.LocalDeclarationStatement(varDecl).NormalizeWhitespace().WithTrailingTrivia(SyntaxFactory.Whitespace("\n")));

                                    updatedStatements[beforeStatement] = locals;
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new ReplacerException(outVar, e);
                }
            }

            var annotatedStatemnts = new Dictionary<SyntaxAnnotation, List<LocalDeclarationStatementSyntax>>();
            var annotatedDiscards = new Dictionary<SyntaxAnnotation, string>();
            var annotatedDiscardVars = new Dictionary<SyntaxAnnotation, string>();
            var annotatedAssigments = new List<SyntaxAnnotation>();
            var annotatedMembers = new Dictionary<SyntaxAnnotation, string>();
            var annotatedOutVars = new Dictionary<SyntaxAnnotation, string>();
            var annotatedRenames = new Dictionary<SyntaxAnnotation, string>();

            var keys = updatedStatements.Keys.Cast<SyntaxNode>()
                            .Concat(updatedDiscards.Keys.Cast<SyntaxNode>())
                            .Concat(updatedDiscardVars.Keys.Cast<SyntaxNode>())
                            .Concat(discardAssigments)
                            .Concat(updatedMembers.Keys.Cast<SyntaxNode>())
                            .Concat(outVarFinalNames.Keys.Cast<SyntaxNode>())
                            .Concat(referencesToRename.Keys.Cast<SyntaxNode>());

            root = root.ReplaceNodes(keys, (n1, n2) =>
            {
                var annotation = new SyntaxAnnotation();

                if (n1 is AssignmentExpressionSyntax)
                {
                    annotatedAssigments.Add(annotation);
                }
                else if (n1 is DiscardDesignationSyntax)
                {
                    annotatedDiscards[annotation] = updatedDiscards[(DiscardDesignationSyntax)n1];
                }
                else if (n1 is ArgumentSyntax argSyn)
                {
                    if (outVarFinalNames.TryGetValue(argSyn, out var outVarName))
                    {
                        annotatedOutVars[annotation] = outVarName;
                    }
                    else
                    {
                        annotatedDiscardVars[annotation] = updatedDiscardVars[argSyn];
                    }
                }
                else if (n1 is IdentifierNameSyntax idSyn && referencesToRename.TryGetValue(idSyn, out var renameTo))
                {
                    annotatedRenames[annotation] = renameTo;
                }
                else if (n1 is MemberAccessExpressionSyntax)
                {
                    annotatedMembers[annotation] = updatedMembers[(MemberAccessExpressionSyntax)n1];
                }
                else
                {
                    annotatedStatemnts[annotation] = updatedStatements[(StatementSyntax)n1];
                }

                n2 = n2.WithAdditionalAnnotations(annotation);
                return n2;
            });

            foreach (var annotation in annotatedDiscards.Keys)
            {
                var annotatedNode = root.GetAnnotatedNodes(annotation).First();
                var name = annotatedDiscards[annotation];

                root = root.ReplaceNode(annotatedNode, SyntaxFactory.SingleVariableDesignation(SyntaxFactory.Identifier(name)).NormalizeWhitespace());
            }

            foreach (var annotation in annotatedDiscardVars.Keys)
            {
                var annotatedNode = root.GetAnnotatedNodes(annotation).First();
                var name = annotatedDiscardVars[annotation];

                root = root.ReplaceNode(annotatedNode, ((ArgumentSyntax)annotatedNode).WithExpression(SyntaxFactory.IdentifierName(name)));
            }

            foreach (var annotation in annotatedAssigments)
            {
                var annotatedNode = root.GetAnnotatedNodes(annotation).First();
                root = root.ReplaceNode(annotatedNode, ((AssignmentExpressionSyntax)annotatedNode).WithLeft(SyntaxFactory.IdentifierName("H5.Script.Discard")));
            }

            foreach (var annotation in annotatedOutVars.Keys)
            {
                var annotatedNode = root.GetAnnotatedNodes(annotation).First();
                var argNode = (ArgumentSyntax)annotatedNode;
                if (!(((DeclarationExpressionSyntax)argNode.Expression).Designation is SingleVariableDesignationSyntax))
                {
                    continue;
                }

                var finalName = annotatedOutVars[annotation];
                var replacement = SyntaxFactory.Argument(SyntaxFactory.IdentifierName(finalName))
                    .WithRefKindKeyword(SyntaxFactory.Token(SyntaxKind.OutKeyword))
                    .WithRefOrOutKeyword(SyntaxFactory.Token(SyntaxKind.OutKeyword))
                    .NormalizeWhitespace();
                root = root.ReplaceNode(annotatedNode, replacement);
            }

            // Any out-var arguments that weren't annotated above (e.g. ones introduced by other
            // rewrites running before this pass) still need the existing flat replacement so the
            // generated code uses `out X` instead of `out var X`.
            outVars = root
               .DescendantNodes()
               .OfType<ArgumentSyntax>()
               .Where(arg => arg.Expression is DeclarationExpressionSyntax && arg.RefOrOutKeyword.IsKind(SyntaxKind.OutKeyword));

            root = root.ReplaceNodes(outVars, (n1, n2) =>
            {
                if (!(((DeclarationExpressionSyntax)n2.Expression).Designation is SingleVariableDesignationSyntax designation))
                {
                    return n2;
                }

                return SyntaxFactory.Argument(SyntaxFactory.IdentifierName(designation.Identifier)).WithRefKindKeyword(SyntaxFactory.Token(SyntaxKind.OutKeyword)).WithRefOrOutKeyword(SyntaxFactory.Token(SyntaxKind.OutKeyword)).NormalizeWhitespace();
            });

            foreach (var annotation in annotatedRenames.Keys)
            {
                var annotatedNode = root.GetAnnotatedNodes(annotation).FirstOrDefault();
                if (annotatedNode == null)
                {
                    continue;
                }

                var newName = annotatedRenames[annotation];
                root = root.ReplaceNode(annotatedNode, SyntaxFactory.IdentifierName(newName).WithTriviaFrom(annotatedNode));
            }

            foreach (var annotation in annotatedStatemnts.Keys)
            {
                var annotatedNode = root.GetAnnotatedNodes(annotation).First();
                var varStatements = annotatedStatemnts[annotation];

                if (annotatedNode.Parent is BlockSyntax || !(annotatedNode is StatementSyntax))
                {
                    root = root.InsertNodesBefore(annotatedNode, varStatements);
                }
                else
                {
                    var list = new List<StatementSyntax>(varStatements);
                    list.Add((StatementSyntax)annotatedNode);
                    root = root.ReplaceNode(annotatedNode, SyntaxFactory.Block(list).NormalizeWhitespace());
                }
            }

            var discardPatterns = root.DescendantNodes().OfType<IsPatternExpressionSyntax>().Where(pattern => pattern.Pattern is DeclarationPatternSyntax dp && dp.Designation.Kind() == SyntaxKind.DiscardDesignation);

            if (discardPatterns.Any())
            {
                root = root.ReplaceNodes(discardPatterns, (n1, n2) => {
                    return SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression, SyntaxFactory.Token(SyntaxKind.TrueKeyword));
                });
            }

            foreach (var annotation in annotatedMembers.Keys)
            {
                var annotatedNode = root.GetAnnotatedNodes(annotation).First();
                var name = annotatedMembers[annotation];

                root = root.ReplaceNode(annotatedNode, ((MemberAccessExpressionSyntax)annotatedNode).WithName(SyntaxFactory.IdentifierName(name)).NormalizeWhitespace());
            }

            return root;
        }
    }
}