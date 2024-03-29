using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.TypeSystem.Implementation;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.CSharp.Resolver;
using Object.Net.Utilities;

namespace H5.Translator
{
    public abstract partial class ConversionBlock : AbstractEmitterBlock
    {
        public ConversionBlock(IEmitter emitter, AstNode node)
            : base(emitter, node)
        {
        }

        protected sealed override void DoEmit()
        {
            AfterOutput = "";
            AfterOutput2 = "";
            var expression = GetExpression();

            if (expressionInWork.Contains(expression))
            {
                if (!expressionIgnoreUserDefine.Contains(expression))
                {
                    if (expressionMap.ContainsKey(expression))
                    {
                        Write(expressionMap[expression]);
                    }
                    else
                    {
                        EmitConversionExpression();
                    }

                    return;
                }
            }
            else
            {
                expressionInWork.Add(expression);
            }

            int parenthesesLevel = 0;
            bool check = expression != null && !expression.IsNull && expression.Parent != null;

            if (check)
            {
                parenthesesLevel = CheckConversion(expression);
            }

            if (DisableEmitConversionExpression)
            {
                if (expressionInWork.Contains(expression) && !expressionIgnoreUserDefine.Contains(expression))
                {
                    expressionInWork.Remove(expression);
                }
                return;
            }

            if (expression != null && expressionMap.ContainsKey(expression))
            {
                Write(expressionMap[expression]);
            }
            else
            {
                EmitConversionExpression();
            }

            if (expressionInWork.Contains(expression) && !expressionIgnoreUserDefine.Contains(expression))
            {
                expressionInWork.Remove(expression);
            }

            if (parenthesesLevel > 0)
            {
                for (int i = 0; i < parenthesesLevel; i++)
                {
                    WriteCloseParentheses();
                }
            }

            if (AfterOutput.Length > 0)
            {
                Write(AfterOutput);
            }

            if (AfterOutput2.Length > 0)
            {
                Write(AfterOutput2);
            }
        }

        protected virtual string AfterOutput { get; set; }

        protected virtual string AfterOutput2 { get; set; }

        internal static List<Expression> expressionInWork = new List<Expression>();
        internal static List<Expression> expressionIgnoreUserDefine = new List<Expression>();
        internal static Dictionary<Expression, string> expressionMap = new Dictionary<Expression, string>();

        protected virtual bool DisableEmitConversionExpression { get; set; }

        protected virtual int CheckConversion(Expression expression)
        {
            return CheckConversion(this, expression);
        }

        public static bool IsUserDefinedConversion(AbstractEmitterBlock block, Expression expression)
        {
            Conversion conversion = null;

            try
            {
                // The resolveNode call required to get GetConversion not fail
                block.Emitter.Resolver.ResolveNode(expression);
                conversion = block.Emitter.Resolver.Resolver.GetConversion(expression);

                if (conversion == null)
                {
                    return false;
                }

                return conversion.IsUserDefined;
            }
            catch
            {
            }

            return false;
        }

        public static int CheckConversion(ConversionBlock block, Expression expression)
        {
            try
            {
                var rr = block.Emitter.Resolver.ResolveNode(expression);
                var conversion = block.Emitter.Resolver.Resolver.GetConversion(expression);
                var expectedType = block.Emitter.Resolver.Resolver.GetExpectedType(expression);
                int level = 0;

                if (expression.Parent is ConstructorInitializer ci && expectedType.Equals(rr.Type))
                {
                    var prr = block.Emitter.Resolver.ResolveNode(expression.Parent);

                    if (prr is InvocationResolveResult irr)
                    {
                        var args = irr.GetArgumentsForCall();

                        for (int i = 0; i < ci.Arguments.Count; i++)
                        {
                            var item = ci.Arguments.ElementAt(i);
                            if (item.Equals(expression))
                            {
                                if(args.Count > i)
                                {
                                    expectedType = args[i].Type;
                                }

                                break;
                            }
                        }
                    }
                }
                else if (expression.Parent is ParenthesizedExpression && expression.Parent.Parent is CastExpression)
                {
                    var prr = block.Emitter.Resolver.ResolveNode(expression.Parent);
                    var pconversion = block.Emitter.Resolver.Resolver.GetConversion((Expression)expression.Parent);
                    var pexpectedType = block.Emitter.Resolver.Resolver.GetExpectedType((Expression)expression.Parent);

                    if (pconversion.IsBoxingConversion)
                    {
                        rr = prr;
                        conversion = pconversion;
                        expectedType = pexpectedType;
                        expression = (Expression)expression.Parent;
                    }
                }

                return DoConversion(block, expression, conversion, expectedType, level, rr);
            }
            catch
            {
            }

            return 0;
        }

        internal static string GetBoxedType(IType itype, IEmitter emitter)
        {
            if (NullableType.IsNullable(itype))
            {
                itype = NullableType.GetUnderlyingType(itype);
            }

            return H5Types.ToJsName(itype, emitter);
        }

        internal static string GetInlineMethod(IEmitter emitter, string name, IType returnType, IType type, Expression expression)
        {
            var methodDef = GetBoxedMethod(name, returnType, type);

            if (methodDef != null)
            {
                var inline = emitter.GetInline(methodDef);

                if (inline != null)
                {
                    bool isNullable = NullableType.IsNullable(type);

                    if (isNullable)
                    {
                        string template = "System.Nullable.{1}Fn({0})";
                        var methodRef = GetInlineMethod(emitter, name, returnType,
                            NullableType.GetUnderlyingType(type), expression);

                        return methodRef == null ? $"System.Nullable.{name.ToLowerCamelCase()}" : string.Format(template, methodRef, name.ToLowerCamelCase());
                    }

                    var attr = methodDef.Attributes.First(a => a.AttributeType.FullName == "H5.TemplateAttribute");
                    bool delegated = false;
                    if (attr != null && attr.NamedArguments.Count > 0)
                    {
                        var namedArg = attr.NamedArguments.FirstOrDefault(arg => arg.Key.Name == CS.Attributes.Template.PROPERTY_FN);

                        if (namedArg.Key != null)
                        {
                            inline = namedArg.Value.ConstantValue.ToString();
                            delegated = true;
                        }
                    }

                    var writer = new Writer
                    {
                        InlineCode = inline,
                        Output = emitter.Output,
                        IsNewLine = emitter.IsNewLine
                    };
                    emitter.IsNewLine = false;
                    emitter.Output = new StringBuilder();

                    var argsInfo = new ArgumentsInfo(emitter, expression, (IMethod)methodDef);
                    argsInfo.ArgumentsExpressions = new Expression[] {expression};
                    argsInfo.ArgumentsNames = new string[] {"this"};
                    argsInfo.ThisArgument = "obj";
                    argsInfo.ThisType = type;
                    new InlineArgumentsBlock(emitter, argsInfo, writer.InlineCode).Emit();

                    var result = emitter.Output.ToString();
                    emitter.Output = writer.Output;
                    emitter.IsNewLine = writer.IsNewLine;

                    result = delegated ? result : string.Format("function (obj) {{ return {0}; }}", result);
                    return result;
                }

            }

            return null;
        }

