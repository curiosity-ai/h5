using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using System;
using System.Collections.Generic;
using System.Linq;

namespace H5.Translator
{
    public class CastBlock : ConversionBlock
    {
        public CastBlock(IEmitter emitter, CastExpression castExpression)
            : base(emitter, castExpression)
        {
            Emitter = emitter;
            CastExpression = castExpression;
        }

        public CastBlock(IEmitter emitter, AsExpression asExpression)
            : base(emitter, asExpression)
        {
            Emitter = emitter;
            AsExpression = asExpression;
        }

        public CastBlock(IEmitter emitter, IsExpression isExpression)
            : base(emitter, isExpression)
        {
            Emitter = emitter;
            IsExpression = isExpression;
        }

        public CastBlock(IEmitter emitter, IType iType)
            : base(emitter, null)
        {
            Emitter = emitter;
            IType = iType;
        }

        public CastBlock(IEmitter emitter, AstType astType)
            : base(emitter, astType)
        {
            Emitter = emitter;
            AstType = astType;
        }

        public CastExpression CastExpression { get; set; }

        public AsExpression AsExpression { get; set; }

        public IsExpression IsExpression { get; set; }

        public IType IType { get; set; }

        public AstType AstType { get; set; }

        protected override Expression GetExpression()
        {
            if (CastExpression != null)
            {
                return CastExpression;
            }
            else if (AsExpression != null)
            {
                return AsExpression;
            }
            else if (IsExpression != null)
            {
                return IsExpression;
            }

            return null;
        }

        protected override void EmitConversionExpression()
        {
            if (CastExpression != null)
            {
                EmitCastExpression(CastExpression.Expression, CastExpression.Type, CS.Ops.CAST);
            }
            else if (AsExpression != null)
            {
                EmitCastExpression(AsExpression.Expression, AsExpression.Type, CS.Ops.AS);
            }
            else if (IsExpression != null)
            {
                EmitCastExpression(IsExpression.Expression, IsExpression.Type, CS.Ops.IS);
            }
            else if (IType != null)
            {
                EmitCastType(IType);
            }
            else if (AstType != null)
            {
                EmitCastType(AstType);
            }
        }

