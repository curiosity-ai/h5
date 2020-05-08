using Bridge.Contract;
using ICSharpCode.NRefactory.CSharp;

namespace Bridge.Translator
{
    public abstract class AbstractCommentBlock : AbstractEmitterBlock
    {
        public AbstractCommentBlock(IEmitter emitter, AstNode node)
            : base(emitter, node)
        {
        }

        public override void Emit()
        {
            this.EmitComment();
            base.Emit();
        }

        public virtual void EmitComment()
        {
            XmlToJsDoc.EmitComment(this, this.Emitter.Translator.EmitNode);
        }
    }
}