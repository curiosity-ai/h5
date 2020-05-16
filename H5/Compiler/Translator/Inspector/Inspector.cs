using H5.Contract;
using H5.Contract.Constants;

using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.TypeSystem;
using System.Collections.Generic;
using System.Linq;
using NRAttribute = ICSharpCode.NRefactory.CSharp.Attribute;

namespace H5.Translator
{
    public partial class Inspector : Visitor
    {
        internal IEmitter Emitter { get; private set; }

        public Inspector(IAssemblyInfo config = null)
        {
            Types = new List<ITypeInfo>();
            IgnoredTypes = new List<string>();
            AssemblyInfo = config ?? new AssemblyInfo();

            Emitter = new TempEmitter { AssemblyInfo = AssemblyInfo };
        }

        protected virtual bool HasAttribute(EntityDeclaration type, string name)
        {
            foreach (var i in type.Attributes)
            {
                foreach (var j in i.Attributes)
                {
                    if (j.Type.ToString() == name)
                    {
                        return true;
                    }

                    var resolveResult = Resolver.ResolveNode(j);
                    if (resolveResult != null && resolveResult.Type != null && resolveResult.Type.FullName == (name + "Attribute"))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        protected virtual bool TryGetAttribute(EntityDeclaration type, string attributeName, out NRAttribute attribute)
        {
            foreach (var i in type.Attributes)
            {
                foreach (var j in i.Attributes)
                {
                    if (j.Type.ToString() == attributeName)
                    {
                        attribute = j;
                        return true;
                    }

                    // FIXME: Will not try to get the attribute via Resolver.ResolveNode() (see above): it returns a
                    //        different type, without minimum information needed to make a full NRAttribute -fzm
                }
            }

            attribute = default(NRAttribute);
            return false;
        }

        protected virtual bool IsObjectLiteral(EntityDeclaration declaration)
        {
            return HasAttribute(declaration, Translator.H5_ASSEMBLY + ".ObjectLiteral");
        }

        protected virtual bool IsNonScriptable(EntityDeclaration declaration)
        {
            return HasAttribute(declaration, Translator.H5_ASSEMBLY + ".NonScriptable");
        }

        protected virtual bool HasExternal(EntityDeclaration declaration)
        {
            return HasAttribute(declaration, Translator.H5_ASSEMBLY + ".External") ||
                   HasAttribute(declaration, Translator.H5_ASSEMBLY + ".Ignore") ||
                   IsVirtual(declaration as TypeDeclaration);
        }

        protected virtual bool IsVirtual(TypeDeclaration typeDeclaration)
        {
            if (typeDeclaration == null)
            {
                return false;
            }

            var resolveResult = Resolver.ResolveNode(typeDeclaration);
            var typeDef = resolveResult?.Type?.GetDefinition();
            return typeDef != null && Validator.IsVirtualTypeStatic(typeDef);
        }

        protected virtual bool HasTemplate(EntityDeclaration declaration)
        {
            return HasAttribute(declaration, Translator.H5_ASSEMBLY + ".Template");
        }

        protected virtual bool HasScript(EntityDeclaration declaration)
        {
            return HasAttribute(declaration, Translator.H5_ASSEMBLY + ".Script");
        }

        private Expression GetDefaultFieldInitializer(AstType type)
        {
            return new PrimitiveExpression(GetDefaultFieldValue(type, Resolver), "?");
        }

        public static object GetDefaultFieldValue(AstType type, IMemberResolver resolver)
        {
            if (type is PrimitiveType primitiveType)
            {
                switch (primitiveType.KnownTypeCode)
                {
                    case KnownTypeCode.Decimal:
                        return 0m;

                    case KnownTypeCode.Int64:
                        return 0L;

                    case KnownTypeCode.UInt64:
                        return 0UL;

                    case KnownTypeCode.Int16:
                    case KnownTypeCode.Int32:
                    case KnownTypeCode.UInt16:
                    case KnownTypeCode.UInt32:
                    case KnownTypeCode.Byte:
                    case KnownTypeCode.Double:
                    case KnownTypeCode.SByte:
                    case KnownTypeCode.Single:
                        return 0;

                    case KnownTypeCode.Boolean:
                        return false;
                }
            }

            var resolveResult = resolver.ResolveNode(type);

            var o = GetDefaultFieldValue(resolveResult.Type, type, false);

            if (o != null)
            {
                return o;
            }

            if (!resolveResult.IsError && NullableType.IsNullable(resolveResult.Type))
            {
                return null;
            }

            if (!resolveResult.IsError && (resolveResult.Type.IsKnownType(KnownTypeCode.Enum) || resolveResult.Type.Kind == TypeKind.Enum))
            {
                return 0;
            }

            if (!resolveResult.IsError && resolveResult.Type.Kind == TypeKind.Struct)
            {
                return type;
            }

            return null;
        }

        public static object GetDefaultFieldValue(IType type, AstType astType, bool wrapType = true)
        {
            if (type.Kind == TypeKind.TypeParameter && astType != null)
            {
                if (type is ITypeParameter parameter && (
                    parameter.Owner.Attributes.Any(a => a.AttributeType.FullName == "H5.IgnoreGenericAttribute") ||
                    parameter.Owner.DeclaringTypeDefinition != null && parameter.Owner.DeclaringTypeDefinition.Attributes.Any(a => a.AttributeType.FullName == "H5.IgnoreGenericAttribute")))
                {
                    return null;
                }
                return new RawValue(JS.Funcs.H5_GETDEFAULTVALUE + "(" + type.Name + ")");
            }

            if (type.IsKnownType(KnownTypeCode.Decimal))
            {
                return 0m;
            }

            if (type.IsKnownType(KnownTypeCode.Int64))
            {
                return 0L;
            }

            if (type.IsKnownType(KnownTypeCode.UInt64))
            {
                return 0UL;
            }

            if (type.IsKnownType(KnownTypeCode.Char) ||
                type.IsKnownType(KnownTypeCode.Int16) ||
                type.IsKnownType(KnownTypeCode.Int32) ||
                type.IsKnownType(KnownTypeCode.UInt16) ||
                type.IsKnownType(KnownTypeCode.UInt32) ||
                type.IsKnownType(KnownTypeCode.Byte) ||
                type.IsKnownType(KnownTypeCode.Double) ||
                type.IsKnownType(KnownTypeCode.SByte) ||
                type.IsKnownType(KnownTypeCode.Single) ||
                type.IsKnownType(KnownTypeCode.Enum))
            {
                return 0;
            }

            if (NullableType.IsNullable(type))
            {
                return null;
            }

            if (type.IsKnownType(KnownTypeCode.Boolean))
            {
                return false;
            }

            if (type.IsKnownType(KnownTypeCode.Enum) || type.Kind == TypeKind.Enum)
            {
                return 0;
            }

            if (type.Kind == TypeKind.Struct && wrapType)
            {
                return type;
            }

            return null;
        }

        public static string GetStructDefaultValue(AstType type, IEmitter emitter)
        {
            var rr = emitter.Resolver.ResolveNode(type);
            return GetStructDefaultValue(rr.Type, emitter);
        }

        public static string GetStructDefaultValue(IType type, IEmitter emitter)
        {
            if (type.IsKnownType(KnownTypeCode.DateTime))
            {
                return string.Format("{0}()", JS.Types.System.DateTime.GET_DEFAULT_VALUE);
            }

            var isGeneric = type.TypeArguments.Count > 0 && !Helpers.IsIgnoreGeneric(type, emitter);

            if (emitter.Validator.IsObjectLiteral(emitter.GetTypeDefinition(type)))
            {
                return "{}";
            }

            return string.Concat("new ", isGeneric ? "(" : "", H5Types.ToJsName(type, emitter), isGeneric ? ")" : "", "()");
        }

        protected virtual bool IsValidStaticInitializer(Expression expr)
        {
            if (expr.IsNull || expr is PrimitiveExpression)
            {
                return true;
            }


            if (!(expr is ArrayCreateExpression arrayExpr))
            {
                return false;
            }

            try
            {
                new ArrayInitializerVisitor().VisitArrayCreateExpression(arrayExpr);

                return true;
            }
            catch (TranslatorException)
            {
                return false;
            }
        }

        protected virtual void FixMethodParameters(AstNodeCollection<ParameterDeclaration> parameters, BlockStatement body)
        {
            /*if (parameters.Count == 0)
            {
                return;
            }

            foreach (var p in parameters)
            {
                string newName = JS.Vars.FIX_ARGUMENT_NAME + p.Name;
                string oldName = p.Name;

                VariableDeclarationStatement varState = new VariableDeclarationStatement(p.Type.Clone(), oldName, new CastExpression(p.Type.Clone(), new IdentifierExpression(newName)));

                p.Name = newName;

                body.InsertChildBefore(body.FirstChild, varState, new Role<VariableDeclarationStatement>("Statement"));
            }*/
        }

        /// <summary>
        /// Checks if the namespace name is likely to conflict with H5 namespace.
        /// </summary>
        /// <param name="namespaceName"></param>
        /// <returns></returns>
        protected static bool IsConflictingNamespace(string namespaceName)
        {
            return (namespaceName == Translator.H5_ASSEMBLY);
        }

        /// <summary>
        /// Validates the type's namespace attribute (if present) against conflicts with H5's namespaces.
        /// </summary>
        /// <param name="tpDecl">The TypeDefinition object of the validated item.</param>
        private void ValidateNamespace(TypeDeclaration tpDecl)
        {
            if (AssemblyInfo.Assembly.EnableReservedNamespaces)
            {
                return;
            }

            NRAttribute nsAt;
            if (TryGetAttribute(tpDecl, "Namespace", out nsAt))
            {
                var nsName = nsAt.Arguments.FirstOrNullObject().ToString().Trim('"');
                if (IsConflictingNamespace(nsName))
                {
                    throw new EmitterException(nsAt, "Custom attribute '[" + nsAt.ToString() +
                        "]' uses reserved namespace name 'H5'."
                        + H5.Translator.Emitter.NEW_LINE
                        + "This name is reserved for H5 core.");
                }
            }
        }

        /// <summary>
        /// Validates the namespace name against conflicts with H5's namespaces.
        /// </summary>
        /// <param name="nsDecl">The NamespaceDefinition object of the validated item.</param>
        private void ValidateNamespace(NamespaceDeclaration nsDecl)
        {
            if (IsConflictingNamespace(nsDecl.FullName))
            {
                throw new EmitterException(nsDecl, "Namespace '" + nsDecl.FullName +
                    "' uses reserved name 'H5'."
                    + H5.Translator.Emitter.NEW_LINE
                    + "This name is reserved for H5 core.");
            }
        }
    }
}