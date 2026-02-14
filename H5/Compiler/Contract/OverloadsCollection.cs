using System;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ITypeDefinition = ICSharpCode.NRefactory.TypeSystem.ITypeDefinition;
using Modifiers = ICSharpCode.NRefactory.CSharp.Modifiers;

namespace H5.Contract
{
    public class OverloadsCollection
    {
        public static OverloadsCollection Create(IEmitter emitter, FieldDeclaration fieldDeclaration)
        {
            OverloadsCollection collection;

            if (emitter.Cache.TryGetNode(fieldDeclaration, false, out collection))
            {
                return collection;
            }

            return new OverloadsCollection(emitter, fieldDeclaration);
        }

        public static OverloadsCollection Create(IEmitter emitter, EventDeclaration eventDeclaration)
        {
            OverloadsCollection collection;

            if (emitter.Cache.TryGetNode(eventDeclaration, false, out collection))
            {
                return collection;
            }

            return new OverloadsCollection(emitter, eventDeclaration);
        }

        public static OverloadsCollection Create(IEmitter emitter, CustomEventDeclaration eventDeclaration, bool remove)
        {
            OverloadsCollection collection;

            if (emitter.Cache.TryGetNode(eventDeclaration, remove, out collection))
            {
                return collection;
            }

            return new OverloadsCollection(emitter, eventDeclaration, remove);
        }

        public static OverloadsCollection Create(IEmitter emitter, MethodDeclaration methodDeclaration)
        {
            OverloadsCollection collection;

            if (emitter.Cache.TryGetNode(methodDeclaration, false, out collection))
            {
                return collection;
            }

            return new OverloadsCollection(emitter, methodDeclaration);
        }

        public static OverloadsCollection Create(IEmitter emitter, ConstructorDeclaration constructorDeclaration)
        {
            OverloadsCollection collection;

            if (emitter.Cache.TryGetNode(constructorDeclaration, false, out collection))
            {
                return collection;
            }

            return new OverloadsCollection(emitter, constructorDeclaration);
        }

        public static OverloadsCollection Create(IEmitter emitter, PropertyDeclaration propDeclaration, bool isSetter = false, bool isField = false)
        {
            OverloadsCollection collection;

            if (emitter.Cache.TryGetNode(propDeclaration, isSetter, out collection))
            {
                return collection;
            }

            return new OverloadsCollection(emitter, propDeclaration, isSetter, isField);
        }

        public static OverloadsCollection Create(IEmitter emitter, IndexerDeclaration indexerDeclaration, bool isSetter = false)
        {
            OverloadsCollection collection;

            if (emitter.Cache.TryGetNode(indexerDeclaration, isSetter, out collection))
            {
                return collection;
            }

            return new OverloadsCollection(emitter, indexerDeclaration, isSetter);
        }

        public static OverloadsCollection Create(IEmitter emitter, OperatorDeclaration operatorDeclaration)
        {
            OverloadsCollection collection;

            if (emitter.Cache.TryGetNode(operatorDeclaration, false, out collection))
            {
                return collection;
            }

            return new OverloadsCollection(emitter, operatorDeclaration);
        }

        public static OverloadsCollection Create(IEmitter emitter, IMember member, bool isSetter = false, bool includeInline = false)
        {
            OverloadsCollection collection;

            if (emitter.Cache.TryGetMember(member, isSetter, includeInline, out collection))
            {
                return collection;
            }

            return new OverloadsCollection(emitter, member, isSetter, includeInline);
        }

        public IEmitter Emitter
        {
            get;
            private set;
        }

        public IType Type
        {
            get;
            private set;
        }

        public ITypeDefinition TypeDefinition
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public string JsName
        {
            get;
            private set;
        }

        public string AltJsName
        {
            get;
            private set;
        }

        public string FieldJsName
        {
            get;
            private set;
        }

        public string ParametersCount
        {
            get;
            private set;
        }

        public bool Static
        {
            get;
            private set;
        }

        public bool Inherit
        {
            get;
            private set;
        }

        public bool Constructor
        {
            get;
            private set;
        }

        public bool IsSetter
        {
            get;
            private set;
        }

        public bool IncludeInline
        {
            get;
            private set;
        }

        public IMember Member
        {
            get;
            private set;
        }

        public IMember OriginalMember{ get; set; }

