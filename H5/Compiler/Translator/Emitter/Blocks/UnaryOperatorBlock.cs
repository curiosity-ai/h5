using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using System;
using System.Linq;

namespace H5.Translator
{
    public class UnaryOperatorBlock : ConversionBlock
    {
        public UnaryOperatorBlock(IEmitter emitter, UnaryOperatorExpression unaryOperatorExpression)
            : base(emitter, unaryOperatorExpression)
        {
            Emitter = emitter;
            UnaryOperatorExpression = unaryOperatorExpression;
        }

        public UnaryOperatorExpression UnaryOperatorExpression { get; set; }

        protected override Expression GetExpression()
        {
            return UnaryOperatorExpression;
        }

        protected override void EmitConversionExpression()
        {
            VisitUnaryOperatorExpression();
        }

        protected bool ResolveOperator(UnaryOperatorExpression unaryOperatorExpression, OperatorResolveResult orr)
        {
            if (orr != null && orr.UserDefinedOperatorMethod != null)
            {
                var method = orr.UserDefinedOperatorMethod;
                var inline = Emitter.GetInline(method);

                if (!string.IsNullOrWhiteSpace(inline))
                {
                    new InlineArgumentsBlock(Emitter, new ArgumentsInfo(Emitter, unaryOperatorExpression, orr, method), inline).Emit();
                    return true;
                }
                else
                {
                    if (orr.IsLiftedOperator)
                    {
                        Write(JS.Types.SYSTEM_NULLABLE + "." + JS.Funcs.Math.LIFT + "(");
                    }

                    Write(H5Types.ToJsName(method.DeclaringType, Emitter));
                    WriteDot();

                    Write(OverloadsCollection.Create(Emitter, method).GetOverloadName());

                    if (orr.IsLiftedOperator)
                    {
                        WriteComma();
                    }
                    else
                    {
                        WriteOpenParentheses();
                    }

                    new ExpressionListBlock(Emitter, new Expression[] { unaryOperatorExpression.Expression }, null, null, 0).Emit();
                    WriteCloseParentheses();

                    return true;
                }
            }

            return false;
        }

