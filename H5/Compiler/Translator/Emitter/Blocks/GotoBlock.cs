using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.TypeSystem;
using System.Collections.Generic;
using System.Linq;

namespace H5.Translator
{
    public class GotoBlock : AbstractEmitterBlock
    {
        public GotoBlock(IEmitter emitter, GotoCaseStatement gotoCaseStatement) : base(emitter, gotoCaseStatement)
        {
            GotoCaseStatement = gotoCaseStatement;
        }

        public GotoBlock(IEmitter emitter, GotoDefaultStatement gotoDefaultStatement) : base(emitter, gotoDefaultStatement)
        {
            GotoDefaultStatement = gotoDefaultStatement;
        }

        public GotoBlock(IEmitter emitter, GotoStatement gotoStatement) : base(emitter, gotoStatement)
        {
            GotoStatement = gotoStatement;
        }

        public GotoCaseStatement GotoCaseStatement
        {
            get; set;
        }

        public GotoDefaultStatement GotoDefaultStatement
        {
            get; set;
        }

        public GotoStatement GotoStatement
        {
            get; set;
        }

        protected override void DoEmit()
        {
            AstNode node = null;
            if (GotoStatement != null)
            {
                node = Emitter.AsyncBlock.AwaitExpressions.First(expr => expr is LabelStatement && ((LabelStatement)expr).Label == GotoStatement.Label);
            }
            else if (GotoCaseStatement != null)
            {
                var switchStatement = GotoCaseStatement.GetParent<SwitchStatement>();
                var rr = Emitter.Resolver.ResolveNode(GotoCaseStatement.LabelExpression, Emitter);

                node = switchStatement.SwitchSections.First(ss => ss.CaseLabels.Any(cl =>
                {
                    if (cl.Expression.IsNull)
                    {
                        return false;
                    }

                    var caseLabel_rr = Emitter.Resolver.ResolveNode(cl.Expression, Emitter);

                    if (caseLabel_rr.ConstantValue is string)
                    {
                        return caseLabel_rr.ConstantValue.Equals(rr.ConstantValue);
                    }

                    return System.Convert.ToInt64(caseLabel_rr.ConstantValue) == System.Convert.ToInt64(rr.ConstantValue);
                }));
            }
            else if (GotoDefaultStatement != null)
            {
                var switchStatement = GotoDefaultStatement.GetParent<SwitchStatement>();
                node = switchStatement.SwitchSections.First(ss => ss.CaseLabels.Any(cl => cl.Expression.IsNull));
            }

            var hashcode = node.GetHashCode();
            Emitter.AsyncBlock.JumpLabels.Add(new AsyncJumpLabel
            {
                Node = node,
                Output = Emitter.Output
            });
            Write(JS.Vars.ASYNC_STEP + " = " + Helpers.PrefixDollar("{", hashcode, "};"));
            WriteNewLine();
            Write("continue;");
        }
    }
}