        private OverloadsCollection(IEmitter emitter, FieldDeclaration fieldDeclaration)
        {
            Emitter = emitter;
            Name = emitter.GetFieldName(fieldDeclaration);
            JsName = Emitter.GetEntityName(fieldDeclaration);
            Inherit = !fieldDeclaration.HasModifier(Modifiers.Static);
            Static = fieldDeclaration.HasModifier(Modifiers.Static);
            Member = FindMember(fieldDeclaration);
            TypeDefinition = Member.DeclaringTypeDefinition;
            Type = Member.DeclaringType;
            InitMembers();
            Emitter.Cache.AddNode(fieldDeclaration, false, this);
        }

        private OverloadsCollection(IEmitter emitter, EventDeclaration eventDeclaration)
        {
            Emitter = emitter;
            Name = emitter.GetEventName(eventDeclaration);
            JsName = Emitter.GetEntityName(eventDeclaration);
            Inherit = !eventDeclaration.HasModifier(Modifiers.Static);
            Static = eventDeclaration.HasModifier(Modifiers.Static);
            Member = FindMember(eventDeclaration);
            TypeDefinition = Member.DeclaringTypeDefinition;
            Type = Member.DeclaringType;
            InitMembers();
            Emitter.Cache.AddNode(eventDeclaration, false, this);
        }

        private OverloadsCollection(IEmitter emitter, CustomEventDeclaration eventDeclaration, bool remove)
        {
            Emitter = emitter;
            Name = eventDeclaration.Name;
            JsName = Helpers.GetEventRef(eventDeclaration, emitter, remove, true);
            AltJsName = Helpers.GetEventRef(eventDeclaration, emitter, !remove, true);
            FieldJsName = emitter.GetEntityName(eventDeclaration);
            Inherit = !eventDeclaration.HasModifier(Modifiers.Static);
            IsSetter = remove;
            Static = eventDeclaration.HasModifier(Modifiers.Static);
            Member = FindMember(eventDeclaration);
            FieldJsName = Helpers.GetEventRef(Member, emitter, true, true, true, false, true); ;
            TypeDefinition = Member.DeclaringTypeDefinition;
            Type = Member.DeclaringType;
            InitMembers();
            Emitter.Cache.AddNode(eventDeclaration, remove, this);
        }

        private OverloadsCollection(IEmitter emitter, MethodDeclaration methodDeclaration)
        {
            Emitter = emitter;
            Name = methodDeclaration.Name;
            JsName = Emitter.GetEntityName(methodDeclaration);
            Inherit = !methodDeclaration.HasModifier(Modifiers.Static);
            Static = methodDeclaration.HasModifier(Modifiers.Static);
            Member = FindMember(methodDeclaration);
            TypeDefinition = Member.DeclaringTypeDefinition;
            Type = Member.DeclaringType;
            InitMembers();
            Emitter.Cache.AddNode(methodDeclaration, false, this);
        }

        private OverloadsCollection(IEmitter emitter, ConstructorDeclaration constructorDeclaration)
        {
            Emitter = emitter;
            Name = constructorDeclaration.Name;
            JsName = Emitter.GetEntityName(constructorDeclaration);
            Inherit = false;
            Constructor = true;
            Static = constructorDeclaration.HasModifier(Modifiers.Static);
            Member = FindMember(constructorDeclaration);
            TypeDefinition = Member.DeclaringTypeDefinition;
            Type = Member.DeclaringType;
            InitMembers();
            Emitter.Cache.AddNode(constructorDeclaration, false, this);
        }

        private OverloadsCollection(IEmitter emitter, PropertyDeclaration propDeclaration, bool isSetter, bool isField)
        {
            Emitter = emitter;
            IsField = isField;
            Name = propDeclaration.Name;
            JsName = Helpers.GetPropertyRef(propDeclaration, emitter, isSetter, true, true);
            AltJsName = Helpers.GetPropertyRef(propDeclaration, emitter, !isSetter, true, true);
            FieldJsName = propDeclaration.Getter != null && propDeclaration.Getter.Body.IsNull ? emitter.GetEntityName(propDeclaration) : null;
            Inherit = !propDeclaration.HasModifier(Modifiers.Static);
            Static = propDeclaration.HasModifier(Modifiers.Static);
            IsSetter = isSetter;
            Member = FindMember(propDeclaration);
            var p = (IProperty)Member;
            FieldJsName = Emitter.GetEntityName(p);
            TypeDefinition = Member.DeclaringTypeDefinition;
            Type = Member.DeclaringType;
            InitMembers();
            Emitter.Cache.AddNode(propDeclaration, isSetter, this);
        }

