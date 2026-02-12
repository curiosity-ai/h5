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
    public class AsyncBlock : AbstractEmitterBlock, IAsyncBlock
    {
        public AsyncBlock(IEmitter emitter, MethodDeclaration methodDeclaration)
            : base(emitter, methodDeclaration)
        {
            Emitter = emitter;
            MethodDeclaration = methodDeclaration;
        }

        public AsyncBlock(IEmitter emitter, LambdaExpression lambdaExpression)
            : base(emitter, lambdaExpression)
        {
            Emitter = emitter;
            LambdaExpression = lambdaExpression;
        }

        public AsyncBlock(IEmitter emitter, AnonymousMethodExpression anonymousMethodExpression)
            : base(emitter, anonymousMethodExpression)
        {
            Emitter = emitter;
            AnonymousMethodExpression = anonymousMethodExpression;
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

                return null;
            }
        }

        public MethodDeclaration MethodDeclaration { get; set; }

        public LambdaExpression LambdaExpression { get; set; }

        public AnonymousMethodExpression AnonymousMethodExpression { get; set; }

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

                return null;
            }
        }

        protected bool PreviousIsAync { get; set; }

        protected bool PreviousIsNativeAsync { get; set; }

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
            PreviousAsyncExpressionHandling = Emitter.AsyncExpressionHandling;
            Emitter.AsyncExpressionHandling = false;

            PreviousIsAync = Emitter.IsAsync;
            PreviousIsNativeAsync = Emitter.IsNativeAsync;
            Emitter.IsAsync = true;

            bool isExplicitlyAsync = false;
            if (MethodDeclaration != null)
            {
                isExplicitlyAsync = MethodDeclaration.HasModifier(Modifiers.Async);
            }
            else if (LambdaExpression != null)
            {
                isExplicitlyAsync = LambdaExpression.IsAsync;
            }
            else if (AnonymousMethodExpression != null)
            {
                isExplicitlyAsync = AnonymousMethodExpression.IsAsync;
            }

            bool hasGoto = HasGoto(Body);

            Emitter.IsNativeAsync = isExplicitlyAsync && !hasGoto;

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

        public bool PreviousAsyncExpressionHandling { get; set; }

        protected void DetectReturnType()
        {
            AstNode node = MethodDeclaration?.ReturnType;

            if (node == null)
            {
                node = AnonymousMethodExpression;
            }

            if (node == null)
            {
                node = LambdaExpression;
            }

            var resolveResult = Emitter.Resolver.ResolveNode(node);

            if (resolveResult is LambdaResolveResult)
            {
                ReturnType = ((LambdaResolveResult)resolveResult).ReturnType;
            }
            else if (resolveResult is TypeResolveResult)
            {
                ReturnType = ((TypeResolveResult)resolveResult).Type;
            }

            IsTaskReturn = ReturnType != null && ReturnType.Name == "Task" && ReturnType.FullName.StartsWith("System.Threading.Tasks.Task");
        }

        protected void FindAwaitNodes()
        {
            AwaitExpressions = GetAwaiters(Body);

            for (int i = 0; i < AwaitExpressions.Length; i++)
            {
                if (AwaitExpressions[i] is Expression)
                {
                    Emitter.AsyncVariables.Add(JS.Vars.ASYNC_TASK + (i + 1));

                    if (IsTaskResult((Expression)AwaitExpressions[i]))
                    {
                        Emitter.AsyncVariables.Add(JS.Vars.ASYNC_TASK_RESULT + (i + 1));
                    }
                }
            }
        }

        protected bool IsTaskResult(Expression expression)
        {
            var resolveResult = Emitter.Resolver.ResolveNode(expression);

            IType type;

            if (resolveResult is DynamicInvocationResolveResult)
            {
                return expression.Parent is UnaryOperatorExpression && !(expression.Parent.Parent is Statement);
            }
            else if (resolveResult is InvocationResolveResult)
            {
                type = ((InvocationResolveResult)resolveResult).Member.ReturnType;
            }
            else
            {
                type = resolveResult.Type;
            }

            if ((type.FullName == "System.Threading.Tasks.TaskAwaiter" || type.FullName == "System.Threading.Tasks.Task") && type.TypeParameterCount > 0)
            {
                return true;
            }

            if (expression.Parent is UnaryOperatorExpression unaryExpr && unaryExpr.Operator == UnaryOperatorType.Await)
            {
                if (Emitter.Resolver.ResolveNode(unaryExpr) is AwaitResolveResult rr)
                {
                    if (rr.GetAwaiterInvocation is InvocationResolveResult awaiterMethod)
                    {
                        type = awaiterMethod.Type;

                        if ((type.FullName == "System.Threading.Tasks.TaskAwaiter" || type.FullName == "System.Threading.Tasks.Task") && type.TypeParameterCount > 0)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        protected void FinishAsyncBlock()
        {
            Emitter.IsAsync = PreviousIsAync;
            Emitter.IsNativeAsync = PreviousIsNativeAsync;
            Emitter.AsyncVariables = PreviousAsyncVariables;
            Emitter.AsyncBlock = PreviousAsyncBlock;
            Emitter.ReplaceAwaiterByVar = ReplaceAwaiterByVar;
            Emitter.AsyncExpressionHandling = PreviousAsyncExpressionHandling;
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

            if (Emitter.IsNativeAsync)
            {
                EmitNativeAsyncBlock();
            }
            else
            {
                EmitAsyncBlock();
            }

            FinishAsyncBlock();
        }

        protected void EmitNativeAsyncBlock()
        {
            BeginBlock();

            if (IsTaskReturn)
            {
                Write("var " + JS.Vars.ASYNC_TCS + " = new " + ((Emitter.AssemblyInfo.Rules.UseShortForms ?? false) ? JS.Types.SHORTEN_TASK_COMPLETION_SOURCE : JS.Types.TASK_COMPLETION_SOURCE) + "();");
                WriteNewLine();
            }

            Write("(async () => ");

            bool needsBraces = true;
            if (Body is BlockStatement)
            {
                // We wrap BlockStatement in braces because Block visitor might skip them based on logic in Block.cs
                // Even if Block visitor emits braces, double braces are valid.
                // If Block visitor skips braces (e.g. if we messed up logic), this ensures we have them for the arrow function.
                BeginBlock();
                Body.AcceptVisitor(Emitter);
                EndBlock();
            }
            else if (Body is Expression && !(Body.Parent is LambdaExpression))
            {
                BeginBlock();
                Body.AcceptVisitor(Emitter);
                EndBlock();
            }
            else
            {
                // Expression body
                Body.AcceptVisitor(Emitter);
            }

            Write(")()");

            if (IsTaskReturn)
            {
                Write(".then(");
                Write("function (r) { " + JS.Vars.ASYNC_TCS + "." + ((Emitter.AssemblyInfo.Rules.UseShortForms ?? false) ? JS.Funcs.SHORTEN_SET_RESULT : JS.Funcs.SET_RESULT) + "(r); }");
                Write(", ");
                Write("function (e) { " + JS.Vars.ASYNC_TCS + "." + ((Emitter.AssemblyInfo.Rules.UseShortForms ?? false) ? JS.Funcs.SHORTEN_SET_EXCEPTION : JS.Funcs.SET_EXCEPTION) + "(e); }");
                Write(");");

                WriteNewLine();
                Write("return " + JS.Vars.ASYNC_TCS + "." + JS.Fields.ASYNC_TASK + ";");
            }

            WriteNewLine();
            EndBlock();
        }

        protected void EmitAsyncBlock()
        {
            BeginBlock();
            WriteVar(true);
            Write(JS.Vars.ASYNC_STEP + " = 0,");
            var pos = Emitter.Output.Length;
            WriteNewLine();

            Indent();
            Write(((Emitter.AssemblyInfo.Rules.UseShortForms ?? false) ? JS.Funcs.SHORTEN_ASYNC_BODY : JS.Funcs.ASYNC_BODY) + " = " + JS.Funcs.H5_BIND + "(this, ");
            WriteFunction();
            Write("() ");

            EmitAsyncBody();

            string temp = Emitter.Output.ToString(pos, Emitter.Output.Length - pos);
            Emitter.Output.Length = pos;

            foreach (var localVar in Emitter.AsyncVariables)
            {
                WriteNewLine();
                Write(localVar);
                WriteComma();
            }

            Emitter.Output.Append(temp);
            Write(", " + JS.Vars.ARGUMENTS + ")");
            WriteSemiColon();
            WriteNewLine();
            WriteNewLine();
            Outdent();

            if (Emitter.AsyncBlock.MethodDeclaration != null &&
                !Emitter.AsyncBlock.MethodDeclaration.HasModifier(Modifiers.Async))
            {
                Write("return ");
            }

            Write(((Emitter.AssemblyInfo.Rules.UseShortForms ?? false) ? JS.Funcs.SHORTEN_ASYNC_BODY : JS.Funcs.ASYNC_BODY) + "();");

            if (IsTaskReturn)
            {
                WriteNewLine();
                Write("return " + JS.Vars.ASYNC_TCS + "." + JS.Fields.ASYNC_TASK + ";");
            }

            WriteNewLine();

            EndBlock();
        }

        protected void EmitAsyncBody()
        {
            BeginBlock();

            var asyncTryVisitor = new AsyncTryVisitor();
            Node.AcceptChildren(asyncTryVisitor);
            var needTry = asyncTryVisitor.Found || IsTaskReturn;

            Emitter.AsyncVariables.Add(JS.Vars.ASYNC_JUMP);
            if (needTry)
            {
                if (IsTaskReturn)
                {
                    Emitter.AsyncVariables.Add(JS.Vars.ASYNC_TCS + " = new " +
                         ((Emitter.AssemblyInfo.Rules.UseShortForms ?? false) ? JS.Types.SHORTEN_TASK_COMPLETION_SOURCE : JS.Types.TASK_COMPLETION_SOURCE)
                         + "()");
                }

                Emitter.AsyncVariables.Add(JS.Vars.ASYNC_RETURN_VALUE);

                Write("try");
                WriteSpace();
                BeginBlock();
            }

            Write("for (;;) ");
            BeginBlock();
            WriteIndent();
            int checkerPos = Emitter.Output.Length;
            WriteNewLine();
            Write("switch (" + JS.Vars.ASYNC_STEP + ") ");

            BeginBlock();

            Step = 0;
            var writer = SaveWriter();
            AddAsyncStep();

            if (Body.Parent is LambdaExpression && Body is Expression && IsTaskReturn && ReturnType.FullName == "System.Threading.Tasks.Task" && ReturnType.TypeParameterCount > 0)
            {
                new ReturnBlock(Emitter, (Expression)Body).Emit();
            }
            else
            {
                var level = Emitter.InitialLevel;
                ((Emitter) Emitter).InitialLevel = 0;
                Emitter.ResetLevel();
                Body.AcceptVisitor(Emitter);
                ((Emitter)Emitter).InitialLevel = level;
            }

            RestoreWriter(writer);

            InjectSteps();

            WriteNewLine();
            EndBlock();

            InjectStepsChecker(checkerPos);
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

        protected void InjectStepsChecker(int pos)
        {
            var list = new List<int>();
            for (int i = 0; i < Steps.Count; i++)
            {
                var step = Steps[i];
                if (string.IsNullOrWhiteSpace(RemoveTokens(step.Output.ToString())) && step.JumpToStep == (i + 1) && step.FromTaskNumber < 0)
                {
                    continue;
                }
                list.Add(i);
            }

            Emitter.Output.Insert(pos, JS.Vars.ASYNC_STEP + " = " +
                ((Emitter.AssemblyInfo.Rules.UseShortForms ?? false) ? JS.Types.System.Array.SHORTEN_MIN : JS.Types.System.Array.MIN)
                 + "(" + Emitter.ToJavaScript(list.ToArray()) + ", " + JS.Vars.ASYNC_STEP + ");");
        }

        protected void InjectCatchHandlers()
        {
            var infos = TryInfos;

            foreach (var info in infos)
            {
                if (info.CatchBlocks.Count > 0)
                {
                    WriteIf();
                    WriteOpenParentheses(true);
                    Write(string.Format(JS.Vars.ASYNC_STEP + " >= {0} && " + JS.Vars.ASYNC_STEP + " <= {1}", info.StartStep, info.EndStep));
                    WriteCloseParentheses(true);
                    WriteSpace();
                    BeginBlock();
                    var firstClause = true;

                    for (int i = 0; i < info.CatchBlocks.Count; i++)
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
                            Write(((Emitter.AssemblyInfo.Rules.UseShortForms ?? false) ? JS.Funcs.SHORTEN_ASYNC_BODY : JS.Funcs.ASYNC_BODY) + "();");
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
                            Write(((Emitter.AssemblyInfo.Rules.UseShortForms ?? false) ? JS.Funcs.SHORTEN_ASYNC_BODY : JS.Funcs.ASYNC_BODY) + "();");
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
                    WriteSpace();
                    BeginBlock();

                    //this.Write(Variables.E + " = " + Variables.ASYNC_E + ";");
                    //this.WriteNewLine();
                    Write(JS.Vars.ASYNC_STEP + " = " + info.FinallyStep + ";");

                    WriteNewLine();
                    Write(((Emitter.AssemblyInfo.Rules.UseShortForms ?? false) ? JS.Funcs.SHORTEN_ASYNC_BODY : JS.Funcs.ASYNC_BODY) + "();");
                    WriteNewLine();
                    Write("return;");

                    WriteNewLine();
                    EndBlock();
                    WriteNewLine();
                }
            }

            if (IsTaskReturn)
            {
                Write(JS.Vars.ASYNC_TCS + "." + ((Emitter.AssemblyInfo.Rules.UseShortForms ?? false) ? JS.Funcs.SHORTEN_SET_EXCEPTION : JS.Funcs.SET_EXCEPTION) + "(" + JS.Vars.ASYNC_E + ");");
            }
            else
            {
                Write("throw " + JS.Vars.ASYNC_E + ";");
            }
        }

        protected void InjectSteps()
        {
            foreach (var label in JumpLabels)
            {
                var tostep = Steps.First(s => s.Node == label.Node);
                label.Output.Replace(Helpers.PrefixDollar("{", label.Node.GetHashCode(), "}"), tostep.Step.ToString());
            }

            for (int i = 0; i < Steps.Count; i++)
            {
                var step = Steps[i];

                if (i != 0)
                {
                    WriteNewLine();
                }

                var output = step.Output.ToString();
                var cleanOutput = RemoveTokens(output);

                if (string.IsNullOrWhiteSpace(cleanOutput) && step.JumpToStep == (i + 1) && step.FromTaskNumber < 0)
                {
                    continue;
                }

                Write("case " + i + ": ");

                BeginBlock();

                bool addNewLine = false;

                if (step.FromTaskNumber > -1)
                {
                    var expression = (Expression)AwaitExpressions[step.FromTaskNumber - 1];

                    if (IsTaskResult(expression))
                    {
                        Write(string.Format("{0}{1} = {2}{1}.{3}();", JS.Vars.ASYNC_TASK_RESULT, step.FromTaskNumber, JS.Vars.ASYNC_TASK,
                            ((Emitter.AssemblyInfo.Rules.UseShortForms ?? false) ? JS.Funcs.SHORTEN_GET_AWAITED_RESULT : JS.Funcs.GET_AWAITED_RESULT)
                            ));
                    }
                    else
                    {
                        Write(string.Format("{0}{1}.{2}();", JS.Vars.ASYNC_TASK, step.FromTaskNumber, ((Emitter.AssemblyInfo.Rules.UseShortForms ?? false) ? JS.Funcs.SHORTEN_GET_AWAITED_RESULT : JS.Funcs.GET_AWAITED_RESULT)));
                    }

                    addNewLine = true;
                }

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

                if (step.JumpToStep > -1 && !IsJumpStatementLast(cleanOutput))
                {
                    if (addNewLine)
                    {
                        WriteNewLine();
                    }

                    Write(JS.Vars.ASYNC_STEP + " = " + step.JumpToStep + ";");
                    WriteNewLine();
                    Write("continue;");
                }
                else if (step.JumpToNode != null && !IsJumpStatementLast(cleanOutput))
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
                else if (i == (Steps.Count - 1) && !IsReturnLast(cleanOutput))
                {
                    if (addNewLine)
                    {
                        WriteNewLine();
                    }

                    if (IsTaskReturn)
                    {
                        Write(JS.Vars.ASYNC_TCS + "." + ((Emitter.AssemblyInfo.Rules.UseShortForms ?? false) ? JS.Funcs.SHORTEN_SET_RESULT : JS.Funcs.SET_RESULT) + "(null);");
                        WriteNewLine();
                    }

                    Write("return;");
                }

                WriteNewLine();
                EndBlock();
            }

            WriteNewLine();
            Write("default: ");
            BeginBlock();

            if (IsTaskReturn)
            {
                Write(JS.Vars.ASYNC_TCS + "." + ((Emitter.AssemblyInfo.Rules.UseShortForms ?? false) ? JS.Funcs.SHORTEN_SET_RESULT : JS.Funcs.SET_RESULT) + "(null);");
                WriteNewLine();
            }

            Write("return;");
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
                foreach (var awaiter in AwaitExpressions)
                {
                    if (child.IsInside(awaiter.StartLocation))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool HasGoto(AstNode node)
        {
            var visitor = new GotoSearchVisitor();
            node.AcceptVisitor(visitor);
            return visitor.Found;
        }
    }

    public class AsyncStep : IAsyncStep
    {
        public AsyncStep(IEmitter emitter, int step, int fromTaskNumber)
        {
            Step = step;
            Emitter = emitter;
            JumpToStep = -1;
            FromTaskNumber = -1;

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

            FromTaskNumber = fromTaskNumber;
        }

        public int FromTaskNumber { get; set; }

        public int JumpToStep { get; set; }

        public AstNode JumpToNode { get; set; }

        public AstNode Node { get; set; }

        public int Step { get; set; }

        protected IEmitter Emitter { get; set; }

        public StringBuilder Output { get; set; }

        public object Label { get; set; }
    }

    public class AsyncJumpLabel : IAsyncJumpLabel
    {
        public StringBuilder Output { get; set; }

        public AstNode Node { get; set; }
    }
}