using HighFive.Contract;
using ICSharpCode.NRefactory.CSharp;

namespace HighFive.Translator
{
    public class TypeBlock : AbstractEmitterBlock
    {
        public TypeBlock(IEmitter emitter, AstType type)
            : base(emitter, type)
        {
            this.Emitter = emitter;
            this.Type = type;
        }

        public AstType Type
        {
            get;
            set;
        }

        protected override void DoEmit()
        {
            this.EmitTypeReference();
        }

        protected virtual void EmitTypeReference()
        {
            AstType astType = this.Type;

            this.Write(HighFiveTypes.ToJsName(astType, this.Emitter));
        }
    }
}