        private OverloadsCollection(IEmitter emitter, IndexerDeclaration indexerDeclaration, bool isSetter)
        {
            Emitter = emitter;
            Name = indexerDeclaration.Name;
            JsName = Helpers.GetPropertyRef(indexerDeclaration, emitter, isSetter, true, true);
            AltJsName = Helpers.GetPropertyRef(indexerDeclaration, emitter, !isSetter, true, true);
            Inherit = true;
            Static = false;
            IsSetter = isSetter;
            Member = FindMember(indexerDeclaration);
            TypeDefinition = Member.DeclaringTypeDefinition;
            Type = Member.DeclaringType;
            InitMembers();
            Emitter.Cache.AddNode(indexerDeclaration, isSetter, this);
        }

        private OverloadsCollection(IEmitter emitter, OperatorDeclaration operatorDeclaration)
        {
            Emitter = emitter;
            Name = operatorDeclaration.Name;
            JsName = Emitter.GetEntityName(operatorDeclaration);
            Inherit = !operatorDeclaration.HasModifier(Modifiers.Static);
            Static = operatorDeclaration.HasModifier(Modifiers.Static);
            Member = FindMember(operatorDeclaration);
            TypeDefinition = Member.DeclaringTypeDefinition;
            Type = Member.DeclaringType;
            InitMembers();
            Emitter.Cache.AddNode(operatorDeclaration, false, this);
        }

        private OverloadsCollection(IEmitter emitter, IMember member, bool isSetter = false, bool includeInline = false, bool isField = false)
        {
            if (member is IMethod method)
            {
                Inherit = !method.IsConstructor && !method.IsStatic;
                Static = method.IsStatic;
                Constructor = method.IsConstructor;
            }
            else if (member is IEntity entity)
            {
                Inherit = !entity.IsStatic;
                Static = entity.IsStatic;
            }

            Emitter = emitter;
            Name = member.Name;
            IsField = isField;

            if (member is IProperty)
            {
                JsName = Helpers.GetPropertyRef(member, emitter, isSetter, true, true);
                AltJsName = Helpers.GetPropertyRef(member, emitter, !isSetter, true, true);
                var p = (IProperty) member;
                FieldJsName = Emitter.GetEntityName(p);
            }
            else if (member is IEvent)
            {
                JsName = Helpers.GetEventRef(member, emitter, isSetter, true, true);
                AltJsName = Helpers.GetEventRef(member, emitter, !isSetter, true, true);
                FieldJsName = Helpers.GetEventRef(member, emitter, true, true, true, false, true);
            }
            else
            {
                JsName = Emitter.GetEntityName(member);
            }

            IncludeInline = includeInline;
            Member = member;
            TypeDefinition = Member.DeclaringTypeDefinition;
            Type = Member.DeclaringType;
            IsSetter = isSetter;
            InitMembers();
            Emitter.Cache.AddMember(member, isSetter, includeInline, this);
        }

        public bool IsField { get; set; }

        public List<IMethod> Methods
        {
            get;
            private set;
        }

        public List<IField> Fields
        {
            get;
            private set;
        }

        public List<IProperty> Properties
        {
            get;
            private set;
        }

        public List<IEvent> Events
        {
            get;
            private set;
        }

        public bool HasOverloads
        {
            get
            {
                return Members.Count > 1;
            }
        }

        protected virtual int GetIndex(IMember member)
        {
            var originalMember = member;

            while (member != null && member.IsOverride && !IsTemplateOverride(member))
            {
                member = InheritanceHelper.GetBaseMember(member);
            }

            if (member == null)
            {
                member = originalMember;
            }

            return Members.IndexOf(member.MemberDefinition);
        }

        private List<IMember> members;

        public List<IMember> Members
        {
            get
            {
                InitMembers();
                return members;
            }
        }

        protected virtual void InitMembers()
        {
            if (members == null)
            {
                Properties = GetPropertyOverloads();
                Events = GetEventOverloads();
                Methods = GetMethodOverloads();
                Fields = GetFieldOverloads();

                members = new List<IMember>();
                members.AddRange(Methods);
                members.AddRange(Properties);
                members.AddRange(Fields);
                members.AddRange(Events);

                SortMembersOverloads();
            }
        }

