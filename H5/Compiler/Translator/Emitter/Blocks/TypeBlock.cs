using H5.Contract;
using ICSharpCode.NRefactory.CSharp;

namespace H5.Translator
{
    public class TypeBlock : AbstractEmitterBlock
    {
        public TypeBlock(IEmitter emitter, AstType type)
            : base(emitter, type)
        {
            Emitter = emitter;
            Type = type;
        }

        public AstType Type { get; set; }

        protected override void DoEmit()
        {
            EmitTypeReference();
        }

        protected virtual void EmitTypeReference()
        {
            AstType astType = Type;

            Write(H5Types.ToJsName(astType, Emitter));
        }
    }
}