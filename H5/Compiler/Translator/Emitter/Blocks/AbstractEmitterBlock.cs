using System.Text;
using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.Semantics;

namespace H5.Translator
{
    public abstract partial class AbstractEmitterBlock : IAbstractEmitterBlock
    {
        private AstNode previousNode;

        public AbstractEmitterBlock(IEmitter emitter, AstNode node)
        {
            Emitter = emitter;
            previousNode = Emitter.Translator.EmitNode;
            Emitter.Translator.EmitNode = node;
        }

        protected abstract void DoEmit();

        public AstNode PreviousNode
        {
            get
            {
                return previousNode;
            }
        }

        public IEmitter Emitter { get; set; }

        public virtual void Emit()
        {
            BeginEmit();
            DoEmit();
            EndEmit();
        }

        private int startPos;
        private int checkPos;
        private StringBuilder checkedOutput;

        protected virtual void BeginEmit()
        {
            if (NeedSequencePoint())
            {
                startPos = Emitter.Output.Length;
                WriteSequencePoint(Emitter.Translator.EmitNode.Region);
                checkPos = Emitter.Output.Length;
                checkedOutput = Emitter.Output;
            }
        }

        protected virtual void EndEmit()
        {
            if (NeedSequencePoint() && checkPos == Emitter.Output.Length && checkedOutput == Emitter.Output)
            {
                Emitter.Output.Length = startPos;
            }
            Emitter.Translator.EmitNode = previousNode;
        }

