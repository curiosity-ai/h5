using System.Collections.Generic;
using System.Linq;
using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;

namespace H5.Translator
{
    public class ConditionalBlock : ConversionBlock
    {
        public ConditionalBlock(IEmitter emitter, ConditionalExpression conditionalExpression)
            : base(emitter, conditionalExpression)
        {
            Emitter = emitter;
            ConditionalExpression = conditionalExpression;
        }

        public ConditionalExpression ConditionalExpression { get; set; }

        public List<IAsyncStep> EmittedAsyncSteps { get; set; }

        protected override Expression GetExpression()
        {
            return ConditionalExpression;
        }

        protected override void EmitConversionExpression()
        {
            var conditionalExpression = ConditionalExpression;

            if (Emitter.IsAsync && GetAwaiters(ConditionalExpression).Length > 0)
            {
                if (Emitter.AsyncBlock.WrittenAwaitExpressions.Contains(conditionalExpression))
                {
                    var index = System.Array.IndexOf(Emitter.AsyncBlock.AwaitExpressions, conditionalExpression) + 1;
                    Write(JS.Vars.ASYNC_TASK_RESULT + index);
                }
                else
                {
                    var index = System.Array.IndexOf(Emitter.AsyncBlock.AwaitExpressions, ConditionalExpression) + 1;
                    WriteAsyncConditionalExpression(index);
                }
            }
            else
            {
                conditionalExpression.Condition.AcceptVisitor(Emitter);
                Write(" ? ");
                conditionalExpression.TrueExpression.AcceptVisitor(Emitter);
                Write(" : ");
                conditionalExpression.FalseExpression.AcceptVisitor(Emitter);
            }
        }

        internal void WriteAsyncConditionalExpression(int index)
        {
            if (Emitter.AsyncBlock.WrittenAwaitExpressions.Contains(ConditionalExpression))
            {
                return;
            }

            Emitter.AsyncBlock.WrittenAwaitExpressions.Add(ConditionalExpression);

            WriteAwaiters(ConditionalExpression.Condition);

            WriteIf();
            WriteOpenParentheses();

            var oldValue = Emitter.ReplaceAwaiterByVar;
            var oldAsyncExpressionHandling = Emitter.AsyncExpressionHandling;
            Emitter.ReplaceAwaiterByVar = true;
            Emitter.AsyncExpressionHandling = true;
            ConditionalExpression.Condition.AcceptVisitor(Emitter);
            WriteCloseParentheses();
            Emitter.ReplaceAwaiterByVar = oldValue;
            Emitter.AsyncExpressionHandling = oldAsyncExpressionHandling;

            int startCount = 0;
            int elseCount = 0;
            IAsyncStep trueStep = null;
            IAsyncStep elseStep = null;

            startCount = Emitter.AsyncBlock.Steps.Count;

            EmittedAsyncSteps = Emitter.AsyncBlock.EmittedAsyncSteps;
            Emitter.AsyncBlock.EmittedAsyncSteps = new List<IAsyncStep>();

            var taskResultVar = JS.Vars.ASYNC_TASK_RESULT + index;
            if (!Emitter.Locals.ContainsKey(taskResultVar))
            {
                AddLocal(taskResultVar, null, AstType.Null);
            }

            WriteSpace();
            BeginBlock();
            Write($"{JS.Vars.ASYNC_STEP} = {Emitter.AsyncBlock.Step};");
            WriteNewLine();
            Write("continue;");
            var writer = SaveWriter();
            Emitter.AsyncBlock.AddAsyncStep();

            WriteAwaiters(ConditionalExpression.TrueExpression);

            oldValue = Emitter.ReplaceAwaiterByVar;
            oldAsyncExpressionHandling = Emitter.AsyncExpressionHandling;
            Emitter.ReplaceAwaiterByVar = true;
            Emitter.AsyncExpressionHandling = true;
            Write(taskResultVar + " = ");
            ConditionalExpression.TrueExpression.AcceptVisitor(Emitter);
            WriteSemiColon();
            Emitter.ReplaceAwaiterByVar = oldValue;
            Emitter.AsyncExpressionHandling = oldAsyncExpressionHandling;

            if (Emitter.AsyncBlock.Steps.Count > startCount)
            {
                trueStep = Emitter.AsyncBlock.Steps.Last();
            }

            if (RestoreWriter(writer) && !IsOnlyWhitespaceOnPenultimateLine(true))
            {
                WriteNewLine();
            }

            EndBlock();
            WriteSpace();

            elseCount = Emitter.AsyncBlock.Steps.Count;

            WriteSpace();
            WriteElse();
            BeginBlock();

            Write($"{JS.Vars.ASYNC_STEP} = {Emitter.AsyncBlock.Step};");
            WriteNewLine();
            Write("continue;");
            writer = SaveWriter();
            Emitter.AsyncBlock.AddAsyncStep();
            WriteAwaiters(ConditionalExpression.FalseExpression);

            oldValue = Emitter.ReplaceAwaiterByVar;
            oldAsyncExpressionHandling = Emitter.AsyncExpressionHandling;
            Emitter.ReplaceAwaiterByVar = true;
            Emitter.AsyncExpressionHandling = true;
            Write(taskResultVar + " = ");
            ConditionalExpression.FalseExpression.AcceptVisitor(Emitter);
            WriteSemiColon();
            Emitter.ReplaceAwaiterByVar = oldValue;
            Emitter.AsyncExpressionHandling = oldAsyncExpressionHandling;

            if (Emitter.AsyncBlock.Steps.Count > elseCount)
            {
                elseStep = Emitter.AsyncBlock.Steps.Last();
            }

            if (RestoreWriter(writer) && !IsOnlyWhitespaceOnPenultimateLine(true))
            {
                WriteNewLine();
            }

            EndBlock();
            WriteSpace();

            if (Emitter.IsAsync && Emitter.AsyncBlock.Steps.Count > startCount)
            {
                if (Emitter.AsyncBlock.Steps.Count <= elseCount && !AbstractEmitterBlock.IsJumpStatementLast(Emitter.Output.ToString()))
                {
                    WriteNewLine();
                    Write($"{JS.Vars.ASYNC_STEP} = {Emitter.AsyncBlock.Step};");
                    WriteNewLine();
                    Write("continue;");
                }

                var nextStep = Emitter.AsyncBlock.AddAsyncStep();

                if (trueStep != null)
                {
                    trueStep.JumpToStep = nextStep.Step;
                }

                if (elseStep != null)
                {
                    elseStep.JumpToStep = nextStep.Step;
                }
            }
            else if (Emitter.IsAsync)
            {
                WriteNewLine();
            }

            if (Emitter.IsAsync)
            {
                Emitter.AsyncBlock.EmittedAsyncSteps = EmittedAsyncSteps;
            }
        }
    }
}