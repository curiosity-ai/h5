using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.TypeSystem;
using System.Collections.Generic;
using System.Linq;

namespace H5.Translator
{
    public class LambdaBlock : AbstractMethodBlock
    {
        public LambdaBlock(IEmitter emitter, LambdaExpression lambdaExpression)
            : this(emitter, lambdaExpression.Parameters, lambdaExpression.Body, lambdaExpression, lambdaExpression.IsAsync)
        {
        }

        public LambdaBlock(IEmitter emitter, AnonymousMethodExpression anonymousMethodExpression)
            : this(emitter, anonymousMethodExpression.Parameters, anonymousMethodExpression.Body, anonymousMethodExpression, anonymousMethodExpression.IsAsync)
        {
        }

        public LambdaBlock(IEmitter emitter, IEnumerable<ParameterDeclaration> parameters, AstNode body, AstNode context, bool isAsync)
            : base(emitter, context)
        {
            Emitter = emitter;
            Parameters = parameters;
            Body = body;
            Context = context;
            IsAsync = isAsync;
        }

        public bool IsAsync { get; set; }

        public IEnumerable<ParameterDeclaration> Parameters { get; set; }

        public AstNode Body { get; set; }

        public AstNode Context { get; set; }

        protected bool PreviousIsAync { get; set; }

        protected List<string> PreviousAsyncVariables { get; set; }

        protected IAsyncBlock PreviousAsyncBlock { get; set; }

        public bool ReplaceAwaiterByVar { get; set; }

        protected override void DoEmit()
        {
            if (Emitter.TempVariables == null)
            {
                ResetLocals();
            }

            var oldReplaceJump = Emitter.ReplaceJump;
            Emitter.ReplaceJump = false;

            var rr = Emitter.Resolver.ResolveNode(Context);

            if (Context is Expression)
            {
                var conversion = Emitter.Resolver.Resolver.GetConversion((Expression)Context);
                if (conversion.IsAnonymousFunctionConversion)
                {
                    var type = Emitter.Resolver.Resolver.GetExpectedType((Expression)Context);
                    if (type.FullName == typeof(System.Linq.Expressions.Expression).FullName && type.TypeParameterCount == 1)
                    {
                        var expr = new ExpressionTreeBuilder(Emitter.Resolver.Compilation, Emitter, Context.GetParent<SyntaxTree>(), this).BuildExpressionTree((LambdaResolveResult)rr);
                        Write(expr);
                        return;
                    }
                }
            }

            var oldParentVariables = Emitter.ParentTempVariables;
            if (Emitter.ParentTempVariables == null)
            {
                Emitter.ParentTempVariables = new Dictionary<string, bool>(Emitter.TempVariables);
            }
            else
            {
                Emitter.ParentTempVariables = new Dictionary<string, bool>(Emitter.ParentTempVariables);
                foreach (var item in Emitter.TempVariables)
                {
                    Emitter.ParentTempVariables.Add(item.Key, item.Value);
                }
            }

            var oldVars = Emitter.TempVariables;
            Emitter.TempVariables = new Dictionary<string, bool>();
            PreviousIsAync = Emitter.IsAsync;
            Emitter.IsAsync = IsAsync;

            PreviousAsyncVariables = Emitter.AsyncVariables;
            Emitter.AsyncVariables = null;

            PreviousAsyncBlock = Emitter.AsyncBlock;
            Emitter.AsyncBlock = null;

            ReplaceAwaiterByVar = Emitter.ReplaceAwaiterByVar;
            Emitter.ReplaceAwaiterByVar = false;

            EmitLambda(Parameters, Body, Context);

            Emitter.IsAsync = PreviousIsAync;
            Emitter.AsyncVariables = PreviousAsyncVariables;
            Emitter.AsyncBlock = PreviousAsyncBlock;
            Emitter.ReplaceAwaiterByVar = ReplaceAwaiterByVar;
            Emitter.TempVariables = oldVars;
            Emitter.ParentTempVariables = oldParentVariables;
            Emitter.ReplaceJump = oldReplaceJump;
        }

