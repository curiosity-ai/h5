using H5.Contract;
using ICSharpCode.NRefactory.CSharp;

namespace H5.Translator
{
    public class EventDeclarationBlock : AbstractEmitterBlock
    {
        public EventDeclarationBlock(IEmitter emitter, EventDeclaration eventDeclaration)
            : base(emitter, eventDeclaration)
        {
            this.Emitter = emitter;
            this.EventDeclaration = eventDeclaration;
        }

        public EventDeclaration EventDeclaration { get; set; }

        protected override void DoEmit()
        {
            var vars = this.EventDeclaration.Variables;
        }
    }
}