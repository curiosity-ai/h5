using H5.Contract;
using H5.Contract.Constants;

using ICSharpCode.NRefactory.CSharp;

using System.Collections.Generic;
using System.Linq;

namespace H5.Translator
{
    public class IfElseBlock : AbstractEmitterBlock
    {
        public IfElseBlock(IEmitter emitter, IfElseStatement ifElseStatement)
            : base(emitter, ifElseStatement)
        {
            Emitter = emitter;
            IfElseStatement = ifElseStatement;
        }

        public IfElseStatement IfElseStatement { get; set; }

        public List<IAsyncStep> EmittedAsyncSteps { get; set; }

        protected override void DoEmit()
        {
            VisitIfElseStatement();
        }

        protected void VisitIfElseStatement()
        {
            IfElseStatement ifElseStatement = IfElseStatement;

            var awaiters = GetAwaiters(ifElseStatement);

            if (awaiters == null || awaiters.Length == 0)
            {
                WriteIf();
                WriteOpenParentheses();
                ifElseStatement.Condition.AcceptVisitor(Emitter);
                WriteCloseParentheses();
                EmitBlockOrIndentedLine(ifElseStatement.TrueStatement);
                if (ifElseStatement.FalseStatement != null && !ifElseStatement.FalseStatement.IsNull)
                {
                    Write(" else");
                    EmitBlockOrIndentedLine(ifElseStatement.FalseStatement);
                }
                return;
            }

            WriteAwaiters(ifElseStatement.Condition);

            WriteIf();
            WriteOpenParentheses();

            var oldValue = Emitter.ReplaceAwaiterByVar;
            Emitter.ReplaceAwaiterByVar = true;
            ifElseStatement.Condition.AcceptVisitor(Emitter);
            Emitter.ReplaceAwaiterByVar = oldValue;

            WriteCloseParentheses();

            int startCount = 0;
            int elseCount = 0;
            IAsyncStep trueStep = null;
            IAsyncStep elseStep = null;

            if (Emitter.IsAsync)
            {
                startCount = Emitter.AsyncBlock.Steps.Count;

                EmittedAsyncSteps = Emitter.AsyncBlock.EmittedAsyncSteps;
                Emitter.AsyncBlock.EmittedAsyncSteps = new List<IAsyncStep>();

                Emitter.IgnoreBlock = ifElseStatement.TrueStatement;
                WriteSpace();
                BeginBlock();
                Write(JS.Vars.ASYNC_STEP + " = " + Emitter.AsyncBlock.Step + ";");
                WriteNewLine();
                Write("continue;");
                var writer = SaveWriter();
                Emitter.AsyncBlock.AddAsyncStep();
                ifElseStatement.TrueStatement.AcceptVisitor(Emitter);

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
            }
            else
            {
                EmitBlockOrIndentedLine(ifElseStatement.TrueStatement);
            }

            if (ifElseStatement.FalseStatement != null && !ifElseStatement.FalseStatement.IsNull)
            {
                WriteElse();
                if (Emitter.IsAsync)
                {
                    Emitter.IgnoreBlock = ifElseStatement.FalseStatement;
                    WriteSpace();
                    BeginBlock();
                    Write(JS.Vars.ASYNC_STEP + " = " + Emitter.AsyncBlock.Step + ";");
                    WriteNewLine();
                    Write("continue;");
                    var writer = SaveWriter();
                    Emitter.AsyncBlock.AddAsyncStep();
                    ifElseStatement.FalseStatement.AcceptVisitor(Emitter);

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
                }
                else
                {
                    EmitBlockOrIndentedLine(ifElseStatement.FalseStatement);
                }
            }

            if (Emitter.IsAsync && Emitter.AsyncBlock.Steps.Count > startCount)
            {
                if (Emitter.AsyncBlock.Steps.Count <= elseCount && !AbstractEmitterBlock.IsJumpStatementLast(Emitter.Output.ToString()))
                {
                    WriteNewLine();
                    Write(JS.Vars.ASYNC_STEP + " = " + Emitter.AsyncBlock.Step + ";");
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