        internal static Statement GetOuterLoop(AstNode context)
        {
            Statement loop = null;
            context.GetParent(node =>
            {
                bool stopSearch = false;

                if (node is ForStatement ||
                    node is ForeachStatement ||
                    node is DoWhileStatement ||
                    node is WhileStatement)
                {
                    loop = (Statement)node;
                }
                else if (node is EntityDeclaration ||
                         node is LambdaExpression ||
                         node is AnonymousMethodExpression)
                {
                    stopSearch = true;
                }

                return stopSearch;
            });

            return loop;
        }

        internal static IVariable[] GetCapturedLoopVariables(IEmitter emitter, AstNode context, IEnumerable<ParameterDeclaration> parameters, bool excludeReadOnly = false)
        {
            var loop = LambdaBlock.GetOuterLoop(context);
            if (loop == null)
            {
                return null;
            }

            var loopVariablesAnalyzer = new LoopVariablesAnalyzer(emitter, excludeReadOnly);
            loopVariablesAnalyzer.Analyze(loop);

            var captureAnalyzer = new CaptureAnalyzer(emitter);
            captureAnalyzer.Analyze(context, parameters.Select(p => p.Name));
            return captureAnalyzer.UsedVariables.Where(v => loopVariablesAnalyzer.Variables.Contains(v)).ToArray();
        }

        private string[] GetCapturedLoopVariablesNames()
        {
            var capturedVariables = LambdaBlock.GetCapturedLoopVariables(Emitter, Context, Parameters);

            if (capturedVariables == null)
            {
                return null;
            }

            List<string> names = new List<string>();
            foreach (var capturedVariable in capturedVariables)
            {
                if (Emitter.LocalsMap != null && Emitter.LocalsMap.ContainsKey(capturedVariable))
                {
                    names.Add(RemoveReferencePart(Emitter.LocalsMap[capturedVariable]));
                }
                else if (Emitter.LocalsNamesMap != null && Emitter.LocalsNamesMap.ContainsKey(capturedVariable.Name))
                {
                    names.Add(RemoveReferencePart(Emitter.LocalsNamesMap[capturedVariable.Name]));
                }
                else
                {
                    names.Add(capturedVariable.Name);
                }
            }

            return names.ToArray();
        }

        private string RemoveReferencePart(string name)
        {
            if (name.EndsWith(".v"))
            {
                name = name.Remove(name.Length - 2);
            }

            return name;
        }

