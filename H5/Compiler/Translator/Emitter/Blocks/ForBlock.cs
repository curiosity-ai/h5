using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using System.Collections.Generic;
using System.Linq;

namespace H5.Translator
{
    public class ForBlock : AbstractEmitterBlock
    {
        public ForBlock(IEmitter emitter, ForStatement forStatement)
            : base(emitter, forStatement)
        {
            Emitter = emitter;
            ForStatement = forStatement;
        }

        public ForStatement ForStatement { get; set; }

        public List<IAsyncStep> EmittedAsyncSteps { get; set; }

        protected override void DoEmit()
        {
            var awaiters = Emitter.IsAsync ? GetAwaiters(ForStatement) : null;

            if (awaiters != null && awaiters.Length > 0)
            {
                VisitAsyncForStatement();
            }
            else
            {
                VisitForStatement();
            }
        }

        protected void VisitAsyncForStatement()
        {
            ForStatement forStatement = ForStatement;
            var oldValue = Emitter.ReplaceAwaiterByVar;
            var jumpStatements = Emitter.JumpStatements;
            Emitter.JumpStatements = new List<IJumpInfo>();

            PushLocals();

            bool newLine = false;

            foreach (var item in forStatement.Initializers)
            {
                if (newLine)
                {
                    WriteNewLine();
                }

                item.AcceptVisitor(Emitter);
                newLine = true;
            }

            RemovePenultimateEmptyLines(true);
            WriteNewLine();
            Write(JS.Vars.ASYNC_STEP + " = " + Emitter.AsyncBlock.Step + ";");
            WriteNewLine();
            Write("continue;");

            IAsyncStep conditionStep = Emitter.AsyncBlock.AddAsyncStep();
            WriteAwaiters(forStatement.Condition);
            Emitter.ReplaceAwaiterByVar = true;
            var lastConditionStep = Emitter.AsyncBlock.Steps.Last();

            WriteIf();
            WriteOpenParentheses(true);

            if (!forStatement.Condition.IsNull)
            {
                forStatement.Condition.AcceptVisitor(Emitter);
            }
            else
            {
                Write("true");
            }

            WriteCloseParentheses(true);
            Emitter.ReplaceAwaiterByVar = oldValue;

            WriteSpace();
            BeginBlock();
            Write(JS.Vars.ASYNC_STEP + " = " + Emitter.AsyncBlock.Step + ";");
            WriteNewLine();
            Write("continue;");

            EmittedAsyncSteps = Emitter.AsyncBlock.EmittedAsyncSteps;
            Emitter.AsyncBlock.EmittedAsyncSteps = new List<IAsyncStep>();
            var writer = SaveWriter();

            Emitter.AsyncBlock.AddAsyncStep();
            Emitter.IgnoreBlock = forStatement.EmbeddedStatement;
            var startCount = Emitter.AsyncBlock.Steps.Count;
            forStatement.EmbeddedStatement.AcceptVisitor(Emitter);
            IAsyncStep loopStep = null;

            if (Emitter.AsyncBlock.Steps.Count > startCount)
            {
                loopStep = Emitter.AsyncBlock.Steps.Last();
            }

            RestoreWriter(writer);

            if (!AbstractEmitterBlock.IsJumpStatementLast(Emitter.Output.ToString()))
            {
                WriteNewLine();
                Write(JS.Vars.ASYNC_STEP + " = " + Emitter.AsyncBlock.Step + ";");
                WriteNewLine();
                Write("continue;");
                WriteNewLine();
                EndBlock();
                WriteSpace();
            }
            else
            {
                WriteNewLine();
                EndBlock();
                WriteSpace();
            }

            if (Emitter.IsAsync)
            {
                Emitter.AsyncBlock.EmittedAsyncSteps = EmittedAsyncSteps;
            }

            IAsyncStep iteratorsStep = Emitter.AsyncBlock.AddAsyncStep();

            /*foreach (var item in forStatement.Iterators)
            {
                this.WriteAwaiters(item);
            }*/

            var lastIteratorStep = Emitter.AsyncBlock.Steps.Last();

            if (loopStep != null)
            {
                loopStep.JumpToStep = iteratorsStep.Step;
            }

            lastIteratorStep.JumpToStep = conditionStep.Step;
            Emitter.ReplaceAwaiterByVar = true;

            var beforeStepsCount = Emitter.AsyncBlock.Steps.Count;
            foreach (var item in forStatement.Iterators)
            {
                item.AcceptVisitor(Emitter);

                if (Emitter.Output.ToString().TrimEnd().Last() != ';')
                {
                    WriteSemiColon();
                }

                WriteNewLine();
            }

            if (beforeStepsCount < Emitter.AsyncBlock.Steps.Count)
            {
                Emitter.AsyncBlock.Steps.Last().JumpToStep = conditionStep.Step;
            }

            Emitter.ReplaceAwaiterByVar = oldValue;

            PopLocals();
            var nextStep = Emitter.AsyncBlock.AddAsyncStep();
            lastConditionStep.JumpToStep = nextStep.Step;

            if (Emitter.JumpStatements.Count > 0)
            {
                Emitter.JumpStatements.Sort((j1, j2) => -j1.Position.CompareTo(j2.Position));
                foreach (var jump in Emitter.JumpStatements)
                {
                    jump.Output.Insert(jump.Position, jump.Break ? nextStep.Step : iteratorsStep.Step);
                }
            }

            Emitter.JumpStatements = jumpStatements;
        }

        protected void VisitForStatement()
        {
            ForStatement forStatement = ForStatement;
            var jumpStatements = Emitter.JumpStatements;
            Emitter.JumpStatements = null;

            PushLocals();
            Emitter.EnableSemicolon = false;

            WriteFor();
            WriteOpenParentheses();

            var old = Emitter.IsAsync;
            Emitter.IsAsync = false;
            foreach (var item in forStatement.Initializers)
            {
                if (item != forStatement.Initializers.First())
                {
                    WriteComma();
                }

                item.AcceptVisitor(Emitter);
            }
            Emitter.IsAsync = old;

            WriteSemiColon();
            WriteSpace();

            if (!forStatement.Condition.IsNull)
            {
                forStatement.Condition.AcceptVisitor(Emitter);
            }

            WriteSemiColon();
            WriteSpace();

            foreach (var item in forStatement.Iterators)
            {
                if (item != forStatement.Iterators.First())
                {
                    WriteComma();
                }

                item.AcceptVisitor(Emitter);
            }

            WriteCloseParentheses();

            Emitter.EnableSemicolon = true;

            EmitBlockOrIndentedLine(forStatement.EmbeddedStatement);

            PopLocals();
            Emitter.JumpStatements = jumpStatements;
        }
    }
}