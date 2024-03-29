﻿using H5.Contract;
using H5.Contract.Constants;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.TypeSystem.Implementation;
using Mono.Cecil;
using Object.Net.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Mono.Cecil.Rocks;
using ICustomAttributeProvider = Mono.Cecil.ICustomAttributeProvider;
using System.Text;

namespace H5.Translator
{
    public class Validator : IValidator
    {
        public virtual bool CanIgnoreType(TypeDefinition type)
        {
            if (IsExternalType(type))
            {
                return true;
            }

            if (type.BaseType != null && type.BaseType.FullName == "System.MulticastDelegate")
            {
                return true;
            }

            return false;
        }

        public virtual void CheckType(TypeDefinition type, ITranslator translator)
        {
            CheckObjectLiteral(type, translator);

            if (CanIgnoreType(type))
            {
                return;
            }

            CheckConstructors(type, translator);
            CheckFields(type, translator);
            CheckProperties(type, translator);
            CheckMethods(type, translator);
            CheckEvents(type, translator);
            CheckFileName(type, translator);
            CheckModule(type, translator);
            CheckModuleDependenies(type, translator);
        }

        public virtual void CheckObjectLiteral(TypeDefinition type, ITranslator translator)
        {
            if (!IsObjectLiteral(type))
            {
                return;
            }

            var objectCreateMode = GetObjectCreateMode(type);

            if (objectCreateMode == 0)
            {
                var ctors = type.GetConstructors();

                foreach (var ctor in ctors)
                {
                    foreach (var parameter in ctor.Parameters)
                    {
                        if (parameter.ParameterType.FullName == "H5.ObjectCreateMode")
                        {
                            TranslatorException.Throw(Constants.Messages.Exceptions.OBJECT_LITERAL_PLAIN_NO_CREATE_MODE_CUSTOM_CONSTRUCTOR, type);
                        }

                        if (parameter.ParameterType.FullName == "H5.ObjectInitializationMode")
                        {
                            continue;
                        }

                        TranslatorException.Throw(Constants.Messages.Exceptions.OBJECT_LITERAL_PLAIN_CUSTOM_CONSTRUCTOR, type);
                    }
                }
            }

            if (type.IsInterface)
            {
                if (type.HasMethods && type.Methods.GroupBy(m => m.Name).Any(g => g.Count() > 1))
                {
                    TranslatorException.Throw(Constants.Messages.Exceptions.OBJECT_LITERAL_INTERFACE_NO_OVERLOAD_METHODS, type);
                }

                if (type.HasEvents)
                {
                    TranslatorException.Throw(Constants.Messages.Exceptions.OBJECT_LITERAL_INTERFACE_NO_EVENTS, type);
                }
            }
            else
            {
                if (type.Methods.Any(m => !m.IsRuntimeSpecialName && m.Name.Contains(".") && !m.Name.Contains("<")) ||
                    type.Properties.Any(m => !m.IsRuntimeSpecialName && m.Name.Contains(".") && !m.Name.Contains("<")))
                {
                    TranslatorException.Throw(Constants.Messages.Exceptions.OBJECT_LITERAL_INTERFACE_NO_EXPLICIT_IMPLEMENTATION, type);
                }
            }

            if (type.BaseType != null)
            {
                TypeDefinition baseType = null;
                try
                {
                    baseType = type.BaseType.Resolve();
                }
                catch (Exception)
                {

                }

                if (objectCreateMode == 1 && baseType != null && baseType.FullName != "System.Object" && baseType.FullName != "System.ValueType" && GetObjectCreateMode(baseType) == 0)
                {
                    TranslatorException.Throw(Constants.Messages.Exceptions.OBJECT_LITERAL_CONSTRUCTOR_INHERITANCE, type);
                }

                if (objectCreateMode == 0 && baseType != null && GetObjectCreateMode(baseType) == 1)
                {
                    TranslatorException.Throw(Constants.Messages.Exceptions.OBJECT_LITERAL_PLAIN_INHERITANCE, type);
                }
            }

            if (type.Interfaces.Count > 0)
            {
                foreach (var @interface in type.Interfaces)
                {
                    TypeDefinition iDef = null;
                    try
                    {
                        iDef = @interface.InterfaceType.Resolve();
                    }
                    catch (Exception)
                    {

                    }

                    if (iDef != null && iDef.FullName != "System.Object" && !IsObjectLiteral(iDef))
                    {
                        TranslatorException.Throw(Constants.Messages.Exceptions.OBJECT_LITERAL_INTERFACE_INHERITANCE, type);
                    }
                }
            }

            if (objectCreateMode == 0)
            {
                var hasVirtualMethods = false;

                foreach (MethodDefinition method in type.Methods)
                {
                    if (AttributeHelper.HasCompilerGeneratedAttribute(method))
                    {
                        continue;
                    }

                    if (method.IsVirtual && !(method.IsSetter || method.IsGetter))
                    {
                        hasVirtualMethods = true;
                        break;
                    }
                }

                if (hasVirtualMethods)
                {
                    TranslatorException.Throw(Constants.Messages.Exceptions.OBJECT_LITERAL_NO_VIRTUAL_METHODS, type);
                }
            }
        }

