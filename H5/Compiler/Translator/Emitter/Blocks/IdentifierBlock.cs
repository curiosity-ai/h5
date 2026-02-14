using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.TypeSystem.Implementation;
using System.Text;

namespace H5.Translator
{
    public class IdentifierBlock : ConversionBlock
    {
        private bool isRefArg;

        public IdentifierBlock(IEmitter emitter, IdentifierExpression identifierExpression)
            : base(emitter, identifierExpression)
        {
            Emitter = emitter;
            IdentifierExpression = identifierExpression;
        }

        public IdentifierExpression IdentifierExpression { get; set; }

        protected override Expression GetExpression()
        {
            return IdentifierExpression;
        }

        protected override void EmitConversionExpression()
        {
            VisitIdentifierExpression();
        }

        protected void VisitIdentifierExpression()
        {
            IdentifierExpression identifierExpression = IdentifierExpression;
            int pos = Emitter.Output.Length;
            ResolveResult resolveResult = null;
            isRefArg = Emitter.IsRefArg;
            Emitter.IsRefArg = false;

            resolveResult = Emitter.Resolver.ResolveNode(identifierExpression);

            var id = identifierExpression.Identifier;

            var isResolved = resolveResult != null && !(resolveResult is ErrorResolveResult);
            var memberResult = resolveResult as MemberResolveResult;

            if (Emitter.Locals != null && Emitter.Locals.ContainsKey(id) && resolveResult is LocalResolveResult)
            {
                var lrr = (LocalResolveResult)resolveResult;
                string name;

                if (Emitter.LocalsMap != null && Emitter.LocalsMap.ContainsKey(lrr.Variable) && !(identifierExpression.Parent is DirectionExpression))
                {
                    name = Emitter.LocalsMap[lrr.Variable];
                }
                else if (Emitter.LocalsNamesMap != null && Emitter.LocalsNamesMap.ContainsKey(id))
                {
                    name = Emitter.LocalsNamesMap[id];
                }
                else
                {
                    name = id;
                }

                Write(name);

                if (Emitter.RefLocals != null && Emitter.RefLocals.Contains(name) && !(identifierExpression.Parent is DirectionExpression))
                {
                    Write(".v");
                }

                Helpers.CheckValueTypeClone(resolveResult, identifierExpression, this, pos);

                return;
            }

            if (resolveResult is TypeResolveResult)
            {
                Write(H5Types.ToJsName(resolveResult.Type, Emitter));
                /*if (this.Emitter.Validator.IsExternalType(resolveResult.Type.GetDefinition()) || resolveResult.Type.Kind == TypeKind.Enum)
                {
                    this.Write(H5Types.ToJsName(resolveResult.Type, this.Emitter));
                }
                else
                {
                    this.Write("H5.get(" + H5Types.ToJsName(resolveResult.Type, this.Emitter) + ")");
                }*/

                return;
            }

            string inlineCode = memberResult != null ? Emitter.GetInline(memberResult.Member) : null;

            var isInvoke = identifierExpression.Parent is InvocationExpression && (((InvocationExpression)(identifierExpression.Parent)).Target == identifierExpression);
            if (memberResult != null && memberResult.Member is IMethod && isInvoke)
            {
                if (Emitter.Resolver.ResolveNode(identifierExpression.Parent) is CSharpInvocationResolveResult i_rr && !i_rr.IsExpandedForm)
                {
                    var tpl = Emitter.GetAttribute(memberResult.Member.Attributes, JS.NS.H5 + ".TemplateAttribute");

                    if (tpl != null && tpl.PositionalArguments.Count == 2)
                    {
                        inlineCode = tpl.PositionalArguments[1].ConstantValue.ToString();
                    }
                }
            }

            if (string.IsNullOrEmpty(inlineCode) && memberResult != null &&
                memberResult.Member is IMethod &&
                !(memberResult is InvocationResolveResult) &&
                !(
                    identifierExpression.Parent is InvocationExpression &&
                    identifierExpression.NextSibling != null &&
                    identifierExpression.NextSibling.Role is TokenRole &&
                    ((TokenRole)identifierExpression.NextSibling.Role).Token == "("
                    )
                )
            {
                if (!(identifierExpression.Parent is InvocationExpression parentInvocation) || parentInvocation.Target != identifierExpression)
                {
                    var method = (IMethod)memberResult.Member;
                    if (method.TypeArguments.Count > 0)
                    {
                        inlineCode = MemberReferenceBlock.GenerateInlineForMethodReference(method, Emitter);
                    }
                }
            }

            bool hasInline = !string.IsNullOrEmpty(inlineCode);
            inlineCode = hasInline ? Helpers.ConvertTokens(Emitter, inlineCode, memberResult.Member) : inlineCode;
            bool hasThis = hasInline && Helpers.HasThis(inlineCode);

            if (hasInline && inlineCode.StartsWith("<self>"))
            {
                hasThis = true;
                inlineCode = inlineCode.Substring(6);
            }

            if (hasThis)
            {
                Emitter.ThisRefCounter++;
                Write("");
                var oldBuilder = Emitter.Output;
                Emitter.Output = new StringBuilder();

                if (memberResult.Member.IsStatic)
                {
                    Write(H5Types.ToJsName(memberResult.Member.DeclaringType, Emitter, ignoreLiteralName: false));
                    /*if (!this.Emitter.Validator.IsExternalType(memberResult.Member.DeclaringTypeDefinition) && memberResult.Member.DeclaringTypeDefinition.Kind != TypeKind.Enum)
                    {
                        this.Write("(H5.get(" + H5Types.ToJsName(memberResult.Member.DeclaringType, this.Emitter) + "))");
                    }
                    else
                    {
                        this.Write(H5Types.ToJsName(memberResult.Member.DeclaringType, this.Emitter));
                    }*/
                }
                else
                {
                    WriteThis();
                }

                var oldInline = inlineCode;
                var thisArg = Emitter.Output.ToString();
                int thisIndex = inlineCode.IndexOf("{this}");
                inlineCode = inlineCode.Replace("{this}", thisArg);
                Emitter.Output = oldBuilder;

                int[] range = null;

                if (thisIndex > -1)
                {
                    range = new[] { thisIndex, thisIndex + thisArg.Length };
                }

                if (resolveResult is InvocationResolveResult)
                {
                    PushWriter(inlineCode, null, thisArg, range);
                }
                else
                {
                    if (memberResult.Member is IMethod)
                    {
                        ResolveResult targetrr = null;
                        if (memberResult.Member.IsStatic)
                        {
                            targetrr = new TypeResolveResult(memberResult.Member.DeclaringType);
                        }

                        new InlineArgumentsBlock(Emitter, new ArgumentsInfo(Emitter, IdentifierExpression, resolveResult), oldInline, (IMethod)memberResult.Member, targetrr).EmitFunctionReference();
                    }
                    else if (memberResult != null && memberResult.Member is IField && inlineCode.Contains("{0}"))
                    {
                        PushWriter(inlineCode, null, thisArg, range);
                    }
                    else if (InlineArgumentsBlock.FormatArgRegex.IsMatch(inlineCode))
                    {
                        PushWriter(inlineCode, null, thisArg, range);
                    }
                    else
                    {
                        Write(inlineCode);
                    }
                }

                return;
            }

            if (hasInline)
            {
                if (!memberResult.Member.IsStatic)
                {
                    inlineCode = "this." + inlineCode;
                }

                if (resolveResult is InvocationResolveResult)
                {
                    PushWriter(inlineCode);
                }
                else
                {
                    if (memberResult.Member is IMethod)
                    {
                        ResolveResult targetrr = null;
                        if (memberResult.Member.IsStatic)
                        {
                            targetrr = new TypeResolveResult(memberResult.Member.DeclaringType);
                        }

                        new InlineArgumentsBlock(Emitter, new ArgumentsInfo(Emitter, IdentifierExpression, resolveResult), inlineCode, (IMethod)memberResult.Member, targetrr).EmitFunctionReference();
                    }
                    else if (InlineArgumentsBlock.FormatArgRegex.IsMatch(inlineCode))
                    {
                        PushWriter(inlineCode);
                    }
                    else
                    {
                        Write(inlineCode);
                    }
                }

                return;
            }

            string appendAdditionalCode = null;
            if (memberResult != null &&
                        memberResult.Member is IMethod &&
                        !(memberResult is InvocationResolveResult) &&
                        !(
                        identifierExpression.Parent is InvocationExpression &&
                        identifierExpression.NextSibling != null &&
                        identifierExpression.NextSibling.Role is TokenRole &&
                        ((TokenRole)identifierExpression.NextSibling.Role).Token == "("
                        )
                )
            {
                if (!(identifierExpression.Parent is InvocationExpression parentInvocation) || parentInvocation.Target != identifierExpression)
                {
                    if (!string.IsNullOrEmpty(inlineCode))
                    {
                        ResolveResult targetrr = null;
                        if (memberResult.Member.IsStatic)
                        {
                            targetrr = new TypeResolveResult(memberResult.Member.DeclaringType);
                        }

                        new InlineArgumentsBlock(Emitter,
                                new ArgumentsInfo(Emitter, identifierExpression, resolveResult), inlineCode,
                                (IMethod)memberResult.Member, targetrr).EmitFunctionReference();
                    }
                    else
                    {
                        var resolvedMethod = (IMethod)memberResult.Member;
                        bool isStatic = resolvedMethod != null && resolvedMethod.IsStatic;

                        if (!isStatic)
                        {
                            var isExtensionMethod = resolvedMethod.IsExtensionMethod;
                            Write(isExtensionMethod ? JS.Funcs.H5_BIND_SCOPE : JS.Funcs.H5_CACHE_BIND);
                            WriteOpenParentheses();
                            WriteThis();
                            Write(", ");
                            appendAdditionalCode = ")";
                        }
                    }
                }
            }

            if (memberResult != null && memberResult.Member.SymbolKind == SymbolKind.Field && Emitter.IsMemberConst(memberResult.Member) && Emitter.IsInlineConst(memberResult.Member))
            {
                WriteScript(memberResult.ConstantValue);
                return;
            }

            if (memberResult != null && memberResult.Member.SymbolKind == SymbolKind.Property && memberResult.TargetResult.Type.Kind != TypeKind.Anonymous)
            {
                bool isStatement = false;
                string valueVar = null;

                if (Emitter.IsUnaryAccessor)
                {
                    isStatement = identifierExpression.Parent is UnaryOperatorExpression && identifierExpression.Parent.Parent is ExpressionStatement;

                    if (NullableType.IsNullable(memberResult.Type))
                    {
                        isStatement = false;
                    }

                    if (!isStatement)
                    {
                        WriteOpenParentheses();

                        valueVar = GetTempVarName();

                        Write(valueVar);
                        Write(" = ");
                    }
                }

                WriteTarget(memberResult);

                if (!string.IsNullOrWhiteSpace(inlineCode))
                {
                    //this.Write(inlineCode);
                    if (resolveResult is InvocationResolveResult || (memberResult.Member.SymbolKind == SymbolKind.Property && Emitter.IsAssignment))
                    {
                        PushWriter(inlineCode);
                    }
                    else
                    {
                        Write(inlineCode);
                    }
                }
                else if (memberResult.Member is IProperty)
                {
                    var name = Helpers.GetPropertyRef(memberResult.Member, Emitter);

                    WriteIdentifier(name);
                }
                else if (!Emitter.IsAssignment)
                {
                    if (Emitter.IsUnaryAccessor)
                    {
                        bool isDecimal = Helpers.IsDecimalType(memberResult.Member.ReturnType, Emitter.Resolver);
                        bool isLong = Helpers.Is64Type(memberResult.Member.ReturnType, Emitter.Resolver);
                        bool isNullable = NullableType.IsNullable(memberResult.Member.ReturnType);
                        if (isStatement)
                        {
                            Write(Helpers.GetPropertyRef(memberResult.Member, Emitter, true));
                            WriteOpenParentheses();

                            if (isDecimal || isLong)
                            {
                                if (isNullable)
                                {
                                    Write(JS.Types.SYSTEM_NULLABLE + "." + JS.Funcs.Math.LIFT1);
                                    WriteOpenParentheses();
                                    if (Emitter.UnaryOperatorType == UnaryOperatorType.Increment ||
                                        Emitter.UnaryOperatorType == UnaryOperatorType.PostIncrement)
                                    {
                                        WriteScript(JS.Funcs.Math.INC);
                                    }
                                    else
                                    {
                                        WriteScript(JS.Funcs.Math.DEC);
                                    }

                                    WriteComma();

                                    WriteTarget(memberResult);

                                    Write(Helpers.GetPropertyRef(memberResult.Member, Emitter, false));
                                    WriteOpenParentheses();
                                    WriteCloseParentheses();
                                    WriteCloseParentheses();
                                }
                                else
                                {
                                    WriteTarget(memberResult);
                                    Write(Helpers.GetPropertyRef(memberResult.Member, Emitter, false));
                                    WriteOpenParentheses();
                                    WriteCloseParentheses();
                                    WriteDot();

                                    if (Emitter.UnaryOperatorType == UnaryOperatorType.Increment ||
                                        Emitter.UnaryOperatorType == UnaryOperatorType.PostIncrement)
                                    {
                                        Write(JS.Funcs.Math.INC);
                                    }
                                    else
                                    {
                                        Write(JS.Funcs.Math.DEC);
                                    }

                                    WriteOpenParentheses();
                                    WriteCloseParentheses();
                                }
                            }
                            else
                            {
                                WriteTarget(memberResult);

                                Write(Helpers.GetPropertyRef(memberResult.Member, Emitter, false));
                                WriteOpenParentheses();
                                WriteCloseParentheses();

                                if (Emitter.UnaryOperatorType == UnaryOperatorType.Increment || Emitter.UnaryOperatorType == UnaryOperatorType.PostIncrement)
                                {
                                    Write("+");
                                }
                                else
                                {
                                    Write("-");
                                }

                                Write("1");
                            }

                            WriteCloseParentheses();
                        }
                        else
                        {
                            Write(Helpers.GetPropertyRef(memberResult.Member, Emitter, false));
                            WriteOpenParentheses();
                            WriteCloseParentheses();
                            WriteComma();

                            WriteTarget(memberResult);
                            Write(Helpers.GetPropertyRef(memberResult.Member, Emitter, true));
                            WriteOpenParentheses();

                            if (isDecimal || isLong)
                            {
                                if (isNullable)
                                {
                                    Write(JS.Types.SYSTEM_NULLABLE + "." + JS.Funcs.Math.LIFT1);
                                    WriteOpenParentheses();
                                    if (Emitter.UnaryOperatorType == UnaryOperatorType.Increment ||
                                        Emitter.UnaryOperatorType == UnaryOperatorType.PostIncrement)
                                    {
                                        WriteScript(JS.Funcs.Math.INC);
                                    }
                                    else
                                    {
                                        WriteScript(JS.Funcs.Math.DEC);
                                    }

                                    WriteComma();
                                    Write(valueVar);
                                    WriteCloseParentheses();
                                }
                                else
                                {
                                    Write(valueVar);

                                    WriteDot();

                                    if (Emitter.UnaryOperatorType == UnaryOperatorType.Increment ||
                                        Emitter.UnaryOperatorType == UnaryOperatorType.PostIncrement)
                                    {
                                        Write(JS.Funcs.Math.INC);
                                    }
                                    else
                                    {
                                        Write(JS.Funcs.Math.DEC);
                                    }

                                    WriteOpenParentheses();
                                    WriteCloseParentheses();
                                }
                            }
                            else
                            {
                                Write(valueVar);

                                if (Emitter.UnaryOperatorType == UnaryOperatorType.Increment || Emitter.UnaryOperatorType == UnaryOperatorType.PostIncrement)
                                {
                                    Write("+");
                                }
                                else
                                {
                                    Write("-");
                                }

                                Write("1");
                            }

                            WriteCloseParentheses();
                            WriteComma();

                            if (Emitter.UnaryOperatorType == UnaryOperatorType.Increment ||
                                Emitter.UnaryOperatorType == UnaryOperatorType.Decrement)
                            {
                                WriteTarget(memberResult);
                                Write(Helpers.GetPropertyRef(memberResult.Member, Emitter, false));
                                WriteOpenParentheses();
                                WriteCloseParentheses();
                            }
                            else
                            {
                                Write(valueVar);
                            }

                            WriteCloseParentheses();

                            if (valueVar != null)
                            {
                                RemoveTempVar(valueVar);
                            }
                        }
                    }
                    else
                    {
                        Write(Helpers.GetPropertyRef(memberResult.Member, Emitter));
                        WriteOpenParentheses();
                        WriteCloseParentheses();
                    }
                }
                else if (Emitter.AssignmentType != AssignmentOperatorType.Assign)
                {
                    string trg;

                    if (memberResult.Member.IsStatic)
                    {
                        trg = H5Types.ToJsName(memberResult.Member.DeclaringType, Emitter, ignoreLiteralName: false);
                    }
                    else
                    {
                        trg = "this";
                    }

                    bool isBool = memberResult != null && NullableType.IsNullable(memberResult.Member.ReturnType) ? NullableType.GetUnderlyingType(memberResult.Member.ReturnType).IsKnownType(KnownTypeCode.Boolean) : memberResult.Member.ReturnType.IsKnownType(KnownTypeCode.Boolean);
                    bool skipGet = false;
                    bool special = Emitter.Resolver.ResolveNode(identifierExpression.Parent) is OperatorResolveResult orr && orr.IsLiftedOperator;

                    if (!special && isBool &&
                        (Emitter.AssignmentType == AssignmentOperatorType.BitwiseAnd ||
                         Emitter.AssignmentType == AssignmentOperatorType.BitwiseOr))
                    {
                        skipGet = true;
                    }

                    if (skipGet)
                    {
                        PushWriter(string.Concat(Helpers.GetPropertyRef(memberResult.Member, Emitter, true), "({0})"));
                    }
                    else
                    {
                        PushWriter(string.Concat(Helpers.GetPropertyRef(memberResult.Member, Emitter, true),
                        "(",
                        trg,
                        ".",
                        Helpers.GetPropertyRef(memberResult.Member, Emitter, false),
                        "()",
                        "{0})"));
                    }
                }
                else
                {
                    PushWriter(Helpers.GetPropertyRef(memberResult.Member, Emitter, true) + "({0})");
                }
            }
            else if (memberResult != null && memberResult.Member is IEvent)
            {
                if (Emitter.IsAssignment &&
                    (Emitter.AssignmentType == AssignmentOperatorType.Add ||
                     Emitter.AssignmentType == AssignmentOperatorType.Subtract))
                {
                    WriteTarget(memberResult);

                    if (!string.IsNullOrWhiteSpace(inlineCode))
                    {
                        Write(inlineCode);
                    }
                    else
                    {
                        Write(Helpers.GetAddOrRemove(Emitter.AssignmentType == AssignmentOperatorType.Add));
                        Write(
                            OverloadsCollection.Create(Emitter, memberResult.Member,
                                Emitter.AssignmentType == AssignmentOperatorType.Subtract).GetOverloadName());
                    }

                    WriteOpenParentheses();
                }
                else
                {
                    WriteTarget(memberResult);
                    Write(Emitter.GetEntityName(memberResult.Member));
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(inlineCode))
                {
                    Write(inlineCode);
                }
                else if (isResolved)
                {
                    if (resolveResult is LocalResolveResult localResolveResult)
                    {
                        Write(localResolveResult.Variable.Name);
                    }
                    else if (memberResult != null)
                    {
                        WriteTarget(memberResult);
                        string name = OverloadsCollection.Create(Emitter, memberResult.Member).GetOverloadName();
                        if (isRefArg)
                        {
                            WriteScript(name);
                        }
                        else if (memberResult.Member is IField)
                        {
                            WriteIdentifier(name);
                        }
                        else
                        {
                            Write(name);
                        }
                    }
                    else
                    {
                        Write(resolveResult.ToString());
                    }
                }
                else
                {
                    throw new EmitterException(identifierExpression, "Cannot resolve identifier: " + id);
                }
            }

            if (appendAdditionalCode != null)
            {
                Write(appendAdditionalCode);
            }

            Helpers.CheckValueTypeClone(resolveResult, identifierExpression, this, pos);
        }

        protected void WriteTarget(MemberResolveResult memberResult)
        {
            bool noTarget = false;
            if (memberResult.Member.IsStatic)
            {
                var target = H5Types.ToJsName(memberResult.Member.DeclaringType, Emitter, ignoreLiteralName: false);
                noTarget = string.IsNullOrWhiteSpace(target);
                Write(target);
            }
            else
            {
                WriteThis();
            }

            if (isRefArg)
            {
                WriteComma();
            }
            else if (!noTarget)
            {
                WriteDot();
            }
        }
    }
}