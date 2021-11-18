using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Expression = ICSharpCode.NRefactory.CSharp.Expression;

namespace H5.Translator
{
    public class BinaryOperatorBlock : ConversionBlock
    {
        public BinaryOperatorBlock(IEmitter emitter, BinaryOperatorExpression binaryOperatorExpression)
            : base(emitter, binaryOperatorExpression)
        {
            Emitter = emitter;
            BinaryOperatorExpression = binaryOperatorExpression;
        }

        public BinaryOperatorExpression BinaryOperatorExpression { get; set; }

        public List<IAsyncStep> EmittedAsyncSteps { get; set; }
        public bool NullStringCheck
        {
            get; private set;
        }

        protected override Expression GetExpression()
        {
            return BinaryOperatorExpression;
        }

        protected override void EmitConversionExpression()
        {
            VisitBinaryOperatorExpression();
        }

        internal void WriteAsyncBinaryExpression(int index)
        {
            if (Emitter.AsyncBlock.WrittenAwaitExpressions.Contains(BinaryOperatorExpression))
            {
                return;
            }

            Emitter.AsyncBlock.WrittenAwaitExpressions.Add(BinaryOperatorExpression);

            WriteAwaiters(BinaryOperatorExpression.Left);

            WriteIf();
            WriteOpenParentheses();

            var oldValue = Emitter.ReplaceAwaiterByVar;
            var oldAsyncExpressionHandling = Emitter.AsyncExpressionHandling;
            Emitter.ReplaceAwaiterByVar = true;
            Emitter.AsyncExpressionHandling = true;

            var isOr = BinaryOperatorExpression.Operator == BinaryOperatorType.BitwiseOr ||
                        BinaryOperatorExpression.Operator == BinaryOperatorType.ConditionalOr;

            if (isOr)
            {
                Write("!");
            }

            BinaryOperatorExpression.Left.AcceptVisitor(Emitter);
            WriteCloseParentheses();
            Emitter.ReplaceAwaiterByVar = oldValue;
            Emitter.AsyncExpressionHandling = oldAsyncExpressionHandling;

            int startCount = 0;
            IAsyncStep trueStep = null;
            startCount = Emitter.AsyncBlock.Steps.Count;

            EmittedAsyncSteps = Emitter.AsyncBlock.EmittedAsyncSteps;
            Emitter.AsyncBlock.EmittedAsyncSteps = new List<IAsyncStep>();

            var taskResultVar = JS.Vars.ASYNC_TASK_RESULT + index;
            if (!Emitter.Locals.ContainsKey(taskResultVar))
            {
                AddLocal(taskResultVar, null, AstType.Null);
            }

            WriteSpace();
            BeginBlock();
            Write($"{JS.Vars.ASYNC_STEP} = {Emitter.AsyncBlock.Step};");
            WriteNewLine();
            Write("continue;");
            var writer = SaveWriter();
            Emitter.AsyncBlock.AddAsyncStep();

            WriteAwaiters(BinaryOperatorExpression.Right);

            oldValue = Emitter.ReplaceAwaiterByVar;
            oldAsyncExpressionHandling = Emitter.AsyncExpressionHandling;
            Emitter.ReplaceAwaiterByVar = true;
            Emitter.AsyncExpressionHandling = true;
            Write(taskResultVar + " = ");
            BinaryOperatorExpression.Right.AcceptVisitor(Emitter);
            WriteSemiColon();
            Emitter.ReplaceAwaiterByVar = oldValue;
            Emitter.AsyncExpressionHandling = oldAsyncExpressionHandling;

            if (Emitter.AsyncBlock.Steps.Count > startCount)
            {
                trueStep = Emitter.AsyncBlock.Steps.Last();
            }

            if (RestoreWriter(writer) && !IsOnlyWhitespaceOnPenultimateLine(true))
            {
                WriteNewLine();
            }

            EndBlock();

            WriteNewLine();
            Write($"{taskResultVar} = {(isOr ? "true" : "false")};");
            WriteNewLine();
            Write($"{JS.Vars.ASYNC_STEP} = {Emitter.AsyncBlock.Step};");
            WriteNewLine();
            Write("continue;");
            var nextStep = Emitter.AsyncBlock.AddAsyncStep();

            if (trueStep != null)
            {
                trueStep.JumpToStep = nextStep.Step;
            }

            Emitter.AsyncBlock.EmittedAsyncSteps = EmittedAsyncSteps;
        }

        private IMethod FindOperatorTrueOrFalse(IType type, bool findTrue)
        {
            var isNullable = NullableType.IsNullable(type);
            if (isNullable)
            {
                type = NullableType.GetUnderlyingType(type);
            }

            return (IMethod)type.GetMethods(null, GetMemberOptions.IgnoreInheritedMembers).Single(m => m.Name == (findTrue ? "op_True" : "op_False") && m.Parameters.Count == 1 && NullableType.IsNullable(m.Parameters[0].Type) == isNullable);
        }

