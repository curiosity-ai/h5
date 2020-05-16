using H5.Contract;
using H5.Contract.Constants;

using ICSharpCode.NRefactory.CSharp;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;

namespace H5.Translator
{
    public class SwitchBlock : AbstractEmitterBlock
    {
        public SwitchBlock(IEmitter emitter, SwitchStatement switchStatement)
            : base(emitter, switchStatement)
        {
            Emitter = emitter;
            SwitchStatement = switchStatement;
        }

        public SwitchBlock(IEmitter emitter, SwitchSection switchSection)
            : base(emitter, switchSection)
        {
            Emitter = emitter;
            SwitchSection = switchSection;
        }

        public SwitchBlock(IEmitter emitter, CaseLabel caseLabel)
            : base(emitter, caseLabel)
        {
            Emitter = emitter;
            CaseLabel = caseLabel;
        }

        public SwitchStatement SwitchStatement { get; set; }

        public SwitchSection SwitchSection { get; set; }

        public CaseLabel CaseLabel { get; set; }

        public SwitchStatement ParentAsyncSwitch { get; set; }

        protected override void DoEmit()
        {
            if (SwitchStatement != null)
            {
                var awaiters = Emitter.IsAsync ? GetAwaiters(SwitchStatement) : null;

                if (awaiters != null && awaiters.Length > 0)
                {
                    VisitAsyncSwitchStatement();
                }
                else
                {
                    VisitSwitchStatement();
                }
            }
            else if (SwitchSection != null)
            {
                if (Emitter.AsyncSwitch != null)
                {
                    throw new EmitterException(SwitchSection, "Async switch section must be handled by VisitAsyncSwitchStatement method");
                }
                else
                {
                    VisitSwitchSection();
                }
            }
            else
            {
                if (Emitter.AsyncSwitch != null)
                {
                    throw new EmitterException(CaseLabel, "Async case label must be handled by VisitAsyncSwitchStatement method");
                }
                else
                {
                    VisitCaseLabel();
                }
            }
        }

        protected void VisitAsyncSwitchStatement()
        {
            SwitchStatement switchStatement = SwitchStatement;
            ParentAsyncSwitch = Emitter.AsyncSwitch;
            Emitter.AsyncSwitch = switchStatement;

            WriteAwaiters(switchStatement.Expression);

            var oldValue = Emitter.ReplaceAwaiterByVar;
            Emitter.ReplaceAwaiterByVar = true;
            string key = null;

            if (switchStatement.Expression is IdentifierExpression)
            {
                var oldBuilder = Emitter.Output;
                Emitter.Output = new StringBuilder();

                switchStatement.Expression.AcceptVisitor(Emitter);
                key = Emitter.Output.ToString().Trim();

                Emitter.Output = oldBuilder;
            }
            else
            {
                key = AddLocal(GetTempVarName(), null, AstType.Null);
                Write(key);
                Write(" = ");
                switchStatement.Expression.AcceptVisitor(Emitter);
                WriteSemiColon();
                WriteNewLine();
            }

            Emitter.ReplaceAwaiterByVar = oldValue;

            var list = switchStatement.SwitchSections.ToList();
            list.Sort((s1, s2) =>
            {
                var lbl = s1.CaseLabels.FirstOrDefault(l => l.Expression.IsNull);

                if (lbl != null)
                {
                    return 1;
                }

                lbl = s2.CaseLabels.FirstOrDefault(l => l.Expression.IsNull);

                if (lbl != null)
                {
                    return -1;
                }

                return 0;
            });

            var jumpStatements = Emitter.JumpStatements;
            Emitter.JumpStatements = new List<IJumpInfo>();
            bool writeElse = false;
            var thisStep = Emitter.AsyncBlock.Steps.Last();

            var rr = Emitter.Resolver.ResolveNode(switchStatement.Expression, Emitter);
            bool is64Bit = Helpers.Is64Type(rr.Type, Emitter.Resolver);

            foreach (var switchSection in list)
            {
                VisitAsyncSwitchSection(switchSection, writeElse, key, is64Bit);
                writeElse = true;
            }

            var nextStep = Emitter.AsyncBlock.AddAsyncStep();
            thisStep.JumpToStep = nextStep.Step;

            if (Emitter.JumpStatements.Count > 0)
            {
                Emitter.JumpStatements.Sort((j1, j2) => -j1.Position.CompareTo(j2.Position));
                foreach (var jump in Emitter.JumpStatements)
                {
                    if (jump.Break)
                    {
                        jump.Output.Insert(jump.Position, nextStep.Step);
                    }
                    else if (jumpStatements != null)
                    {
                        jumpStatements.Add(jump);
                    }
                }
            }

            Emitter.JumpStatements = jumpStatements;
            Emitter.AsyncSwitch = ParentAsyncSwitch;
        }