        protected bool NeedSequencePoint()
        {
            if (Emitter.Translator.EmitNode != null && !Emitter.Translator.EmitNode.Region.IsEmpty)
            {
                if (Emitter.Translator.EmitNode is EntityDeclaration ||
                    Emitter.Translator.EmitNode is BlockStatement ||
                    Emitter.Translator.EmitNode is ArrayInitializerExpression ||
                    Emitter.Translator.EmitNode is PrimitiveExpression ||
                    Emitter.Translator.EmitNode is Comment)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        public virtual void EmitBlockOrIndentedLine(AstNode node)
        {
            bool block = node is BlockStatement;

            if (!block && node is IfElseStatement && node.Parent is IfElseStatement ifStatement && ifStatement.FalseStatement == node)
            {
                block = true;
            }

            if (!block)
            {
                WriteNewLine();
                Indent();
            }
            else
            {
                WriteSpace();
            }

            node.AcceptVisitor(Emitter);

            if (!block)
            {
                Outdent();
            }
        }

        public bool NoValueableSiblings(AstNode node)
        {
            while (node.NextSibling != null)
            {
                var sibling = node.NextSibling;

                if (sibling is NewLineNode || sibling is CSharpTokenNode || sibling is Comment)
                {
                    node = sibling;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        protected AstNode[] GetAwaiters(AstNode node)
        {
            var awaitSearch = new AwaitSearchVisitor(Emitter);
            node.AcceptVisitor(awaitSearch);

            return awaitSearch.GetAwaitExpressions().ToArray();
        }

        protected bool IsDirectAsyncBlockChild(AstNode node)
        {
            var block = node.GetParent<BlockStatement>();

            if (block != null && (block.Parent is MethodDeclaration || block.Parent is AnonymousMethodExpression || block.Parent is LambdaExpression))
            {
                return true;
            }

            return false;
        }

        protected IAsyncStep WriteAwaiter(AstNode node)
        {
            var index = System.Array.IndexOf(Emitter.AsyncBlock.AwaitExpressions, node) + 1;

            if (node is ConditionalExpression expression)
            {
                new ConditionalBlock(Emitter, expression).WriteAsyncConditionalExpression(index);
                return null;
            }

            if (node is BinaryOperatorExpression binaryOperatorExpression)
            {
                if (binaryOperatorExpression.Operator == BinaryOperatorType.BitwiseAnd ||
                    binaryOperatorExpression.Operator == BinaryOperatorType.BitwiseOr ||
                    binaryOperatorExpression.Operator == BinaryOperatorType.ConditionalOr ||
                    binaryOperatorExpression.Operator == BinaryOperatorType.ConditionalAnd)
                {
                    new BinaryOperatorBlock(Emitter, binaryOperatorExpression).WriteAsyncBinaryExpression(index);
                    return null;
                }
            }

            if (Emitter.AsyncBlock.WrittenAwaitExpressions.Contains(node))
            {
                return null;
            }

            Emitter.AsyncBlock.WrittenAwaitExpressions.Add(node);

            Write(JS.Vars.ASYNC_TASK + index + " = ");
            bool customAwaiter = false;
            var oldValue = Emitter.ReplaceAwaiterByVar;
            Emitter.ReplaceAwaiterByVar = true;

            if (node.Parent is UnaryOperatorExpression unaryExpr && unaryExpr.Operator == UnaryOperatorType.Await)
            {
                if (Emitter.Resolver.ResolveNode(unaryExpr) is AwaitResolveResult rr)
                {
                    if (rr.GetAwaiterInvocation is InvocationResolveResult awaiterMethod && awaiterMethod.Member.FullName != "System.Threading.Tasks.Task.GetAwaiter")
                    {
                        WriteCustomAwaiter(node, awaiterMethod);
                        customAwaiter = true;
                    }
                }
            }

            if (!customAwaiter)
            {
                node.AcceptVisitor(Emitter);
            }

            Emitter.ReplaceAwaiterByVar = oldValue;

            WriteSemiColon();
            WriteNewLine();
            Write(JS.Vars.ASYNC_STEP + " = " + Emitter.AsyncBlock.Step + ";");
            WriteNewLine();

            WriteIf();
            WriteOpenParentheses();

            Write(JS.Vars.ASYNC_TASK + index + ".isCompleted()");

            WriteCloseParentheses();

            WriteSpace();

            WriteBlock("continue;");

            Write(JS.Vars.ASYNC_TASK + index + "." + JS.Funcs.CONTINUE_WITH + "(" + JS.Funcs.ASYNC_BODY + ");");

            WriteNewLine();

            if (Emitter.WrapRestCounter > 0)
            {
                EndBlock();
                Write("));");
                WriteNewLine();
                Emitter.WrapRestCounter--;
            }

            Write("return;");

            var asyncStep = Emitter.AsyncBlock.AddAsyncStep(index);

            if (Emitter.AsyncBlock.EmittedAsyncSteps != null)
            {
                Emitter.AsyncBlock.EmittedAsyncSteps.Add(asyncStep);
            }

            return asyncStep;
        }

        private void WriteCustomAwaiter(AstNode node, InvocationResolveResult awaiterMethod)
        {
            var method = awaiterMethod.Member;
            var inline = Emitter.GetInline(method);

            if (!string.IsNullOrWhiteSpace(inline))
            {
                var argsInfo = new ArgumentsInfo(Emitter, node as Expression, awaiterMethod);
                new InlineArgumentsBlock(Emitter, argsInfo, inline).Emit();
            }
            else
            {
                if (method.IsStatic)
                {
                    Write(H5Types.ToJsName(method.DeclaringType, Emitter));
                    WriteDot();
                    Write(OverloadsCollection.Create(Emitter, method).GetOverloadName());
                    WriteOpenParentheses();
                    new ExpressionListBlock(Emitter, new Expression[] { (Expression)node }, null, null, 0).Emit();
                    WriteCloseParentheses();
                }
                else
                {
                    node.AcceptVisitor(Emitter);
                    WriteDot();
                    var name = OverloadsCollection.Create(Emitter, method).GetOverloadName();
                    Write(name);
                    WriteOpenParentheses();
                    WriteCloseParentheses();
                }
            }
        }

        protected void WriteAwaiters(AstNode node)
        {
            var awaiters = Emitter.IsAsync && !node.IsNull ? GetAwaiters(node) : null;

            if (awaiters != null && awaiters.Length > 0)
            {
                var oldValue = Emitter.AsyncExpressionHandling;
                Emitter.AsyncExpressionHandling = true;

                foreach (var awaiter in awaiters)
                {
                    WriteAwaiter(awaiter);
                }

                Emitter.AsyncExpressionHandling = oldValue;
            }
        }

        public AstNode GetParentFinallyBlock(AstNode node, bool stopOnLoops)
        {
            var insideTryFinally = false;
            var target = node.GetParent(n =>
            {
                if (n is LambdaExpression || n is AnonymousMethodExpression || n is MethodDeclaration)
                {
                    return true;
                }

                if (stopOnLoops && (n is WhileStatement || n is ForeachStatement || n is ForStatement || n is DoWhileStatement))
                {
                    return true;
                }

                if (n is TryCatchStatement statement && !statement.FinallyBlock.IsNull)
                {
                    insideTryFinally = true;
                    return true;
                }

                return false;
            });

            return insideTryFinally ? ((TryCatchStatement)target).FinallyBlock : null;
        }

        public CatchClause GetParentCatchBlock(AstNode node, bool stopOnLoops)
        {
            var insideCatch = false;
            var target = node.GetParent(n =>
            {
                if (n is LambdaExpression || n is AnonymousMethodExpression || n is MethodDeclaration)
                {
                    return true;
                }

                if (stopOnLoops && (n is WhileStatement || n is ForeachStatement || n is ForStatement || n is DoWhileStatement))
                {
                    return true;
                }

                if (n is CatchClause)
                {
                    insideCatch = true;
                    return true;
                }

                return false;
            });

            return insideCatch ? (CatchClause)target : null;
        }

        public void WriteIdentifier(string name, bool script = true, bool colon = false)
        {
            var isValid = Helpers.IsValidIdentifier(name);

            if (isValid)
            {
                Write(name);
            }
            else
            {
                if (colon)
                {
                    WriteScript(name);
                }
                else if (Emitter.Output[Emitter.Output.Length - 1] == '.')
                {
                    --Emitter.Output.Length;
                    Write("[");
                    if (script)
                    {
                        WriteScript(name);
                    }
                    else
                    {
                        Write(name);
                    }

                    Write("]");
                }
            }
        }
    }
}