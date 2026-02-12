using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using Object.Net.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.Semantics;

namespace H5.Translator
{
    public class TryCatchBlock : AbstractEmitterBlock
    {
        public TryCatchBlock(IEmitter emitter, TryCatchStatement tryCatchStatement)
            : base(emitter, tryCatchStatement)
        {
            Emitter = emitter;
            TryCatchStatement = tryCatchStatement;
        }

        public TryCatchStatement TryCatchStatement { get; set; }

        protected override void DoEmit()
        {
            var awaiters = Emitter.IsAsync ? GetAwaiters(TryCatchStatement) : null;

            if (awaiters != null && awaiters.Length > 0)
            {
                VisitAsyncTryCatchStatement();
            }
            else
            {
                VisitTryCatchStatement();
            }
        }

        protected void VisitAsyncTryCatchStatement()
        {
            TryCatchStatement tryCatchStatement = TryCatchStatement;

            Emitter.AsyncBlock.Steps.Last().JumpToStep = Emitter.AsyncBlock.Step;

            var tryStep = Emitter.AsyncBlock.AddAsyncStep();
            AsyncTryInfo tryInfo = new AsyncTryInfo();
            tryInfo.StartStep = tryStep.Step;

            Emitter.IgnoreBlock = tryCatchStatement.TryBlock;
            tryCatchStatement.TryBlock.AcceptVisitor(Emitter);
            tryStep = Emitter.AsyncBlock.Steps.Last();
            tryInfo.EndStep = tryStep.Step;

            List<IAsyncStep> catchSteps = new List<IAsyncStep>();

            foreach (var clause in tryCatchStatement.CatchClauses)
            {
                var catchStep = Emitter.AsyncBlock.AddAsyncStep();

                PushLocals();
                var varName = clause.VariableName;

                if (!String.IsNullOrEmpty(varName) && !Emitter.Locals.ContainsKey(varName))
                {
                    varName = AddLocal(varName, clause.VariableNameToken, clause.Type);
                }

                Emitter.IgnoreBlock = clause.Body;
                clause.Body.AcceptVisitor(Emitter);
                Write(JS.Vars.ASYNC_E + " = null;");
                PopLocals();
                WriteNewLine();

                tryInfo.CatchBlocks.Add(new Tuple<string, string, int, int>(varName, clause.Type.IsNull ? JS.Types.System.Exception.NAME : H5Types.ToJsName(clause.Type, Emitter), catchStep.Step, Emitter.AsyncBlock.Steps.Last().Step));
                catchSteps.Add(Emitter.AsyncBlock.Steps.Last());
            }

            if (!Emitter.Locals.ContainsKey(JS.Vars.ASYNC_E))
            {
                AddLocal(JS.Vars.ASYNC_E, null, AstType.Null);
            }

            IAsyncStep finalyStep = null;
            if (!tryCatchStatement.FinallyBlock.IsNull)
            {
                finalyStep = Emitter.AsyncBlock.AddAsyncStep(tryCatchStatement.FinallyBlock);
                Emitter.IgnoreBlock = tryCatchStatement.FinallyBlock;
                tryCatchStatement.FinallyBlock.AcceptVisitor(Emitter);

                var finallyNode = GetParentFinallyBlock(tryCatchStatement, false);

                WriteNewLine();

                WriteIf();
                WriteOpenParentheses();
                Write(JS.Vars.ASYNC_JUMP + " > -1");
                WriteCloseParentheses();
                WriteSpace();
                BeginBlock();
                if (finallyNode != null)
                {
                    var hashcode = finallyNode.GetHashCode();
                    Emitter.AsyncBlock.JumpLabels.Add(new AsyncJumpLabel
                    {
                        Node = finallyNode,
                        Output = Emitter.Output
                    });
                    Write(JS.Vars.ASYNC_STEP + " = " + Helpers.PrefixDollar("{", hashcode, "};"));
                    WriteNewLine();
                    Write("continue;");
                }
                else
                {
                    Write(JS.Vars.ASYNC_STEP + " = " + JS.Vars.ASYNC_JUMP + ";");
                    WriteNewLine();
                    Write(JS.Vars.ASYNC_JUMP + " = null;");
                }

                WriteNewLine();
                EndBlock();

                WriteSpace();
                WriteElse();
                WriteIf();
                WriteOpenParentheses();
                Write(JS.Vars.ASYNC_E);
                WriteCloseParentheses();
                WriteSpace();
                BeginBlock();

                if (Emitter.AsyncBlock.IsTaskReturn)
                {
                    Write(JS.Vars.ASYNC_TCS + "." + ((Emitter.AssemblyInfo.Rules.UseShortForms ?? false) ? JS.Funcs.SHORTEN_SET_EXCEPTION : JS.Funcs.SET_EXCEPTION) + "(" + JS.Vars.ASYNC_E + ");");
                }
                else
                {
                    Write("throw " + JS.Vars.ASYNC_E + ";");
                }

                WriteNewLine();
                WriteReturn(false);
                WriteSemiColon();
                WriteNewLine();
                EndBlock();

                WriteSpace();
                WriteElse();
                WriteIf();
                WriteOpenParentheses();
                Write(JS.Funcs.H5_IS_DEFINED);
                WriteOpenParentheses();
                Write(JS.Vars.ASYNC_RETURN_VALUE);
                WriteCloseParentheses();
                WriteCloseParentheses();
                WriteSpace();
                BeginBlock();

                if (finallyNode != null)
                {
                    var hashcode = finallyNode.GetHashCode();
                    Emitter.AsyncBlock.JumpLabels.Add(new AsyncJumpLabel
                    {
                        Node = finallyNode,
                        Output = Emitter.Output
                    });
                    Write(JS.Vars.ASYNC_STEP + " = " + Helpers.PrefixDollar("{", hashcode, "};"));
                    WriteNewLine();
                    Write("continue;");
                }
                else
                {
                    Write(JS.Vars.ASYNC_TCS + "." + ((Emitter.AssemblyInfo.Rules.UseShortForms ?? false) ? JS.Funcs.SHORTEN_SET_RESULT : JS.Funcs.SET_RESULT) + "(" + JS.Vars.ASYNC_RETURN_VALUE + ");");
                    WriteNewLine();
                    WriteReturn(false);
                    WriteSemiColon();
                }

                WriteNewLine();
                EndBlock();

                if (!Emitter.Locals.ContainsKey(JS.Vars.ASYNC_E))
                {
                    AddLocal(JS.Vars.ASYNC_E, null, AstType.Null);
                }
            }

            var lastFinallyStep = Emitter.AsyncBlock.Steps.Last();

            var nextStep = Emitter.AsyncBlock.AddAsyncStep();
            if (finalyStep != null)
            {
                tryInfo.FinallyStep = finalyStep.Step;
                lastFinallyStep.JumpToStep = nextStep.Step;
            }

            tryStep.JumpToStep = finalyStep != null ? finalyStep.Step : nextStep.Step;

            foreach (var step in catchSteps)
            {
                step.JumpToStep = finalyStep != null ? finalyStep.Step : nextStep.Step;
            }

            Emitter.AsyncBlock.TryInfos.Add(tryInfo);
        }

