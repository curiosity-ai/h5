using HighFive.Contract;
using ICSharpCode.NRefactory.CSharp;
using System.Collections.Generic;

namespace HighFive.Translator.TypeScript
{
    public class OperatorBlock : TypeScriptBlock
    {
        public OperatorBlock(IEmitter emitter, OperatorDeclaration operatorDeclaration)
            : base(emitter, operatorDeclaration)
        {
            this.Emitter = emitter;
            this.OperatorDeclaration = operatorDeclaration;
        }

        public OperatorDeclaration OperatorDeclaration
        {
            get;
            set;
        }

        protected override void DoEmit()
        {
            this.EmitOperatorDeclaration(this.OperatorDeclaration);
        }

        protected void EmitOperatorDeclaration(OperatorDeclaration operatorDeclaration)
        {
            XmlToJsDoc.EmitComment(this, operatorDeclaration);
            var overloads = OverloadsCollection.Create(this.Emitter, operatorDeclaration);

            if (overloads.HasOverloads)
            {
                string name = overloads.GetOverloadName();
                this.Write(name);
            }
            else
            {
                this.Write(this.Emitter.GetEntityName(operatorDeclaration));
            }

            this.EmitMethodParameters(operatorDeclaration.Parameters, operatorDeclaration);

            this.WriteColon();

            var retType = HighFiveTypes.ToTypeScriptName(operatorDeclaration.ReturnType, this.Emitter);
            this.Write(retType);

            this.WriteSemiColon();
            this.WriteNewLine();
        }

        protected virtual void EmitMethodParameters(IEnumerable<ParameterDeclaration> declarations, AstNode context)
        {
            this.WriteOpenParentheses();
            bool needComma = false;

            foreach (var p in declarations)
            {
                var name = this.Emitter.GetParameterName(p);

                if (needComma)
                {
                    this.WriteComma();
                }

                needComma = true;
                this.Write(name);
                this.WriteColon();
                name = HighFiveTypes.ToTypeScriptName(p.Type, this.Emitter);
                this.Write(name);
            }

            this.WriteCloseParentheses();
        }
    }
}