        public virtual bool IsExternalType(TypeDefinition type, bool ignoreLiteral = false)
        {

            string externalAttr = Translator.H5_ASSEMBLY + ".ExternalAttribute";
            string nonScriptableAttr = Translator.H5_ASSEMBLY + ".NonScriptableAttribute";

            var has = HasAttribute(type.CustomAttributes, externalAttr)
                      || HasAttribute(type.CustomAttributes, nonScriptableAttr);

            if (!has && type.DeclaringType != null)
            {
                has = HasAttribute(type.DeclaringType.CustomAttributes, externalAttr) || HasAttribute(type.DeclaringType.CustomAttributes, nonScriptableAttr);
            }

            if (!has)
            {
                if (IsTypeFromH5Core(type.FullName)) return true;

                has = HasAttribute(type.Module.Assembly.CustomAttributes, externalAttr);
            }

            if (!has)
            {
                has = IsVirtualTypeStatic(type);
            }

            return has;
        }

        public static bool IsTypeFromH5Core(string fullName)
        {
            return fullName.StartsWith("H5.Core.") || fullName == "H5.Core"; //For when passing the namespace name
        }

        public static bool IsTypeFromH5ButNotFromH5Core(string fullName)
        {
            if (fullName == "H5") return true;
            if (fullName == "H5.Core") return false;

            return fullName.StartsWith("H5.") && !fullName.StartsWith("H5.Core.");
        }

        public virtual bool IsExternalType(IEntity entity, bool ignoreLiteral = false)
        {
            string externalAttr = Translator.H5_ASSEMBLY + ".ExternalAttribute";
            string nonScriptableAttr = Translator.H5_ASSEMBLY + ".NonScriptableAttribute";

            return
                HasAttribute(entity.Attributes, externalAttr)
                || HasAttribute(entity.Attributes, nonScriptableAttr);
        }

        public virtual bool IsH5Class(IType type)
        {
            foreach (var i in type.GetAllBaseTypes().Where(t => t.Kind == TypeKind.Interface))
            {
                if (i.FullName == JS.Types.H5_IH5Class)
                {
                    return true;
                }
            }

            return false;
        }

        public virtual bool IsH5Class(TypeDefinition type)
        {
            foreach (var i in type.Interfaces)
            {
                if (i.InterfaceType.FullName == JS.Types.H5_IH5Class)
                {
                    return true;
                }
            }

            return false;
        }

        public virtual bool IsExternalType(ITypeDefinition typeDefinition, bool ignoreLiteral = false)
        {

            string externalAttr = Translator.H5_ASSEMBLY + ".ExternalAttribute";

            var has = typeDefinition.Attributes.Any(attr => attr.Constructor != null && attr.Constructor.DeclaringType.FullName == externalAttr);

            if (!has && typeDefinition.DeclaringTypeDefinition != null)
            {
                has = HasAttribute(typeDefinition.DeclaringTypeDefinition.Attributes, externalAttr);
            }

            if (!has)
            {
                if (IsTypeFromH5Core(typeDefinition.FullName)) return true;

                has = typeDefinition.ParentAssembly.AssemblyAttributes.Any(attr => attr.Constructor != null && attr.Constructor.DeclaringType.FullName == externalAttr);
            }

            if (!has)
            {
                has = IsVirtualType(typeDefinition);
            }

            return has;
        }

        public bool IsVirtualType(ITypeDefinition typeDefinition)
        {
            return IsVirtualTypeStatic(typeDefinition);
        }