        protected virtual void EmitCastExpression(Expression expression, AstType type, string method)
        {
            var itype = Emitter.H5Types.ToType(type);
            bool isCastAttr;
            string castCode = GetCastCode(expression, type, method, out isCastAttr);

            var enumType = itype;
            if (NullableType.IsNullable(enumType))
            {
                enumType = NullableType.GetUnderlyingType(enumType);
            }

            var castToEnum = enumType.Kind == TypeKind.Enum;

            if (castToEnum)
            {
                itype = enumType.GetDefinition().EnumUnderlyingType;
                var enumMode = Helpers.EnumEmitMode(enumType);
                if (enumMode >= 3 && enumMode < 7)
                {
                    itype = Emitter.Resolver.Compilation.FindType(KnownTypeCode.String);
                }
            }

            if (expression is NullReferenceExpression || (method != CS.Ops.IS && Helpers.IsIgnoreCast(type, Emitter)) || IsExternalCast(itype))
            {
                if (expression is ParenthesizedExpression)
                {
                    expression = ((ParenthesizedExpression) expression).Expression;
                }

                expression.AcceptVisitor(Emitter);
                return;
            }

            var expressionrr = Emitter.Resolver.ResolveNode(expression);
            var typerr = Emitter.Resolver.ResolveNode(type);

            if (expressionrr.Type.Kind == TypeKind.Enum)
            {
                var enumMode = Helpers.EnumEmitMode(expressionrr.Type);
                if (enumMode >= 3 && enumMode < 7 && Helpers.IsIntegerType(itype, Emitter.Resolver))
                {
                   throw new EmitterException(CastExpression, "Enum underlying type is string and cannot be casted to number");
                }
            }

            if (method == CS.Ops.CAST && expressionrr.Type.Kind != TypeKind.Enum)
            {
                var cast_rr = Emitter.Resolver.ResolveNode(CastExpression);

                if (cast_rr is ConstantResolveResult)
                {
                    var expectedType = Emitter.Resolver.Resolver.GetExpectedType(CastExpression);
                    var value = ((ConstantResolveResult)cast_rr).ConstantValue;

                    WriteCastValue(value, expectedType);
                    return;
                }
                else
                {
                    if (cast_rr is ConversionResolveResult conv_rr && conv_rr.Input is ConstantResolveResult && !conv_rr.Conversion.IsUserDefined)
                    {
                        var expectedType = Emitter.Resolver.Resolver.GetExpectedType(CastExpression);
                        var value = ((ConstantResolveResult)conv_rr.Input).ConstantValue;
                        WriteCastValue(value, expectedType);
                        return;
                    }
                }
            }

            if (method == CS.Ops.IS && castToEnum)
            {
                Write(JS.Types.H5.IS);
                WriteOpenParentheses();
                expression.AcceptVisitor(Emitter);
                Write(", ");
                Write(H5Types.ToJsName(itype, Emitter));
                Write(")");
                return;
            }

            if (expressionrr.Type.Equals(itype))
            {
                if (method == CS.Ops.IS)
                {
                    Write(JS.Funcs.H5_HASVALUE);
                    WriteOpenParentheses();
                }
                expression.AcceptVisitor(Emitter);
                if (method == CS.Ops.IS)
                {
                    Write(")");
                }

                return;
            }

            bool isResultNullable = NullableType.IsNullable(typerr.Type);

            if (castCode != null)
            {
                EmitInlineCast(expressionrr, expression, type, castCode, isCastAttr, method);
                return;
            }

            bool isCast = method == CS.Ops.CAST;
            if (isCast)
            {
                if (IsUserDefinedConversion(this, CastExpression.Expression) || IsUserDefinedConversion(this, CastExpression))
                {
                    expression.AcceptVisitor(Emitter);

                    return;
                }
            }

            var conversion = Emitter.Resolver.Resolver.GetConversion(expression);

            if (conversion.IsNumericConversion || conversion.IsEnumerationConversion || (isCast && conversion.IsIdentityConversion))
            {
                expression.AcceptVisitor(Emitter);
                return;
            }

            bool hasValue = false;

            if (type is SimpleType simpleType && simpleType.Identifier == "dynamic")
            {
                if (method == CS.Ops.CAST || method == CS.Ops.AS)
                {
                    expression.AcceptVisitor(Emitter);
                    return;
                }
                else if (method == CS.Ops.IS)
                {
                    hasValue = true;
                    method = "hasValue";
                }
            }

            bool unbox = Emitter.Rules.Boxing == BoxingRule.Managed && !(itype.IsReferenceType.HasValue ? itype.IsReferenceType.Value : true) && !NullableType.IsNullable(itype) && isCast && conversion.IsUnboxingConversion;
            if (unbox)
            {
                Write("System.Nullable.getValue(");
            }

            var typeDef = itype.Kind == TypeKind.TypeParameter ? null : Emitter.GetTypeDefinition(type, true);
            if (typeDef != null && method == CS.Ops.IS && itype.Kind != TypeKind.Interface && Emitter.Validator.IsObjectLiteral(typeDef) && Emitter.Validator.GetObjectCreateMode(typeDef) == 0)
            {
                throw new EmitterException(type, $"ObjectLiteral type ({itype.FullName}) with Plain mode cannot be used in 'is' operator. Please define cast logic in Cast attribute or use Constructor mode.");
            }

            Write(JS.NS.H5);
            WriteDot();
            Write(method);
            WriteOpenParentheses();

            expression.AcceptVisitor(Emitter);

            if (!hasValue)
            {
                WriteComma();
                EmitCastType(itype);
            }

            if (isResultNullable && method != CS.Ops.IS)
            {
                WriteComma();
                WriteScript(true);
            }

            WriteCloseParentheses();

            if (unbox)
            {
                Write(")");
            }
        }

