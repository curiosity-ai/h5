using H5.Contract;
using H5.Contract.Constants;
using H5.Translator.Utils;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using System;
using System.Linq;

namespace H5.Translator
{
    public class AssignmentBlock : ConversionBlock
    {
        public AssignmentBlock(IEmitter emitter, AssignmentExpression assignmentExpression)
            : base(emitter, assignmentExpression)
        {
            Emitter = emitter;
            AssignmentExpression = assignmentExpression;
        }

        public AssignmentExpression AssignmentExpression { get; set; }

        protected override Expression GetExpression()
        {
            return AssignmentExpression;
        }

        protected override void EmitConversionExpression()
        {
            VisitAssignmentExpression();
        }

        protected bool ResolveOperator(AssignmentExpression assignmentExpression, OperatorResolveResult orr, int initCount, bool thisAssignment)
        {
            var method = orr?.UserDefinedOperatorMethod;

            if (method != null)
            {
                var inline = Emitter.GetInline(method);

                if (!string.IsNullOrWhiteSpace(inline))
                {
                    if (Emitter.Writers.Count == initCount && !thisAssignment)
                    {
                        Write("= ");
                    }

                    new InlineArgumentsBlock(Emitter,
                        new ArgumentsInfo(Emitter, assignmentExpression, orr, method), inline).Emit();

                    if (Emitter.Writers.Count > initCount)
                    {
                        PopWriter();
                    }
                    return true;
                }
                else if (!Emitter.Validator.IsExternalType(method.DeclaringTypeDefinition))
                {
                    if (Emitter.Writers.Count == initCount && !thisAssignment)
                    {
                        Write("= ");
                    }

                    if (orr.IsLiftedOperator)
                    {
                        Write(JS.Types.SYSTEM_NULLABLE + "." + JS.Funcs.Math.LIFT + ".(");
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

                    new ExpressionListBlock(Emitter,
                        new Expression[] { assignmentExpression.Left, assignmentExpression.Right }, null, null, 0).Emit();
                    WriteCloseParentheses();

                    if (Emitter.Writers.Count > initCount)
                    {
                        PopWriter();
                    }
                    return true;
                }
            }

            return false;
        }

        protected bool IsUserOperator(OperatorResolveResult orr)
        {
            var method = orr?.UserDefinedOperatorMethod;

            if (method != null)
            {
                var inline = Emitter.GetInline(method);

                if (!string.IsNullOrWhiteSpace(inline))
                {
                    return true;
                }
                else if (!Emitter.Validator.IsExternalType(method.DeclaringTypeDefinition))
                {
                    return true;
                }
            }

            return false;
        }

        protected void VisitAssignmentExpression()
        {
            AssignmentExpression assignmentExpression = AssignmentExpression;
            var oldAssigment = Emitter.IsAssignment;
            var oldAssigmentType = Emitter.AssignmentType;
            string variable = null;

            bool needReturnValue = !(assignmentExpression.Parent is ExpressionStatement);

            if (needReturnValue && assignmentExpression.Parent is LambdaExpression)
            {
                if (Emitter.Resolver.ResolveNode(assignmentExpression.Parent) is LambdaResolveResult lambdarr && lambdarr.ReturnType.Kind == TypeKind.Void)
                {
                    needReturnValue = false;
                }
            }

            var delegateAssigment = false;
            bool isEvent = false;
            var initCount = Emitter.Writers.Count;

            var asyncExpressionHandling = Emitter.AsyncExpressionHandling;

            WriteAwaiters(assignmentExpression.Left);
            WriteAwaiters(assignmentExpression.Right);

            var leftResolverResult = Emitter.Resolver.ResolveNode(assignmentExpression.Left);
            var rightResolverResult = Emitter.Resolver.ResolveNode(assignmentExpression.Right);
            var rr = Emitter.Resolver.ResolveNode(assignmentExpression);
            var orr = rr as OperatorResolveResult;
            bool isDecimal = Helpers.IsDecimalType(rr.Type, Emitter.Resolver);
            bool isLong = Helpers.Is64Type(rr.Type, Emitter.Resolver);
            var expectedType = Emitter.Resolver.Resolver.GetExpectedType(assignmentExpression);
            bool isDecimalExpected = Helpers.IsDecimalType(expectedType, Emitter.Resolver);
            bool isLongExpected = Helpers.Is64Type(expectedType, Emitter.Resolver);
            bool isUserOperator = IsUserOperator(orr);

            var rrType = rr.Type;

            if (rrType.Kind == TypeKind.Enum)
            {
                rrType = rrType.GetDefinition().EnumUnderlyingType;
            }

            bool isUint = rrType.IsKnownType(KnownTypeCode.UInt16) ||
                          rrType.IsKnownType(KnownTypeCode.UInt32) ||
                          rrType.IsKnownType(KnownTypeCode.UInt64);

            if (!isLong && rr.Type.Kind == TypeKind.Enum && Helpers.Is64Type(rr.Type.GetDefinition().EnumUnderlyingType, Emitter.Resolver))
            {
                isLong = true;
            }

            if (!isLongExpected && expectedType.Kind == TypeKind.Enum && Helpers.Is64Type(expectedType.GetDefinition().EnumUnderlyingType, Emitter.Resolver))
            {
                isLongExpected = true;
            }

            var charToString = -1;

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

            var memberTargetrr = leftResolverResult as MemberResolveResult;
            bool isField = (memberTargetrr != null && memberTargetrr.Member is IField &&
                           (memberTargetrr.TargetResult is ThisResolveResult ||
                            memberTargetrr.TargetResult is LocalResolveResult)) || leftResolverResult is ThisResolveResult || leftResolverResult is LocalResolveResult || leftResolverResult is ConstantResolveResult;

            var rightMemberTargetrr = rightResolverResult as MemberResolveResult;
            bool isRightSimple = (rightMemberTargetrr != null && rightMemberTargetrr.Member is IField &&
                           (rightMemberTargetrr.TargetResult is ThisResolveResult ||
                            rightMemberTargetrr.TargetResult is LocalResolveResult)) || rightResolverResult is ThisResolveResult || rightResolverResult is LocalResolveResult || rightResolverResult is ConstantResolveResult;

            var needTempVar = needReturnValue && (!isRightSimple && !isField || assignmentExpression.Operator != AssignmentOperatorType.Assign);
            /*if (assignmentExpression.Operator == AssignmentOperatorType.Any)
            {
                needTempVar = false;
            }*/

            if (needReturnValue)
            {
                if (needTempVar)
                {
                    variable = GetTempVarName();
                    Write("(" + variable + " = ");

                    var oldValue1 = Emitter.ReplaceAwaiterByVar;
                    Emitter.ReplaceAwaiterByVar = true;
                    assignmentExpression.Right.AcceptVisitor(Emitter);

                    Emitter.ReplaceAwaiterByVar = oldValue1;
                    Write(", ");
                }
                else
                {
                    Write("(");
                }
            }

            if (assignmentExpression.Operator == AssignmentOperatorType.Divide && Emitter.Rules.Integer == IntegerRule.Managed &&
                !(Emitter.IsJavaScriptOverflowMode && !InsideOverflowContext(Emitter, assignmentExpression)) &&
                !isLong && !isLongExpected &&
                (
                    (Helpers.IsIntegerType(leftResolverResult.Type, Emitter.Resolver) &&
                    Helpers.IsIntegerType(rightResolverResult.Type, Emitter.Resolver)) ||

                    (Helpers.IsIntegerType(Emitter.Resolver.Resolver.GetExpectedType(assignmentExpression.Left), Emitter.Resolver) &&
                    Helpers.IsIntegerType(Emitter.Resolver.Resolver.GetExpectedType(assignmentExpression.Right), Emitter.Resolver))
                ))
            {
                Emitter.IsAssignment = true;
                Emitter.AssignmentType = AssignmentOperatorType.Assign;
                var oldValue1 = Emitter.ReplaceAwaiterByVar;
                Emitter.ReplaceAwaiterByVar = true;
                AcceptLeftExpression(assignmentExpression.Left, memberTargetrr);

                if (Emitter.Writers.Count == initCount)
                {
                    Write(" = ");
                }

                Emitter.ReplaceAwaiterByVar = oldValue1;
                Emitter.AssignmentType = oldAssigmentType;
                Emitter.IsAssignment = oldAssigment;

                Write(JS.Types.H5_INT + "." + JS.Funcs.Math.DIV + "(");
                assignmentExpression.Left.AcceptVisitor(Emitter);
                Write(", ");
                oldValue1 = Emitter.ReplaceAwaiterByVar;
                Emitter.ReplaceAwaiterByVar = true;

                assignmentExpression.Right.AcceptVisitor(Emitter);

                Write(")");

                Emitter.ReplaceAwaiterByVar = oldValue1;
                Emitter.AsyncExpressionHandling = asyncExpressionHandling;

                if (Emitter.Writers.Count > initCount)
                {
                    PopWriter();
                }

                if (needReturnValue && !isField)
                {
                    if (needTempVar)
                    {
                        Write(", " + variable);
                    }
                    else
                    {
                        Write(", ");
                        Emitter.IsAssignment = false;
                        assignmentExpression.Right.AcceptVisitor(Emitter);
                        Emitter.IsAssignment = oldAssigment;
                    }
                }

                if (needReturnValue)
                {
                    Write(")");
                }

                return;
            }

            if (assignmentExpression.Operator == AssignmentOperatorType.Multiply && Emitter.Rules.Integer == IntegerRule.Managed &&
                !(Emitter.IsJavaScriptOverflowMode && !InsideOverflowContext(Emitter, assignmentExpression)) &&
                !isLong && !isLongExpected &&
                (
                    (Helpers.IsInteger32Type(leftResolverResult.Type, Emitter.Resolver) &&
                    Helpers.IsInteger32Type(rightResolverResult.Type, Emitter.Resolver) &&
                    Helpers.IsInteger32Type(rr.Type, Emitter.Resolver)) ||

                    (Helpers.IsInteger32Type(Emitter.Resolver.Resolver.GetExpectedType(assignmentExpression.Left), Emitter.Resolver) &&
                    Helpers.IsInteger32Type(Emitter.Resolver.Resolver.GetExpectedType(assignmentExpression.Right), Emitter.Resolver) &&
                    Helpers.IsInteger32Type(rr.Type, Emitter.Resolver))
                ))
            {
                Emitter.IsAssignment = true;
                Emitter.AssignmentType = AssignmentOperatorType.Assign;
                var oldValue1 = Emitter.ReplaceAwaiterByVar;
                Emitter.ReplaceAwaiterByVar = true;
                AcceptLeftExpression(assignmentExpression.Left, memberTargetrr);

                if (Emitter.Writers.Count == initCount)
                {
                    Write(" = ");
                }

                Emitter.ReplaceAwaiterByVar = oldValue1;
                Emitter.AssignmentType = oldAssigmentType;
                Emitter.IsAssignment = oldAssigment;

                isUint = NullableType.GetUnderlyingType(rr.Type).IsKnownType(KnownTypeCode.UInt32);
                Write(JS.Types.H5_INT + "." + (isUint ? JS.Funcs.Math.UMUL : JS.Funcs.Math.MUL) + "(");
                assignmentExpression.Left.AcceptVisitor(Emitter);
                Write(", ");
                oldValue1 = Emitter.ReplaceAwaiterByVar;
                Emitter.ReplaceAwaiterByVar = true;

                assignmentExpression.Right.AcceptVisitor(Emitter);

                if (IsInCheckedContext(Emitter, assignmentExpression))
                {
                    Write(", 1");
                }

                Write(")");

                Emitter.ReplaceAwaiterByVar = oldValue1;
                Emitter.AsyncExpressionHandling = asyncExpressionHandling;

                if (Emitter.Writers.Count > initCount)
                {
                    PopWriter();
                }

                if (needReturnValue && !isField)
                {
                    if (needTempVar)
                    {
                        Write(", " + variable);
                    }
                    else
                    {
                        Write(", ");
                        Emitter.IsAssignment = false;
                        assignmentExpression.Right.AcceptVisitor(Emitter);
                        Emitter.IsAssignment = oldAssigment;
                    }
                }

                if (needReturnValue)
                {
                    Write(")");
                }

                return;
            }

            bool templateDelegateAssigment = false;

            if (assignmentExpression.Operator == AssignmentOperatorType.Add
                || assignmentExpression.Operator == AssignmentOperatorType.Subtract)
            {
                var add = assignmentExpression.Operator == AssignmentOperatorType.Add;

                if (Emitter.Validator.IsDelegateOrLambda(leftResolverResult))
                {
                    delegateAssigment = true;

                    if (leftResolverResult is MemberResolveResult leftMemberResolveResult)
                    {
                        isEvent = leftMemberResolveResult.Member is IEvent;
                        Emitter.IsAssignment = true;
                        Emitter.AssignmentType = assignmentExpression.Operator;
                        templateDelegateAssigment = !string.IsNullOrWhiteSpace(Emitter.GetInline(leftMemberResolveResult.Member));
                        Emitter.IsAssignment = false;
                    }

                    if (!isEvent)
                    {
                        Emitter.IsAssignment = true;
                        Emitter.AssignmentType = AssignmentOperatorType.Assign;
                        AcceptLeftExpression(assignmentExpression.Left, memberTargetrr);
                        Emitter.IsAssignment = false;

                        if (Emitter.Writers.Count == initCount)
                        {
                            Write(" = ");
                        }

                        Write(add ? JS.Funcs.H5_COMBINE : JS.Funcs.H5_REMOVE);
                        WriteOpenParentheses();
                    }
                }
            }

            bool nullable = orr != null && orr.IsLiftedOperator;
            string root = JS.Types.SYSTEM_NULLABLE + ".";

            bool special = nullable;

            Emitter.IsAssignment = true;
            Emitter.AssignmentType = assignmentExpression.Operator;
            var oldValue = Emitter.ReplaceAwaiterByVar;
            Emitter.ReplaceAwaiterByVar = true;

            bool thisAssignment = leftResolverResult is ThisResolveResult;

            if (!thisAssignment)
            {
                if (special || (isDecimal && isDecimalExpected) || (isLong && isLongExpected) || isUserOperator)
                {
                    Emitter.AssignmentType = AssignmentOperatorType.Assign;
                }

                if (delegateAssigment && !isEvent)
                {
                    Emitter.IsAssignment = false;
                }

                AcceptLeftExpression(assignmentExpression.Left, memberTargetrr);

                if (delegateAssigment)
                {
                    Emitter.IsAssignment = true;
                }
            }
            else
            {
                Write("(");
            }

            Emitter.ReplaceAwaiterByVar = oldValue;
            Emitter.AssignmentType = oldAssigmentType;
            Emitter.IsAssignment = oldAssigment;

            if (Emitter.Writers.Count == initCount && !delegateAssigment && !thisAssignment)
            {
                WriteSpace();
            }

            if (isDecimal && isDecimalExpected)
            {
                if (Emitter.Writers.Count == initCount)
                {
                    Write("= ");
                }

                oldValue = Emitter.ReplaceAwaiterByVar;
                Emitter.ReplaceAwaiterByVar = true;

                HandleDecimal(rr, variable);

                if (Emitter.Writers.Count > initCount)
                {
                    PopWriter();
                }

                if (needTempVar)
                {
                    Write(", " + variable + ")");
                }
                else if (needReturnValue)
                {
                    if (!isField)
                    {
                        Write(", ");
                        Emitter.IsAssignment = false;
                        assignmentExpression.Right.AcceptVisitor(Emitter);
                        Emitter.IsAssignment = oldAssigment;
                    }

                    Write(")");
                }

                Emitter.ReplaceAwaiterByVar = oldValue;
                return;
            }

            if (isLong && isLongExpected)
            {
                if (Emitter.Writers.Count == initCount)
                {
                    Write("= ");
                }

                oldValue = Emitter.ReplaceAwaiterByVar;
                Emitter.ReplaceAwaiterByVar = true;

                HandleLong(rr, variable, isUint);

                if (Emitter.Writers.Count > initCount)
                {
                    PopWriter();
                }

                if (needTempVar)
                {
                    Write(", " + variable + ")");
                }
                else if (needReturnValue)
                {
                    if (!isField)
                    {
                        Write(", ");
                        Emitter.IsAssignment = false;
                        assignmentExpression.Right.AcceptVisitor(Emitter);
                        Emitter.IsAssignment = oldAssigment;
                    }

                    Write(")");
                }
                Emitter.ReplaceAwaiterByVar = oldValue;
                return;
            }

            if (ResolveOperator(assignmentExpression, orr, initCount, thisAssignment))
            {
                if (thisAssignment)
                {
                    Write(")." + JS.Funcs.CLONE + "(this)");
                }
                else if (needReturnValue)
                {
                    Write(")");
                }
                return;
            }

            bool isBool = NullableType.IsNullable(rr.Type) ? NullableType.GetUnderlyingType(rr.Type).IsKnownType(KnownTypeCode.Boolean) : rr.Type.IsKnownType(KnownTypeCode.Boolean);

            if (!delegateAssigment)
            {
                if (!special)
                {
                    switch (assignmentExpression.Operator)
                    {
                        case AssignmentOperatorType.Assign:
                            break;

                        case AssignmentOperatorType.Add:
                            Write("+");
                            break;

                        case AssignmentOperatorType.BitwiseAnd:
                            if (!isBool)
                            {
                                Write("&");
                            }
                            break;

                        case AssignmentOperatorType.BitwiseOr:
                            if (!isBool)
                            {
                                Write("|");
                            }

                            break;

                        case AssignmentOperatorType.Divide:
                            Write("/");
                            break;

                        case AssignmentOperatorType.ExclusiveOr:
                            if (!isBool) {
                                Write("^");
                            }
                            break;

                        case AssignmentOperatorType.Modulus:
                            Write("%");
                            break;

                        case AssignmentOperatorType.Multiply:
                            Write("*");
                            break;

                        case AssignmentOperatorType.ShiftLeft:
                            Write("<<");
                            break;

                        case AssignmentOperatorType.ShiftRight:
                            Write(isUint ? ">>>" : ">>");
                            break;

                        case AssignmentOperatorType.Subtract:
                            Write("-");
                            break;

                        default:
                            throw new EmitterException(assignmentExpression,
                                "Unsupported assignment operator: " + assignmentExpression.Operator.ToString());
                    }
                }

                if (special)
                {
                    if (Emitter.Writers.Count == initCount)
                    {
                        Write("= ");
                    }
                    Write(root);

                    switch (assignmentExpression.Operator)
                    {
                        case AssignmentOperatorType.Assign:
                            break;

                        case AssignmentOperatorType.Add:
                            Write(JS.Funcs.Math.ADD);
                            break;

                        case AssignmentOperatorType.BitwiseAnd:
                            Write(isBool ? JS.Funcs.Math.AND : JS.Funcs.Math.BAND);
                            break;

                        case AssignmentOperatorType.BitwiseOr:
                            Write(isBool ? JS.Funcs.Math.OR : JS.Funcs.Math.BOR);
                            break;

                        case AssignmentOperatorType.Divide:
                            Write(JS.Funcs.Math.DIV);
                            break;

                        case AssignmentOperatorType.ExclusiveOr:
                            Write(JS.Funcs.Math.XOR);
                            break;

                        case AssignmentOperatorType.Modulus:
                            Write(JS.Funcs.Math.MOD);
                            break;

                        case AssignmentOperatorType.Multiply:
                            Write(JS.Funcs.Math.MUL);
                            break;

                        case AssignmentOperatorType.ShiftLeft:
                            Write(JS.Funcs.Math.SL);
                            break;

                        case AssignmentOperatorType.ShiftRight:
                            Write(isUint ? JS.Funcs.Math.SRR : JS.Funcs.Math.SR);
                            break;

                        case AssignmentOperatorType.Subtract:
                            Write(JS.Funcs.Math.SUB);
                            break;

                        default:
                            throw new EmitterException(assignmentExpression,
                                "Unsupported assignment operator: " + assignmentExpression.Operator.ToString());
                    }

                    WriteOpenParentheses();

                    assignmentExpression.Left.AcceptVisitor(Emitter);
                    Write(", ");
                }

                if (Emitter.Writers.Count == initCount && !thisAssignment && !special)
                {
                    Write("= ");
                }
            }
            else if (!isEvent)
            {
                WriteComma();
            }

            if (!special && isBool && (assignmentExpression.Operator == AssignmentOperatorType.BitwiseAnd || assignmentExpression.Operator == AssignmentOperatorType.BitwiseOr || assignmentExpression.Operator == AssignmentOperatorType.ExclusiveOr))
            {
                if (assignmentExpression.Operator != AssignmentOperatorType.ExclusiveOr)
                {
                    Write("!!(");
                }

                assignmentExpression.Left.AcceptVisitor(Emitter);

                string op = null;
                switch(assignmentExpression.Operator)
                {
                    case AssignmentOperatorType.BitwiseAnd:
                        op = " & ";
                        break;
                    case AssignmentOperatorType.BitwiseOr:
                        op = " | ";
                        break;
                    case AssignmentOperatorType.ExclusiveOr:
                        op = " != ";
                        break;
                }
                Write(op);
            }

            oldValue = Emitter.ReplaceAwaiterByVar;
            Emitter.ReplaceAwaiterByVar = true;

            if (charToString == 1)
            {
                Write(JS.Funcs.STRING_FROMCHARCODE + "(");
            }

            if (needTempVar)
            {
                int pos = Emitter.Output.Length;
                Write(variable);
                Helpers.CheckValueTypeClone(rr, assignmentExpression.Right, this, pos);
            }
            else
            {
                var wrap = assignmentExpression.Operator != AssignmentOperatorType.Assign
                    && Emitter.Writers.Count > initCount
                    && !AssigmentExpressionHelper.CheckIsRightAssigmentExpression(assignmentExpression);

                if (wrap)
                {
                    WriteOpenParentheses();
                }

                assignmentExpression.Right.AcceptVisitor(Emitter);

                if (wrap)
                {
                    WriteCloseParentheses();
                }
            }

            if (!special && isBool &&
                (assignmentExpression.Operator == AssignmentOperatorType.BitwiseAnd ||
                 assignmentExpression.Operator == AssignmentOperatorType.BitwiseOr))
            {
                WriteCloseParentheses();
            }

            if (charToString == 1)
            {
                WriteCloseParentheses();
            }

            if (special)
            {
                WriteCloseParentheses();
            }

            if (thisAssignment)
            {
                Write(")." + JS.Funcs.CLONE + "(this)");
            }

            Emitter.ReplaceAwaiterByVar = oldValue;
            Emitter.AsyncExpressionHandling = asyncExpressionHandling;

            if (Emitter.Writers.Count > initCount)
            {
                var writerCount = Emitter.Writers.Count;
                for (int i = initCount; i < writerCount; i++)
                {
                    PopWriter();
                }
            }

            if (delegateAssigment && !templateDelegateAssigment)
            {
                WriteCloseParentheses();
            }

            if (needTempVar)
            {
                Write(", " + variable + ")");
            }
            else if (needReturnValue)
            {
                if (!isField)
                {
                    Write(", ");
                    Emitter.IsAssignment = false;
                    assignmentExpression.Right.AcceptVisitor(Emitter);
                    Emitter.IsAssignment = oldAssigment;
                }

                Write(")");
            }
        }

        private void AcceptLeftExpression(Expression left, ResolveResult rr)
        {
            if (!Emitter.InConstructor || !(rr is MemberResolveResult mrr) || !(mrr.Member is IProperty) || mrr.Member.IsStatic || mrr.Member.DeclaringTypeDefinition == null || !mrr.Member.DeclaringTypeDefinition.Equals(Emitter.TypeInfo.Type))
            {
                left.AcceptVisitor(Emitter);
            }
            else
            {
                var property = (IProperty)mrr.Member;
                var proto = mrr.IsVirtualCall || property.IsVirtual || property.IsOverride;

                var td = Emitter.GetTypeDefinition();
                var prop = td.Properties.FirstOrDefault(p => p.Name == mrr.Member.Name);

                if (proto && prop != null && prop.SetMethod == null)
                {
                    var name = OverloadsCollection.Create(Emitter, mrr.Member).GetOverloadName();
                    Write(JS.Types.H5.ENSURE_BASE_PROPERTY + "(this, \"" + name + "\"");

                    if (Emitter.Validator.IsExternalType(property.DeclaringTypeDefinition) && !Emitter.Validator.IsH5Class(property.DeclaringTypeDefinition))
                    {
                        Write(", \"" + H5Types.ToJsName(property.DeclaringType, Emitter, isAlias: true) + "\"");
                    }

                    Write(")");

                    WriteDot();
                    var alias = H5Types.ToJsName(mrr.Member.DeclaringType, Emitter, isAlias: true);
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
                    left.AcceptVisitor(Emitter);
                }
            }
        }

        private void HandleType(ResolveResult resolveOperator, string variable, string op_name, KnownTypeCode typeCode)
        {
            if (AssignmentExpression.Operator == AssignmentOperatorType.Assign)
            {
                if (variable != null)
                {
                    Write(variable);
                }
                else
                {
                    new ExpressionListBlock(Emitter, new Expression[] { AssignmentExpression.Right }, null, null, 0).Emit();
                }

                return;
            }

            var orr = resolveOperator as OperatorResolveResult;
            var method = orr?.UserDefinedOperatorMethod;
            var assigmentType = Helpers.TypeOfAssignment(AssignmentExpression.Operator);
            if (orr != null && method == null)
            {
                var name = Helpers.GetBinaryOperatorMethodName(assigmentType);
                var type = NullableType.IsNullable(orr.Type) ? NullableType.GetUnderlyingType(orr.Type) : orr.Type;
                method = type.GetMethods(m => m.Name == name, GetMemberOptions.IgnoreInheritedMembers).FirstOrDefault();
            }

            if (method != null)
            {
                var inline = Emitter.GetInline(method);

                if (orr.IsLiftedOperator)
                {
                    Write(JS.Types.SYSTEM_NULLABLE + ".");
                    string action = JS.Funcs.Math.LIFT2;

                    Write(action);
                    WriteOpenParentheses();
                    WriteScript(op_name);
                    WriteComma();
                    if (variable != null)
                    {
                        new ExpressionListBlock(Emitter, new Expression[] { AssignmentExpression.Left }, null, null, 0).Emit();
                    }
                    else
                    {
                        new ExpressionListBlock(Emitter, new Expression[] { AssignmentExpression.Left, AssignmentExpression.Right }, null, null, 0).Emit();
                    }
                    AddOveflowFlag(typeCode, op_name);
                    WriteCloseParentheses();
                }
                else if (!string.IsNullOrWhiteSpace(inline))
                {
                    new InlineArgumentsBlock(Emitter,
                        new ArgumentsInfo(Emitter, AssignmentExpression, orr, method), inline).Emit();
                }
                else if (!Emitter.Validator.IsExternalType(method.DeclaringTypeDefinition))
                {
                    Write(H5Types.ToJsName(method.DeclaringType, Emitter));
                    WriteDot();

                    Write(OverloadsCollection.Create(Emitter, method).GetOverloadName());

                    WriteOpenParentheses();

                    if (variable != null)
                    {
                        new ExpressionListBlock(Emitter, new Expression[] { AssignmentExpression.Left }, null, null, 0).Emit();
                        Write(", " + variable);
                    }
                    else
                    {
                        new ExpressionListBlock(Emitter, new Expression[] { AssignmentExpression.Left, AssignmentExpression.Right }, null, null, 0).Emit();
                    }

                    WriteCloseParentheses();
                }
            }
            else
            {
                if (orr.IsLiftedOperator)
                {
                    Write(JS.Types.SYSTEM_NULLABLE + ".");
                    string action = JS.Funcs.Math.LIFT2;

                    Write(action);
                    WriteOpenParentheses();
                    WriteScript(op_name);
                    WriteComma();
                    if (variable != null)
                    {
                        new ExpressionListBlock(Emitter, new Expression[] { AssignmentExpression.Left }, null, null, 0).Emit();
                    }
                    else
                    {
                        new ExpressionListBlock(Emitter, new Expression[] { AssignmentExpression.Left, AssignmentExpression.Right }, null, null, 0).Emit();
                    }
                    AddOveflowFlag(typeCode, op_name);
                    WriteCloseParentheses();
                }
                else
                {
                    AssignmentExpression.Left.AcceptVisitor(Emitter);
                    WriteDot();
                    Write(op_name);
                    WriteOpenParentheses();
                    AssignmentExpression.Right.AcceptVisitor(Emitter);
                    AddOveflowFlag(typeCode, op_name);
                    WriteCloseParentheses();
                }
            }
        }

        private void AddOveflowFlag(KnownTypeCode typeCode, string op_name)
        {
            if ((typeCode == KnownTypeCode.Int64 || typeCode == KnownTypeCode.UInt64) && IsInCheckedContext(Emitter, AssignmentExpression))
            {
                if (op_name == JS.Funcs.Math.ADD || op_name == JS.Funcs.Math.SUB || op_name == JS.Funcs.Math.MUL)
                {
                    Write(", 1");
                }
            }
        }

        private void HandleDecimal(ResolveResult resolveOperator, string variable)
        {
            var assigmentType = Helpers.TypeOfAssignment(AssignmentExpression.Operator);

            string op_name = null;

            if (AssignmentExpression.Operator != AssignmentOperatorType.Assign)
            {
                switch (assigmentType)
                {
                    case BinaryOperatorType.GreaterThan:
                        op_name = JS.Funcs.Math.GT;
                        break;

                    case BinaryOperatorType.GreaterThanOrEqual:
                        op_name = JS.Funcs.Math.GTE;
                        break;

                    case BinaryOperatorType.Equality:
                        op_name = JS.Funcs.Math.EQUALS;
                        break;

                    case BinaryOperatorType.InEquality:
                        op_name = JS.Funcs.Math.NE;
                        break;

                    case BinaryOperatorType.LessThan:
                        op_name = JS.Funcs.Math.LT;
                        break;

                    case BinaryOperatorType.LessThanOrEqual:
                        op_name = JS.Funcs.Math.LTE;
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
            }
            HandleType(resolveOperator, variable, op_name, KnownTypeCode.Decimal);
        }

        private void HandleLong(ResolveResult resolveOperator, string variable, bool isUnsigned)
        {
            var assigmentType = Helpers.TypeOfAssignment(AssignmentExpression.Operator);

            string op_name = null;
            if (AssignmentExpression.Operator != AssignmentOperatorType.Assign)
            {
                switch (assigmentType)
                {
                    case BinaryOperatorType.GreaterThan:
                        op_name = JS.Funcs.Math.GT;
                        break;

                    case BinaryOperatorType.GreaterThanOrEqual:
                        op_name = JS.Funcs.Math.GTE;
                        break;

                    case BinaryOperatorType.Equality:
                        op_name = JS.Funcs.Math.EQUALS;
                        break;

                    case BinaryOperatorType.InEquality:
                        op_name = JS.Funcs.Math.NE;
                        break;

                    case BinaryOperatorType.LessThan:
                        op_name = JS.Funcs.Math.LT;
                        break;

                    case BinaryOperatorType.LessThanOrEqual:
                        op_name = JS.Funcs.Math.LTE;
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
                        op_name = isUnsigned ? JS.Funcs.Math.SHRU : JS.Funcs.Math.SHR;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            HandleType(resolveOperator, variable, op_name, isUnsigned ? KnownTypeCode.UInt64 : KnownTypeCode.Int64);
        }
    }
}