        protected void VisitTryCatchStatement()
        {
            EmitTryBlock();

            var count = TryCatchStatement.CatchClauses.Count;

            if (count > 0)
            {
                var firstClause = TryCatchStatement.CatchClauses.Count == 1 ? TryCatchStatement.CatchClauses.First() : null;
                var exceptionType = (firstClause == null || firstClause.Type.IsNull) ? null : H5Types.ToJsName(firstClause.Type, Emitter);
                var isBaseException = exceptionType == null || exceptionType == JS.Types.System.Exception.NAME;

                if (count == 1 && isBaseException)
                {
                    EmitSingleCatchBlock();
                }
                else
                {
                    EmitMultipleCatchBlock();
                }
            }

            EmitFinallyBlock();
        }

        protected virtual void EmitTryBlock()
        {
            TryCatchStatement tryCatchStatement = TryCatchStatement;

            WriteTry();

            tryCatchStatement.TryBlock.AcceptVisitor(Emitter);
        }

        protected virtual void EmitFinallyBlock()
        {
            TryCatchStatement tryCatchStatement = TryCatchStatement;

            if (!tryCatchStatement.FinallyBlock.IsNull)
            {
                WriteSpace();
                WriteFinally();
                tryCatchStatement.FinallyBlock.AcceptVisitor(Emitter);
            }
        }

