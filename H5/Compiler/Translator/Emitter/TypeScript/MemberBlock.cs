using H5.Contract;

using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using System.Linq;

namespace H5.Translator.TypeScript
{
    public class MemberBlock : TypeScriptBlock
    {
        public MemberBlock(IEmitter emitter, ITypeInfo typeInfo, bool staticBlock)
            : base(emitter, typeInfo.TypeDeclaration)
        {
            Emitter = emitter;
            TypeInfo = typeInfo;
            StaticBlock = staticBlock;
        }

        public ITypeInfo TypeInfo { get; set; }

        public bool StaticBlock { get; set; }

        protected override void DoEmit()
        {
            EmitFields(StaticBlock ? TypeInfo.StaticConfig : TypeInfo.InstanceConfig);
        }

        protected virtual void EmitFields(TypeConfigInfo info)
        {
            if (info.Fields.Count > 0)
            {
                foreach (var field in info.Fields)
                {
                    if (field.Entity.HasModifier(Modifiers.Public) || TypeInfo.IsEnum)
                    {
                        if (field.Entity is FieldDeclaration fieldDecl)
                        {
                            foreach (var variableInitializer in fieldDecl.Variables)
                            {
                                WriteFieldDeclaration(field, variableInitializer);
                            }
                        }
                        else
                        {
                            WriteFieldDeclaration(field, null);
                        }
                    }
                }
            }

            if (info.Events.Count > 0)
            {
                foreach (var ev in info.Events)
                {
                    if (ev.Entity.HasModifier(Modifiers.Public) || TypeInfo.Type.Kind == TypeKind.Interface)
                    {
                        if (Emitter.Resolver.ResolveNode(ev.VarInitializer) is MemberResolveResult memberResult)
                        {
                            var ignoreInterface = memberResult.Member.DeclaringType.Kind == TypeKind.Interface &&
                                      memberResult.Member.DeclaringType.TypeParameterCount > 0;

                            WriteEvent(ev, Helpers.GetEventRef(memberResult.Member, Emitter, false, ignoreInterface: ignoreInterface), true);
                            WriteEvent(ev, Helpers.GetEventRef(memberResult.Member, Emitter, true, ignoreInterface: ignoreInterface), false);

                            if (!ignoreInterface && TypeInfo.Type.Kind == TypeKind.Interface)
                            {
                                WriteEvent(ev, Helpers.GetEventRef(memberResult.Member, Emitter, false, ignoreInterface: true), true);
                                WriteEvent(ev, Helpers.GetEventRef(memberResult.Member, Emitter, true, ignoreInterface: true), false);
                            }
                        }
                        else
                        {
                            var name = ev.GetName(Emitter);
                            name = Helpers.ReplaceFirstDollar(name);

                            WriteEvent(ev, Helpers.GetAddOrRemove(true, name), true);
                            WriteEvent(ev, Helpers.GetAddOrRemove(false, name), false);
                        }
                    }
                }
            }

            /*if (info.Properties.Count > 0)
            {
                foreach (var prop in info.Properties)
                {
                    if (prop.Entity.HasModifier(Modifiers.Public))
                    {
                        var name = prop.GetName(this.Emitter);
                        name = Helpers.ReplaceFirstDollar(name);

                        this.WriteProp(prop, name);
                    }
                }
            }*/

            new MethodsBlock(Emitter, TypeInfo, StaticBlock).Emit();
        }

        private void WriteFieldDeclaration(TypeConfigItem field, VariableInitializer variableInitializer)
        {
            XmlToJsDoc.EmitComment(this, field.Entity, null, variableInitializer);

            if (TypeInfo.IsEnum)
            {
                Write(EnumBlock.GetEnumItemName(Emitter, field));
            }
            else
            {
                Write(field.GetName(Emitter));
            }

            if (field.VarInitializer != null)
            {
                var field_rr = Emitter.Resolver.ResolveNode(field.VarInitializer);
                if (field_rr is MemberResolveResult mrr && mrr.Member.Attributes.Any(a => a.AttributeType.FullName == "H5.OptionalAttribute"))
                {
                    Write("?");
                }
            }

            WriteColon();

            string typeName = TypeInfo.IsEnum
                ? (Helpers.IsStringNameEnum(TypeInfo.Type) ? "string" : "number")
                : H5Types.ToTypeScriptName(field.Entity.ReturnType, Emitter);
            Write(typeName);

            if (!TypeInfo.IsEnum)
            {
                var resolveResult = Emitter.Resolver.ResolveNode(field.Entity.ReturnType);
                if (resolveResult != null && (resolveResult.Type.IsReferenceType.HasValue && resolveResult.Type.IsReferenceType.Value || resolveResult.Type.IsKnownType(KnownTypeCode.NullableOfT)))
                {
                    Write(" | null");
                }
            }

            WriteSemiColon();
            WriteNewLine();
        }

        private void WriteEvent(TypeConfigItem ev, string name, bool adder)
        {
            XmlToJsDoc.EmitComment(this, ev.Entity, adder);
            Write(name);
            WriteOpenParentheses();
            Write("value");
            WriteColon();
            string typeName = H5Types.ToTypeScriptName(ev.Entity.ReturnType, Emitter);
            Write(typeName);

            var resolveResult = Emitter.Resolver.ResolveNode(ev.Entity.ReturnType);
            if (resolveResult != null && (resolveResult.Type.IsReferenceType.HasValue && resolveResult.Type.IsReferenceType.Value || resolveResult.Type.IsKnownType(KnownTypeCode.NullableOfT)))
            {
                Write(" | null");
            }

            WriteCloseParentheses();
            WriteColon();
            Write("void");

            WriteSemiColon();
            WriteNewLine();
        }

        private void WriteProp(TypeConfigItem ev, string name)
        {
            XmlToJsDoc.EmitComment(this, ev.Entity);
            Write(name);
            WriteColon();

            string typeName = H5Types.ToTypeScriptName(ev.Entity.ReturnType, Emitter);
            Write(typeName);

            var resolveResult = Emitter.Resolver.ResolveNode(ev.Entity.ReturnType);
            if (resolveResult != null && (resolveResult.Type.IsReferenceType.HasValue && resolveResult.Type.IsReferenceType.Value || resolveResult.Type.IsKnownType(KnownTypeCode.NullableOfT)))
            {
                Write(" | null");
            }

            WriteSemiColon();
            WriteNewLine();
        }
    }
}