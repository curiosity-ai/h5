using H5.Contract;
using H5.Contract.Constants;

using ICSharpCode.NRefactory.CSharp;

namespace H5.Translator
{
    public class ContinueBlock : AbstractEmitterBlock
    {
        public ContinueBlock(IEmitter emitter, ContinueStatement continueStatement)
            : base(emitter, continueStatement)
        {
            Emitter = emitter;
            ContinueStatement = continueStatement;
        }

        public ContinueStatement ContinueStatement { get; set; }

        protected override void DoEmit()
        {
            if (Emitter.JumpStatements != null)
            {
                var finallyNode = GetParentFinallyBlock(ContinueStatement, true);

                if (finallyNode != null)
                {
                    var hashcode = finallyNode.GetHashCode();
                    Emitter.AsyncBlock.JumpLabels.Add(new AsyncJumpLabel
                    {
                        Node = finallyNode,
                        Output = Emitter.Output
                    });
                    Write(JS.Vars.ASYNC_STEP + " = " + Helpers.PrefixDollar("{", hashcode, "};"));
                    WriteNewLine();
                    Write(JS.Vars.ASYNC_JUMP + " = ");
                    Emitter.JumpStatements.Add(new JumpInfo(Emitter.Output, Emitter.Output.Length, false));
                    WriteSemiColon();
                    WriteNewLine();
                }
                else
                {
                    Write(JS.Vars.ASYNC_STEP + " = ");
                    Emitter.JumpStatements.Add(new JumpInfo(Emitter.Output, Emitter.Output.Length, false));

                    WriteSemiColon();
                    WriteNewLine();
                }
            }

            if (Emitter.ReplaceJump && Emitter.JumpStatements == null)
            {
                Write("return {jump:1}");
            }
            else
            {
                Write("continue");
            }

            WriteSemiColon();
            WriteNewLine();
        }
    }
}