        public static bool IsVirtualTypeStatic(TypeDefinition typeDefinition)
        {
            string virtualAttr = Translator.H5_ASSEMBLY + ".VirtualAttribute";
            CustomAttribute attr = attr = typeDefinition.CustomAttributes.FirstOrDefault(a => a.AttributeType.FullName == virtualAttr);

            if (attr == null && typeDefinition.DeclaringType != null)
            {
                return IsVirtualTypeStatic(typeDefinition.DeclaringType);
            }

            if (attr == null)
            {
                if (IsTypeFromH5Core(typeDefinition.FullName)) return true;

                attr = typeDefinition.Module.Assembly.CustomAttributes.FirstOrDefault(a => a.AttributeType.FullName == virtualAttr);
            }

            bool isVirtual = false;

            if (attr != null)
            {
                if (attr.ConstructorArguments.Count == 0)
                {
                    isVirtual = true;
                }
                else
                {
                    var value = (int)attr.ConstructorArguments[0].Value;

                    switch (value)
                    {
                        case 0:
                            isVirtual = true;
                            break;
                        case 1:
                            isVirtual = !typeDefinition.IsInterface;
                            break;
                        case 2:
                            isVirtual = typeDefinition.IsInterface;
                            break;
                    }
                }
            }

            return isVirtual;
        }

        public static bool IsVirtualTypeStatic(ITypeDefinition typeDefinition)
        {
            string virtualAttr = Translator.H5_ASSEMBLY + ".VirtualAttribute";
            IAttribute attr = null;
            if (typeDefinition.GetDefinition() != null)
            {
                attr =
                    typeDefinition.GetDefinition().Attributes.FirstOrDefault(
                        a => a.AttributeType.FullName == virtualAttr);
            }

            if (attr == null && typeDefinition.DeclaringType != null)
            {
                return IsVirtualTypeStatic(typeDefinition.DeclaringType.GetDefinition());
            }

            if (attr == null)
            {
                if (IsTypeFromH5Core(typeDefinition.FullName)) return true;

                attr = typeDefinition.ParentAssembly.AssemblyAttributes.FirstOrDefault(a => a.AttributeType.FullName == virtualAttr);
            }

            bool isVirtual = false;

            if (attr != null)
            {
                if (attr.PositionalArguments.Count == 0)
                {
                    isVirtual = true;
                }
                else
                {
                    var value = (int)attr.PositionalArguments[0].ConstantValue;

                    switch (value)
                    {
                        case 0:
                            isVirtual = true;
                            break;
                        case 1:
                            isVirtual = typeDefinition.Kind != TypeKind.Interface;
                            break;
                        case 2:
                            isVirtual = typeDefinition.Kind == TypeKind.Interface;
                            break;
                    }
                }
            }

            return isVirtual;
        }

        public virtual bool IsExternalInterface(ITypeDefinition typeDefinition, out bool isNative)
        {
            string externalAttr = Translator.H5_ASSEMBLY + ".ExternalInterfaceAttribute";
            var attr = typeDefinition.Attributes.FirstOrDefault(a => a.Constructor != null && (a.Constructor.DeclaringType.FullName == externalAttr));

            if (attr == null)
            {
                if (IsTypeFromH5Core(typeDefinition.FullName))
                {
                    isNative = false; //RFO: Retyped.dom has ExternalInterfaceAttribute(false), which maps to isNative = false as bellow
                    return true;
                }

                attr = typeDefinition.ParentAssembly.AssemblyAttributes.FirstOrDefault(a => a.Constructor != null && (a.Constructor.DeclaringType.FullName == externalAttr));
            }

            isNative = attr != null && attr.PositionalArguments.Count == 1 && (bool)attr.PositionalArguments[0].ConstantValue;

            if (attr == null)
            {
                isNative = typeDefinition.ParentAssembly.AssemblyName == CS.NS.H5 || !IsExternalType(typeDefinition);
            }

            return attr != null;
        }