        internal static IMember GetBoxedMethod(string name, IType returnType, IType type)
        {
            return type.GetMembers().FirstOrDefault(m =>
            {
                if (m.Name == name && !m.IsStatic && m.ReturnType.Equals(returnType) && m.IsOverride)
                {
                    if (m is IMethod method && method.Parameters.Count == 0 && method.TypeParameters.Count == 0)
                    {
                        return true;
                    }
                }

                return false;
            });
        }

        private static bool IsUnpackGenericInterfaceObject(IType interfaceType)
        {
            return interfaceType.Kind == TypeKind.Interface && interfaceType.Namespace == "System" && (interfaceType.Name == "IComparable" || interfaceType.Name == "IEquatable" || interfaceType.Name == "IFormattable");
        }

        private static bool IsUnpackGenericArrayInterfaceObject(IType interfaceType)
        {
            if (interfaceType is ParameterizedType pt)
            {
                KnownTypeCode tc = pt.GetDefinition().KnownTypeCode;
                if (tc == KnownTypeCode.IListOfT || tc == KnownTypeCode.ICollectionOfT || tc == KnownTypeCode.IEnumerableOfT || tc == KnownTypeCode.IReadOnlyListOfT)
                {
                    var type = pt.GetTypeArgument(0);
                    return type.IsKnownType(KnownTypeCode.Object) || IsUnpackGenericInterfaceObject(type);
                }
            }

            if (interfaceType is TypeWithElementType typeWithEl)
            {
                var type = typeWithEl.ElementType;
                return type.IsKnownType(KnownTypeCode.Object) || IsUnpackGenericInterfaceObject(type);
            }

            return false;
        }

        private static bool IsUnpackArrayObject(IType type)
        {
            if (type is TypeWithElementType typeWithEl)
            {
                var elementType = typeWithEl.ElementType;
                return elementType.IsKnownType(KnownTypeCode.Object);
            }

            return false;
        }

