using H5.Contract;
using ICSharpCode.NRefactory.CSharp;

namespace H5.Translator
{
    public class OperatorBlock : AbstractMethodBlock
    {
        public OperatorBlock(IEmitter emitter, OperatorDeclaration operatorDeclaration)
            : base(emitter, operatorDeclaration)
        {
            Emitter = emitter;
            OperatorDeclaration = operatorDeclaration;
        }

        public OperatorDeclaration OperatorDeclaration { get; set; }

        protected override void DoEmit()
        {
            EmitOperatorDeclaration(OperatorDeclaration);
        }

        protected void EmitOperatorDeclaration(OperatorDeclaration operatorDeclaration)
        {
            foreach (var attrSection in operatorDeclaration.Attributes)
            {
                foreach (var attr in attrSection.Attributes)
                {
                    var rr = Emitter.Resolver.ResolveNode(attr.Type, Emitter);
                    if (rr.Type.FullName == "H5.ExternalAttribute")
                    {
                        return;
                    }
                }
            }

            XmlToJsDoc.EmitComment(this, operatorDeclaration);
            EnsureComma();
            ResetLocals();
            var prevMap = BuildLocalsMap();
            var prevNamesMap = BuildLocalsNamesMap();
            AddLocals(operatorDeclaration.Parameters, operatorDeclaration.Body);

            var overloads = OverloadsCollection.Create(Emitter, operatorDeclaration);

            if (overloads.HasOverloads)
            {
                string name = overloads.GetOverloadName();
                Write(name);
            }
            else
            {
                Write(Emitter.GetEntityName(operatorDeclaration));
            }

            WriteColon();

            WriteFunction();

            EmitMethodParameters(operatorDeclaration.Parameters, null, operatorDeclaration);

            WriteSpace();

            var script = Emitter.GetScript(operatorDeclaration);

            if (script == null)
            {
                operatorDeclaration.Body.AcceptVisitor(Emitter);
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