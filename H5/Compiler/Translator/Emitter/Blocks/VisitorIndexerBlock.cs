using H5.Contract;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;

namespace H5.Translator
{
    public class VisitorIndexerBlock : AbstractMethodBlock
    {
        public VisitorIndexerBlock(IEmitter emitter, IndexerDeclaration indexerDeclaration)
            : base(emitter, indexerDeclaration)
        {
            Emitter = emitter;
            IndexerDeclaration = indexerDeclaration;
        }

        public IndexerDeclaration IndexerDeclaration { get; set; }

        public CompilerRule OldRules { get; private set; }

        protected override void BeginEmit()
        {
            base.BeginEmit();
            OldRules = Emitter.Rules;


            if (Emitter.Resolver.ResolveNode(IndexerDeclaration) is MemberResolveResult rr)
            {
                Emitter.Rules = Rules.Get(Emitter, rr.Member);
            }
        }

        protected override void EndEmit()
        {
            base.EndEmit();
            Emitter.Rules = OldRules;
        }

        protected override void DoEmit()
        {
            IProperty prop = null;
            if (Emitter.Resolver.ResolveNode(IndexerDeclaration) is MemberResolveResult rr)
            {
                prop = rr.Member as IProperty;

                if (prop != null && Emitter.Validator.IsExternalType(prop))
                {
                    return;
                }
            }

            EmitIndexerMethod(IndexerDeclaration, prop, IndexerDeclaration.Getter, prop?.Getter, false);
            EmitIndexerMethod(IndexerDeclaration, prop, IndexerDeclaration.Setter, prop?.Setter, true);
        }

        protected virtual void EmitIndexerMethod(IndexerDeclaration indexerDeclaration, IProperty prop, Accessor accessor, IMethod propAccessor, bool setter)
        {
            var isIgnore = propAccessor != null && Emitter.Validator.IsExternalType(propAccessor);

            if (!accessor.IsNull && Emitter.GetInline(accessor) == null && !isIgnore)
            {
                EnsureComma();

                ResetLocals();

                var prevMap = BuildLocalsMap();
                var prevNamesMap = BuildLocalsNamesMap();

                if (setter)
                {
                    AddLocals(new ParameterDeclaration[] {new ParameterDeclaration {Name = "value"}}, accessor.Body);
                }
                else
                {
                    AddLocals(new ParameterDeclaration[0], accessor.Body);
                }

                XmlToJsDoc.EmitComment(this, IndexerDeclaration, !setter);

                string accName = null;

                if (prop != null)
                {
                    accName = Emitter.GetEntityNameFromAttr(prop, setter);

                    if (string.IsNullOrEmpty(accName))
                    {
                        var member_rr = (MemberResolveResult)Emitter.Resolver.ResolveNode(indexerDeclaration);

                        var overloads = OverloadsCollection.Create(Emitter, indexerDeclaration, setter);
                        accName = overloads.GetOverloadName(false, Helpers.GetSetOrGet(setter), OverloadsCollection.ExcludeTypeParameterForDefinition(member_rr));
                    }
                }

                Write(accName);
                WriteColon();
                WriteFunction();
                EmitMethodParameters(indexerDeclaration.Parameters, null, indexerDeclaration, setter);

                if (setter)
                {
                    Write(", value)");
                }
                WriteSpace();

                var script = Emitter.GetScript(accessor);

                if (script == null)
                {
                    if (YieldBlock.HasYield(accessor.Body))
                    {
                        new GeneratorBlock(Emitter, accessor).Emit();
                    }
                    else
                    {
                        accessor.Body.AcceptVisitor(Emitter);
                    }
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