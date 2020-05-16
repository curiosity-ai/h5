using H5.Contract;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using System.Collections.Generic;

namespace H5.Translator.TypeScript
{
    public class IndexerBlock : TypeScriptBlock
    {
        public IndexerBlock(IEmitter emitter, IndexerDeclaration indexerDeclaration)
            : base(emitter, indexerDeclaration)
        {
            Emitter = emitter;
            IndexerDeclaration = indexerDeclaration;
        }

        public IndexerDeclaration IndexerDeclaration { get; set; }

        protected override void DoEmit()
        {
            EmitIndexerMethod(IndexerDeclaration, IndexerDeclaration.Getter, false);
            EmitIndexerMethod(IndexerDeclaration, IndexerDeclaration.Setter, true);
        }

        protected virtual void EmitIndexerMethod(IndexerDeclaration indexerDeclaration, Accessor accessor, bool setter)
        {
            if (!accessor.IsNull && Emitter.GetInline(accessor) == null)
            {
                var memberResult = Emitter.Resolver.ResolveNode(IndexerDeclaration) as MemberResolveResult;
                var isInterface = memberResult.Member.DeclaringType.Kind == TypeKind.Interface;
                var ignoreInterface = isInterface &&
                                      memberResult.Member.DeclaringType.TypeParameterCount > 0;

                WriteAccessor(indexerDeclaration, setter, ignoreInterface);

                if (!ignoreInterface && isInterface)
                {
                    WriteAccessor(indexerDeclaration, setter, true);
                }
            }
        }

        private void WriteAccessor(IndexerDeclaration indexerDeclaration, bool setter, bool ignoreInterface)
        {
            XmlToJsDoc.EmitComment(this, IndexerDeclaration, !setter);
            string name = Helpers.GetPropertyRef(IndexerDeclaration, Emitter, setter, false, ignoreInterface);
            Write(name);

            EmitMethodParameters(indexerDeclaration.Parameters, null, indexerDeclaration, setter);

            if (setter)
            {
                Write(", value");
                WriteColon();
                name = H5Types.ToTypeScriptName(indexerDeclaration.ReturnType, Emitter);
                Write(name);
                WriteCloseParentheses();
                WriteColon();
                Write("void");
            }
            else
            {
                WriteColon();
                name = H5Types.ToTypeScriptName(indexerDeclaration.ReturnType, Emitter);
                Write(name);
            }

            WriteSemiColon();
            WriteNewLine();
        }

        protected virtual void EmitMethodParameters(IEnumerable<ParameterDeclaration> declarations, IEnumerable<TypeParameterDeclaration> typeParamsdeclarations, AstNode context, bool skipClose)
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

            if (!skipClose)
            {
                WriteCloseParentheses();
            }
        }
    }
}