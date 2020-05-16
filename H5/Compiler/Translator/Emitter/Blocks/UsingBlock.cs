using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.Semantics;

namespace H5.Translator
{
    public class UsingBlock : AbstractEmitterBlock
    {
        public UsingBlock(IEmitter emitter, UsingStatement usingStatement)
            : base(emitter, usingStatement)
        {
            Emitter = emitter;
            UsingStatement = usingStatement;
        }

        public UsingStatement UsingStatement { get; set; }

        protected override void DoEmit()
        {
            IEnumerable<AstNode> inner = null;

            var res = UsingStatement.ResourceAcquisition;
            if (res is VariableDeclarationStatement varStat)
            {
                VariableDeclarationStatement = varStat;
                inner = varStat.Variables.Skip(1);
                res = varStat.Variables.First();
            }

            EmitUsing(res, inner);
        }

        public VariableDeclarationStatement VariableDeclarationStatement
        {
            get; set;
        }

        protected virtual void EmitUsing(AstNode expression, IEnumerable<AstNode> inner)
        {
            string temp = null;
            string name = null;
            bool isReferenceLocal = false;

            PushLocals();

            if (expression is VariableInitializer varInit)
            {
                var block = new VariableBlock(Emitter, VariableDeclarationStatement);
                block.Emit();
                name = block.lastVarName;
                isReferenceLocal = block.lastIsReferenceLocal;

                if (isReferenceLocal)
                {
                    name = name + ".v";
                }

                /*name = this.AddLocal(varInit.Name, expression, this.VariableDeclarationStatement.Type);

                this.WriteVar();
                this.Write(name);
                this.Write(" = ");
                varInit.Initializer.AcceptVisitor(this.Emitter);
                this.WriteSemiColon();
                this.WriteNewLine();*/
            }
            else if (expression is IdentifierExpression)
            {
                var resolveResult = Emitter.Resolver.ResolveNode(expression, Emitter);
                var id = ((IdentifierExpression)expression).Identifier;

                if (Emitter.Locals != null && Emitter.Locals.ContainsKey(id) && resolveResult is LocalResolveResult)
                {
                    var lrr = (LocalResolveResult)resolveResult;
                    if (Emitter.LocalsMap != null && Emitter.LocalsMap.ContainsKey(lrr.Variable) && !(expression.Parent is DirectionExpression))
                    {
                        name = Emitter.LocalsMap[lrr.Variable];
                    }
                    else if (Emitter.LocalsNamesMap != null && Emitter.LocalsNamesMap.ContainsKey(id))
                    {
                        name = Emitter.LocalsNamesMap[id];
                    }
                    else
                    {
                        name = id;
                    }
                }
            }

            if (name == null)
            {
                temp = GetTempVarName();
                name = temp;
                Write(temp);
                Write(" = ");
                expression.AcceptVisitor(Emitter);
                WriteSemiColon();
                WriteNewLine();
            }

            WriteTry();

            if (inner != null && inner.Any())
            {
                BeginBlock();
                EmitUsing(inner.First(), inner.Skip(1));
                EndBlock();
                WriteNewLine();
            }
            else
            {
                bool block = UsingStatement.EmbeddedStatement is BlockStatement;

                if (!block)
                {
                    BeginBlock();
                }

                UsingStatement.EmbeddedStatement.AcceptVisitor(Emitter);

                if (!block)
                {
                    EndBlock();
                    WriteNewLine();
                }
            }

            WriteFinally();
            BeginBlock();

            Write("if (" + JS.Funcs.H5_HASVALUE + "(" + name + ")) ");
            BeginBlock();
            Write(name);
            Write(".");
            Write(JS.Funcs.DISPOSE);
            Write("();");
            WriteNewLine();
            EndBlock();
            WriteNewLine();
            EndBlock();
            WriteNewLine();

            if (temp != null)
            {
                RemoveTempVar(temp);
            }

            PopLocals();
        }
    }
}