        private static int DoConversion(ConversionBlock block, Expression expression, Conversion conversion, IType expectedType,
            int level, ResolveResult rr, bool ignoreConversionResolveResult = false, bool ignoreBoxing = false)
        {
            bool isExtensionMethodArgument = false;
            bool isBoxing = conversion.IsBoxingConversion;
            if (expression.Parent is MemberReferenceExpression && expression.Parent.Parent is InvocationExpression)
            {
                if (block.Emitter.Resolver.ResolveNode(expression.Parent.Parent) is CSharpInvocationResolveResult inv_rr && inv_rr.IsExtensionMethodInvocation)
                {
                    conversion = block.Emitter.Resolver.Resolver.GetConversion((Expression)expression.Parent);
                    isExtensionMethodArgument = true;

                    if (expression.Parent.Parent is InvocationExpression ie && ie.Target is MemberReferenceExpression mre && expression.Equals(mre.Target))
                    {
                        isBoxing = conversion.IsBoxingConversion;
                    }
                }
            }

            if (expressionIgnoreUserDefine.Contains(expression) && conversion.IsUserDefined)
            {
                expectedType = conversion.Method.Parameters.First().Type;
            }

            if (block.Emitter.IsAssignment)
            {
                return level;
            }

            if (!ignoreBoxing && expectedType.Kind != TypeKind.Dynamic)
            {
                var isArgument = isExtensionMethodArgument;

                if (!isArgument)
                {
                    isArgument = expression.Parent is InvocationExpression inv && inv.Arguments.Contains(expression);
                }

                if (!isArgument)
                {
                    isArgument = expression.Parent is IndexerExpression idx && idx.Arguments.Contains(expression);
                }

                if (!isArgument)
                {
                    isArgument = expression.Parent is AssignmentExpression ae && ae.Right == expression;
                }

                if (isArgument)
                {
                    var inv_rr = block.Emitter.Resolver.ResolveNode(expression.Parent);
                    var parent_rr = inv_rr as MemberResolveResult;

                    if (parent_rr == null && inv_rr is OperatorResolveResult result)
                    {
                        var orr = result;
                        if (orr.Operands[0] is LocalResolveResult)
                        {
                            isArgument = false;
                        }
                        else if (orr.Operands[0] is ArrayAccessResolveResult)
                        {
                            isArgument = false;
                        }
                        else if (orr.Operands[0] is MemberResolveResult memberResolveResult)
                        {
                            parent_rr = memberResolveResult;
                        }
                    }

                    if (parent_rr != null)
                    {
                        var memberDeclaringTypeDefinition = parent_rr.Member.DeclaringTypeDefinition;
                        isArgument = (block.Emitter.Validator.IsExternalType(memberDeclaringTypeDefinition) || block.Emitter.Validator.IsExternalType(parent_rr.Member))
                                     && !(memberDeclaringTypeDefinition.Namespace == CS.NS.SYSTEM || memberDeclaringTypeDefinition.Namespace.StartsWith(CS.NS.SYSTEM + "."));

                        var attr = parent_rr.Member.Attributes.FirstOrDefault(a => a.AttributeType.FullName == "H5.UnboxAttribute");

                        if (attr != null)
                        {
                            isArgument = (bool)attr.PositionalArguments.First().ConstantValue;
                        }
                        else
                        {
                            attr = memberDeclaringTypeDefinition.Attributes.FirstOrDefault(a => a.AttributeType.FullName == "H5.UnboxAttribute");

                            if (attr != null)
                            {
                                isArgument = (bool)attr.PositionalArguments.First().ConstantValue;
                            }
                        }
                    }
                }

                var nobox = block.Emitter.TemplateModifier == "nobox";
                var isStringConcat = false;
                if (expression.Parent is BinaryOperatorExpression binaryOperatorExpression)
                {
                    var resolveOperator = block.Emitter.Resolver.ResolveNode(binaryOperatorExpression);
                    var expectedParentType = block.Emitter.Resolver.Resolver.GetExpectedType(binaryOperatorExpression);
                    var resultIsString = expectedParentType.IsKnownType(KnownTypeCode.String) || resolveOperator.Type.IsKnownType(KnownTypeCode.String);
                    isStringConcat = resultIsString && binaryOperatorExpression.Operator == BinaryOperatorType.Add;
                }

                bool needBox = IsBoxable(rr.Type, block.Emitter)
                    || rr.Type.IsKnownType(KnownTypeCode.NullableOfT) && IsBoxable(NullableType.GetUnderlyingType(rr.Type), block.Emitter);
                var nullable = rr.Type.IsKnownType(KnownTypeCode.NullableOfT);

                if (isBoxing && !isStringConcat && block.Emitter.Rules.Boxing == BoxingRule.Managed)
                {
                    if (!nobox && needBox && !isArgument)
                    {
                        block.Write(JS.Types.H5.BOX);
                        block.WriteOpenParentheses();
                        block.AfterOutput2 += ", " + GetBoxedType(rr.Type, block.Emitter);

                        var inlineMethod = GetInlineMethod(block.Emitter, CS.Methods.TOSTRING,
                            block.Emitter.Resolver.Compilation.FindType(KnownTypeCode.String), rr.Type, expression);

                        if (inlineMethod != null)
                        {
                            block.AfterOutput2 += ", " + inlineMethod;
                        }

                        inlineMethod = GetInlineMethod(block.Emitter, CS.Methods.GETHASHCODE,
                            block.Emitter.Resolver.Compilation.FindType(KnownTypeCode.Int32), rr.Type, expression);

                        if (inlineMethod != null)
                        {
                            block.AfterOutput2 += ", " + inlineMethod;
                        }

                        block.AfterOutput2 += ")";

                        if (rr.Type.Kind == TypeKind.TypeParameter)
                        {
                            block.Emitter.ForbidLifting = true;
                        }
                    }
                    else  if (!Helpers.IsImmutableStruct(block.Emitter, NullableType.GetUnderlyingType(rr.Type)))
                    {
                        if (nullable)
                        {
                            block.Write(JS.Types.SYSTEM_NULLABLE + "." + JS.Funcs.Math.LIFT1 + "(\"" + JS.Funcs.CLONE + "\", ");
                            block.AfterOutput2 += ")";
                        }
                        else
                        {
                            block.AfterOutput2 += "." + JS.Funcs.CLONE + "()";
                        }
                    }
                }

                if (conversion.IsUnboxingConversion || isArgument && (expectedType.IsKnownType(KnownTypeCode.Object) || IsUnpackArrayObject(expectedType)) && (rr.Type.IsKnownType(KnownTypeCode.Object) || IsUnpackGenericInterfaceObject(rr.Type) || IsUnpackGenericArrayInterfaceObject(rr.Type)))
                {
                    if (!nobox && block.Emitter.Rules.Boxing == BoxingRule.Managed)
                    {
                        block.Write(JS.Types.H5.UNBOX);
                        block.WriteOpenParentheses();
                        if (conversion.IsUnboxingConversion)
                        {
                            block.AfterOutput2 += string.Format(", {0})", GetBoxedType(expectedType, block.Emitter));
                        }
                        else
                        {
                            block.AfterOutput2 += ")";
                        }

                    }
                }
                else if (conversion.IsUnboxingConversion && !Helpers.IsImmutableStruct(block.Emitter, NullableType.GetUnderlyingType(rr.Type)))
                {
                    if (nullable)
                    {
                        block.Write(JS.Types.SYSTEM_NULLABLE + "." + JS.Funcs.Math.LIFT1 + "(\"" + JS.Funcs.CLONE + "\", ");
                        block.AfterOutput2 += ")";
                    }
                    else
                    {
                        block.AfterOutput2 += "." + JS.Funcs.CLONE + "()";
                    }
                }
            }

            if (expression is ParenthesizedExpression parExpression && parExpression.Expression is CastExpression)
            {
                return level;
            }

            if (conversion.IsUserDefined && expression.Parent is CastExpression castExp && castExp.Expression == expression)
            {
                var parentConversion = block.Emitter.Resolver.Resolver.GetConversion(castExp);

                if (!parentConversion.IsUserDefined || parentConversion.Method.Equals(conversion.Method))
                {
                    return level;
                }
            }

            if (rr is ConstantResolveResult && expression is CastExpression && !conversion.IsUserDefined)
            {
                return level;
            }

            var convrr = rr as ConversionResolveResult;

            if (convrr != null && convrr.Input is ConstantResolveResult && !convrr.Conversion.IsUserDefined)
            {
                return level;
            }

            string afterUserDefined = "";
            if (convrr != null && !conversion.IsUserDefined && !ignoreConversionResolveResult)
            {
                if (expectedType != convrr.Type)
                {
                    if (expression.Parent is CastExpression castExp2 && castExp2.Expression == expression)
                    {
                        var parentExpectedType = block.Emitter.Resolver.Resolver.GetExpectedType(castExp2);
                        var parentrr = block.Emitter.Resolver.ResolveNode(expression.Parent) as ConversionResolveResult;

                        if (parentrr != null && parentrr.Type != expectedType || parentrr == null && expectedType != parentExpectedType)
                        {
                            level = DoConversion(block, expression, conversion, expectedType, level, rr, true, true);
                            afterUserDefined = block.AfterOutput;
                            block.AfterOutput = "";
                        }
                    }
                    else
                    {
                        level = DoConversion(block, expression, conversion, expectedType, level, rr, true, true);
                        afterUserDefined = block.AfterOutput;
                        block.AfterOutput = "";
                    }
                }

                conversion = convrr.Conversion;
                rr = convrr.Input;
                expectedType = convrr.Type;
            }

            var isNumLifted = conversion != null && conversion.IsImplicit && conversion.IsLifted &&
                              conversion.IsNumericConversion && !(expression is BinaryOperatorExpression);
            bool skipUserConversion = conversion == null || (!conversion.IsUserDefined &&
                                                             (Helpers.IsDecimalType(expectedType, block.Emitter.Resolver) ||
                                                              Helpers.Is64Type(expectedType, block.Emitter.Resolver) ||
                                                              conversion.IsIdentityConversion ||
                                                              isNumLifted));

            if (!skipUserConversion && conversion.IsUserDefined &&
                (expression is BinaryOperatorExpression || expression is UnaryOperatorExpression ||
                 expression.Parent is AssignmentExpression))
            {
                level = CheckUserDefinedConversion(block, expression, conversion, level, rr, expectedType);

                if (conversion.IsUserDefined && block.DisableEmitConversionExpression)
                {
                    return level;
                }

                afterUserDefined = block.AfterOutput + afterUserDefined;
                block.AfterOutput = "";
            }

            if (!(conversion.IsExplicit && conversion.IsNumericConversion))
            {
                if (CheckDecimalConversion(block, expression, rr, expectedType, conversion, ignoreConversionResolveResult))
                {
                    block.AfterOutput += ")";
                }
            }

            if (Helpers.IsDecimalType(expectedType, block.Emitter.Resolver) && !conversion.IsUserDefined)
            {
                var s = block.AfterOutput;
                block.AfterOutput = "";
                if (!((expression.Parent is CastExpression) && !(expression is CastExpression)))
                {
                    CheckNumericConversion(block, expression, rr, expectedType, conversion);
                }

                block.AfterOutput =block.AfterOutput + s + afterUserDefined;
                return level;
            }

            if (!(conversion.IsExplicit && conversion.IsNumericConversion))
            {
                if (CheckLongConversion(block, expression, rr, expectedType, conversion, ignoreConversionResolveResult))
                {
                    block.AfterOutput += ")";
                }
            }

            if (Helpers.Is64Type(expectedType, block.Emitter.Resolver) && !conversion.IsUserDefined)
            {
                var s = block.AfterOutput;
                block.AfterOutput = "";
                if (!((expression.Parent is CastExpression) && !(expression is CastExpression)))
                {
                    CheckNumericConversion(block, expression, rr, expectedType, conversion);
                }

                block.AfterOutput = block.AfterOutput + s + afterUserDefined;
                return level;
            }

            if (!((expression.Parent is CastExpression) && !(expression is CastExpression)))
            {
                CheckNumericConversion(block, expression, rr, expectedType, conversion);
            }

            if (conversion.IsIdentityConversion)
            {
                block.AfterOutput = block.AfterOutput + afterUserDefined;
                return level;
            }

            if (isNumLifted && !conversion.IsUserDefined)
            {
                block.AfterOutput = block.AfterOutput + afterUserDefined;
                return level;
            }
            bool isLifted = conversion.IsLifted && !isNumLifted && !(block is CastBlock) &&
                            !Helpers.IsDecimalType(expectedType, block.Emitter.Resolver) &&
                            !Helpers.Is64Type(expectedType, block.Emitter.Resolver) && !NullableType.IsNullable(expectedType);
            if (isLifted)
            {
                block.Write(JS.Types.SYSTEM_NULLABLE + ".getValue(");
                block.AfterOutput += ")";
            }

            if (conversion.IsUserDefined &&
                !(expression is BinaryOperatorExpression || expression is UnaryOperatorExpression ||
                  expression.Parent is AssignmentExpression))
            {
                level = CheckUserDefinedConversion(block, expression, conversion, level, rr, expectedType);
            }

            block.AfterOutput = block.AfterOutput + afterUserDefined;
            return level;
        }