        protected void VisitAsyncSwitchSection(SwitchSection switchSection, bool writeElse, string switchKey, bool is64Bit)
        {
            var list = switchSection.CaseLabels.ToList();

            list.Sort((l1, l2) =>
            {
                if (l1.Expression.IsNull)
                {
                    return 1;
                }

                if (l2.Expression.IsNull)
                {
                    return -1;
                }

                return 0;
            });

            if (writeElse)
            {
                WriteElse();
            }

            if (list.Any(l => l.Expression.IsNull))
            {
                if (!writeElse)
                {
                    WriteElse();
                }
            }
            else
            {
                WriteIf();
                WriteOpenParentheses();

                var oldValue = Emitter.ReplaceAwaiterByVar;
                Emitter.ReplaceAwaiterByVar = true;
                bool writeOr = false;

                foreach (var label in list)
                {
                    if (writeOr)
                    {
                        WriteSpace();
                        Write("||");
                        WriteSpace();
                    }

                    Write(switchKey);
                    if (is64Bit)
                    {
                        WriteDot();
                        Write(JS.Funcs.Math.EQ);
                        WriteOpenParentheses();
                    }
                    else
                    {
                        Write(" === ");
                    }

                    label.Expression.AcceptVisitor(Emitter);

                    if (is64Bit)
                    {
                        Write(")");
                    }

                    if (label.Expression is NullReferenceExpression)
                    {
                        WriteSpace();
                        Write("||");
                        WriteSpace();
                        Write(switchKey);
                        Write(" === undefined");
                    }

                    writeOr = true;
                }

                WriteCloseParentheses();
                Emitter.ReplaceAwaiterByVar = oldValue;
            }

            var isBlock = false;
            if (switchSection.Statements.Count == 1 && switchSection.Statements.First() is BlockStatement)
            {
                isBlock = true;
                Emitter.IgnoreBlock = switchSection.Statements.First();
            }

            WriteSpace();
            BeginBlock();
            Write(JS.Vars.ASYNC_STEP + " = " + Emitter.AsyncBlock.Step + ";");
            WriteNewLine();
            Write("continue;");
            var writer = SaveWriter();
            var step = Emitter.AsyncBlock.AddAsyncStep();
            step.Node = switchSection;

            if (!isBlock)
            {
                PushLocals();
            }

            switchSection.Statements.AcceptVisitor(Emitter);

            if (!isBlock)
            {
                PopLocals();
            }

            if (RestoreWriter(writer) && !IsOnlyWhitespaceOnPenultimateLine(true))
            {
                WriteNewLine();
            }

            EndBlock();
            WriteNewLine();
        }

        protected void VisitSwitchStatement()
        {
            SwitchStatement switchStatement = SwitchStatement;
            ParentAsyncSwitch = Emitter.AsyncSwitch;
            Emitter.AsyncSwitch = null;

            var jumpStatements = Emitter.JumpStatements;
            Emitter.JumpStatements = null;

            WriteSwitch();
            WriteOpenParentheses();
            var rr = Emitter.Resolver.ResolveNode(switchStatement.Expression, Emitter);
            bool is64Bit = false;
            bool wrap = true;

            if (Helpers.Is64Type(rr.Type, Emitter.Resolver))
            {
                is64Bit = true;
                wrap = !(rr is LocalResolveResult || rr is MemberResolveResult);
            }

            if (is64Bit && wrap)
            {
                WriteOpenParentheses();
            }

            switchStatement.Expression.AcceptVisitor(Emitter);

            if (is64Bit)
            {
                if (wrap)
                {
                    WriteCloseParentheses();
                }

                WriteDot();
                Write(JS.Funcs.TOSTIRNG);
                WriteOpenCloseParentheses();
            }

            WriteCloseParentheses();
            WriteSpace();

            BeginBlock();
            switchStatement.SwitchSections.ToList().ForEach(s => s.AcceptVisitor(Emitter));
            EndBlock();
            WriteNewLine();
            Emitter.JumpStatements = jumpStatements;
            Emitter.AsyncSwitch = ParentAsyncSwitch;
        }

        protected void VisitSwitchSection()
        {
            SwitchSection switchSection = SwitchSection;

            switchSection.CaseLabels.ToList().ForEach(l => l.AcceptVisitor(Emitter));
            Indent();

            var isBlock = switchSection.Statements.Count == 1 && switchSection.Statements.First() is BlockStatement;
            if (!isBlock)
            {
                PushLocals();
            }
            var children = switchSection.Children.Where(c => c.Role == Roles.EmbeddedStatement || c.Role == Roles.Comment);
            children.ToList().ForEach(s => s.AcceptVisitor(Emitter));

            if (!isBlock)
            {
                PopLocals();
            }

            Outdent();
        }

        protected void VisitCaseLabel()
        {
            CaseLabel caseLabel = CaseLabel;

            if (caseLabel.Expression.IsNull)
            {
                Write("default");
            }
            else
            {
                Write("case ");

                var rr = Emitter.Resolver.ResolveNode(caseLabel.Expression.GetParent<SwitchStatement>().Expression, Emitter);
                var caserr = Emitter.Resolver.ResolveNode(caseLabel.Expression, Emitter);

                if (Helpers.Is64Type(rr.Type, Emitter.Resolver))
                {
                    if (caserr is ConstantResolveResult)
                    {
                        WriteScript(caserr.ConstantValue.ToString());
                    }
                    else
                    {
                        caseLabel.Expression.AcceptVisitor(Emitter);
                        WriteDot();
                        Write(JS.Funcs.TOSTIRNG);
                        WriteOpenCloseParentheses();
                    }
                }
                else
                {
                    caseLabel.Expression.AcceptVisitor(Emitter);
                }

                if (caserr.Type.Kind == TypeKind.Null)
                {
                    WriteColon();
                    WriteNewLine();
                    Write("case undefined");
                }
            }

            WriteColon();
            WriteNewLine();
        }
    }
}