        private bool IsExternalCast(IType type)
        {
            if (Emitter.Rules.ExternalCast == ExternalCastRule.Plain)
            {
                var typeDef = type.GetDefinition();
                if (typeDef == null || !Emitter.Validator.IsExternalType(typeDef))
                {
                    return false;
                }

                var fullName = type.FullName;

                if (Validator.IsTypeFromH5ButNotFromH5Core(fullName) || fullName.StartsWith("System."))
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        private void WriteCastValue(object value, IType expectedType)
        {
            string typeName = null;

            if (value == null || value.GetType().FullName != expectedType.ReflectionName)
            {
                if (Helpers.IsDecimalType(expectedType, Emitter.Resolver))
                {
                    typeName = JS.Types.SYSTEM_DECIMAL;
                }
                else if (Helpers.IsLongType(expectedType, Emitter.Resolver))
                {
                    typeName = JS.Types.System.Int64.NAME;
                }
                else if (Helpers.IsULongType(expectedType, Emitter.Resolver))
                {
                    typeName = JS.Types.SYSTEM_UInt64;
                }
            }

            if (typeName != null)
            {
                Write(typeName);
                WriteOpenParentheses();
                WriteScript(value);
                WriteCloseParentheses();
            }
            if (value is float && expectedType.IsKnownType(KnownTypeCode.Double))
            {
                WriteScript((double)(float)value);
            }
            else
            {
                WriteScript(value);
            }
        }

        protected virtual void EmitCastType(AstType astType)
        {
            var resolveResult = Emitter.Resolver.ResolveNode(astType);

            if (NullableType.IsNullable(resolveResult.Type))
            {
                Write(H5Types.ToJsName(NullableType.GetUnderlyingType(resolveResult.Type), Emitter));
            }
            else if (resolveResult.Type.Kind == TypeKind.Delegate)
            {
                Write(JS.Types.FUNCTION);
            }
            /*else if (resolveResult.Type.Kind == TypeKind.Array)
            {
                this.EmitArray(resolveResult.Type);
            }*/
            else
            {
                astType.AcceptVisitor(Emitter);
            }
        }

        protected virtual void EmitCastType(IType iType)
        {
            if (NullableType.IsNullable(iType))
            {
                Write(H5Types.ToJsName(NullableType.GetUnderlyingType(iType), Emitter));
            }
            else if (iType.Kind == TypeKind.Delegate)
            {
                Write(JS.Types.FUNCTION);
            }
            /*else if (iType.Kind == TypeKind.Array)
            {
                this.EmitArray(iType);
            }*/
            else if (iType.Kind == TypeKind.Anonymous)
            {
                Write(JS.Types.System.Object.NAME);
            }
            else
            {
                Write(H5Types.ToJsName(iType, Emitter));
            }
        }

        protected virtual string GetCastCode(Expression expression, AstType astType, string op, out bool isCastAttr)
        {
            isCastAttr = false;

            if (!(Emitter.Resolver.ResolveNode(astType) is TypeResolveResult resolveResult))
            {
                return null;
            }

            var exprResolveResult = Emitter.Resolver.ResolveNode(expression);
            string inline = null;
            bool isOp = op == CS.Ops.IS;

            var method = isOp ? null : GetCastMethod(exprResolveResult.Type, resolveResult.Type, out inline);

            if (method == null && !isOp && (NullableType.IsNullable(exprResolveResult.Type) || NullableType.IsNullable(resolveResult.Type)))
            {
                method = GetCastMethod(NullableType.IsNullable(exprResolveResult.Type) ? NullableType.GetUnderlyingType(exprResolveResult.Type) : exprResolveResult.Type,
                                            NullableType.IsNullable(resolveResult.Type) ? NullableType.GetUnderlyingType(resolveResult.Type) : resolveResult.Type, out inline);
            }

            if (inline != null)
            {
                InlineMethod = method;
                return inline;
            }

            IEnumerable<IAttribute> attributes = null;
            var type = resolveResult.Type.GetDefinition();

            if (type != null)
            {
                attributes = type.Attributes;
            }
            else
            {
                if (resolveResult.Type is ParameterizedType paramType)
                {
                    attributes = paramType.GetDefinition().Attributes;
                }
            }

            if (attributes != null)
            {
                var attribute = Emitter.GetAttribute(attributes, Translator.H5_ASSEMBLY + ".CastAttribute");

                if (attribute != null)
                {
                    isCastAttr = true;
                    return attribute.PositionalArguments[0].ConstantValue.ToString();
                }
            }

            return null;
        }

        private void EmitArray(IType iType)
        {
            string typedArrayName = null;
            if (Emitter.AssemblyInfo.UseTypedArrays && (typedArrayName = Helpers.GetTypedArrayName(iType)) != null)
            {
                Write(typedArrayName);
            }
            else
            {
                Write(JS.Types.ARRAY);
            }
        }


        protected virtual void EmitInlineCast(ResolveResult expressionrr, Expression expression, AstType astType, string castCode, bool isCastAttr, string method)
        {
            Write("");
            string name;

            if (InlineMethod == null)
            {
                name = "{this}";
            }
            else
            {
                name = "{" + InlineMethod.Parameters[0].Name + "}";
                if (!castCode.Contains(name))
                {
                    name = "{this}";
                }
            }

            string tempVar = null;
            string expressionStr;
            bool isField = expressionrr is MemberResolveResult memberTargetrr && memberTargetrr.Member is IField &&
                           (memberTargetrr.TargetResult is ThisResolveResult ||
                            memberTargetrr.TargetResult is LocalResolveResult);

            var oldBuilder = SaveWriter();
            var sb = NewWriter();

            expression.AcceptVisitor(Emitter);

            expressionStr = sb.ToString();
            RestoreWriter(oldBuilder);

            if (!(expressionrr is ThisResolveResult || expressionrr is ConstantResolveResult || expressionrr is LocalResolveResult || isField) && isCastAttr)
            {
                tempVar = GetTempVarName();
            }

            if (castCode.Contains(name))
            {
                castCode = castCode.Replace(name, tempVar ?? expressionStr);
            }

            if (castCode.Contains("{0}"))
            {
                oldBuilder = SaveWriter();
                sb = NewWriter();
                EmitCastType(astType);
                castCode = castCode.Replace("{0}", sb.ToString());
                RestoreWriter(oldBuilder);
            }

            if (isCastAttr)
            {
                if (tempVar != null)
                {
                    castCode = string.Format("({0} = {1}, H5.{2}({0}, {4}({0}) && ({3})))", tempVar, expressionStr, method, castCode, JS.Funcs.H5_HASVALUE);
                    RemoveTempVar(tempVar);
                }
                else
                {
                    castCode = string.Format("H5.{1}({0}, {3}({0}) && ({2}))", expressionStr, method, castCode, JS.Funcs.H5_HASVALUE);
                }
            }

            Write(castCode);
        }

        public IMethod GetCastMethod(IType fromType, IType toType, out string template)
        {
            string inline = null;
            var method = fromType.GetMethods().FirstOrDefault(m =>
            {
                if (m.IsOperator && (m.Name == "op_Explicit" || m.Name == "op_Implicit") &&
                    m.Parameters.Count == 1 &&
                    m.ReturnType.ReflectionName == toType.ReflectionName &&
                    m.Parameters[0].Type.ReflectionName == fromType.ReflectionName
                    )
                {
                    string tmpInline = Emitter.GetInline(m);

                    if (!string.IsNullOrWhiteSpace(tmpInline))
                    {
                        inline = tmpInline;
                        return true;
                    }
                }

                return false;
            });

            if (method == null)
            {
                method = toType.GetMethods().FirstOrDefault(m =>
                {
                    if (m.IsOperator && (m.Name == "op_Explicit" || m.Name == "op_Implicit") &&
                        m.Parameters.Count == 1 &&
                        m.ReturnType.ReflectionName == toType.ReflectionName &&
                        (m.Parameters[0].Type.ReflectionName == fromType.ReflectionName)
                        )
                    {
                        string tmpInline = Emitter.GetInline(m);

                        if (!string.IsNullOrWhiteSpace(tmpInline))
                        {
                            inline = tmpInline;
                            return true;
                        }
                    }

                    return false;
                });
            }

            if (method == null && CastExpression != null)
            {
                var conversion = Emitter.Resolver.Resolver.GetConversion(CastExpression);

                if (conversion.IsUserDefined)
                {
                    method = conversion.Method;

                    string tmpInline = Emitter.GetInline(method);

                    if (!string.IsNullOrWhiteSpace(tmpInline))
                    {
                        inline = tmpInline;
                    }
                }
            }

            template = inline;
            return method;
        }

        public IMethod InlineMethod { get; set; }
    }
}