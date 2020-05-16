using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace H5.Translator
{
    public class GeneratorBlock : AbstractEmitterBlock, IAsyncBlock
    {
        public GeneratorBlock(IEmitter emitter, MethodDeclaration methodDeclaration)
            : base(emitter, methodDeclaration)
        {
            Emitter = emitter;
            MethodDeclaration = methodDeclaration;
        }

        public GeneratorBlock(IEmitter emitter, LambdaExpression lambdaExpression)
            : base(emitter, lambdaExpression)
        {

            Emitter = emitter;
            LambdaExpression = lambdaExpression;
        }

        public GeneratorBlock(IEmitter emitter, AnonymousMethodExpression anonymousMethodExpression)
            : base(emitter, anonymousMethodExpression)
        {
            Emitter = emitter;
            AnonymousMethodExpression = anonymousMethodExpression;
        }

        public GeneratorBlock(IEmitter emitter, Accessor accessor)
             : base(emitter, accessor)
         {
             Emitter = emitter;
             Accessor = accessor;
         }

        public AstNode Node
        {
            get
            {
                if (MethodDeclaration != null)
                {
                    return MethodDeclaration;
                }

                if (AnonymousMethodExpression != null)
                {
                    return AnonymousMethodExpression;
                }

                if (LambdaExpression != null)
                {
                    return LambdaExpression;
                }

                if (Accessor != null)
                {
                    return Accessor;
                }

                return null;
            }
        }

        public MethodDeclaration MethodDeclaration { get; set; }

        public LambdaExpression LambdaExpression { get; set; }

        public AnonymousMethodExpression AnonymousMethodExpression { get; set; }

        public Accessor Accessor { get; set; }

        public List<IAsyncJumpLabel> JumpLabels { get; set; }

        public AstNode Body
        {
            get
            {
                if (MethodDeclaration != null)
                {
                    return MethodDeclaration.Body;
                }

                if (LambdaExpression != null)
                {
                    return LambdaExpression.Body;
                }

                if (AnonymousMethodExpression != null)
                {
                    return AnonymousMethodExpression.Body;
                }

                if (Accessor != null)
                {
                    return Accessor.Body;
                }

                return null;
            }
        }

        public List<string> Parameters
        {
            get
            {
                AstNodeCollection<ParameterDeclaration> prms = null;
                AstNodeCollection<TypeParameterDeclaration> tprms = null;

                if (MethodDeclaration != null)
                {
                    prms = MethodDeclaration.Parameters;

                    if (MethodDeclaration.TypeParameters.Count > 0 &&
                        !Helpers.IsIgnoreGeneric(MethodDeclaration, Emitter))
                    {
                        tprms = MethodDeclaration.TypeParameters;
                    }
                }
                else if (LambdaExpression != null)
                {
                    prms = LambdaExpression.Parameters;
                }
                else if (AnonymousMethodExpression != null)
                {
                    prms = AnonymousMethodExpression.Parameters;
                }

                var result = new List<string>();

                if (tprms != null)
                {
                    foreach (var typeParameterDeclaration in tprms)
                    {
                        result.Add(typeParameterDeclaration.Name);
                    }
                }

                if (prms != null)
                {
                    foreach (var parameterDeclaration in prms)
                    {
                        var name = Emitter.GetParameterName(parameterDeclaration);
                        if (Emitter.LocalsNamesMap != null && Emitter.LocalsNamesMap.ContainsKey(name))
                        {
                            name = Emitter.LocalsNamesMap[name];
                        }

                        result.Add(name);
                    }
                }

                return result;
            }
        }

        protected bool PreviousIsAync { get; set; }

        protected bool PreviousIsYield { get; set; }

        protected List<string> PreviousAsyncVariables { get; set; }

        protected IAsyncBlock PreviousAsyncBlock { get; set; }

        public AstNode[] AwaitExpressions { get; set; }

        public List<AstNode> WrittenAwaitExpressions { get; set; }

        public IType ReturnType { get; set; }

        public bool IsTaskReturn { get; set; }

        public bool IsEnumeratorReturn { get; set; }

        public int Step { get; set; }

        public List<IAsyncStep> Steps { get; set; }

        public List<IAsyncStep> EmittedAsyncSteps { get; set; }

        public bool ReplaceAwaiterByVar { get; set; }

        public List<IAsyncTryInfo> TryInfos { get; set; }

        public void InitAsyncBlock()
        {
            PreviousIsAync = Emitter.IsAsync;
            PreviousIsYield = Emitter.IsYield;
            Emitter.IsAsync = true;
            Emitter.IsYield = true;

            PreviousAsyncVariables = Emitter.AsyncVariables;
            Emitter.AsyncVariables = new List<string>();

            PreviousAsyncBlock = Emitter.AsyncBlock;
            Emitter.AsyncBlock = this;

            ReplaceAwaiterByVar = Emitter.ReplaceAwaiterByVar;
            Emitter.ReplaceAwaiterByVar = false;

            DetectReturnType();
            FindAwaitNodes();

            Steps = new List<IAsyncStep>();
            TryInfos = new List<IAsyncTryInfo>();
            JumpLabels = new List<IAsyncJumpLabel>();
            WrittenAwaitExpressions = new List<AstNode>();
        }

        protected void FindAwaitNodes()
        {
            AwaitExpressions = GetAwaiters(Body);
        }

        protected void DetectReturnType()
        {
            if (MethodDeclaration != null)
            {
                ReturnType = Emitter.Resolver.ResolveNode(MethodDeclaration.ReturnType, Emitter).Type;
            }
            else if (LambdaExpression != null)
            {
                ReturnType = ((LambdaResolveResult)Emitter.Resolver.ResolveNode(LambdaExpression, Emitter)).ReturnType;
            }
            else if (AnonymousMethodExpression != null)
            {
                ReturnType = ((LambdaResolveResult)Emitter.Resolver.ResolveNode(AnonymousMethodExpression, Emitter)).ReturnType;
            }
            else if (Accessor != null)
            {
                ReturnType = Emitter.Resolver.ResolveNode(((EntityDeclaration)Accessor.Parent).ReturnType, Emitter).Type;
            }

            IsEnumeratorReturn = ReturnType.Name == "IEnumerator";
        }

        protected void FinishGeneratorBlock()
        {
            Emitter.IsAsync = PreviousIsAync;
            Emitter.IsYield = PreviousIsYield;
            Emitter.AsyncVariables = PreviousAsyncVariables;
            Emitter.AsyncBlock = PreviousAsyncBlock;
            Emitter.ReplaceAwaiterByVar = ReplaceAwaiterByVar;
        }

        protected override void DoEmit()
        {
            Emit(false);
        }

        public void Emit(bool skipInit)
        {
            if (!skipInit)
            {
                InitAsyncBlock();
            }

            EmitGeneratorBlock();
            FinishGeneratorBlock();
        }

        protected void EmitGeneratorBlock()
        {
            BeginBlock();
            var args = Parameters;

            if (!IsEnumeratorReturn)
            {
                WriteReturn(true);
                Write("new ");

                if (ReturnType.IsParameterized)
                {
                    Write("(H5.GeneratorEnumerable$1(" + H5Types.ToJsName(ReturnType.TypeArguments[0], Emitter) + "))");
                }
                else
                {
                    Write("H5.GeneratorEnumerable");
                }

                WriteOpenParentheses();

                Write(JS.Funcs.H5_BIND + "(this, ");
                WriteFunction();
                if (args.Count > 0)
                {
                    WriteOpenParentheses();

                    for (int i = 0; i < args.Count; i++)
                    {
                        Write(args[i]);

                        if (i < args.Count - 1)
                        {
                            Write(", ");
                        }
                    }

                    WriteCloseParentheses();
                }
                else
                {
                    WriteOpenCloseParentheses(true);
                }

                WriteSpace();

                BeginBlock();
            }

            WriteVar(true);
            Write(JS.Vars.ASYNC_STEP + " = 0");
            Emitter.Comma = true;
            Indent();

            // This is required to add async variables into Emitter.AsyncVariables and emit them prior to body
            IWriterInfo writerInfo = SaveWriter();
            StringBuilder body = NewWriter();
            Emitter.ResetLevel(writerInfo.Level - 1);
            EmitGeneratorBody();
            RestoreWriter(writerInfo);

            foreach (var localVar in Emitter.AsyncVariables)
            {
                EnsureComma(true);
                Write(localVar);
                Emitter.Comma = true;
            }

            Emitter.Comma = false;
            WriteSemiColon();
            Outdent();
            WriteNewLine();
            WriteNewLine();

            WriteVar(true);
            Write(JS.Vars.ENUMERATOR + " = new ");

            if (ReturnType.IsParameterized)
            {
                Write("(" + JS.Types.H5.Generator.NAME_GENERIC +"(" + H5Types.ToJsName(ReturnType.TypeArguments[0], Emitter) + "))");
            }
            else
            {
                Write(JS.Types.H5.Generator.NAME);
            }

            WriteOpenParentheses();
            Write(JS.Funcs.H5_BIND + "(this, ");
            WriteFunction();
            WriteOpenCloseParentheses(true);

            Write(body);

            WriteCloseParentheses();
            EmitFinallyHandler();
            WriteCloseParentheses();
            WriteSemiColon();
            WriteNewLine();

            WriteReturn(true);
            Write(JS.Vars.ENUMERATOR);
            WriteSemiColon();
            WriteNewLine();

            if (!IsEnumeratorReturn)
            {
                EndBlock();
                if (args.Count > 0)
                {
                    Write(", arguments");
                }
                WriteCloseParentheses();
                WriteCloseParentheses();
                WriteSemiColon();
                WriteNewLine();
            }

            EndBlock();
        }

        protected void EmitGeneratorBody()
        {
            BeginBlock();

            var asyncTryVisitor = new AsyncTryVisitor();
            Node.AcceptChildren(asyncTryVisitor);
            var needTry = true;

            Emitter.AsyncVariables.Add(JS.Vars.ASYNC_JUMP);
            if (needTry)
            {
                Emitter.AsyncVariables.Add(JS.Vars.ASYNC_RETURN_VALUE);

                WriteTry();
                BeginBlock();
            }

            WriteFor();
            Write("(;;) ");
            BeginBlock();

            WriteSwitch();
            Write("(" + JS.Vars.ASYNC_STEP + ") ");
            BeginBlock();

            Step = 0;
            var writer = SaveWriter();
            AddAsyncStep();

            Body.AcceptVisitor(Emitter);

            RestoreWriter(writer);

            InjectSteps();

            WriteNewLine();
            EndBlock();

            WriteNewLine();
            EndBlock();

            if (needTry)
            {
                if (!Emitter.Locals.ContainsKey(JS.Vars.ASYNC_E))
                {
                    AddLocal(JS.Vars.ASYNC_E, null, AstType.Null);
                }

                WriteNewLine();
                EndBlock();
                Write(" catch(" + JS.Vars.ASYNC_E1 + ") ");
                BeginBlock();
                Write(JS.Vars.ASYNC_E + " = " + JS.Types.System.Exception.CREATE + "(" + JS.Vars.ASYNC_E1 + ");");
                WriteNewLine();
                InjectCatchHandlers();

                WriteNewLine();
                EndBlock();
            }

            WriteNewLine();
            EndBlock();
        }

        protected void InjectCatchHandlers()
        {
            var infos = TryInfos;

            foreach(var info in infos)
            {
                if (info.CatchBlocks.Count > 0)
                {
                    WriteIf();
                    WriteOpenParentheses(true);
                    Write(string.Format(JS.Vars.ASYNC_STEP + " >= {0} && " + JS.Vars.ASYNC_STEP + " <= {1}", info.StartStep, info.EndStep));
                    WriteCloseParentheses(true);
                    BeginBlock();
                    var firstClause = true;

                    for(int i = 0; i < info.CatchBlocks.Count; i++)
                    {
                        var clause = info.CatchBlocks[i];
                        var varName = clause.Item1;
                        var exceptionType = clause.Item2;
                        var step = clause.Item3;
                        var isBaseException = exceptionType == JS.Types.System.Exception.NAME;

                        if (info.CatchBlocks.Count == 1 && isBaseException)
                        {
                            if (!string.IsNullOrEmpty(varName))
                            {
                                Write(varName + " = " + JS.Vars.ASYNC_E + ";");
                                WriteNewLine();
                            }

                            Write(JS.Vars.ASYNC_STEP + " = " + step + ";");

                            WriteNewLine();
                            Write(JS.Vars.ENUMERATOR + "." + JS.Funcs.ASYNC_YIELD_BODY + "();");
                            WriteNewLine();
                            Write("return;");
                        }
                        else
                        {
                            if (!firstClause)
                            {
                                WriteSpace();
                                WriteElse();
                            }

                            if (!isBaseException)
                            {
                                WriteIf();
                                WriteOpenParentheses();
                                Write(JS.Types.H5.IS + "(" + JS.Vars.ASYNC_E + ", " + exceptionType + ")");
                                WriteCloseParentheses();
                                WriteSpace();
                            }

                            firstClause = false;

                            BeginBlock();

                            if (!string.IsNullOrEmpty(varName))
                            {
                                Write(varName + " = " + JS.Vars.ASYNC_E + ";");
                                WriteNewLine();
                            }

                            Write(JS.Vars.ASYNC_STEP + " = " + step + ";");

                            WriteNewLine();
                            Write(JS.Vars.ENUMERATOR + "." + JS.Funcs.ASYNC_YIELD_BODY + "();");
                            WriteNewLine();
                            Write("return;");
                            WriteNewLine();
                            EndBlock();
                        }
                    }

                    WriteNewLine();
                    EndBlock();
                    WriteNewLine();
                }

                if (info.FinallyStep > 0)
                {
                    if (!Emitter.Locals.ContainsKey(JS.Vars.ASYNC_E))
                    {
                        AddLocal(JS.Vars.ASYNC_E, null, AstType.Null);
                    }

                    WriteIf();
                    WriteOpenParentheses();
                    Write(string.Format(JS.Vars.ASYNC_STEP + " >= {0} && " + JS.Vars.ASYNC_STEP + " <= {1}", info.StartStep, info.CatchBlocks.Count > 0 ? info.CatchBlocks.Last().Item4 : info.EndStep));
                    WriteCloseParentheses();
                    BeginBlock();

                    //this.Write(Variables.E + " = " + Variables.ASYNC_E + ";");
                    WriteNewLine();
                    Write(JS.Vars.ASYNC_STEP + " = " + info.FinallyStep + ";");

                    WriteNewLine();
                    Write(JS.Vars.ENUMERATOR + "." + JS.Funcs.ASYNC_YIELD_BODY + "();");
                    WriteNewLine();
                    Write("return;");

                    WriteNewLine();
                    EndBlock();
                    WriteNewLine();
                }
            }

            Write("throw " + JS.Vars.ASYNC_E + ";");
        }

        protected bool EmitFinallyHandler()
        {
            var infos = TryInfos;
            bool needHeader = true;

            foreach (var info in infos)
            {
                if (info.FinallyStep > 0)
                {
                    if (!Emitter.Locals.ContainsKey(JS.Vars.ASYNC_E))
                    {
                        AddLocal(JS.Vars.ASYNC_E, null, AstType.Null);
                    }

                    if (needHeader)
                    {
                        Write(", function () ");
                        BeginBlock();
                        needHeader = false;
                    }

                    WriteIf();
                    WriteOpenParentheses();
                    Write(string.Format(JS.Vars.ASYNC_STEP + " >= {0} && " + JS.Vars.ASYNC_STEP + " <= {1}", info.StartStep, info.CatchBlocks.Count > 0 ? info.CatchBlocks.Last().Item4 : info.EndStep));
                    WriteCloseParentheses();
                    BeginBlock();

                    WriteNewLine();
                    Write(JS.Vars.ASYNC_STEP + " = " + info.FinallyStep + ";");

                    WriteNewLine();
                    Write(JS.Vars.ENUMERATOR + "." + JS.Funcs.ASYNC_YIELD_BODY + "();");
                    WriteNewLine();
                    Write("return;");

                    WriteNewLine();
                    EndBlock();
                    WriteNewLine();
                }
            }

            if (!needHeader)
            {
                WriteNewLine();
                EndBlock();
            }

            return !needHeader;
        }

        protected void InjectSteps()
        {
            foreach(var label in JumpLabels)
            {
                var tostep = Steps.First(s => s.Node == label.Node);
                label.Output.Replace(Helpers.PrefixDollar("{", label.Node.GetHashCode(), "}"), tostep.Step.ToString());
            }

            for(int i = 0; i < Steps.Count; i++)
            {
                var step = Steps[i];

                if (i != 0)
                {
                    WriteNewLine();
                }

                var output = step.Output.ToString();
                var cleanOutput = RemoveTokens(output);

                Write("case " + i + ": ");

                BeginBlock();

                bool addNewLine = false;

                if (!string.IsNullOrWhiteSpace(cleanOutput))
                {
                    if (addNewLine)
                    {
                        WriteNewLine();
                    }

                    Write(WriteIndentToString(output.TrimEnd()));
                }

                if (!IsOnlyWhitespaceOnPenultimateLine(false))
                {
                    addNewLine = true;
                }

                if (step.JumpToStep > -1 && !AbstractEmitterBlock.IsJumpStatementLast(cleanOutput))
                {
                    if (addNewLine)
                    {
                        WriteNewLine();
                    }

                    Write(JS.Vars.ASYNC_STEP + " = " + step.JumpToStep + ";");
                    WriteNewLine();
                    Write("continue;");
                }
                else if (step.JumpToNode != null && !AbstractEmitterBlock.IsJumpStatementLast(cleanOutput))
                {
                    var tostep = Steps.First(s => s.Node == step.JumpToNode);

                    if (addNewLine)
                    {
                        WriteNewLine();
                    }

                    Write(JS.Vars.ASYNC_STEP + " = " + tostep.Step + ";");
                    WriteNewLine();
                    Write("continue;");
                }
                else if (i == (Steps.Count - 1) && !AbstractEmitterBlock.IsReturnLast(cleanOutput))
                {
                    if (addNewLine)
                    {
                        WriteNewLine();
                    }
                }

                WriteNewLine();
                EndBlock();
            }

            WriteNewLine();
            Write("default: ");
            BeginBlock();

            Write("return false;");
            WriteNewLine();
            EndBlock();
        }

        public IAsyncStep AddAsyncStep(int fromTaskNumber = -1)
        {
            var step = Step++;
            var asyncStep = new AsyncStep(Emitter, step, fromTaskNumber);
            Steps.Add(asyncStep);

            return asyncStep;
        }

        public IAsyncStep AddAsyncStep(AstNode node)
        {
            var asyncStep = AddAsyncStep();
            asyncStep.Node = node;

            return asyncStep;
        }

        public bool IsParentForAsync(AstNode child)
        {
            if (child is IfElseStatement)
            {
                return false;
            }
            else
            {
                foreach(var awaiter in AwaitExpressions)
                {
                    if (child.IsInside(awaiter.StartLocation))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }

    public class GeneratorStep : IAsyncStep
    {
        public GeneratorStep(IEmitter emitter, int step)
        {
            Step = step;
            Emitter = emitter;
            JumpToStep = -1;

            if (Emitter.LastSavedWriter != null)
            {
                Emitter.LastSavedWriter.Comma = Emitter.Comma;
                Emitter.LastSavedWriter.IsNewLine = Emitter.IsNewLine;
                Emitter.LastSavedWriter.Level = Emitter.Level;
                Emitter.LastSavedWriter = null;
            }

            Output = new StringBuilder();
            Emitter.Output = Output;
            Emitter.IsNewLine = false;
            Emitter.ResetLevel();
            Emitter.Comma = false;
        }

        public int FromTaskNumber { get; set; }

        public int JumpToStep { get; set; }

        public AstNode JumpToNode { get; set; }

        public AstNode Node { get; set; }

        public int Step { get; set; }

        protected IEmitter Emitter { get; set; }

        public StringBuilder Output { get; set; }

        public object Label
        {
            get; set;
        }
    }

    public class GeneratorJumpLabel : IAsyncJumpLabel
    {
        public StringBuilder Output { get; set; }

        public AstNode Node { get; set; }
    }
}