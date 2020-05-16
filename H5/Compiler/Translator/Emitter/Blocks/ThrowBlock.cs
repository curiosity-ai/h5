using H5.Contract;
using H5.Contract.Constants;

using ICSharpCode.NRefactory.CSharp;

using System;
using System.Linq;
using ICSharpCode.NRefactory.Semantics;

namespace H5.Translator
{
    public class ThrowBlock : AbstractEmitterBlock
    {
        public ThrowBlock(IEmitter emitter, ThrowStatement throwStatement)
            : base(emitter, throwStatement)
        {
            Emitter = emitter;
            ThrowStatement = throwStatement;
        }

        public ThrowStatement ThrowStatement { get; set; }

        protected override void DoEmit()
        {
            var oldValue = Emitter.ReplaceAwaiterByVar;

            if (Emitter.IsAsync)
            {
                WriteAwaiters(ThrowStatement.Expression);
                Emitter.ReplaceAwaiterByVar = true;
            }

            WriteThrow();

            if (ThrowStatement.Expression.IsNull)
            {
                string name = Emitter.CatchBlockVariable ?? JS.Vars.ASYNC_E;
                Write(name);
            }
            else
            {
                ThrowStatement.Expression.AcceptVisitor(Emitter);
            }

            WriteSemiColon();
            WriteNewLine();
            Emitter.ReplaceAwaiterByVar = oldValue;
        }
    }
}