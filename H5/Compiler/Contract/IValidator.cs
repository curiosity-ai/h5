using ICSharpCode.NRefactory.TypeSystem;
using Mono.Cecil;

namespace H5.Contract
{
    public interface IValidator
    {
        bool CanIgnoreType(Mono.Cecil.TypeDefinition type);

        void CheckIdentifier(string name, ICSharpCode.NRefactory.CSharp.AstNode context);

        void CheckConstructors(Mono.Cecil.TypeDefinition type, ITranslator translator);

        void CheckFields(Mono.Cecil.TypeDefinition type, ITranslator translator);

        void CheckFileName(Mono.Cecil.TypeDefinition type, ITranslator translator);

        void CheckMethodArguments(Mono.Cecil.MethodDefinition method);

        void CheckMethods(Mono.Cecil.TypeDefinition type, ITranslator translator);

        void CheckModule(Mono.Cecil.TypeDefinition type, ITranslator translator);

        void CheckModuleDependenies(Mono.Cecil.TypeDefinition type, ITranslator translator);

        void CheckProperties(Mono.Cecil.TypeDefinition type, ITranslator translator);

        void CheckType(Mono.Cecil.TypeDefinition type, ITranslator translator);

        ICSharpCode.NRefactory.TypeSystem.IAttribute GetAttribute(System.Collections.Generic.IEnumerable<ICSharpCode.NRefactory.TypeSystem.IAttribute> attributes, string name);

        Mono.Cecil.CustomAttribute GetAttribute(System.Collections.Generic.IEnumerable<Mono.Cecil.CustomAttribute> attributes, string name);

        string GetAttributeValue(System.Collections.Generic.IEnumerable<Mono.Cecil.CustomAttribute> attributes, string name);

        string GetCustomConstructor(Mono.Cecil.TypeDefinition type);

        string GetCustomTypeName(Mono.Cecil.TypeDefinition type, IEmitter emitter, bool excludeNs, bool asDefinition = true);

        System.Collections.Generic.HashSet<string> GetParentTypes(System.Collections.Generic.IDictionary<string, Mono.Cecil.TypeDefinition> allTypes);

        bool HasAttribute(ITypeDefinition typeDefinition, string name);

        bool HasAttribute(System.Collections.Generic.IEnumerable<IAttribute> attributes, string name);

        bool HasAttribute(System.Collections.Generic.IEnumerable<Mono.Cecil.CustomAttribute> attributes, string name);

        bool IsDelegateOrLambda(ICSharpCode.NRefactory.Semantics.ResolveResult result);

        bool IsExternalType(TypeDefinition type, bool ignoreLiteral = false);

        bool IsExternalType(ICSharpCode.NRefactory.TypeSystem.ITypeDefinition typeDefinition, bool ignoreLiteral = false);

        bool IsVirtualType(ICSharpCode.NRefactory.TypeSystem.ITypeDefinition typeDefinition);

        bool IsExternalInterface(ICSharpCode.NRefactory.TypeSystem.ITypeDefinition typeDefinition, out bool isNative);

        IExternalInterface IsExternalInterface(ICSharpCode.NRefactory.TypeSystem.ITypeDefinition typeDefinition);

        bool IsImmutableType(ICustomAttributeProvider type);

        bool IsExternalType(IEntity enity, bool ignoreLiteral = false);

        bool IsH5Class(Mono.Cecil.TypeDefinition type);

        bool IsH5Class(IType type);

        bool IsObjectLiteral(ICSharpCode.NRefactory.TypeSystem.ITypeDefinition type);

        bool IsObjectLiteral(Mono.Cecil.TypeDefinition type);

        bool IsAccessorsIndexer(IEntity enity);

        int GetObjectInitializationMode(TypeDefinition type);

        int GetObjectCreateMode(TypeDefinition type);
    }
}