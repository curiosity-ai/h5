using H5.Contract;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.TypeSystem;
using System.Collections.Generic;
using System.Linq;

namespace H5.Translator
{
    public class TypeInfo : ITypeInfo
    {
        public TypeInfo()
        {
            StaticMethods = new Dictionary<string, List<MethodDeclaration>>();
            InstanceMethods = new Dictionary<string, List<MethodDeclaration>>();
            StaticProperties = new Dictionary<string, List<EntityDeclaration>>();
            InstanceProperties = new Dictionary<string, List<EntityDeclaration>>();
            FieldsDeclarations = new Dictionary<string, FieldDeclaration>();
            EventsDeclarations = new Dictionary<string, EventDeclaration>();
            Dependencies = new List<IPluginDependency>();
            Ctors = new List<ConstructorDeclaration>();
            Operators = new Dictionary<OperatorType, List<OperatorDeclaration>>();
            StaticConfig = new TypeConfigInfo();
            InstanceConfig = new TypeConfigInfo();
            PartialTypeDeclarations = new List<TypeDeclaration>();
        }

        public string Key { get; set; }

        public TypeConfigInfo StaticConfig { get; set; }

        public TypeConfigInfo InstanceConfig { get; set; }

        public Dictionary<OperatorType, List<OperatorDeclaration>> Operators
        {
            get;
            protected set;
        }

        public Dictionary<string, EventDeclaration> EventsDeclarations { get; set; }

        public TypeDeclaration TypeDeclaration { get; set; }

        public List<TypeDeclaration> PartialTypeDeclarations { get; set; }

        public bool IsStatic { get; set; }

        public ClassType ClassType { get; set; }

        public string Namespace { get; set; }

        public string Name { get; set; }

        public List<ConstructorDeclaration> Ctors { get; set; }

        public ConstructorDeclaration StaticCtor { get; set; }

        public Dictionary<string, FieldDeclaration> FieldsDeclarations
        {
            get;
            protected set;
        }

        public Dictionary<string, List<MethodDeclaration>> StaticMethods
        {
            get;
            protected set;
        }

        public Dictionary<string, List<MethodDeclaration>> InstanceMethods
        {
            get;
            protected set;
        }

        public Dictionary<string, List<EntityDeclaration>> StaticProperties
        {
            get;
            protected set;
        }

        public Dictionary<string, List<EntityDeclaration>> InstanceProperties
        {
            get;
            protected set;
        }

        public bool HasStatic
        {
            get
            {
                return StaticConfig.HasMembers
                       || StaticMethods.Count > 0
                       || StaticProperties.Count > 0
                       || StaticCtor != null
                       || Operators.Count > 0;
            }
        }

