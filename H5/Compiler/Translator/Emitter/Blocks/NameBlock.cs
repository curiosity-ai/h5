using H5.Contract;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;

namespace H5.Translator
{
    public class NameBlock : AbstractEmitterBlock
    {
        public NameBlock(IEmitter emitter, NamedExpression namedExpression)
            : this(emitter, namedExpression.Name, namedExpression, namedExpression.Expression, null)
        {
        }

        public NameBlock(IEmitter emitter, string name, Expression namedExpression, Expression expression, bool? isSet)
            : base(emitter, null)
        {
            Emitter = emitter;
            NamedExpression = namedExpression;
            Expression = expression;
            Name = name;
            IsSet = isSet;

            Emitter.Translator.EmitNode = namedExpression ?? expression;
        }

        public bool? IsSet { get; set; }

        public string Name { get; set; }

        public Expression Expression { get; set; }

        public Expression NamedExpression { get; set; }

        protected override void DoEmit()
        {
            EmitNameExpression(Name, NamedExpression, Expression);
        }

        protected virtual void EmitNameExpression(string name, Expression namedExpression, Expression expression)
        {
            var resolveResult = Emitter.Resolver.ResolveNode(namedExpression, Emitter);

            if (resolveResult is MemberResolveResult)
            {
                var member = ((MemberResolveResult)resolveResult).Member;
                name = Emitter.GetEntityName(member);

                bool isSet = IsSet ?? !(expression is ArrayInitializerExpression);
                if (member is IProperty)
                {
                    Write(Helpers.GetPropertyRef(member, Emitter, isSet));
                }
                else if (member is IEvent)
                {
                    Write(Helpers.GetEventRef(member, Emitter, !isSet));
                }
                else
                {
                    Write(name);
                }
            }
            else
            {
                Write(name);
            }

            WriteColon();
            expression.AcceptVisitor(Emitter);
        }
    }
}