        protected virtual void SortMembersOverloads()
        {
            Members.Sort((m1, m2) =>
            {
                if (m1.DeclaringType != m2.DeclaringType)
                {
                    return m1.DeclaringTypeDefinition.IsDerivedFrom(m2.DeclaringTypeDefinition) ? 1 : -1;
                }

                var iCount1 = m1.ImplementedInterfaceMembers.Count;
                var iCount2 = m2.ImplementedInterfaceMembers.Count;
                if (iCount1 > 0 && iCount2 == 0)
                {
                    return -1;
                }

                if (iCount2 > 0 && iCount1 == 0)
                {
                    return 1;
                }

                if (iCount1 > 0 && iCount2 > 0)
                {
                    foreach (var im1 in m1.ImplementedInterfaceMembers)
                    {
                        foreach (var im2 in m2.ImplementedInterfaceMembers)
                        {
                            if (im1.DeclaringType != im2.DeclaringType)
                            {
                                if (im1.DeclaringTypeDefinition.IsDerivedFrom(im2.DeclaringTypeDefinition))
                                {
                                    return 1;
                                }

                                if (im2.DeclaringTypeDefinition.IsDerivedFrom(im1.DeclaringTypeDefinition))
                                {
                                    return -1;
                                }
                            }
                        }
                    }
                }

                var method1 = m1 as IMethod;
                var method2 = m2 as IMethod;

                if ((method1 != null && method1.IsConstructor) &&
                    (method2 == null || !method2.IsConstructor))
                {
                    return -1;
                }

                if ((method2 != null && method2.IsConstructor) &&
                    (method1 == null || !method1.IsConstructor))
                {
                    return 1;
                }

                if ((method1 != null && method1.IsConstructor) &&
                    (method2 != null && method2.IsConstructor))
                {
                    return string.Compare(MemberToString(m1), MemberToString(m2));
                }

                var a1 = GetAccessibilityWeight(m1.Accessibility);
                var a2 = GetAccessibilityWeight(m2.Accessibility);
                if (a1 != a2)
                {
                    return a1.CompareTo(a2);
                }

                var v1 = m1 is IField ? 1 : (m1 is IEvent ? 2 : (m1 is IProperty ? 3 : (m1 is IMethod ? 4 : 5)));
                var v2 = m2 is IField ? 1 : (m2 is IEvent ? 2 : (m2 is IProperty ? 3 : (m2 is IMethod ? 4 : 5)));

                if (v1 != v2)
                {
                    return v1.CompareTo(v2);
                }

                var name1 = MemberToString(m1);
                var name2 = MemberToString(m2);

                return string.Compare(name1, name2);
            });
        }

        protected virtual int GetAccessibilityWeight(Accessibility a)
        {
            int w = 0;
            switch (a)
            {
                case Accessibility.None:
                    w = 4;
                    break;

                case Accessibility.Private:
                    w = 4;
                    break;

                case Accessibility.Public:
                    w = 1;
                    break;

                case Accessibility.Protected:
                    w = 3;
                    break;

                case Accessibility.Internal:
                    w = 2;
                    break;

                case Accessibility.ProtectedOrInternal:
                    w = 2;
                    break;

                case Accessibility.ProtectedAndInternal:
                    w = 3;
                    break;
            }

            return w;
        }

        protected virtual string MemberToString(IMember member)
        {
            if (member is IMethod)
            {
                return MethodToString((IMethod)member);
            }

            return member.Name;
        }

        protected virtual string MethodToString(IMethod m)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(m.ReturnType.ToString()).Append(" ");
            sb.Append(m.Name).Append(" ");
            sb.Append(m.TypeParameters.Count).Append(" ");

            foreach (var p in m.Parameters)
            {
                sb.Append(p.Type.ToString()).Append(" ");
            }

            return sb.ToString();
        }

        public virtual bool IsTemplateOverride(IMember member)
        {
            if (member.IsOverride)
            {
                member = InheritanceHelper.GetBaseMember(member);

                if (member != null)
                {
                    var inline = Emitter.GetInline(member);
                    bool isInline = !string.IsNullOrWhiteSpace(inline);
                    if (isInline)
                    {
                        if (member.IsOverride)
                        {
                            return IsTemplateOverride(member);
                        }
                        return true;
                    }
                }
            }

            return false;
        }

