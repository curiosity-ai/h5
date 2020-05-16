using H5.Contract;
using H5.Contract.Constants;

using ICSharpCode.NRefactory.CSharp;

using System.Collections.Generic;
using System.Linq;

namespace H5.Translator
{
    public class WhileBlock : AbstractEmitterBlock
    {
        public WhileBlock(IEmitter emitter, WhileStatement whileStatement)
            : base(emitter, whileStatement)
        {
            Emitter = emitter;
            WhileStatement = whileStatement;
        }

        public WhileStatement WhileStatement { get; set; }

        protected override void DoEmit()
        {
            var awaiters = Emitter.IsAsync ? GetAwaiters(WhileStatement) : null;

            if (awaiters != null && awaiters.Length > 0)
            {
                VisitAsyncWhileStatement();
            }
            else
            {
                VisitWhileStatement();
            }
        }

        protected void VisitAsyncWhileStatement()
        {
            var oldValue = Emitter.ReplaceAwaiterByVar;
            var jumpStatements = Emitter.JumpStatements;
            Emitter.JumpStatements = new List<IJumpInfo>();

            IAsyncStep conditionStep = null;
            var lastStep = Emitter.AsyncBlock.Steps.Last();

            if (string.IsNullOrWhiteSpace(lastStep.Output.ToString()))
            {
                conditionStep = lastStep;
            }
            else
            {
                lastStep.JumpToStep = Emitter.AsyncBlock.Step;
                conditionStep = Emitter.AsyncBlock.AddAsyncStep();
            }

            WriteAwaiters(WhileStatement.Condition);
            Emitter.ReplaceAwaiterByVar = true;

            WriteIf();
            WriteOpenParentheses(true);
            WhileStatement.Condition.AcceptVisitor(Emitter);
            WriteCloseParentheses(true);
            Emitter.ReplaceAwaiterByVar = oldValue;

            WriteSpace();
            BeginBlock();

            Write(JS.Vars.ASYNC_STEP + " = " + Emitter.AsyncBlock.Step + ";");
            WriteNewLine();
            Write("continue;");

            var writer = SaveWriter();

            Emitter.AsyncBlock.AddAsyncStep();
            Emitter.IgnoreBlock = WhileStatement.EmbeddedStatement;

            var startCount = Emitter.AsyncBlock.Steps.Count;

            WhileStatement.EmbeddedStatement.AcceptVisitor(Emitter);

            if (!IsJumpStatementLast(Emitter.Output.ToString()))
            {
                WriteNewLine();
                Write(JS.Vars.ASYNC_STEP + " = " + conditionStep.Step + ";");
                WriteNewLine();
                Write("continue;");
            }

            RestoreWriter(writer);

            WriteNewLine();
            EndBlock();
            WriteSpace();

            if (!IsJumpStatementLast(Emitter.Output.ToString()))
            {
                WriteNewLine();
                Write(JS.Vars.ASYNC_STEP + " = " + Emitter.AsyncBlock.Step + ";");
                WriteNewLine();
                Write("continue;");
            }

            var nextStep = Emitter.AsyncBlock.AddAsyncStep();
            conditionStep.JumpToStep = nextStep.Step;

            if (Emitter.JumpStatements.Count > 0)
            {
                Emitter.JumpStatements.Sort((j1, j2) => -j1.Position.CompareTo(j2.Position));
                foreach (var jump in Emitter.JumpStatements)
                {
                    jump.Output.Insert(jump.Position, jump.Break ? nextStep.Step : conditionStep.Step);
                }
            }

            Emitter.JumpStatements = jumpStatements;
        }

        protected void VisitWhileStatement()
        {
            var jumpStatements = Emitter.JumpStatements;
            Emitter.JumpStatements = null;

            WriteWhile();
            WriteOpenParentheses();
            WhileStatement.Condition.AcceptVisitor(Emitter);
            WriteCloseParentheses();
            EmitBlockOrIndentedLine(WhileStatement.EmbeddedStatement);

            Emitter.JumpStatements = jumpStatements;
        }
    }
}