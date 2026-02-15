using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using System.Collections.Generic;
using System.Linq;

namespace H5.Translator
{
    public class ExpressionListBlock : AbstractEmitterBlock
    {
        public ExpressionListBlock(IEmitter emitter, IEnumerable<Expression> expressions, Expression paramArg, AstNode invocation, int openBracketPosition, bool newLine = false)
            : base(emitter, null)
        {
            Emitter = emitter;
            Expressions = expressions;
            ParamExpression = paramArg;
            InvocationExpression = invocation;
            OpenBracketPosition = openBracketPosition;
            NewLine = newLine;
        }

        public bool NewLine { get; set; }

        public int OpenBracketPosition { get; set; }

        public IEnumerable<Expression> Expressions { get; set; }

        public Expression ParamExpression { get; set; }

        public AstNode InvocationExpression { get; set; }

        public bool IgnoreExpandParams { get; set; }

        protected override void DoEmit()
        {
            var oldIsAssignment = Emitter.IsAssignment;
            var oldUnary = Emitter.IsUnaryAccessor;
            Emitter.IsAssignment = false;
            Emitter.IsUnaryAccessor = false;
            EmitExpressionList(Expressions, ParamExpression);
            Emitter.IsAssignment = oldIsAssignment;
            Emitter.IsUnaryAccessor = oldUnary;
        }

        protected virtual void EmitExpressionList(IEnumerable<Expression> expressions, Expression paramArg)
        {
            bool needComma = false;
            int count = Emitter.Writers.Count;
            bool wrapByBrackets = !IgnoreExpandParams;
            bool expandParams = false;
            bool isApply = false;

            if (paramArg != null && InvocationExpression != null && !IgnoreExpandParams)
            {
                if (Emitter.Resolver.ResolveNode(InvocationExpression) is CSharpInvocationResolveResult rr)
                {
                    expandParams = rr.Member.Attributes.Any(a => a.AttributeType.FullName == "H5.ExpandParamsAttribute");
                    wrapByBrackets = rr.IsExpandedForm && !expandParams;
                }
            }

            if (paramArg != null && expandParams)
            {
                var resolveResult = Emitter.Resolver.ResolveNode(paramArg);

                if (resolveResult.Type.Kind == TypeKind.Array && !(paramArg is ArrayCreateExpression) && expressions.Last() == paramArg)
                {
                    bool needConcat = expressions.Count() > 1;

                    if (InvocationExpression is ObjectCreateExpression)
                    {
                        if (needConcat)
                        {
                            Write("[");
                        }
                    }
                    else
                    {
                        var scope = "null";

                        if (InvocationExpression != null)
                        {
                            if (Emitter.Resolver.ResolveNode(InvocationExpression) is MemberResolveResult rr && !rr.Member.IsStatic && InvocationExpression is InvocationExpression)
                            {
                                var oldWriter = SaveWriter();
                                var sb = NewWriter();
                                var target = ((InvocationExpression)InvocationExpression).Target;

                                if (target is MemberReferenceExpression)
                                {
                                    target = ((MemberReferenceExpression)target).Target;
                                }
                                else if (target is IdentifierExpression)
                                {
                                    target = new ThisReferenceExpression();
                                }

                                target.AcceptVisitor(Emitter);
                                scope = sb.ToString();
                                RestoreWriter(oldWriter);
                            }
                        }

                        var pos = OpenBracketPosition;

                        if (pos > -1)
                        {
                            Emitter.Output.Insert(pos, "." + JS.Funcs.APPLY);
                            pos += 7;

                            Emitter.Output.Insert(pos, scope + ", " + (needConcat ? "[" : ""));
                        }
                    }

                    isApply = needConcat;
                }
            }

            if (NewLine)
            {
                WriteNewLine();
                Indent();
            }

            foreach (var expr in expressions)
            {
                if (expr == null)
                {
                    continue;
                }

                var expressionToEmit = expr;

                if (expr is NamedArgumentExpression namedArg)
                {
                    expressionToEmit = namedArg.Expression;
                }

                Emitter.Translator.EmitNode = expressionToEmit;
                var isParamsArg = expressionToEmit == paramArg;

                if (needComma && !(isParamsArg && isApply))
                {
                    WriteComma();
                    if (NewLine)
                    {
                        WriteNewLine();
                        WriteIndent();
                    }
                }

                needComma = true;

                if (expressionToEmit is DirectionExpression directExpr)
                {
                    var resolveResult = Emitter.Resolver.ResolveNode(expressionToEmit);

                    if (resolveResult is ByReferenceResolveResult byReferenceResolveResult && !(byReferenceResolveResult.ElementResult is LocalResolveResult))
                    {
                        if (byReferenceResolveResult.ElementResult is MemberResolveResult mr && mr.Member.FullName == "H5.Ref.Value" && directExpr.Expression is MemberReferenceExpression mre)
                        {
                            mre.Target.AcceptVisitor(Emitter);
                        }
                        else
                        {
                            Write(JS.Funcs.H5_REF + "(");

                            Emitter.IsRefArg = true;
                            expressionToEmit.AcceptVisitor(Emitter);
                            Emitter.IsRefArg = false;

                            if (Emitter.Writers.Count != count)
                            {
                                PopWriter();
                                count = Emitter.Writers.Count;
                            }

                            Write(")");
                        }

                        continue;
                    }
                }

                if (isParamsArg)
                {
                    if (wrapByBrackets)
                    {
                        WriteOpenBracket();
                    }
                    else if (isApply)
                    {
                        Write("].concat(");
                    }
                }

                int pos = Emitter.Output.Length;

                if (expandParams && isParamsArg && expressionToEmit is ArrayCreateExpression)
                {
                    new ExpressionListBlock(Emitter, ((ArrayCreateExpression)expressionToEmit).Initializer.Elements, null, null, 0).DoEmit();
                }
                else
                {
                    expressionToEmit.AcceptVisitor(Emitter);

                    if (isParamsArg && isApply)
                    {
                        Write(")");
                    }
                }

                if (Emitter.Writers.Count != count)
                {
                    PopWriter();
                    count = Emitter.Writers.Count;
                }

                if (expressionToEmit is AssignmentExpression)
                {
                    Helpers.CheckValueTypeClone(Emitter.Resolver.ResolveNode(expressionToEmit), expressionToEmit, this, pos);
                }
            }

            if (NewLine)
            {
                WriteNewLine();
                Outdent();
            }

            if (wrapByBrackets && paramArg != null)
            {
                WriteCloseBracket();
            }
        }
    }
}