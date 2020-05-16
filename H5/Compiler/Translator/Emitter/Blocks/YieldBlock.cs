using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.TypeSystem;
using System.Collections.Generic;
using System.Linq;

namespace H5.Translator
{
    public class YieldBlock : AbstractEmitterBlock
    {
        public YieldBlock(IEmitter emitter, YieldBreakStatement yieldBreakStatement)
            : base(emitter, yieldBreakStatement)
        {
            Emitter = emitter;
            YieldBreakStatement = yieldBreakStatement;
        }

        public YieldBlock(IEmitter emitter, YieldReturnStatement yieldReturnStatement)
            : base(emitter, yieldReturnStatement)
        {
            Emitter = emitter;
            YieldReturnStatement = yieldReturnStatement;
        }

        public YieldBreakStatement YieldBreakStatement { get; set; }

        public YieldReturnStatement YieldReturnStatement { get; set; }

        public List<IAsyncStep> EmittedAsyncSteps { get; set; }

        protected override void DoEmit()
        {
            EmittedAsyncSteps = Emitter.AsyncBlock.EmittedAsyncSteps;
            Emitter.AsyncBlock.EmittedAsyncSteps = new List<IAsyncStep>();

            if (YieldReturnStatement != null)
            {
                Write(JS.Vars.ENUMERATOR + "." + JS.Fields.CURRENT + " = ");
                YieldReturnStatement.Expression.AcceptVisitor(Emitter);
                WriteSemiColon();
                WriteNewLine();

                Write(JS.Vars.ASYNC_STEP + " = " + Emitter.AsyncBlock.Step);
                WriteSemiColon();
                WriteNewLine();

                WriteReturn(true);
                Write("true");
                WriteSemiColon();
                WriteNewLine();

                Emitter.AsyncBlock.AddAsyncStep();
            }
            else if (YieldBreakStatement != null)
            {
                WriteReturn(true);
                Write("false");

                WriteSemiColon();
                WriteNewLine();
            }

            Emitter.AsyncBlock.EmittedAsyncSteps = EmittedAsyncSteps;
        }

        public static bool HasYield(AstNode node)
        {
            var visitor = new YieldSearchVisitor();
            node.AcceptVisitor(visitor);
            return visitor.Found;
        }
    }
}