using ICSharpCode.NRefactory.CSharp;

namespace Bridge.Translator
{
    public abstract partial class Visitor : IAstVisitor
    {
        public virtual void VisitAttributeSection(AttributeSection attributeSection)
        {
            //throw this.CreateException(attributeSection);
        }

        public virtual void VisitCSharpTokenNode(CSharpTokenNode cSharpTokenNode)
        {
            //throw this.CreateException(cSharpTokenNode);
        }

        public virtual void VisitComment(Comment comment)
        {
            //throw this.CreateException(comment);
        }

        public virtual void VisitDefaultValueExpression(DefaultValueExpression defaultValueExpression)
        {
            //throw this.CreateException(defaultValueExpression);
        }

        public virtual void VisitIdentifier(Identifier identifier)
        {
            //throw this.CreateException(identifier);
        }

        public virtual void VisitNullReferenceExpression(NullReferenceExpression nullReferenceExpression)
        {
            //throw this.CreateException(nullReferenceExpression);
        }

        public virtual void VisitPreProcessorDirective(PreProcessorDirective preProcessorDirective)
        {
            //throw this.CreateException(preProcessorDirective);
        }

        public virtual void VisitTypeParameterDeclaration(TypeParameterDeclaration typeParameterDeclaration)
        {
            //throw this.CreateException(typeParameterDeclaration);
        }

        public virtual void VisitPrimitiveType(PrimitiveType primitiveType)
        {
            //throw this.CreateException(primitiveType);
        }

        public virtual void VisitSimpleType(SimpleType simpleType)
        {
            //throw this.CreateException(simpleType);
        }

        public virtual void VisitNullNode(AstNode nullNode)
        {
            //throw this.CreateException(nullNode);
        }

        public virtual void VisitNewLine(NewLineNode newLineNode)
        {
            //throw this.CreateException(newLineNode);
        }

        public virtual void VisitConstraint(Constraint constraint)
        {
        }

        public virtual void VisitOperatorDeclaration(OperatorDeclaration operatorDeclaration)
        {
            /*if (this.ThrowException)
            {
                throw (Exception)this.CreateException(operatorDeclaration);
            }*/
        }

        public virtual void VisitMemberType(MemberType memberType)
        {
            /*if (this.ThrowException)
            {
                throw (Exception)this.CreateException(memberType);
            }*/
        }

        public virtual void VisitUsingAliasDeclaration(UsingAliasDeclaration usingAliasDeclaration)
        {
        }
    }
}