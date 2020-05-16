using ICSharpCode.NRefactory.CSharp;
using System.Collections.Generic;
using H5.Contract;
using ICSharpCode.NRefactory.TypeSystem;

namespace H5.Translator
{
    public class ReferenceArgumentVisitor : DepthFirstAstVisitor
    {
        public ReferenceArgumentVisitor(IEmitter emitter)
        {
            DirectionExpression = new List<Expression>();
            DirectionVariables = new List<IVariable>();
            Emitter = emitter;
        }

        public IEmitter Emitter
        {
            get; set;
        }

        public List<Expression> DirectionExpression { get; set; }

        public List<IVariable> DirectionVariables { get; set; }

        public override void VisitDirectionExpression(DirectionExpression directionExpression)
        {
            DirectionExpression.Add(directionExpression.Expression);
            base.VisitDirectionExpression(directionExpression);
        }

        public override void VisitLambdaExpression(LambdaExpression lambdaExpression)
        {
            var capturedVariables = LambdaBlock.GetCapturedLoopVariables(Emitter, lambdaExpression, lambdaExpression.Parameters, true);

            if (capturedVariables != null)
            {
                DirectionVariables.AddRange(capturedVariables);
            }

            base.VisitLambdaExpression(lambdaExpression);
        }

        public override void VisitAnonymousMethodExpression(AnonymousMethodExpression anonymousMethodExpression)
        {
            var capturedVariables = LambdaBlock.GetCapturedLoopVariables(Emitter, anonymousMethodExpression, anonymousMethodExpression.Parameters, true);

            if (capturedVariables != null)
            {
                DirectionVariables.AddRange(capturedVariables);
            }

            base.VisitAnonymousMethodExpression(anonymousMethodExpression);
        }
    }
}