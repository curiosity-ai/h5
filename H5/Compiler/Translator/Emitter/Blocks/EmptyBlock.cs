using H5.Contract;
using ICSharpCode.NRefactory.CSharp;

namespace H5.Translator
{
    public class EmptyBlock : AbstractEmitterBlock
    {
        public EmptyBlock(IEmitter emitter, EmptyStatement emptyStatement)
            : base(emitter, emptyStatement)
        {
            Emitter = emitter;
            EmptyStatement = emptyStatement;
        }

        public EmptyStatement EmptyStatement { get; set; }

        protected override void DoEmit()
        {
            WriteSemiColon(true);
        }
    }
}