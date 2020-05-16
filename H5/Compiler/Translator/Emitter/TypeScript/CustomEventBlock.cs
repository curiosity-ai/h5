using H5.Contract;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;

namespace H5.Translator.TypeScript
{
    public class CustomEventBlock : TypeScriptBlock
    {
        public CustomEventBlock(IEmitter emitter, CustomEventDeclaration customEventDeclaration)
            : base(emitter, customEventDeclaration)
        {
            Emitter = emitter;
            CustomEventDeclaration = customEventDeclaration;
        }

        public CustomEventDeclaration CustomEventDeclaration { get; set; }

        protected override void DoEmit()
        {
            EmitPropertyMethod(CustomEventDeclaration, CustomEventDeclaration.AddAccessor, false);
            EmitPropertyMethod(CustomEventDeclaration, CustomEventDeclaration.RemoveAccessor, true);
        }

        protected virtual void EmitPropertyMethod(CustomEventDeclaration customEventDeclaration, Accessor accessor, bool remover)
        {
            if (!accessor.IsNull && Emitter.GetInline(accessor) == null)
            {
                XmlToJsDoc.EmitComment(this, customEventDeclaration);
                var memberResult = Emitter.Resolver.ResolveNode(customEventDeclaration, Emitter) as MemberResolveResult;
                var ignoreInterface = memberResult.Member.DeclaringType.Kind == TypeKind.Interface &&
                                          memberResult.Member.DeclaringType.TypeParameterCount > 0;
                Write(Helpers.GetEventRef(customEventDeclaration, Emitter, remover, false, ignoreInterface));
                WriteOpenParentheses();
                Write("value");
                WriteColon();
                var retType = H5Types.ToTypeScriptName(customEventDeclaration.ReturnType, Emitter);
                Write(retType);
                WriteCloseParentheses();
                WriteColon();
                Write("void");

                WriteSemiColon();
                WriteNewLine();
            }
        }
    }
}