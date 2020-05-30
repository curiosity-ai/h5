using H5.Contract;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.Semantics;

namespace H5.Translator
{
    public class VisitorCustomEventBlock : AbstractMethodBlock
    {
        public VisitorCustomEventBlock(IEmitter emitter, CustomEventDeclaration customEventDeclaration)
            : base(emitter, customEventDeclaration)
        {
            Emitter = emitter;
            CustomEventDeclaration = customEventDeclaration;
        }

        public CustomEventDeclaration CustomEventDeclaration { get; set; }

        public CompilerRule OldRules { get; private set; }

        protected override void BeginEmit()
        {
            base.BeginEmit();
            OldRules = Emitter.Rules;


            if (Emitter.Resolver.ResolveNode(CustomEventDeclaration) is MemberResolveResult rr)
            {
                Emitter.Rules = Rules.Get(Emitter, rr.Member);
            }
        }

        protected override void DoEmit()
        {
            EmitPropertyMethod(CustomEventDeclaration, CustomEventDeclaration.AddAccessor, false);
            EmitPropertyMethod(CustomEventDeclaration, CustomEventDeclaration.RemoveAccessor, true);
        }

        protected virtual void EmitPropertyMethod(CustomEventDeclaration customEventDeclaration, Accessor accessor, bool remover)
        {
            if (!accessor.IsNull && Emitter.GetInline(accessor) == null)
            {
                EnsureComma();

                ResetLocals();

                var prevMap = BuildLocalsMap();
                var prevNamesMap = BuildLocalsNamesMap();

                AddLocals(new ParameterDeclaration[] { new ParameterDeclaration { Name = "value" } }, accessor.Body);
                XmlToJsDoc.EmitComment(this, CustomEventDeclaration);
                var member_rr = (MemberResolveResult)Emitter.Resolver.ResolveNode(customEventDeclaration);

                Write(Helpers.GetEventRef(customEventDeclaration, Emitter, remover, false, false, OverloadsCollection.ExcludeTypeParameterForDefinition(member_rr)));
                WriteColon();
                WriteFunction();
                var m_rr = (MemberResolveResult)Emitter.Resolver.ResolveNode(customEventDeclaration);
                WriteOpenParentheses();
                Write("value");
                WriteCloseParentheses();
                WriteSpace();

                var script = Emitter.GetScript(accessor);

                if (script == null)
                {
                    accessor.Body.AcceptVisitor(Emitter);
                }
                else
                {
                    BeginBlock();

                    WriteLines(script);

                    EndBlock();
                }

                ClearLocalsMap(prevMap);
                ClearLocalsNamesMap(prevNamesMap);
                Emitter.Comma = true;
            }
        }
    }
}