        protected virtual List<IMethod> GetMethodOverloads(List<IMethod> list = null, ITypeDefinition typeDef = null)
        {
            typeDef = typeDef ?? TypeDefinition;

            bool isTop = list == null;
            list = list ?? new List<IMethod>();
            var toStringOverride = (JsName == "toString" && Member is IMethod && ((IMethod)Member).Parameters.Count == 0);
            if (Member != null && Member.IsOverride && (!IsTemplateOverride(Member) || toStringOverride))
            {
                if (OriginalMember == null)
                {
                    OriginalMember = Member;
                }

                Member = InheritanceHelper.GetBaseMember(Member);
                typeDef = Member.DeclaringTypeDefinition;
            }

            if (typeDef != null)
            {
                var isExternalType = Emitter.Validator.IsExternalType(typeDef);
                bool externalFound = false;

                var oldIncludeInline = IncludeInline;
                if (toStringOverride)
                {
                    IncludeInline = true;
                }

                var methods = typeDef.Methods.Where(m =>
                {
                    if (m.IsExplicitInterfaceImplementation)
                    {
                        return false;
                    }

                    if (!IncludeInline)
                    {
                        var inline = Emitter.GetInline(m);
                        if (!string.IsNullOrWhiteSpace(inline) && !(m.Name == "ToString" && m.Parameters.Count == 0 && !m.IsOverride))
                        {
                            return false;
                        }
                    }

                    var name = Emitter.GetEntityName(m);
                    if ((name == JsName || name == AltJsName || name == FieldJsName) && m.IsStatic == Static &&
                        (m.IsConstructor && JsName == JS.Funcs.CONSTRUCTOR || m.IsConstructor == Constructor))
                    {
                        if (m.IsConstructor != Constructor && (m.Parameters.Count > 0 || m.DeclaringTypeDefinition != TypeDefinition))
                        {
                            return false;
                        }

                        if (m.IsOverride && (!IsTemplateOverride(m) || m.Name == "ToString" && m.Parameters.Count == 0))
                        {
                            return false;
                        }

                        if (!isExternalType)
                        {
                            var isExtern = !m.HasBody && !m.IsAbstract || Emitter.Validator.IsExternalType(m);
                            if (isExtern)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (externalFound)
                            {
                                return false;
                            }

                            externalFound = true;
                        }

                        return true;
                    }

                    return false;
                });

                IncludeInline = oldIncludeInline;

                list.AddRange(methods);

                if (Inherit)
                {
                    var baseTypeDefinitions = typeDef.DirectBaseTypes.Where(t => t.Kind == typeDef.Kind || (typeDef.Kind == TypeKind.Struct && t.Kind == TypeKind.Class));

                    foreach (var baseTypeDef in baseTypeDefinitions)
                    {
                        list = GetMethodOverloads(list, baseTypeDef.GetDefinition());
                    }
                }
            }

            var returnMethods = isTop ? list.Distinct().ToList() : list;
            return returnMethods;
        }

        protected virtual List<IProperty> GetPropertyOverloads(List<IProperty> list = null, ITypeDefinition typeDef = null)
        {
            typeDef = typeDef ?? TypeDefinition;

            bool isTop = list == null;
            list = list ?? new List<IProperty>();

            if (Member != null && Member.IsOverride && !IsTemplateOverride(Member))
            {
                if (OriginalMember == null)
                {
                    OriginalMember = Member;
                }

                Member = InheritanceHelper.GetBaseMember(Member);
                typeDef = Member.DeclaringTypeDefinition;
            }

            if (typeDef != null)
            {
                bool isMember = Member is IMethod;
                var properties = typeDef.Properties.Where(p =>
                {
                    if (p.IsExplicitInterfaceImplementation)
                    {
                        return false;
                    }

                    var canGet = p.CanGet && p.Getter != null;
                    var canSet = p.CanSet && p.Setter != null;

                    if (!IncludeInline)
                    {
                        var inline = canGet ? Emitter.GetInline(p.Getter) : null;
                        if (!string.IsNullOrWhiteSpace(inline))
                        {
                            return false;
                        }

                        inline = canSet ? Emitter.GetInline(p.Setter) : null;
                        if (!string.IsNullOrWhiteSpace(inline))
                        {
                            return false;
                        }

                        if (p.IsIndexer && canGet && p.Getter.Attributes.Any(a => a.AttributeType.FullName == "H5.ExternalAttribute"))
                        {
                            return false;
                        }
                    }

                    bool eq = false;
                    bool? equalsByGetter = null;

                    if (p.IsStatic == Static)
                    {
                        var fieldName = Emitter.GetEntityName(p);

                        if (fieldName != null && (fieldName == JsName || fieldName == AltJsName || fieldName == FieldJsName))
                        {
                            eq = true;
                        }

                        if (!eq && p.IsIndexer)
                        {
                            var getterIgnore = canGet && Emitter.Validator.IsExternalType(p.Getter);
                            var setterIgnore = canSet && Emitter.Validator.IsExternalType(p.Setter);
                            var getterName = canGet ? Helpers.GetPropertyRef(p, Emitter, false, true, true) : null;
                            var setterName = canSet ? Helpers.GetPropertyRef(p, Emitter, true, true, true) : null;

                            if (!getterIgnore && getterName != null && (getterName == JsName || getterName == AltJsName || getterName == FieldJsName))
                            {
                                eq = true;
                                equalsByGetter = true;
                            }
                            else if (!setterIgnore && setterName != null && (setterName == JsName || setterName == AltJsName || setterName == FieldJsName))
                            {
                                eq = true;
                                equalsByGetter = false;
                            }
                        }
                    }

                    if (eq)
                    {
                        if (p.IsOverride && !IsTemplateOverride(p))
                        {
                            return false;
                        }

                        if (equalsByGetter.HasValue && isMember && AltJsName == null)
                        {
                            AltJsName = Helpers.GetPropertyRef(p, Emitter, equalsByGetter.Value, true, true);
                        }

                        return true;
                    }

                    return false;
                });

                list.AddRange(properties);

                if (Inherit)
                {
                    var baseTypeDefinitions = typeDef.DirectBaseTypes.Where(t => t.Kind == typeDef.Kind || (typeDef.Kind == TypeKind.Struct && t.Kind == TypeKind.Class));

                    foreach (var baseTypeDef in baseTypeDefinitions)
                    {
                        list = GetPropertyOverloads(list, baseTypeDef.GetDefinition());
                    }
                }
            }

            var returnProperties = isTop ? list.Distinct().ToList() : list;
            return returnProperties;
        }

