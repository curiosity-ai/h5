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
            this.Emitter = emitter;
            this.BaseReferenceExpression = baseReferenceExpression;
        }

        public BaseReferenceExpression BaseReferenceExpression
        {
            get;
            set;
        }

        protected override Expression GetExpression()
        {
            return this.BaseReferenceExpression;
        }

        protected override void EmitConversionExpression()
        {
            var proto = false;
            var isProperty = false;
            IMember member = null;
            if (this.BaseReferenceExpression.Parent != null)
            {
                if (this.Emitter.Resolver.ResolveNode(this.BaseReferenceExpression.Parent, this.Emitter) is MemberResolveResult rr)
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
                    if (this.Emitter.GetInline(member) == null)
                    {
                        var name = OverloadsCollection.Create(this.Emitter, member).GetOverloadName(true);
                        this.Write(JS.Types.H5.ENSURE_BASE_PROPERTY + "(this, " + this.Emitter.ToJavaScript(name));

                        if (this.Emitter.Validator.IsExternalType(member.DeclaringTypeDefinition) && !this.Emitter.Validator.IsH5Class(member.DeclaringTypeDefinition))
                        {
                            this.Write(", \"" + H5Types.ToJsName(member.DeclaringType, this.Emitter, isAlias: true) + "\"");
                        }

                        this.Write(")");
                    }
                    else
                    {
                        this.WriteThis();
                    }
                }
                else
                {
                    var baseType = this.Emitter.GetBaseTypeDefinition();

                    if (this.Emitter.TypeInfo.GetBaseTypes(this.Emitter).Any())
                    {
                        this.Write(H5Types.ToJsName(this.Emitter.TypeInfo.GetBaseClass(this.Emitter), this.Emitter), "." + JS.Fields.PROTOTYPE);
                    }
                    else
                    {
                        this.Write(H5Types.ToJsName(baseType, this.Emitter), "." + JS.Fields.PROTOTYPE);
                    }
                }
            }
            else
            {
                this.WriteThis();
            }
        }
    }
}