        public virtual IExternalInterface IsExternalInterface(ITypeDefinition typeDefinition)
        {
            string externalAttr = Translator.H5_ASSEMBLY + ".ExternalInterfaceAttribute";

            var attr = typeDefinition.Attributes.FirstOrDefault(a => a.Constructor != null && (a.Constructor.DeclaringType.FullName == externalAttr));

            if (attr == null)
            {
                if (IsTypeFromH5Core(typeDefinition.FullName))
                {
                    //RFO: Retyped.dom has ExternalInterfaceAttribute(false), which maps to isNative = false bellow
                    return new ExternalInterface() { IsNativeImplementation = false, IsSimpleImplementation = true };
                }

                attr = typeDefinition.ParentAssembly.AssemblyAttributes.FirstOrDefault(a => a.Constructor != null && (a.Constructor.DeclaringType.FullName == externalAttr));
            }

            if (attr != null)
            {
                var ei = new ExternalInterface();
                if (attr.PositionalArguments.Count == 1)
                {
                    bool isNative = (bool)attr.PositionalArguments[0].ConstantValue;

                    if (isNative)
                    {
                        ei.IsNativeImplementation = true;
                    }
                    else
                    {
                        ei.IsSimpleImplementation = true;
                    }
                }

                if (attr.NamedArguments.Count == 1)
                {
                    if (attr.NamedArguments[0].Key.Name == "IsVirtual")
                    {
                        ei.IsVirtual = (bool)attr.NamedArguments[0].Value.ConstantValue;
                    }
                }

                return ei;
            }

            return null;
        }

        public virtual bool IsImmutableType(ICustomAttributeProvider type)
        {
            string attrName = Translator.H5_ASSEMBLY + ".ImmutableAttribute";

            return HasAttribute(type.CustomAttributes, attrName);
        }

        
        public virtual bool HasAttribute(IEnumerable<CustomAttribute> attributes, string name)
        {
            return GetAttribute(attributes, name) != null;
        }

        public bool HasAttribute(IEnumerable<IAttribute> attributes, string name)
        {
            return GetAttribute(attributes, name) != null;
        }

        public virtual bool HasAttribute(ITypeDefinition typeDefinition, string name)
        {
            IAttribute attr = null;
            if (typeDefinition.GetDefinition() != null)
            {
                attr =
                    typeDefinition.GetDefinition().Attributes.FirstOrDefault(a => a.AttributeType.FullName == name);
            }

            //RFO: not sure if need this one:
            //if (attr == null && typeDefinition.DeclaringType != null)
            //{
            //    return HasAttribute(typeDefinition.DeclaringType.GetDefinition(), name);
            //}

            return attr != null;
        }

        public virtual CustomAttribute GetAttribute(IEnumerable<CustomAttribute> attributes, string name)
        {
            CustomAttribute a = attributes
                .FirstOrDefault(attr => attr.AttributeType.FullName == name);
            return a;
        }

        public virtual IAttribute GetAttribute(IEnumerable<IAttribute> attributes, string name)
        {
            IAttribute a = attributes
                .FirstOrDefault(attr => attr.AttributeType.FullName == name);
            return a;
        }

        public virtual string GetAttributeValue(IEnumerable<CustomAttribute> attributes, string name)
        {
            CustomAttribute a = GetAttribute(attributes, name);

            if (a != null)
            {
                return (string)a.ConstructorArguments[0].Value;
            }

            return null;
        }

        public virtual bool IsObjectLiteral(TypeDefinition type)
        {
            return HasAttribute(type.CustomAttributes, Translator.H5_ASSEMBLY + ".ObjectLiteralAttribute");
        }

        public int GetObjectInitializationMode(TypeDefinition type)
        {
            var attr = type.CustomAttributes.FirstOrDefault(a => a.AttributeType.FullName == Translator.H5_ASSEMBLY + ".ObjectLiteralAttribute");

            if (attr != null)
            {
                var args = attr.ConstructorArguments;

                if (args.Count > 0)
                {
                    if (args[0].Type.FullName == Translator.H5_ASSEMBLY + ".ObjectInitializationMode")
                    {
                        return (int)args[0].Value;
                    }
                }
            }

            return 0;
        }

        public int GetObjectCreateMode(TypeDefinition type)
        {
            var attr = type.CustomAttributes.FirstOrDefault(a => a.AttributeType.FullName == Translator.H5_ASSEMBLY + ".ObjectLiteralAttribute");

            if (attr != null)
            {
                var args = attr.ConstructorArguments;

                if (args.Count > 0)
                {
                    for (int i = 0; i < args.Count; i++)
                    {
                        if (args[i].Type.FullName == Translator.H5_ASSEMBLY + ".ObjectCreateMode")
                        {
                            return (int)args[i].Value;
                        }
                    }
                }
            }

            return 0;
        }

