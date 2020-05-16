using H5.Contract;
using ICSharpCode.NRefactory.CSharp;
using System.Collections.Generic;
using ICSharpCode.NRefactory.Semantics;

namespace H5.Translator
{
    public abstract class AbstractObjectCreateBlock : AbstractEmitterBlock
    {
        public AbstractObjectCreateBlock(IEmitter emitter, AstNode node)
            : base(emitter, node)
        {
        }

        protected virtual void WriteObjectInitializer(IEnumerable<Expression> expressions, bool changeCase, bool valuesOnly = false)
        {
            bool needComma = false;

            foreach (Expression item in expressions)
            {
                NamedExpression namedExression = null;
                NamedArgumentExpression namedArgumentExpression = null;
                IdentifierExpression identifierExpression = null;
                MemberReferenceExpression memberReferenceExpression = null;

                namedExression = item as NamedExpression;
                if (namedExression == null)
                {
                    namedArgumentExpression = item as NamedArgumentExpression;

                    if (namedArgumentExpression == null)
                    {
                        identifierExpression = item as IdentifierExpression;
                        if (identifierExpression == null)
                        {
                            memberReferenceExpression = item as MemberReferenceExpression;
                        }
                    }
                }

                if (needComma)
                {
                    WriteComma();
                }

                needComma = true;
                string name = null;
                Expression expression;


                if (Emitter.Resolver.ResolveNode(item, Emitter) is MemberResolveResult rr)
                {
                    name = Emitter.GetEntityName(rr.Member);
                    changeCase = false;
                }

                if (namedExression != null)
                {
                    name = name ?? namedExression.Name;
                    expression = namedExression.Expression;
                }
                else if (namedArgumentExpression != null)
                {
                    name = name ?? namedArgumentExpression.Name;
                    expression = namedArgumentExpression.Expression;
                }
                else if (identifierExpression != null)
                {
                    name = name ?? identifierExpression.Identifier;
                    expression = identifierExpression;
                }
                else
                {
                    name = name ?? memberReferenceExpression.MemberName;
                    expression = memberReferenceExpression;
                }

                if (changeCase)
                {
                    name = Object.Net.Utilities.StringUtils.ToLowerCamelCase(name);
                }

                if (!valuesOnly)
                {
                    Write(name, ": ");
                }

                expression.AcceptVisitor(Emitter);
            }
        }
    }
}