        protected virtual List<IField> GetFieldOverloads(List<IField> list = null, ITypeDefinition typeDef = null)
        {
            typeDef = typeDef ?? TypeDefinition;

            bool isTop = list == null;
            list = list ?? new List<IField>();

            if (typeDef != null)
            {
                var fields = typeDef.Fields.Where(f =>
                {
                    if (f.IsExplicitInterfaceImplementation)
                    {
                        return false;
                    }

                    var inline = Emitter.GetInline(f);
                    if (!string.IsNullOrWhiteSpace(inline))
                    {
                        return false;
                    }

                    var name = Emitter.GetEntityName(f);
                    if ((name == JsName || name == AltJsName || name == FieldJsName) && f.IsStatic == Static)
                    {
                        return true;
                    }

                    return false;
                });

                list.AddRange(fields);

                if (Inherit)
                {
                    var baseTypeDefinitions = typeDef.DirectBaseTypes.Where(t => t.Kind == typeDef.Kind || (typeDef.Kind == TypeKind.Struct && t.Kind == TypeKind.Class));

                    foreach (var baseTypeDef in baseTypeDefinitions)
                    {
                        list = GetFieldOverloads(list, baseTypeDef.GetDefinition());
                    }
                }
            }

            var returnFields = isTop ? list.Distinct().ToList() : list;
            return returnFields;
        }

        protected virtual List<IEvent> GetEventOverloads(List<IEvent> list = null, ITypeDefinition typeDef = null)
        {
            typeDef = typeDef ?? TypeDefinition;

            bool isTop = list == null;
            list = list ?? new List<IEvent>();

            if (typeDef != null)
            {
                var events = typeDef.Events.Where(e =>
                {
                    if (e.IsExplicitInterfaceImplementation)
                    {
                        return false;
                    }

                    var inline = e.AddAccessor != null ? Emitter.GetInline(e.AddAccessor) : null;
                    if (!string.IsNullOrWhiteSpace(inline))
                    {
                        return false;
                    }

                    inline = e.RemoveAccessor != null ? Emitter.GetInline(e.RemoveAccessor) : null;
                    if (!string.IsNullOrWhiteSpace(inline))
                    {
                        return false;
                    }

                    bool eq = false;
                    bool? equalsByAdd = null;
                    if (e.IsStatic == Static)
                    {
                        var addName = e.AddAccessor != null && e.CanAdd ? Helpers.GetEventRef(e, Emitter, false, true, true) : null;
                        var removeName = e.RemoveAccessor != null && e.CanRemove ? Helpers.GetEventRef(e, Emitter, true, true, true) : null;
                        var fieldName = Helpers.GetEventRef(e, Emitter, true, true, true, false, true);
                        if (addName != null && (addName == JsName || addName == AltJsName || addName == FieldJsName))
                        {
                            eq = true;
                            equalsByAdd = true;
                        }
                        else if (removeName != null && (removeName == JsName || removeName == AltJsName || removeName == FieldJsName))
                        {
                            eq = true;
                            equalsByAdd = false;
                        }
                        else if (fieldName != null && (fieldName == JsName || fieldName == AltJsName || fieldName == FieldJsName))
                        {
                            eq = true;
                        }
                    }

                    if (eq)
                    {
                        if (e.IsOverride && !IsTemplateOverride(e))
                        {
                            return false;
                        }

                        if (equalsByAdd.HasValue && Member is IMethod && AltJsName == null)
                        {
                            AltJsName = Helpers.GetEventRef(e, Emitter, equalsByAdd.Value, true, true);
                        }

                        return true;
                    }

                    return false;
                });

                list.AddRange(events);

                if (Inherit)
                {
                    var baseTypeDefinitions = typeDef.DirectBaseTypes.Where(t => t.Kind == typeDef.Kind || (typeDef.Kind == TypeKind.Struct && t.Kind == TypeKind.Class));

                    foreach (var baseTypeDef in baseTypeDefinitions)
                    {
                        list = GetEventOverloads(list, baseTypeDef.GetDefinition());
                    }
                }
            }

            var returnEvents = isTop ? list.Distinct().ToList() : list;
            return returnEvents;
        }

