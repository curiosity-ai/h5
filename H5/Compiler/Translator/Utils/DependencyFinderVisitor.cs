using H5.Contract;
using ICSharpCode.NRefactory.CSharp;
using System.Collections.Generic;
using System.Linq;

namespace H5.Translator
{
    public class DependencyFinderVisitor : DepthFirstAstVisitor
    {
        public DependencyFinderVisitor(IEmitter emitter, ITypeInfo type)
        {
            this.Emitter = emitter;
            this.Type = type;
            this.Dependencies = new List<ITypeInfo>();
        }

        public override void VisitSimpleType(SimpleType simpleType)
        {
            this.CheckDependency(simpleType);
            base.VisitSimpleType(simpleType);
        }

        public override void VisitMemberReferenceExpression(MemberReferenceExpression memberReferenceExpression)
        {
            this.CheckDependency(memberReferenceExpression.Target);
            base.VisitMemberReferenceExpression(memberReferenceExpression);
        }

        public override void VisitIdentifierExpression(IdentifierExpression identifierExpression)
        {
            this.CheckDependency(identifierExpression);
            base.VisitIdentifierExpression(identifierExpression);
        }

        public override void VisitObjectCreateExpression(ObjectCreateExpression objectCreateExpression)
        {
            this.CheckDependency(objectCreateExpression.Type);
            base.VisitObjectCreateExpression(objectCreateExpression);
        }

        public override void VisitConstructorDeclaration(ConstructorDeclaration methodDeclaration)
        {
            if (methodDeclaration.HasModifier(Modifiers.Static))
            {
                base.VisitConstructorDeclaration(methodDeclaration);
            }
        }

        public override void VisitMethodDeclaration(MethodDeclaration methodDeclaration)
        {
            if (methodDeclaration.HasModifier(Modifiers.Static))
            {
                this.CheckDependency(methodDeclaration.ReturnType);
                base.VisitMethodDeclaration(methodDeclaration);
            }
        }

        public override void VisitFieldDeclaration(FieldDeclaration fieldDeclaration)
        {
            if (fieldDeclaration.HasModifier(Modifiers.Static))
            {
                this.CheckDependency(fieldDeclaration.ReturnType);
                base.VisitFieldDeclaration(fieldDeclaration);
            }
        }

        public override void VisitPropertyDeclaration(PropertyDeclaration propertyDeclaration)
        {
            if (propertyDeclaration.HasModifier(Modifiers.Static))
            {
                this.CheckDependency(propertyDeclaration.ReturnType);
                if (!propertyDeclaration.Getter.IsNull)
                {
                    propertyDeclaration.Getter.AcceptVisitor(this);
                }
            }
        }

        public override void VisitAccessor(Accessor accessor)
        {
            var prop = accessor.GetParent<PropertyDeclaration>();
            if (prop != null && prop.HasModifier(Modifiers.Static))
            {
                this.CheckDependency(prop.ReturnType);
                base.VisitAccessor(accessor);
            }
        }

        public override void VisitFixedFieldDeclaration(FixedFieldDeclaration fixedFieldDeclaration)
        {
            if (fixedFieldDeclaration.HasModifier(Modifiers.Static))
            {
                this.CheckDependency(fixedFieldDeclaration.ReturnType);
                base.VisitFixedFieldDeclaration(fixedFieldDeclaration);
            }
        }

        public void CheckDependency(AstNode node)
        {
            var rr = this.Emitter.Resolver.ResolveNode(node, this.Emitter);

            if (!rr.IsError)
            {
                var typeInfo = this.Emitter.H5Types.Get(rr.Type, true);

                if (typeInfo != null && typeInfo.TypeInfo != null && typeInfo.Type.FullName != this.Type.Type.FullName && this.Dependencies.All(d => d.Type.FullName != typeInfo.TypeInfo.Type.FullName))
                {
                    this.Dependencies.Add(typeInfo.TypeInfo);
                }
            }
        }

        public IEmitter Emitter { get; set; }

        public List<ITypeInfo> Dependencies { get; set; }

        public ITypeInfo Type { get; set; }
    }
}