        protected void VisitUnaryOperatorExpression()
        {
            var unaryOperatorExpression = UnaryOperatorExpression;
            var oldType = Emitter.UnaryOperatorType;
            var oldAccessor = Emitter.IsUnaryAccessor;
            var resolveOperator = Emitter.Resolver.ResolveNode(unaryOperatorExpression);
            var expectedType = Emitter.Resolver.Resolver.GetExpectedType(unaryOperatorExpression);
            bool isDecimalExpected = Helpers.IsDecimalType(expectedType, Emitter.Resolver);
            bool isDecimal = Helpers.IsDecimalType(resolveOperator.Type, Emitter.Resolver);
            bool isLongExpected = Helpers.Is64Type(expectedType, Emitter.Resolver);
            bool isLong = Helpers.Is64Type(resolveOperator.Type, Emitter.Resolver);
            OperatorResolveResult orr = resolveOperator as OperatorResolveResult;
            int count = Emitter.Writers.Count;

            if (resolveOperator is ConstantResolveResult crr)
            {
                object constantValue = crr.ConstantValue;

                if (unaryOperatorExpression.Operator == UnaryOperatorType.Minus && SyntaxHelper.IsNumeric(constantValue.GetType()) && Convert.ToDouble(constantValue) == 0)
                {
                    Write("-");
                    constantValue = 0; //Fixes https://github.com/theolivenbaum/h5/issues/68
                }

                WriteScript(constantValue);
                return;
            }

            if (Helpers.IsDecimalType(resolveOperator.Type, Emitter.Resolver))
            {
                isDecimal = true;
                isDecimalExpected = true;
            }

            if (isDecimal && isDecimalExpected && unaryOperatorExpression.Operator != UnaryOperatorType.Await)
            {
                HandleDecimal(resolveOperator);
                return;
            }

            if (ResolveOperator(unaryOperatorExpression, orr))
            {
                return;
            }

            if (Helpers.Is64Type(resolveOperator.Type, Emitter.Resolver))
            {
                isLong = true;
                isLongExpected = true;
            }

            if (isLong && isLongExpected && unaryOperatorExpression.Operator != UnaryOperatorType.Await)
            {
                HandleDecimal(resolveOperator, true);
                return;
            }

            if (ResolveOperator(unaryOperatorExpression, orr))
            {
                return;
            }

            var op = unaryOperatorExpression.Operator;
            var argResolverResult = Emitter.Resolver.ResolveNode(unaryOperatorExpression.Expression);
            bool nullable = NullableType.IsNullable(argResolverResult.Type);

            if (nullable)
            {
                if (op != UnaryOperatorType.Increment &&
                    op != UnaryOperatorType.Decrement &&
                    op != UnaryOperatorType.PostIncrement &&
                    op != UnaryOperatorType.PostDecrement)
                {
                    Write(JS.Types.SYSTEM_NULLABLE + ".");
                }
            }

            bool isAccessor = false;


            if (argResolverResult is MemberResolveResult memberArgResolverResult)
            {
                if (memberArgResolverResult.Member is IProperty prop)
                {
                    var isIgnore = memberArgResolverResult.Member.DeclaringTypeDefinition != null && Emitter.Validator.IsExternalType(memberArgResolverResult.Member.DeclaringTypeDefinition);
                    var inlineAttr = prop.Getter != null ? Emitter.GetAttribute(prop.Getter.Attributes, Translator.H5_ASSEMBLY + ".TemplateAttribute") : null;
                    var ignoreAccessor = prop.Getter != null && Emitter.Validator.IsExternalType(prop.Getter);
                    var isAccessorsIndexer = Emitter.Validator.IsAccessorsIndexer(memberArgResolverResult.Member);

                    isAccessor = prop.IsIndexer;

                    if (inlineAttr == null && (isIgnore || ignoreAccessor) && !isAccessorsIndexer)
                    {
                        isAccessor = false;
                    }
                }
            }
            else if (argResolverResult is ArrayAccessResolveResult)
            {
                isAccessor = ((ArrayAccessResolveResult)argResolverResult).Indexes.Count > 1;
            }

            Emitter.UnaryOperatorType = op;

            if ((isAccessor) &&
                (op == UnaryOperatorType.Increment ||
                 op == UnaryOperatorType.Decrement ||
                 op == UnaryOperatorType.PostIncrement ||
                 op == UnaryOperatorType.PostDecrement))
            {
                Emitter.IsUnaryAccessor = true;

                if (nullable)
                {
                    Write(JS.Funcs.H5_HASVALUE);
                    WriteOpenParentheses();
                    Emitter.IsUnaryAccessor = false;
                    unaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                    Write(") ? ");
                    Emitter.IsUnaryAccessor = true;
                    unaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                    Write(" : null)");
                }
                else
                {
                    unaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                }

                Emitter.IsUnaryAccessor = oldAccessor;

                if (Emitter.Writers.Count > count)
                {
                    PopWriter();
                }
            }
            else
            {
                switch (op)
                {
                    case UnaryOperatorType.BitNot:
                        if (nullable)
                        {
                            Write("bnot(");
                            unaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                            Write(")");
                        }
                        else
                        {
                            Write("~");
                            unaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                        }
                        break;

                    case UnaryOperatorType.Decrement:
                        if (nullable)
                        {
                            Write(JS.Funcs.H5_HASVALUE);
                            WriteOpenParentheses();
                            unaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                            Write(") ? ");
                            Write("--");
                            unaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                            Write(" : null)");
                        }
                        else
                        {
                            Write("--");
                            unaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                        }
                        break;

                    case UnaryOperatorType.Increment:
                        if (nullable)
                        {
                            Write(JS.Funcs.H5_HASVALUE);
                            WriteOpenParentheses();
                            unaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                            Write(") ? ");
                            Write("++");
                            unaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                            Write(" : null)");
                        }
                        else
                        {
                            Write("++");
                            unaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                        }
                        break;

                    case UnaryOperatorType.Minus:
                        if (nullable)
                        {
                            Write("neg(");
                            unaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                            Write(")");
                        }
                        else
                        {
                            Write("-");
                            unaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                        }
                        break;

                    case UnaryOperatorType.Not:
                        if (nullable)
                        {
                            Write("not(");
                            unaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                            Write(")");
                        }
                        else
                        {
                            Write("!");
                            unaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                        }
                        break;

                    case UnaryOperatorType.Plus:
                        if (nullable)
                        {
                            Write("pos(");
                            unaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                            Write(")");
                        }
                        else
                        {
                            unaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                        }

                        break;

                    case UnaryOperatorType.PostDecrement:
                        if (nullable)
                        {
                            Write(JS.Funcs.H5_HASVALUE);
                            WriteOpenParentheses();
                            unaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                            Write(") ? ");
                            unaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                            Write("--");
                            Write(" : null)");
                        }
                        else
                        {
                            unaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                            Write("--");
                        }
                        break;

                    case UnaryOperatorType.PostIncrement:
                        if (nullable)
                        {
                            Write(JS.Funcs.H5_HASVALUE);
                            WriteOpenParentheses();
                            unaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                            Write(") ? ");
                            unaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                            Write("++");
                            Write(" : null)");
                        }
                        else
                        {
                            unaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                            Write("++");
                        }
                        break;

                    case UnaryOperatorType.Await:
                        if (Emitter.ReplaceAwaiterByVar)
                        {
                            var index = Array.IndexOf(Emitter.AsyncBlock.AwaitExpressions, unaryOperatorExpression.Expression) + 1;
                            Write(JS.Vars.ASYNC_TASK_RESULT + index);
                        }
                        else
                        {
                            var oldValue = Emitter.ReplaceAwaiterByVar;
                            var oldAsyncExpressionHandling = Emitter.AsyncExpressionHandling;

                            if (Emitter.IsAsync && !Emitter.AsyncExpressionHandling)
                            {
                                WriteAwaiters(unaryOperatorExpression.Expression);
                                Emitter.ReplaceAwaiterByVar = true;
                                Emitter.AsyncExpressionHandling = true;
                            }

                            WriteAwaiter(unaryOperatorExpression.Expression);

                            Emitter.ReplaceAwaiterByVar = oldValue;
                            Emitter.AsyncExpressionHandling = oldAsyncExpressionHandling;
                        }
                        break;

                    default:
                        throw new EmitterException(unaryOperatorExpression, "Unsupported unary operator: " + unaryOperatorExpression.Operator.ToString());
                }

                if (Emitter.Writers.Count > count)
                {
                    PopWriter();
                }
            }

            Emitter.UnaryOperatorType = oldType;
        }

