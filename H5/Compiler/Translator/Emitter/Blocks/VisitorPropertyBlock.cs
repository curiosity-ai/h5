using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.Semantics;
using System.Linq;
using ICSharpCode.NRefactory.TypeSystem;

namespace H5.Translator
{
    public class VisitorPropertyBlock : AbstractMethodBlock
    {
        public VisitorPropertyBlock(IEmitter emitter, PropertyDeclaration propertyDeclaration)
            : base(emitter, propertyDeclaration)
        {
            Emitter = emitter;
            PropertyDeclaration = propertyDeclaration;
        }

        public PropertyDeclaration PropertyDeclaration { get; set; }
        public CompilerRule OldRules { get; private set; }

        protected override void BeginEmit()
        {
            base.BeginEmit();
            OldRules = Emitter.Rules;


            if (Emitter.Resolver.ResolveNode(PropertyDeclaration) is MemberResolveResult rr)
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
            var memberResult = Emitter.Resolver.ResolveNode(PropertyDeclaration) as MemberResolveResult;

            if (memberResult != null &&
                memberResult.Member.Attributes.Any(a => a.AttributeType.FullName == "H5.ExternalAttribute")
                )
            {
                return;
            }

            EmitPropertyMethod(PropertyDeclaration, PropertyDeclaration.Getter, ((IProperty)memberResult.Member).Getter, false, false);
            EmitPropertyMethod(PropertyDeclaration, PropertyDeclaration.Setter, ((IProperty)memberResult.Member).Setter, true, false);
        }

        public virtual void EmitPropertyMethod(PropertyDeclaration propertyDeclaration, Accessor accessor, IMethod method, bool setter, bool isObjectLiteral)
        {
            if ((!accessor.IsNull || method != null && Helpers.IsScript(method)) && Emitter.GetInline(accessor) == null)
            {
                EnsureComma();

                ResetLocals();

                var prevMap = BuildLocalsMap();
                var prevNamesMap = BuildLocalsNamesMap();

                if (setter)
                {
                    AddLocals(new ParameterDeclaration[] { new ParameterDeclaration { Name = "value" } }, accessor.Body);
                }
                else
                {
                    AddLocals(new ParameterDeclaration[0], accessor.Body);
                }

                //XmlToJsDoc.EmitComment(this, this.PropertyDeclaration);

                Write(setter ? JS.Funcs.Property.SET : JS.Funcs.Property.GET);

                WriteColon();
                WriteFunction();

                var m_rr = (MemberResolveResult)Emitter.Resolver.ResolveNode(propertyDeclaration);
                var nm = Helpers.GetFunctionName(Emitter.AssemblyInfo.NamedFunctions, m_rr.Member, Emitter, setter);
                if (nm != null)
                {
                    Write(nm);
                }

                WriteOpenParentheses();
                Write(setter ? "value" : "");
                WriteCloseParentheses();
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