        private Dictionary<Tuple<bool, string, bool, bool>, string> overloadName = new Dictionary<Tuple<bool, string, bool, bool>, string>();

        public string GetOverloadName(bool skipInterfaceName = false, string prefix = null, bool withoutTypeParams = false, bool isObjectLiteral = false, bool excludeTypeOnly = false)
        {
            if (Member == null)
            {
                if (Members.Count == 1)
                {
                    Member = Members[0];
                }
                else
                {
                    return JsName;
                }
            }

            var key = new Tuple<bool, string, bool, bool>(skipInterfaceName, prefix, withoutTypeParams, isObjectLiteral);
            string name = null;
            var contains = overloadName.ContainsKey(key);
            if (!contains && Member != null)
            {
                name = GetOverloadName(Member, skipInterfaceName, prefix, withoutTypeParams, isObjectLiteral, excludeTypeOnly);
                overloadName[key] = name;
            }
            else if (contains)
            {
                name = overloadName[key];
            }

            return name;
        }

        public static string NormalizeInterfaceName(string interfaceName)
        {
            return Regex.Replace(interfaceName, @"[\.\(\)\,]", JS.Vars.D.ToString());
        }

        public static string GetInterfaceMemberName(IEmitter emitter, IMember interfaceMember, string name, string prefix, bool withoutTypeParams = false, bool isSetter = false, bool excludeTypeOnly = false)
        {
            var interfaceMemberName = name ?? OverloadsCollection.Create(emitter, interfaceMember, isSetter).GetOverloadName(true, prefix);
            var interfaceName = H5Types.ToJsName(interfaceMember.DeclaringType, emitter, false, false, true, withoutTypeParams, excludeTypeOnly: excludeTypeOnly);

            if (interfaceName.StartsWith("\""))
            {
                if (interfaceName.EndsWith(")"))
                {
                    return $"{interfaceName} + \"{JS.Vars.D}{interfaceMemberName}\"";
                }

                if (interfaceName.EndsWith("\""))
                {
                    interfaceName = interfaceName.Substring(0, interfaceName.Length - 1);
                }

                return $"{interfaceName}{JS.Vars.D}{interfaceMemberName}\"";
            }

            return $"{interfaceName}{(interfaceName.EndsWith(JS.Vars.D.ToString()) ? "" : JS.Vars.D.ToString())}{interfaceMemberName}";
        }

        public static bool ExcludeTypeParameterForDefinition(MemberResolveResult rr)
        {
            return rr != null && OverloadsCollection.ExcludeTypeParameterForDefinition(rr.Member);
        }

        public static bool ExcludeTypeParameterForDefinition(IMember member)
        {
            if (member.ImplementedInterfaceMembers.Count == 0)
            {
                return false;
            }

            if (member.ImplementedInterfaceMembers.Any(im =>
                {
                    var typeDef = im.DeclaringTypeDefinition;
                    var type = im.DeclaringType;

                    return typeDef != null && !Helpers.IsIgnoreGeneric(typeDef) && type != null &&
                           type.TypeArguments.Count > 0 && Helpers.IsTypeParameterType(type);
                }))
            {
                return true;
            }

            return false;
        }

