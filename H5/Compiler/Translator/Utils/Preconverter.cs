using H5.Contract;
using H5.Contract.Constants;
using H5.Translator.Utils;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using Expression = ICSharpCode.NRefactory.CSharp.Expression;
using ExpressionStatement = ICSharpCode.NRefactory.CSharp.ExpressionStatement;
using ParenthesizedExpression = ICSharpCode.NRefactory.CSharp.ParenthesizedExpression;
using Statement = ICSharpCode.NRefactory.CSharp.Statement;
using ICSharpCode.NRefactory.PatternMatching;
using Mono.Cecil;
using System.Text;
using ICSharpCode.NRefactory.TypeSystem.Implementation;
using Mosaik.Core;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace H5.Translator
{
    public class PreconverterDetecter : DepthFirstAstVisitor
    {
        public PreconverterDetecter(MemberResolver resolver, IEmitter emitter)
        {
            Resolver = resolver;
            Emitter = emitter;
        }

        public bool Found { get; set; }

        public MemberResolver Resolver { get; set; }

        internal IEmitter Emitter
        {
            get; private set;
        }
        internal CompilerRule Rules
        {
            get; private set;
        }

        public override void VisitUnaryOperatorExpression(UnaryOperatorExpression unaryOperatorExpression)
        {
            if (Found)
            {
                return;
            }

            if (Rules.Integer == IntegerRule.Managed && (unaryOperatorExpression.Operator == UnaryOperatorType.Increment ||
                unaryOperatorExpression.Operator == UnaryOperatorType.PostIncrement ||
                unaryOperatorExpression.Operator == UnaryOperatorType.Decrement ||
                unaryOperatorExpression.Operator == UnaryOperatorType.PostDecrement))
            {
                var rr = Resolver.ResolveNode(unaryOperatorExpression);

                if (rr is ErrorResolveResult)
                {
                    Found = true;
                }
                else
                {
                    if (rr is OperatorResolveResult orr && !Helpers.IsFloatType(orr.Type, Resolver) && !Helpers.Is64Type(orr.Type, Resolver))
                    {
                        Found = true;
                    }
                }
            }

            base.VisitUnaryOperatorExpression(unaryOperatorExpression);
        }

        public override void VisitAssignmentExpression(AssignmentExpression assignmentExpression)
        {
            if (Found)
            {
                return;
            }

            if (assignmentExpression.Operator != AssignmentOperatorType.Any && assignmentExpression.Operator != AssignmentOperatorType.Assign)
            {
                var rr = Resolver.ResolveNode(assignmentExpression);
                var isInt = Helpers.IsIntegerType(rr.Type, Resolver);
                if (Rules.Integer == IntegerRule.Managed && isInt || !(assignmentExpression.Parent is ExpressionStatement))
                {
                    Found = true;
                }

                if (assignmentExpression.Operator == AssignmentOperatorType.Add && rr.Type.IsKnownType(KnownTypeCode.String))
                {
                    Found = true;
                }

                if (Found && !isInt && assignmentExpression.Parent is LambdaExpression)
                {
                    if (Resolver.ResolveNode(assignmentExpression.Parent) is LambdaResolveResult lambdarr && lambdarr.ReturnType.Kind == TypeKind.Void)
                    {
                        Found = false;
                    }
                }
            }

            base.VisitAssignmentExpression(assignmentExpression);
        }

        public override void VisitMethodDeclaration(MethodDeclaration methodDeclaration)
        {
            if (Found)
            {
                return;
            }

            if (Resolver.ResolveNode(methodDeclaration) is MemberResolveResult rr)
            {
                Rules = Contract.Rules.Get(Emitter, rr.Member);
            }
            else
            {
                Rules = Contract.Rules.Default;
            }

            base.VisitMethodDeclaration(methodDeclaration);
        }

        public override void VisitPropertyDeclaration(PropertyDeclaration propertyDeclaration)
        {
            if (Found)
            {
                return;
            }

            if (Resolver.ResolveNode(propertyDeclaration) is MemberResolveResult rr)
            {
                Rules = Contract.Rules.Get(Emitter, rr.Member);
            }
            else
            {
                Rules = Contract.Rules.Default;
            }

            base.VisitPropertyDeclaration(propertyDeclaration);
        }

        public override void VisitIndexerDeclaration(IndexerDeclaration indexerDeclaration)
        {
            if (Found)
            {
                return;
            }

            if (Resolver.ResolveNode(indexerDeclaration) is MemberResolveResult rr)
            {
                Rules = Contract.Rules.Get(Emitter, rr.Member);
            }
            else
            {
                Rules = Contract.Rules.Default;
            }

            base.VisitIndexerDeclaration(indexerDeclaration);
        }

        public override void VisitCustomEventDeclaration(CustomEventDeclaration eventDeclaration)
        {
            if (Found)
            {
                return;
            }

            if (Resolver.ResolveNode(eventDeclaration) is MemberResolveResult rr)
            {
                Rules = Contract.Rules.Get(Emitter, rr.Member);
            }
            else
            {
                Rules = Contract.Rules.Default;
            }

            base.VisitCustomEventDeclaration(eventDeclaration);
        }

        public override void VisitTypeDeclaration(TypeDeclaration typeDeclaration)
        {
            if (Found)
            {
                return;
            }

            if (Resolver.ResolveNode(typeDeclaration) is TypeResolveResult rr)
            {
                Rules = Contract.Rules.Get(Emitter, rr.Type.GetDefinition());
            }
            else
            {
                Rules = Contract.Rules.Default;
            }

            base.VisitTypeDeclaration(typeDeclaration);
        }

        public override void VisitInvocationExpression(InvocationExpression invocationExpression)
        {
            if (Found)
            {
                return;
            }


            if (Resolver.ResolveNode(invocationExpression) is InvocationResolveResult rr && rr.IsError)
            {
                Found = true;
            }

            base.VisitInvocationExpression(invocationExpression);
        }

        public override void VisitUsingStatement(UsingStatement usingStatement)
        {
            if (Found)
            {
                return;
            }

            var awaitSearch = new AwaitSearchVisitor(null);
            usingStatement.AcceptVisitor(awaitSearch);

            var awaiters = awaitSearch.GetAwaitExpressions().ToArray();

            if (awaiters.Length > 0)
            {
                Found = true;
            }

            base.VisitUsingStatement(usingStatement);
        }
    }

    public class PreconverterFixer : DepthFirstAstVisitor<AstNode>
    {
        private static ILogger Logger = ApplicationLogging.CreateLogger<PreconverterFixer>();

        public PreconverterFixer(MemberResolver resolver, IEmitter emitter)
        {
            Resolver = resolver;
            Emitter = emitter;
        }

        public MemberResolver Resolver { get; set; }

        internal IEmitter Emitter
        {
            get; private set;
        }
        internal CompilerRule Rules
        {
            get; private set;
        }

        public override AstNode VisitMethodDeclaration(MethodDeclaration methodDeclaration)
        {
            if (Resolver.ResolveNode(methodDeclaration) is MemberResolveResult rr)
            {
                Rules = Contract.Rules.Get(Emitter, rr.Member);
            }
            else
            {
                Rules = Contract.Rules.Default;
            }

            return base.VisitMethodDeclaration(methodDeclaration);
        }

        public override AstNode VisitPropertyDeclaration(PropertyDeclaration propertyDeclaration)
        {
            if (Resolver.ResolveNode(propertyDeclaration) is MemberResolveResult rr)
            {
                Rules = Contract.Rules.Get(Emitter, rr.Member);
            }
            else
            {
                Rules = Contract.Rules.Default;
            }

            return base.VisitPropertyDeclaration(propertyDeclaration);
        }

        public override AstNode VisitIndexerDeclaration(IndexerDeclaration indexerDeclaration)
        {
            if (Resolver.ResolveNode(indexerDeclaration) is MemberResolveResult rr)
            {
                Rules = Contract.Rules.Get(Emitter, rr.Member);
            }
            else
            {
                Rules = Contract.Rules.Default;
            }

            return base.VisitIndexerDeclaration(indexerDeclaration);
        }

        public override AstNode VisitCustomEventDeclaration(CustomEventDeclaration eventDeclaration)
        {
            if (Resolver.ResolveNode(eventDeclaration) is MemberResolveResult rr)
            {
                Rules = Contract.Rules.Get(Emitter, rr.Member);
            }
            else
            {
                Rules = Contract.Rules.Default;
            }

            return base.VisitCustomEventDeclaration(eventDeclaration);
        }

        public override AstNode VisitTypeDeclaration(TypeDeclaration typeDeclaration)
        {
            if (Resolver.ResolveNode(typeDeclaration) is TypeResolveResult rr)
            {
                Rules = Contract.Rules.Get(Emitter, rr.Type.GetDefinition());
            }
            else
            {
                Rules = Contract.Rules.Default;
            }

            return base.VisitTypeDeclaration(typeDeclaration);
        }

        protected override AstNode VisitChildren(AstNode node)
        {
            List<AstNode> newChildren = null;

            int i = 0;
            foreach (var child in node.Children)
            {
                var newChild = child.AcceptVisitor(this);
                if (newChild != null)
                {
                    newChildren = newChildren ?? Enumerable.Repeat((AstNode)null, i).ToList();
                    newChildren.Add(newChild);
                }
                else if (newChildren != null)
                {
                    newChildren.Add(null);
                }
                i++;
            }

            if (newChildren == null)
            {
                return null;
            }

            var result = node.Clone();

            i = 0;
            foreach (var children in result.Children)
            {
                if (newChildren[i] != null)
                    children.ReplaceWith(newChildren[i]);
                i++;
            }

            return result;
        }

        public override AstNode VisitUsingStatement(UsingStatement usingStatement)
        {
            var awaitSearch = new AwaitSearchVisitor(null);
            usingStatement.AcceptVisitor(awaitSearch);

            var awaiters = awaitSearch.GetAwaitExpressions().ToArray();
            UsingStatement node = (UsingStatement)base.VisitUsingStatement(usingStatement) ?? (UsingStatement)usingStatement.Clone();
            if (awaiters.Length > 0)
            {
                IEnumerable<AstNode> inner = null;

                var res = node.ResourceAcquisition;
                var varStat = res as VariableDeclarationStatement;
                if (varStat != null)
                {
                    inner = varStat.Variables.Skip(1);
                    res = varStat.Variables.First();
                }

                return EmitUsing(node, res, inner, varStat);
            }

            return node;
        }

        private static int counter = 0;

        protected virtual string GetTempVarName()
        {
            return "_h5Tmp_" + ++counter;
        }

        protected virtual Statement EmitUsing(UsingStatement usingStatement, AstNode expression, IEnumerable<AstNode> inner, VariableDeclarationStatement varStat)
        {
            string name = null;
            BlockStatement wrapper = null;

            if (expression is VariableInitializer varInit)
            {
                name = varInit.Name;
                wrapper = new BlockStatement();
                wrapper.Statements.Add(new VariableDeclarationStatement(varStat != null ? varStat.Type.Clone() : AstType.Null, varInit.Name, varInit.Initializer.Clone()));
            }
            else if (expression is IdentifierExpression)
            {
                name = ((IdentifierExpression)expression).Identifier;
            }
            else
            {
                name = GetTempVarName();
                wrapper = new BlockStatement();
                wrapper.Statements.Add(new VariableDeclarationStatement(varStat != null ? varStat.Type.Clone() : AstType.Null, name, expression.Clone() as Expression));
            }

            var tryCatchStatement = new TryCatchStatement();
            if (wrapper != null)
            {
                wrapper.Statements.Add(tryCatchStatement);
            }

            if (inner != null && inner.Any())
            {
                var block = new BlockStatement();
                block.Statements.Add(EmitUsing(usingStatement, inner.First(), inner.Skip(1), varStat));
                tryCatchStatement.TryBlock = block;
            }
            else
            {
                if (!(usingStatement.EmbeddedStatement is BlockStatement block))
                {
                    block = new BlockStatement();
                    block.Add(usingStatement.EmbeddedStatement.Clone());
                }
                else
                {
                    block = (BlockStatement)block.Clone();
                }

                tryCatchStatement.TryBlock = block;
            }

            var finallyBlock = new BlockStatement();
            var dispose = new InvocationExpression(
                new MemberReferenceExpression(
                    new MemberReferenceExpression(new TypeReferenceExpression(new MemberType(new SimpleType("global"), CS.NS.H5) { IsDoubleColon = true }), "Script"), "Write"),
                new PrimitiveExpression(
                    string.Format("if (" + JS.Funcs.H5_HASVALUE + "({0})) {0}." + JS.Funcs.DISPOSE + "();", name))
            );

            finallyBlock.Statements.Add(dispose);

            tryCatchStatement.FinallyBlock = finallyBlock;
            return wrapper ?? (Statement)tryCatchStatement;
        }

        public override AstNode VisitInvocationExpression(InvocationExpression invocationExpression)
        {
            try
            {
                if (Resolver.ResolveNode(invocationExpression) is CSharpInvocationResolveResult rr && rr.IsError)
                {
                    InvocationExpression clonInvocationExpression = (InvocationExpression)base.VisitInvocationExpression(invocationExpression);
                    if (clonInvocationExpression == null)
                    {
                        clonInvocationExpression = (InvocationExpression)invocationExpression.Clone();
                    }

                    var map = rr.GetArgumentToParameterMap();
                    var orig = clonInvocationExpression.Arguments.ToArray();
                    var result = clonInvocationExpression.Arguments.ToArray();

                    if (rr.IsExtensionMethodInvocation)
                    {
                        for (int i = 1; i < map.Count; i++)
                        {
                            if (map[i] < 0)
                            {
                                continue;
                            }

                            result[i - 1] = orig[map[i] - 1];
                        }
                    }
                    else
                    {
                        for (int i = 0; i < map.Count; i++)
                        {
                            if (map[i] < 0)
                            {
                                continue;
                            }

                            result[i] = orig[map[i]];
                        }
                    }

                    clonInvocationExpression.Arguments.ReplaceWith(result);
                    return clonInvocationExpression;
                }
            }
            catch(Exception e)
            {
                var fileName = invocationExpression.GetParent<SyntaxTree>()?.FileName;

                Logger.ZLogWarning("VisitInvocationExpression fails in {0} ({1}): {2}", fileName, invocationExpression.StartLocation.ToString(), e.Message);
            }

            return base.VisitInvocationExpression(invocationExpression);
        }

        public override AstNode VisitUnaryOperatorExpression(UnaryOperatorExpression unaryOperatorExpression)
        {
            if (Rules.Integer == IntegerRule.Managed && (unaryOperatorExpression.Operator == UnaryOperatorType.Increment ||
                unaryOperatorExpression.Operator == UnaryOperatorType.PostIncrement ||
                unaryOperatorExpression.Operator == UnaryOperatorType.Decrement ||
                unaryOperatorExpression.Operator == UnaryOperatorType.PostDecrement))
            {
                var rr = Resolver.ResolveNode(unaryOperatorExpression);
                var expression_rr = Resolver.ResolveNode(unaryOperatorExpression.Expression);

                if (rr is ErrorResolveResult)
                {
                    UnaryOperatorExpression clonUnaryOperatorExpression = (UnaryOperatorExpression)base.VisitUnaryOperatorExpression(unaryOperatorExpression);
                    if (clonUnaryOperatorExpression == null)
                    {
                        clonUnaryOperatorExpression = (UnaryOperatorExpression)unaryOperatorExpression.Clone();
                    }

                    bool isPost = clonUnaryOperatorExpression.Operator == UnaryOperatorType.PostDecrement ||
                                  clonUnaryOperatorExpression.Operator == UnaryOperatorType.PostIncrement;

                    var isStatement = unaryOperatorExpression.Parent is ExpressionStatement;
                    var isIncr = clonUnaryOperatorExpression.Operator == UnaryOperatorType.Increment || clonUnaryOperatorExpression.Operator == UnaryOperatorType.PostIncrement;
                    AssignmentExpression ae;

                    ae = new AssignmentExpression(clonUnaryOperatorExpression.Expression.Clone(), new BinaryOperatorExpression(clonUnaryOperatorExpression.Expression.Clone(), isIncr ? BinaryOperatorType.Add : BinaryOperatorType.Subtract, new PrimitiveExpression(1)));

                    if (isPost && !isStatement)
                    {
                        return new InvocationExpression(new MemberReferenceExpression(new MemberReferenceExpression(new TypeReferenceExpression(new MemberType(new SimpleType("global"), CS.NS.H5) { IsDoubleColon = true }), "Script"), "Identity"), clonUnaryOperatorExpression.Expression.Clone(), ae);
                    }
                    else
                    {
                        if (isStatement)
                        {
                            return ae;
                        }

                        return new ParenthesizedExpression(ae);
                    }
                }
                else
                {
                    var orr = (OperatorResolveResult)rr;

                    if (Helpers.IsFloatType(orr.Type, Resolver) || Helpers.Is64Type(orr.Type, Resolver))
                    {
                        return base.VisitUnaryOperatorExpression(unaryOperatorExpression);
                    }

                    UnaryOperatorExpression clonUnaryOperatorExpression = (UnaryOperatorExpression)base.VisitUnaryOperatorExpression(unaryOperatorExpression);
                    if (clonUnaryOperatorExpression == null)
                    {
                        clonUnaryOperatorExpression = (UnaryOperatorExpression)unaryOperatorExpression.Clone();
                    }

                    bool isPost = clonUnaryOperatorExpression.Operator == UnaryOperatorType.PostDecrement ||
                                  clonUnaryOperatorExpression.Operator == UnaryOperatorType.PostIncrement;

                    var isStatement = unaryOperatorExpression.Parent is ExpressionStatement;
                    var isIncr = clonUnaryOperatorExpression.Operator == UnaryOperatorType.Increment || clonUnaryOperatorExpression.Operator == UnaryOperatorType.PostIncrement;
                    var needReturnOriginal = isPost && !isStatement;

                    Expression expression = clonUnaryOperatorExpression.Expression.Clone();
                    Expression expressionAfter = clonUnaryOperatorExpression.Expression.Clone();

                    AssignmentExpression ae = null;

                    if (orr.UserDefinedOperatorMethod != null)
                    {
                        ae = new AssignmentExpression(expressionAfter.Clone(), clonUnaryOperatorExpression);
                    }
                    else if (clonUnaryOperatorExpression.Expression is MemberReferenceExpression mre && expression_rr is MemberResolveResult member_rr)
                    {
                        if (needReturnOriginal)
                        {
                            bool isSimple = (member_rr != null &&
                               (member_rr.TargetResult is ThisResolveResult ||
                                member_rr.TargetResult is LocalResolveResult ||
                                member_rr.TargetResult is TypeResolveResult ||
                                member_rr.TargetResult is ConstantResolveResult));

                            expression = isSimple ? mre.Clone() : new MemberReferenceExpression(new InvocationExpression(new MemberReferenceExpression(new MemberReferenceExpression(new TypeReferenceExpression(new MemberType(new SimpleType("global"), CS.NS.H5) { IsDoubleColon = true }), "Script"), "ToTemp"), new PrimitiveExpression("idx" + tempKey), mre.Target.Clone()), mre.MemberName);
                            expressionAfter = new InvocationExpression(new MemberReferenceExpression(new MemberReferenceExpression(new TypeReferenceExpression(new MemberType(new SimpleType("global"), CS.NS.H5) { IsDoubleColon = true }), "Script"), "FromTemp"), new PrimitiveExpression("idx" + tempKey++), mre.Target.Clone());
                        } else
                        {
                            expressionAfter = null;
                        }

                        ae = BuildMemberReferenceReplacement(isIncr ? BinaryOperatorType.Add : BinaryOperatorType.Subtract, mre, member_rr, expressionAfter);
                    }
                    else if (clonUnaryOperatorExpression.Expression is IndexerExpression ie && expression_rr is ArrayAccessResolveResult array_rr)
                    {
                        IndexerExpression cacheIndexer = null;
                        if (needReturnOriginal)
                        {
                            var array_target_rr = array_rr.Array as MemberResolveResult;
                            bool isSimpleTarget = (array_target_rr != null && array_target_rr.Member is IField &&
                               (array_target_rr.TargetResult is ThisResolveResult ||
                                array_target_rr.TargetResult is LocalResolveResult ||
                                array_target_rr.TargetResult is TypeResolveResult ||
                                array_target_rr.TargetResult is ConstantResolveResult)) ||
                                (array_rr.Array is ThisResolveResult ||
                                array_rr.Array is LocalResolveResult ||
                                array_rr.Array is ConstantResolveResult);

                            bool simpleIndex = true;

                            foreach (var index in array_rr.Indexes)
                            {
                                var indexMemberTargetrr = index as MemberResolveResult;
                                bool isIndexSimple = (indexMemberTargetrr != null && indexMemberTargetrr.Member is IField &&
                                               (indexMemberTargetrr.TargetResult is ThisResolveResult ||
                                                indexMemberTargetrr.TargetResult is LocalResolveResult)) || index is ThisResolveResult || index is LocalResolveResult || index is ConstantResolveResult;

                                if (!isIndexSimple)
                                {
                                    simpleIndex = false;
                                    break;
                                }
                            }
                            var leftIndexerArgs = new List<Expression>();
                            var rightIndexerArgs = new List<Expression>();

                            foreach (var index in ie.Arguments)
                            {
                                var expr = simpleIndex ? index.Clone() : new InvocationExpression(new MemberReferenceExpression(new MemberReferenceExpression(new TypeReferenceExpression(new MemberType(new SimpleType("global"), CS.NS.H5) { IsDoubleColon = true }), "Script"), "ToTemp"), new PrimitiveExpression("idx" + tempKey), index.Clone());
                                leftIndexerArgs.Add(expr);

                                expr = simpleIndex ? index.Clone() : new InvocationExpression(new MemberReferenceExpression(new MemberReferenceExpression(new TypeReferenceExpression(new MemberType(new SimpleType("global"), CS.NS.H5) { IsDoubleColon = true }), "Script"), "FromTemp"), new PrimitiveExpression("idx" + tempKey++), index.Clone());
                                rightIndexerArgs.Add(expr);
                            }

                            var leftExpr = new InvocationExpression(new MemberReferenceExpression(new MemberReferenceExpression(new TypeReferenceExpression(new MemberType(new SimpleType("global"), CS.NS.H5) { IsDoubleColon = true }), "Script"), "ToTemp"), new PrimitiveExpression("idx" + tempKey), ie.Target.Clone());
                            var rightExpr = new InvocationExpression(new MemberReferenceExpression(new MemberReferenceExpression(new TypeReferenceExpression(new MemberType(new SimpleType("global"), CS.NS.H5) { IsDoubleColon = true }), "Script"), "FromTemp"), new PrimitiveExpression("idx" + tempKey++), ie.Target.Clone());

                            var leftIndexer = new IndexerExpression(isSimpleTarget ? ie.Target.Clone() : leftExpr, leftIndexerArgs);
                            var rightIndexer = new IndexerExpression(isSimpleTarget ? ie.Target.Clone() : rightExpr, rightIndexerArgs);

                            expression = leftIndexer;
                            cacheIndexer = rightIndexer;
                        }

                        ae = BuildIndexerReplacement(isStatement, isIncr ? BinaryOperatorType.Add : BinaryOperatorType.Subtract, ie, array_rr, cacheIndexer);
                    }
                    else
                    {
                        ae = new AssignmentExpression(expressionAfter.Clone(),
                                 new BinaryOperatorExpression(expressionAfter.Clone(), isIncr ? BinaryOperatorType.Add : BinaryOperatorType.Subtract, new PrimitiveExpression(1)));
                    }

                    if (needReturnOriginal)
                    {
                        return new InvocationExpression(new MemberReferenceExpression(new MemberReferenceExpression(new TypeReferenceExpression(new MemberType(new SimpleType("global"), CS.NS.H5) { IsDoubleColon = true }), "Script"), "Identity"), expression, new ParenthesizedExpression(ae));
                    }
                    else
                    {
                        if (isStatement)
                        {
                            return ae;
                        }

                        return new ParenthesizedExpression(ae);
                    }
                }
            }

            return base.VisitUnaryOperatorExpression(unaryOperatorExpression);
        }

        private AssignmentExpression BuildMemberReferenceReplacement(BinaryOperatorType opType, MemberReferenceExpression memberExpr, MemberResolveResult member_rr, Expression target, Expression addExpr = null)
        {
            bool isSimple = (member_rr != null &&
                           (member_rr.TargetResult is ThisResolveResult ||
                            member_rr.TargetResult is LocalResolveResult ||
                            member_rr.TargetResult is TypeResolveResult ||
                            member_rr.TargetResult is ConstantResolveResult));

            if (isSimple)
            {
                target = memberExpr.Target.Clone();
            }

            var leftExpr = target != null ? target.Clone() : new InvocationExpression(new MemberReferenceExpression(new MemberReferenceExpression(new TypeReferenceExpression(new MemberType(new SimpleType("global"), CS.NS.H5) { IsDoubleColon = true }), "Script"), "ToTemp"), new PrimitiveExpression("idx" + tempKey), memberExpr.Target.Clone());
            var rightExpr = target != null ? target.Clone() : new InvocationExpression(new MemberReferenceExpression(new MemberReferenceExpression(new TypeReferenceExpression(new MemberType(new SimpleType("global"), CS.NS.H5) { IsDoubleColon = true }), "Script"), "FromTemp"), new PrimitiveExpression("idx" + tempKey++), memberExpr.Target.Clone());

            var ae = new AssignmentExpression(new MemberReferenceExpression(leftExpr, memberExpr.MemberName),
                                 new BinaryOperatorExpression(new MemberReferenceExpression(rightExpr, memberExpr.MemberName),
                                 opType,
                                 addExpr ?? new PrimitiveExpression(1)));

            return ae;
        }

        private AssignmentExpression BuildIndexerReplacement(bool isStatement, BinaryOperatorType opType, IndexerExpression indexerExpr, ArrayAccessResolveResult array_rr, IndexerExpression cacheIndexer, Expression addExpr = null)
        {
            var array_target_rr = array_rr.Array as MemberResolveResult;

            bool isSimpleTarget = (array_target_rr != null && array_target_rr.Member is IField &&
                               (array_target_rr.TargetResult is ThisResolveResult ||
                                array_target_rr.TargetResult is LocalResolveResult ||
                                array_target_rr.TargetResult is TypeResolveResult ||
                                array_target_rr.TargetResult is ConstantResolveResult)) ||
                                (array_rr.Array is ThisResolveResult ||
                                array_rr.Array is LocalResolveResult ||
                                array_rr.Array is ConstantResolveResult);

            bool simpleIndex = true;

            foreach (var index in array_rr.Indexes)
            {
                var indexMemberTargetrr = index as MemberResolveResult;
                bool isIndexSimple = (indexMemberTargetrr != null && indexMemberTargetrr.Member is IField &&
                               (indexMemberTargetrr.TargetResult is ThisResolveResult ||
                                indexMemberTargetrr.TargetResult is LocalResolveResult)) || index is ThisResolveResult || index is LocalResolveResult || index is ConstantResolveResult;

                if (!isIndexSimple)
                {
                    simpleIndex = false;
                    break;
                }
            }

            var leftIndexerArgs = new List<Expression>();
            var rightIndexerArgs = new List<Expression>();

            foreach (var index in indexerExpr.Arguments)
            {
                var expr = simpleIndex ? index.Clone() : new InvocationExpression(new MemberReferenceExpression(new MemberReferenceExpression(new TypeReferenceExpression(new MemberType(new SimpleType("global"), CS.NS.H5) { IsDoubleColon = true }), "Script"), "ToTemp"), new PrimitiveExpression("idx" + tempKey), index.Clone());
                leftIndexerArgs.Add(expr);

                expr = simpleIndex ? index.Clone() : new InvocationExpression(new MemberReferenceExpression(new MemberReferenceExpression(new TypeReferenceExpression(new MemberType(new SimpleType("global"), CS.NS.H5) { IsDoubleColon = true }), "Script"), "FromTemp"), new PrimitiveExpression("idx" + tempKey++), index.Clone());
                rightIndexerArgs.Add(expr);
            }

            var leftExpr = new InvocationExpression(new MemberReferenceExpression(new MemberReferenceExpression(new TypeReferenceExpression(new MemberType(new SimpleType("global"), CS.NS.H5) { IsDoubleColon = true }), "Script"), "ToTemp"), new PrimitiveExpression("idx" + tempKey), indexerExpr.Target.Clone());
            var rightExpr = new InvocationExpression(new MemberReferenceExpression(new MemberReferenceExpression(new TypeReferenceExpression(new MemberType(new SimpleType("global"), CS.NS.H5) { IsDoubleColon = true }), "Script"), "FromTemp"), new PrimitiveExpression("idx" + tempKey++), indexerExpr.Target.Clone());

            var leftIndexer = cacheIndexer != null ? cacheIndexer.Clone() : new IndexerExpression(isSimpleTarget ? indexerExpr.Target.Clone() : leftExpr, leftIndexerArgs);
            var rightIndexer = cacheIndexer != null ? cacheIndexer.Clone() : new IndexerExpression(isSimpleTarget ? indexerExpr.Target.Clone() : rightExpr, rightIndexerArgs);

            var wrapRightExpression = (addExpr == null || AssigmentExpressionHelper.CheckIsExpression(addExpr))
                ? addExpr ?? new PrimitiveExpression(1)
                : new ParenthesizedExpression(addExpr ?? new PrimitiveExpression(1));

            var ae = new AssignmentExpression(leftIndexer, new BinaryOperatorExpression(
                rightIndexer,
                opType,
                wrapRightExpression));

            return ae;
        }

        private int tempKey = 1;

        public override AstNode VisitAssignmentExpression(AssignmentExpression assignmentExpression)
        {
            var rr = Resolver.ResolveNode(assignmentExpression);
            var orr = rr as OperatorResolveResult;
            var simpleIndex = true;

            if (orr != null)
            {
                if (orr.Operands[0] is ArrayAccessResolveResult accessrr)
                {
                    foreach (var index in accessrr.Indexes)
                    {
                        var indexMemberTargetrr = index as MemberResolveResult;
                        bool isIndexSimple = (indexMemberTargetrr != null && indexMemberTargetrr.Member is IField &&
                                       (indexMemberTargetrr.TargetResult is ThisResolveResult ||
                                        indexMemberTargetrr.TargetResult is LocalResolveResult)) || index is ThisResolveResult || index is LocalResolveResult || index is ConstantResolveResult;


                        if (!isIndexSimple)
                        {
                            simpleIndex = false;
                        }
                    }
                }
            }

            bool found = false;
            var isInt = Helpers.IsIntegerType(rr.Type, Resolver);
            if (Rules.Integer == IntegerRule.Managed && isInt || !(assignmentExpression.Parent is ExpressionStatement))
            {
                found = true;
            }

            if (found && !isInt && assignmentExpression.Parent is LambdaExpression)
            {
                if (Resolver.ResolveNode(assignmentExpression.Parent) is LambdaResolveResult lambdarr && lambdarr.ReturnType.Kind == TypeKind.Void)
                {
                    found = false;
                }
            }

            if (assignmentExpression.Operator == AssignmentOperatorType.Add && rr.Type.IsKnownType(KnownTypeCode.String))
            {
                found = true;
            }

            if (assignmentExpression.Operator != AssignmentOperatorType.Any &&
                assignmentExpression.Operator != AssignmentOperatorType.Assign &&
                found)
            {
                AssignmentExpression clonAssignmentExpression = (AssignmentExpression)base.VisitAssignmentExpression(assignmentExpression);
                if (clonAssignmentExpression == null)
                {
                    clonAssignmentExpression = (AssignmentExpression)assignmentExpression.Clone();
                }

                List<Expression> leftIndexerArgs = null;
                List<Expression> rightIndexerArgs = null;
                var left = clonAssignmentExpression.Left;

                if (clonAssignmentExpression.Left is IndexerExpression indexerExpr && !simpleIndex)
                {
                    leftIndexerArgs = new List<Expression>();
                    rightIndexerArgs = new List<Expression>();

                    foreach (var index in indexerExpr.Arguments)
                    {
                        var expr = new InvocationExpression(new MemberReferenceExpression(new MemberReferenceExpression(new TypeReferenceExpression(new MemberType(new SimpleType("global"), CS.NS.H5) { IsDoubleColon = true }), "Script"), "ToTemp"), new PrimitiveExpression("idx" + tempKey), index.Clone());
                        leftIndexerArgs.Add(expr);

                        expr = new InvocationExpression(new MemberReferenceExpression(new MemberReferenceExpression(new TypeReferenceExpression(new MemberType(new SimpleType("global"), CS.NS.H5) { IsDoubleColon = true }), "Script"), "FromTemp"), new PrimitiveExpression("idx" + tempKey++), index.Clone());
                        rightIndexerArgs.Add(expr);
                    }

                    clonAssignmentExpression.Left = new IndexerExpression(indexerExpr.Target.Clone(), leftIndexerArgs);
                }
                else
                {
                    indexerExpr = null;
                }

                var op = clonAssignmentExpression.Operator;
                clonAssignmentExpression.Operator = AssignmentOperatorType.Assign;
                BinaryOperatorType opType = ToBinaryOp(op);

                var wrapRightExpression = AssigmentExpressionHelper.CheckIsRightAssigmentExpression(clonAssignmentExpression)
                    ? clonAssignmentExpression.Right.Clone()
                    : new ParenthesizedExpression(clonAssignmentExpression.Right.Clone());

                var isStatement = assignmentExpression.Parent is ExpressionStatement;

                if (left is MemberReferenceExpression mre && orr?.Operands[0] is MemberResolveResult member_rr)
                {
                    return BuildMemberReferenceReplacement(opType, mre, member_rr, null, clonAssignmentExpression.Right.Clone());
                }
                else if (left is IndexerExpression ie && orr?.Operands[0] is ArrayAccessResolveResult array_rr)
                {
                    return BuildIndexerReplacement(isStatement, opType, ie, array_rr, null, clonAssignmentExpression.Right.Clone());
                }

                clonAssignmentExpression.Right = new BinaryOperatorExpression(
                    indexerExpr != null ? new IndexerExpression(indexerExpr.Target.Clone(), rightIndexerArgs) : clonAssignmentExpression.Left.Clone(),
                    opType,
                    wrapRightExpression);

                return clonAssignmentExpression;
            }

            return base.VisitAssignmentExpression(assignmentExpression);
        }

        private static BinaryOperatorType ToBinaryOp(AssignmentOperatorType op)
        {
            BinaryOperatorType opType;
            switch (op)
            {
                case AssignmentOperatorType.Add:
                    opType = BinaryOperatorType.Add;
                    break;

                case AssignmentOperatorType.Subtract:
                    opType = BinaryOperatorType.Subtract;
                    break;

                case AssignmentOperatorType.Multiply:
                    opType = BinaryOperatorType.Multiply;
                    break;

                case AssignmentOperatorType.Divide:
                    opType = BinaryOperatorType.Divide;
                    break;

                case AssignmentOperatorType.Modulus:
                    opType = BinaryOperatorType.Modulus;
                    break;

                case AssignmentOperatorType.ShiftLeft:
                    opType = BinaryOperatorType.ShiftLeft;
                    break;

                case AssignmentOperatorType.ShiftRight:
                    opType = BinaryOperatorType.ShiftRight;
                    break;

                case AssignmentOperatorType.BitwiseAnd:
                    opType = BinaryOperatorType.BitwiseAnd;
                    break;

                case AssignmentOperatorType.BitwiseOr:
                    opType = BinaryOperatorType.BitwiseOr;
                    break;

                case AssignmentOperatorType.ExclusiveOr:
                    opType = BinaryOperatorType.ExclusiveOr;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return opType;
        }
    }
}