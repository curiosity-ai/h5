using System;
using H5.Contract;
using ICSharpCode.NRefactory.CSharp;
using System.Linq;
using System.Xml.Schema;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;

namespace H5.Translator
{
    public class VisitorMethodBlock : AbstractMethodBlock
    {
        public VisitorMethodBlock(IEmitter emitter, MethodDeclaration methodDeclaration)
            : base(emitter, methodDeclaration)
        {
            Emitter = emitter;
            MethodDeclaration = methodDeclaration;
        }

        public MethodDeclaration MethodDeclaration { get; set; }
        public CompilerRule OldRules { get; private set; }

        protected override void BeginEmit()
        {
            base.BeginEmit();
            OldRules = Emitter.Rules;


            if (Emitter.Resolver.ResolveNode(MethodDeclaration) is MemberResolveResult rr)
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
            VisitMethodDeclaration(MethodDeclaration);
        }

        protected void VisitMethodDeclaration(MethodDeclaration methodDeclaration)
        {
            foreach (var attrSection in methodDeclaration.Attributes)
            {
                foreach (var attr in attrSection.Attributes)
                {
                    var rr = Emitter.Resolver.ResolveNode(attr.Type);
                    if (rr.Type.FullName == "H5.ExternalAttribute")
                    {
                        return;
                    }
                    else if (rr.Type.FullName == "H5.InitAttribute")
                    {
                        InitPosition initPosition = InitPosition.After;

                        if (attr.HasArgumentList)
                        {
                            if (attr.Arguments.Any())
                            {
                                var argExpr = attr.Arguments.First();
                                var argrr = Emitter.Resolver.ResolveNode(argExpr);
                                if (argrr.ConstantValue is int)
                                {
                                    initPosition = (InitPosition)argrr.ConstantValue;
                                }
                            }
                        }

                        if (initPosition > 0)
                        {
                            return;
                        }
                    }
                }
            }

            EnsureComma();
            ResetLocals();

            var prevMap = BuildLocalsMap();
            var prevNamesMap = BuildLocalsNamesMap();

            AddLocals(methodDeclaration.Parameters, methodDeclaration.Body);

            var overloads = OverloadsCollection.Create(Emitter, methodDeclaration);
            XmlToJsDoc.EmitComment(this, MethodDeclaration);
            var isEntryPoint = Helpers.IsEntryPointMethod(Emitter, MethodDeclaration);
            var member_rr = (MemberResolveResult)Emitter.Resolver.ResolveNode(MethodDeclaration);

            string name = overloads.GetOverloadName(false, null, excludeTypeOnly: OverloadsCollection.ExcludeTypeParameterForDefinition(member_rr));

            if (isEntryPoint)
            {
                Write(JS.Funcs.ENTRY_POINT_NAME);
            }
            else
            {
                Write(name);
            }

            WriteColon();

            WriteFunction();

            if (isEntryPoint)
            {
                Write(name);
                WriteSpace();
            }
            else
            {
                var nm = Helpers.GetFunctionName(Emitter.AssemblyInfo.NamedFunctions, member_rr.Member, Emitter);
                if (nm != null)
                {
                    Write(nm);
                    WriteSpace();
                }
            }

            EmitMethodParameters(methodDeclaration.Parameters, methodDeclaration.TypeParameters.Count > 0 && Helpers.IsIgnoreGeneric(methodDeclaration, Emitter) ? null : methodDeclaration.TypeParameters, methodDeclaration);

            WriteSpace();

            var script = Emitter.GetScript(methodDeclaration);

            if (script == null)
            {
                if (YieldBlock.HasYield(methodDeclaration.Body))
                {
                    new GeneratorBlock(Emitter, methodDeclaration).Emit();
                }
                else if (methodDeclaration.HasModifier(Modifiers.Async) || AsyncBlock.HasGoto(methodDeclaration.Body))
                {
                    new AsyncBlock(Emitter, methodDeclaration).Emit();
                }
                else
                {
                    methodDeclaration.Body.AcceptVisitor(Emitter);
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