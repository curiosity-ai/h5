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
            Emitter = emitter;
            Type = type;
            Dependencies = new List<ITypeInfo>();
        }

        public override void VisitSimpleType(SimpleType simpleType)
        {
            CheckDependency(simpleType);
            base.VisitSimpleType(simpleType);
        }

        public override void VisitMemberReferenceExpression(MemberReferenceExpression memberReferenceExpression)
        {
            CheckDependency(memberReferenceExpression.Target);
            base.VisitMemberReferenceExpression(memberReferenceExpression);
        }

        public override void VisitIdentifierExpression(IdentifierExpression identifierExpression)
        {
            CheckDependency(identifierExpression);
            base.VisitIdentifierExpression(identifierExpression);
        }

        public override void VisitObjectCreateExpression(ObjectCreateExpression objectCreateExpression)
        {
            CheckDependency(objectCreateExpression.Type);
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
                CheckDependency(methodDeclaration.ReturnType);
                base.VisitMethodDeclaration(methodDeclaration);
            }
        }

        public override void VisitFieldDeclaration(FieldDeclaration fieldDeclaration)
        {
            if (fieldDeclaration.HasModifier(Modifiers.Static))
            {
                CheckDependency(fieldDeclaration.ReturnType);
                base.VisitFieldDeclaration(fieldDeclaration);
            }
        }

        public override void VisitPropertyDeclaration(PropertyDeclaration propertyDeclaration)
        {
            if (propertyDeclaration.HasModifier(Modifiers.Static))
            {
                CheckDependency(propertyDeclaration.ReturnType);
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
                CheckDependency(prop.ReturnType);
                base.VisitAccessor(accessor);
            }
        }

        public override void VisitFixedFieldDeclaration(FixedFieldDeclaration fixedFieldDeclaration)
        {
            if (fixedFieldDeclaration.HasModifier(Modifiers.Static))
            {
                CheckDependency(fixedFieldDeclaration.ReturnType);
                base.VisitFixedFieldDeclaration(fixedFieldDeclaration);
            }
        }

        public void CheckDependency(AstNode node)
        {
            var rr = Emitter.Resolver.ResolveNode(node, Emitter);

            if (!rr.IsError)
            {
                var typeInfo = Emitter.H5Types.Get(rr.Type, true);

                if (typeInfo != null && typeInfo.TypeInfo != null && typeInfo.Type.FullName != Type.Type.FullName && Dependencies.All(d => d.Type.FullName != typeInfo.TypeInfo.Type.FullName))
                {
                    Dependencies.Add(typeInfo.TypeInfo);
                }
            }
        }

        public IEmitter Emitter { get; set; }

        public List<ITypeInfo> Dependencies { get; set; }

        public ITypeInfo Type { get; set; }
    }
}