using H5.Contract;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.TypeSystem;
using System.Linq;
using ICSharpCode.NRefactory.CSharp.Analysis;

namespace H5.Translator
{
    public class Block : AbstractEmitterBlock
    {
        public Block(IEmitter emitter, BlockStatement blockStatement)
            : base(emitter, blockStatement)
        {
            Emitter = emitter;
            BlockStatement = blockStatement;

            if (blockStatement.Parent is BlockStatement && Emitter.IsAsync)
            {
                Emitter.IgnoreBlock = blockStatement;
            }

            if (Emitter.IgnoreBlock == blockStatement || Emitter.IsAsync && GetAwaiters(blockStatement).Length > 0)
            {
                AsyncNoBraces = true;
            }

            if (Emitter.NoBraceBlock == blockStatement)
            {
                NoBraces = true;
            }
        }

        public BlockStatement BlockStatement { get; set; }

        protected bool AddEndBlock { get; set; }

        public bool AsyncNoBraces { get; set; }

        public bool NoBraces { get; set; }

        public int BeginPosition { get; set; }

        public int SignaturePosition { get; set; }

        public bool IsYield { get; set; }

        public IType ReturnType { get; set; }

        private IType OldReturnType { get; set; }

        public string LoopVar { get; set; }

        public bool? OldReplaceJump { get; set; }

        protected override void DoEmit()
        {
            EmitBlock();
        }

        protected virtual bool KeepLineAfterBlock(BlockStatement block)
        {
            var parent = block.Parent;

            if (AsyncNoBraces || NoBraces)
            {
                return true;
            }

            if (parent is TryCatchStatement tcs && tcs.TryBlock.Equals(BlockStatement))
            {
                return true;
            }

            if (parent is CatchClause)
            {
                return true;
            }

            if (parent is AnonymousMethodExpression)
            {
                return true;
            }

            if (parent is LambdaExpression)
            {
                return true;
            }

            if (parent is MethodDeclaration)
            {
                return true;
            }

            if (parent is OperatorDeclaration)
            {
                return true;
            }

            if (parent is Accessor && (parent.Parent is PropertyDeclaration || parent.Parent is CustomEventDeclaration || parent.Parent is IndexerDeclaration))
            {
                return true;
            }


            if (parent is DoWhileStatement loop)
            {
                return true;
            }

            if (parent is IfElseStatement ifStatement && ifStatement.FalseStatement != null && !ifStatement.FalseStatement.IsNull && ifStatement.FalseStatement != block)
            {
                return true;
            }

            return false;
        }

        public void EmitBlock()
        {
            BeginEmitBlock();
            DoEmitBlock();
            EndEmitBlock();
        }

        private bool? isMethodBlock;
        public bool IsMethodBlock
        {
            get
            {
                if (!isMethodBlock.HasValue)
                {
                    isMethodBlock = BlockStatement.Parent is MethodDeclaration ||
                                         BlockStatement.Parent is AnonymousMethodExpression ||
                                         BlockStatement.Parent is LambdaExpression ||
                                         BlockStatement.Parent is ConstructorDeclaration ||
                                         BlockStatement.Parent is OperatorDeclaration ||
                                         BlockStatement.Parent is Accessor;
                }

                return isMethodBlock.Value;
            }
        }

        public int OldWrapRestCounter { get; private set; }

