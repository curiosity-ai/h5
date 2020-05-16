using H5.Contract;
using H5.Contract.Constants;

using ICSharpCode.NRefactory.CSharp;

using System.Collections.Generic;
using System.Linq;

namespace H5.Translator
{
    public class DoWhileBlock : AbstractEmitterBlock
    {
        public DoWhileBlock(IEmitter emitter, DoWhileStatement doWhileStatement)
            : base(emitter, doWhileStatement)
        {
            Emitter = emitter;
            DoWhileStatement = doWhileStatement;
        }

        public DoWhileStatement DoWhileStatement { get; set; }

        protected override void DoEmit()
        {
            var awaiters = Emitter.IsAsync ? GetAwaiters(DoWhileStatement) : null;

            if (awaiters != null && awaiters.Length > 0)
            {
                VisitAsyncDoWhileStatement();
            }
            else
            {
                VisitDoWhileStatement();
            }
        }

        protected void VisitAsyncDoWhileStatement()
        {
            DoWhileStatement doWhileStatement = DoWhileStatement;

            var oldValue = Emitter.ReplaceAwaiterByVar;
            var jumpStatements = Emitter.JumpStatements;
            Emitter.JumpStatements = new List<IJumpInfo>();

            var loopStep = Emitter.AsyncBlock.Steps.Last();

            if (!string.IsNullOrWhiteSpace(loopStep.Output.ToString()))
            {
                loopStep = Emitter.AsyncBlock.AddAsyncStep();
            }

            Emitter.IgnoreBlock = doWhileStatement.EmbeddedStatement;
            doWhileStatement.EmbeddedStatement.AcceptVisitor(Emitter);

            Emitter.AsyncBlock.Steps.Last().JumpToStep = Emitter.AsyncBlock.Step;
            var conditionStep = Emitter.AsyncBlock.AddAsyncStep();
            WriteAwaiters(doWhileStatement.Condition);

            WriteIf();
            WriteOpenParentheses(true);
            Emitter.ReplaceAwaiterByVar = true;
            doWhileStatement.Condition.AcceptVisitor(Emitter);
            WriteCloseParentheses(true);
            Emitter.ReplaceAwaiterByVar = oldValue;

            WriteSpace();
            BeginBlock();
            WriteNewLine();
            Write(JS.Vars.ASYNC_STEP + " = " + loopStep.Step + ";");
            WriteNewLine();
            Write("continue;");
            WriteNewLine();
            EndBlock();

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

        protected void VisitDoWhileStatement()
        {
            DoWhileStatement doWhileStatement = DoWhileStatement;
            var jumpStatements = Emitter.JumpStatements;
            Emitter.JumpStatements = null;

            WriteDo();
            EmitBlockOrIndentedLine(doWhileStatement.EmbeddedStatement);

            if (doWhileStatement.EmbeddedStatement is BlockStatement)
            {
                WriteSpace();
            }

            WriteWhile();
            WriteOpenParentheses();

            doWhileStatement.Condition.AcceptVisitor(Emitter);

            WriteCloseParentheses();
            WriteSemiColon();

            WriteNewLine();

            Emitter.JumpStatements = jumpStatements;
        }
    }
}