        public static bool IsBoxable(IType type, IEmitter emitter)
        {
            if (type.Kind == TypeKind.Enum && emitter.Validator.IsExternalType(type.GetDefinition()))
            {
                var enumMode = Helpers.EnumEmitMode(type);
                if (enumMode >= 3 && enumMode < 7 || enumMode == 2)
                {
                    return false;
                }
            }

            return type.Kind == TypeKind.Enum
                   || type.IsKnownType(KnownTypeCode.Enum)
                   || type.IsKnownType(KnownTypeCode.Boolean)
                   || type.IsKnownType(KnownTypeCode.DateTime)
                   || type.IsKnownType(KnownTypeCode.Char)
                   || type.IsKnownType(KnownTypeCode.Byte)
                   || type.IsKnownType(KnownTypeCode.Double)
                   || type.IsKnownType(KnownTypeCode.Single)
                   || type.IsKnownType(KnownTypeCode.Int16)
                   || type.IsKnownType(KnownTypeCode.Int32)
                   || type.IsKnownType(KnownTypeCode.SByte)
                   || type.IsKnownType(KnownTypeCode.UInt16)
                   || type.IsKnownType(KnownTypeCode.UInt32);
        }

        private static int CheckUserDefinedConversion(ConversionBlock block, Expression expression, Conversion conversion, int level, ResolveResult rr, IType expectedType)
        {
            if (conversion.IsUserDefined && !expressionIgnoreUserDefine.Contains(expression))
            {
                var method = conversion.Method;

                string inline = block.Emitter.GetInline(method);

                if (conversion.IsExplicit && !string.IsNullOrWhiteSpace(inline))
                {
                    // Still returns true if Nullable.lift( was written.
                    return level;
                }

                if (!string.IsNullOrWhiteSpace(inline))
                {
                    expressionIgnoreUserDefine.Add(expression);

                    if (expression is InvocationExpression invocExp)
                    {
                        new InlineArgumentsBlock(block.Emitter, new ArgumentsInfo(block.Emitter, invocExp, method), inline, method).Emit();
                    }
                    else if (expression is ObjectCreateExpression createExp)
                    {
                        new InlineArgumentsBlock(block.Emitter, new ArgumentsInfo(block.Emitter, createExp, method), inline).Emit();
                    }
                    /*else if (expression is UnaryOperatorExpression)
                    {
                        var unaryExpression = (UnaryOperatorExpression)expression;
                        var resolveOperator = block.Emitter.Resolver.ResolveNode(unaryExpression);
                        OperatorResolveResult orr = resolveOperator as OperatorResolveResult;
                        if (orr != null)
                        {
                            new InlineArgumentsBlock(block.Emitter,
                                new ArgumentsInfo(block.Emitter, unaryExpression, orr, method), inline).Emit();
                        }
                        else
                        {
                            new InlineArgumentsBlock(block.Emitter, new ArgumentsInfo(block.Emitter, expression, method), inline, method).Emit();
                        }
                    }*/
                    else
                    {
                        new InlineArgumentsBlock(block.Emitter, new ArgumentsInfo(block.Emitter, expression, method), inline, method).Emit();
                    }

                    block.DisableEmitConversionExpression = true;
                    expressionIgnoreUserDefine.Remove(expression);

                    // Still returns true if Nullable.lift( was written.
                    return level;
                }
                else
                {
                    if (method.DeclaringTypeDefinition != null && (block.Emitter.Validator.IsExternalType(method.DeclaringTypeDefinition) || Helpers.IsIgnoreCast(method.DeclaringTypeDefinition, block.Emitter)))
                    {
                        // Still returns true if Nullable.lift( was written.
                        return level;
                    }

                    block.Write(H5Types.ToJsName(method.DeclaringType, block.Emitter));
                    block.WriteDot();

                    block.Write(OverloadsCollection.Create(block.Emitter, method).GetOverloadName());
                }

                block.WriteOpenParentheses();
                block.AfterOutput += ")";

                var arg = method.Parameters[0];

                if (Helpers.IsDecimalType(arg.Type, block.Emitter.Resolver, arg.IsParams) && !Helpers.IsDecimalType(rr.Type, block.Emitter.Resolver) && !expression.IsNull)
                {
                    block.Write(JS.Types.SYSTEM_DECIMAL);
                    if (NullableType.IsNullable(arg.Type) && ShouldBeLifted(expression))
                    {
                        block.Write("." + JS.Funcs.Math.LIFT);
                    }
                    if (expression is CastExpression castExp && castExp.Expression is ParenthesizedExpression)
                    {
                        return level;
                    }
                    block.WriteOpenParentheses();
                    block.AfterOutput += ")";
                }

                if (Helpers.Is64Type(arg.Type, block.Emitter.Resolver, arg.IsParams) && !Helpers.Is64Type(rr.Type, block.Emitter.Resolver) && !expression.IsNull)
                {
                    var isUint = Helpers.IsULongType(arg.Type, block.Emitter.Resolver, arg.IsParams);
                    block.Write(isUint ? JS.Types.SYSTEM_UInt64 : JS.Types.System.Int64.NAME);
                    if (NullableType.IsNullable(arg.Type) && ShouldBeLifted(expression))
                    {
                        block.Write("." + JS.Funcs.Math.LIFT);
                    }
                    if (expression is CastExpression castExp && castExp.Expression is ParenthesizedExpression)
                    {
                        return level;
                    }
                    block.WriteOpenParentheses();
                    block.AfterOutput += ")";
                }
            }

            return level;
        }