        public bool HasRealStatic(IEmitter emitter)
        {
            var result = ClassType == ClassType.Struct
                       || StaticConfig.HasMembers
                       || StaticProperties.Count > 0
                       || StaticCtor != null
                       || Operators.Count > 0;

            if (result)
            {
                return true;
            }

            if (StaticMethods.Any(group =>
            {
                foreach (var method in group.Value)
                {
                    if (Helpers.IsEntryPointMethod(emitter, method))
                    {
                        return false;
                    }

                    if (method.Attributes.Count == 0)
                    {
                        return true;
                    }

                    foreach (var attrSection in method.Attributes)
                    {
                        foreach (var attr in attrSection.Attributes)
                        {
                            var rr = emitter.Resolver.ResolveNode(attr.Type);
                            if (rr.Type.FullName == "H5.InitAttribute")
                            {
                                if (!attr.HasArgumentList)
                                {
                                    return true;
                                }

                                var expr = attr.Arguments.First();

                                var argrr = emitter.Resolver.ResolveNode(expr);
                                if (argrr.ConstantValue is int)
                                {
                                    var value = (InitPosition)argrr.ConstantValue;

                                    if (value == InitPosition.Before || value == InitPosition.Top)
                                    {
                                        return false;
                                    }
                                }

                                return true;
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                }

                return false;
            }))
            {
                return true;
            }

            if (Type.GetConstructors().Any(c => c.Parameters.Count == 0 && emitter.GetInline(c) != null))
            {
                return true;
            }

            return false;
        }

        public bool HasRealInstantiable(IEmitter emitter)
        {
            if (HasInstantiable)
            {
                return true;
            }

            if (StaticMethods.Any(group =>
            {
                return group.Value.Any(method => Helpers.IsEntryPointMethod(emitter, method));
            }))
            {
                return true;
            }

            return false;
        }

        public bool HasInstantiable
        {
            get
            {
                return InstanceConfig.HasMembers
                       || InstanceMethods.Count > 0
                       || InstanceProperties.Count > 0
                       || Ctors.Count > 0;
            }
        }

        private object enumValue = -1;

        public virtual object LastEnumValue
        {
            get
            {
                return enumValue;
            }
            set
            {
                enumValue = value;
            }
        }

        public bool IsEnum { get; set; }

        public Module Module { get; set; }

        public List<IPluginDependency> Dependencies { get; set; }

        public ITypeInfo ParentType { get; set; }

        public bool IsObjectLiteral { get; set; }

        public string FileName { get; set; }

        public IType Type { get; set; }

        public string JsName { get; set; }

        public AstType GetBaseClass(IEmitter emitter)
        {
            var types = GetBaseTypes(emitter);
            var baseClass = types.FirstOrDefault(t => emitter.Resolver.ResolveNode(t).Type.Kind != TypeKind.Interface);

            return baseClass ?? types.First();
        }

        private List<AstType> baseTypes;

        public List<AstType> GetBaseTypes(IEmitter emitter)
        {
            if (baseTypes != null)
            {
                return baseTypes;
            }

            baseTypes = new List<AstType>();
            baseTypes.AddRange(TypeDeclaration.BaseTypes);

            foreach (var partialTypeDeclaration in PartialTypeDeclarations)
            {
                var appendTypes = new List<AstType>();
                var insertTypes = new List<AstType>();
                foreach (var baseType in partialTypeDeclaration.BaseTypes)
                {
                    var t = emitter.Resolver.ResolveNode(baseType);
                    if (baseTypes.All(bt => emitter.Resolver.ResolveNode(bt).Type.FullName != t.Type.FullName))
                    {
                        if (t.Type.Kind != TypeKind.Interface)
                        {
                            insertTypes.Add(baseType);
                        }
                        else
                        {
                            appendTypes.Add(baseType);
                        }
                    }
                }

                baseTypes.AddRange(appendTypes);
                baseTypes.InsertRange(0, insertTypes);
            }

            return baseTypes;
        }

        public string GetNamespace(IEmitter emitter, bool nons = false)
        {
            if (emitter == null)
            {
                throw new System.ArgumentNullException("emitter");
            }

            var name = Namespace;

            var h5Type = emitter.H5Types.Get(Key);
            var cas = h5Type.TypeDefinition.CustomAttributes;

            // Search for an 'NamespaceAttribute' entry
            foreach (var ca in cas)
            {
                if (ca.AttributeType.Name == "NameAttribute" && ca.ConstructorArguments.Count > 0)
                {
                    if (ca.ConstructorArguments[0].Value is string constructorArgumentValue)
                    {
                        name = constructorArgumentValue.Contains(".") ? constructorArgumentValue.Substring(0, constructorArgumentValue.LastIndexOf(".")) : null;

                        break;
                    }
                }

                if (ca.AttributeType.Name == "NamespaceAttribute" && ca.ConstructorArguments.Count > 0)
                {
                    var constructorArgumentValue = ca.ConstructorArguments[0].Value as string;

                    if (!string.IsNullOrWhiteSpace(constructorArgumentValue))
                    {
                        name = constructorArgumentValue;
                    }

                    if (ca.ConstructorArguments[0].Value is bool)
                    {
                        if (!(bool)ca.ConstructorArguments[0].Value)
                        {
                            name = null;
                        }
                    }
                }
            }

            if (name == null && !nons)
            {
                name = emitter.Translator.DefaultNamespace;
            }

            return name;
        }
    }
}