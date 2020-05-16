using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.TypeSystem.Implementation;
using Object.Net.Utilities;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace H5.Translator
{
    public class MemberReferenceBlock : ConversionBlock
    {
        public MemberReferenceBlock(IEmitter emitter, MemberReferenceExpression memberReferenceExpression)
            : base(emitter, memberReferenceExpression)
        {
            Emitter = emitter;
            MemberReferenceExpression = memberReferenceExpression;
        }

        public MemberReferenceExpression MemberReferenceExpression { get; set; }

        protected override Expression GetExpression()
        {
            return MemberReferenceExpression;
        }

        protected override void EmitConversionExpression()
        {
            VisitMemberReferenceExpression();
        }

        protected string WriteTarget(ResolveResult resolveResult, bool isInterfaceMember, MemberResolveResult memberTargetrr, ResolveResult targetrr, bool openParentheses)
        {
            string interfaceTempVar = null;
            if (isInterfaceMember)
            {
                MemberResolveResult member = resolveResult as MemberResolveResult;
                bool nativeImplementation = true;
                var externalInterface = member != null && Emitter.Validator.IsExternalInterface(member.Member.DeclaringTypeDefinition, out nativeImplementation);
                bool isField = memberTargetrr != null && memberTargetrr.Member is IField && (memberTargetrr.TargetResult is ThisResolveResult || memberTargetrr.TargetResult is LocalResolveResult);
                bool variance = false;

                if (member != null)
                {
                    var itypeDef = member.Member.DeclaringTypeDefinition;
                    variance = MetadataUtils.IsJsGeneric(itypeDef, Emitter) &&
                        itypeDef.TypeParameters != null &&
                        itypeDef.TypeParameters.Any(typeParameter => typeParameter.Variance != VarianceModifier.Invariant);
                }

                if ((externalInterface && !nativeImplementation || variance) && !(targetrr is ThisResolveResult || targetrr is TypeResolveResult || targetrr is LocalResolveResult || isField))
                {
                    if (openParentheses)
                    {
                        WriteOpenParentheses();
                    }

                    interfaceTempVar = GetTempVarName();
                    Write(interfaceTempVar);
                    Write(" = ");
                }
            }

            WriteSimpleTarget(resolveResult);

            return interfaceTempVar;
        }

        protected void WriteSimpleTarget(ResolveResult resolveResult)
        {
            if (!(resolveResult is MemberResolveResult member) || !member.Member.IsStatic)
            {
                MemberReferenceExpression.Target.AcceptVisitor(Emitter);
                return;
            }

            var imethod = member.Member as IMethod;
            var imember = member.Member;
            if ((imethod != null && imethod.IsExtensionMethod) || imember == null)
            {
                MemberReferenceExpression.Target.AcceptVisitor(Emitter);
                return;
            }
            var target = H5Types.ToJsName(member.Member.DeclaringType, Emitter, ignoreLiteralName: false);
            NoTarget = string.IsNullOrWhiteSpace(target);

            if (member.Member.IsStatic
                && target != CS.NS.H5
                && !Validator.IsTypeFromH5ButNotFromH5Core(target)
                && MemberReferenceExpression.Target.ToString().StartsWith(CS.NS.GLOBAL))
            {
                Write(JS.Types.H5.Global.DOTNAME);
            }

            Write(target);
        }

        public bool NoTarget
        {
            get; set;
        }

        private void WriteInterfaceMember(string interfaceTempVar, MemberResolveResult resolveResult, bool isSetter, string prefix = null)
        {
            var itypeDef = resolveResult.Member.DeclaringTypeDefinition;
            var externalInterface = Emitter.Validator.IsExternalInterface(itypeDef);
            bool variance = MetadataUtils.IsJsGeneric(itypeDef, Emitter) &&
                itypeDef.TypeParameters != null &&
                itypeDef.TypeParameters.Any(typeParameter => typeParameter.Variance != VarianceModifier.Invariant);

            if (interfaceTempVar != null && externalInterface == null && !variance)
            {
                WriteComma();
                Write(interfaceTempVar);
            }

            if (externalInterface != null && externalInterface.IsDualImplementation || variance)
            {
                if (interfaceTempVar != null)
                {
                    WriteCloseParentheses();
                }

                WriteOpenBracket();
                Write(JS.Funcs.H5_GET_I);
                WriteOpenParentheses();

                if (interfaceTempVar != null)
                {
                    Write(interfaceTempVar);
                }
                else
                {
                    WriteSimpleTarget(resolveResult);
                }

                WriteComma();

                var interfaceName = OverloadsCollection.Create(Emitter, resolveResult.Member, isSetter).GetOverloadName(false, prefix);

                if (interfaceName.StartsWith("\""))
                {
                    Write(interfaceName);
                }
                else
                {
                    WriteScript(interfaceName);
                }

                if (variance)
                {
                    WriteComma();
                    WriteScript(OverloadsCollection.Create(Emitter, resolveResult.Member, isSetter).GetOverloadName(false, prefix, withoutTypeParams: true));
                }

                /*this.WriteComma();
                this.WriteScript(OverloadsCollection.Create(Emitter, resolveResult.Member, isSetter).GetOverloadName(true, prefix));*/

                Write(")");
                WriteCloseBracket();

                return;
            }

            WriteOpenBracket();
            Write(OverloadsCollection.Create(Emitter, resolveResult.Member, isSetter).GetOverloadName(externalInterface != null && externalInterface.IsSimpleImplementation, prefix));
            WriteCloseBracket();

            if (interfaceTempVar != null)
            {
                WriteCloseParentheses();
            }
        }

        protected void VisitMemberReferenceExpression()
        {
            MemberReferenceExpression memberReferenceExpression = MemberReferenceExpression;
            int pos = Emitter.Output.Length;
            bool isRefArg = Emitter.IsRefArg;
            Emitter.IsRefArg = false;

            ResolveResult resolveResult = null;
            ResolveResult expressionResolveResult = null;
            string targetVar = null;
            string valueVar = null;
            bool isStatement = false;
            bool isConstTarget = false;

            var targetrr = Emitter.Resolver.ResolveNode(memberReferenceExpression.Target, Emitter);
            if (targetrr is ConstantResolveResult)
            {
                isConstTarget = true;
            }

            var memberTargetrr = targetrr as MemberResolveResult;
            if (memberTargetrr != null && memberTargetrr.Type.Kind == TypeKind.Enum && memberTargetrr.Member is DefaultResolvedField && Helpers.EnumEmitMode(memberTargetrr.Type) == 2)
            {
                isConstTarget = true;
            }

            if (memberReferenceExpression.Target is ParenthesizedExpression ||
                (targetrr is ConstantResolveResult && targetrr.Type.IsKnownType(KnownTypeCode.Int64)) ||
                (targetrr is ConstantResolveResult && targetrr.Type.IsKnownType(KnownTypeCode.UInt64)) ||
                (targetrr is ConstantResolveResult && targetrr.Type.IsKnownType(KnownTypeCode.Decimal)))
            {
                isConstTarget = false;
            }

            var isInvoke = memberReferenceExpression.Parent is InvocationExpression && (((InvocationExpression)(memberReferenceExpression.Parent)).Target == memberReferenceExpression);
            if (isInvoke)
            {
                resolveResult = Emitter.Resolver.ResolveNode(memberReferenceExpression.Parent, Emitter);
                expressionResolveResult = Emitter.Resolver.ResolveNode(memberReferenceExpression, Emitter);

                if (expressionResolveResult is InvocationResolveResult)
                {
                    resolveResult = expressionResolveResult;
                }
                else if (expressionResolveResult is MemberResolveResult)
                {
                    if (((MemberResolveResult)expressionResolveResult).Member is IProperty)
                    {
                        resolveResult = expressionResolveResult;
                    }
                }
            }
            else
            {
                resolveResult = Emitter.Resolver.ResolveNode(memberReferenceExpression, Emitter);
            }

            bool oldIsAssignment = Emitter.IsAssignment;
            bool oldUnary = Emitter.IsUnaryAccessor;

            if (resolveResult == null)
            {
                Emitter.IsAssignment = false;
                Emitter.IsUnaryAccessor = false;
                if (isConstTarget)
                {
                    Write("(");
                }
                memberReferenceExpression.Target.AcceptVisitor(Emitter);
                if (isConstTarget)
                {
                    Write(")");
                }
                Emitter.IsAssignment = oldIsAssignment;
                Emitter.IsUnaryAccessor = oldUnary;
                WriteDot();
                string name = memberReferenceExpression.MemberName;
                Write(name.ToLowerCamelCase());

                return;
            }

            bool isDynamic = false;
            if (resolveResult is DynamicInvocationResolveResult dynamicResolveResult)
            {
                if (dynamicResolveResult.Target is MethodGroupResolveResult group && group.Methods.Count() > 1)
                {
                    var method = group.Methods.FirstOrDefault(m =>
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
                    }) ?? group.Methods.Last();

                    isDynamic = true;
                    resolveResult = new MemberResolveResult(new TypeResolveResult(method.DeclaringType), method);
                    resolveResult = new InvocationResolveResult(resolveResult, method, dynamicResolveResult.Arguments);
                }
            }

            if (resolveResult is MethodGroupResolveResult oldResult)
            {
                resolveResult = Emitter.Resolver.ResolveNode(memberReferenceExpression.Parent, Emitter);

                if (resolveResult is DynamicInvocationResolveResult)
                {
                    var method = oldResult.Methods.Last();
                    resolveResult = new MemberResolveResult(new TypeResolveResult(method.DeclaringType), method);
                }
            }

            MemberResolveResult member = resolveResult as MemberResolveResult;
            var globalTarget = member != null ? Emitter.IsGlobalTarget(member.Member) : null;

            if (member != null &&
                member.Member.Attributes.Any(a => a.AttributeType.FullName == "H5.NonScriptableAttribute"))
            {
                throw new EmitterException(MemberReferenceExpression, "Member " + member.ToString() + " is marked as not usable from script");
            }

            if (!(resolveResult is InvocationResolveResult) && member != null && member.Member is IMethod)
            {
                var interceptor = Emitter.Plugins.OnReference(this, MemberReferenceExpression, member);

                if (interceptor.Cancel)
                {
                    return;
                }

                if (!string.IsNullOrEmpty(interceptor.Replacement))
                {
                    Write(interceptor.Replacement);
                    return;
                }
            }

            if (globalTarget != null && globalTarget.Item1)
            {
                var target = globalTarget.Item2;

                if (!string.IsNullOrWhiteSpace(target))
                {
                    bool assign = false;
                    var memberExpression = member.Member is IMethod ? memberReferenceExpression.Parent.Parent : memberReferenceExpression.Parent;
                    var targetExpression = member.Member is IMethod ? memberReferenceExpression.Parent : memberReferenceExpression;
                    if (memberExpression is AssignmentExpression assignment && assignment.Right == targetExpression)
                    {
                        assign = true;
                    }
                    else
                    {
                        if (memberExpression is VariableInitializer varInit && varInit.Initializer == targetExpression)
                        {
                            assign = true;
                        }
                        else if (memberExpression is InvocationExpression targetInvocation)
                        {
                            if (targetInvocation.Arguments.Any(a => a == targetExpression))
                            {
                                assign = true;
                            }
                        }
                    }

                    if (assign)
                    {
                        if (resolveResult is InvocationResolveResult)
                        {
                            PushWriter(target);
                        }
                        else
                        {
                            Write(target);
                        }

                        return;
                    }
                }

                if (resolveResult is InvocationResolveResult)
                {
                    PushWriter("");
                }

                return;
            }

            Tuple<bool, bool, string> inlineInfo = member != null ? (isDynamic ? ((Emitter)Emitter).GetInlineCodeFromMember(member.Member, null) : Emitter.GetInlineCode(memberReferenceExpression)) : null;
            //string inline = member != null ? this.Emitter.GetInline(member.Member) : null;
            string inline = inlineInfo?.Item3;

            if (string.IsNullOrEmpty(inline) && member != null &&
                member.Member is IMethod &&
                !(member is InvocationResolveResult) &&
                !(
                    memberReferenceExpression.Parent is InvocationExpression &&
                    memberReferenceExpression.NextSibling != null &&
                    memberReferenceExpression.NextSibling.Role is TokenRole &&
                    ((TokenRole)memberReferenceExpression.NextSibling.Role).Token == "("
                    )
                )
            {
                if (!(memberReferenceExpression.Parent is InvocationExpression parentInvocation) || parentInvocation.Target != memberReferenceExpression)
                {
                    var method = (IMethod)member.Member;
                    if (method.TypeArguments.Count > 0 || method.IsExtensionMethod)
                    {
                        inline = MemberReferenceBlock.GenerateInlineForMethodReference(method, Emitter);
                    }
                }
            }

            if (member != null && member.Member is IMethod && isInvoke)
            {
                if (Emitter.Resolver.ResolveNode(memberReferenceExpression.Parent, Emitter) is CSharpInvocationResolveResult i_rr && !i_rr.IsExpandedForm)
                {
                    var tpl = Emitter.GetAttribute(member.Member.Attributes, JS.NS.H5 + ".TemplateAttribute");

                    if (tpl != null && tpl.PositionalArguments.Count == 2)
                    {
                        inline = tpl.PositionalArguments[1].ConstantValue.ToString();
                    }
                }
            }

            bool hasInline = !string.IsNullOrEmpty(inline);
            bool hasThis = hasInline && Helpers.HasThis(inline);
            inline = hasInline ? Helpers.ConvertTokens(Emitter, inline, member.Member) : inline;
            bool isInterfaceMember = false;

            if (hasInline && inline.StartsWith("<self>"))
            {
                hasThis = true;
                inline = inline.Substring(6);
            }

            bool nativeImplementation = true;
            bool isInterface = inline == null && member != null && member.Member.DeclaringTypeDefinition != null && member.Member.DeclaringTypeDefinition.Kind == TypeKind.Interface;
            var hasTypeParemeter = isInterface && Helpers.IsTypeParameterType(member.Member.DeclaringType);
            if (isInterface)
            {
                var itypeDef = member.Member.DeclaringTypeDefinition;
                var variance = MetadataUtils.IsJsGeneric(itypeDef, Emitter) &&
                    itypeDef.TypeParameters != null &&
                    itypeDef.TypeParameters.Any(typeParameter => typeParameter.Variance != VarianceModifier.Invariant);

                if (variance)
                {
                    isInterfaceMember = true;
                }
                else
                {
                    var ei = Emitter.Validator.IsExternalInterface(itypeDef);

                    if (ei != null)
                    {
                        nativeImplementation = ei.IsNativeImplementation;
                    }
                    else
                    {
                        nativeImplementation = member.Member.DeclaringTypeDefinition.ParentAssembly.AssemblyName == CS.NS.H5 ||
                                               !Emitter.Validator.IsExternalType(member.Member.DeclaringTypeDefinition);
                    }

                    if (ei != null && ei.IsSimpleImplementation)
                    {
                        nativeImplementation = false;
                        isInterfaceMember = false;
                    }
                    else if (ei != null || hasTypeParemeter)
                    {
                        if (hasTypeParemeter || !nativeImplementation)
                        {
                            isInterfaceMember = true;
                        }
                    }
                }
            }

            string interfaceTempVar = null;

            if (hasThis)
            {
                Write("");
                var oldBuilder = Emitter.Output;
                var oldInline = inline;
                string thisArg = null;
                bool isSimple = true;

                if (MemberReferenceExpression.Target is BaseReferenceExpression)
                {
                    thisArg = "this";
                }
                else
                {
                    Emitter.Output = new StringBuilder();
                    Emitter.IsAssignment = false;
                    Emitter.IsUnaryAccessor = false;
                    if (isConstTarget)
                    {
                        Write("(");
                    }
                    WriteSimpleTarget(resolveResult);
                    if (isConstTarget)
                    {
                        Write(")");
                    }

                    thisArg = Emitter.Output.ToString();

                    if (Regex.Matches(inline, @"\{(\*?)this\}").Count > 1)
                    {
                        var mrr = resolveResult as MemberResolveResult;
                        bool isField = mrr != null && mrr.Member is IField &&
                                       (mrr.TargetResult is ThisResolveResult ||
                                        mrr.TargetResult is LocalResolveResult || mrr.TargetResult is MemberResolveResult && ((MemberResolveResult)mrr.TargetResult).Member is IField);

                        isSimple = (mrr != null && (mrr.TargetResult is ThisResolveResult || mrr.TargetResult is ConstantResolveResult || mrr.TargetResult is LocalResolveResult)) || isField;
                    }
                }

                int thisIndex;
                inline = member != null ? Helpers.ConvertTokens(Emitter, inline, member.Member) : inline;
                if (!isSimple)
                {
                    StringBuilder sb = new StringBuilder();

                    sb.Append("(");
                    var tempVar = GetTempVarName();
                    inline = inline.Replace("{this}", tempVar);
                    thisIndex = tempVar.Length + 2;

                    sb.Append(tempVar);
                    sb.Append(" = ");
                    sb.Append(thisArg);
                    sb.Append(", ");

                    sb.Append(inline);
                    sb.Append(")");

                    inline = sb.ToString();
                }
                else
                {
                    thisIndex = inline.IndexOf("{this}", StringComparison.Ordinal);
                    inline = inline.Replace("{this}", thisArg);
                }

                if (member != null && member.Member is IProperty)
                {
                    Emitter.Output = new StringBuilder();
                    inline = inline.Replace("{0}", "[[0]]");
                    new InlineArgumentsBlock(Emitter, new ArgumentsInfo(Emitter, memberReferenceExpression, resolveResult), inline).Emit();
                    inline = Emitter.Output.ToString();
                    inline = inline.Replace("[[0]]", "{0}");
                }
                else if (member != null && member.Member is IEvent)
                {
                    Emitter.Output = new StringBuilder();
                    inline = inline.Replace("{0}", "[[0]]");
                    new InlineArgumentsBlock(Emitter, new ArgumentsInfo(Emitter, memberReferenceExpression, resolveResult), inline).Emit();
                    inline = Emitter.Output.ToString();
                    inline = inline.Replace("[[0]]", "{0}");
                }

                Emitter.Output = new StringBuilder(inline);
                Helpers.CheckValueTypeClone(resolveResult, MemberReferenceExpression, this, pos);
                inline = Emitter.Output.ToString();

                Emitter.IsAssignment = oldIsAssignment;
                Emitter.IsUnaryAccessor = oldUnary;
                Emitter.Output = oldBuilder;

                int[] range = null;

                if (thisIndex > -1)
                {
                    range = new[] { thisIndex, thisIndex + thisArg.Length };
                }

                if (resolveResult is InvocationResolveResult)
                {
                    PushWriter(inline, null, thisArg, range);
                }
                else
                {
                    if (member != null && member.Member is IMethod)
                    {
                        new InlineArgumentsBlock(Emitter, new ArgumentsInfo(Emitter, memberReferenceExpression, resolveResult), oldInline, (IMethod)member.Member, targetrr).EmitFunctionReference();
                    }
                    else if (member != null && member.Member is IField && inline.Contains("{0}"))
                    {
                        PushWriter(inline, null, thisArg, range);
                    }
                    else if (InlineArgumentsBlock.FormatArgRegex.IsMatch(inline))
                    {
                        PushWriter(inline, null, thisArg, range);
                    }
                    else
                    {
                        Write(inline);
                    }
                }

                return;
            }

            if (member != null && member.Member.SymbolKind == SymbolKind.Field && Emitter.IsMemberConst(member.Member) && Emitter.IsInlineConst(member.Member))
            {
                bool wrap = false;

                if (memberReferenceExpression.Parent is MemberReferenceExpression parentExpression)
                {
                    var ii = Emitter.GetInlineCode(parentExpression);

                    if (string.IsNullOrEmpty(ii.Item3))
                    {
                        wrap = true;
                        WriteOpenParentheses();
                    }
                }

                WriteScript(H5.Translator.Emitter.ConvertConstant(member.ConstantValue, memberReferenceExpression, Emitter));

                if (wrap)
                {
                    WriteCloseParentheses();
                }
            }
            else if (hasInline && member.Member.IsStatic)
            {
                if (resolveResult is InvocationResolveResult)
                {
                    PushWriter(inline);
                }
                else
                {
                    if (member != null && member.Member is IMethod)
                    {
                        new InlineArgumentsBlock(Emitter, new ArgumentsInfo(Emitter, memberReferenceExpression, resolveResult), inline, (IMethod)member.Member, targetrr).EmitFunctionReference();
                    }
                    else
                    {
                        new InlineArgumentsBlock(Emitter, new ArgumentsInfo(Emitter, memberReferenceExpression, resolveResult), inline).Emit();
                    }
                }
            }
            else
            {
                if (member != null && member.IsCompileTimeConstant && member.Member.DeclaringType.Kind == TypeKind.Enum)
                {
                    if (member.Member.DeclaringType is ITypeDefinition typeDef)
                    {
                        var enumMode = Helpers.EnumEmitMode(typeDef);

                        if ((Emitter.Validator.IsExternalType(typeDef) && enumMode == -1) || enumMode == 2)
                        {
                            WriteScript(member.ConstantValue);

                            return;
                        }

                        if (enumMode >= 3 && enumMode < 7)
                        {
                            string enumStringName = Emitter.GetEntityName(member.Member);
                            WriteScript(enumStringName);
                            return;
                        }
                    }
                }

                if (resolveResult is TypeResolveResult typeResolveResult)
                {
                    Write(H5Types.ToJsName(typeResolveResult.Type, Emitter));
                    return;
                }
                else
                {
                    if (member != null &&
                        member.Member is IMethod &&
                        !(member is InvocationResolveResult) &&
                        !(
                            memberReferenceExpression.Parent is InvocationExpression &&
                            memberReferenceExpression.NextSibling != null &&
                            memberReferenceExpression.NextSibling.Role is TokenRole &&
                            ((TokenRole)memberReferenceExpression.NextSibling.Role).Token == "("
                        )
                    )
                    {
                        if (!(memberReferenceExpression.Parent is InvocationExpression parentInvocation) || parentInvocation.Target != memberReferenceExpression)
                        {
                            if (!string.IsNullOrEmpty(inline))
                            {
                                if (!(resolveResult is InvocationResolveResult) && member != null && member.Member is IMethod)
                                {
                                    new InlineArgumentsBlock(Emitter,
                                        new ArgumentsInfo(Emitter, memberReferenceExpression, resolveResult), inline,
                                        (IMethod)member.Member, targetrr).EmitFunctionReference();
                                }
                                else if (resolveResult is InvocationResolveResult ||
                                         (member.Member.SymbolKind == SymbolKind.Property && Emitter.IsAssignment))
                                {
                                    PushWriter(inline);
                                }
                                else
                                {
                                    Write(inline);
                                }
                            }
                            else
                            {
                                var resolvedMethod = (IMethod)member.Member;
                                bool isStatic = resolvedMethod != null && resolvedMethod.IsStatic;

                                var isExtensionMethod = resolvedMethod.IsExtensionMethod;

                                Emitter.IsAssignment = false;
                                Emitter.IsUnaryAccessor = false;

                                if (!isStatic)
                                {
                                    Write(isExtensionMethod ? JS.Funcs.H5_BIND_SCOPE : JS.Funcs.H5_CACHE_BIND);
                                    WriteOpenParentheses();

                                    if (memberReferenceExpression.Target is BaseReferenceExpression)
                                    {
                                        WriteThis();
                                    }
                                    else
                                    {
                                        interfaceTempVar = WriteTarget(resolveResult, isInterfaceMember, memberTargetrr, targetrr, false);
                                    }

                                    Write(", ");
                                }

                                Emitter.IsAssignment = oldIsAssignment;
                                Emitter.IsUnaryAccessor = oldUnary;

                                if (isExtensionMethod)
                                {
                                    Write(H5Types.ToJsName(resolvedMethod.DeclaringType, Emitter));
                                }
                                else
                                {
                                    Emitter.IsAssignment = false;
                                    Emitter.IsUnaryAccessor = false;
                                    if (isConstTarget)
                                    {
                                        Write("(");
                                    }

                                    if (interfaceTempVar != null)
                                    {
                                        Write(interfaceTempVar);
                                    }
                                    else
                                    {
                                        WriteSimpleTarget(resolveResult);
                                    }

                                    if (isConstTarget)
                                    {
                                        Write(")");
                                    }
                                    Emitter.IsAssignment = oldIsAssignment;
                                    Emitter.IsUnaryAccessor = oldUnary;
                                }

                                if (isInterfaceMember)
                                {
                                    WriteInterfaceMember(interfaceTempVar, member, false);
                                }
                                else
                                {
                                    WriteDot();
                                    Write(OverloadsCollection.Create(Emitter, member.Member).GetOverloadName(!nativeImplementation));
                                }

                                if (!isStatic)
                                {
                                    Write(")");
                                }
                            }

                            return;
                        }
                    }

                    bool isProperty = false;

                    if (member != null && member.Member.SymbolKind == SymbolKind.Property && (member.Member.DeclaringTypeDefinition == null || !Emitter.Validator.IsObjectLiteral(member.Member.DeclaringTypeDefinition)))
                    {
                        isProperty = true;
                        bool writeTargetVar = false;

                        if (Emitter.IsAssignment && Emitter.AssignmentType != AssignmentOperatorType.Assign)
                        {
                            writeTargetVar = true;
                        }
                        else if (Emitter.IsUnaryAccessor)
                        {
                            writeTargetVar = true;

                            isStatement = memberReferenceExpression.Parent is UnaryOperatorExpression && memberReferenceExpression.Parent.Parent is ExpressionStatement;

                            if (NullableType.IsNullable(member.Type))
                            {
                                isStatement = false;
                            }

                            if (!isStatement)
                            {
                                WriteOpenParentheses();
                            }
                        }

                        if (writeTargetVar)
                        {
                            bool isField = memberTargetrr != null && memberTargetrr.Member is IField && (memberTargetrr.TargetResult is ThisResolveResult || memberTargetrr.TargetResult is LocalResolveResult);

                            if (!(targetrr is ThisResolveResult || targetrr is TypeResolveResult || targetrr is LocalResolveResult || isField))
                            {
                                targetVar = GetTempVarName();

                                Write(targetVar);
                                Write(" = ");
                            }
                        }
                    }

                    if (isProperty && Emitter.IsUnaryAccessor && !isStatement && targetVar == null)
                    {
                        valueVar = GetTempVarName();

                        Write(valueVar);
                        Write(" = ");
                    }

                    Emitter.IsAssignment = false;
                    Emitter.IsUnaryAccessor = false;
                    if (isConstTarget)
                    {
                        Write("(");
                    }

                    if (targetVar == null && isInterfaceMember)
                    {
                        interfaceTempVar = WriteTarget(resolveResult, isInterfaceMember, memberTargetrr, targetrr, true);
                    }
                    else
                    {
                        WriteSimpleTarget(resolveResult);
                    }

                    if (member != null && targetrr != null && targetrr.Type.Kind == TypeKind.Delegate && (member.Member.Name == "Invoke"))
                    {
                        if (!(member.Member is IMethod method && method.IsExtensionMethod))
                        {
                            return;
                        }
                    }

                    if (isConstTarget)
                    {
                        Write(")");
                    }
                    Emitter.IsAssignment = oldIsAssignment;
                    Emitter.IsUnaryAccessor = oldUnary;

                    if (targetVar != null)
                    {
                        if (Emitter.IsUnaryAccessor && !isStatement)
                        {
                            WriteComma(false);

                            valueVar = GetTempVarName();

                            Write(valueVar);
                            Write(" = ");

                            Write(targetVar);
                        }
                        else
                        {
                            WriteSemiColon();
                            WriteNewLine();
                            Write(targetVar);
                        }
                    }
                }


                if (!(targetrr is MemberResolveResult targetResolveResult) || Emitter.IsGlobalTarget(targetResolveResult.Member) == null)
                {
                    if (isRefArg)
                    {
                        WriteComma();
                    }
                    else if (!isInterfaceMember && !NoTarget)
                    {
                        WriteDot();
                    }
                }

                if (member == null)
                {
                    if (targetrr != null && targetrr.Type.Kind == TypeKind.Dynamic)
                    {
                        Write(memberReferenceExpression.MemberName);
                    }
                    else
                    {
                        Write(memberReferenceExpression.MemberName.ToLowerCamelCase());
                    }
                }
                else if (!string.IsNullOrEmpty(inline))
                {
                    if (!(resolveResult is InvocationResolveResult) && member != null && member.Member is IMethod)
                    {
                        new InlineArgumentsBlock(Emitter, new ArgumentsInfo(Emitter, memberReferenceExpression, resolveResult), inline, (IMethod)member.Member, targetrr).EmitFunctionReference();
                    }
                    else if (resolveResult is InvocationResolveResult ||
                        (member.Member.SymbolKind == SymbolKind.Property && Emitter.IsAssignment) ||
                        (member.Member != null && member.Member is IEvent))
                    {
                        PushWriter(inline);
                    }
                    else
                    {
                        Write(inline);
                    }
                }
                else if (member.Member.SymbolKind == SymbolKind.Property && (member.Member.DeclaringTypeDefinition == null || (!Emitter.Validator.IsObjectLiteral(member.Member.DeclaringTypeDefinition) || member.Member.IsStatic)))
                {
                    if (member.Member is IProperty && targetrr != null && targetrr.Type.GetDefinition() != null && Emitter.Validator.IsObjectLiteral(targetrr.Type.GetDefinition()) && !Emitter.Validator.IsObjectLiteral(member.Member.DeclaringTypeDefinition))
                    {
                        Write(Emitter.GetLiteralEntityName(member.Member));
                    }
                    else
                    {
                        if (isInterfaceMember)
                        {
                            WriteInterfaceMember(interfaceTempVar ?? targetVar, member, false);
                        }
                        else
                        {
                            var name = OverloadsCollection.Create(Emitter, member.Member).GetOverloadName(!nativeImplementation);
                            var property = (IProperty)member.Member;
                            var proto = member.IsVirtualCall || property.IsVirtual || property.IsOverride;

                            if (MemberReferenceExpression.Target is BaseReferenceExpression && !property.IsIndexer && proto)
                            {
                                var alias = H5Types.ToJsName(member.Member.DeclaringType, Emitter,
                                    isAlias: true);
                                if (alias.StartsWith("\""))
                                {
                                    alias = alias.Insert(1, "$");
                                    name = alias + "+\"$" + name + "\"";
                                    WriteIdentifier(name, false);
                                }
                                else
                                {
                                    name = "$" + alias + "$" + name;
                                    WriteIdentifier(name);
                                }
                            }
                            else
                            {
                                WriteIdentifier(name);
                            }
                        }
                    }
                }
                else if (member.Member.SymbolKind == SymbolKind.Field)
                {
                    bool isConst = Emitter.IsMemberConst(member.Member);

                    if (isConst && Emitter.IsInlineConst(member.Member))
                    {
                        WriteScript(H5.Translator.Emitter.ConvertConstant(member.ConstantValue, memberReferenceExpression, Emitter));
                    }
                    else
                    {
                        if (isInterfaceMember)
                        {
                            WriteInterfaceMember(interfaceTempVar ?? targetVar, member, false);
                        }
                        else
                        {
                            var fieldName = OverloadsCollection.Create(Emitter, member.Member).GetOverloadName(!nativeImplementation);

                            if (isRefArg)
                            {
                                WriteScript(fieldName);
                            }
                            else
                            {
                                WriteIdentifier(fieldName);
                            }
                        }
                    }
                }
                else if (resolveResult is InvocationResolveResult invocationResult)
                {
                    if (isInterfaceMember)
                    {
                        WriteInterfaceMember(interfaceTempVar ?? targetVar, member, false);
                    }
                    else if (expressionResolveResult is MemberResolveResult expresssionMember &&
                        resolveResult is CSharpInvocationResolveResult cInvocationResult &&
                        cInvocationResult.IsDelegateInvocation &&
                        invocationResult.Member != expresssionMember.Member)
                    {
                        Write(OverloadsCollection.Create(Emitter, expresssionMember.Member).GetOverloadName(!nativeImplementation));
                    }
                    else
                    {
                        Write(OverloadsCollection.Create(Emitter, invocationResult.Member).GetOverloadName(!nativeImplementation));
                    }
                }
                else if (member.Member is IEvent)
                {
                    if (Emitter.IsAssignment &&
                        (Emitter.AssignmentType == AssignmentOperatorType.Add ||
                         Emitter.AssignmentType == AssignmentOperatorType.Subtract))
                    {
                        if (isInterfaceMember)
                        {
                            WriteInterfaceMember(interfaceTempVar ?? targetVar, member, Emitter.AssignmentType == AssignmentOperatorType.Subtract, Helpers.GetAddOrRemove(Emitter.AssignmentType == AssignmentOperatorType.Add));
                        }
                        else
                        {
                            Write(Helpers.GetEventRef(member.Member, Emitter, Emitter.AssignmentType != AssignmentOperatorType.Add, ignoreInterface: !nativeImplementation));
                        }

                        WriteOpenParentheses();
                    }
                    else
                    {
                        if (isInterfaceMember)
                        {
                            WriteInterfaceMember(interfaceTempVar ?? targetVar, member, false);
                        }
                        else
                        {
                            Write(Emitter.GetEntityName(member.Member));
                        }
                    }
                }
                else
                {
                    if (isInterfaceMember)
                    {
                        WriteInterfaceMember(interfaceTempVar ?? targetVar, member, false);
                    }
                    else
                    {
                        var memberName = Emitter.GetEntityName(member.Member);
                        if (isRefArg)
                        {
                            WriteScript(memberName);
                        }
                        else
                        {
                            WriteIdentifier(memberName);
                        }
                    }
                }

                Helpers.CheckValueTypeClone(resolveResult, memberReferenceExpression, this, pos);
            }
        }

        public static string GenerateInlineForMethodReference(IMethod method, IEmitter emitter)
        {
            StringBuilder sb = new StringBuilder();
            var parameters = method.Parameters;

            if (!method.IsStatic)
            {
                sb.Append("{this}");
            }
            else
            {
                sb.Append(H5Types.ToJsName(method.DeclaringType, emitter));
            }

            sb.Append(".");
            sb.Append(OverloadsCollection.Create(emitter, method).GetOverloadName());
            sb.Append("(");

            bool needComma = false;

            if (!Helpers.IsIgnoreGeneric(method, emitter))
            {
                foreach (var typeArgument in method.TypeArguments)
                {
                    if (needComma)
                    {
                        sb.Append(", ");
                    }

                    needComma = true;
                    if (typeArgument.Kind == TypeKind.TypeParameter)
                    {
                        sb.Append("{");
                        sb.Append(typeArgument.Name);
                        sb.Append("}");
                    }
                    else
                    {
                        sb.Append(H5Types.ToJsName(typeArgument, emitter));
                    }
                }
            }

            foreach (var parameter in parameters)
            {
                if (needComma)
                {
                    sb.Append(", ");
                }

                needComma = true;
                sb.Append("{");
                sb.Append(parameter.Name);
                sb.Append("}");
            }

            sb.Append(")");
            return sb.ToString();
        }
    }
}