using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using System.Collections.Generic;
using System.Globalization;

namespace H5.Contract
{
    public class TypeConfigItem
    {
        public string Name { get; set; }

        public EntityDeclaration Entity { get; set; }

        public Expression Initializer { get; set; }

        public VariableInitializer VarInitializer { get; set; }

        public bool IsConst { get; set; }

        public IMember InterfaceMember{ get; set; }

        public IMember DerivedMember{ get; set; }

        public bool IsPropertyInitializer{ get; set; }

        public string GetName(IEmitter emitter, bool withoutTypeParams = false)
        {
            string fieldName = Name;

            if (VarInitializer != null)
            {
                var rr = emitter.Resolver.ResolveNode(VarInitializer) as MemberResolveResult;
                fieldName = OverloadsCollection.Create(emitter, rr.Member).GetOverloadName(false, null, withoutTypeParams);
            }
            else if (Entity is PropertyDeclaration)
            {
                fieldName = OverloadsCollection.Create(emitter, (PropertyDeclaration)Entity, isField: true).GetOverloadName(false, null, withoutTypeParams);
            }
            else
            {
                if (Entity != null)
                {
                    if (emitter.Resolver.ResolveNode(Entity) is MemberResolveResult rr)
                    {
                        fieldName = OverloadsCollection.Create(emitter, rr.Member).GetOverloadName(false, null, withoutTypeParams);
                    }
                }
            }
            return fieldName;
        }
    }

    public class TypeConfigInfo
    {
        public TypeConfigInfo()
        {
            Fields = new List<TypeConfigItem>();
            Events = new List<TypeConfigItem>();
            Properties = new List<TypeConfigItem>();
            Alias = new List<TypeConfigItem>();
            AutoPropertyInitializers = new List<TypeConfigItem>();
        }

        public bool HasMembers
        {
            get
            {
                return Fields.Count > 0 || Events.Count > 0 || Properties.Count > 0 || Alias.Count > 0;
            }
        }

        public bool HasConfigMembers
        {
            get
            {
                return Events.Count > 0 || Properties.Count > 0 || Alias.Count > 0;
            }
        }

        public List<TypeConfigItem> Fields { get; set; }

        public List<TypeConfigItem> Events { get; set; }

        public List<TypeConfigItem> Properties { get; set; }

        public List<TypeConfigItem> Alias { get; set; }

        public List<TypeConfigItem> AutoPropertyInitializers { get; set; }
    }
}