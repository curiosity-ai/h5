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
                throw (Exception)CreateException(accessor);
            }
        }

        public virtual void VisitAnonymousMethodExpression(AnonymousMethodExpression anonymousMethodExpression)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(anonymousMethodExpression);
            }
        }

        public virtual void VisitAnonymousTypeCreateExpression(AnonymousTypeCreateExpression anonymousTypeCreateExpression)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(anonymousTypeCreateExpression);
            }
        }

        public virtual void VisitArrayCreateExpression(ArrayCreateExpression arrayCreateExpression)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(arrayCreateExpression);
            }
        }

        public virtual void VisitArrayInitializerExpression(ArrayInitializerExpression arrayInitializerExpression)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(arrayInitializerExpression);
            }
        }

        public virtual void VisitArraySpecifier(ArraySpecifier arraySpecifier)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(arraySpecifier);
            }
        }

        public virtual void VisitAsExpression(AsExpression asExpression)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(asExpression);
            }
        }

        public virtual void VisitAssignmentExpression(AssignmentExpression assignmentExpression)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(assignmentExpression);
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
                throw (Exception)CreateException(baseReferenceExpression);
            }
        }

        public virtual void VisitBinaryOperatorExpression(BinaryOperatorExpression binaryOperatorExpression)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(binaryOperatorExpression);
            }
        }

        public virtual void VisitBlockStatement(BlockStatement blockStatement)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(blockStatement);
            }
        }

        public virtual void VisitBreakStatement(BreakStatement breakStatement)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(breakStatement);
            }
        }

        public virtual void VisitCaseLabel(CaseLabel caseLabel)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(caseLabel);
            }
        }

        public virtual void VisitCastExpression(CastExpression castExpression)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(castExpression);
            }
        }

        public virtual void VisitCatchClause(CatchClause catchClause)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(catchClause);
            }
        }

        public virtual void VisitCheckedExpression(CheckedExpression checkedExpression)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(checkedExpression);
            }
        }

        public virtual void VisitCheckedStatement(CheckedStatement checkedStatement)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(checkedStatement);
            }
        }

        public virtual void VisitComposedType(ComposedType composedType)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(composedType);
            }
        }

        public virtual void VisitConditionalExpression(ConditionalExpression conditionalExpression)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(conditionalExpression);
            }
        }

        public virtual void VisitConstructorDeclaration(ConstructorDeclaration constructorDeclaration)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(constructorDeclaration);
            }
        }

        public virtual void VisitConstructorInitializer(ConstructorInitializer constructorInitializer)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(constructorInitializer);
            }
        }

        public virtual void VisitContinueStatement(ContinueStatement continueStatement)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(continueStatement);
            }
        }

        public virtual void VisitCustomEventDeclaration(CustomEventDeclaration customEventDeclaration)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(customEventDeclaration);
            }
        }

        public virtual void VisitDelegateDeclaration(DelegateDeclaration delegateDeclaration)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(delegateDeclaration);
            }
        }

        public virtual void VisitDestructorDeclaration(DestructorDeclaration destructorDeclaration)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(destructorDeclaration);
            }
        }

        public virtual void VisitDirectionExpression(DirectionExpression directionExpression)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(directionExpression);
            }
        }

        public virtual void VisitDoWhileStatement(DoWhileStatement doWhileStatement)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(doWhileStatement);
            }
        }

        public virtual void VisitDocumentationReference(DocumentationReference documentationReference)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(documentationReference);
            }
        }

        public virtual void VisitEmptyStatement(EmptyStatement emptyStatement)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(emptyStatement);
            }
        }

        public virtual void VisitEnumMemberDeclaration(EnumMemberDeclaration enumMemberDeclaration)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(enumMemberDeclaration);
            }
        }

        public virtual void VisitEventDeclaration(EventDeclaration eventDeclaration)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(eventDeclaration);
            }
        }

        public virtual void VisitExpressionStatement(ExpressionStatement expressionStatement)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(expressionStatement);
            }
        }

        public virtual void VisitExternAliasDeclaration(ExternAliasDeclaration externAliasDeclaration)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(externAliasDeclaration);
            }
        }

        public virtual void VisitFieldDeclaration(FieldDeclaration fieldDeclaration)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(fieldDeclaration);
            }
        }

        public virtual void VisitFixedFieldDeclaration(FixedFieldDeclaration fixedFieldDeclaration)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(fixedFieldDeclaration);
            }
        }

        public virtual void VisitFixedStatement(FixedStatement fixedStatement)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(fixedStatement);
            }
        }

        public virtual void VisitFixedVariableInitializer(FixedVariableInitializer fixedVariableInitializer)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(fixedVariableInitializer);
            }
        }

        public virtual void VisitForStatement(ForStatement forStatement)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(forStatement);
            }
        }

        public virtual void VisitForeachStatement(ForeachStatement foreachStatement)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(foreachStatement);
            }
        }

        public virtual void VisitGotoCaseStatement(GotoCaseStatement gotoCaseStatement)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(gotoCaseStatement);
            }
        }

        public virtual void VisitGotoDefaultStatement(GotoDefaultStatement gotoDefaultStatement)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(gotoDefaultStatement);
            }
        }

        public virtual void VisitGotoStatement(GotoStatement gotoStatement)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(gotoStatement);
            }
        }

        public virtual void VisitIdentifierExpression(IdentifierExpression identifierExpression)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(identifierExpression);
            }
        }

        public virtual void VisitIfElseStatement(IfElseStatement ifElseStatement)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(ifElseStatement);
            }
        }

        public virtual void VisitIndexerDeclaration(IndexerDeclaration indexerDeclaration)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(indexerDeclaration);
            }
        }

        public virtual void VisitIndexerExpression(IndexerExpression indexerExpression)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(indexerExpression);
            }
        }

        public virtual void VisitInvocationExpression(InvocationExpression invocationExpression)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(invocationExpression);
            }
        }

        public virtual void VisitIsExpression(IsExpression isExpression)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(isExpression);
            }
        }

        public virtual void VisitLabelStatement(LabelStatement labelStatement)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(labelStatement);
            }
        }

        public virtual void VisitLambdaExpression(LambdaExpression lambdaExpression)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(lambdaExpression);
            }
        }

        public virtual void VisitLockStatement(LockStatement lockStatement)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(lockStatement);
            }
        }

        public virtual void VisitMemberReferenceExpression(MemberReferenceExpression memberReferenceExpression)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(memberReferenceExpression);
            }
        }

        public virtual void VisitMethodDeclaration(MethodDeclaration methodDeclaration)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(methodDeclaration);
            }
        }

        public virtual void VisitNamedArgumentExpression(NamedArgumentExpression namedArgumentExpression)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(namedArgumentExpression);
            }
        }

        public virtual void VisitNamedExpression(NamedExpression namedExpression)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(namedExpression);
            }
        }

        public virtual void VisitNamespaceDeclaration(NamespaceDeclaration namespaceDeclaration)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(namespaceDeclaration);
            }
        }

        public virtual void VisitObjectCreateExpression(ObjectCreateExpression objectCreateExpression)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(objectCreateExpression);
            }
        }

        public virtual void VisitParameterDeclaration(ParameterDeclaration parameterDeclaration)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(parameterDeclaration);
            }
        }

        public virtual void VisitParenthesizedExpression(ParenthesizedExpression parenthesizedExpression)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(parenthesizedExpression);
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
                throw (Exception)CreateException(pointerReferenceExpression);
            }
        }

        public virtual void VisitPrimitiveExpression(PrimitiveExpression primitiveExpression)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(primitiveExpression);
            }
        }

        public virtual void VisitPropertyDeclaration(PropertyDeclaration propertyDeclaration)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(propertyDeclaration);
            }
        }

        public virtual void VisitQueryContinuationClause(QueryContinuationClause queryContinuationClause)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(queryContinuationClause);
            }
        }

        public virtual void VisitQueryExpression(QueryExpression queryExpression)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(queryExpression);
            }
        }

        public virtual void VisitQueryFromClause(QueryFromClause queryFromClause)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(queryFromClause);
            }
        }

        public virtual void VisitQueryGroupClause(QueryGroupClause queryGroupClause)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(queryGroupClause);
            }
        }

        public virtual void VisitQueryJoinClause(QueryJoinClause queryJoinClause)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(queryJoinClause);
            }
        }

        public virtual void VisitQueryLetClause(QueryLetClause queryLetClause)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(queryLetClause);
            }
        }

        public virtual void VisitQueryOrderClause(QueryOrderClause queryOrderClause)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(queryOrderClause);
            }
        }

        public virtual void VisitQueryOrdering(QueryOrdering queryOrdering)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(queryOrdering);
            }
        }

        public virtual void VisitQuerySelectClause(QuerySelectClause querySelectClause)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(querySelectClause);
            }
        }

        public virtual void VisitQueryWhereClause(QueryWhereClause queryWhereClause)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(queryWhereClause);
            }
        }

        public virtual void VisitReturnStatement(ReturnStatement returnStatement)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(returnStatement);
            }
        }

        public virtual void VisitSizeOfExpression(SizeOfExpression sizeOfExpression)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(sizeOfExpression);
            }
        }

        public virtual void VisitStackAllocExpression(StackAllocExpression stackAllocExpression)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(stackAllocExpression);
            }
        }

        public virtual void VisitSwitchSection(SwitchSection switchSection)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(switchSection);
            }
        }

        public virtual void VisitSwitchStatement(SwitchStatement switchStatement)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(switchStatement);
            }
        }

        public virtual void VisitSyntaxTree(SyntaxTree syntaxTree)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(syntaxTree);
            }
        }

        public virtual void VisitText(TextNode textNode)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(textNode);
            }
        }

        public virtual void VisitThisReferenceExpression(ThisReferenceExpression thisReferenceExpression)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(thisReferenceExpression);
            }
        }

        public virtual void VisitThrowStatement(ThrowStatement throwStatement)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(throwStatement);
            }
        }

        public virtual void VisitTryCatchStatement(TryCatchStatement tryCatchStatement)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(tryCatchStatement);
            }
        }

        public virtual void VisitTypeDeclaration(TypeDeclaration typeDeclaration)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(typeDeclaration);
            }
        }

        public virtual void VisitTypeOfExpression(TypeOfExpression typeOfExpression)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(typeOfExpression);
            }
        }

        public virtual void VisitTypeReferenceExpression(TypeReferenceExpression typeReferenceExpression)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(typeReferenceExpression);
            }
        }

        public virtual void VisitUnaryOperatorExpression(UnaryOperatorExpression unaryOperatorExpression)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(unaryOperatorExpression);
            }
        }

        public virtual void VisitUncheckedExpression(UncheckedExpression uncheckedExpression)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(uncheckedExpression);
            }
        }

        public virtual void VisitUncheckedStatement(UncheckedStatement uncheckedStatement)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(uncheckedStatement);
            }
        }

        public virtual void VisitUndocumentedExpression(UndocumentedExpression undocumentedExpression)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(undocumentedExpression);
            }
        }

        public virtual void VisitUnsafeStatement(UnsafeStatement unsafeStatement)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(unsafeStatement);
            }
        }

        public virtual void VisitUsingDeclaration(UsingDeclaration usingDeclaration)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(usingDeclaration);
            }
        }

        public virtual void VisitUsingStatement(UsingStatement usingStatement)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(usingStatement);
            }
        }

        public virtual void VisitVariableDeclarationStatement(VariableDeclarationStatement variableDeclarationStatement)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(variableDeclarationStatement);
            }
        }

        public virtual void VisitVariableInitializer(VariableInitializer variableInitializer)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(variableInitializer);
            }
        }

        public virtual void VisitWhileStatement(WhileStatement whileStatement)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(whileStatement);
            }
        }

        public virtual void VisitWhitespace(WhitespaceNode whitespaceNode)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(whitespaceNode);
            }
        }

        public virtual void VisitYieldBreakStatement(YieldBreakStatement yieldBreakStatement)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(yieldBreakStatement);
            }
        }

        public virtual void VisitYieldReturnStatement(YieldReturnStatement yieldReturnStatement)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(yieldReturnStatement);
            }
        }

        public virtual void VisitErrorNode(AstNode errorNode)
        {
            if (ThrowException)
            {
                throw (Exception)CreateException(errorNode);
            }
        }
    }
}