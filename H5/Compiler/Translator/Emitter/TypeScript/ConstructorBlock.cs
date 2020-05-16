using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using Object.Net.Utilities;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.TypeSystem;

namespace H5.Translator.TypeScript
{
    public partial class ConstructorBlock : TypeScriptBlock
    {
        public ConstructorBlock(IEmitter emitter, ITypeInfo typeInfo)
            : base(emitter, typeInfo.TypeDeclaration)
        {
            Emitter = emitter;
            TypeInfo = typeInfo;
        }

        public ITypeInfo TypeInfo { get; set; }

        protected override void DoEmit()
        {
            EmitCtorForInstantiableClass();
        }

        protected virtual void EmitCtorForInstantiableClass()
        {
            var typeDef = Emitter.GetTypeDefinition();
            string name = Emitter.Validator.GetCustomTypeName(typeDef, Emitter, true, false);

            if (name.IsEmpty())
            {
                name = H5Types.ToTypeScriptName(TypeInfo.Type, Emitter, false, true);
            }

            if (TypeInfo.Ctors.Count == 0)
            {
                Write("new ");
                WriteOpenCloseParentheses();
                WriteColon();
                Write(name);
                WriteSemiColon();
                WriteNewLine();
            }
            else if (TypeInfo.Ctors.Count == 1)
            {
                var ctor = TypeInfo.Ctors.First();
                if (!ctor.HasModifier(Modifiers.Public))
                {
                    return;
                }

                XmlToJsDoc.EmitComment(this, ctor);

                Write("new ");
                EmitMethodParameters(ctor.Parameters, ctor);
                WriteColon();
                Write(name);
                WriteSemiColon();
                WriteNewLine();
            }
            else
            {
                foreach (var ctor in TypeInfo.Ctors)
                {
                    if (!ctor.HasModifier(Modifiers.Public))
                    {
                        continue;
                    }

                    if (ctor.Parameters.Count == 0)
                    {
                        XmlToJsDoc.EmitComment(this, ctor);
                        Write("new ()");
                        WriteColon();
                        Write(name);
                        WriteSemiColon();
                        WriteNewLine();
                    }

                    XmlToJsDoc.EmitComment(this, ctor);
                    var ctorName = JS.Funcs.CONSTRUCTOR;

                    if (TypeInfo.Ctors.Count > 1 && ctor.Parameters.Count > 0)
                    {
                        var overloads = OverloadsCollection.Create(Emitter, ctor);
                        ctorName = overloads.GetOverloadName();
                    }

                    Write(ctorName);
                    WriteColon();
                    BeginBlock();

                    WriteNew();
                    EmitMethodParameters(ctor.Parameters, ctor);
                    WriteColon();

                    Write(name);
                    WriteNewLine();
                    EndBlock();

                    WriteSemiColon();
                    WriteNewLine();
                }
            }
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

                var resolveResult = Emitter.Resolver.ResolveNode(p.Type);
                if (resolveResult != null && (resolveResult.Type.IsReferenceType.HasValue && resolveResult.Type.IsReferenceType.Value || resolveResult.Type.IsKnownType(KnownTypeCode.NullableOfT)))
                {
                    Write(" | null");
                }
            }

            WriteCloseParentheses();
        }
    }
}