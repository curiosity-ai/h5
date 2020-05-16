using System.Linq;
using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;

namespace H5.Translator
{
    public class BaseReferenceBlock : ConversionBlock
    {
        public BaseReferenceBlock(IEmitter emitter, BaseReferenceExpression baseReferenceExpression)
            : base(emitter, baseReferenceExpression)
        {
            Emitter = emitter;
            BaseReferenceExpression = baseReferenceExpression;
        }

        public BaseReferenceExpression BaseReferenceExpression { get; set; }

        protected override Expression GetExpression()
        {
            return BaseReferenceExpression;
        }

        protected override void EmitConversionExpression()
        {
            var proto = false;
            var isProperty = false;
            IMember member = null;
            if (BaseReferenceExpression.Parent != null)
            {
                if (Emitter.Resolver.ResolveNode(BaseReferenceExpression.Parent, Emitter) is MemberResolveResult rr)
                {
                    if (rr.IsVirtualCall)
                    {
                        proto = true;
                    }
                    else
                    {
                        if (rr.Member is IMethod method && (method.IsVirtual || method.IsOverride))
                        {
                            proto = true;
                        }
                        else
                        {
                            if (rr.Member is IProperty prop && (prop.IsVirtual || prop.IsOverride))
                            {
                                proto = true;
                            }
                        }
                    }

                    if (rr.Member is IProperty iproperty && !iproperty.IsIndexer)
                    {
                        isProperty = true;
                        member = rr.Member;
                    }
                }
            }

            if (proto)
            {
                if (isProperty)
                {
                    if (Emitter.GetInline(member) == null)
                    {
                        var name = OverloadsCollection.Create(Emitter, member).GetOverloadName(true);
                        Write(JS.Types.H5.ENSURE_BASE_PROPERTY + "(this, " + Emitter.ToJavaScript(name));

                        if (Emitter.Validator.IsExternalType(member.DeclaringTypeDefinition) && !Emitter.Validator.IsH5Class(member.DeclaringTypeDefinition))
                        {
                            Write(", \"" + H5Types.ToJsName(member.DeclaringType, Emitter, isAlias: true) + "\"");
                        }

                        Write(")");
                    }
                    else
                    {
                        WriteThis();
                    }
                }
                else
                {
                    var baseType = Emitter.GetBaseTypeDefinition();

                    if (Emitter.TypeInfo.GetBaseTypes(Emitter).Any())
                    {
                        Write(H5Types.ToJsName(Emitter.TypeInfo.GetBaseClass(Emitter), Emitter), "." + JS.Fields.PROTOTYPE);
                    }
                    else
                    {
                        Write(H5Types.ToJsName(baseType, Emitter), "." + JS.Fields.PROTOTYPE);
                    }
                }
            }
            else
            {
                WriteThis();
            }
        }
    }
}