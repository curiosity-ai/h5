using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;

namespace H5.Translator
{
    public class BreakBlock : AbstractEmitterBlock
    {
        public BreakBlock(IEmitter emitter, BreakStatement breakStatement)
            : base(emitter, breakStatement)
        {
            Emitter        = emitter;
            BreakStatement = breakStatement;
        }

        public BreakBlock(IEmitter emitter, YieldBreakStatement breakStatement)
            : base(emitter, breakStatement)
        {
            Emitter             = emitter;
            YieldBreakStatement = breakStatement;
        }

        public BreakStatement BreakStatement { get; set; }

        public YieldBreakStatement YieldBreakStatement { get; set; }

        protected override void DoEmit()
        {
            if (Emitter.JumpStatements != null)
            {
                var finallyNode = GetParentFinallyBlock(BreakStatement ?? (AstNode)YieldBreakStatement, true);

                if (finallyNode != null)
                {
                    var hashcode = finallyNode.GetHashCode();

                    Emitter.AsyncBlock.JumpLabels.Add(new AsyncJumpLabel
                    {
                        Node   = finallyNode,
                        Output = Emitter.Output
                    });
                    Write(JS.Vars.ASYNC_STEP + " = " + Helpers.PrefixDollar("{", hashcode, "};"));
                    WriteNewLine();
                    Write(JS.Vars.ASYNC_JUMP + " = ");
                    Emitter.JumpStatements.Add(new JumpInfo(Emitter.Output, Emitter.Output.Length, true));
                    WriteSemiColon();
                    WriteNewLine();
                }
                else
                {
                    Write(JS.Vars.ASYNC_STEP + " = ");
                    Emitter.JumpStatements.Add(new JumpInfo(Emitter.Output, Emitter.Output.Length, true));

                    WriteSemiColon();
                    WriteNewLine();
                }

                Write("continue");
            }
            else
            {
                if (Emitter.ReplaceJump)
                {
                    var found = false;

                    BreakStatement.GetParent(n =>
                    {
                        if (n is SwitchStatement)
                        {
                            found = true;
                            return true;
                        }

                        if (n is ForStatement || n is ForeachStatement || n is WhileStatement || n is DoWhileStatement || n is AnonymousMethodExpression || n is LambdaExpression)
                        {
                            found = false;
                            return true;
                        }

                        return false;
                    });

                    if (!found)
                    {
                        Write("return {jump:2}");
                    }
                    else
                    {
                        Write("break");
                    }
                }
                else
                {
                    Write("break");
                }
            }

            WriteSemiColon();
            WriteNewLine();
        }
    }
}