        private delegate bool IsType(IType type, IMemberResolver resolver, bool allowArray = false);

        private static bool CheckTypeConversion(ConversionBlock block, Expression expression, ResolveResult rr, IType expectedType, Conversion conversion, string typeName, IsType isType, bool ignoreConversionResolveResult)
        {
            if (conversion.IsUserDefined)
            {
                var m = conversion.Method;
                if (isType(m.ReturnType, block.Emitter.Resolver))
                {
                    return false;
                }
            }

            if (expression is CastExpression castExp && !ignoreConversionResolveResult)
            {
                var nestedExpr = castExp.Expression;
                var nested_rr = block.Emitter.Resolver.ResolveNode(nestedExpr);

                if (!(nested_rr is ConversionResolveResult))
                {
                    return false;
                }
            }

            if (expression.Parent is InvocationExpression invocationExpression && invocationExpression.Arguments.Any(a => a == expression))
            {
                var index = invocationExpression.Arguments.ToList().IndexOf(expression);
                var methodResolveResult = block.Emitter.Resolver.ResolveNode(invocationExpression) as MemberResolveResult;

                if (methodResolveResult is CSharpInvocationResolveResult invocationResolveResult && invocationResolveResult.IsExtensionMethodInvocation)
                {
                    index++;
                }

                if (methodResolveResult != null)
                {
                    var m = methodResolveResult.Member as IMethod;
                    var arg = m.Parameters[index < m.Parameters.Count ? index : (m.Parameters.Count - 1)];

                    if (isType(arg.Type, block.Emitter.Resolver, arg.IsParams) && !isType(rr.Type, block.Emitter.Resolver))
                    {
                        if (expression.IsNull)
                        {
                            return false;
                        }

                        block.Write(typeName);
                        if (NullableType.IsNullable(arg.Type) && ShouldBeLifted(expression))
                        {
                            block.Write("." + JS.Funcs.Math.LIFT);
                        }
                        if (!ignoreConversionResolveResult && expression is CastExpression castExp2 && castExp2.Expression is ParenthesizedExpression)
                        {
                            return false;
                        }
                        block.WriteOpenParentheses();
                        return true;
                    }
                }
            }

            if (expression.Parent is ObjectCreateExpression objectCreateExpression && objectCreateExpression.Arguments.Any(a => a == expression))
            {
                var index = objectCreateExpression.Arguments.ToList().IndexOf(expression);

                if (block.Emitter.Resolver.ResolveNode(objectCreateExpression) is MemberResolveResult methodResolveResult)
                {
                    var m = methodResolveResult.Member as IMethod;
                    var arg = m.Parameters[index < m.Parameters.Count ? index : (m.Parameters.Count - 1)];

                    if (isType(arg.Type, block.Emitter.Resolver, arg.IsParams) && !isType(rr.Type, block.Emitter.Resolver))
                    {
                        if (expression.IsNull)
                        {
                            return false;
                        }

                        block.Write(typeName);
                        if (NullableType.IsNullable(arg.Type) && ShouldBeLifted(expression))
                        {
                            block.Write("." + JS.Funcs.Math.LIFT);
                        }
                        if (!ignoreConversionResolveResult && expression is CastExpression castExp3 && castExp3.Expression is ParenthesizedExpression)
                        {
                            return false;
                        }
                        block.WriteOpenParentheses();
                        return true;
                    }
                }
            }

            if (expression.Parent is NamedArgumentExpression namedArgExpression)
            {
                var namedArgResolveResult = block.Emitter.Resolver.ResolveNode(namedArgExpression) as NamedArgumentResolveResult;

                if (isType(namedArgResolveResult.Type, block.Emitter.Resolver) && !isType(rr.Type, block.Emitter.Resolver))
                {
                    if (expression.IsNull)
                    {
                        return false;
                    }

                    block.Write(typeName);
                    if (NullableType.IsNullable(namedArgResolveResult.Type) && ShouldBeLifted(expression))
                    {
                        block.Write("." + JS.Funcs.Math.LIFT);
                    }
                    if (!ignoreConversionResolveResult && expression is CastExpression castExp4 && castExp4.Expression is ParenthesizedExpression)
                    {
                        return false;
                    }
                    block.WriteOpenParentheses();
                    return true;
                }
            }

            if (expression.Parent is NamedExpression namedExpression)
            {
                var namedResolveResult = block.Emitter.Resolver.ResolveNode(namedExpression);

                if (isType(namedResolveResult.Type, block.Emitter.Resolver) && !isType(rr.Type, block.Emitter.Resolver))
                {
                    if (expression.IsNull)
                    {
                        return false;
                    }

                    block.Write(typeName);
                    if (NullableType.IsNullable(namedResolveResult.Type) && ShouldBeLifted(expression))
                    {
                        block.Write("." + JS.Funcs.Math.LIFT);
                    }
                    if (!ignoreConversionResolveResult && expression is CastExpression castExp5 && castExp5.Expression is ParenthesizedExpression)
                    {
                        return false;
                    }
                    block.WriteOpenParentheses();
                    return true;
                }
            }

            if (expression.Parent is BinaryOperatorExpression binaryOpExpr)
            {
                var idx = binaryOpExpr.Left == expression ? 0 : 1;

                if (block.Emitter.Resolver.ResolveNode(binaryOpExpr) is OperatorResolveResult binaryOpRr && isType(binaryOpRr.Operands[idx].Type, block.Emitter.Resolver) && !isType(rr.Type, block.Emitter.Resolver))
                {
                    var isNullable = NullableType.IsNullable(binaryOpRr.Operands[idx].Type);
                    if (expression.IsNull)
                    {
                        return false;
                    }

                    block.Write(typeName);
                    if (isNullable && ShouldBeLifted(expression))
                    {
                        block.Write("." + JS.Funcs.Math.LIFT);
                    }
                    if (!ignoreConversionResolveResult && expression is CastExpression castExp6 && castExp6.Expression is ParenthesizedExpression)
                    {
                        return false;
                    }
                    block.WriteOpenParentheses();
                    return true;
                }
            }

            if (expression.Parent is ConditionalExpression conditionalExpr && conditionalExpr.Condition != expression)
            {
                var idx = conditionalExpr.TrueExpression == expression ? 1 : 2;

                if (block.Emitter.Resolver.ResolveNode(conditionalExpr) is OperatorResolveResult conditionalrr && isType(conditionalrr.Operands[idx].Type, block.Emitter.Resolver) && !isType(rr.Type, block.Emitter.Resolver))
                {
                    if (expression.IsNull)
                    {
                        return false;
                    }

                    block.Write(typeName);
                    if (NullableType.IsNullable(conditionalrr.Operands[idx].Type) && ShouldBeLifted(expression))
                    {
                        block.Write("." + JS.Funcs.Math.LIFT);
                    }
                    if (!ignoreConversionResolveResult && expression is CastExpression castExp7 && castExp7.Expression is ParenthesizedExpression)
                    {
                        return false;
                    }
                    block.WriteOpenParentheses();
                    return true;
                }
            }

            if (expression.Parent is AssignmentExpression assignmentExpr)
            {
                var assigmentRr = block.Emitter.Resolver.ResolveNode(assignmentExpr) as OperatorResolveResult;

                if (isType(assigmentRr.Operands[1].Type, block.Emitter.Resolver) && !isType(rr.Type, block.Emitter.Resolver))
                {
                    if (expression.IsNull)
                    {
                        return false;
                    }

                    block.Write(typeName);
                    if (NullableType.IsNullable(assigmentRr.Operands[1].Type) && ShouldBeLifted(expression))
                    {
                        block.Write("." + JS.Funcs.Math.LIFT);
                    }
                    if (!ignoreConversionResolveResult && expression is CastExpression castExp8 && castExp8.Expression is ParenthesizedExpression)
                    {
                        return false;
                    }
                    block.WriteOpenParentheses();
                    return true;
                }
            }

            if (expression.Parent is IndexerExpression indexerExpr)
            {
                var index = indexerExpr.Arguments.ToList().IndexOf(expression);

                if (index >= 0)
                {
                    if (block.Emitter.Resolver.ResolveNode(indexerExpr) is InvocationResolveResult invocationrr)
                    {
                        var parameters = invocationrr.Member.Parameters;
                        if (parameters.Count <= index)
                        {
                            index = parameters.Count - 1;
                        }

                        if (isType(invocationrr.Member.Parameters.ElementAt(index).Type, block.Emitter.Resolver) && !isType(rr.Type, block.Emitter.Resolver))
                        {
                            if (expression.IsNull)
                            {
                                return false;
                            }

                            block.Write(typeName);
                            if (NullableType.IsNullable(invocationrr.Member.Parameters.ElementAt(index).Type) && ShouldBeLifted(expression))
                            {
                                block.Write("." + JS.Funcs.Math.LIFT);
                            }
                            if (!ignoreConversionResolveResult && expression is CastExpression castExp9 && castExp9.Expression is ParenthesizedExpression)
                            {
                                return false;
                            }
                            block.WriteOpenParentheses();
                            return true;
                        }
                    }
                }
            }

            if (expression.Parent is ArrayInitializerExpression arrayInit)
            {
                while (arrayInit.Parent is ArrayInitializerExpression arrayInitExp)
                {
                    arrayInit = arrayInitExp;
                }

                IType elementType = null;
                if (arrayInit.Parent is ArrayCreateExpression arrayCreate)
                {
                    var rrArrayType = block.Emitter.Resolver.ResolveNode(arrayCreate);
                    elementType = rrArrayType.Type is TypeWithElementType typeWIthElement ? typeWIthElement.ElementType : rrArrayType.Type;
                }
                else
                {
                    var rrElemenet = block.Emitter.Resolver.ResolveNode(arrayInit.Parent);
                    if (rrElemenet.Type is ParameterizedType pt && pt.TypeArguments.Count > 0)
                    {
                        if (pt.TypeArguments.Count == 1)
                        {
                            elementType = pt.TypeArguments.First();
                        }
                        else
                        {
                            var index = 0;
                            arrayInit = expression.Parent as ArrayInitializerExpression;

                            for (int i = 0; i < arrayInit.Elements.Count; i++)
                            {
                                if (expression == arrayInit.Elements.ElementAt(i))
                                {
                                    index = i;
                                    break;
                                }
                            }

                            elementType = index < pt.TypeArguments.Count ? pt.TypeArguments.ElementAt(index) : pt.TypeArguments.ElementAt(0);
                        }
                    }
                    else
                    {
                        if (rrElemenet.Type is TypeWithElementType arrayType)
                        {
                            elementType = arrayType.ElementType;
                        }
                        else
                        {
                            elementType = rrElemenet.Type;
                        }
                    }
                }

                if (elementType != null && isType(elementType, block.Emitter.Resolver) && !isType(rr.Type, block.Emitter.Resolver))
                {
                    if (expression.IsNull)
                    {
                        return false;
                    }

                    block.Write(typeName);
                    if (NullableType.IsNullable(elementType) && ShouldBeLifted(expression))
                    {
                        block.Write("." + JS.Funcs.Math.LIFT);
                    }
                    if (!ignoreConversionResolveResult && expression is CastExpression castExp2 && castExp2.Expression is ParenthesizedExpression)
                    {
                        return false;
                    }
                    block.WriteOpenParentheses();
                    return true;
                }
                else if (Helpers.Is64Type(rr.Type, block.Emitter.Resolver)
                         && Helpers.IsFloatType(elementType, block.Emitter.Resolver)
                         && !Helpers.IsDecimalType(elementType, block.Emitter.Resolver)
                         && isType(rr.Type, block.Emitter.Resolver))
                {
                    block.Write(JS.Types.System.Int64.TONUMBER);
                    if (!ignoreConversionResolveResult && expression is CastExpression castExp3 && castExp3.Expression is ParenthesizedExpression)
                    {
                        return false;
                    }
                    block.WriteOpenParentheses();
                    return true;
                }
            }

            if (isType(expectedType, block.Emitter.Resolver) && !isType(rr.Type, block.Emitter.Resolver) && !(conversion.IsExplicit && conversion.IsNumericConversion))
            {
                ResolveResult castTypeRr = null;
                if (expression.Parent is CastExpression castExpr)
                {
                    castTypeRr = block.Emitter.Resolver.ResolveNode(castExpr.Type);
                }

                /*if (castTypeRr == null || !isType(castTypeRr.Type, block.Emitter.Resolver))*/
                if (castTypeRr == null || !conversion.IsExplicit)
                {
                    if (expression.IsNull)
                    {
                        return false;
                    }

                    block.Write(typeName);
                    if (NullableType.IsNullable(expectedType) && ShouldBeLifted(expression))
                    {
                        block.Write("." + JS.Funcs.Math.LIFT);
                    }

                    if (!ignoreConversionResolveResult && expression is CastExpression castExp2 && castExp2.Expression is ParenthesizedExpression)
                    {
                        return false;
                    }
                    block.WriteOpenParentheses();
                    return true;
                }
            }

            return false;
        }

