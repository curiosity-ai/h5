using H5.Contract;
using H5.Contract.Constants;

using ICSharpCode.NRefactory.CSharp;

using System.Collections.Generic;
using System.Linq;

namespace H5.Translator
{
    public abstract class AbstractMethodBlock : AbstractEmitterBlock
    {
        public AbstractMethodBlock(IEmitter emitter, AstNode node)
            : base(emitter, node)
        {
        }

        protected virtual void EmitMethodParameters(IEnumerable<ParameterDeclaration> declarations, IEnumerable<TypeParameterDeclaration> typeParamsdeclarations, AstNode context, bool skipCloseParentheses = false)
        {
            WriteOpenParentheses();
            bool needComma = false;

            if (typeParamsdeclarations != null && typeParamsdeclarations.Any())
            {
                EmitTypeParameters(typeParamsdeclarations, context);

                if (declarations.Any())
                {
                    EnsureComma(false);
                }
            }

            foreach (var p in declarations)
            {
                var name = Emitter.GetParameterName(p);

                name = name.Replace(JS.Vars.FIX_ARGUMENT_NAME, "");

                if (Emitter.LocalsNamesMap != null && Emitter.LocalsNamesMap.ContainsKey(name))
                {
                    name = Emitter.LocalsNamesMap[name];
                }

                if (needComma)
                {
                    WriteComma();
                }

                needComma = true;
                WriteSourceMapName(p.Name);
                WriteSequencePoint(p.Region);
                Write(name);
            }

            if (!skipCloseParentheses)
            {
                WriteCloseParentheses();
            }
        }

        protected virtual void EmitTypeParameters(IEnumerable<TypeParameterDeclaration> declarations, AstNode context)
        {
            bool needComma = false;

            foreach (var p in declarations)
            {
                Emitter.Validator.CheckIdentifier(p.Name, context);

                if (needComma)
                {
                    WriteComma();
                }

                needComma = true;
                WriteSourceMapName(p.Name);
                WriteSequencePoint(p.Region);
                Write(p.Name.Replace(JS.Vars.FIX_ARGUMENT_NAME, ""));
                Emitter.Comma = true;
            }
        }
    }
}