        private void AddOveflowFlag(KnownTypeCode typeCode, string op_name, bool lifted)
        {
            if ((typeCode == KnownTypeCode.Int64 || typeCode == KnownTypeCode.UInt64) && IsInCheckedContext(Emitter, UnaryOperatorExpression))
            {
                if (op_name == JS.Funcs.Math.NEG || op_name == JS.Funcs.Math.DEC || op_name == JS.Funcs.Math.INC)
                {
                    if (lifted)
                    {
                        Write(", ");
                    }
                    Write("1");
                }
            }
        }

        private void HandleDecimal(ResolveResult resolveOperator, bool isLong = false)
        {
            var orr = resolveOperator as OperatorResolveResult;
            var op = UnaryOperatorExpression.Operator;
            var oldType = Emitter.UnaryOperatorType;
            var oldAccessor = Emitter.IsUnaryAccessor;
            var typeCode = isLong ? KnownTypeCode.Int64 : KnownTypeCode.Decimal;
            Emitter.UnaryOperatorType = op;

            var argResolverResult = Emitter.Resolver.ResolveNode(UnaryOperatorExpression.Expression);
            bool nullable = NullableType.IsNullable(argResolverResult.Type);
            bool isAccessor = false;

            if (argResolverResult is MemberResolveResult memberArgResolverResult && memberArgResolverResult.Member is IProperty)
            {
                var isIgnore = Emitter.Validator.IsExternalType(memberArgResolverResult.Member.DeclaringTypeDefinition);
                var inlineAttr = Emitter.GetAttribute(memberArgResolverResult.Member.Attributes, Translator.H5_ASSEMBLY + ".TemplateAttribute");
                var ignoreAccessor = Emitter.Validator.IsExternalType(((IProperty)memberArgResolverResult.Member).Getter);
                var isAccessorsIndexer = Emitter.Validator.IsAccessorsIndexer(memberArgResolverResult.Member);

                isAccessor = ((IProperty)memberArgResolverResult.Member).IsIndexer;

                if (inlineAttr == null && (isIgnore || ignoreAccessor) && !isAccessorsIndexer)
                {
                    isAccessor = false;
                }
            }
            else if (argResolverResult is ArrayAccessResolveResult)
            {
                isAccessor = ((ArrayAccessResolveResult)argResolverResult).Indexes.Count > 1;
            }

            var isOneOp = op == UnaryOperatorType.Increment ||
                           op == UnaryOperatorType.Decrement ||
                           op == UnaryOperatorType.PostIncrement ||
                           op == UnaryOperatorType.PostDecrement;

            if (isAccessor && isOneOp)
            {
                Emitter.IsUnaryAccessor = true;

                if (nullable)
                {
                    Write(JS.Funcs.H5_HASVALUE);
                    WriteOpenParentheses();
                    Emitter.IsUnaryAccessor = false;
                    UnaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                    Write(") ? ");
                    Emitter.IsUnaryAccessor = true;
                    UnaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                    Write(" : null)");
                }
                else
                {
                    UnaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                }

                Emitter.UnaryOperatorType = oldType;
                Emitter.IsUnaryAccessor = oldAccessor;

                return;
            }

            var method = orr?.UserDefinedOperatorMethod;

            if (orr != null && method == null)
            {
                var name = Helpers.GetUnaryOperatorMethodName(UnaryOperatorExpression.Operator);
                var type = NullableType.IsNullable(orr.Type) ? NullableType.GetUnderlyingType(orr.Type) : orr.Type;
                method = type.GetMethods(m => m.Name == name, GetMemberOptions.IgnoreInheritedMembers).FirstOrDefault();
            }

            if (orr != null && orr.IsLiftedOperator)
            {
                if (!isOneOp)
                {
                    Write(JS.Types.SYSTEM_NULLABLE + ".");
                }

                string action = JS.Funcs.Math.LIFT1;
                string op_name = null;

                switch (UnaryOperatorExpression.Operator)
                {
                    case UnaryOperatorType.Minus:
                        op_name = JS.Funcs.Math.NEG;
                        break;

                    case UnaryOperatorType.Plus:
                        op_name = "clone";
                        break;

                    case UnaryOperatorType.BitNot:
                        op_name = "not";
                        break;

                    case UnaryOperatorType.Increment:
                    case UnaryOperatorType.Decrement:
                        Write(JS.Funcs.H5_HASVALUE);
                        WriteOpenParentheses();
                        UnaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                        Write(") ? ");
                        WriteOpenParentheses();
                        UnaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                        Write(" = " + JS.Types.SYSTEM_NULLABLE + "." + JS.Funcs.Math.LIFT1 + "(\"" + (op == UnaryOperatorType.Decrement ? JS.Funcs.Math.DEC : JS.Funcs.Math.INC) + "\", ");
                        UnaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                        AddOveflowFlag(typeCode, JS.Funcs.Math.DEC, true);
                        Write(")");
                        WriteCloseParentheses();

                        Write(" : null");
                        break;

                    case UnaryOperatorType.PostIncrement:
                    case UnaryOperatorType.PostDecrement:
                        Write(JS.Funcs.H5_HASVALUE);
                        WriteOpenParentheses();
                        UnaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                        Write(") ? ");
                        WriteOpenParentheses();
                        var valueVar = GetTempVarName();

                        Write(valueVar);
                        Write(" = ");

                        UnaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                        WriteComma();
                        UnaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                        Write(" = " + JS.Types.SYSTEM_NULLABLE + "." + JS.Funcs.Math.LIFT1 + "(\"" + (op == UnaryOperatorType.PostDecrement ? JS.Funcs.Math.DEC : JS.Funcs.Math.INC) + "\", ");
                        UnaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                        AddOveflowFlag(typeCode, JS.Funcs.Math.DEC, true);
                        Write(")");
                        WriteComma();
                        Write(valueVar);
                        WriteCloseParentheses();
                        RemoveTempVar(valueVar);

                        Write(" : null");
                        break;
                }

                if (!isOneOp)
                {
                    Write(action);
                    WriteOpenParentheses();
                    WriteScript(op_name);
                    WriteComma();
                    new ExpressionListBlock(Emitter,
                        new Expression[] { UnaryOperatorExpression.Expression }, null, null, 0).Emit();
                    AddOveflowFlag(typeCode, op_name, true);
                    WriteCloseParentheses();
                }
            }
            else if (method == null)
            {
                string op_name = null;
                var isStatement = UnaryOperatorExpression.Parent is ExpressionStatement;

                if (isStatement)
                {
                    if (op == UnaryOperatorType.PostIncrement)
                    {
                        op = UnaryOperatorType.Increment;
                    }
                    else if (op == UnaryOperatorType.PostDecrement)
                    {
                        op = UnaryOperatorType.Decrement;
                    }
                }

                switch (op)
                {
                    case UnaryOperatorType.Minus:
                        op_name = JS.Funcs.Math.NEG;
                        break;

                    case UnaryOperatorType.Plus:
                        op_name = "clone";
                        break;

                    case UnaryOperatorType.BitNot:
                        op_name = "not";
                        break;

                    case UnaryOperatorType.Increment:
                    case UnaryOperatorType.Decrement:
                        if (!isStatement)
                        {
                            WriteOpenParentheses();
                        }

                        UnaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                        Write(" = ");
                        UnaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                        Write("." + (op == UnaryOperatorType.Decrement ? JS.Funcs.Math.DEC : JS.Funcs.Math.INC) + "(");
                        AddOveflowFlag(typeCode, JS.Funcs.Math.DEC, false);
                        Write(")");

                        if (!isStatement)
                        {
                            WriteCloseParentheses();
                        }
                        break;

                    case UnaryOperatorType.PostIncrement:
                    case UnaryOperatorType.PostDecrement:
                        WriteOpenParentheses();
                        var valueVar = GetTempVarName();

                        Write(valueVar);
                        Write(" = ");

                        UnaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                        WriteComma();
                        UnaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                        Write(" = ");
                        UnaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                        Write("." + (op == UnaryOperatorType.PostDecrement ? JS.Funcs.Math.DEC : JS.Funcs.Math.INC) + "(");
                        AddOveflowFlag(typeCode, JS.Funcs.Math.DEC, false);
                        Write("), ");
                        Write(valueVar);
                        WriteCloseParentheses();
                        RemoveTempVar(valueVar);
                        break;
                }

                if (!isOneOp)
                {
                    UnaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                    WriteDot();
                    Write(op_name);
                    WriteOpenParentheses();
                    AddOveflowFlag(typeCode, op_name, false);
                    WriteCloseParentheses();
                }
            }
            else
            {
                var inline = Emitter.GetInline(method);

                if (!string.IsNullOrWhiteSpace(inline))
                {
                    if (isOneOp)
                    {
                        var isStatement = UnaryOperatorExpression.Parent is ExpressionStatement;

                        if (isStatement || UnaryOperatorExpression.Operator == UnaryOperatorType.Increment ||
                            UnaryOperatorExpression.Operator == UnaryOperatorType.Decrement)
                        {
                            if (!isStatement)
                            {
                                WriteOpenParentheses();
                            }

                            UnaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                            Write(" = ");
                            new InlineArgumentsBlock(Emitter,
                                new ArgumentsInfo(Emitter, UnaryOperatorExpression, orr, method), inline).Emit
                                ();
                            if (!isStatement)
                            {
                                WriteCloseParentheses();
                            }
                        }
                        else
                        {
                            WriteOpenParentheses();
                            var valueVar = GetTempVarName();

                            Write(valueVar);
                            Write(" = ");

                            UnaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                            WriteComma();
                            UnaryOperatorExpression.Expression.AcceptVisitor(Emitter);
                            Write(" = ");
                            new InlineArgumentsBlock(Emitter, new ArgumentsInfo(Emitter, UnaryOperatorExpression, orr, method), inline).Emit();
                            WriteComma();
                            Write(valueVar);
                            WriteCloseParentheses();
                            RemoveTempVar(valueVar);
                        }
                    }
                    else
                    {
                        new InlineArgumentsBlock(Emitter,
                        new ArgumentsInfo(Emitter, UnaryOperatorExpression, orr, method), inline).Emit();
                    }
                }
                else if (!Emitter.Validator.IsExternalType(method.DeclaringTypeDefinition))
                {
                    Write(H5Types.ToJsName(method.DeclaringType, Emitter));
                    WriteDot();

                    Write(OverloadsCollection.Create(Emitter, method).GetOverloadName());

                    WriteOpenParentheses();

                    new ExpressionListBlock(Emitter,
                        new Expression[] { UnaryOperatorExpression.Expression }, null, null, 0).Emit();
                    WriteCloseParentheses();
                }
            }

            Emitter.UnaryOperatorType = oldType;
        }
    }
}