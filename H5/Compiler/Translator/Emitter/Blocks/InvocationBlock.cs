using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace H5.Translator
{
    public class InvocationBlock : ConversionBlock
    {
        public InvocationBlock(IEmitter emitter, InvocationExpression invocationExpression)
            : base(emitter, invocationExpression)
        {
            Emitter = emitter;
            InvocationExpression = invocationExpression;
        }

        public InvocationExpression InvocationExpression { get; set; }

        protected override Expression GetExpression()
        {
            return InvocationExpression;
        }

        protected override void EmitConversionExpression()
        {
            VisitInvocationExpression();
        }

        protected virtual bool IsEmptyPartialInvoking(IMethod method)
        {
            return method != null && method.IsPartial && !method.HasBody;
        }

        protected void WriteThisExtension(Expression target)
        {
            if (target.HasChildren)
            {
                var first = target.Children.ElementAt(0);

                if (first is Expression expression)
                {
                    expression.AcceptVisitor(Emitter);
                }
                else
                {
                    WriteThis();
                }
            }
        }

        public static bool IsConditionallyRemoved(InvocationExpression invocationExpression, IEntity entity)
        {
            if (entity == null)
            {
                return false;
            }
            var result = new List<string>();
            foreach (var a in entity.Attributes)
            {
                var type = a.AttributeType.GetDefinition();
                if (type != null && type.FullName.Equals("System.Diagnostics.ConditionalAttribute", StringComparison.Ordinal))
                {
                    if (a.PositionalArguments.Count > 0)
                    {
                        if (a.PositionalArguments[0].ConstantValue is string symbol)
                        {
                            result.Add(symbol);
                        }
                    }
                }
            }

            if (result.Count > 0)
            {
                var syntaxTree = invocationExpression.GetParent<SyntaxTree>();
                if (syntaxTree != null)
                {
                    return !result.Intersect(syntaxTree.ConditionalSymbols).Any();
                }
            }

            return false;
        }

        protected void VisitInvocationExpression()
        {
            InvocationExpression invocationExpression = InvocationExpression;
            int pos = Emitter.Output.Length;

            if (Emitter.IsForbiddenInvocation(invocationExpression))
            {
                throw new EmitterException(invocationExpression, "This method cannot be invoked directly");
            }

            var oldValue = Emitter.ReplaceAwaiterByVar;
            var oldAsyncExpressionHandling = Emitter.AsyncExpressionHandling;

            if (Emitter.IsAsync && !Emitter.AsyncExpressionHandling)
            {
                WriteAwaiters(invocationExpression);
                Emitter.ReplaceAwaiterByVar = true;
                Emitter.AsyncExpressionHandling = true;
            }

            Tuple<bool, bool, string> inlineInfo = Emitter.GetInlineCode(invocationExpression);
            var argsInfo = new ArgumentsInfo(Emitter, invocationExpression);

            var argsExpressions = argsInfo.ArgumentsExpressions;
            var paramsArg = argsInfo.ParamsExpression;

            var targetResolve = Emitter.Resolver.ResolveNode(invocationExpression);
            var csharpInvocation = targetResolve as CSharpInvocationResolveResult;
            MemberReferenceExpression targetMember = invocationExpression.Target as MemberReferenceExpression;
            bool isObjectLiteral = csharpInvocation != null && csharpInvocation.Member.DeclaringTypeDefinition != null ? Emitter.Validator.IsObjectLiteral(csharpInvocation.Member.DeclaringTypeDefinition) : false;

            var interceptor = Emitter.Plugins.OnInvocation(this, InvocationExpression, targetResolve as InvocationResolveResult);

            if (interceptor.Cancel)
            {
                Emitter.SkipSemiColon = true;
                Emitter.ReplaceAwaiterByVar = oldValue;
                Emitter.AsyncExpressionHandling = oldAsyncExpressionHandling;
                return;
            }

            if (!string.IsNullOrEmpty(interceptor.Replacement))
            {
                Write(interceptor.Replacement);
                Emitter.ReplaceAwaiterByVar = oldValue;
                Emitter.AsyncExpressionHandling = oldAsyncExpressionHandling;
                return;
            }

            if (inlineInfo != null)
            {
                bool isStaticMethod = inlineInfo.Item1;
                bool isInlineMethod = inlineInfo.Item2;
                string inlineScript = inlineInfo.Item3;

                if (isInlineMethod)
                {
                    if (invocationExpression.Arguments.Count > 0)
                    {
                        var code = invocationExpression.Arguments.First();

                        if (!(code is PrimitiveExpression inlineExpression))
                        {
                            throw new EmitterException(invocationExpression, "Only primitive expression can be inlined");
                        }

                        string value = inlineExpression.Value.ToString().Trim();

                        if (value.Length > 0)
                        {
                            value = InlineArgumentsBlock.ReplaceInlineArgs(this, inlineExpression.Value.ToString(), invocationExpression.Arguments.Skip(1).ToArray());
                            Write(value);

                            value = value.Trim();
                            if (value[value.Length - 1] == ';' || value.EndsWith("*/", StringComparison.InvariantCulture) || value.StartsWith("//"))
                            {
                                Emitter.SkipSemiColon = true;
                                WriteNewLine();
                            }
                        }
                        else
                        {
                            // Empty string, emit nothing.
                            Emitter.SkipSemiColon = true;
                        }

                        Emitter.ReplaceAwaiterByVar = oldValue;
                        Emitter.AsyncExpressionHandling = oldAsyncExpressionHandling;

                        return;
                    }
                }
                else
                {
                    bool isBase = invocationExpression.Target is MemberReferenceExpression targetMemberRef && targetMemberRef.Target is BaseReferenceExpression;

                    if (!String.IsNullOrEmpty(inlineScript) && (isBase || invocationExpression.Target is IdentifierExpression))
                    {
                        argsInfo.ThisArgument = "this";
                        bool noThis = !Helpers.HasThis(inlineScript);

                        if (inlineScript.StartsWith("<self>"))
                        {
                            noThis = false;
                            inlineScript = inlineScript.Substring(6);
                        }

                        if (!noThis)
                        {
                            Emitter.ThisRefCounter++;
                        }

                        if (!isStaticMethod && noThis)
                        {
                            WriteThis();
                            WriteDot();
                        }

                        new InlineArgumentsBlock(Emitter, argsInfo, inlineScript).Emit();
                        Emitter.ReplaceAwaiterByVar = oldValue;
                        Emitter.AsyncExpressionHandling = oldAsyncExpressionHandling;

                        return;
                    }
                }
            }

            if (targetMember != null || isObjectLiteral)
            {
                var member = targetMember != null ? Emitter.Resolver.ResolveNode(targetMember.Target) : null;

                if (targetResolve != null)
                {
                    InvocationResolveResult invocationResult;
                    bool isExtensionMethodInvocation = false;
                    if (csharpInvocation != null)
                    {
                        if (member != null && member.Type.Kind == TypeKind.Delegate && (/*csharpInvocation.Member.Name == "Invoke" || */csharpInvocation.Member.Name == "BeginInvoke" || csharpInvocation.Member.Name == "EndInvoke") && !csharpInvocation.IsExtensionMethodInvocation)
                        {
                            throw new EmitterException(invocationExpression, "Delegate's 'Invoke' methods are not supported. Please use direct delegate invoke.");
                        }

                        if (csharpInvocation.IsExtensionMethodInvocation)
                        {
                            invocationResult = csharpInvocation;
                            isExtensionMethodInvocation = true;
                            if (invocationResult.Member is IMethod resolvedMethod && resolvedMethod.IsExtensionMethod)
                            {
                                string inline = Emitter.GetInline(resolvedMethod);
                                bool isNative = IsNativeMethod(resolvedMethod);

                                if (string.IsNullOrWhiteSpace(inline) && isNative)
                                {
                                    invocationResult = null;
                                }
                            }
                        }
                        else
                        {
                            invocationResult = null;
                        }

                        if (IsEmptyPartialInvoking(csharpInvocation.Member as IMethod) || IsConditionallyRemoved(invocationExpression, csharpInvocation.Member))
                        {
                            Emitter.SkipSemiColon = true;
                            Emitter.ReplaceAwaiterByVar = oldValue;
                            Emitter.AsyncExpressionHandling = oldAsyncExpressionHandling;

                            return;
                        }
                    }
                    else
                    {
                        invocationResult = targetResolve as InvocationResolveResult;

                        if (invocationResult != null && (IsEmptyPartialInvoking(invocationResult.Member as IMethod) || IsConditionallyRemoved(invocationExpression, invocationResult.Member)))
                        {
                            Emitter.SkipSemiColon = true;
                            Emitter.ReplaceAwaiterByVar = oldValue;
                            Emitter.AsyncExpressionHandling = oldAsyncExpressionHandling;

                            return;
                        }
                    }

                    if (invocationResult == null)
                    {
                        invocationResult = Emitter.Resolver.ResolveNode(invocationExpression) as InvocationResolveResult;
                    }

                    if (invocationResult != null)
                    {
                        if (invocationResult.Member is IMethod resolvedMethod && (resolvedMethod.IsExtensionMethod || isObjectLiteral))
                        {
                            string inline = Emitter.GetInline(resolvedMethod);
                            bool isNative = IsNativeMethod(resolvedMethod);

                            if (isExtensionMethodInvocation || isObjectLiteral)
                            {
                                if (!string.IsNullOrWhiteSpace(inline))
                                {
                                    Write("");
                                    StringBuilder savedBuilder = Emitter.Output;
                                    Emitter.Output = new StringBuilder();
                                    WriteThisExtension(invocationExpression.Target);
                                    argsInfo.ThisArgument = Emitter.Output.ToString();
                                    Emitter.Output = savedBuilder;
                                    new InlineArgumentsBlock(Emitter, argsInfo, inline).Emit();
                                }
                                else if (!isNative)
                                {
                                    var overloads = OverloadsCollection.Create(Emitter, resolvedMethod);

                                    if (isObjectLiteral && !resolvedMethod.IsStatic && resolvedMethod.DeclaringType.Kind == TypeKind.Interface)
                                    {
                                        Write("H5.getType(");
                                        WriteThisExtension(invocationExpression.Target);
                                        Write(").");
                                    }
                                    else
                                    {
                                        string name = H5Types.ToJsName(resolvedMethod.DeclaringType, Emitter, ignoreLiteralName: false) + ".";
                                        Write(name);
                                    }

                                    if (isObjectLiteral && !resolvedMethod.IsStatic)
                                    {
                                        Write(JS.Fields.PROTOTYPE + "." + overloads.GetOverloadName() + "." + JS.Funcs.CALL);
                                    }
                                    else
                                    {
                                        Write(overloads.GetOverloadName());
                                    }

                                    var isIgnoreClass = resolvedMethod.DeclaringTypeDefinition != null && Emitter.Validator.IsExternalType(resolvedMethod.DeclaringTypeDefinition);
                                    int openPos = Emitter.Output.Length;
                                    WriteOpenParentheses();

                                    Emitter.Comma = false;

                                    if (isObjectLiteral && !resolvedMethod.IsStatic)
                                    {
                                        WriteThisExtension(invocationExpression.Target);
                                        Emitter.Comma = true;
                                    }

                                    if (!isIgnoreClass && !Helpers.IsIgnoreGeneric(resolvedMethod, Emitter) && argsInfo.HasTypeArguments)
                                    {
                                        EnsureComma(false);
                                        new TypeExpressionListBlock(Emitter, argsInfo.TypeArguments).Emit();
                                        Emitter.Comma = true;
                                    }

                                    if (!isObjectLiteral && resolvedMethod.IsStatic)
                                    {
                                        EnsureComma(false);
                                        WriteThisExtension(invocationExpression.Target);
                                        Emitter.Comma = true;
                                    }

                                    if (invocationExpression.Arguments.Count > 0)
                                    {
                                        EnsureComma(false);
                                    }

                                    new ExpressionListBlock(Emitter, argsExpressions, paramsArg, invocationExpression, openPos).Emit();

                                    WriteCloseParentheses();
                                }

                                if (!string.IsNullOrWhiteSpace(inline) || !isNative)
                                {
                                    Emitter.ReplaceAwaiterByVar = oldValue;
                                    Emitter.AsyncExpressionHandling = oldAsyncExpressionHandling;

                                    return;
                                }
                            }
                            else if (isNative)
                            {
                                if (!string.IsNullOrWhiteSpace(inline))
                                {
                                    Write("");
                                    StringBuilder savedBuilder = Emitter.Output;
                                    Emitter.Output = new StringBuilder();
                                    WriteThisExtension(invocationExpression.Target);
                                    argsInfo.ThisArgument = Emitter.Output.ToString();
                                    Emitter.Output = savedBuilder;
                                    new InlineArgumentsBlock(Emitter, argsInfo, inline).Emit();
                                }
                                else
                                {
                                    argsExpressions.First().AcceptVisitor(Emitter);
                                    WriteDot();
                                    string name = Emitter.GetEntityName(resolvedMethod);
                                    Write(name);
                                    int openPos = Emitter.Output.Length;
                                    WriteOpenParentheses();
                                    new ExpressionListBlock(Emitter, argsExpressions.Skip(1), paramsArg, invocationExpression, openPos).Emit();
                                    WriteCloseParentheses();
                                }

                                Emitter.ReplaceAwaiterByVar = oldValue;
                                Emitter.AsyncExpressionHandling = oldAsyncExpressionHandling;

                                return;
                            }
                        }
                    }
                }
            }

            var proto = false;
            if (targetMember != null && targetMember.Target is BaseReferenceExpression)
            {
                if (Emitter.Resolver.ResolveNode(targetMember) is MemberResolveResult rr)
                {
                    proto = rr.IsVirtualCall;

                    /*var method = rr.Member as IMethod;
                    if (method != null && method.IsVirtual)
                    {
                        proto = true;
                    }
                    else
                    {
                        var prop = rr.Member as IProperty;

                        if (prop != null && prop.IsVirtual)
                        {
                            proto = true;
                        }
                    }*/
                }
            }

            if (proto)
            {
                var baseType = Emitter.GetBaseMethodOwnerTypeDefinition(targetMember.MemberName, targetMember.TypeArguments.Count);

                bool isIgnore = Emitter.Validator.IsExternalType(baseType);

                bool needComma = false;

                var resolveResult = Emitter.Resolver.ResolveNode(targetMember);

                string name = null;

                if (Emitter.TypeInfo.GetBaseTypes(Emitter).Any())
                {
                    name = H5Types.ToJsName(Emitter.TypeInfo.GetBaseClass(Emitter), Emitter);
                }
                else
                {
                    name = H5Types.ToJsName(baseType, Emitter);
                }

                string baseMethod;
                bool isIgnoreGeneric = false;
                if (resolveResult is MemberResolveResult memberResult)
                {
                    baseMethod = OverloadsCollection.Create(Emitter, memberResult.Member).GetOverloadName();
                    isIgnoreGeneric = Helpers.IsIgnoreGeneric(memberResult.Member, Emitter);
                }
                else
                {
                    baseMethod = targetMember.MemberName;
                    baseMethod = Object.Net.Utilities.StringUtils.ToLowerCamelCase(baseMethod);
                }

                Write(name, "." + JS.Fields.PROTOTYPE + ".", baseMethod);

                WriteCall();
                WriteOpenParentheses();
                WriteThis();
                Emitter.Comma = true;
                if (!isIgnore && !isIgnoreGeneric && argsInfo.HasTypeArguments)
                {
                    new TypeExpressionListBlock(Emitter, argsInfo.TypeArguments).Emit();
                }

                needComma = false;

                foreach (var arg in argsExpressions)
                {
                    if (arg == null)
                    {
                        continue;
                    }

                    EnsureComma(false);

                    if (needComma)
                    {
                        WriteComma();
                    }

                    needComma = true;
                    arg.AcceptVisitor(Emitter);
                }
                Emitter.Comma = false;
                WriteCloseParentheses();
            }
            else
            {
                IMethod method = null;

                if (Emitter.Resolver.ResolveNode(invocationExpression) is DynamicInvocationResolveResult dynamicResolveResult)
                {
                    if (dynamicResolveResult.Target is MethodGroupResolveResult group && group.Methods.Count() > 1)
                    {
                        method = group.Methods.FirstOrDefault(m =>
                        {
                            if (dynamicResolveResult.Arguments.Count != m.Parameters.Count)
                            {
                                return false;
                            }

                            for (int i = 0; i < m.Parameters.Count; i++)
                            {
                                var argType = dynamicResolveResult.Arguments[i].Type;

                                if (argType.Kind == TypeKind.Dynamic)
                                {
                                    argType = Emitter.Resolver.Compilation.FindType(TypeCode.Object);
                                }

                                if (!m.Parameters[i].Type.Equals(argType))
                                {
                                    return false;
                                }
                            }

                            return true;
                        });

                        if (method == null)
                        {
                            throw new EmitterException(invocationExpression, Constants.Messages.Exceptions.DYNAMIC_INVOCATION_TOO_MANY_OVERLOADS);
                        }
                    }
                }
                else
                {
                    var targetResolveResult = Emitter.Resolver.ResolveNode(invocationExpression.Target);

                    if (targetResolveResult is MemberResolveResult invocationResolveResult)
                    {
                        method = invocationResolveResult.Member as IMethod;
                    }
                }

                if (IsEmptyPartialInvoking(method) || IsConditionallyRemoved(invocationExpression, method))
                {
                    Emitter.SkipSemiColon = true;
                    Emitter.ReplaceAwaiterByVar = oldValue;
                    Emitter.AsyncExpressionHandling = oldAsyncExpressionHandling;
                    return;
                }

                bool isIgnore = method != null && method.DeclaringTypeDefinition != null && Emitter.Validator.IsExternalType(method.DeclaringTypeDefinition);

                bool needExpand = false;
                if (method != null)
                {
                    string paramsName = null;

                    var paramsParam = method.Parameters.FirstOrDefault(p => p.IsParams);
                    if (paramsParam != null)
                    {
                        paramsName = paramsParam.Name;
                    }

                    if (paramsName != null)
                    {
                        if (csharpInvocation != null && !csharpInvocation.IsExpandedForm)
                        {
                            needExpand = true;
                        }
                    }
                }

                int count = Emitter.Writers.Count;
                invocationExpression.Target.AcceptVisitor(Emitter);

                if (Emitter.Writers.Count > count)
                {
                    var writer = Emitter.Writers.Pop();

                    if (method != null && method.IsExtensionMethod)
                    {
                        StringBuilder savedBuilder = Emitter.Output;
                        Emitter.Output = new StringBuilder();
                        WriteThisExtension(invocationExpression.Target);
                        argsInfo.ThisArgument = Emitter.Output.ToString();
                        Emitter.Output = savedBuilder;
                    }
                    else if (writer.ThisArg != null)
                    {
                        argsInfo.ThisArgument = writer.ThisArg;
                    }

                    new InlineArgumentsBlock(Emitter, argsInfo, writer.InlineCode) { IgnoreRange = writer.IgnoreRange }.Emit();
                    var result = Emitter.Output.ToString();
                    Emitter.Output = writer.Output;
                    Emitter.IsNewLine = writer.IsNewLine;
                    Write(result);

                    if (writer.Callback != null)
                    {
                        writer.Callback.Invoke();
                    }
                }
                else
                {
                    if (needExpand && isIgnore)
                    {
                        Write("." + JS.Funcs.APPLY);
                    }
                    int openPos = Emitter.Output.Length;
                    WriteOpenParentheses();

                    bool isIgnoreGeneric = false;

                    if (targetResolve is InvocationResolveResult invocationResult)
                    {
                        isIgnoreGeneric = Helpers.IsIgnoreGeneric(invocationResult.Member, Emitter);
                    }

                    bool isWrapRest = false;

                    if (needExpand && isIgnore)
                    {
                        StringBuilder savedBuilder = Emitter.Output;
                        Emitter.Output = new StringBuilder();
                        WriteThisExtension(invocationExpression.Target);
                        var thisArg = Emitter.Output.ToString();
                        Emitter.Output = savedBuilder;

                        Write(thisArg);

                        Emitter.Comma = true;

                        if (!isIgnore && !isIgnoreGeneric && argsInfo.HasTypeArguments)
                        {
                            new TypeExpressionListBlock(Emitter, argsInfo.TypeArguments).Emit();
                        }

                        EnsureComma(false);

                        if (argsExpressions.Length > 1)
                        {
                            WriteOpenBracket();
                            var elb = new ExpressionListBlock(Emitter, argsExpressions.Take(argsExpressions.Length - 1).ToArray(), paramsArg, invocationExpression, openPos);
                            elb.IgnoreExpandParams = true;
                            elb.Emit();
                            WriteCloseBracket();
                            Write(".concat(");
                            elb = new ExpressionListBlock(Emitter, new Expression[] { argsExpressions[argsExpressions.Length - 1] }, paramsArg, invocationExpression, openPos);
                            elb.IgnoreExpandParams = true;
                            elb.Emit();
                            Write(")");
                        }
                        else
                        {
                            new ExpressionListBlock(Emitter, argsExpressions, paramsArg, invocationExpression, -1).Emit();
                        }
                    }
                    else
                    {
                        if (method != null && method.Attributes.Any(a => a.AttributeType.FullName == "H5.WrapRestAttribute"))
                        {
                            isWrapRest = true;
                        }

                        Emitter.Comma = false;
                        if (!isIgnore && !isIgnoreGeneric && argsInfo.HasTypeArguments)
                        {
                            new TypeExpressionListBlock(Emitter, argsInfo.TypeArguments).Emit();
                        }

                        if (invocationExpression.Arguments.Count > 0 || argsExpressions.Length > 0 && !argsExpressions.All(expr => expr == null))
                        {
                            EnsureComma(false);
                        }

                        new ExpressionListBlock(Emitter, argsExpressions, paramsArg, invocationExpression, openPos).Emit();
                    }


                    if (isWrapRest)
                    {
                        EnsureComma(false);
                        Write("H5.fn.bind(this, function () ");
                        BeginBlock();
                        Emitter.WrapRestCounter++;
                        Emitter.SkipSemiColon = true;
                    } else
                    {
                        Emitter.Comma = false;
                        WriteCloseParentheses();
                    }
                }
            }

            if (targetResolve is InvocationResolveResult irr && irr.Member.MemberDefinition != null && irr.Member.MemberDefinition.ReturnType.Kind == TypeKind.TypeParameter)
            {
                Helpers.CheckValueTypeClone(Emitter.Resolver.ResolveNode(invocationExpression), invocationExpression, this, pos);
            }

            Emitter.ReplaceAwaiterByVar = oldValue;
            Emitter.AsyncExpressionHandling = oldAsyncExpressionHandling;
        }

        private bool IsNativeMethod(IMethod resolvedMethod)
        {
            return resolvedMethod.DeclaringTypeDefinition != null &&
                   Emitter.Validator.IsExternalType(resolvedMethod.DeclaringTypeDefinition);
        }
    }
}