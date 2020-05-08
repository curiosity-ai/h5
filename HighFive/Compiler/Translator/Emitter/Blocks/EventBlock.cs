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
            this.Emitter = emitter;
            this.Events = events;
        }

        public IEnumerable<EventDeclaration> Events
        {
            get;
            set;
        }

        protected override void DoEmit()
        {
            this.EmitEvents(this.Events);
        }

        protected virtual void EmitEvents(IEnumerable<EventDeclaration> events)
        {
            foreach (var evt in events)
            {
                foreach (var evtVar in evt.Variables)
                {
                    this.Emitter.Translator.EmitNode = evtVar;
                    string name = this.Emitter.GetEntityName(evt);

                    this.Write("this.", name, " = ");
                    evtVar.Initializer.AcceptVisitor(this.Emitter);
                    this.WriteSemiColon();
                    this.WriteNewLine();
                }
            }
        }
    }
}