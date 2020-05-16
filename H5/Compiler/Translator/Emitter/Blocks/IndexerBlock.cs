using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace H5.Translator
{
    public class IndexerAccessor
    {
        public IAttribute InlineAttr { get; set; }

        public string InlineCode { get; set; }

        public IMethod Method { get; set; }

        public bool IgnoreAccessor { get; set; }
    }

    public class IndexerBlock : ConversionBlock
    {
        private bool isRefArg;

        public IndexerBlock(IEmitter emitter, IndexerExpression indexerExpression)
            : base(emitter, indexerExpression)
        {
            Emitter = emitter;
            IndexerExpression = indexerExpression;
        }

        public IndexerExpression IndexerExpression { get; set; }

        protected override Expression GetExpression()
        {
            return IndexerExpression;
        }

        protected override void EmitConversionExpression()
        {
            VisitIndexerExpression();
        }

        protected void VisitIndexerExpression()
        {
            isRefArg = Emitter.IsRefArg;
            Emitter.IsRefArg = false;

            IndexerExpression indexerExpression = IndexerExpression;
            int pos = Emitter.Output.Length;
            var resolveResult = Emitter.Resolver.ResolveNode(indexerExpression);
            var memberResolveResult = resolveResult as MemberResolveResult;


            if (resolveResult is ArrayAccessResolveResult arrayAccess && arrayAccess.Indexes.Count > 1)
            {
                EmitMultiDimArrayAccess(indexerExpression);
                Helpers.CheckValueTypeClone(resolveResult, indexerExpression, this, pos);
                return;
            }

            var isIgnore = true;
            var isAccessorsIndexer = false;

            IProperty member = null;

            IndexerAccessor current = null;

            if (memberResolveResult != null)
            {
                var resolvedMember = memberResolveResult.Member;
                isIgnore = Emitter.Validator.IsExternalType(resolvedMember.DeclaringTypeDefinition);
                isAccessorsIndexer = Emitter.Validator.IsAccessorsIndexer(resolvedMember);

                if (resolvedMember is IProperty property)
                {
                    member = property;
                    current = IndexerBlock.GetIndexerAccessor(Emitter, member, Emitter.IsAssignment);
                }
            }

            if (current != null && current.InlineAttr != null)
            {
                EmitInlineIndexer(indexerExpression, current);
            }
            else if (!(isIgnore || (current != null && current.IgnoreAccessor)) || isAccessorsIndexer)
            {
                EmitAccessorIndexer(indexerExpression, memberResolveResult, member);
            }
            else
            {
                EmitSingleDimArrayIndexer(indexerExpression);
            }

            Helpers.CheckValueTypeClone(resolveResult, indexerExpression, this, pos);
        }

        private void WriteInterfaceMember(string interfaceTempVar, MemberResolveResult resolveResult, bool isSetter, string prefix = null)
        {
            if (interfaceTempVar != null)
            {
                WriteComma();
                Write(interfaceTempVar);
            }

            var itypeDef = resolveResult.Member.DeclaringTypeDefinition;
            var externalInterface = Emitter.Validator.IsExternalInterface(itypeDef);
            bool variance = MetadataUtils.IsJsGeneric(itypeDef, Emitter) &&
                itypeDef.TypeParameters != null &&
                itypeDef.TypeParameters.Any(typeParameter => typeParameter.Variance != VarianceModifier.Invariant);

            WriteOpenBracket();
            if (externalInterface != null && externalInterface.IsDualImplementation || variance)
            {
                Write(JS.Funcs.H5_GET_I);
                WriteOpenParentheses();

                if (interfaceTempVar != null)
                {
                    Write(interfaceTempVar);
                }
                else
                {
                    var oldIsAssignment = Emitter.IsAssignment;
                    var oldUnary = Emitter.IsUnaryAccessor;

                    Emitter.IsAssignment = false;
                    Emitter.IsUnaryAccessor = false;
                    IndexerExpression.Target.AcceptVisitor(Emitter);
                    Emitter.IsAssignment = oldIsAssignment;
                    Emitter.IsUnaryAccessor = oldUnary;
                }

                WriteComma();

                var interfaceName = Helpers.GetPropertyRef(resolveResult.Member, Emitter, isSetter, ignoreInterface:false);

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
                    WriteScript(Helpers.GetPropertyRef(resolveResult.Member, Emitter, isSetter, ignoreInterface: false, withoutTypeParams:true));
                }

                Write(")");
            }
            else if (externalInterface == null || externalInterface.IsNativeImplementation)
            {
                Write(Helpers.GetPropertyRef(resolveResult.Member, Emitter, isSetter, ignoreInterface: false));
            }
            else
            {
                Write(Helpers.GetPropertyRef(resolveResult.Member, Emitter, isSetter, ignoreInterface: true));
            }

            WriteCloseBracket();

            if (interfaceTempVar != null)
            {
                WriteCloseParentheses();
            }
        }

        public static IndexerAccessor GetIndexerAccessor(IEmitter emitter, IProperty member, bool setter)
        {
            var method = setter ? member.Setter : member.Getter;

            if (method == null)
            {
                return null;
            }

            var inlineAttr = emitter.GetAttribute(method.Attributes, Translator.H5_ASSEMBLY + ".TemplateAttribute");
            var ignoreAccessor = emitter.Validator.IsExternalType(method);

            return new IndexerAccessor
            {
                IgnoreAccessor = ignoreAccessor,
                InlineAttr = inlineAttr,
                InlineCode = emitter.GetInline(method),
                Method = method
            };
        }

        protected virtual void EmitInlineIndexer(IndexerExpression indexerExpression, IndexerAccessor current)
        {
            var oldIsAssignment = Emitter.IsAssignment;
            var oldUnary = Emitter.IsUnaryAccessor;
            var inlineCode = current.InlineCode;
            var rr = Emitter.Resolver.ResolveNode(indexerExpression) as MemberResolveResult;
            if (rr != null)
            {
                inlineCode = Helpers.ConvertTokens(Emitter, inlineCode, rr.Member);
            }

            bool hasThis = inlineCode != null && inlineCode.Contains("{this}");

            if (inlineCode != null && inlineCode.StartsWith("<self>"))
            {
                hasThis = true;
                inlineCode = inlineCode.Substring(6);
            }

            if (!hasThis && current.InlineAttr != null)
            {
                Emitter.IsAssignment = false;
                Emitter.IsUnaryAccessor = false;
                indexerExpression.Target.AcceptVisitor(Emitter);
                Emitter.IsAssignment = oldIsAssignment;
                Emitter.IsUnaryAccessor = oldUnary;
            }

            if (hasThis)
            {
                Write("");
                var oldBuilder = Emitter.Output;
                Emitter.Output = new StringBuilder();
                Emitter.IsAssignment = false;
                Emitter.IsUnaryAccessor = false;
                indexerExpression.Target.AcceptVisitor(Emitter);
                int thisIndex = inlineCode.IndexOf("{this}");
                var thisArg = Emitter.Output.ToString();
                inlineCode = inlineCode.Replace("{this}", thisArg);

                Emitter.Output = new StringBuilder();
                inlineCode = inlineCode.Replace("{value}", "[[value]]");
                new InlineArgumentsBlock(Emitter, new ArgumentsInfo(Emitter, indexerExpression, rr as InvocationResolveResult), inlineCode).Emit();
                inlineCode = Emitter.Output.ToString();
                inlineCode = inlineCode.Replace("[[value]]", "{0}");

                Emitter.IsAssignment = oldIsAssignment;
                Emitter.IsUnaryAccessor = oldUnary;
                Emitter.Output = oldBuilder;
                int[] range = null;

                if (thisIndex > -1)
                {
                    range = new[] { thisIndex, thisIndex + thisArg.Length };
                }

                PushWriter(inlineCode, null, thisArg, range);

                if (!Emitter.IsAssignment)
                {
                    PopWriter();
                }

                return;
            }

            if (inlineCode != null)
            {
                WriteDot();
                PushWriter(inlineCode);
                Emitter.IsAssignment = false;
                Emitter.IsUnaryAccessor = false;
                new ExpressionListBlock(Emitter, indexerExpression.Arguments, null, null, 0).Emit();
                Emitter.IsAssignment = oldIsAssignment;
                Emitter.IsUnaryAccessor = oldUnary;

                if (!Emitter.IsAssignment)
                {
                    PopWriter();
                }
                else
                {
                    WriteComma();
                }
            }
        }

        protected virtual void EmitAccessorIndexer(IndexerExpression indexerExpression, MemberResolveResult memberResolveResult, IProperty member)
        {
            string targetVar = null;
            string valueVar = null;
            bool writeTargetVar = false;
            bool isStatement = false;
            var oldIsAssignment = Emitter.IsAssignment;
            var oldUnary = Emitter.IsUnaryAccessor;
            var isInterfaceMember = false;
            bool nativeImplementation = true;
            var hasTypeParemeter = Helpers.IsTypeParameterType(memberResolveResult.Member.DeclaringType);
            var isExternalInterface = false;

            if (memberResolveResult.Member.DeclaringTypeDefinition != null &&
                memberResolveResult.Member.DeclaringTypeDefinition.Kind == TypeKind.Interface)
            {
                var itypeDef = memberResolveResult.Member.DeclaringTypeDefinition;
                var variance = MetadataUtils.IsJsGeneric(itypeDef, Emitter) &&
                    itypeDef.TypeParameters != null &&
                    itypeDef.TypeParameters.Any(typeParameter => typeParameter.Variance != VarianceModifier.Invariant);

                if (variance)
                {
                    isInterfaceMember = true;
                }
                else
                {
                    var ei =
                        Emitter.Validator.IsExternalInterface(memberResolveResult.Member.DeclaringTypeDefinition);

                    if (ei != null)
                    {
                        nativeImplementation = ei.IsNativeImplementation;
                        isExternalInterface = true;
                    }
                    else
                    {
                        nativeImplementation =
                            memberResolveResult.Member.DeclaringTypeDefinition.ParentAssembly.AssemblyName == CS.NS.H5 ||
                            !Emitter.Validator.IsExternalType(memberResolveResult.Member.DeclaringTypeDefinition);
                    }

                    if (ei != null && ei.IsSimpleImplementation)
                    {
                        nativeImplementation = false;
                        isExternalInterface = false;
                    }
                    else if (hasTypeParemeter || ei != null && !nativeImplementation)
                    {
                        isInterfaceMember = true;
                        writeTargetVar = true;
                    }
                }
            }

            if (Emitter.IsUnaryAccessor)
            {
                writeTargetVar = true;

                isStatement = indexerExpression.Parent is UnaryOperatorExpression &&
                              indexerExpression.Parent.Parent is ExpressionStatement;

                if (memberResolveResult != null && NullableType.IsNullable(memberResolveResult.Type))
                {
                    isStatement = false;
                }

                if (!isStatement)
                {
                    WriteOpenParentheses();
                }
            }

            var targetrr = Emitter.Resolver.ResolveNode(indexerExpression.Target);
            bool isField = targetrr is MemberResolveResult memberTargetrr && memberTargetrr.Member is IField &&
                           (memberTargetrr.TargetResult is ThisResolveResult ||
                           memberTargetrr.TargetResult is TypeResolveResult ||
                            memberTargetrr.TargetResult is LocalResolveResult);
            bool isSimple = targetrr is ThisResolveResult || targetrr is LocalResolveResult ||
                            targetrr is ConstantResolveResult || isField;
            bool needTemp = isExternalInterface && !nativeImplementation && !isSimple;

            if (isInterfaceMember && (!Emitter.IsUnaryAccessor || isStatement) && needTemp)
            {
                WriteOpenParentheses();
            }

            if (writeTargetVar)
            {
                if (needTemp)
                {
                    targetVar = GetTempVarName();
                    Write(targetVar);
                    Write(" = ");
                }
            }

            if (Emitter.IsUnaryAccessor && !isStatement && targetVar == null)
            {
                valueVar = GetTempVarName();

                Write(valueVar);
                Write(" = ");
            }

            Emitter.IsAssignment = false;
            Emitter.IsUnaryAccessor = false;
            indexerExpression.Target.AcceptVisitor(Emitter);
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
                else if (!isInterfaceMember)
                {
                    WriteSemiColon();
                    WriteNewLine();
                    Write(targetVar);
                }
            }

            if (!isInterfaceMember)
            {
                WriteDot();
            }

            bool isBase = indexerExpression.Target is BaseReferenceExpression;

            var argsInfo = new ArgumentsInfo(Emitter, indexerExpression);
            var argsExpressions = argsInfo.ArgumentsExpressions;
            var paramsArg = argsInfo.ParamsExpression;
            var name = Helpers.GetPropertyRef(member, Emitter, Emitter.IsAssignment, ignoreInterface: !nativeImplementation);

            if (!Emitter.IsAssignment)
            {
                if (Emitter.IsUnaryAccessor)
                {
                    var oldWriter = SaveWriter();
                    NewWriter();
                    new ExpressionListBlock(Emitter, argsExpressions, paramsArg, null, 0).Emit();
                    var paramsStr = Emitter.Output.ToString();
                    RestoreWriter(oldWriter);

                    bool isDecimal = Helpers.IsDecimalType(member.ReturnType, Emitter.Resolver);
                    bool isLong = Helpers.Is64Type(member.ReturnType, Emitter.Resolver);
                    bool isNullable = NullableType.IsNullable(member.ReturnType);
                    if (isStatement)
                    {
                        if (isInterfaceMember)
                        {
                            WriteInterfaceMember(targetVar, memberResolveResult, true, JS.Funcs.Property.SET);
                        }
                        else
                        {
                            Write(Helpers.GetPropertyRef(memberResolveResult.Member, Emitter, true, ignoreInterface: !nativeImplementation));
                        }

                        WriteOpenParentheses();
                        Write(paramsStr);
                        WriteComma(false);

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

                                if (targetVar != null)
                                {
                                    Write(targetVar);
                                }
                                else
                                {
                                    indexerExpression.Target.AcceptVisitor(Emitter);
                                }

                                if (!isInterfaceMember)
                                {
                                    WriteDot();
                                    Write(Helpers.GetPropertyRef(member, Emitter, false, ignoreInterface: !nativeImplementation));
                                }
                                else
                                {
                                    WriteInterfaceMember(targetVar, memberResolveResult, false, JS.Funcs.Property.GET);
                                }

                                WriteOpenParentheses();
                                Write(paramsStr);
                                WriteCloseParentheses();

                                WriteCloseParentheses();
                            }
                            else
                            {
                                if (targetVar != null)
                                {
                                    Write(targetVar);
                                }
                                else
                                {
                                    indexerExpression.Target.AcceptVisitor(Emitter);
                                }

                                if (!isInterfaceMember)
                                {
                                    WriteDot();
                                    Write(Helpers.GetPropertyRef(member, Emitter, false, ignoreInterface: !nativeImplementation));
                                }
                                else
                                {
                                    WriteInterfaceMember(targetVar, memberResolveResult, false, JS.Funcs.Property.GET);
                                }

                                WriteOpenParentheses();
                                Write(paramsStr);
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
                                WriteOpenCloseParentheses();
                            }
                        }
                        else
                        {
                            if (targetVar != null)
                            {
                                Write(targetVar);
                            }
                            else
                            {
                                indexerExpression.Target.AcceptVisitor(Emitter);
                            }

                            if (!isInterfaceMember)
                            {
                                WriteDot();
                                Write(Helpers.GetPropertyRef(member, Emitter, false, ignoreInterface: !nativeImplementation));
                            }
                            else
                            {
                                WriteInterfaceMember(targetVar, memberResolveResult, false, JS.Funcs.Property.GET);
                            }

                            WriteOpenParentheses();
                            Write(paramsStr);
                            WriteCloseParentheses();

                            if (Emitter.UnaryOperatorType == UnaryOperatorType.Increment ||
                                Emitter.UnaryOperatorType == UnaryOperatorType.PostIncrement)
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
                        if (!isInterfaceMember)
                        {
                            Write(Helpers.GetPropertyRef(member, Emitter, false, ignoreInterface: !nativeImplementation));
                        }
                        else
                        {
                            WriteInterfaceMember(targetVar, memberResolveResult, false, JS.Funcs.Property.GET);
                        }

                        WriteOpenParentheses();
                        Write(paramsStr);
                        WriteCloseParentheses();
                        WriteComma();

                        if (targetVar != null)
                        {
                            Write(targetVar);
                        }
                        else
                        {
                            indexerExpression.Target.AcceptVisitor(Emitter);
                        }
                        if (!isInterfaceMember)
                        {
                            WriteDot();
                            Write(Helpers.GetPropertyRef(member, Emitter, true, ignoreInterface: !nativeImplementation));
                        }
                        else
                        {
                            WriteInterfaceMember(targetVar, memberResolveResult, true, JS.Funcs.Property.SET);
                        }

                        WriteOpenParentheses();
                        Write(paramsStr);
                        WriteComma(false);

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
                                WriteOpenCloseParentheses();
                            }
                        }
                        else
                        {
                            Write(valueVar);

                            if (Emitter.UnaryOperatorType == UnaryOperatorType.Increment ||
                                Emitter.UnaryOperatorType == UnaryOperatorType.PostIncrement)
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

                        bool isPreOp = Emitter.UnaryOperatorType == UnaryOperatorType.Increment ||
                                       Emitter.UnaryOperatorType == UnaryOperatorType.Decrement;

                        if (isPreOp)
                        {
                            if (targetVar != null)
                            {
                                Write(targetVar);
                            }
                            else
                            {
                                indexerExpression.Target.AcceptVisitor(Emitter);
                            }
                            if (!isInterfaceMember)
                            {
                                WriteDot();
                                Write(Helpers.GetPropertyRef(member, Emitter, false, ignoreInterface: !nativeImplementation));
                            }
                            else
                            {
                                WriteInterfaceMember(targetVar, memberResolveResult, false, JS.Funcs.Property.GET);
                            }
                            WriteOpenParentheses();
                            Write(paramsStr);
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

                    if (targetVar != null)
                    {
                        RemoveTempVar(targetVar);
                    }
                }
                else
                {
                    if (!isInterfaceMember)
                    {
                        Write(name);
                    }
                    else
                    {
                        WriteInterfaceMember(targetVar, memberResolveResult, Emitter.IsAssignment, Helpers.GetSetOrGet(Emitter.IsAssignment));
                    }

                    if (isBase)
                    {
                        WriteCall();
                        WriteOpenParentheses();
                        WriteThis();
                        WriteComma(false);
                    }
                    else
                    {
                        WriteOpenParentheses();
                    }

                    new ExpressionListBlock(Emitter, argsExpressions, paramsArg, null, 0).Emit();
                    WriteCloseParentheses();
                }
            }
            else
            {
                if (Emitter.AssignmentType != AssignmentOperatorType.Assign)
                {
                    var oldWriter = SaveWriter();
                    NewWriter();
                    new ExpressionListBlock(Emitter, argsExpressions, paramsArg, null, 0).Emit();
                    var paramsStr = Emitter.Output.ToString();
                    RestoreWriter(oldWriter);

                    string memberStr;
                    if (isInterfaceMember)
                    {
                        oldWriter = SaveWriter();
                        NewWriter();

                        Emitter.IsAssignment = false;
                        Emitter.IsUnaryAccessor = false;
                        WriteInterfaceMember(targetVar, memberResolveResult, Emitter.IsAssignment, Helpers.GetSetOrGet(Emitter.IsAssignment));
                        Emitter.IsAssignment = oldIsAssignment;
                        Emitter.IsUnaryAccessor = oldUnary;
                        memberStr = Emitter.Output.ToString();
                        RestoreWriter(oldWriter);
                    }
                    else
                    {
                        memberStr = name;
                    }

                    string getterMember;
                    if (isInterfaceMember)
                    {
                        oldWriter = SaveWriter();
                        NewWriter();

                        Emitter.IsAssignment = false;
                        Emitter.IsUnaryAccessor = false;
                        WriteInterfaceMember(targetVar, memberResolveResult, false, JS.Funcs.Property.GET);
                        Emitter.IsAssignment = oldIsAssignment;
                        Emitter.IsUnaryAccessor = oldUnary;
                        getterMember = Emitter.Output.ToString();
                        RestoreWriter(oldWriter);
                    }
                    else
                    {
                        getterMember = "." + Helpers.GetPropertyRef(memberResolveResult.Member, Emitter, false, ignoreInterface: !nativeImplementation);
                    }

                    if (targetVar != null)
                    {
                        PushWriter(string.Concat(
                            memberStr,
                            "(",
                            paramsStr,
                            ", ",
                            targetVar,
                            getterMember,
                            isBase ? "." + JS.Funcs.CALL : "",
                            "(",
                            isBase ? "this, " : "",
                            paramsStr,
                            ") {0})"));

                        RemoveTempVar(targetVar);
                    }
                    else
                    {
                        oldWriter = SaveWriter();
                        NewWriter();

                        Emitter.IsAssignment = false;
                        Emitter.IsUnaryAccessor = false;
                        indexerExpression.Target.AcceptVisitor(Emitter);
                        Emitter.IsAssignment = oldIsAssignment;
                        Emitter.IsUnaryAccessor = oldUnary;

                        var trg = Emitter.Output.ToString();

                        RestoreWriter(oldWriter);
                        PushWriter(string.Concat(
                            memberStr,
                            "(",
                            paramsStr,
                            ", ",
                            trg,
                            getterMember,
                            isBase ? "." + JS.Funcs.CALL : "",
                            "(",
                            isBase ? "this, " : "",
                            paramsStr,
                            ") {0})"));
                    }
                }
                else
                {
                    if (!isInterfaceMember)
                    {
                        Write(name);
                    }
                    else
                    {
                        WriteInterfaceMember(targetVar, memberResolveResult, Emitter.IsAssignment, Helpers.GetSetOrGet(Emitter.IsAssignment));
                    }

                    if (isBase)
                    {
                        WriteCall();
                        WriteOpenParentheses();
                        WriteThis();
                        WriteComma(false);
                    }
                    else
                    {
                        WriteOpenParentheses();
                    }

                    Emitter.IsAssignment = false;
                    Emitter.IsUnaryAccessor = false;
                    new ExpressionListBlock(Emitter, argsExpressions, paramsArg, null, 0).Emit();
                    Emitter.IsAssignment = oldIsAssignment;
                    Emitter.IsUnaryAccessor = oldUnary;
                    PushWriter(", {0})");
                }
            }
        }

        protected virtual void EmitMultiDimArrayAccess(IndexerExpression indexerExpression)
        {
            string targetVar = null;
            bool writeTargetVar = false;
            bool isStatement = false;
            string valueVar = null;
            var resolveResult = Emitter.Resolver.ResolveNode(indexerExpression);

            if (Emitter.IsAssignment && Emitter.AssignmentType != AssignmentOperatorType.Assign)
            {
                writeTargetVar = true;
            }
            else if (Emitter.IsUnaryAccessor)
            {
                writeTargetVar = true;

                isStatement = indexerExpression.Parent is UnaryOperatorExpression && indexerExpression.Parent.Parent is ExpressionStatement;

                if (NullableType.IsNullable(resolveResult.Type))
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
                var targetrr = Emitter.Resolver.ResolveNode(indexerExpression.Target);
                bool isField = targetrr is MemberResolveResult memberTargetrr && memberTargetrr.Member is IField && (memberTargetrr.TargetResult is ThisResolveResult || memberTargetrr.TargetResult is LocalResolveResult);

                if (!(targetrr is ThisResolveResult || targetrr is LocalResolveResult || isField))
                {
                    targetVar = GetTempVarName();

                    Write(targetVar);
                    Write(" = ");
                }
            }

            if (Emitter.IsUnaryAccessor && !isStatement && targetVar == null)
            {
                valueVar = GetTempVarName();

                Write(valueVar);
                Write(" = ");
            }

            var oldIsAssignment = Emitter.IsAssignment;
            var oldUnary = Emitter.IsUnaryAccessor;

            Emitter.IsAssignment = false;
            Emitter.IsUnaryAccessor = false;
            indexerExpression.Target.AcceptVisitor(Emitter);
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

            if (isRefArg)
            {
                WriteComma();
            }
            else
            {
                WriteDot();
            }

            var argsInfo = new ArgumentsInfo(Emitter, indexerExpression);
            var argsExpressions = argsInfo.ArgumentsExpressions;
            var paramsArg = argsInfo.ParamsExpression;

            if (!Emitter.IsAssignment)
            {
                if (Emitter.IsUnaryAccessor)
                {
                    bool isDecimal = Helpers.IsDecimalType(resolveResult.Type, Emitter.Resolver);
                    bool isLong = Helpers.Is64Type(resolveResult.Type, Emitter.Resolver);
                    bool isNullable = NullableType.IsNullable(resolveResult.Type);

                    if (isStatement)
                    {
                        Write(JS.Funcs.Property.SET);
                        WriteOpenParentheses();
                        WriteOpenBracket();
                        new ExpressionListBlock(Emitter, argsExpressions, paramsArg, null, 0).Emit();
                        WriteCloseBracket();
                        WriteComma(false);

                        if (isDecimal || isLong)
                        {
                            if (isNullable)
                            {
                                Write(JS.Types.SYSTEM_NULLABLE + "." + JS.Funcs.Math.LIFT1);
                                WriteOpenParentheses();
                                if (Emitter.UnaryOperatorType == UnaryOperatorType.Increment || Emitter.UnaryOperatorType == UnaryOperatorType.PostIncrement)
                                {
                                    WriteScript(JS.Funcs.Math.INC);
                                }
                                else
                                {
                                    WriteScript(JS.Funcs.Math.DEC);
                                }
                                WriteComma();

                                if (targetVar != null)
                                {
                                    Write(targetVar);
                                }
                                else
                                {
                                    indexerExpression.Target.AcceptVisitor(Emitter);
                                }

                                WriteDot();

                                Write(JS.Funcs.Property.GET);
                                WriteOpenParentheses();
                                WriteOpenBracket();
                                new ExpressionListBlock(Emitter, argsExpressions, paramsArg, null, 0).Emit();
                                WriteCloseBracket();
                                WriteCloseParentheses();
                                WriteCloseParentheses();
                            }
                            else
                            {
                                if (targetVar != null)
                                {
                                    Write(targetVar);
                                }
                                else
                                {
                                    indexerExpression.Target.AcceptVisitor(Emitter);
                                }

                                WriteDot();

                                Write(JS.Funcs.Property.GET);
                                WriteOpenParentheses();
                                WriteOpenBracket();
                                new ExpressionListBlock(Emitter, argsExpressions, paramsArg, null, 0).Emit();
                                WriteCloseBracket();
                                WriteCloseParentheses();
                                WriteDot();
                                if (Emitter.UnaryOperatorType == UnaryOperatorType.Increment || Emitter.UnaryOperatorType == UnaryOperatorType.PostIncrement)
                                {
                                    Write(JS.Funcs.Math.INC);
                                }
                                else
                                {
                                    Write(JS.Funcs.Math.DEC);
                                }

                                WriteOpenCloseParentheses();
                            }
                        }
                        else
                        {
                            if (targetVar != null)
                            {
                                Write(targetVar);
                            }
                            else
                            {
                                indexerExpression.Target.AcceptVisitor(Emitter);
                            }

                            WriteDot();

                            Write(JS.Funcs.Property.GET);
                            WriteOpenParentheses();
                            WriteOpenBracket();
                            new ExpressionListBlock(Emitter, argsExpressions, paramsArg, null, 0).Emit();
                            WriteCloseBracket();
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
                        Write(JS.Funcs.Property.GET);
                        WriteOpenParentheses();
                        WriteOpenBracket();
                        new ExpressionListBlock(Emitter, argsExpressions, paramsArg, null, 0).Emit();
                        WriteCloseBracket();
                        WriteCloseParentheses();
                        WriteComma();

                        if (targetVar != null)
                        {
                            Write(targetVar);
                        }
                        else
                        {
                            indexerExpression.Target.AcceptVisitor(Emitter);
                        }
                        WriteDot();
                        Write(JS.Funcs.Property.SET);
                        WriteOpenParentheses();
                        WriteOpenBracket();
                        new ExpressionListBlock(Emitter, argsExpressions, paramsArg, null, 0).Emit();
                        WriteCloseBracket();
                        WriteComma(false);

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

                                WriteDot();

                                Write(JS.Funcs.Property.GET);
                                WriteOpenParentheses();
                                WriteOpenBracket();
                                new ExpressionListBlock(Emitter, argsExpressions, paramsArg, null, 0).Emit();
                                WriteCloseBracket();
                                WriteCloseParentheses();
                                WriteCloseParentheses();
                            }
                            else
                            {
                                if (targetVar != null)
                                {
                                    Write(targetVar);
                                }
                                else
                                {
                                    indexerExpression.Target.AcceptVisitor(Emitter);
                                }

                                WriteDot();

                                Write(JS.Funcs.Property.GET);
                                WriteOpenParentheses();
                                WriteOpenBracket();
                                new ExpressionListBlock(Emitter, argsExpressions, paramsArg, null, 0).Emit();
                                WriteCloseBracket();
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

                                WriteOpenCloseParentheses();
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

                        var isPreOp = Emitter.UnaryOperatorType == UnaryOperatorType.Increment ||
                                      Emitter.UnaryOperatorType == UnaryOperatorType.Decrement;

                        if (isPreOp)
                        {
                            if (targetVar != null)
                            {
                                Write(targetVar);
                            }
                            else
                            {
                                indexerExpression.Target.AcceptVisitor(Emitter);
                            }

                            WriteDot();

                            Write(JS.Funcs.Property.GET);
                            WriteOpenParentheses();
                            WriteOpenBracket();
                            new ExpressionListBlock(Emitter, argsExpressions, paramsArg, null, 0).Emit();
                            WriteCloseBracket();
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

                    if (targetVar != null)
                    {
                        RemoveTempVar(targetVar);
                    }
                }
                else
                {
                    if (!isRefArg)
                    {
                        Write(JS.Funcs.Property.GET);
                        WriteOpenParentheses();
                    }

                    WriteOpenBracket();
                    new ExpressionListBlock(Emitter, argsExpressions, paramsArg, null, 0).Emit();
                    WriteCloseBracket();
                    if (!isRefArg)
                    {
                        WriteCloseParentheses();
                    }
                }
            }
            else
            {
                if (Emitter.AssignmentType != AssignmentOperatorType.Assign)
                {
                    var oldWriter = SaveWriter();
                    NewWriter();
                    new ExpressionListBlock(Emitter, argsExpressions, paramsArg, null, 0).Emit();
                    var paramsStr = Emitter.Output.ToString();
                    RestoreWriter(oldWriter);

                    if (targetVar != null)
                    {
                        PushWriter(string.Concat(
                            JS.Funcs.Property.SET,
                            "([",
                            paramsStr,
                            "],",
                            targetVar,
                            ".get([",
                            paramsStr,
                            "]) {0})"), () =>
                            {
                                RemoveTempVar(targetVar);
                            });
                    }
                    else
                    {
                        oldWriter = SaveWriter();
                        NewWriter();

                        Emitter.IsAssignment = false;
                        Emitter.IsUnaryAccessor = false;
                        indexerExpression.Target.AcceptVisitor(Emitter);
                        Emitter.IsAssignment = oldIsAssignment;
                        Emitter.IsUnaryAccessor = oldUnary;

                        var trg = Emitter.Output.ToString();

                        RestoreWriter(oldWriter);
                        PushWriter(string.Concat(
                            JS.Funcs.Property.SET,
                            "([",
                            paramsStr,
                            "],",
                            trg,
                            ".get([",
                            paramsStr,
                            "]) {0})"));
                    }
                }
                else
                {
                    Write(JS.Funcs.Property.SET);
                    WriteOpenParentheses();
                    WriteOpenBracket();
                    new ExpressionListBlock(Emitter, argsExpressions, paramsArg, null, 0).Emit();
                    WriteCloseBracket();
                    PushWriter(", {0})");
                }
            }
        }

        protected virtual void EmitSingleDimArrayIndexer(IndexerExpression indexerExpression)
        {
            var oldIsAssignment = Emitter.IsAssignment;
            var oldUnary = Emitter.IsUnaryAccessor;
            Emitter.IsAssignment = false;
            Emitter.IsUnaryAccessor = false;

            var targetrr = Emitter.Resolver.ResolveNode(indexerExpression.Target);
            bool isField = targetrr is MemberResolveResult memberTargetrr && memberTargetrr.Member is IField &&
                           (memberTargetrr.TargetResult is ThisResolveResult ||
                            memberTargetrr.TargetResult is TypeResolveResult ||
                            memberTargetrr.TargetResult is LocalResolveResult);
            bool isArray = targetrr.Type.Kind == TypeKind.Array && !ConversionBlock.IsInUncheckedContext(Emitter, indexerExpression, false);
            bool isSimple = !isArray || (targetrr is ThisResolveResult || targetrr is LocalResolveResult ||
                            targetrr is ConstantResolveResult || isField);
            string targetVar = null;

            if (!isSimple)
            {
                WriteOpenParentheses();
                targetVar = GetTempVarName();
                Write(targetVar);
                Write(" = ");
            }


            if (indexerExpression.Target is BaseReferenceExpression && Emitter.Resolver.ResolveNode(indexerExpression) is MemberResolveResult rr && Emitter.Validator.IsExternalType(rr.Member.DeclaringTypeDefinition) && !Emitter.Validator.IsH5Class(rr.Member.DeclaringTypeDefinition))
            {
                Write("this");
            }
            else
            {
                indexerExpression.Target.AcceptVisitor(Emitter);
            }

            if (!isSimple)
            {
                WriteCloseParentheses();
            }

            Emitter.IsAssignment = oldIsAssignment;
            Emitter.IsUnaryAccessor = oldUnary;

            if (indexerExpression.Arguments.Count != 1)
            {
                throw new EmitterException(indexerExpression, "Only one index is supported");
            }

            var index = indexerExpression.Arguments.First();


            if (!isArray && index is PrimitiveExpression primitive && primitive.Value != null &&
                Regex.Match(primitive.Value.ToString(), "^[_$a-z][_$a-z0-9]*$", RegexOptions.IgnoreCase).Success)
            {
                if (isRefArg)
                {
                    WriteComma();
                    WriteScript(primitive.Value);
                }
                else
                {
                    WriteDot();
                    Write(primitive.Value);
                }
            }
            else
            {
                Emitter.IsAssignment = false;
                Emitter.IsUnaryAccessor = false;
                if (isRefArg)
                {
                    WriteComma();
                }
                else
                {
                    WriteOpenBracket();
                    if (isArray && Emitter.Rules.ArrayIndex == ArrayIndexRule.Managed)
                    {
                        Write(JS.Types.System.Array.INDEX);
                        WriteOpenParentheses();
                    }
                }

                index.AcceptVisitor(Emitter);

                if (!isRefArg)
                {
                    if (isArray && Emitter.Rules.ArrayIndex == ArrayIndexRule.Managed)
                    {
                        WriteComma();

                        if (targetVar != null)
                        {
                            Write(targetVar);
                        }
                        else
                        {
                            indexerExpression.Target.AcceptVisitor(Emitter);
                        }

                        WriteCloseParentheses();
                    }
                    WriteCloseBracket();
                }

                Emitter.IsAssignment = oldIsAssignment;
                Emitter.IsUnaryAccessor = oldUnary;
            }
        }
    }
}