using ICSharpCode.NRefactory.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using H5.Contract;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;

namespace H5.Translator
{
    public class AwaitSearchVisitor : DepthFirstAstVisitor
    {
        public AwaitSearchVisitor(IEmitter emitter)
        {
            AwaitExpressions = new List<AstNode>();
            InsertPosition = -1;
            Emitter = emitter;
        }

        public IEmitter Emitter { get; set; }

        private List<AstNode> AwaitExpressions { get; set; }

        public List<AstNode> GetAwaitExpressions()
        {
            return AwaitExpressions.ToList();
        }

        private int InsertPosition { get; set; }

        private void Add(AstNode node)
        {
            if (InsertPosition >= 0)
            {
                AwaitExpressions.Insert(InsertPosition++, node);
            }
            else
            {
                AwaitExpressions.Add(node);
            }
        }

        public override void VisitConditionalExpression(ConditionalExpression conditionalExpression)
        {
            var count = AwaitExpressions.Count;
            var idx = InsertPosition;

            base.VisitConditionalExpression(conditionalExpression);

            if (AwaitExpressions.Count > count)
            {
                AwaitExpressions.Insert(idx > -1 ? idx : 0, conditionalExpression);
            }
        }

        public override void VisitBinaryOperatorExpression(BinaryOperatorExpression binaryOperatorExpression)
        {
            if (Emitter != null && binaryOperatorExpression.GetParent<SyntaxTree>() != null)
            {
                if (Emitter.Resolver.ResolveNode(binaryOperatorExpression, Emitter) is OperatorResolveResult rr && rr.Type.IsKnownType(KnownTypeCode.Boolean))
                {
                    var count = AwaitExpressions.Count;
                    var idx = InsertPosition;

                    base.VisitBinaryOperatorExpression(binaryOperatorExpression);

                    if (AwaitExpressions.Count > count && (
                        binaryOperatorExpression.Operator == BinaryOperatorType.BitwiseAnd ||
                        binaryOperatorExpression.Operator == BinaryOperatorType.BitwiseOr ||
                        binaryOperatorExpression.Operator == BinaryOperatorType.ConditionalOr ||
                        binaryOperatorExpression.Operator == BinaryOperatorType.ConditionalAnd))
                    {
                        AwaitExpressions.Insert(idx > -1 ? idx : 0, binaryOperatorExpression);
                    }

                    return;
                }
            }

            base.VisitBinaryOperatorExpression(binaryOperatorExpression);
        }

        public override void VisitLambdaExpression(LambdaExpression lambdaExpression)
        {
        }

        public override void VisitAnonymousMethodExpression(AnonymousMethodExpression anonymousMethodExpression)
        {
        }

        public override void VisitInvocationExpression(InvocationExpression invocationExpression)
        {
            int oldPos = -2;
            if (invocationExpression.Parent is UnaryOperatorExpression uo && uo.Operator == UnaryOperatorType.Await)
            {
                oldPos = InsertPosition;
                InsertPosition = Math.Max(InsertPosition - 1, 0);
            }

            base.VisitInvocationExpression(invocationExpression);

            if (oldPos > -2)
            {
                InsertPosition = oldPos;
            }
        }

        public override void VisitYieldReturnStatement(YieldReturnStatement yieldReturnStatement)
        {
            Add(yieldReturnStatement);

            base.VisitYieldReturnStatement(yieldReturnStatement);
        }

        public override void VisitYieldBreakStatement(YieldBreakStatement yieldBreakStatement)
        {
            Add(yieldBreakStatement);

            base.VisitYieldBreakStatement(yieldBreakStatement);
        }

        public override void VisitUnaryOperatorExpression(UnaryOperatorExpression unaryOperatorExpression)
        {
            if (unaryOperatorExpression.Operator == UnaryOperatorType.Await)
            {
                Add(unaryOperatorExpression.Expression);
            }

            base.VisitUnaryOperatorExpression(unaryOperatorExpression);
        }

        public override void VisitGotoCaseStatement(GotoCaseStatement gotoCaseStatement)
        {
            Add(gotoCaseStatement);
            base.VisitGotoCaseStatement(gotoCaseStatement);
        }

        public override void VisitGotoDefaultStatement(GotoDefaultStatement gotoDefaultStatement)
        {
            Add(gotoDefaultStatement);
            base.VisitGotoDefaultStatement(gotoDefaultStatement);
        }

        public override void VisitGotoStatement(GotoStatement gotoStatement)
        {
            Add(gotoStatement);
            base.VisitGotoStatement(gotoStatement);
        }

        public override void VisitLabelStatement(LabelStatement labelStatement)
        {
            Add(labelStatement);
            base.VisitLabelStatement(labelStatement);
        }
    }

    public class AsyncTryVisitor : DepthFirstAstVisitor
    {
        public AsyncTryVisitor()
        {
        }

        public bool Found { get; set; }

        public override void VisitLambdaExpression(LambdaExpression lambdaExpression)
        {
        }

        public override void VisitAnonymousMethodExpression(AnonymousMethodExpression anonymousMethodExpression)
        {
        }

        public override void VisitUnaryOperatorExpression(UnaryOperatorExpression unaryOperatorExpression)
        {
            if (unaryOperatorExpression.Operator == UnaryOperatorType.Await)
            {
                var tryBlock = unaryOperatorExpression.GetParent<TryCatchStatement>();

                if (tryBlock != null)
                {
                    Found = true;
                }
            }

            base.VisitUnaryOperatorExpression(unaryOperatorExpression);
        }
    }
}