        public void DoEmitBlock()
        {
            if (BlockStatement.Parent is MethodDeclaration)
            {
                var methodDeclaration = (MethodDeclaration)BlockStatement.Parent;
                if (!methodDeclaration.ReturnType.IsNull)
                {
                    var rr = Emitter.Resolver.ResolveNode(methodDeclaration.ReturnType, Emitter);
                    ReturnType = rr.Type;
                }
                ConvertParamsToReferences(methodDeclaration.Parameters);
            }
            else if (BlockStatement.Parent is AnonymousMethodExpression)
            {
                var methodDeclaration = (AnonymousMethodExpression)BlockStatement.Parent;
                var rr = Emitter.Resolver.ResolveNode(methodDeclaration, Emitter);
                ReturnType = rr.Type;
                ConvertParamsToReferences(methodDeclaration.Parameters);
            }
            else if (BlockStatement.Parent is LambdaExpression)
            {
                var methodDeclaration = (LambdaExpression)BlockStatement.Parent;
                var rr = Emitter.Resolver.ResolveNode(methodDeclaration, Emitter);
                ReturnType = rr.Type;
                ConvertParamsToReferences(methodDeclaration.Parameters);
            }
            else if (BlockStatement.Parent is ConstructorDeclaration)
            {
                ConvertParamsToReferences(((ConstructorDeclaration)BlockStatement.Parent).Parameters);
            }
            else if (BlockStatement.Parent is OperatorDeclaration)
            {
                ConvertParamsToReferences(((OperatorDeclaration)BlockStatement.Parent).Parameters);
            }
            else if (BlockStatement.Parent is Accessor)
            {
                var role = BlockStatement.Parent.Role.ToString();

                if (role == "Setter")
                {
                    ConvertParamsToReferences(new ParameterDeclaration[] { new ParameterDeclaration { Name = "value" } });
                }
                else if (role == "Getter")
                {
                    var methodDeclaration = (Accessor)BlockStatement.Parent;
                    if (!methodDeclaration.ReturnType.IsNull)
                    {
                        var rr = Emitter.Resolver.ResolveNode(methodDeclaration.ReturnType, Emitter);
                        ReturnType = rr.Type;
                    }
                }
            }

            if (IsMethodBlock && YieldBlock.HasYield(BlockStatement))
            {
                IsYield = true;
            }

            if (IsMethodBlock)
            {
                OldReturnType = Emitter.ReturnType;
                Emitter.ReturnType = ReturnType;
            }

            if (Emitter.BeforeBlock != null)
            {
                Emitter.BeforeBlock();
                Emitter.BeforeBlock = null;
            }

            var ra = ReachabilityAnalysis.Create(BlockStatement, Emitter.Resolver.Resolver);
            BlockStatement.Children.ToList().ForEach(child =>
            {
                if (child is Statement statement && !ra.IsReachable(statement))
                {
                    return;
                }

                child.AcceptVisitor(Emitter);
            });
        }

        public void EndEmitBlock()
        {
            if (IsMethodBlock)
            {
                Emitter.ReturnType = OldReturnType;

                if (Emitter.WrapRestCounter > 0)
                {
                    for (int i = 0; i < Emitter.WrapRestCounter; i++)
                    {
                        EndBlock();
                        Write("));");
                        WriteNewLine();
                    }
                }

                Emitter.WrapRestCounter = OldWrapRestCounter;
            }

            if (!NoBraces && (!Emitter.IsAsync || (!AsyncNoBraces && BlockStatement.Parent != Emitter.AsyncBlock.Node)))
            {
                if (IsMethodBlock && BeginPosition == Emitter.Output.Length)
                {
                    EndBlock();
                    Emitter.Output.Length = SignaturePosition;
                    WriteOpenCloseBrace();
                }
                else
                {
                    EndBlock();
                }
            }

            if (AddEndBlock)
            {
                WriteNewLine();
                EndBlock();
            }

            if (OldReplaceJump.HasValue)
            {
                Emitter.ReplaceJump = OldReplaceJump.Value;
            }

            if (!KeepLineAfterBlock(BlockStatement))
            {
                WriteNewLine();
            }

            if (IsMethodBlock && !Emitter.IsAsync)
            {
                EmitTempVars(BeginPosition);
            }

            PopLocals();
        }

        public void BeginEmitBlock()
        {
            PushLocals();

            if (!NoBraces && (!Emitter.IsAsync || (!AsyncNoBraces && BlockStatement.Parent != Emitter.AsyncBlock.Node)))
            {
                SignaturePosition = Emitter.Output.Length;
                BeginBlock();
            }

            BeginPosition = Emitter.Output.Length;

            if (IsMethodBlock)
            {
                OldWrapRestCounter = Emitter.WrapRestCounter;
                Emitter.WrapRestCounter = 0;
            }
        }
    }
}