        protected bool ResolveOperator(BinaryOperatorExpression binaryOperatorExpression, OperatorResolveResult orr)
        {
            var method = orr?.UserDefinedOperatorMethod;

            if (method != null)
            {
                var inline = Emitter.GetInline(method);

                if (!string.IsNullOrWhiteSpace(inline))
                {
                    new InlineArgumentsBlock(Emitter,
                        new ArgumentsInfo(Emitter, binaryOperatorExpression, orr, method), inline).Emit();
                    return true;
                }
                else if (!Emitter.Validator.IsExternalType(method.DeclaringTypeDefinition))
                {
                    bool addClose = false;
                    string leftInterfaceTempVar = null;

                    if (orr.OperatorType == ExpressionType.OrElse || orr.OperatorType == ExpressionType.AndAlso)
                    {
                        var orElse = orr.OperatorType == ExpressionType.OrElse;
                        var left = orr.Operands[0];
                        bool isField = left is MemberResolveResult memberTargetrr && memberTargetrr.Member is IField &&
                                       (memberTargetrr.TargetResult is ThisResolveResult ||
                                        memberTargetrr.TargetResult is LocalResolveResult);

                        if (!(left is ThisResolveResult || left is TypeResolveResult || left is LocalResolveResult || left is ConstantResolveResult || isField))
                        {
                            WriteOpenParentheses();

                            leftInterfaceTempVar = GetTempVarName();
                            Write(leftInterfaceTempVar);
                            Write(" = ");

                            binaryOperatorExpression.Left.AcceptVisitor(Emitter);

                            WriteComma();

                            addClose = true;
                        }

                        var m = FindOperatorTrueOrFalse(left.Type, orElse);

                        Write(H5Types.ToJsName(m.DeclaringType, Emitter));
                        WriteDot();
                        Write(OverloadsCollection.Create(Emitter, m).GetOverloadName());

                        WriteOpenParentheses();

                        if (leftInterfaceTempVar != null)
                        {
                            Write(leftInterfaceTempVar);
                        }
                        else
                        {
                            binaryOperatorExpression.Left.AcceptVisitor(Emitter);
                        }

                        WriteCloseParentheses();

                        Write(" ? ");

                        if (leftInterfaceTempVar != null)
                        {
                            Write(leftInterfaceTempVar);
                        }
                        else
                        {
                            binaryOperatorExpression.Left.AcceptVisitor(Emitter);
                        }

                        Write(" : ");
                    }

                    if (orr.IsLiftedOperator)
                    {
                        Write(JS.Types.SYSTEM_NULLABLE + ".");

                        string action = JS.Funcs.Math.LIFT;

                        switch (BinaryOperatorExpression.Operator)
                        {
                            case BinaryOperatorType.GreaterThan:
                                action = JS.Funcs.Math.LIFTCMP;
                                break;

                            case BinaryOperatorType.GreaterThanOrEqual:
                                action = JS.Funcs.Math.LIFTCMP;
                                break;

                            case BinaryOperatorType.Equality:
                                action = JS.Funcs.Math.LIFTEQ;
                                break;

                            case BinaryOperatorType.InEquality:
                                action = JS.Funcs.Math.LIFTNE;
                                break;

                            case BinaryOperatorType.LessThan:
                                action = JS.Funcs.Math.LIFTCMP;
                                break;

                            case BinaryOperatorType.LessThanOrEqual:
                                action = JS.Funcs.Math.LIFTCMP;
                                break;
                        }

                        Write(action + "(");
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

                    if (leftInterfaceTempVar != null)
                    {
                        Write(leftInterfaceTempVar);
                        Write(", ");
                        binaryOperatorExpression.Right.AcceptVisitor(Emitter);
                    }
                    else
                    {
                        new ExpressionListBlock(Emitter,
                        new Expression[] { binaryOperatorExpression.Left, binaryOperatorExpression.Right }, null, null, 0).Emit();
                    }

                    WriteCloseParentheses();

                    if (addClose)
                    {
                        WriteCloseParentheses();
                    }

                    return true;
                }
            }

            return false;
        }

        public static bool IsOperatorSimple(BinaryOperatorExpression binaryOperatorExpression, IEmitter emitter)
        {
            var leftResolverResult = emitter.Resolver.ResolveNode(binaryOperatorExpression.Left);
            var rightResolverResult = emitter.Resolver.ResolveNode(binaryOperatorExpression.Right);
            bool leftIsSimple = binaryOperatorExpression.Left is PrimitiveExpression || leftResolverResult.Type.IsKnownType(KnownTypeCode.String) ||
                                 leftResolverResult.Type.IsReferenceType != null && !leftResolverResult.Type.IsReferenceType.Value;

            bool rightIsSimple = binaryOperatorExpression.Right is PrimitiveExpression || rightResolverResult.Type.IsKnownType(KnownTypeCode.String) ||
                                 rightResolverResult.Type.IsReferenceType != null && !rightResolverResult.Type.IsReferenceType.Value;

            bool isSimpleConcat = leftIsSimple && rightIsSimple;

            if (!isSimpleConcat)
            {
                var be = binaryOperatorExpression.Left as BinaryOperatorExpression;
                leftIsSimple = be != null ? IsOperatorSimple(be, emitter) : leftIsSimple;

                be = binaryOperatorExpression.Right as BinaryOperatorExpression;
                rightIsSimple = be != null ? IsOperatorSimple(be, emitter) : rightIsSimple;
                isSimpleConcat = leftIsSimple && rightIsSimple;
            }

            return isSimpleConcat;
        }

        protected void VisitBinaryOperatorExpression()
        {
            BinaryOperatorExpression binaryOperatorExpression = BinaryOperatorExpression;

            if (Emitter.IsAsync && (
                binaryOperatorExpression.Operator == BinaryOperatorType.BitwiseAnd ||
                binaryOperatorExpression.Operator == BinaryOperatorType.BitwiseOr ||
                binaryOperatorExpression.Operator == BinaryOperatorType.ConditionalOr ||
                binaryOperatorExpression.Operator == BinaryOperatorType.ConditionalAnd
                ) && GetAwaiters(binaryOperatorExpression).Length > 0)
            {
                if (Emitter.AsyncBlock.WrittenAwaitExpressions.Contains(binaryOperatorExpression))
                {
                    var index = Array.IndexOf(Emitter.AsyncBlock.AwaitExpressions, binaryOperatorExpression) + 1;
                    Write(JS.Vars.ASYNC_TASK_RESULT + index);
                }
                else
                {
                    var index = Array.IndexOf(Emitter.AsyncBlock.AwaitExpressions, binaryOperatorExpression) + 1;
                    WriteAsyncBinaryExpression(index);
                }

                return;
            }

            var resolveOperator = Emitter.Resolver.ResolveNode(binaryOperatorExpression);
            var expectedType = Emitter.Resolver.Resolver.GetExpectedType(binaryOperatorExpression);
            bool isDecimalExpected = Helpers.IsDecimalType(expectedType, Emitter.Resolver);
            bool isDecimal = Helpers.IsDecimalType(resolveOperator.Type, Emitter.Resolver);
            bool isLongExpected = Helpers.Is64Type(expectedType, Emitter.Resolver);
            bool isLong = Helpers.Is64Type(resolveOperator.Type, Emitter.Resolver);
            OperatorResolveResult orr = resolveOperator as OperatorResolveResult;
            var leftResolverResult = Emitter.Resolver.ResolveNode(binaryOperatorExpression.Left);
            var rightResolverResult = Emitter.Resolver.ResolveNode(binaryOperatorExpression.Right);
            var charToString = -1;
            string variable = null;
            bool leftIsNull = BinaryOperatorExpression.Left is NullReferenceExpression;
            bool rightIsNull = BinaryOperatorExpression.Right is NullReferenceExpression;
            bool isUint = resolveOperator.Type.IsKnownType(KnownTypeCode.UInt16) ||
                          resolveOperator.Type.IsKnownType(KnownTypeCode.UInt32) ||
                          resolveOperator.Type.IsKnownType(KnownTypeCode.UInt64);

            var isFloatResult = Helpers.IsFloatType(resolveOperator.Type, Emitter.Resolver);
            var leftExpected = Emitter.Resolver.Resolver.GetExpectedType(binaryOperatorExpression.Left);
            var rightExpected = Emitter.Resolver.Resolver.GetExpectedType(binaryOperatorExpression.Right);
            var strictNullChecks = Emitter.AssemblyInfo.StrictNullChecks;

            if (orr != null && orr.Type.IsKnownType(KnownTypeCode.String))
            {
                for (int i = 0; i < orr.Operands.Count; i++)
                {
                    if (orr.Operands[i] is ConversionResolveResult crr && crr.Input.Type.IsKnownType(KnownTypeCode.Char))
                    {
                        charToString = i;
                    }
                }
            }

            if (resolveOperator is ConstantResolveResult)
            {
                WriteScript(((ConstantResolveResult)resolveOperator).ConstantValue);
                return;
            }

            var resultIsString = expectedType.IsKnownType(KnownTypeCode.String) || resolveOperator.Type.IsKnownType(KnownTypeCode.String);
            var isStringConcat = resultIsString && binaryOperatorExpression.Operator == BinaryOperatorType.Add;
            var toStringForLeft = false;
            var toStringForRight = false;

            var parentBinary = binaryOperatorExpression.Parent as BinaryOperatorExpression;
            bool parentIsString = resultIsString && parentBinary != null && parentBinary.Operator == BinaryOperatorType.Add;

            if (parentIsString)
            {
                var parentResolveOperator = Emitter.Resolver.ResolveNode(binaryOperatorExpression.Parent) as OperatorResolveResult;

                if (parentResolveOperator != null && parentResolveOperator.UserDefinedOperatorMethod != null || IsOperatorSimple(parentBinary, Emitter))
                {
                    parentIsString = false;
                }
            }

            bool isSimpleConcat = isStringConcat && IsOperatorSimple(binaryOperatorExpression, Emitter);

            if (charToString == -1 && isStringConcat && !leftResolverResult.Type.IsKnownType(KnownTypeCode.String))
            {
                toStringForLeft = true;
            }

            if (charToString == -1 && isStringConcat && !rightResolverResult.Type.IsKnownType(KnownTypeCode.String))
            {
                toStringForRight = true;
            }

            if (!isStringConcat && (Helpers.IsDecimalType(leftResolverResult.Type, Emitter.Resolver) || Helpers.IsDecimalType(rightResolverResult.Type, Emitter.Resolver)))
            {
                isDecimal = true;
                isDecimalExpected = true;
            }

            if (isDecimal && isDecimalExpected && binaryOperatorExpression.Operator != BinaryOperatorType.NullCoalescing)
            {
                HandleDecimal(resolveOperator);
                return;
            }

            var isLeftLong = Helpers.Is64Type(leftExpected, Emitter.Resolver);
            var isRightLong = Helpers.Is64Type(rightExpected, Emitter.Resolver);

            if (!isLeftLong && !isRightLong)
            {
                if (leftExpected.Kind == TypeKind.Enum && Helpers.Is64Type(leftExpected.GetDefinition().EnumUnderlyingType, Emitter.Resolver))
                {
                    isLeftLong = true;
                }

                if (rightExpected.Kind == TypeKind.Enum && Helpers.Is64Type(rightExpected.GetDefinition().EnumUnderlyingType, Emitter.Resolver))
                {
                    isRightLong = true;
                }
            }

            if (!(resultIsString && binaryOperatorExpression.Operator == BinaryOperatorType.Add) && (isLeftLong || isRightLong))
            {
                isLong = true;
                isLongExpected = true;
            }

            if (isLong && isLongExpected && binaryOperatorExpression.Operator != BinaryOperatorType.NullCoalescing)
            {
                if (!isFloatResult || binaryOperatorExpression.Operator == BinaryOperatorType.Divide && isLeftLong)
                {
                    HandleLong(resolveOperator, isUint);
                    return;
                }
            }

            var delegateOperator = false;

            if (ResolveOperator(binaryOperatorExpression, orr))
            {
                return;
            }

            if (binaryOperatorExpression.Operator == BinaryOperatorType.Equality || binaryOperatorExpression.Operator == BinaryOperatorType.InEquality)
            {
                if (leftIsNull || rightIsNull)
                {
                    WritePart(binaryOperatorExpression.Left, toStringForLeft, leftResolverResult);

                    if (binaryOperatorExpression.Operator == BinaryOperatorType.Equality)
                    {
                        Write(strictNullChecks ? " === " : " == ");
                    }
                    else
                    {
                        Write(strictNullChecks ? " !== " : " != ");
                    }

                    WritePart(binaryOperatorExpression.Right, toStringForRight, rightResolverResult);
                    return;
                }
            }

            var insideOverflowContext = InsideOverflowContext(Emitter, binaryOperatorExpression);
            if (binaryOperatorExpression.Operator == BinaryOperatorType.Divide && Emitter.Rules.Integer == IntegerRule.Managed &&
                !(Emitter.IsJavaScriptOverflowMode && !insideOverflowContext) &&
                (
                    (Helpers.IsIntegerType(leftResolverResult.Type, Emitter.Resolver) &&
                    Helpers.IsIntegerType(rightResolverResult.Type, Emitter.Resolver)) ||

                    (Helpers.IsIntegerType(Emitter.Resolver.Resolver.GetExpectedType(binaryOperatorExpression.Left), Emitter.Resolver) &&
                    Helpers.IsIntegerType(Emitter.Resolver.Resolver.GetExpectedType(binaryOperatorExpression.Right), Emitter.Resolver))
                ))
            {
                Write(JS.Types.H5_INT + "." + JS.Funcs.Math.DIV + "(");
                WritePart(binaryOperatorExpression.Left, toStringForLeft, leftResolverResult);
                Write(", ");
                WritePart(binaryOperatorExpression.Right, toStringForRight, rightResolverResult);
                Write(")");
                return;
            }

            if (binaryOperatorExpression.Operator == BinaryOperatorType.Multiply && Emitter.Rules.Integer == IntegerRule.Managed &&
                !(Emitter.IsJavaScriptOverflowMode && !insideOverflowContext) &&
                (
                    (Helpers.IsInteger32Type(leftResolverResult.Type, Emitter.Resolver) &&
                    Helpers.IsInteger32Type(rightResolverResult.Type, Emitter.Resolver) &&
                    Helpers.IsInteger32Type(resolveOperator.Type, Emitter.Resolver)) ||

                    (Helpers.IsInteger32Type(Emitter.Resolver.Resolver.GetExpectedType(binaryOperatorExpression.Left), Emitter.Resolver) &&
                    Helpers.IsInteger32Type(Emitter.Resolver.Resolver.GetExpectedType(binaryOperatorExpression.Right), Emitter.Resolver) &&
                    Helpers.IsInteger32Type(resolveOperator.Type, Emitter.Resolver))
                ))
            {
                isUint = NullableType.GetUnderlyingType(resolveOperator.Type).IsKnownType(KnownTypeCode.UInt32);
                Write(JS.Types.H5_INT + "." + (isUint ? JS.Funcs.Math.UMUL : JS.Funcs.Math.MUL) + "(");
                WritePart(binaryOperatorExpression.Left, toStringForLeft, leftResolverResult);
                Write(", ");
                WritePart(binaryOperatorExpression.Right, toStringForRight, rightResolverResult);

                if (IsInCheckedContext(Emitter, BinaryOperatorExpression))
                {
                    Write(", 1");
                }

                Write(")");
                return;
            }

            if (binaryOperatorExpression.Operator == BinaryOperatorType.Add ||
                binaryOperatorExpression.Operator == BinaryOperatorType.Subtract)
            {
                var add = binaryOperatorExpression.Operator == BinaryOperatorType.Add;

                if (expectedType.Kind == TypeKind.Delegate || Emitter.Validator.IsDelegateOrLambda(leftResolverResult) && Emitter.Validator.IsDelegateOrLambda(rightResolverResult))
                {
                    delegateOperator = true;
                    Write(add ? JS.Funcs.H5_COMBINE : JS.Funcs.H5_REMOVE);
                    WriteOpenParentheses();
                }
            }

            NullStringCheck = isStringConcat && !parentIsString && isSimpleConcat;
            if (isStringConcat && !parentIsString && !isSimpleConcat)
            {
                Write(JS.Types.System.String.CONCAT);
                WriteOpenParentheses();
            }

            bool nullable = orr != null && orr.IsLiftedOperator;
            bool isCoalescing = (Emitter.AssemblyInfo.StrictNullChecks ||
                                 NullableType.IsNullable(leftResolverResult.Type) ||
                                 leftResolverResult.Type.IsKnownType(KnownTypeCode.String) ||
                                 leftResolverResult.Type.IsKnownType(KnownTypeCode.Object)
                                ) && binaryOperatorExpression.Operator == BinaryOperatorType.NullCoalescing;
            string root = JS.Types.SYSTEM_NULLABLE + ".";
            bool special = nullable;
            bool rootSpecial = nullable;
            bool isBool = NullableType.IsNullable(resolveOperator.Type) ? NullableType.GetUnderlyingType(resolveOperator.Type).IsKnownType(KnownTypeCode.Boolean) : resolveOperator.Type.IsKnownType(KnownTypeCode.Boolean);
            bool toBool = isBool && !rootSpecial && !delegateOperator && (binaryOperatorExpression.Operator == BinaryOperatorType.BitwiseAnd || binaryOperatorExpression.Operator == BinaryOperatorType.BitwiseOr);
            bool isRefEquals = !isCoalescing && !strictNullChecks &&
                    (binaryOperatorExpression.Operator == BinaryOperatorType.InEquality || binaryOperatorExpression.Operator == BinaryOperatorType.Equality) &&
                    leftExpected.IsReferenceType.HasValue && leftExpected.IsReferenceType.Value &&
                    rightExpected.IsReferenceType.HasValue && rightExpected.IsReferenceType.Value;

            if (rootSpecial)
            {
                Write(root);
            }
            else if (!isRefEquals)
            {
                if (isCoalescing)
                {
                    Write("(");
                    variable = GetTempVarName();
                    Write(variable);
                    Write(" = ");
                }
                else if (charToString == 0)
                {
                    Write(JS.Funcs.STRING_FROMCHARCODE + "(");
                }

                if (toBool)
                {
                    Write("!!(");
                }

                WritePart(binaryOperatorExpression.Left, toStringForLeft, leftResolverResult, isCoalescing);

                if (isCoalescing)
                {
                    Write(", ");
                    Write(variable);

                    Write(strictNullChecks ? " !== null" : " != null");

                    Write(" ? ");

                    expressionMap.Add(binaryOperatorExpression.Left, variable);
                    //this.Write(variable);
                    binaryOperatorExpression.Left.AcceptVisitor(Emitter);
                    expressionMap.Remove(binaryOperatorExpression.Left);
                }
                else if (charToString == 0)
                {
                    Write(")");
                }
            }

            if (isRefEquals)
            {
                if (binaryOperatorExpression.Operator == BinaryOperatorType.InEquality)
                {
                    Write("!");
                }
                Write(((Emitter.AssemblyInfo.Rules.UseShortForms ?? false) ? JS.Funcs.SHORTEN_H5_REFERENCEEQUALS : JS.Funcs.H5_REFERENCEEQUALS) );
                special = true;
            }

            if (!delegateOperator && (!isStringConcat || isSimpleConcat))
            {
                if (!special)
                {
                    WriteSpace();
                }

                switch (binaryOperatorExpression.Operator)
                {
                    case BinaryOperatorType.Add:
                        Write(rootSpecial ? JS.Funcs.Math.ADD : "+");
                        break;

                    case BinaryOperatorType.BitwiseAnd:
                        if (isBool)
                        {
                            Write(rootSpecial ? JS.Funcs.Math.AND : "&");
                        }
                        else
                        {
                            Write(rootSpecial ? JS.Funcs.Math.BAND : "&");
                        }

                        break;

                    case BinaryOperatorType.BitwiseOr:
                        if (isBool)
                        {
                            Write(rootSpecial ? JS.Funcs.Math.OR : "|");
                        }
                        else
                        {
                            Write(rootSpecial ? JS.Funcs.Math.BOR : "|");
                        }
                        break;

                    case BinaryOperatorType.ConditionalAnd:
                        Write(rootSpecial ? JS.Funcs.Math.AND : "&&");
                        break;

                    case BinaryOperatorType.NullCoalescing:
                        Write(isCoalescing ? ":" : "||");
                        break;

                    case BinaryOperatorType.ConditionalOr:
                        Write(rootSpecial ? JS.Funcs.Math.OR : "||");
                        break;

                    case BinaryOperatorType.Divide:
                        Write(rootSpecial ? JS.Funcs.Math.DIV : "/");
                        break;

                    case BinaryOperatorType.Equality:
                        if (!isRefEquals)
                        {
                            Write(rootSpecial ? "eq" : "===");
                        }

                        break;

                    case BinaryOperatorType.ExclusiveOr:
                        Write(rootSpecial ? JS.Funcs.Math.XOR : (isBool ? "!=" : "^"));
                        break;

                    case BinaryOperatorType.GreaterThan:
                        Write(rootSpecial ? JS.Funcs.Math.GT : ">");
                        break;

                    case BinaryOperatorType.GreaterThanOrEqual:
                        Write(rootSpecial ? JS.Funcs.Math.GTE : ">=");
                        break;

                    case BinaryOperatorType.InEquality:
                        if (!isRefEquals)
                        {
                            Write(rootSpecial ? "neq" : "!==");
                        }
                        break;

                    case BinaryOperatorType.LessThan:
                        Write(rootSpecial ? JS.Funcs.Math.LT : "<");
                        break;

                    case BinaryOperatorType.LessThanOrEqual:
                        Write(rootSpecial ? JS.Funcs.Math.LTE : "<=");
                        break;

                    case BinaryOperatorType.Modulus:
                        Write(rootSpecial ? JS.Funcs.Math.MOD : "%");
                        break;

                    case BinaryOperatorType.Multiply:
                        Write(rootSpecial ? JS.Funcs.Math.MUL : "*");
                        break;

                    case BinaryOperatorType.ShiftLeft:
                        Write(rootSpecial ? JS.Funcs.Math.SL : "<<");
                        break;

                    case BinaryOperatorType.ShiftRight:
                        if (isUint)
                        {
                            Write(rootSpecial ? JS.Funcs.Math.SRR : ">>>");
                        }
                        else
                        {
                            Write(rootSpecial ? JS.Funcs.Math.SR : ">>");
                        }

                        break;

                    case BinaryOperatorType.Subtract:
                        Write(rootSpecial ? JS.Funcs.Math.SUB : "-");
                        break;

                    default:
                        throw new EmitterException(binaryOperatorExpression, "Unsupported binary operator: " + binaryOperatorExpression.Operator.ToString());
                }
            }
            else
            {
                WriteComma();
            }

            if (special)
            {
                WriteOpenParentheses();
                if (charToString == 0)
                {
                    Write(JS.Funcs.STRING_FROMCHARCODE + "(");
                }

                WritePart(binaryOperatorExpression.Left, toStringForLeft, leftResolverResult);

                if (charToString == 0)
                {
                    Write(")");
                }

                WriteComma();
            }
            else if (!delegateOperator && (!isStringConcat || isSimpleConcat))
            {
                WriteSpace();
            }

            if (charToString == 1)
            {
                Write(JS.Funcs.STRING_FROMCHARCODE + "(");
            }

            WritePart(binaryOperatorExpression.Right, toStringForRight, rightResolverResult);

            if (toBool)
            {
                WriteCloseParentheses();
            }

            if (charToString == 1 || isCoalescing)
            {
                WriteCloseParentheses();
            }

            if (delegateOperator || special || isStringConcat && !parentIsString && !isSimpleConcat)
            {
                WriteCloseParentheses();
            }
        }

        private void HandleType(ResolveResult resolveOperator, KnownTypeCode typeCode, string op_name, string action)
        {
            var orr = resolveOperator as OperatorResolveResult;
            var method = orr?.UserDefinedOperatorMethod;

            if (orr != null && method == null)
            {
                var name = Helpers.GetBinaryOperatorMethodName(BinaryOperatorExpression.Operator);
                var type = Emitter.Resolver.Compilation.FindType(typeCode);
                method = type.GetMethods(m => m.Name == name, GetMemberOptions.IgnoreInheritedMembers).FirstOrDefault();
            }

            if (method != null)
            {
                var inline = Emitter.GetInline(method);

                if (orr.IsLiftedOperator)
                {
                    Write(JS.Types.SYSTEM_NULLABLE + ".");
                    Write(action);
                    WriteOpenParentheses();
                    WriteScript(op_name);
                    WriteComma();
                    new ExpressionListBlock(Emitter,
                        new Expression[] { BinaryOperatorExpression.Left, BinaryOperatorExpression.Right }, null, null, 0)
                        .Emit();
                    AddOveflowFlag(typeCode, op_name);
                    WriteCloseParentheses();
                }
                else if (!string.IsNullOrWhiteSpace(inline))
                {
                    new InlineArgumentsBlock(Emitter,
                        new ArgumentsInfo(Emitter, BinaryOperatorExpression, orr, method), inline).Emit();
                }
                else if (!Emitter.Validator.IsExternalType(method.DeclaringTypeDefinition))
                {
                    Write(H5Types.ToJsName(method.DeclaringType, Emitter));
                    WriteDot();

                    Write(OverloadsCollection.Create(Emitter, method).GetOverloadName());

                    WriteOpenParentheses();

                    new ExpressionListBlock(Emitter,
                        new Expression[] { BinaryOperatorExpression.Left, BinaryOperatorExpression.Right }, null, null, 0)
                        .Emit();
                    AddOveflowFlag(typeCode, op_name);
                    WriteCloseParentheses();
                }
            }
            else
            {
                if (orr.IsLiftedOperator)
                {
                    Write(JS.Types.SYSTEM_NULLABLE + ".");
                    Write(action);
                    WriteOpenParentheses();
                    WriteScript(op_name);
                    WriteComma();
                    new ExpressionListBlock(Emitter,
                        new Expression[] { BinaryOperatorExpression.Left, BinaryOperatorExpression.Right }, null, null, 0)
                        .Emit();
                    AddOveflowFlag(typeCode, op_name);
                    WriteCloseParentheses();
                }
                else
                {
                    BinaryOperatorExpression.Left.AcceptVisitor(Emitter);
                    WriteDot();
                    Write(op_name);
                    WriteOpenParentheses();
                    BinaryOperatorExpression.Right.AcceptVisitor(Emitter);
                    AddOveflowFlag(typeCode, op_name);
                    WriteCloseParentheses();
                }
            }
        }

        private void AddOveflowFlag(KnownTypeCode typeCode, string op_name)
        {
            if ((typeCode == KnownTypeCode.Int64 || typeCode == KnownTypeCode.UInt64) && IsInCheckedContext(Emitter, BinaryOperatorExpression))
            {
                if (op_name == JS.Funcs.Math.ADD || op_name == JS.Funcs.Math.SUB || op_name == JS.Funcs.Math.MUL)
                {
                    Write(", 1");
                }
            }
        }

        private void HandleDecimal(ResolveResult resolveOperator)
        {
            string action = JS.Funcs.Math.LIFT2;
            string op_name = null;

            switch (BinaryOperatorExpression.Operator)
            {
                case BinaryOperatorType.GreaterThan:
                    op_name = JS.Funcs.Math.GT;
                    action = JS.Funcs.Math.LIFTCMP;
                    break;

                case BinaryOperatorType.GreaterThanOrEqual:
                    op_name = JS.Funcs.Math.GTE;
                    action = JS.Funcs.Math.LIFTCMP;
                    break;

                case BinaryOperatorType.Equality:
                    op_name = JS.Funcs.Math.EQUALS;
                    action = JS.Funcs.Math.LIFTEQ;
                    break;

                case BinaryOperatorType.InEquality:
                    op_name = JS.Funcs.Math.NE;
                    action = JS.Funcs.Math.LIFTNE;
                    break;

                case BinaryOperatorType.LessThan:
                    op_name = JS.Funcs.Math.LT;
                    action = JS.Funcs.Math.LIFTCMP;
                    break;

                case BinaryOperatorType.LessThanOrEqual:
                    op_name = JS.Funcs.Math.LTE;
                    action = JS.Funcs.Math.LIFTCMP;
                    break;

                case BinaryOperatorType.Add:
                    op_name = JS.Funcs.Math.ADD;
                    break;

                case BinaryOperatorType.Subtract:
                    op_name = JS.Funcs.Math.SUB;
                    break;

                case BinaryOperatorType.Multiply:
                    op_name = JS.Funcs.Math.MUL;
                    break;

                case BinaryOperatorType.Divide:
                    op_name = JS.Funcs.Math.DIV;
                    break;

                case BinaryOperatorType.Modulus:
                    op_name = JS.Funcs.Math.MOD;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            HandleType(resolveOperator, KnownTypeCode.Decimal, op_name, action);
        }

        private void HandleLong(ResolveResult resolveOperator, bool isUint)
        {
            string action = JS.Funcs.Math.LIFT2;
            string op_name = null;

            switch (BinaryOperatorExpression.Operator)
            {
                case BinaryOperatorType.GreaterThan:
                    op_name = JS.Funcs.Math.GT;
                    action = JS.Funcs.Math.LIFTCMP;
                    break;

                case BinaryOperatorType.GreaterThanOrEqual:
                    op_name = JS.Funcs.Math.GTE;
                    action = JS.Funcs.Math.LIFTCMP;
                    break;

                case BinaryOperatorType.Equality:
                    op_name = JS.Funcs.Math.EQUALS;
                    action = JS.Funcs.Math.LIFTEQ;
                    break;

                case BinaryOperatorType.InEquality:
                    op_name = JS.Funcs.Math.NE;
                    action = JS.Funcs.Math.LIFTNE;
                    break;

                case BinaryOperatorType.LessThan:
                    op_name = JS.Funcs.Math.LT;
                    action = JS.Funcs.Math.LIFTCMP;
                    break;

                case BinaryOperatorType.LessThanOrEqual:
                    op_name = JS.Funcs.Math.LTE;
                    action = JS.Funcs.Math.LIFTCMP;
                    break;

                case BinaryOperatorType.Add:
                    op_name = JS.Funcs.Math.ADD;
                    break;

                case BinaryOperatorType.Subtract:
                    op_name = JS.Funcs.Math.SUB;
                    break;

                case BinaryOperatorType.Multiply:
                    op_name = JS.Funcs.Math.MUL;
                    break;

                case BinaryOperatorType.Divide:
                    op_name = JS.Funcs.Math.DIV;
                    if (Helpers.IsFloatType(resolveOperator.Type, Emitter.Resolver))
                    {
                        op_name = "JS.Funcs.Math.TO_NUMBER_DIVIDED";
                    }
                    break;

                case BinaryOperatorType.Modulus:
                    op_name = JS.Funcs.Math.MOD;
                    break;

                case BinaryOperatorType.BitwiseAnd:
                    op_name = JS.Funcs.Math.AND;
                    break;

                case BinaryOperatorType.BitwiseOr:
                    op_name = JS.Funcs.Math.OR;
                    break;

                case BinaryOperatorType.ExclusiveOr:
                    op_name = JS.Funcs.Math.XOR;
                    break;

                case BinaryOperatorType.ShiftLeft:
                    op_name = JS.Funcs.Math.SHL;
                    break;

                case BinaryOperatorType.ShiftRight:
                    op_name = isUint ? JS.Funcs.Math.SHRU : JS.Funcs.Math.SHR;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            HandleType(resolveOperator, isUint ? KnownTypeCode.UInt64 : KnownTypeCode.Int64, op_name, action);
        }

        private void WritePart(Expression expression, bool toString, ResolveResult rr, bool isCoalescing = false)
        {
            if (isCoalescing)
            {
                expressionInWork.Add(expression);
            }

            bool wrapString = false;
            if (NullStringCheck && rr.Type.IsKnownType(KnownTypeCode.String))
            {
                wrapString = !(expression is BinaryOperatorExpression) && !(expression is PrimitiveExpression || rr.Type.IsReferenceType != null && !rr.Type.IsReferenceType.Value);
            }

            if (toString)
            {
                var toStringMethod = rr.Type.GetMembers().FirstOrDefault(m =>
                {
                    if (m.Name == CS.Methods.TOSTRING && !m.IsStatic && m.ReturnType.IsKnownType(KnownTypeCode.String) && m.IsOverride)
                    {
                        if (m is IMethod method && method.Parameters.Count == 0 && method.TypeParameters.Count == 0)
                        {
                            return true;
                        }
                    }

                    return false;
                });

                if (toStringMethod != null)
                {
                    var inline = Emitter.GetInline(toStringMethod);

                    if (inline != null)
                    {
                        var writer = new Writer
                        {
                            InlineCode = inline,
                            Output = Emitter.Output,
                            IsNewLine = Emitter.IsNewLine
                        };
                        Emitter.IsNewLine = false;
                        Emitter.Output = new StringBuilder();

                        expression.AcceptVisitor(Emitter);

                        string result = Emitter.Output.ToString();
                        Emitter.Output = writer.Output;
                        Emitter.IsNewLine = writer.IsNewLine;

                        var argsInfo = new ArgumentsInfo(Emitter, expression, (IMethod)toStringMethod);
                        argsInfo.ArgumentsExpressions = new Expression[] { expression };
                        argsInfo.ArgumentsNames = new string[] { "this" };
                        argsInfo.ThisArgument = result;
                        new InlineArgumentsBlock(Emitter, argsInfo, writer.InlineCode).Emit();
                        return;
                    }
                }
            }

            if (wrapString)
            {
                Write("(");
            }

            expression.AcceptVisitor(Emitter);

            if (wrapString)
            {
                Write(" || \"\")");
            }

            if (isCoalescing)
            {
                expressionInWork.Remove(expression);
            }
        }
    }
}