        protected virtual void EmitSingleCatchBlock()
        {
            TryCatchStatement tryCatchStatement = TryCatchStatement;

            foreach (var clause in tryCatchStatement.CatchClauses)
            {
                PushLocals();

                var varName = clause.VariableName;

                if (String.IsNullOrEmpty(varName))
                {
                    varName = AddLocal(GetUniqueName(JS.Vars.E), null, AstType.Null);
                }
                else
                {
                    varName = AddLocal(varName, clause.VariableNameToken, clause.Type);
                }

                var oldVar = Emitter.CatchBlockVariable;
                Emitter.CatchBlockVariable = varName;

                WriteSpace();
                WriteCatch();
                WriteOpenParentheses();
                Write(varName);
                WriteCloseParentheses();
                WriteSpace();

                BeginBlock();
                Write(string.Format("{0} = " + JS.Types.System.Exception.CREATE + "({0});", varName));

                WriteNewLine();
                Emitter.NoBraceBlock = clause.Body;
                clause.Body.AcceptVisitor(Emitter);
                if (!Emitter.IsNewLine)
                {
                    WriteNewLine();
                }

                EndBlock();

                if (tryCatchStatement.FinallyBlock.IsNull)
                {
                    WriteNewLine();
                }

                PopLocals();
                Emitter.CatchBlockVariable = oldVar;
            }
        }

        protected virtual void EmitMultipleCatchBlock()
        {
            TryCatchStatement tryCatchStatement = TryCatchStatement;

            WriteSpace();
            WriteCatch();
            WriteOpenParentheses();
            var varName = AddLocal(GetUniqueName(JS.Vars.E), null, AstType.Null);

            var oldVar = Emitter.CatchBlockVariable;
            Emitter.CatchBlockVariable = varName;

            Write(varName);
            WriteCloseParentheses();
            WriteSpace();
            BeginBlock();
            Write(string.Format("{0} = " + JS.Types.System.Exception.CREATE + "({0});", varName));
            WriteNewLine();

            var catchVars = new Dictionary<string, string>();
            var writeVar = false;

            foreach (var clause in tryCatchStatement.CatchClauses)
            {
                if (clause.VariableName.IsNotEmpty() && !catchVars.ContainsKey(clause.VariableName))
                {
                    if (!writeVar)
                    {
                        writeVar = true;
                        WriteVar(true);
                        Emitter.Comma = false;
                    }

                    EnsureComma(false);
                    catchVars.Add(clause.VariableName, clause.VariableName);
                    Write(clause.VariableName);
                    Emitter.Comma = true;
                }
            }

            Emitter.Comma = false;
            if (writeVar)
            {
                WriteSemiColon(true);
            }

            var firstClause = true;
            var writeElse = true;
            var needNewLine = false;

            foreach (var clause in tryCatchStatement.CatchClauses)
            {
                var exceptionType = clause.Type.IsNull ? null : H5Types.ToJsName(clause.Type, Emitter);
                var isBaseException = exceptionType == null || exceptionType == JS.Types.System.Exception.NAME;

                if (!firstClause)
                {
                    WriteSpace();
                    WriteElse();
                }

                if (isBaseException)
                {
                    writeElse = false;
                }
                else
                {
                    WriteIf();
                    WriteOpenParentheses();
                    Write(string.Format(JS.Types.H5.IS + "({0}, {1})", varName, exceptionType));
                    WriteCloseParentheses();
                    WriteSpace();
                }

                firstClause = false;

                PushLocals();
                BeginBlock();

                if (clause.VariableName.IsNotEmpty())
                {
                    Write(clause.VariableName + " = " + varName);
                    WriteSemiColon();
                    WriteNewLine();
                }

                Emitter.NoBraceBlock = clause.Body;
                clause.Body.AcceptVisitor(Emitter);
                Emitter.NoBraceBlock = null;
                EndBlock();

                needNewLine = true;

                PopLocals();
            }

            if (writeElse)
            {
                WriteSpace();
                WriteElse();
                BeginBlock();
                Write("throw " + varName);
                WriteSemiColon();
                WriteNewLine();
                EndBlock();
                needNewLine = true;
            }

            if (needNewLine)
            {
                WriteNewLine();
                needNewLine = false;
            }

            EndBlock();
            if (tryCatchStatement.FinallyBlock.IsNull)
            {
                WriteNewLine();
            }
            Emitter.CatchBlockVariable = oldVar;
        }
    }

    public class AsyncTryInfo : IAsyncTryInfo
    {
        public int StartStep { get; set; }

        public int EndStep { get; set; }

        public int FinallyStep { get; set; }

        private List<Tuple<string, string, int, int>> catchBlocks;

        public List<Tuple<string, string, int, int>> CatchBlocks
        {
            get
            {
                if (catchBlocks == null)
                {
                    catchBlocks = new List<Tuple<string, string, int, int>>();
                }
                return catchBlocks;
            }
        }
    }
}