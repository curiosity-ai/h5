using H5.Contract;
using ICSharpCode.NRefactory.CSharp;
using System.Collections.Generic;

namespace H5.Translator
{
    public class TypeExpressionListBlock : AbstractEmitterBlock
    {
        public TypeExpressionListBlock(IEmitter emitter, IEnumerable<TypeParamExpression> expressions)
            : base(emitter, null)
        {
            Emitter = emitter;
            Expressions = expressions;
        }

        public TypeExpressionListBlock(IEmitter emitter, IEnumerable<AstType> types)
            : base(emitter, null)
        {
            Emitter = emitter;
            Types = types;
        }

        public IEnumerable<TypeParamExpression> Expressions { get; set; }

        public IEnumerable<AstType> Types { get; set; }

        protected override void DoEmit()
        {
            if (Expressions != null)
            {
                EmitExpressionList(Expressions);
            }
            else if (Types != null)
            {
                EmitExpressionList(Types);
            }
        }

        protected virtual void EmitExpressionList(IEnumerable<TypeParamExpression> expressions)
        {
            bool needComma = false;

            foreach (var expr in expressions)
            {
                if (expr.Inherited)
                {
                    continue;
                }
                EnsureComma(false);

                Emitter.Translator.EmitNode = expr.AstType;
                if (needComma)
                {
                    WriteComma();
                }

                needComma = true;

                if (expr.AstType != null)
                {
                    Write(H5Types.ToJsName(expr.AstType, Emitter));
                }
                else if (expr.IType != null)
                {
                    Write(H5Types.ToJsName(expr.IType, Emitter));
                }
                else
                {
                    throw new EmitterException(PreviousNode, "There is no type information");
                }
            }

            if (needComma)
            {
                Emitter.Comma = true;
            }
        }

        protected virtual void EmitExpressionList(IEnumerable<AstType> types)
        {
            bool needComma = false;

            foreach (var type in types)
            {
                EnsureComma(false);
                Emitter.Translator.EmitNode = type;
                if (needComma)
                {
                    WriteComma();
                }

                needComma = true;
                Write(H5Types.ToJsName(type, Emitter));
            }

            if (needComma)
            {
                Emitter.Comma = true;
            }
        }
    }
}