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
            this.Emitter = emitter;
            this.TypeInfo = typeInfo;
            this.StaticBlock = staticBlock;
        }

        public ITypeInfo TypeInfo
        {
            get;
            set;
        }

        public bool StaticBlock
        {
            get;
            set;
        }

        protected override void DoEmit()
        {
            this.EmitFields(this.StaticBlock ? this.TypeInfo.StaticConfig : this.TypeInfo.InstanceConfig);
        }

        protected virtual void EmitFields(TypeConfigInfo info)
        {
            if (info.Fields.Count > 0)
            {
                foreach (var field in info.Fields)
                {
                    if (field.Entity.HasModifier(Modifiers.Public) || this.TypeInfo.IsEnum)
                    {
                        if (field.Entity is FieldDeclaration fieldDecl)
                        {
                            foreach (var variableInitializer in fieldDecl.Variables)
                            {
                                this.WriteFieldDeclaration(field, variableInitializer);
                            }
                        }
                        else
                        {
                            this.WriteFieldDeclaration(field, null);
                        }
                    }
                }
            }

            if (info.Events.Count > 0)
            {
                foreach (var ev in info.Events)
                {
                    if (ev.Entity.HasModifier(Modifiers.Public) || this.TypeInfo.Type.Kind == TypeKind.Interface)
                    {
                        if (this.Emitter.Resolver.ResolveNode(ev.VarInitializer, this.Emitter) is MemberResolveResult memberResult)
                        {
                            var ignoreInterface = memberResult.Member.DeclaringType.Kind == TypeKind.Interface &&
                                      memberResult.Member.DeclaringType.TypeParameterCount > 0;

                            this.WriteEvent(ev, Helpers.GetEventRef(memberResult.Member, this.Emitter, false, ignoreInterface: ignoreInterface), true);
                            this.WriteEvent(ev, Helpers.GetEventRef(memberResult.Member, this.Emitter, true, ignoreInterface: ignoreInterface), false);

                            if (!ignoreInterface && this.TypeInfo.Type.Kind == TypeKind.Interface)
                            {
                                this.WriteEvent(ev, Helpers.GetEventRef(memberResult.Member, this.Emitter, false, ignoreInterface: true), true);
                                this.WriteEvent(ev, Helpers.GetEventRef(memberResult.Member, this.Emitter, true, ignoreInterface: true), false);
                            }
                        }
                        else
                        {
                            var name = ev.GetName(this.Emitter);
                            name = Helpers.ReplaceFirstDollar(name);

                            this.WriteEvent(ev, Helpers.GetAddOrRemove(true, name), true);
                            this.WriteEvent(ev, Helpers.GetAddOrRemove(false, name), false);
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

            new MethodsBlock(this.Emitter, this.TypeInfo, this.StaticBlock).Emit();
        }

        private void WriteFieldDeclaration(TypeConfigItem field, VariableInitializer variableInitializer)
        {
            XmlToJsDoc.EmitComment(this, field.Entity, null, variableInitializer);

            if (this.TypeInfo.IsEnum)
            {
                this.Write(EnumBlock.GetEnumItemName(this.Emitter, field));
            }
            else
            {
                this.Write(field.GetName(this.Emitter));
            }

            if (field.VarInitializer != null)
            {
                var field_rr = this.Emitter.Resolver.ResolveNode(field.VarInitializer, this.Emitter);
                if (field_rr is MemberResolveResult mrr && mrr.Member.Attributes.Any(a => a.AttributeType.FullName == "H5.OptionalAttribute"))
                {
                    this.Write("?");
                }
            }

            this.WriteColon();

            string typeName = this.TypeInfo.IsEnum
                ? (Helpers.IsStringNameEnum(this.TypeInfo.Type) ? "string" : "number")
                : H5Types.ToTypeScriptName(field.Entity.ReturnType, this.Emitter);
            this.Write(typeName);

            if (!this.TypeInfo.IsEnum)
            {
                var resolveResult = this.Emitter.Resolver.ResolveNode(field.Entity.ReturnType, this.Emitter);
                if (resolveResult != null && (resolveResult.Type.IsReferenceType.HasValue && resolveResult.Type.IsReferenceType.Value || resolveResult.Type.IsKnownType(KnownTypeCode.NullableOfT)))
                {
                    this.Write(" | null");
                }
            }

            this.WriteSemiColon();
            this.WriteNewLine();
        }

        private void WriteEvent(TypeConfigItem ev, string name, bool adder)
        {
            XmlToJsDoc.EmitComment(this, ev.Entity, adder);
            this.Write(name);
            this.WriteOpenParentheses();
            this.Write("value");
            this.WriteColon();
            string typeName = H5Types.ToTypeScriptName(ev.Entity.ReturnType, this.Emitter);
            this.Write(typeName);

            var resolveResult = this.Emitter.Resolver.ResolveNode(ev.Entity.ReturnType, this.Emitter);
            if (resolveResult != null && (resolveResult.Type.IsReferenceType.HasValue && resolveResult.Type.IsReferenceType.Value || resolveResult.Type.IsKnownType(KnownTypeCode.NullableOfT)))
            {
                this.Write(" | null");
            }

            this.WriteCloseParentheses();
            this.WriteColon();
            this.Write("void");

            this.WriteSemiColon();
            this.WriteNewLine();
        }

        private void WriteProp(TypeConfigItem ev, string name)
        {
            XmlToJsDoc.EmitComment(this, ev.Entity);
            this.Write(name);
            this.WriteColon();

            string typeName = H5Types.ToTypeScriptName(ev.Entity.ReturnType, this.Emitter);
            this.Write(typeName);

            var resolveResult = this.Emitter.Resolver.ResolveNode(ev.Entity.ReturnType, this.Emitter);
            if (resolveResult != null && (resolveResult.Type.IsReferenceType.HasValue && resolveResult.Type.IsReferenceType.Value || resolveResult.Type.IsKnownType(KnownTypeCode.NullableOfT)))
            {
                this.Write(" | null");
            }

            this.WriteSemiColon();
            this.WriteNewLine();
        }
    }
}