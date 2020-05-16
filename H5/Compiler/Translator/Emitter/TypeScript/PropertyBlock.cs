using H5.Contract;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.Semantics;
using System.Linq;
using ICSharpCode.NRefactory.TypeSystem;

namespace H5.Translator.TypeScript
{
    public class PropertyBlock : TypeScriptBlock
    {
        public PropertyBlock(IEmitter emitter, PropertyDeclaration propertyDeclaration)
            : base(emitter, propertyDeclaration)
        {
            Emitter = emitter;
            PropertyDeclaration = propertyDeclaration;
        }

        public PropertyDeclaration PropertyDeclaration { get; set; }

        protected override void DoEmit()
        {
            EmitPropertyMethod(PropertyDeclaration);
        }

        protected virtual void EmitPropertyMethod(PropertyDeclaration propertyDeclaration)
        {
            var memberResult = Emitter.Resolver.ResolveNode(propertyDeclaration) as MemberResolveResult;
            var isInterface = memberResult != null && memberResult.Member.DeclaringType.Kind == TypeKind.Interface;

            if (!isInterface && !propertyDeclaration.HasModifier(Modifiers.Public))
            {
                return;
            }

            if (memberResult != null &&
                propertyDeclaration.Getter.IsNull && propertyDeclaration.Setter.IsNull)
            {
                return;
            }

            if (!propertyDeclaration.Getter.IsNull && Emitter.GetInline(propertyDeclaration.Getter) == null)
            {
                XmlToJsDoc.EmitComment(this, PropertyDeclaration);

                var ignoreInterface = isInterface &&
                                      memberResult.Member.DeclaringType.TypeParameterCount > 0;
                WriteAccessor(propertyDeclaration, memberResult, ignoreInterface);

                if (!ignoreInterface && isInterface)
                {
                    WriteAccessor(propertyDeclaration, memberResult, true);
                }
            }
        }

        private void WriteAccessor(PropertyDeclaration p, MemberResolveResult memberResult, bool ignoreInterface)
        {
            string name = Helpers.GetPropertyRef(memberResult.Member, Emitter, false, false, ignoreInterface);
            Write(name);

            var property_rr = Emitter.Resolver.ResolveNode(p);
            if (property_rr is MemberResolveResult mrr && mrr.Member.Attributes.Any(a => a.AttributeType.FullName == "H5.OptionalAttribute"))
            {
                Write("?");
            }

            WriteColon();
            name = H5Types.ToTypeScriptName(p.ReturnType, Emitter);
            Write(name);

            var resolveResult = Emitter.Resolver.ResolveNode(p.ReturnType);
            if (resolveResult != null && (resolveResult.Type.IsReferenceType.HasValue && resolveResult.Type.IsReferenceType.Value || resolveResult.Type.IsKnownType(KnownTypeCode.NullableOfT)))
            {
                Write(" | null");
            }

            WriteSemiColon();
            WriteNewLine();
        }
    }
}