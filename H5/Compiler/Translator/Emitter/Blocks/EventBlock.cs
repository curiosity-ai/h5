using H5.Contract;
using ICSharpCode.NRefactory.CSharp;
using System.Collections.Generic;

namespace H5.Translator
{
    public class EventBlock : AbstractEmitterBlock
    {
        public EventBlock(IEmitter emitter, IEnumerable<EventDeclaration> events)
            : base(emitter, null)
        {
            Emitter = emitter;
            Events = events;
        }

        public IEnumerable<EventDeclaration> Events { get; set; }

        protected override void DoEmit()
        {
            EmitEvents(Events);
        }

        protected virtual void EmitEvents(IEnumerable<EventDeclaration> events)
        {
            foreach (var evt in events)
            {
                foreach (var evtVar in evt.Variables)
                {
                    Emitter.Translator.EmitNode = evtVar;
                    string name = Emitter.GetEntityName(evt);

                    Write("this.", name, " = ");
                    evtVar.Initializer.AcceptVisitor(Emitter);
                    WriteSemiColon();
                    WriteNewLine();
                }
            }
        }
    }
}