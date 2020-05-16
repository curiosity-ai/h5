using H5.Contract;
using ICSharpCode.NRefactory.CSharp;
using System.Collections.Generic;

namespace H5.Translator
{
    public class ExpressionBlock : AbstractEmitterBlock
    {
        public ExpressionBlock(IEmitter emitter, ExpressionStatement expressionStatement)
            : base(emitter, expressionStatement)
        {
            Emitter = emitter;
            ExpressionStatement = expressionStatement;
        }

        public ExpressionStatement ExpressionStatement { get; set; }

        protected override void DoEmit()
        {
            if (ExpressionStatement.IsNull)
            {
                return;
            }

            var oldSemiColon = Emitter.EnableSemicolon;

            if (Emitter.IsAsync)
            {
                var awaitSearch = new AwaitSearchVisitor(Emitter);
                ExpressionStatement.Expression.AcceptVisitor(awaitSearch);
            }

            bool isAwaiter = ExpressionStatement.Expression is UnaryOperatorExpression && ((UnaryOperatorExpression)ExpressionStatement.Expression).Operator == UnaryOperatorType.Await;

            ExpressionStatement.Expression.AcceptVisitor(Emitter);

            if (Emitter.EnableSemicolon && !isAwaiter)
            {
                WriteSemiColon(true);
            }

            if (oldSemiColon)
            {
                Emitter.EnableSemicolon = true;
            }
        }
    }
}