        private static bool IsTypeConversion(ConversionBlock block, Expression expression, ResolveResult rr, IType expectedType, Conversion conversion, string typeName, IsType isType)
        {
            if (conversion.IsUserDefined)
            {
                var m = conversion.Method;
                if (isType(m.ReturnType, block.Emitter.Resolver))
                {
                    return false;
                }
            }

            if (expression.Parent is InvocationExpression invocationExpression && invocationExpression.Arguments.Any(a => a == expression))
            {
                var index = invocationExpression.Arguments.ToList().IndexOf(expression);

                if (block.Emitter.Resolver.ResolveNode(invocationExpression) is MemberResolveResult methodResolveResult)
                {
                    var m = methodResolveResult.Member as IMethod;
                    var arg = m.Parameters[index < m.Parameters.Count ? index : (m.Parameters.Count - 1)];

                    if (isType(arg.Type, block.Emitter.Resolver, arg.IsParams) && !isType(rr.Type, block.Emitter.Resolver))
                    {
                        if (expression.IsNull)
                        {
                            return false;
                        }

                        if (expression is CastExpression castExp && castExp.Expression is ParenthesizedExpression)
                        {
                            return false;
                        }
                        return true;
                    }
                }
            }

            if (expression.Parent is ObjectCreateExpression objectCreateExpression && objectCreateExpression.Arguments.Any(a => a == expression))
            {
                var index = objectCreateExpression.Arguments.ToList().IndexOf(expression);

                if (block.Emitter.Resolver.ResolveNode(objectCreateExpression) is MemberResolveResult methodResolveResult)
                {
                    var m = methodResolveResult.Member as IMethod;
                    var arg = m.Parameters[index < m.Parameters.Count ? index : (m.Parameters.Count - 1)];

                    if (isType(arg.Type, block.Emitter.Resolver, arg.IsParams) && !isType(rr.Type, block.Emitter.Resolver))
                    {
                        if (expression.IsNull)
                        {
                            return false;
                        }

                        if (expression is CastExpression castExp2 && castExp2.Expression is ParenthesizedExpression)
                        {
                            return false;
                        }
                        return true;
                    }
                }
            }

            if (expression.Parent is NamedArgumentExpression namedArgExpression)
            {
                var namedArgResolveResult = block.Emitter.Resolver.ResolveNode(namedArgExpression) as NamedArgumentResolveResult;

                if (isType(namedArgResolveResult.Type, block.Emitter.Resolver) && !isType(rr.Type, block.Emitter.Resolver))
                {
                    if (expression.IsNull)
                    {
                        return false;
                    }

                    if (expression is CastExpression castExp3 && castExp3.Expression is ParenthesizedExpression)
                    {
                        return false;
                    }

                    return true;
                }
            }

            if (expression.Parent is NamedExpression namedExpression)
            {
                var namedResolveResult = block.Emitter.Resolver.ResolveNode(namedExpression);

                if (isType(namedResolveResult.Type, block.Emitter.Resolver) && !isType(rr.Type, block.Emitter.Resolver))
                {
                    if (expression.IsNull)
                    {
                        return false;
                    }

                    if (expression is CastExpression castExp4 && castExp4.Expression is ParenthesizedExpression)
                    {
                        return false;
                    }
                    return true;
                }
            }

            if (expression.Parent is BinaryOperatorExpression binaryOpExpr)
            {
                var idx = binaryOpExpr.Left == expression ? 0 : 1;

                if (block.Emitter.Resolver.ResolveNode(binaryOpExpr) is OperatorResolveResult binaryOpRr && isType(binaryOpRr.Operands[idx].Type, block.Emitter.Resolver) && !isType(rr.Type, block.Emitter.Resolver))
                {
                    if (expression.IsNull)
                    {
                        return false;
                    }

                    if (expression is CastExpression castExp5 && castExp5.Expression is ParenthesizedExpression)
                    {
                        return false;
                    }

                    return true;
                }
            }

            if (expression.Parent is ConditionalExpression conditionalExpr && conditionalExpr.Condition != expression)
            {
                var idx = conditionalExpr.TrueExpression == expression ? 0 : 1;

                if (block.Emitter.Resolver.ResolveNode(conditionalExpr) is OperatorResolveResult conditionalrr && isType(conditionalrr.Operands[idx].Type, block.Emitter.Resolver) && !isType(rr.Type, block.Emitter.Resolver))
                {
                    if (expression.IsNull)
                    {
                        return false;
                    }

                    if (expression is CastExpression castExp6 && castExp6.Expression is ParenthesizedExpression)
                    {
                        return false;
                    }

                    return true;
                }
            }

            if (expression.Parent is AssignmentExpression assignmentExpr)
            {
                var assigmentRr = block.Emitter.Resolver.ResolveNode(assignmentExpr) as OperatorResolveResult;

                if (isType(assigmentRr.Operands[1].Type, block.Emitter.Resolver) && !isType(rr.Type, block.Emitter.Resolver))
                {
                    if (expression.IsNull)
                    {
                        return false;
                    }

                    if (expression is CastExpression castExp7 && castExp7.Expression is ParenthesizedExpression)
                    {
                        return false;
                    }

                    return true;
                }
            }

            if (expression.Parent is ArrayInitializerExpression arrayInit)
            {
                while (arrayInit.Parent is ArrayInitializerExpression arrayInitExp)
                {
                    arrayInit = arrayInitExp;
                }

                IType elementType = null;
                if (arrayInit.Parent is ArrayCreateExpression arrayCreate)
                {
                    var rrArrayType = block.Emitter.Resolver.ResolveNode(arrayCreate);
                    if (rrArrayType.Type is TypeWithElementType typeWithElementType)
                    {
                        elementType = typeWithElementType.ElementType;
                    }
                    else
                    {
                        elementType = rrArrayType.Type;
                    }
                }
                else
                {
                    var rrElemenet = block.Emitter.Resolver.ResolveNode(arrayInit.Parent);
                    if (rrElemenet.Type is ParameterizedType pt)
                    {
                        elementType = pt.TypeArguments.Count > 0 ? pt.TypeArguments.First() : null;
                    }
                    else
                    {
                        if (rrElemenet.Type is TypeWithElementType arrayType)
                        {
                            elementType = arrayType.ElementType;
                        }
                        else
                        {
                            elementType = rrElemenet.Type;
                        }
                    }
                }

                if (elementType != null && isType(elementType, block.Emitter.Resolver) && !isType(rr.Type, block.Emitter.Resolver))
                {
                    if (expression.IsNull)
                    {
                        return false;
                    }

                    if (expression is CastExpression castExp8 && castExp8.Expression is ParenthesizedExpression)
                    {
                        return false;
                    }

                    return true;
                }
                else if (Helpers.Is64Type(rr.Type, block.Emitter.Resolver)
                         && Helpers.IsFloatType(elementType, block.Emitter.Resolver)
                         && !Helpers.IsDecimalType(elementType, block.Emitter.Resolver)
                         && isType(rr.Type, block.Emitter.Resolver))
                {
                    if (expression is CastExpression castExp9 && castExp9.Expression is ParenthesizedExpression)
                    {
                        return false;
                    }
                    return true;
                }
            }

            if (isType(expectedType, block.Emitter.Resolver) && !isType(rr.Type, block.Emitter.Resolver) && !(conversion.IsExplicit && conversion.IsNumericConversion))
            {
                ResolveResult castTypeRr = null;
                if (expression.Parent is CastExpression castExpr)
                {
                    castTypeRr = block.Emitter.Resolver.ResolveNode(castExpr.Type);
                }

                /*if (castTypeRr == null || !isType(castTypeRr.Type, block.Emitter.Resolver))*/
                if (castTypeRr == null || !conversion.IsExplicit)
                {
                    if (expression.IsNull)
                    {
                        return false;
                    }

                    if (expression is CastExpression castExp10 && castExp10.Expression is ParenthesizedExpression)
                    {
                        return false;
                    }

                    return true;
                }
            }

            return false;
        }

