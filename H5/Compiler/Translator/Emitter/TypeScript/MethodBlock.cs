using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using System.Collections.Generic;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;

namespace H5.Translator.TypeScript
{
    public class MethodBlock : TypeScriptBlock
    {
        public MethodBlock(IEmitter emitter, MethodDeclaration methodDeclaration)
            : base(emitter, methodDeclaration)
        {
            Emitter = emitter;
            MethodDeclaration = methodDeclaration;
        }

        public MethodDeclaration MethodDeclaration { get; set; }

        protected override void DoEmit()
        {
            VisitMethodDeclaration(MethodDeclaration);
        }

        protected void VisitMethodDeclaration(MethodDeclaration methodDeclaration)
        {
            XmlToJsDoc.EmitComment(this, MethodDeclaration);
            var overloads = OverloadsCollection.Create(Emitter, methodDeclaration);
            var memberResult = Emitter.Resolver.ResolveNode(methodDeclaration) as MemberResolveResult;
            var isInterface = memberResult.Member.DeclaringType.Kind == TypeKind.Interface;
            var ignoreInterface = isInterface &&
                                      memberResult.Member.DeclaringType.TypeParameterCount > 0;
            WriteSignature(methodDeclaration, overloads, ignoreInterface, isInterface);
            if (!ignoreInterface && isInterface)
            {
                WriteSignature(methodDeclaration, overloads, true, isInterface);
            }
        }

        private void WriteSignature(MethodDeclaration methodDeclaration, OverloadsCollection overloads, bool ignoreInterface, bool isInterface)
        {
            if (!isInterface && !methodDeclaration.HasModifier(Modifiers.Public))
            {
                return;
            }

            string name = overloads.GetOverloadName(ignoreInterface);
            Write(name);

            bool needComma = false;
            var isGeneric = methodDeclaration.TypeParameters.Count > 0;
            if (isGeneric)
            {
                Write("<");
                foreach (var p in methodDeclaration.TypeParameters)
                {
                    if (needComma)
                    {
                        WriteComma();
                    }

                    needComma = true;
                    Write(p.Name);
                }
                Write(">");

                WriteOpenParentheses();

                var comma = false;
                foreach (var p in methodDeclaration.TypeParameters)
                {
                    if (comma)
                    {
                        WriteComma();
                    }
                    Write(p.Name);
                    WriteColon();
                    WriteOpenBrace();
                    Write(JS.Fields.PROTOTYPE);
                    WriteColon();
                    Write(p.Name);

                    WriteCloseBrace();
                    comma = true;
                }
            }
            else
            {
                WriteOpenParentheses();
            }

            if (needComma && methodDeclaration.Parameters.Count > 0)
            {
                WriteComma();
            }

            EmitMethodParameters(methodDeclaration.Parameters, methodDeclaration);

            WriteCloseParentheses();

            WriteColon();

            var retType = H5Types.ToTypeScriptName(methodDeclaration.ReturnType, Emitter);
            Write(retType);

            var resolveResult = Emitter.Resolver.ResolveNode(methodDeclaration.ReturnType);
            if (resolveResult != null && (resolveResult.Type.IsReferenceType.HasValue && resolveResult.Type.IsReferenceType.Value || resolveResult.Type.IsKnownType(KnownTypeCode.NullableOfT)))
            {
                Write(" | null");
            }

            WriteSemiColon();
            WriteNewLine();
        }

        protected virtual void EmitMethodParameters(IEnumerable<ParameterDeclaration> declarations, AstNode context)
        {
            bool needComma = false;

            foreach (var p in declarations)
            {
                var name = Emitter.GetParameterName(p);
                bool optional = p.DefaultExpression != null && !p.DefaultExpression.IsNull;

                if (needComma)
                {
                    WriteComma();
                }

                needComma = true;
                Write(name);

                if (optional)
                {
                    Write("?");
                }

                WriteColon();
                name = H5Types.ToTypeScriptName(p.Type, Emitter);

                var resolveResult = Emitter.Resolver.ResolveNode(p.Type);
                if (resolveResult != null && (resolveResult.Type.IsReferenceType.HasValue && resolveResult.Type.IsReferenceType.Value || resolveResult.Type.IsKnownType(KnownTypeCode.NullableOfT)))
                {
                    name += " | null";
                }

                if (p.ParameterModifier == ParameterModifier.Out || p.ParameterModifier == ParameterModifier.Ref)
                {
                    name = "{v: " + name + "}";
                }
                Write(name);
            }
        }
    }
}