using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using System;
using System.Collections.Generic;
using System.Linq;

namespace H5.Translator
{
    public partial class Inspector : Visitor
    {
        public override void VisitSyntaxTree(SyntaxTree node)
        {
            node.AcceptChildren(this);
        }

        public override void VisitUsingDeclaration(UsingDeclaration usingDeclaration)
        {
        }

        public override void VisitNamespaceDeclaration(NamespaceDeclaration namespaceDeclaration)
        {
            if (!String.IsNullOrEmpty(Namespace))
            {
                //throw (EmitterException)this.CreateException(namespaceDeclaration, "Nested namespaces are not supported");
            }

            if (!AssemblyInfo.Assembly.EnableReservedNamespaces)
            {
                ValidateNamespace(namespaceDeclaration);
            }

            var prevNamespace = Namespace;

            Namespace = namespaceDeclaration.Name;

            namespaceDeclaration.AcceptChildren(this);

            Namespace = prevNamespace;
        }

        public override void VisitTypeDeclaration(TypeDeclaration typeDeclaration)
        {
            if (CurrentType != null)
            {
                NestedTypes = NestedTypes ?? new List<Tuple<TypeDeclaration, ITypeInfo>>();
                NestedTypes.Add(new Tuple<TypeDeclaration, ITypeInfo>(typeDeclaration, CurrentType));
                return;
            }

            ValidateNamespace(typeDeclaration);

            var rr = Resolver.ResolveNode(typeDeclaration);
            var fullName = rr.Type.ReflectionName;
            var partialType = Types.FirstOrDefault(t => t.Key == fullName);
            var add = true;
            var ignored = IgnoredTypes.Contains(fullName);
            var external = HasExternal(typeDeclaration);

            if (!external)
            {
                var resolveResult = Resolver.ResolveNode(typeDeclaration);
                if (resolveResult != null && resolveResult.Type != null)
                {
                    var def = resolveResult.Type.GetDefinition();

                    external =  def != null && (Validator.IsTypeFromH5Core(def.FullName) || def.ParentAssembly.AssemblyAttributes.Any(a => a.AttributeType.FullName == "H5.ExternalAttribute"));
                }
            }

            if ((external || ignored || IsNonScriptable(typeDeclaration)) && !IsObjectLiteral(typeDeclaration))
            {
                if (partialType != null)
                {
                    Types.Remove(partialType);
                }

                if (!ignored)
                {
                    IgnoredTypes.Add(fullName);
                }

                return;
            }

            if (partialType == null)
            {
                ITypeInfo parentTypeInfo = null;
                var parentTypeDeclaration = typeDeclaration.GetParent<TypeDeclaration>();
                if (parentTypeDeclaration != null)
                {
                    var rr1 = Resolver.ResolveNode(parentTypeDeclaration);
                    var parentName = rr1.Type.ReflectionName;
                    parentTypeInfo = Types.FirstOrDefault(t => t.Key == parentName);
                }

                CurrentType = new TypeInfo()
                {
                    Key = rr.Type.ReflectionName,
                    TypeDeclaration = typeDeclaration,
                    ParentType = parentTypeInfo,
                    Name = typeDeclaration.Name,
                    ClassType = typeDeclaration.ClassType,
                    Namespace = Namespace,
                    IsEnum = typeDeclaration.ClassType == ClassType.Enum,
                    IsStatic = typeDeclaration.ClassType == ClassType.Enum || typeDeclaration.HasModifier(Modifiers.Static),
                    IsObjectLiteral = IsObjectLiteral(typeDeclaration),
                    Type = rr.Type
                };
            }
            else
            {
                CurrentType = partialType;
                CurrentType.PartialTypeDeclarations.Add(typeDeclaration);
                add = false;
            }

            if (typeDeclaration.ClassType != ClassType.Interface)
            {
                typeDeclaration.AcceptChildren(this);
            }
            else
            {
                typeDeclaration.AcceptChildren(this);
            }

            if (add)
            {
                Types.Add(CurrentType);
            }

            if (typeDeclaration.ClassType != ClassType.Interface)
            {
                AddMissingAliases(typeDeclaration);
            }

            CurrentType = null;

            while (NestedTypes != null && NestedTypes.Count > 0)
            {
                var types = NestedTypes;
                NestedTypes = null;
                foreach (var nestedType in types)
                {
                    VisitTypeDeclaration(nestedType.Item1);
                }
            }
        }

