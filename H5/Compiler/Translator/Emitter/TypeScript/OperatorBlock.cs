using H5.Contract;
using ICSharpCode.NRefactory.CSharp;
using System.Collections.Generic;

namespace H5.Translator.TypeScript
{
    public class OperatorBlock : TypeScriptBlock
    {
        public OperatorBlock(IEmitter emitter, OperatorDeclaration operatorDeclaration)
            : base(emitter, operatorDeclaration)
        {
            Emitter = emitter;
            OperatorDeclaration = operatorDeclaration;
        }

        public OperatorDeclaration OperatorDeclaration { get; set; }

        protected override void DoEmit()
        {
            EmitOperatorDeclaration(OperatorDeclaration);
        }

        protected void EmitOperatorDeclaration(OperatorDeclaration operatorDeclaration)
        {
            XmlToJsDoc.EmitComment(this, operatorDeclaration);
            var overloads = OverloadsCollection.Create(Emitter, operatorDeclaration);

            if (overloads.HasOverloads)
            {
                string name = overloads.GetOverloadName();
                Write(name);
            }
            else
            {
                Write(Emitter.GetEntityName(operatorDeclaration));
            }

            EmitMethodParameters(operatorDeclaration.Parameters, operatorDeclaration);

            WriteColon();

            var retType = H5Types.ToTypeScriptName(operatorDeclaration.ReturnType, Emitter);
            Write(retType);

            WriteSemiColon();
            WriteNewLine();
        }

        protected virtual void EmitMethodParameters(IEnumerable<ParameterDeclaration> declarations, AstNode context)
        {
            WriteOpenParentheses();
            bool needComma = false;

            foreach (var p in declarations)
            {
                var name = Emitter.GetParameterName(p);

                if (needComma)
                {
                    WriteComma();
                }

                needComma = true;
                Write(name);
                WriteColon();
                name = H5Types.ToTypeScriptName(p.Type, Emitter);
                Write(name);
            }

            WriteCloseParentheses();
        }
    }
}