        public virtual bool IsObjectLiteral(ITypeDefinition type)
        {
            return HasAttribute(type, Translator.H5_ASSEMBLY + ".ObjectLiteralAttribute");
        }

        private Stack<TypeDefinition> _stack = new Stack<TypeDefinition>();

        public virtual string GetCustomTypeName(TypeDefinition type, IEmitter emitter, bool excludeNs, bool asDefinition = true)
        {
            if (_stack.Contains(type))
            {
                return null;
            }

            var nsAtrr = excludeNs ? null : GetAttribute(type.CustomAttributes, Translator.H5_ASSEMBLY + ".NamespaceAttribute");
            bool hasNs = nsAtrr != null && nsAtrr.ConstructorArguments.Count > 0;
            var nameAttr = GetAttribute(type.CustomAttributes, Translator.H5_ASSEMBLY + ".NameAttribute");

            string name = null;
            bool changeCase = false;
            if (nameAttr != null && nameAttr.ConstructorArguments.Count > 0)
            {
                if (nameAttr.ConstructorArguments[0].Value is string val)
                {
                    name = Helpers.ConvertNameTokens(val, type.Name);
                }
                else if (nameAttr.ConstructorArguments[0].Value is bool boolValue)
                {
                    if (boolValue)
                    {
                        if (hasNs)
                        {
                            changeCase = true;
                        }
                        else
                        {
                            _stack.Push(type);
                            name = H5Types.ToJsName(type, emitter);
                            var i = name.LastIndexOf(".");

                            if (i > -1)
                            {
                                char[] chars = name.ToCharArray();
                                chars[i + 1] = Char.ToLowerInvariant(chars[i + 1]);
                                name = new string(chars);
                            }
                            else
                            {
                                name = name.ToLowerCamelCase();
                            }
                            _stack.Pop();

                            return name;
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(name))
            {
                if (excludeNs)
                {
                    var idx = name.LastIndexOf('.');

                    if (idx > -1)
                    {
                        name = name.Substring(idx + 1);
                    }
                }

                var typeDef = emitter.H5Types.Get(type).Type;

                if (typeDef != null && !asDefinition && typeDef.TypeArguments.Count > 0 && !Helpers.IsIgnoreGeneric(typeDef, emitter, true))
                {
                    StringBuilder sb = new StringBuilder(name);
                    bool needComma = false;
                    sb.Append("<");
                    foreach (var typeArg in typeDef.TypeArguments)
                    {
                        if (needComma)
                        {
                            sb.Append(",");
                        }

                        needComma = true;
                        sb.Append(H5Types.ToTypeScriptName(typeArg, emitter));
                    }
                    sb.Append(">");
                    name = sb.ToString();
                }

                return name;
            }

            if (hasNs)
            {
                var arg = nsAtrr.ConstructorArguments[0];
                name = "";
                if (arg.Value is string)
                {
                    name = arg.Value.ToString();
                }

                if (arg.Value is bool booVal && booVal)
                {
                    return null;
                }

                if (type.IsNested)
                {
                    name = (string.IsNullOrEmpty(name) ? "" : (name + ".")) + H5Types.GetParentNames(emitter, type);
                }


                var typeName = emitter.GetTypeName(emitter.H5Types.Get(type).Type.GetDefinition(), type);
                name = (string.IsNullOrEmpty(name) ? "" : (name + ".")) + H5Types.ConvertName(changeCase ? typeName.ToLowerCamelCase() : typeName);

                return name;
            }

            if (HasAttribute(type.CustomAttributes, Translator.H5_ASSEMBLY + ".ObjectLiteralAttribute"))
            {
                var mode = GetObjectCreateMode(type);
                if (emitter.Validator.IsExternalType(type) && mode == 0)
                {
                    return JS.Types.System.Object.NAME;
                }
            }

            return null;
        }

        public virtual string GetCustomConstructor(TypeDefinition type)
        {
            string ctor = GetAttributeValue(type.CustomAttributes, Translator.H5_ASSEMBLY + ".ConstructorAttribute");

            if (!string.IsNullOrEmpty(ctor))
            {
                return ctor;
            }

            if (HasAttribute(type.CustomAttributes, Translator.H5_ASSEMBLY + ".ObjectLiteralAttribute") && GetObjectCreateMode(type) == 0)
            {
                return "{ }";
            }

            return null;
        }

        public virtual void CheckConstructors(TypeDefinition type, ITranslator translator)
        {
            if (type.HasMethods)
            {
                var ctors = type.Methods.Where(method => method.IsConstructor);

                foreach (MethodDefinition ctor in ctors)
                {
                    CheckMethodArguments(ctor);
                }
            }
        }

        public virtual void CheckFields(TypeDefinition type, ITranslator translator)
        {

        }

        public virtual void CheckProperties(TypeDefinition type, ITranslator translator)
        {
            /*if (type.HasProperties && this.IsObjectLiteral(type))
            {
                var objectCreateMode = this.GetObjectCreateMode(type);
                if (objectCreateMode == 0)
                {
                    foreach (PropertyDefinition prop in type.Properties)
                    {
                        if ((prop.GetMethod != null && prop.GetMethod.IsVirtual) || (prop.SetMethod != null && prop.SetMethod.IsVirtual))
                        {
                            TranslatorException.Throw(Constants.Messages.Exceptions.OBJECT_LITERAL_NO_VIRTUAL_METHODS, type);
                        }
                    }
                }
            }*/
        }

        public virtual void CheckEvents(TypeDefinition type, ITranslator translator)
        {
        }

        public virtual void CheckMethods(TypeDefinition type, ITranslator translator)
        {
            foreach (MethodDefinition method in type.Methods)
            {
                if (AttributeHelper.HasCompilerGeneratedAttribute(method))
                {
                    continue;
                }

                CheckMethodArguments(method);
            }
        }

        public virtual void CheckMethodArguments(MethodDefinition method)
        {
        }

        public virtual HashSet<string> GetParentTypes(IDictionary<string, TypeDefinition> allTypes)
        {
            var result = new HashSet<string>();

            foreach (var type in allTypes.Values)
            {
                if (type.BaseType != null)
                {
                    string parentName = type.BaseType.FullName.LeftOf('<').Replace('`', JS.Vars.D);

                    if (!allTypes.ContainsKey(parentName))
                    {
                        TranslatorException.Throw("Unknown type {0}", parentName);
                    }

                    if (!result.Contains(parentName))
                    {
                        result.Add(parentName);
                    }
                }
            }
            return result;
        }

        public virtual void CheckFileName(TypeDefinition type, ITranslator translator)
        {
            if (type.HasCustomAttributes)
            {
                var attr = GetAttribute(type.CustomAttributes, Translator.H5_ASSEMBLY + ".FileNameAttribute");

                if (attr != null)
                {
                    var typeInfo = EnsureTypeInfo(type, translator);

                    var obj = GetAttributeArgumentValue(attr, 0);

                    if (obj is string)
                    {
                        typeInfo.FileName = obj.ToString();
                    }
                }
            }
        }

        public virtual void CheckModule(TypeDefinition type, ITranslator translator)
        {
            if (type.HasCustomAttributes)
            {
                var attr = GetAttribute(type.CustomAttributes, Translator.H5_ASSEMBLY + ".ModuleAttribute");

                if (attr != null)
                {
                    ReadModuleFromAttribute(type, translator, attr);
                }
            }

            if (type.Module.Assembly.HasCustomAttributes)
            {
                var attr = GetAttribute(type.Module.Assembly.CustomAttributes, Translator.H5_ASSEMBLY + ".ModuleAttribute");

                if (attr != null)
                {
                    ReadModuleFromAttribute(type, translator, attr);
                }
            }

            if (translator.AssemblyInfo.Module != null && type.Module.Assembly.Equals(translator.AssemblyDefinition))
            {
                var typeInfo = EnsureTypeInfo(type, translator);
                if (typeInfo.Module == null)
                {
                    typeInfo.Module = translator.AssemblyInfo.Module;
                }
            }
        }

        private void ReadModuleFromAttribute(TypeDefinition type, ITranslator translator, CustomAttribute attr)
        {
            var typeInfo = EnsureTypeInfo(type, translator);
            Module module = null;

            if (attr.ConstructorArguments.Count == 1)
            {
                var obj = attr.ConstructorArguments[0].Value;

                if (obj is bool boolObj)
                {
                    module = new Module(boolObj, null);
                }
                else if (obj is string)
                {
                    module = new Module(obj.ToString(), null);
                }
                else if (obj is int intObj)
                {
                    module = new Module("", (ModuleType)intObj, null);
                }
                else
                {
                    module = new Module(null);
                }
            }
            else if (attr.ConstructorArguments.Count == 2)
            {
                if (attr.ConstructorArguments[0].Value is string)
                {
                    var name = attr.ConstructorArguments[0].Value;
                    var preventName = attr.ConstructorArguments[1].Value;

                    module = new Module(name != null ? name.ToString() : "", null, (bool)preventName);
                }
                else if (attr.ConstructorArguments[1].Value is bool)
                {
                    var mtype = attr.ConstructorArguments[0].Value;
                    var preventName = attr.ConstructorArguments[1].Value;

                    module = new Module("", (ModuleType)(int)mtype, null, (bool)preventName);
                }
                else
                {
                    var mtype = attr.ConstructorArguments[0].Value;
                    var name = attr.ConstructorArguments[1].Value;

                    module = new Module(name != null ? name.ToString() : "", (ModuleType)(int)mtype, null);
                }
            }
            else if (attr.ConstructorArguments.Count == 3)
            {
                var mtype = attr.ConstructorArguments[0].Value;
                var name = attr.ConstructorArguments[1].Value;
                var preventName = attr.ConstructorArguments[2].Value;

                module = new Module(name != null ? name.ToString() : "", (ModuleType)(int)mtype, null, (bool)preventName);
            }
            else
            {
                module = new Module(null);
            }

            if (attr.Properties.Count > 0)
            {
                foreach (var prop in attr.Properties)
                {
                    if (prop.Name == "Name")
                    {
                        module.Name = prop.Argument.Value != null ? (string)prop.Argument.Value : "";
                    }
                    else if (prop.Name == "ExportAsNamespace")
                    {
                        module.ExportAsNamespace = prop.Argument.Value != null ? (string)prop.Argument.Value : "";
                    }
                }
            }

            typeInfo.Module = module;
        }

        public virtual void CheckModuleDependenies(TypeDefinition type, ITranslator translator)
        {
            if (type.HasCustomAttributes)
            {
                var attrName = Translator.H5_ASSEMBLY + ".ModuleDependencyAttribute";
                var attrs = type.CustomAttributes.Where(attr => attr.AttributeType.FullName == attrName).ToList();

                if (attrs.Count > 0)
                {
                    foreach (var attr in attrs)
                    {
                        var typeInfo = EnsureTypeInfo(type, translator);

                        if (attr.ConstructorArguments.Count > 0)
                        {
                            ModuleDependency dependency = new ModuleDependency();
                            var obj = GetAttributeArgumentValue(attr, 0);
                            dependency.DependencyName = obj is string ? obj.ToString() : "";

                            if (attr.ConstructorArguments.Count > 1)
                            {
                                obj = GetAttributeArgumentValue(attr, 1);
                                dependency.VariableName = obj is string ? obj.ToString() : "";
                            }
                            else
                            {
                                dependency.VariableName = Module.EscapeName(dependency.DependencyName);
                            }

                            typeInfo.Dependencies.Add(dependency);
                        }
                    }
                }
            }
        }

        protected virtual object GetAttributeArgumentValue(CustomAttribute attr, int index)
        {
            return attr.ConstructorArguments.ElementAt(index).Value;
        }

        protected virtual ITypeInfo EnsureTypeInfo(TypeDefinition type, ITranslator translator)
        {
            string key = H5Types.GetTypeDefinitionKey(type);
            ITypeInfo typeInfo = null;

            if (translator.TypeInfoDefinitions.ContainsKey(key))
            {
                typeInfo = translator.TypeInfoDefinitions[key];
            }
            else
            {
                typeInfo = new TypeInfo();
                translator.TypeInfoDefinitions[key] = typeInfo;
            }
            return typeInfo;
        }

        public virtual bool IsDelegateOrLambda(ResolveResult result)
        {
            return result.Type.Kind == TypeKind.Delegate || result is LambdaResolveResult;
        }

        public virtual void CheckIdentifier(string name, AstNode context)
        {
            if (Helpers.IsReservedWord(null, name))
            {
                throw new EmitterException(context, "Cannot use '" + name + "' as identifier");
            }
        }

        public virtual bool IsAccessorsIndexer(IEntity entity)
        {
            if (entity == null)
            {
                return false;
            }

            if (HasAttribute(entity.Attributes, CS.Attributes.ACCESSORSINDEXER_ATTRIBUTE_NAME))
            {
                return true;
            }

            return false;
        }
    }
}