        private void AddMissingAliases(TypeDeclaration typeDeclaration)
        {
            var type = Resolver.ResolveNode(typeDeclaration).Type;
            var interfaces = type.DirectBaseTypes.Where(t => t.Kind == TypeKind.Interface).ToArray();
            var members = type.GetMembers(null, GetMemberOptions.IgnoreInheritedMembers).ToArray();
            var baseTypes = type.GetNonInterfaceBaseTypes().ToArray().Reverse();

            if (interfaces.Length > 0)
            {
                foreach (var baseInterface in interfaces)
                {
                    var interfaceMembers = baseInterface.GetMembers().Where(m => m.DeclaringTypeDefinition.Kind == TypeKind.Interface);
                    foreach (var interfaceMember in interfaceMembers)
                    {
                        var isDirectlyImplemented = members.Any(m => m.ImplementedInterfaceMembers.Contains(interfaceMember));
                        if (!isDirectlyImplemented)
                        {
                            foreach (var baseType in baseTypes)
                            {
                                //var derivedMember = InheritanceHelper.GetDerivedMember(interfaceMember, baseType.GetDefinition());
                                IMember derivedMember = null;
                                IEnumerable<IMember> baseMembers;
                                if (interfaceMember.SymbolKind == SymbolKind.Accessor)
                                {
                                    baseMembers = baseType.GetAccessors(null, GetMemberOptions.IgnoreInheritedMembers).Where(m => m.Name == interfaceMember.Name && TypeComparer.Equals(m.ReturnType, interfaceMember.ReturnType));
                                }
                                else
                                {
                                    baseMembers = baseType.GetMembers(null, GetMemberOptions.IgnoreInheritedMembers).Where(m => m.Name == interfaceMember.Name && TypeComparer.Equals(m.ReturnType, interfaceMember.ReturnType));
                                }

                                foreach (IMember baseMember in baseMembers)
                                {
                                    if (baseMember.IsPrivate)
                                    {
                                        continue;
                                    }
                                    if (SignatureComparer.Ordinal.Equals(interfaceMember, baseMember))
                                    {
                                        derivedMember = baseMember.Specialize(interfaceMember.Substitution);
                                        break;
                                    }
                                }

                                if (derivedMember != null && !derivedMember.ImplementedInterfaceMembers.Contains(interfaceMember) && !CurrentType.InstanceConfig.Alias.Any(a => typeDeclaration.Equals(a.Entity) && interfaceMember.Equals(a.InterfaceMember) && derivedMember.Equals(a.DerivedMember)))
                                {
                                    CurrentType.InstanceConfig.Alias.Add(new TypeConfigItem { Entity = typeDeclaration, InterfaceMember = interfaceMember, DerivedMember = derivedMember });
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public override void VisitFieldDeclaration(FieldDeclaration fieldDeclaration)
        {
            bool isStatic = CurrentType.ClassType == ClassType.Enum
                || fieldDeclaration.HasModifier(Modifiers.Static)
                || fieldDeclaration.HasModifier(Modifiers.Const);

            foreach (var item in fieldDeclaration.Variables)
            {
                var rr = Resolver.ResolveNode(item) as MemberResolveResult;
                if (fieldDeclaration.HasModifier(Modifiers.Const) && rr != null && rr.Member.Attributes.Any(a => a.AttributeType.FullName == Translator.H5_ASSEMBLY + ".InlineConstAttribute"))
                {
                    continue;
                }

                Expression initializer = item.Initializer;

                if (initializer.IsNull)
                {
                    if (CurrentType.ClassType == ClassType.Enum)
                    {
                        throw (EmitterException)CreateException(fieldDeclaration, "Enum items must be explicitly numbered");
                    }

                    initializer = GetDefaultFieldInitializer(fieldDeclaration.ReturnType);
                }

                CurrentType.FieldsDeclarations.Add(item.Name, fieldDeclaration);

                string prefix = SharpSixRewriter.AutoInitFieldPrefix;
                bool autoInitializer = item.Name.StartsWith(prefix);
                string name = autoInitializer ? item.Name.Substring(prefix.Length) : item.Name;

                if (isStatic)
                {
                    var collection = CurrentType.StaticConfig.Fields;
                    if (autoInitializer)
                    {
                        collection = CurrentType.StaticConfig.AutoPropertyInitializers;
                        var prop = CurrentType.StaticConfig.Properties.FirstOrDefault(p => p.Name == name);

                        if (prop == null)
                        {
                            prop = CurrentType.StaticConfig.Fields.FirstOrDefault(p => p.Name == name);
                        }

                        if (prop != null)
                        {
                            prop.Initializer = initializer;
                            prop.IsPropertyInitializer = true;
                        }
                    }

                    collection.Add(new TypeConfigItem
                    {
                        Name = name,
                        Entity = fieldDeclaration,
                        IsConst = fieldDeclaration.HasModifier(Modifiers.Const),
                        VarInitializer = item,
                        Initializer = initializer
                    });
                }
                else
                {
                    var collection = CurrentType.InstanceConfig.Fields;
                    if (autoInitializer)
                    {
                        collection = CurrentType.InstanceConfig.AutoPropertyInitializers;
                        var prop = CurrentType.InstanceConfig.Properties.FirstOrDefault(p => p.Name == name);

                        if (prop == null)
                        {
                            prop = CurrentType.InstanceConfig.Fields.FirstOrDefault(p => p.Name == name);
                        }

                        if (prop != null)
                        {
                            prop.Initializer = initializer;
                            prop.IsPropertyInitializer = true;
                        }
                    }

                    collection.Add(new TypeConfigItem
                    {
                        Name = name,
                        Entity = fieldDeclaration,
                        VarInitializer = item,
                        Initializer = initializer
                    });
                }

                if (OverloadsCollection.NeedCreateAlias(rr))
                {
                    var config = isStatic
                    ? CurrentType.StaticConfig
                    : CurrentType.InstanceConfig;
                    config.Alias.Add(new TypeConfigItem { Entity = fieldDeclaration, VarInitializer = item });
                }
            }
        }

        public override void VisitConstructorDeclaration(ConstructorDeclaration constructorDeclaration)
        {
            if (HasTemplate(constructorDeclaration) || constructorDeclaration.HasModifier(Modifiers.Extern) && !HasScript(constructorDeclaration))
            {
                return;
            }

            bool isStatic = constructorDeclaration.HasModifier(Modifiers.Static);

            FixMethodParameters(constructorDeclaration.Parameters, constructorDeclaration.Body);

            if (isStatic)
            {
                CurrentType.StaticCtor = constructorDeclaration;
            }
            else
            {
                CurrentType.Ctors.Add(constructorDeclaration);
            }
        }

        public override void VisitOperatorDeclaration(OperatorDeclaration operatorDeclaration)
        {
            if (HasTemplate(operatorDeclaration))
            {
                return;
            }

            FixMethodParameters(operatorDeclaration.Parameters, operatorDeclaration.Body);

            Dictionary<OperatorType, List<OperatorDeclaration>> dict = CurrentType.Operators;

            var key = operatorDeclaration.OperatorType;
            if (dict.ContainsKey(key))
            {
                dict[key].Add(operatorDeclaration);
            }
            else
            {
                dict.Add(key, new List<OperatorDeclaration>(new[] { operatorDeclaration }));
            }
        }

        public override void VisitIndexerDeclaration(IndexerDeclaration indexerDeclaration)
        {
            if (indexerDeclaration.HasModifier(Modifiers.Abstract))
            {
                return;
            }

            IDictionary<string, List<EntityDeclaration>> dict = CurrentType.InstanceProperties;

            var key = indexerDeclaration.Name;

            if (dict.ContainsKey(key))
            {
                dict[key].Add(indexerDeclaration);
            }
            else
            {
                dict.Add(key, new List<EntityDeclaration>(new[] { indexerDeclaration }));
            }

            var rr = Resolver.ResolveNode(indexerDeclaration) as MemberResolveResult;
            if (OverloadsCollection.NeedCreateAlias(rr))
            {
                var config = rr.Member.IsStatic
                ? CurrentType.StaticConfig
                : CurrentType.InstanceConfig;
                config.Alias.Add(new TypeConfigItem { Entity = indexerDeclaration });
            }
        }

        public override void VisitMethodDeclaration(MethodDeclaration methodDeclaration)
        {
            if (methodDeclaration.HasModifier(Modifiers.Abstract) || HasTemplate(methodDeclaration))
            {
                return;
            }

            FixMethodParameters(methodDeclaration.Parameters, methodDeclaration.Body);

            bool isStatic = methodDeclaration.HasModifier(Modifiers.Static);

            Dictionary<string, List<MethodDeclaration>> dict = isStatic
                ? CurrentType.StaticMethods
                : CurrentType.InstanceMethods;

            var key = methodDeclaration.Name;

            if (dict.ContainsKey(key))
            {
                dict[key].Add(methodDeclaration);
            }
            else
            {
                dict.Add(key, new List<MethodDeclaration>(new[] { methodDeclaration }));
            }

            var memberrr = Resolver.ResolveNode(methodDeclaration) as MemberResolveResult;

            if (OverloadsCollection.NeedCreateAlias(memberrr))
            {
                var config = isStatic
                ? CurrentType.StaticConfig
                : CurrentType.InstanceConfig;
                config.Alias.Add(new TypeConfigItem { Entity = methodDeclaration });
            }
        }

        public override void VisitCustomEventDeclaration(CustomEventDeclaration customEventDeclaration)
        {
            if (customEventDeclaration.HasModifier(Modifiers.Abstract))
            {
                return;
            }

            bool isStatic = customEventDeclaration.HasModifier(Modifiers.Static);

            IDictionary<string, List<EntityDeclaration>> dict = isStatic
                ? CurrentType.StaticProperties
                : CurrentType.InstanceProperties;

            var key = customEventDeclaration.Name;

            if (dict.ContainsKey(key))
            {
                dict[key].Add(customEventDeclaration);
            }
            else
            {
                dict.Add(key, new List<EntityDeclaration>(new[] { customEventDeclaration }));
            }

            var rr = Resolver.ResolveNode(customEventDeclaration) as MemberResolveResult;
            if (OverloadsCollection.NeedCreateAlias(rr))
            {
                var config = rr.Member.IsStatic
                ? CurrentType.StaticConfig
                : CurrentType.InstanceConfig;
                config.Alias.Add(new TypeConfigItem { Entity = customEventDeclaration });
            }
        }

        public override void VisitPropertyDeclaration(PropertyDeclaration propertyDeclaration)
        {
            if (propertyDeclaration.HasModifier(Modifiers.Abstract))
            {
                return;
            }

            bool isStatic = propertyDeclaration.HasModifier(Modifiers.Static);

            IDictionary<string, List<EntityDeclaration>> dict = isStatic
                ? CurrentType.StaticProperties
                : CurrentType.InstanceProperties;

            var key = propertyDeclaration.Name;

            if (dict.ContainsKey(key))
            {
                dict[key].Add(propertyDeclaration);
            }
            else
            {
                dict.Add(key, new List<EntityDeclaration>(new[] { propertyDeclaration }));
            }

            var rr = Resolver.ResolveNode(propertyDeclaration) as MemberResolveResult;
            if (OverloadsCollection.NeedCreateAlias(rr))
            {
                var config = rr.Member.IsStatic
                ? CurrentType.StaticConfig
                : CurrentType.InstanceConfig;
                config.Alias.Add(new TypeConfigItem { Entity = propertyDeclaration });
            }

            if (!HasExternal(propertyDeclaration)
                && !HasTemplate(propertyDeclaration.Getter))
            {
                Expression initializer = GetDefaultFieldInitializer(propertyDeclaration.ReturnType);
                TypeConfigInfo info = isStatic ? CurrentType.StaticConfig : CurrentType.InstanceConfig;

                bool autoPropertyToField = false;
                if (rr != null && rr.Member != null && Helpers.IsAutoProperty((IProperty)rr.Member))
                {
                    var rules = Rules.Get(Emitter, rr.Member);

                    if (rules.AutoProperty.HasValue)
                    {
                        autoPropertyToField = rules.AutoProperty.Value == AutoPropertyRule.Plain;
                    }
                    else
                    {
                        autoPropertyToField = HasFieldAttribute(rr.Member);

                        if (!autoPropertyToField && rr.Member.ImplementedInterfaceMembers.Count > 0)
                        {
                            foreach (var interfaceMember in rr.Member.ImplementedInterfaceMembers)
                            {
                                autoPropertyToField = HasFieldAttribute(interfaceMember, false);

                                if (autoPropertyToField)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }

                var autoInitializer = info.AutoPropertyInitializers.FirstOrDefault(f => f.Name == key);

                if (autoInitializer != null)
                {
                    initializer = autoInitializer.Initializer;
                }

                if (!autoPropertyToField)
                {
                    info.Properties.Add(new TypeConfigItem
                    {
                        Name = key,
                        Entity = propertyDeclaration,
                        Initializer = initializer,
                        IsPropertyInitializer = autoInitializer != null
                    });
                }
                else
                {
                    info.Fields.Add(new TypeConfigItem
                    {
                        Name = key,
                        Entity = propertyDeclaration,
                        Initializer = initializer
                    });
                }
            }
        }

        private bool HasFieldAttribute(IMember member, bool checkAssembly = true)
        {


            var fieldAttributeName = "H5.FieldAttribute";
            var autoPropertyToField = member.Attributes.Any(a => a.AttributeType.FullName == fieldAttributeName);

            if (!autoPropertyToField)
            {
                autoPropertyToField = member.DeclaringTypeDefinition.Attributes.Any(a => a.AttributeType.FullName == fieldAttributeName);
            }

            if (!autoPropertyToField && checkAssembly)
            {
                autoPropertyToField = member.DeclaringTypeDefinition.ParentAssembly.AssemblyAttributes.Any(a => a.AttributeType.FullName == fieldAttributeName);
            }

            return autoPropertyToField;
        }

        private void CheckFieldProperty(PropertyDeclaration propertyDeclaration, MemberResolveResult resolvedProperty)
        {
            if (HasExternal(propertyDeclaration) || CurrentType.IsObjectLiteral)
            {
                return;
            }

            if (resolvedProperty != null && resolvedProperty.Member != null)
            {
                var possiblyWrongGetter = !propertyDeclaration.Getter.IsNull
                        && !propertyDeclaration.Getter.Body.IsNull
                        && !HasTemplate(propertyDeclaration.Getter)
                        && !HasScript(propertyDeclaration.Getter);

                var possiblyWrongSetter = !propertyDeclaration.Setter.IsNull
                    && !propertyDeclaration.Setter.Body.IsNull
                    && !HasTemplate(propertyDeclaration.Setter)
                    && !HasScript(propertyDeclaration.Setter);

                if (possiblyWrongGetter || possiblyWrongSetter)
                {
                    var message = string.Format(
                            Constants.Messages.Exceptions.FIELD_PROPERTY_MARKED_ADVISE,
                            resolvedProperty.Member.ToString(),
                            possiblyWrongGetter ? "getter" : string.Empty,
                            possiblyWrongSetter ? (possiblyWrongGetter ? " and " : string.Empty) + "setter" : string.Empty
                        );

                    throw new EmitterException(propertyDeclaration, message);
                }
            }
        }

        public override void VisitDelegateDeclaration(DelegateDeclaration delegateDeclaration)
        {
        }

        public override void VisitEnumMemberDeclaration(EnumMemberDeclaration enumMemberDeclaration)
        {
            Expression initializer = enumMemberDeclaration.Initializer;
            var member = Resolver.ResolveNode(enumMemberDeclaration) as MemberResolveResult;
            var initializerIsString = false;
            if (member != null)
            {
                var enumMode = Helpers.EnumEmitMode(member.Member.DeclaringTypeDefinition);

                if (enumMode >= 3 && enumMode < 7)
                {
                    initializerIsString = true;
                    string enumStringName = member.Member.Name;
                    var attr = Helpers.GetInheritedAttribute(member.Member, Translator.H5_ASSEMBLY + ".NameAttribute");

                    if (attr != null)
                    {
                        var value = attr.PositionalArguments.First().ConstantValue;
                        string name = null;
                        if (value is string)
                        {
                            name = value.ToString();
                            name = Helpers.ConvertNameTokens(name, enumStringName);
                        }
                        else if (value is bool boolean)
                        {
                            name = boolean ? Object.Net.Utilities.StringUtils.ToLowerCamelCase(member.Member.Name) : member.Member.Name;
                        }

                        if (member.Member.IsStatic && Helpers.IsReservedStaticName(name, false))
                        {
                            name = Helpers.ChangeReservedWord(name);
                        }
                        initializer = new PrimitiveExpression(name);
                    }
                    else
                    {
                        switch (enumMode)
                        {
                            case 3:
                                enumStringName = Object.Net.Utilities.StringUtils.ToLowerCamelCase(member.Member.Name);
                                break;

                            case 4:
                                break;

                            case 5:
                                enumStringName = enumStringName.ToLowerInvariant();
                                break;

                            case 6:
                                enumStringName = enumStringName.ToUpperInvariant();
                                break;
                        }

                        initializer = new PrimitiveExpression(enumStringName);
                    }
                }
            }

            if (!initializerIsString)
            {
                if (enumMemberDeclaration.Initializer.IsNull)
                {
                    dynamic i = CurrentType.LastEnumValue;
                    ++i;
                    CurrentType.LastEnumValue = i;

                    if (member != null && member.Member.DeclaringTypeDefinition.EnumUnderlyingType.IsKnownType(KnownTypeCode.Int64))
                    {
                        initializer = new PrimitiveExpression(Convert.ToInt64(CurrentType.LastEnumValue));
                    }
                    else if (member != null && member.Member.DeclaringTypeDefinition.EnumUnderlyingType.IsKnownType(KnownTypeCode.UInt64))
                    {
                        initializer = new PrimitiveExpression(Convert.ToUInt64(CurrentType.LastEnumValue));
                    }
                    else
                    {
                        initializer = new PrimitiveExpression(CurrentType.LastEnumValue);
                    }
                }
                else
                {
                    if (Resolver.ResolveNode(enumMemberDeclaration.Initializer) is ConstantResolveResult rr)
                    {
                        if (member != null && member.Member.DeclaringTypeDefinition.EnumUnderlyingType.IsKnownType(KnownTypeCode.Int64))
                        {
                            initializer = new PrimitiveExpression(Convert.ToInt64(rr.ConstantValue));
                        }
                        else if (member != null && member.Member.DeclaringTypeDefinition.EnumUnderlyingType.IsKnownType(KnownTypeCode.UInt64))
                        {
                            initializer = new PrimitiveExpression(Convert.ToUInt64(rr.ConstantValue));
                        }
                        else
                        {
                            initializer = new PrimitiveExpression(rr.ConstantValue);
                        }
                        CurrentType.LastEnumValue = rr.ConstantValue;
                    }
                }
            }

            CurrentType.StaticConfig.Fields.Add(new TypeConfigItem
            {
                Name = enumMemberDeclaration.Name,
                Entity = enumMemberDeclaration,
                Initializer = initializer
            });
        }

        public override void VisitEventDeclaration(EventDeclaration eventDeclaration)
        {
            bool isStatic = eventDeclaration.HasModifier(Modifiers.Static);
            foreach (var item in eventDeclaration.Variables)
            {
                Expression initializer = item.Initializer;
                CurrentType.EventsDeclarations.Add(item.Name, eventDeclaration);
                if (isStatic)
                {
                    CurrentType.StaticConfig.Events.Add(new TypeConfigItem
                    {
                        Name = item.Name,
                        Entity = eventDeclaration,
                        Initializer = initializer,
                        VarInitializer = item
                    });
                }
                else
                {
                    CurrentType.InstanceConfig.Events.Add(new TypeConfigItem
                    {
                        Name = item.Name,
                        Entity = eventDeclaration,
                        Initializer = initializer,
                        VarInitializer = item
                    });
                }

                var rr = Resolver.ResolveNode(item) as MemberResolveResult;
                if (OverloadsCollection.NeedCreateAlias(rr))
                {
                    var config = rr.Member.IsStatic
                    ? CurrentType.StaticConfig
                    : CurrentType.InstanceConfig;
                    config.Alias.Add(new TypeConfigItem { Entity = eventDeclaration, VarInitializer = item });
                }
            }
        }

        public override void VisitAttributeSection(AttributeSection attributeSection)
        {
            if (attributeSection.AttributeTarget != "assembly")
            {
                return;
            }

            foreach (var attr in attributeSection.Attributes)
            {
                var name = attr.Type.ToString();
                var resolveResult = Resolver.ResolveNode(attr);

                ReadModuleInfo(attr, name, resolveResult);
                ReadFileNameInfo(attr, name, resolveResult);
                ReadOutputPathInfo(attr, name, resolveResult);
                ReadOutputByInfo(attr, name, resolveResult);
                ReadModuleDependency(attr, name, resolveResult);
                ReadReflectionInfo(attr, name, resolveResult);
            }
        }

        protected virtual bool ReadReflectionInfo(ICSharpCode.NRefactory.CSharp.Attribute attr, string name, ResolveResult resolveResult)
        {
            if (resolveResult != null && resolveResult.Type != null && resolveResult.Type.FullName == Translator.H5_ASSEMBLY + ".ReflectableAttribute")
            {
                var config = ((H5DotJson_AssemblySettings)AssemblyInfo).ReflectionInternal;

                if (attr.Arguments.Count > 0)
                {
                    if (attr.Arguments.Count > 1)
                    {
                        var list = new List<MemberAccessibility>();
                        for (int i = 0; i < attr.Arguments.Count; i++)
                        {
                            object v = GetAttributeArgumentValue(attr, resolveResult, i);
                            list.Add((MemberAccessibility)(int)v);
                        }

                        config.MemberAccessibility = list.ToArray();
                    }
                    else
                    {
                        object v = GetAttributeArgumentValue(attr, resolveResult, 0);

                        if (v is bool boolean)
                        {
                            config.Disabled = !boolean;
                        }
                        else if (v is string)
                        {
                            if (string.IsNullOrEmpty(config.Filter))
                            {
                                config.Filter = v.ToString();
                            }
                            else
                            {
                                config.Filter += ";" + v.ToString();
                            }
                        }
                        else if (v is int intVal)
                        {
                            IType t = GetAttributeArgumentType(attr, resolveResult, 0);

                            if (t.FullName == "H5.TypeAccessibility")
                            {
                                config.TypeAccessibility = (TypeAccessibility)intVal;
                            }
                            else
                            {
                                config.MemberAccessibility = new[] { (MemberAccessibility)intVal };
                            }
                        }
                        else if (v is int[] intArray)
                        {
                            config.MemberAccessibility = intArray.Cast<MemberAccessibility>().ToArray();
                        }
                    }
                }
                else
                {
                    config.Disabled = false;
                }

                return true;
            }

            return false;
        }

        protected virtual bool ReadModuleInfo(ICSharpCode.NRefactory.CSharp.Attribute attr, string name, ResolveResult resolveResult)
        {
            if ((name == (Translator.H5_ASSEMBLY + ".Module")) ||
                (resolveResult != null && resolveResult.Type != null && resolveResult.Type.FullName == (Translator.H5_ASSEMBLY + ".ModuleAttribute")))
            {
                Module module = null;
                var args = ((InvocationResolveResult) resolveResult).Arguments.Select(a=> a.ConstantValue).ToList();
                var positionArgs =
                    ((InvocationResolveResult) resolveResult).InitializerStatements.Select(s => new
                    {
                        Name = ((MemberResolveResult)((OperatorResolveResult)s).Operands[0]).Member.Name,
                        Value = ((OperatorResolveResult)s).Operands[1].ConstantValue
                    }).ToList();

                if (args.Count == 1)
                {
                    var obj = args[0];

                    if (obj is bool boolean)
                    {
                        module = new Module(boolean, null);
                    }
                    else if (obj is string)
                    {
                        module = new Module(obj.ToString(), null);
                    }
                    else if (obj is int intVal)
                    {
                        module = new Module("", (ModuleType)intVal, null);
                    }
                    else
                    {
                        module = new Module(null);
                    }
                }
                else if (args.Count == 2)
                {
                    var first = args[0];
                    var second = args[1];

                    if (first is string)
                    {
                        var mname = first;
                        var preventName = second;

                        module = new Module(mname != null ? mname.ToString() : "", null, (bool)preventName);
                    }
                    else if (second is bool)
                    {
                        var mtype = first;
                        var preventName = second;

                        module = new Module("", (ModuleType)(int)mtype, null, (bool)preventName);
                    }
                    else
                    {
                        var mtype = first;
                        var mname = second;

                        module = new Module(mname != null ? mname.ToString() : "", (ModuleType)(int)mtype, null);
                    }
                }
                else if (args.Count == 3)
                {
                    var mtype = args[0];
                    var mname = args[1];
                    var preventName = args[2];

                    module = new Module(mname != null ? mname.ToString() : "", (ModuleType)(int)mtype, null, (bool)preventName);
                }
                else
                {
                    module = new Module(null);
                }

                if (positionArgs.Count > 0)
                {
                    foreach (var arg in positionArgs)
                    {
                        if (arg.Name == "Name")
                        {
                            module.Name = arg.Value != null ? (string)arg.Value : "";
                        }
                        else if (arg.Name == "ExportAsNamespace")
                        {
                            module.ExportAsNamespace = arg.Value != null ? (string)arg.Value : "";
                        }
                    }
                }

                AssemblyInfo.Module = module;

                return true;
            }

            return false;
        }

        protected virtual bool ReadFileNameInfo(ICSharpCode.NRefactory.CSharp.Attribute attr, string name, ResolveResult resolveResult)
        {
            if ((name == (Translator.H5_ASSEMBLY + ".FileName")) ||
                (resolveResult != null && resolveResult.Type != null && resolveResult.Type.FullName == (Translator.H5_ASSEMBLY + ".FileNameAttribute")))
            {
                if (attr.Arguments.Count > 0)
                {
                    object nameObj = GetAttributeArgumentValue(attr, resolveResult, 0);

                    if (nameObj is string)
                    {
                        AssemblyInfo.FileName = nameObj.ToString();
                    }
                }

                return true;
            }

            return false;
        }

        protected virtual bool ReadOutputPathInfo(ICSharpCode.NRefactory.CSharp.Attribute attr, string name, ResolveResult resolveResult)
        {
            var configHelper = new ConfigHelper();

            if ((name == (Translator.H5_ASSEMBLY + ".Output")) ||
                (resolveResult != null && resolveResult.Type != null && resolveResult.Type.FullName == (Translator.H5_ASSEMBLY + ".OutputPathAttribute")))
            {
                if (attr.Arguments.Count > 0)
                {
                    object nameObj = GetAttributeArgumentValue(attr, resolveResult, 0);

                    if (nameObj is string)
                    {
                        AssemblyInfo.Output = configHelper.ConvertPath(nameObj.ToString());
                    }
                }

                return true;
            }

            return false;
        }

        protected virtual bool ReadOutputByInfo(ICSharpCode.NRefactory.CSharp.Attribute attr, string name, ResolveResult resolveResult)
        {
            if ((name == (Translator.H5_ASSEMBLY + ".OutputBy")) ||
                (resolveResult != null && resolveResult.Type != null && resolveResult.Type.FullName == (Translator.H5_ASSEMBLY + ".OutputByAttribute")))
            {
                if (attr.Arguments.Count > 0)
                {
                    object nameObj = GetAttributeArgumentValue(attr, resolveResult, 0);

                    if (nameObj != null)
                    {
                        AssemblyInfo.OutputBy = (OutputBy)Enum.ToObject(typeof(OutputBy), nameObj);
                    }

                    if (attr.Arguments.Count > 1)
                    {
                        nameObj = GetAttributeArgumentValue(attr, resolveResult, 1);

                        if (nameObj is int intVal)
                        {
                            AssemblyInfo.StartIndexInName = intVal;
                        }
                    }
                }

                return true;
            }

            return false;
        }

        protected virtual bool ReadModuleDependency(ICSharpCode.NRefactory.CSharp.Attribute attr, string name, ResolveResult resolveResult)
        {
            if ((name == (Translator.H5_ASSEMBLY + ".ModuleDependency")) ||
                (resolveResult != null && resolveResult.Type != null && resolveResult.Type.FullName == (Translator.H5_ASSEMBLY + ".ModuleDependencyAttribute")))
            {
                if (attr.Arguments.Count > 0)
                {
                    ModuleDependency dependency = new ModuleDependency();
                    object nameObj = GetAttributeArgumentValue(attr, resolveResult, 0);

                    if (nameObj is string)
                    {
                        dependency.DependencyName = nameObj.ToString();
                    }

                    nameObj = GetAttributeArgumentValue(attr, resolveResult, 1);

                    if (nameObj is string)
                    {
                        dependency.VariableName = nameObj.ToString();
                    }
                    else
                    {
                        dependency.VariableName = Module.EscapeName(dependency.DependencyName);
                    }

                    AssemblyInfo.Dependencies.Add(dependency);
                }

                return true;
            }

            return false;
        }

        protected virtual object GetAttributeArgumentValue(ICSharpCode.NRefactory.CSharp.Attribute attr, ResolveResult resolveResult, int index)
        {
            object nameObj = null;

            if (!(resolveResult is ErrorResolveResult) && (resolveResult is InvocationResolveResult result))
            {
                nameObj = result.Arguments.Skip(index).Take(1).First().ConstantValue;
            }
            else
            {
                var arg = attr.Arguments.Skip(index).Take(1).First();

                if (arg is PrimitiveExpression primitive)
                {
                    nameObj = primitive.Value;
                }
            }
            return nameObj;
        }

        protected virtual IType GetAttributeArgumentType(ICSharpCode.NRefactory.CSharp.Attribute attr, ResolveResult resolveResult, int index)
        {
            if (!(resolveResult is ErrorResolveResult) && (resolveResult is InvocationResolveResult result))
            {
                var arg = result.Arguments.Skip(index).Take(1).First();
                return arg.Type;
            }
            else
            {
                var arg = attr.Arguments.Skip(index).Take(1).First();

                if (arg is PrimitiveExpression primitive)
                {
                    return Resolver.ResolveNode(primitive).Type;
                }
            }
            return null;
        }
    }
}