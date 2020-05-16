using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using EmptyStatement = ICSharpCode.NRefactory.CSharp.EmptyStatement;

namespace H5.Translator
{
    public class ForeachBlock : AbstractEmitterBlock
    {
        public ForeachBlock(IEmitter emitter, ForeachStatement foreachStatement)
            : base(emitter, foreachStatement)
        {
            Emitter = emitter;
            ForeachStatement = foreachStatement;
        }

        public ForeachStatement ForeachStatement { get; set; }

        public IMethod CastMethod
        {
            get; set;
        }

        protected override void DoEmit()
        {
            var awaiters = Emitter.IsAsync ? GetAwaiters(ForeachStatement) : null;

            if (awaiters != null && awaiters.Length > 0)
            {
                VisitAsyncForeachStatement();
            }
            else
            {
                VisitForeachStatement();
            }
        }

        protected virtual string GetNextIteratorName()
        {
            var index = Emitter.IteratorCount++;
            var result = JS.Vars.ITERATOR;

            if (index > 0)
            {
                result += index;
            }

            return result;
        }

        protected void VisitAsyncForeachStatement()
        {
            ForeachStatement foreachStatement = ForeachStatement;

            if (foreachStatement.EmbeddedStatement is EmptyStatement)
            {
                return;
            }

            var oldValue = Emitter.ReplaceAwaiterByVar;
            var jumpStatements = Emitter.JumpStatements;
            Emitter.JumpStatements = new List<IJumpInfo>();
            WriteAwaiters(foreachStatement.InExpression);

            bool containsAwaits = false;
            var awaiters = GetAwaiters(foreachStatement.EmbeddedStatement);

            if (awaiters != null && awaiters.Length > 0)
            {
                containsAwaits = true;
            }

            Emitter.ReplaceAwaiterByVar = true;

            if (!containsAwaits)
            {
                VisitForeachStatement(oldValue);
                return;
            }

            var iteratorName = AddLocal(GetTempVarName(), null, AstType.Null);

            var for_rr = (ForEachResolveResult)Emitter.Resolver.ResolveNode(foreachStatement, Emitter);
            var get_rr = for_rr.GetEnumeratorCall as InvocationResolveResult;
            var in_rr = Emitter.Resolver.ResolveNode(foreachStatement.InExpression, Emitter);
            var inline = get_rr != null ? Emitter.GetInline(get_rr.Member) : null;
            var checkEnum = in_rr.Type.Kind != TypeKind.Array && !in_rr.Type.IsKnownType(KnownTypeCode.String) &&
                           !in_rr.Type.IsKnownType(KnownTypeCode.Array);
            var isGenericEnumerable = for_rr.CollectionType.IsParameterized &&
                                      for_rr.CollectionType.FullName == "System.Collections.Generic.IEnumerable";
            var emitInline = checkEnum && !isGenericEnumerable && inline != null;

            Write(iteratorName, " = ");

            if (!emitInline)
            {
                Write(JS.Funcs.H5_GET_ENUMERATOR);
                WriteOpenParentheses();
                foreachStatement.InExpression.AcceptVisitor(Emitter);
            }

            if (checkEnum)
            {
                if (for_rr.CollectionType.IsParameterized &&
                for_rr.CollectionType.FullName == "System.Collections.Generic.IEnumerable")
                {
                    WriteComma(false);
                    Write(H5Types.ToJsName(((ParameterizedType)for_rr.CollectionType).TypeArguments[0], Emitter));
                }
                else if (get_rr != null)
                {
                    if (inline != null)
                    {
                        var argsInfo = new ArgumentsInfo(Emitter, foreachStatement.InExpression, get_rr);
                        new InlineArgumentsBlock(Emitter, argsInfo, inline).Emit();
                    }
                    else
                    {
                        var name = OverloadsCollection.Create(Emitter, get_rr.Member).GetOverloadName();

                        if (name != "GetEnumerator" && name != "System$Collections$IEnumerable$GetEnumerator")
                        {
                            WriteComma(false);
                            WriteScript(name);
                        }
                    }
                }
            }

            Emitter.ReplaceAwaiterByVar = oldValue;
            if (!emitInline)
            {
                WriteCloseParentheses();
            }
            WriteSemiColon();
            WriteNewLine();
            Write(JS.Vars.ASYNC_STEP + " = " + Emitter.AsyncBlock.Step + ";");
            WriteNewLine();
            Write("continue;");
            WriteNewLine();

            IAsyncStep conditionStep = Emitter.AsyncBlock.AddAsyncStep();

            WriteIf();
            WriteOpenParentheses();
            Write(iteratorName);
            WriteDot();
            Write(JS.Funcs.MOVE_NEXT);
            WriteOpenCloseParentheses();
            WriteCloseParentheses();
            WriteSpace();
            BeginBlock();

            PushLocals();

            var varName = AddLocal(foreachStatement.VariableName, foreachStatement.VariableNameToken, foreachStatement.VariableType);

            WriteVar();
            Write(varName + " = ");

            var rr = Emitter.Resolver.ResolveNode(foreachStatement, Emitter) as ForEachResolveResult;

            bool isReferenceLocal = false;

            if (Emitter.LocalsMap != null && Emitter.LocalsMap.ContainsKey(rr.ElementVariable))
            {
                isReferenceLocal = Emitter.LocalsMap[rr.ElementVariable].EndsWith(".v");
            }

            if (isReferenceLocal)
            {
                Write("{ v : ");
            }

            string castCode = GetCastCode(rr.ElementType, rr.ElementVariable.Type);

            if (castCode != null)
            {
                EmitInlineCast(iteratorName + "." + JS.Funcs.GET_CURRENT, castCode);
            }
            else if (CastMethod != null)
            {
                Write(H5Types.ToJsName(CastMethod.DeclaringType, Emitter));
                WriteDot();
                Write(OverloadsCollection.Create(Emitter, CastMethod).GetOverloadName());
                WriteOpenParentheses();
                var pos = Emitter.Output.Length;
                Write(iteratorName + "." + JS.Funcs.GET_CURRENT);
                Helpers.CheckValueTypeClone(rr, ForeachStatement.InExpression, this, pos);
                WriteCloseParentheses();
            }
            else
            {
                var needCast = !rr.ElementType.Equals(rr.ElementVariable.Type);
                if (needCast)
                {
                    Write(JS.Funcs.H5_CAST);
                    WriteOpenParentheses();
                }

                var pos = Emitter.Output.Length;
                Write(iteratorName);

                WriteDot();
                Write(JS.Funcs.GET_CURRENT);
                Helpers.CheckValueTypeClone(rr, ForeachStatement.InExpression, this, pos);

                if (needCast)
                {
                    Write(", ", H5Types.ToJsName(rr.ElementVariable.Type, Emitter), ")");
                }
            }

            if (isReferenceLocal)
            {
                Write(" }");
            }

            WriteSemiColon();
            WriteNewLine();

            Write(JS.Vars.ASYNC_STEP + " = " + Emitter.AsyncBlock.Step + ";");
            WriteNewLine();
            Write("continue;");


            var writer = SaveWriter();
            Emitter.AsyncBlock.AddAsyncStep();
            Emitter.IgnoreBlock = foreachStatement.EmbeddedStatement;
            var startCount = Emitter.AsyncBlock.Steps.Count;

            if (foreachStatement.EmbeddedStatement is BlockStatement block)
            {
                block.AcceptChildren(Emitter);
            }
            else
            {
                foreachStatement.EmbeddedStatement.AcceptVisitor(Emitter);
            }

            IAsyncStep loopStep = null;

            if (Emitter.AsyncBlock.Steps.Count > startCount)
            {
                loopStep = Emitter.AsyncBlock.Steps.Last();
                loopStep.JumpToStep = conditionStep.Step;
            }

            RestoreWriter(writer);

            if (!AbstractEmitterBlock.IsJumpStatementLast(Emitter.Output.ToString()))
            {
                Write(JS.Vars.ASYNC_STEP + " = " + conditionStep.Step + ";");
                WriteNewLine();
                Write("continue;");
                WriteNewLine();
            }

            PopLocals();

            WriteNewLine();
            EndBlock();
            WriteNewLine();

            var nextStep = Emitter.AsyncBlock.AddAsyncStep();
            conditionStep.JumpToStep = nextStep.Step;

            if (Emitter.JumpStatements.Count > 0)
            {
                Emitter.JumpStatements.Sort((j1, j2) => -j1.Position.CompareTo(j2.Position));
                foreach (var jump in Emitter.JumpStatements)
                {
                    jump.Output.Insert(jump.Position, jump.Break ? nextStep.Step : conditionStep.Step);
                }
            }

            Emitter.JumpStatements = jumpStatements;
        }