        public static bool NeedCreateAlias(MemberResolveResult rr)
        {
            if (rr == null || rr.Member.ImplementedInterfaceMembers.Count == 0)
            {
                return false;
            }

            if (rr.Member.ImplementedInterfaceMembers.Count > 0 &&
                rr.Member.ImplementedInterfaceMembers.Any(im => im.DeclaringTypeDefinition.TypeParameters.Any(tp => tp.Variance != VarianceModifier.Invariant)))
            {
                return true;
            }

            if (rr.Member.IsExplicitInterfaceImplementation)
            {
                var explicitInterfaceMember = rr.Member.ImplementedInterfaceMembers.First();
                var typeDef = explicitInterfaceMember.DeclaringTypeDefinition;
                var type = explicitInterfaceMember.DeclaringType;

                return typeDef != null && !Helpers.IsIgnoreGeneric(typeDef) && type != null && type.TypeArguments.Count > 0 && Helpers.IsTypeParameterType(type);
            }

            return true;
        }

        protected virtual string GetOverloadName(IMember definition, bool skipInterfaceName = false, string prefix = null, bool withoutTypeParams = false, bool isObjectLiteral = false, bool excludeTypeOnly = false)
        {
            IMember interfaceMember = null;
            if (definition.IsExplicitInterfaceImplementation)
            {
                interfaceMember = definition.ImplementedInterfaceMembers.First();
            }
            else if (definition.DeclaringTypeDefinition != null && definition.DeclaringTypeDefinition.Kind == TypeKind.Interface)
            {
                interfaceMember = definition;
            }

            if (interfaceMember != null && !skipInterfaceName && !Emitter.Validator.IsObjectLiteral(interfaceMember.DeclaringTypeDefinition))
            {
                return OverloadsCollection.GetInterfaceMemberName(Emitter, interfaceMember, null, prefix, withoutTypeParams, IsSetter, excludeTypeOnly);
            }

            string name = isObjectLiteral ? Emitter.GetLiteralEntityName(definition) : Emitter.GetEntityName(definition);
            if (name.StartsWith("." + JS.Funcs.CONSTRUCTOR))
            {
                name = JS.Funcs.CONSTRUCTOR;
            }

            var attr = Helpers.GetInheritedAttribute(definition, "H5.NameAttribute");

            var iProperty = definition as IProperty;

            if (attr == null && iProperty != null && !IsField)
            {
                var acceessor = IsSetter ? iProperty.Setter : iProperty.Getter;

                if (acceessor != null)
                {
                    attr = Helpers.GetInheritedAttribute(acceessor, "H5.NameAttribute");

                    if (attr != null)
                    {
                        name = Emitter.GetEntityName(acceessor);
                    }
                }
            }

            if (attr != null)
            {
                if (!(iProperty != null || definition is IEvent))
                {
                    prefix = null;
                }
            }

            if (attr != null && definition.ImplementedInterfaceMembers.Count > 0)
            {
                if (Members.Where(member => member.ImplementedInterfaceMembers.Count > 0)
                        .Any(member => definition.ImplementedInterfaceMembers.Any(implementedInterfaceMember => member.ImplementedInterfaceMembers.Any(m => m.DeclaringTypeDefinition == implementedInterfaceMember.DeclaringTypeDefinition))))
                {
                    attr = null;
                }
            }

            bool skipSuffix = false;
            if (definition.DeclaringTypeDefinition != null &&
                Emitter.Validator.IsExternalType(definition.DeclaringTypeDefinition))
            {
                if (definition.DeclaringTypeDefinition.Kind == TypeKind.Interface)
                {
                    skipSuffix = definition.DeclaringTypeDefinition.ParentAssembly.AssemblyName != CS.NS.H5;
                }
                else
                {
                    skipSuffix = true;
                }
            }

            if (attr != null || skipSuffix)
            {
                return prefix != null ? $"{prefix}{name}" : name;
            }

            var isCtor = definition is IMethod iDefinition && iDefinition.IsConstructor;

            if (isCtor)
            {
                name = JS.Funcs.CONSTRUCTOR;
            }

            var index = GetIndex(definition);

            if (index > 0)
            {
                if (isCtor)
                {
                    name = $"{JS.Vars.D}{name}{index}";
                }
                else
                {
                    name += Helpers.PrefixDollar(index);
                    name = Helpers.ReplaceFirstDollar(name);
                }
            }

            return prefix != null ? $"{prefix}{name}" : name;
        }

        protected virtual IMember FindMember(EntityDeclaration entity)
        {
            if (Emitter.Resolver.ResolveNode(entity) is MemberResolveResult rr)
            {
                return rr.Member;
            }

            return null;
        }
    }
}