using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.Semantics;

namespace H5.Translator
{
    public class VariableBlock : AbstractEmitterBlock
    {
        internal string lastVarName;
        internal bool lastIsReferenceLocal;

        public VariableDeclarationStatement VariableDeclarationStatement { get; set; }

        public VariableBlock(IEmitter emitter, VariableDeclarationStatement variableDeclarationStatement)
            : base(emitter, variableDeclarationStatement)
        {
            Emitter = emitter;
            VariableDeclarationStatement = variableDeclarationStatement;
        }

        protected override void DoEmit()
        {
            VisitVariableDeclarationStatement();
        }

        protected virtual void VisitVariableDeclarationStatement()
        {
            bool needVar = true;
            bool needComma = false;
            bool addSemicolon = !Emitter.IsAsync;

            var oldSemiColon = Emitter.EnableSemicolon;
            var asyncExpressionHandling = Emitter.AsyncExpressionHandling;

            foreach (var variable in VariableDeclarationStatement.Variables)
            {
                WriteSourceMapName(variable.Name);

                var varName = AddLocal(variable.Name, variable, VariableDeclarationStatement.Type);
                lastVarName = varName;

                if (variable.Initializer != null && !variable.Initializer.IsNull && variable.Initializer.ToString().Contains(JS.Vars.FIX_ARGUMENT_NAME))
                {
                    continue;
                }

                if (needVar)
                {
                    WriteVar();
                    needVar = false;
                }

                bool isReferenceLocal = false;

                if (Emitter.LocalsMap != null && Emitter.Resolver.ResolveNode(variable) is LocalResolveResult lrr && Emitter.LocalsMap.ContainsKey(lrr.Variable))
                {
                    isReferenceLocal = Emitter.LocalsMap[lrr.Variable].EndsWith(".v");
                }

                lastIsReferenceLocal = isReferenceLocal;
                var hasInitializer = !variable.Initializer.IsNull;

                if (variable.Initializer.IsNull && !VariableDeclarationStatement.Type.IsVar())
                {
                    var typeDef = Emitter.GetTypeDefinition(VariableDeclarationStatement.Type, true);

                    if (typeDef != null && typeDef.IsValueType && !Emitter.Validator.IsExternalType(typeDef))
                    {
                        hasInitializer = true;
                    }
                }

                if ((!Emitter.IsAsync || hasInitializer || isReferenceLocal) && needComma)
                {
                    if (Emitter.IsAsync)
                    {
                        WriteSemiColon(true);
                    }
                    else
                    {
                        WriteComma();
                    }
                }

                needComma = true;

                WriteAwaiters(variable.Initializer);

                if (!Emitter.IsAsync || hasInitializer || isReferenceLocal)
                {
                    Write(varName);
                }

                if (hasInitializer)
                {
                    addSemicolon = true;
                    Write(" = ");

                    if (isReferenceLocal)
                    {
                        Write("{ v : ");
                    }

                    var oldValue = Emitter.ReplaceAwaiterByVar;
                    Emitter.ReplaceAwaiterByVar = true;

                    if (!variable.Initializer.IsNull)
                    {
                        variable.Initializer.AcceptVisitor(Emitter);
                    }
                    else
                    {
                        var typerr = Emitter.Resolver.ResolveNode(VariableDeclarationStatement.Type).Type;
                        var isGeneric = typerr.TypeArguments.Count > 0 && !Helpers.IsIgnoreGeneric(typerr, Emitter);
                        Write(string.Concat("new ", isGeneric ? "(" : "", H5Types.ToJsName(VariableDeclarationStatement.Type, Emitter), isGeneric ? ")" : "", "()"));
                    }
                    Emitter.ReplaceAwaiterByVar = oldValue;

                    if (isReferenceLocal)
                    {
                        Write(" }");
                    }
                }
                else if (isReferenceLocal)
                {
                    addSemicolon = true;
                    Write(" = { }");
                }
            }

            Emitter.AsyncExpressionHandling = asyncExpressionHandling;

            if (Emitter.EnableSemicolon && !needVar && addSemicolon)
            {
                WriteSemiColon(true);
            }

            if (oldSemiColon)
            {
                Emitter.EnableSemicolon = true;
            }
        }
    }
}