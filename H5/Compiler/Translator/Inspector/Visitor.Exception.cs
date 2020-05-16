using H5.Contract;
using ICSharpCode.NRefactory.CSharp;
using System;

namespace H5.Translator
{
    public abstract partial class Visitor : IAstVisitor
    {
        public virtual IVisitorException CreateException(AstNode node, string message)
        {
            if (String.IsNullOrEmpty(message))
            {
                message = String.Format("Language construction {0} is not supported", node.GetType().Name);
            }

            return new EmitterException(node, message);
        }

        public virtual IVisitorException CreateException(AstNode node)
        {
            return CreateException(node, null);
        }

        private bool throwException = true;

        public virtual bool ThrowException
        {
            get
            {
                return throwException;
            }
            set
            {
                throwException = value;
            }
        }

        public virtual void VisitAccessor(Accessor accessor)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(accessor);
            }
        }

        public virtual void VisitAnonymousMethodExpression(AnonymousMethodExpression anonymousMethodExpression)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(anonymousMethodExpression);
            }
        }

        public virtual void VisitAnonymousTypeCreateExpression(AnonymousTypeCreateExpression anonymousTypeCreateExpression)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(anonymousTypeCreateExpression);
            }
        }

        public virtual void VisitArrayCreateExpression(ArrayCreateExpression arrayCreateExpression)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(arrayCreateExpression);
            }
        }

        public virtual void VisitArrayInitializerExpression(ArrayInitializerExpression arrayInitializerExpression)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(arrayInitializerExpression);
            }
        }

        public virtual void VisitArraySpecifier(ArraySpecifier arraySpecifier)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(arraySpecifier);
            }
        }

        public virtual void VisitAsExpression(AsExpression asExpression)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(asExpression);
            }
        }

        public virtual void VisitAssignmentExpression(AssignmentExpression assignmentExpression)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(assignmentExpression);
            }
        }

        public virtual void VisitAttribute(ICSharpCode.NRefactory.CSharp.Attribute attribute)
        {
            throw new NotImplementedException();
        }

        public virtual void VisitBaseReferenceExpression(BaseReferenceExpression baseReferenceExpression)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(baseReferenceExpression);
            }
        }

        public virtual void VisitBinaryOperatorExpression(BinaryOperatorExpression binaryOperatorExpression)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(binaryOperatorExpression);
            }
        }

        public virtual void VisitBlockStatement(BlockStatement blockStatement)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(blockStatement);
            }
        }

        public virtual void VisitBreakStatement(BreakStatement breakStatement)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(breakStatement);
            }
        }

        public virtual void VisitCaseLabel(CaseLabel caseLabel)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(caseLabel);
            }
        }

        public virtual void VisitCastExpression(CastExpression castExpression)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(castExpression);
            }
        }

        public virtual void VisitCatchClause(CatchClause catchClause)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(catchClause);
            }
        }

        public virtual void VisitCheckedExpression(CheckedExpression checkedExpression)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(checkedExpression);
            }
        }

        public virtual void VisitCheckedStatement(CheckedStatement checkedStatement)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(checkedStatement);
            }
        }

        public virtual void VisitComposedType(ComposedType composedType)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(composedType);
            }
        }

        public virtual void VisitConditionalExpression(ConditionalExpression conditionalExpression)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(conditionalExpression);
            }
        }

        public virtual void VisitConstructorDeclaration(ConstructorDeclaration constructorDeclaration)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(constructorDeclaration);
            }
        }

        public virtual void VisitConstructorInitializer(ConstructorInitializer constructorInitializer)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(constructorInitializer);
            }
        }

        public virtual void VisitContinueStatement(ContinueStatement continueStatement)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(continueStatement);
            }
        }

        public virtual void VisitCustomEventDeclaration(CustomEventDeclaration customEventDeclaration)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(customEventDeclaration);
            }
        }

        public virtual void VisitDelegateDeclaration(DelegateDeclaration delegateDeclaration)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(delegateDeclaration);
            }
        }

        public virtual void VisitDestructorDeclaration(DestructorDeclaration destructorDeclaration)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(destructorDeclaration);
            }
        }

        public virtual void VisitDirectionExpression(DirectionExpression directionExpression)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(directionExpression);
            }
        }

        public virtual void VisitDoWhileStatement(DoWhileStatement doWhileStatement)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(doWhileStatement);
            }
        }

        public virtual void VisitDocumentationReference(DocumentationReference documentationReference)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(documentationReference);
            }
        }

        public virtual void VisitEmptyStatement(EmptyStatement emptyStatement)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(emptyStatement);
            }
        }

        public virtual void VisitEnumMemberDeclaration(EnumMemberDeclaration enumMemberDeclaration)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(enumMemberDeclaration);
            }
        }

        public virtual void VisitEventDeclaration(EventDeclaration eventDeclaration)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(eventDeclaration);
            }
        }

        public virtual void VisitExpressionStatement(ExpressionStatement expressionStatement)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(expressionStatement);
            }
        }

        public virtual void VisitExternAliasDeclaration(ExternAliasDeclaration externAliasDeclaration)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(externAliasDeclaration);
            }
        }

        public virtual void VisitFieldDeclaration(FieldDeclaration fieldDeclaration)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(fieldDeclaration);
            }
        }

        public virtual void VisitFixedFieldDeclaration(FixedFieldDeclaration fixedFieldDeclaration)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(fixedFieldDeclaration);
            }
        }

        public virtual void VisitFixedStatement(FixedStatement fixedStatement)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(fixedStatement);
            }
        }

        public virtual void VisitFixedVariableInitializer(FixedVariableInitializer fixedVariableInitializer)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(fixedVariableInitializer);
            }
        }

        public virtual void VisitForStatement(ForStatement forStatement)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(forStatement);
            }
        }

        public virtual void VisitForeachStatement(ForeachStatement foreachStatement)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(foreachStatement);
            }
        }

        public virtual void VisitGotoCaseStatement(GotoCaseStatement gotoCaseStatement)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(gotoCaseStatement);
            }
        }

        public virtual void VisitGotoDefaultStatement(GotoDefaultStatement gotoDefaultStatement)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(gotoDefaultStatement);
            }
        }

        public virtual void VisitGotoStatement(GotoStatement gotoStatement)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(gotoStatement);
            }
        }

        public virtual void VisitIdentifierExpression(IdentifierExpression identifierExpression)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(identifierExpression);
            }
        }

        public virtual void VisitIfElseStatement(IfElseStatement ifElseStatement)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(ifElseStatement);
            }
        }

        public virtual void VisitIndexerDeclaration(IndexerDeclaration indexerDeclaration)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(indexerDeclaration);
            }
        }

        public virtual void VisitIndexerExpression(IndexerExpression indexerExpression)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(indexerExpression);
            }
        }

        public virtual void VisitInvocationExpression(InvocationExpression invocationExpression)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(invocationExpression);
            }
        }

        public virtual void VisitIsExpression(IsExpression isExpression)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(isExpression);
            }
        }

        public virtual void VisitLabelStatement(LabelStatement labelStatement)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(labelStatement);
            }
        }

        public virtual void VisitLambdaExpression(LambdaExpression lambdaExpression)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(lambdaExpression);
            }
        }

        public virtual void VisitLockStatement(LockStatement lockStatement)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(lockStatement);
            }
        }

        public virtual void VisitMemberReferenceExpression(MemberReferenceExpression memberReferenceExpression)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(memberReferenceExpression);
            }
        }

        public virtual void VisitMethodDeclaration(MethodDeclaration methodDeclaration)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(methodDeclaration);
            }
        }

        public virtual void VisitNamedArgumentExpression(NamedArgumentExpression namedArgumentExpression)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(namedArgumentExpression);
            }
        }

        public virtual void VisitNamedExpression(NamedExpression namedExpression)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(namedExpression);
            }
        }

        public virtual void VisitNamespaceDeclaration(NamespaceDeclaration namespaceDeclaration)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(namespaceDeclaration);
            }
        }

        public virtual void VisitObjectCreateExpression(ObjectCreateExpression objectCreateExpression)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(objectCreateExpression);
            }
        }

        public virtual void VisitParameterDeclaration(ParameterDeclaration parameterDeclaration)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(parameterDeclaration);
            }
        }

        public virtual void VisitParenthesizedExpression(ParenthesizedExpression parenthesizedExpression)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(parenthesizedExpression);
            }
        }

        public virtual void VisitPatternPlaceholder(AstNode placeholder, ICSharpCode.NRefactory.PatternMatching.Pattern pattern)
        {
            throw new NotImplementedException();
        }

        public virtual void VisitPointerReferenceExpression(PointerReferenceExpression pointerReferenceExpression)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(pointerReferenceExpression);
            }
        }

        public virtual void VisitPrimitiveExpression(PrimitiveExpression primitiveExpression)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(primitiveExpression);
            }
        }

        public virtual void VisitPropertyDeclaration(PropertyDeclaration propertyDeclaration)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(propertyDeclaration);
            }
        }

        public virtual void VisitQueryContinuationClause(QueryContinuationClause queryContinuationClause)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(queryContinuationClause);
            }
        }

        public virtual void VisitQueryExpression(QueryExpression queryExpression)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(queryExpression);
            }
        }

        public virtual void VisitQueryFromClause(QueryFromClause queryFromClause)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(queryFromClause);
            }
        }

        public virtual void VisitQueryGroupClause(QueryGroupClause queryGroupClause)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(queryGroupClause);
            }
        }

        public virtual void VisitQueryJoinClause(QueryJoinClause queryJoinClause)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(queryJoinClause);
            }
        }

        public virtual void VisitQueryLetClause(QueryLetClause queryLetClause)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(queryLetClause);
            }
        }

        public virtual void VisitQueryOrderClause(QueryOrderClause queryOrderClause)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(queryOrderClause);
            }
        }

        public virtual void VisitQueryOrdering(QueryOrdering queryOrdering)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(queryOrdering);
            }
        }

        public virtual void VisitQuerySelectClause(QuerySelectClause querySelectClause)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(querySelectClause);
            }
        }

        public virtual void VisitQueryWhereClause(QueryWhereClause queryWhereClause)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(queryWhereClause);
            }
        }

        public virtual void VisitReturnStatement(ReturnStatement returnStatement)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(returnStatement);
            }
        }

        public virtual void VisitSizeOfExpression(SizeOfExpression sizeOfExpression)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(sizeOfExpression);
            }
        }

        public virtual void VisitStackAllocExpression(StackAllocExpression stackAllocExpression)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(stackAllocExpression);
            }
        }

        public virtual void VisitSwitchSection(SwitchSection switchSection)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(switchSection);
            }
        }

        public virtual void VisitSwitchStatement(SwitchStatement switchStatement)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(switchStatement);
            }
        }

        public virtual void VisitSyntaxTree(SyntaxTree syntaxTree)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(syntaxTree);
            }
        }

        public virtual void VisitText(TextNode textNode)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(textNode);
            }
        }

        public virtual void VisitThisReferenceExpression(ThisReferenceExpression thisReferenceExpression)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(thisReferenceExpression);
            }
        }

        public virtual void VisitThrowStatement(ThrowStatement throwStatement)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(throwStatement);
            }
        }

        public virtual void VisitTryCatchStatement(TryCatchStatement tryCatchStatement)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(tryCatchStatement);
            }
        }

        public virtual void VisitTypeDeclaration(TypeDeclaration typeDeclaration)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(typeDeclaration);
            }
        }

        public virtual void VisitTypeOfExpression(TypeOfExpression typeOfExpression)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(typeOfExpression);
            }
        }

        public virtual void VisitTypeReferenceExpression(TypeReferenceExpression typeReferenceExpression)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(typeReferenceExpression);
            }
        }

        public virtual void VisitUnaryOperatorExpression(UnaryOperatorExpression unaryOperatorExpression)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(unaryOperatorExpression);
            }
        }

        public virtual void VisitUncheckedExpression(UncheckedExpression uncheckedExpression)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(uncheckedExpression);
            }
        }

        public virtual void VisitUncheckedStatement(UncheckedStatement uncheckedStatement)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(uncheckedStatement);
            }
        }

        public virtual void VisitUndocumentedExpression(UndocumentedExpression undocumentedExpression)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(undocumentedExpression);
            }
        }

        public virtual void VisitUnsafeStatement(UnsafeStatement unsafeStatement)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(unsafeStatement);
            }
        }

        public virtual void VisitUsingDeclaration(UsingDeclaration usingDeclaration)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(usingDeclaration);
            }
        }

        public virtual void VisitUsingStatement(UsingStatement usingStatement)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(usingStatement);
            }
        }

        public virtual void VisitVariableDeclarationStatement(VariableDeclarationStatement variableDeclarationStatement)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(variableDeclarationStatement);
            }
        }

        public virtual void VisitVariableInitializer(VariableInitializer variableInitializer)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(variableInitializer);
            }
        }

        public virtual void VisitWhileStatement(WhileStatement whileStatement)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(whileStatement);
            }
        }

        public virtual void VisitWhitespace(WhitespaceNode whitespaceNode)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(whitespaceNode);
            }
        }

        public virtual void VisitYieldBreakStatement(YieldBreakStatement yieldBreakStatement)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(yieldBreakStatement);
            }
        }

        public virtual void VisitYieldReturnStatement(YieldReturnStatement yieldReturnStatement)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(yieldReturnStatement);
            }
        }

        public virtual void VisitErrorNode(AstNode errorNode)
        {
            if (ThrowException)
            {
                throw (System.Exception)CreateException(errorNode);
            }
        }
    }
}