        private static bool CheckDecimalConversion(ConversionBlock block, Expression expression, ResolveResult rr, IType expectedType, Conversion conversion, bool ignoreConversionResolveResult)
        {
            return CheckTypeConversion(block, expression, rr, expectedType, conversion, JS.Types.SYSTEM_DECIMAL, Helpers.IsDecimalType, ignoreConversionResolveResult);
        }

        private static bool CheckLongConversion(ConversionBlock block, Expression expression, ResolveResult rr, IType expectedType, Conversion conversion, bool ignoreConversionResolveResult)
        {
            return CheckTypeConversion(block, expression, rr, expectedType, conversion, JS.Types.System.Int64.NAME, Helpers.IsLongType, ignoreConversionResolveResult) ||
                   CheckTypeConversion(block, expression, rr, expectedType, conversion, JS.Types.SYSTEM_UInt64, Helpers.IsULongType, ignoreConversionResolveResult);
        }

        private static bool IsLongConversion(ConversionBlock block, Expression expression, ResolveResult rr, IType expectedType, Conversion conversion)
        {
            return IsTypeConversion(block, expression, rr, expectedType, conversion, JS.Types.System.Int64.NAME, Helpers.IsLongType) ||
                   IsTypeConversion(block, expression, rr, expectedType, conversion, JS.Types.SYSTEM_UInt64, Helpers.IsULongType);
        }

        public static bool ShouldBeLifted(Expression expr)
        {
            return !(expr is PrimitiveExpression || expr.IsNull);
        }

        protected abstract void EmitConversionExpression();

        protected abstract Expression GetExpression();
    }
}