        protected void VisitForeachStatement(bool? replaceAwaiterByVar = null)
        {
            ForeachStatement foreachStatement = ForeachStatement;
            var jumpStatements = Emitter.JumpStatements;
            Emitter.JumpStatements = null;

            if (foreachStatement.EmbeddedStatement is EmptyStatement)
            {
                return;
            }

            WriteSourceMapName(foreachStatement.VariableName);
            var iteratorVar = GetTempVarName();
            var iteratorName = AddLocal(iteratorVar, null, AstType.Null);

            var rr = (ForEachResolveResult)Emitter.Resolver.ResolveNode(foreachStatement, Emitter);
            var get_rr = rr.GetEnumeratorCall as InvocationResolveResult;
            var in_rr = Emitter.Resolver.ResolveNode(foreachStatement.InExpression, Emitter);
            var inline = get_rr != null ? Emitter.GetInline(get_rr.Member) : null;
            var checkEnum = in_rr.Type.Kind != TypeKind.Array && !in_rr.Type.IsKnownType(KnownTypeCode.String) &&
                           !in_rr.Type.IsKnownType(KnownTypeCode.Array);
            var isGenericEnumerable = rr.CollectionType.IsParameterized &&
                                      rr.CollectionType.FullName == "System.Collections.Generic.IEnumerable";
            var emitInline = checkEnum && !isGenericEnumerable && inline != null;

            Write(iteratorName, " = ");

            if (!emitInline)
            {
                Write(JS.Funcs.H5_GET_ENUMERATOR);
                WriteOpenParentheses();
                foreachStatement.InExpression.AcceptVisitor(Emitter);
            }

            if (checkEnum)
            {
                if (isGenericEnumerable)
                {
                    WriteComma(false);
                    Write(H5Types.ToJsName(((ParameterizedType)rr.CollectionType).TypeArguments[0], Emitter));
                }
                else if (get_rr != null)
                {
                    if (inline != null)
                    {
                        var argsInfo = new ArgumentsInfo(Emitter, foreachStatement.InExpression, get_rr);
                        new InlineArgumentsBlock(Emitter, argsInfo, inline).Emit();
                    }
                    else
                    {
                        var name = OverloadsCollection.Create(Emitter, get_rr.Member).GetOverloadName();

                        if (name != "GetEnumerator" && name != "System$Collections$IEnumerable$GetEnumerator")
                        {
                            WriteComma(false);
                            WriteScript(name);
                        }
                    }
                }
            }

            if (!emitInline)
            {
                WriteCloseParentheses();
            }
            WriteSemiColon();
            WriteNewLine();

            WriteTry();
            BeginBlock();
            WriteWhile();
            WriteOpenParentheses();
            Write(iteratorName);
            WriteDot();
            Write(JS.Funcs.MOVE_NEXT);
            WriteOpenCloseParentheses();
            WriteCloseParentheses();
            WriteSpace();
            BeginBlock();

            PushLocals();
            Action ac = () =>
            {
                bool isReferenceLocal = false;

                if (Emitter.LocalsMap != null && Emitter.LocalsMap.ContainsKey(rr.ElementVariable))
                {
                    isReferenceLocal = Emitter.LocalsMap[rr.ElementVariable].EndsWith(".v");
                }

                var varName = AddLocal(foreachStatement.VariableName, foreachStatement.VariableNameToken, foreachStatement.VariableType);

                WriteVar();
                Write(varName + " = ");

                if (isReferenceLocal)
                {
                    Write("{ v : ");
                }

                string castCode = GetCastCode(rr.ElementType, rr.ElementVariable.Type);

                if (castCode != null)
                {
                    EmitInlineCast(iteratorName + "." + JS.Funcs.GET_CURRENT, castCode);
                }
                else if (CastMethod != null)
                {
                    Write(H5Types.ToJsName(CastMethod.DeclaringType, Emitter));
                    WriteDot();
                    Write(OverloadsCollection.Create(Emitter, CastMethod).GetOverloadName());
                    WriteOpenParentheses();
                    int pos = Emitter.Output.Length;
                    Write(iteratorName + "." + JS.Funcs.GET_CURRENT);
                    Helpers.CheckValueTypeClone(rr, ForeachStatement.InExpression, this, pos);
                    WriteCloseParentheses();
                }
                else
                {
                    var needCast = !rr.ElementType.Equals(rr.ElementVariable.Type);
                    if (needCast)
                    {
                        Write(JS.Funcs.H5_CAST);
                        WriteOpenParentheses();
                    }
                    int pos = Emitter.Output.Length;
                    Write(iteratorName);

                    WriteDot();
                    Write(JS.Funcs.GET_CURRENT);

                    Helpers.CheckValueTypeClone(rr, ForeachStatement.InExpression, this, pos);

                    if (needCast)
                    {
                        Write(", ", H5Types.ToJsName(rr.ElementVariable.Type, Emitter), ")");
                    }
                }

                if (isReferenceLocal)
                {
                    Write(" }");
                }

                WriteSemiColon();
                WriteNewLine();
            };
            Emitter.BeforeBlock = ac;


            if (replaceAwaiterByVar.HasValue)
            {
                Emitter.ReplaceAwaiterByVar = replaceAwaiterByVar.Value;
            }

            if (foreachStatement.EmbeddedStatement is BlockStatement block)
            {
                Emitter.NoBraceBlock = block;
            }

            foreachStatement.EmbeddedStatement.AcceptVisitor(Emitter);

            PopLocals();

            if (!Emitter.IsNewLine)
            {
                WriteNewLine();
            }

            EndBlock();
            WriteNewLine();

            EndBlock();
            WriteSpace();
            WriteFinally();
            BeginBlock();
            Write($"if ({JS.Types.H5.IS}({iteratorName}, {JS.Types.System.IDisposable.NAME})) ");
            BeginBlock();
            Write($"{iteratorName}.{JS.Types.System.IDisposable.INTERFACE_DISPOSE}();");
            WriteNewLine();
            EndBlock();
            WriteNewLine();
            EndBlock();
            WriteNewLine();
            Emitter.JumpStatements = jumpStatements;
        }

        protected virtual string GetCastCode(IType fromType, IType toType)
        {
            string inline = null;

            var method = GetCastMethod(fromType, toType, out inline);

            if (method == null && (NullableType.IsNullable(fromType) || NullableType.IsNullable(toType)))
            {
                method = GetCastMethod(NullableType.IsNullable(fromType) ? NullableType.GetUnderlyingType(fromType) : fromType,
                                            NullableType.IsNullable(toType) ? NullableType.GetUnderlyingType(toType) : toType, out inline);
            }

            CastMethod = method;
            return inline;
        }

        protected virtual void EmitInlineCast(string thisExpr, string castCode)
        {
            Write("");
            string name;

            if (CastMethod == null)
            {
                name = "{this}";
            }
            else
            {
                name = "{" + CastMethod.Parameters[0].Name + "}";
                if (!castCode.Contains(name))
                {
                    name = "{this}";
                }
            }

            if (castCode.Contains(name))
            {
                castCode = castCode.Replace(name, thisExpr);
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
                    inline = Emitter.GetInline(m);
                    return true;
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
                        inline = Emitter.GetInline(m);
                        return true;
                    }

                    return false;
                });
            }

            template = inline;
            return method;
        }
    }
}