using System.Linq;
using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.TypeSystem;

namespace H5.Translator
{
    public class ReturnBlock : AbstractEmitterBlock
    {
        public ReturnBlock(IEmitter emitter, ReturnStatement returnStatement)
            : base(emitter, returnStatement)
        {
            Emitter = emitter;
            ReturnStatement = returnStatement;
        }

        public ReturnBlock(IEmitter emitter, Expression expression)
            : base(emitter, expression)
        {
            Emitter = emitter;
            Expression = expression;
        }

        public Expression Expression { get; set; }

        public ReturnStatement ReturnStatement { get; set; }

        protected override void DoEmit()
        {
            VisitReturnStatement();
        }

        protected void VisitReturnStatement()
        {
            ReturnStatement returnStatement = ReturnStatement;
            Expression expression = Expression;

            if (Emitter.IsAsync && !Emitter.IsNativeAsync && (Emitter.AsyncBlock.MethodDeclaration == null || Emitter.AsyncBlock.MethodDeclaration.HasModifier(Modifiers.Async)))
            {
                var finallyNode = GetParentFinallyBlock(returnStatement ?? (AstNode)expression, false);
                var catchNode = GetParentCatchBlock(returnStatement ?? (AstNode)expression, false);
                expression = returnStatement != null ? returnStatement.Expression : expression;

                if (Emitter.AsyncBlock != null && Emitter.AsyncBlock.IsTaskReturn)
                {
                    WriteAwaiters(expression);

                    if (finallyNode != null)
                    {
                        Write(JS.Vars.ASYNC_RETURN_VALUE + " = ");
                    }
                    else
                    {
                        Write(JS.Vars.ASYNC_TCS + "." + ((Emitter.AssemblyInfo.Rules.UseShortForms ?? false) ? JS.Funcs.SHORTEN_SET_RESULT : JS.Funcs.SET_RESULT) + "(");
                    }

                    if (!expression.IsNull)
                    {
                        var oldValue = Emitter.ReplaceAwaiterByVar;
                        Emitter.ReplaceAwaiterByVar = true;
                        expression.AcceptVisitor(Emitter);
                        Emitter.ReplaceAwaiterByVar = oldValue;
                    }
                    else
                    {
                        Write("null");
                    }

                    Write(finallyNode != null ? ";" : ");");
                    WriteNewLine();
                }

                if (finallyNode != null)
                {
                    if (catchNode == null || !catchNode.Body.Statements.Last().Equals(ReturnStatement))
                    {
                        if (catchNode != null)
                        {
                            Write(JS.Vars.ASYNC_E + " = null;");
                        }

                        var hashcode = finallyNode.GetHashCode();
                        Emitter.AsyncBlock.JumpLabels.Add(new AsyncJumpLabel
                        {
                            Node = finallyNode,
                            Output = Emitter.Output
                        });
                        Write(JS.Vars.ASYNC_STEP + " = " + Helpers.PrefixDollar("{", hashcode, "};"));
                        WriteNewLine();
                        Write("continue;");
                        WriteNewLine();
                    }
                }
                else
                {
                    WriteReturn(false);
                    WriteSemiColon();
                    WriteNewLine();
                }
            }
            else
            {
                WriteReturn(false);
                expression = returnStatement != null ? returnStatement.Expression : expression;

                if (Emitter.ReturnType != null && Emitter.ReturnType.Kind == TypeKind.ByReference && !expression.IsNull)
                {
                    WriteSpace();
                    Emitter.EmitReference(expression);
                }
                else if (Emitter.ReplaceJump && Emitter.JumpStatements == null)
                {
                    WriteSpace();
                    Write("{jump: 3");

                    if (!expression.IsNull)
                    {
                        Write(", v: ");
                        expression.AcceptVisitor(Emitter);
                    }

                    Write("}");
                }
                else if (!expression.IsNull)
                {
                    WriteSpace();
                    expression.AcceptVisitor(Emitter);
                }

                WriteSemiColon();
                WriteNewLine();
            }
        }
    }
}