        protected virtual void EmitLambda(IEnumerable<ParameterDeclaration> parameters, AstNode body, AstNode context)
        {
            var rr = Emitter.Resolver.ResolveNode(context);
            var oldLifting = Emitter.ForbidLifting;
            Emitter.ForbidLifting = false;
            var noLiftingRule = Emitter.Rules.Lambda == LambdaRule.Plain;
            CaptureAnalyzer analyzer = null;

            if (!noLiftingRule)
            {
                analyzer = new CaptureAnalyzer(Emitter);
                analyzer.Analyze(Body, Parameters.Select(p => p.Name));
            }

            var oldLevel = Emitter.Level;
            if (!noLiftingRule && analyzer.UsedVariables.Count == 0)
            {
                Emitter.ResetLevel();
                Indent();
            }

            IAsyncBlock asyncBlock = null;
            PushLocals();

            if (IsAsync)
            {
                if (context is LambdaExpression)
                {
                    asyncBlock = new AsyncBlock(Emitter, (LambdaExpression)context);
                }
                else
                {
                    asyncBlock = new AsyncBlock(Emitter, (AnonymousMethodExpression)context);
                }

                asyncBlock.InitAsyncBlock();
            }
            else if (YieldBlock.HasYield(body))
            {
                IsAsync = true;
                if (context is LambdaExpression)
                {
                    asyncBlock = new GeneratorBlock(Emitter, (LambdaExpression)context);
                }
                else
                {
                    asyncBlock = new GeneratorBlock(Emitter, (AnonymousMethodExpression)context);
                }

                asyncBlock.InitAsyncBlock();
            }

            var prevMap = BuildLocalsMap();
            var prevNamesMap = BuildLocalsNamesMap();
            AddLocals(parameters, body);

            bool block = body is BlockStatement;
            Write("");

            var savedThisCount = Emitter.ThisRefCounter;
            var capturedVariables = GetCapturedLoopVariablesNames();
            var hasCapturedVariables = capturedVariables != null && capturedVariables.Length > 0;
            if (hasCapturedVariables)
            {
                Write("(function ($me, ");
                Write(string.Join(", ", capturedVariables) + ") ");
                BeginBlock();
                Write("return ");
            }

            var savedPos = Emitter.Output.Length;

            WriteFunction();
            EmitMethodParameters(parameters, null, context);
            WriteSpace();

            int pos = 0;
            if (!block && !IsAsync)
            {
                BeginBlock();
                pos = Emitter.Output.Length;
            }

            bool isSimpleLambda = body.Parent is LambdaExpression && !block && !IsAsync;

            if (isSimpleLambda)
            {
                ConvertParamsToReferences(parameters);

                if (!(rr is LambdaResolveResult lrr) || lrr.ReturnType.Kind != TypeKind.Void)
                {
                    WriteReturn(true);
                }
            }

            if (IsAsync)
            {
                asyncBlock.Emit(true);
            }
            else
            {
                body.AcceptVisitor(Emitter);
            }

            if (isSimpleLambda)
            {
                WriteSemiColon();
            }

            if (!block && !IsAsync)
            {
                WriteNewLine();
                EndBlock();
            }

            if (!block && !IsAsync)
            {
                EmitTempVars(pos);
            }

            if (!noLiftingRule && analyzer.UsedVariables.Count == 0)
            {
                if (!Emitter.ForbidLifting)
                {
                    var name = "f" + (Emitter.NamedFunctions.Count + 1);
                    var code = Emitter.Output.ToString().Substring(savedPos);
                    var codeForComare = RemoveTokens(code);

                    var pair = Emitter.NamedFunctions.FirstOrDefault(p =>
                    {
                        if (Emitter.AssemblyInfo.SourceMap.Enabled)
                        {
                            return RemoveTokens(p.Value) == codeForComare;
                        }

                        return p.Value == code;
                    });

                    if (pair.Key != null && pair.Value != null)
                    {
                        name = pair.Key;
                    }
                    else
                    {
                        Emitter.NamedFunctions.Add(name, code);
                    }

                    Emitter.Output.Remove(savedPos, Emitter.Output.Length - savedPos);
                    Emitter.Output.Insert(savedPos, JS.Vars.D_ + "." + H5Types.ToJsName(Emitter.TypeInfo.Type, Emitter, true) + "." + name);
                }

                Emitter.ResetLevel(oldLevel);
            }

            Emitter.ForbidLifting = oldLifting;


            var methodDeclaration = Body.GetParent<MethodDeclaration>();
            var thisCaptured = Emitter.ThisRefCounter > savedThisCount ||
                               IsAsync && methodDeclaration != null &&
                               !methodDeclaration.HasModifier(Modifiers.Static);

            if (thisCaptured)
            {
                Emitter.Output.Insert(savedPos, JS.Funcs.H5_BIND + (hasCapturedVariables ? "($me, " : "(this, "));
                WriteCloseParentheses();
            }

            if (hasCapturedVariables)
            {
                WriteSemiColon(true);
                EndBlock();
                Write(")(");

                Write("this, ");

                Write(string.Join(", ", capturedVariables));
                Write(")");
            }

            PopLocals();
            ClearLocalsMap(prevMap);
            ClearLocalsNamesMap(prevNamesMap);
        }
    }
}