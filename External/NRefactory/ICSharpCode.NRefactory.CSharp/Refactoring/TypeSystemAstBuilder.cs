﻿// Copyright (c) 2010-2013 AlphaSierraPapa for the SharpDevelop Team
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.CSharp.TypeSystem;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.TypeSystem.Implementation;
using ICSharpCode.NRefactory.Utils;

namespace ICSharpCode.NRefactory.CSharp.Refactoring
{
    /// <summary>
    /// Converts from type system to the C# AST.
    /// </summary>
    public class TypeSystemAstBuilder
    {
        readonly CSharpResolver resolver;

        #region Constructor
        /// <summary>
        /// Creates a new TypeSystemAstBuilder.
        /// </summary>
        /// <param name="resolver">
        /// A resolver initialized for the position where the type will be inserted.
        /// </param>
        public TypeSystemAstBuilder(CSharpResolver resolver)
        {
            if (resolver == null)
                throw new ArgumentNullException("resolver");
            this.resolver = resolver;
            InitProperties();
        }

        /// <summary>
        /// Creates a new TypeSystemAstBuilder.
        /// </summary>
        public TypeSystemAstBuilder()
        {
            InitProperties();
        }
        #endregion

        #region Properties
        void InitProperties()
        {
            this.ShowAccessibility = true;
            this.ShowModifiers = true;
            this.ShowBaseTypes = true;
            this.ShowTypeParameters = true;
            this.ShowTypeParameterConstraints = true;
            this.ShowParameterNames = true;
            this.ShowConstantValues = true;
            this.UseAliases = true;
        }

        /// <summary>
        /// Specifies whether the ast builder should add annotations to type references.
        /// The default value is <c>false</c>.
        /// </summary>
        public bool AddTypeReferenceAnnotations { get; set; }

        /// <summary>
        /// Specifies whether the ast builder should add ResolveResult annotations to AST nodes.
        /// The default value is <c>false</c>.
        /// </summary>
        public bool AddResolveResultAnnotations { get; set; }

        /// <summary>
        /// Controls the accessibility modifiers are shown.
        /// The default value is <c>true</c>.
        /// </summary>
        public bool ShowAccessibility { get; set; }

        /// <summary>
        /// Controls the non-accessibility modifiers are shown.
        /// The default value is <c>true</c>.
        /// </summary>
        public bool ShowModifiers { get; set; }

        /// <summary>
        /// Controls whether base type references are shown.
        /// The default value is <c>true</c>.
        /// </summary>
        public bool ShowBaseTypes { get; set; }

        /// <summary>
        /// Controls whether type parameter declarations are shown.
        /// The default value is <c>true</c>.
        /// </summary>
        public bool ShowTypeParameters { get; set; }

        /// <summary>
        /// Controls whether constraints on type parameter declarations are shown.
        /// Has no effect if ShowTypeParameters is false.
        /// The default value is <c>true</c>.
        /// </summary>
        public bool ShowTypeParameterConstraints { get; set; }

        /// <summary>
        /// Controls whether the names of parameters are shown.
        /// The default value is <c>true</c>.
        /// </summary>
        public bool ShowParameterNames { get; set; }

        /// <summary>
        /// Controls whether to show default values of optional parameters, and the values of constant fields.
        /// The default value is <c>true</c>.
        /// </summary>
        public bool ShowConstantValues { get; set; }

        /// <summary>
        /// Controls whether to show attributes.
        /// The default value is <c>false</c>.
        /// </summary>
        public bool ShowAttributes { get; set; }

        /// <summary>
        /// Controls whether to use fully-qualified type names or short type names.
        /// The default value is <c>false</c>.
        /// </summary>
        public bool AlwaysUseShortTypeNames { get; set; }

        /// <summary>
        /// Controls whether to generate a body that throws a <c>System.NotImplementedException</c>.
        /// The default value is <c>false</c>.
        /// </summary>
        public bool GenerateBody { get; set; }

        /// <summary>
        /// Controls whether to generate custom events.
        /// The default value is <c>false</c>.
        /// </summary>
        public bool UseCustomEvents { get; set; }

        /// <summary>
        /// Controls if unbound type argument names are inserted in the ast or not.
        /// The default value is <c>false</c>.
        /// </summary>
        public bool ConvertUnboundTypeArguments { get; set;}

        /// <summary>
        /// Controls if aliases should be used inside the type name or not.
        /// The default value is <c>true</c>.
        /// </summary>
        public bool UseAliases { get; set;}
        #endregion

        #region Convert Type
        public AstType ConvertType(IType type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            AstType astType = ConvertTypeHelper(type);
            if (AddTypeReferenceAnnotations)
                astType.AddAnnotation(type);
            if (AddResolveResultAnnotations)
                astType.AddAnnotation(new TypeResolveResult(type));
            return astType;
        }

        public AstType ConvertType(FullTypeName fullTypeName)
        {
            if (resolver != null) {
                foreach (var asm in resolver.Compilation.Assemblies) {
                    var def = asm.GetTypeDefinition(fullTypeName);
                    if (def != null) {
                        return ConvertType(def);
                    }
                }
            }
            TopLevelTypeName top = fullTypeName.TopLevelTypeName;
            AstType type;
            if (string.IsNullOrEmpty(top.Namespace)) {
                type = new SimpleType(top.Name);
            } else {
                type = new SimpleType(top.Namespace).MemberType(top.Name);
            }
            for (int i = 0; i < fullTypeName.NestingLevel; i++) {
                type = type.MemberType(fullTypeName.GetNestedTypeName(i));
            }
            return type;
        }

        AstType ConvertTypeHelper(IType type)
        {
            if (type is TypeWithElementType typeWithElementType)
            {
                if (typeWithElementType is PointerType)
                {
                    return ConvertType(typeWithElementType.ElementType).MakePointerType();
                }
                else if (typeWithElementType is ArrayType)
                {
                    return ConvertType(typeWithElementType.ElementType).MakeArrayType(((ArrayType)type).Dimensions);
                }
                else
                {
                    // e.g. ByReferenceType; not supported as type in C#
                    return ConvertType(typeWithElementType.ElementType);
                }
            }
            if (type is ParameterizedType pt)
            {
                if (pt.Name == "Nullable" && pt.Namespace == "System" && pt.TypeParameterCount == 1)
                {
                    return ConvertType(pt.TypeArguments[0]).MakeNullableType();
                }
                return ConvertTypeHelper(pt.GetDefinition(), pt.TypeArguments);
            }
            if (type is ITypeDefinition typeDef)
            {
                if (typeDef.TypeParameterCount > 0)
                {
                    // Unbound type
                    IType[] typeArguments = new IType[typeDef.TypeParameterCount];
                    for (int i = 0; i < typeArguments.Length; i++)
                    {
                        typeArguments[i] = SpecialType.UnboundTypeArgument;
                    }
                    return ConvertTypeHelper(typeDef, typeArguments);
                }
                else
                {
                    return ConvertTypeHelper(typeDef, EmptyList<IType>.Instance);
                }
            }
            return new SimpleType(type.Name);
        }

        AstType ConvertTypeHelper(ITypeDefinition typeDef, IList<IType> typeArguments)
        {
            Debug.Assert(typeArguments.Count >= typeDef.TypeParameterCount);

            string keyword = KnownTypeReference.GetCSharpNameByTypeCode(typeDef.KnownTypeCode);
            if (keyword != null)
                return new PrimitiveType(keyword);

            // The number of type parameters belonging to outer classes
            int outerTypeParameterCount;
            if (typeDef.DeclaringType != null)
                outerTypeParameterCount = typeDef.DeclaringType.TypeParameterCount;
            else
                outerTypeParameterCount = 0;

            if (resolver != null) {
                // Look if there's an alias to the target type
                if (UseAliases) {
                    for (ResolvedUsingScope usingScope = resolver.CurrentUsingScope; usingScope != null; usingScope = usingScope.Parent) {
                        foreach (var pair in usingScope.UsingAliases) {
                            if (pair.Value is TypeResolveResult) {
                                if (TypeMatches(pair.Value.Type, typeDef, typeArguments))
                                    return new SimpleType(pair.Key);
                            }
                        }
                    }
                }

                IList<IType> localTypeArguments;
                if (typeDef.TypeParameterCount > outerTypeParameterCount) {
                    localTypeArguments = new IType[typeDef.TypeParameterCount - outerTypeParameterCount];
                    for (int i = 0; i < localTypeArguments.Count; i++) {
                        localTypeArguments[i] = typeArguments[outerTypeParameterCount + i];
                    }
                } else {
                    localTypeArguments = EmptyList<IType>.Instance;
                }
                ResolveResult rr = resolver.ResolveSimpleName(typeDef.Name, localTypeArguments);
                TypeResolveResult trr = rr as TypeResolveResult;
                if (trr != null || (localTypeArguments.Count == 0 && resolver.IsVariableReferenceWithSameType(rr, typeDef.Name, out trr))) {
                    if (!trr.IsError && TypeMatches(trr.Type, typeDef, typeArguments)) {
                        // We can use the short type name
                        SimpleType shortResult = new SimpleType(typeDef.Name);
                        AddTypeArguments(shortResult, typeDef, typeArguments, outerTypeParameterCount, typeDef.TypeParameterCount);
                        return shortResult;
                    }
                }
            }

            if (AlwaysUseShortTypeNames) {
                var shortResult = new SimpleType(typeDef.Name);
                AddTypeArguments(shortResult, typeDef, typeArguments, outerTypeParameterCount, typeDef.TypeParameterCount);
                return shortResult;
            }
            MemberType result = new MemberType();
            if (typeDef.DeclaringTypeDefinition != null) {
                // Handle nested types
                result.Target = ConvertTypeHelper(typeDef.DeclaringTypeDefinition, typeArguments);
            } else {
                // Handle top-level types
                if (string.IsNullOrEmpty(typeDef.Namespace)) {
                    result.Target = new SimpleType("global");
                    result.IsDoubleColon = true;
                } else {
                    result.Target = ConvertNamespace(typeDef.Namespace);
                }
            }
            result.MemberName = typeDef.Name;
            AddTypeArguments(result, typeDef, typeArguments, outerTypeParameterCount, typeDef.TypeParameterCount);
            return result;
        }

        /// <summary>
        /// Gets whether 'type' is the same as 'typeDef' parameterized with the given type arguments.
        /// </summary>
        bool TypeMatches(IType type, ITypeDefinition typeDef, IList<IType> typeArguments)
        {
            if (typeDef.TypeParameterCount == 0) {
                return typeDef.Equals(type);
            } else {
                if (!typeDef.Equals(type.GetDefinition()))
                    return false;
                if (!(type is ParameterizedType pt))
                {
                    return typeArguments.All(t => t.Kind == TypeKind.UnboundTypeArgument);
                }
                var ta = pt.TypeArguments;
                for (int i = 0; i < ta.Count; i++) {
                    if (!ta[i].Equals(typeArguments[i]))
                        return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Adds type arguments to the result type.
        /// </summary>
        /// <param name="result">The result AST node (a SimpleType or MemberType)</param>
        /// <param name="typeDef">The type definition that owns the type parameters</param>
        /// <param name="typeArguments">The list of type arguments</param>
        /// <param name="startIndex">Index of first type argument to add</param>
        /// <param name="endIndex">Index after last type argument to add</param>
        void AddTypeArguments(AstType result, ITypeDefinition typeDef, IList<IType> typeArguments, int startIndex, int endIndex)
        {
            Debug.Assert(endIndex <= typeDef.TypeParameterCount);
            for (int i = startIndex; i < endIndex; i++) {
                if (ConvertUnboundTypeArguments && typeArguments[i].Kind == TypeKind.UnboundTypeArgument) {
                    result.AddChild(new SimpleType(typeDef.TypeParameters[i].Name), Roles.TypeArgument);
                } else {
                    result.AddChild(ConvertType(typeArguments[i]), Roles.TypeArgument);
                }
            }
        }

        public AstType ConvertNamespace(string namespaceName)
        {
            if (resolver != null) {
                // Look if there's an alias to the target namespace
                if (UseAliases) {
                    for (ResolvedUsingScope usingScope = resolver.CurrentUsingScope; usingScope != null; usingScope = usingScope.Parent) {
                        foreach (var pair in usingScope.UsingAliases) {
                            if (pair.Value is NamespaceResolveResult nrr && nrr.NamespaceName == namespaceName)
                                return new SimpleType(pair.Key);
                        }
                    }
                }
            }

            int pos = namespaceName.LastIndexOf('.');
            if (pos < 0) {
                if (IsValidNamespace(namespaceName)) {
                    return new SimpleType(namespaceName);
                } else {
                    return new MemberType {
                        Target = new SimpleType("global"),
                        IsDoubleColon = true,
                        MemberName = namespaceName
                    };
                }
            } else {
                string parentNamespace = namespaceName.Substring(0, pos);
                string localNamespace = namespaceName.Substring(pos + 1);
                return new MemberType {
                    Target = ConvertNamespace(parentNamespace),
                    MemberName = localNamespace
                };
            }
        }

        bool IsValidNamespace(string firstNamespacePart)
        {
            if (resolver == null)
                return true; // just assume namespaces are valid if we don't have a resolver
            return resolver.ResolveSimpleName(firstNamespacePart, EmptyList<IType>.Instance) is NamespaceResolveResult nrr && !nrr.IsError && nrr.NamespaceName == firstNamespacePart;
        }
        #endregion

        #region Convert Attribute
        public Attribute ConvertAttribute(IAttribute attribute)
        {
            Attribute attr = new Attribute();
            attr.Type = ConvertType(attribute.AttributeType);
            if (attr.Type is SimpleType st && st.Identifier.EndsWith("Attribute", StringComparison.Ordinal))
            {
                st.Identifier = st.Identifier.Substring(0, st.Identifier.Length - 9);
            }
            else if (attr.Type is MemberType mt && mt.MemberName.EndsWith("Attribute", StringComparison.Ordinal))
            {
                mt.MemberName = mt.MemberName.Substring(0, mt.MemberName.Length - 9);
            }
            foreach (ResolveResult arg in attribute.PositionalArguments) {
                attr.Arguments.Add(ConvertConstantValue(arg));
            }
            foreach (var pair in attribute.NamedArguments) {
                attr.Arguments.Add(new NamedExpression(pair.Key.Name, ConvertConstantValue(pair.Value)));
            }
            return attr;
        }
        #endregion

        #region Convert Constant Value
        public Expression ConvertConstantValue(ResolveResult rr)
        {
            if (rr == null)
                throw new ArgumentNullException("rr");
            if (rr is ConversionResolveResult) {
                // unpack ConversionResolveResult if necessary
                // (e.g. a boxing conversion or string->object reference conversion)
                rr = ((ConversionResolveResult)rr).Input;
            }

            if (rr is TypeOfResolveResult) {
                var expr = new TypeOfExpression(ConvertType(((TypeOfResolveResult)rr).ReferencedType));
                if (AddResolveResultAnnotations)
                    expr.AddAnnotation(rr);
                return expr;
            } else if (rr is ArrayCreateResolveResult acrr)
            {
                ArrayCreateExpression ace = new ArrayCreateExpression();
                ace.Type = ConvertType(acrr.Type);
                if (ace.Type is ComposedType composedType)
                {
                    composedType.ArraySpecifiers.MoveTo(ace.AdditionalArraySpecifiers);
                    if (!composedType.HasNullableSpecifier && composedType.PointerRank == 0)
                        ace.Type = composedType.BaseType;
                }

                if (acrr.SizeArguments != null && acrr.InitializerElements == null)
                {
                    ace.AdditionalArraySpecifiers.FirstOrNullObject().Remove();
                    ace.Arguments.AddRange(acrr.SizeArguments.Select(ConvertConstantValue));
                }
                if (acrr.InitializerElements != null)
                {
                    ArrayInitializerExpression initializer = new ArrayInitializerExpression();
                    initializer.Elements.AddRange(acrr.InitializerElements.Select(ConvertConstantValue));
                    ace.Initializer = initializer;
                }
                if (AddResolveResultAnnotations)
                    ace.AddAnnotation(rr);
                return ace;
            }
            else if (rr.IsCompileTimeConstant)
            {
                return ConvertConstantValue(rr.Type, rr.ConstantValue);
            }
            else
            {
                return new ErrorExpression();
            }
        }

        public Expression ConvertConstantValue(IType type, object constantValue)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (constantValue == null) {
                if (type.IsReferenceType == true) {
                    var expr = new NullReferenceExpression();
                    if (AddResolveResultAnnotations)
                        expr.AddAnnotation(new ConstantResolveResult(SpecialType.NullType, null));
                    return expr;
                } else {
                    var expr = new DefaultValueExpression(ConvertType(type));
                    if (AddResolveResultAnnotations)
                        expr.AddAnnotation(new ConstantResolveResult(type, null));
                    return expr;
                }
            } else if (type.Kind == TypeKind.Enum) {
                return ConvertEnumValue(type, (long)CSharpPrimitiveCast.Cast(TypeCode.Int64, constantValue, false));
            } else {
                return new PrimitiveExpression(constantValue);
            }
        }

        bool IsFlagsEnum(ITypeDefinition type)
        {
            IType flagsAttributeType = type.Compilation.FindType(typeof(System.FlagsAttribute));
            return type.GetAttribute(flagsAttributeType) != null;
        }

        Expression ConvertEnumValue(IType type, long val)
        {
            ITypeDefinition enumDefinition = type.GetDefinition();
            TypeCode enumBaseTypeCode = ReflectionHelper.GetTypeCode(enumDefinition.EnumUnderlyingType);
            foreach (IField field in enumDefinition.Fields) {
                if (field.IsConst && object.Equals(CSharpPrimitiveCast.Cast(TypeCode.Int64, field.ConstantValue, false), val))
                    return ConvertType(type).Member(field.Name);
            }
            if (IsFlagsEnum(enumDefinition)) {
                long enumValue = val;
                Expression expr = null;
                long negatedEnumValue = ~val;
                // limit negatedEnumValue to the appropriate range
                switch (enumBaseTypeCode) {
                    case TypeCode.Byte:
                    case TypeCode.SByte:
                        negatedEnumValue &= byte.MaxValue;
                        break;
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                        negatedEnumValue &= ushort.MaxValue;
                        break;
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                        negatedEnumValue &= uint.MaxValue;
                        break;
                }
                Expression negatedExpr = null;
                foreach (IField field in enumDefinition.Fields.Where(fld => fld.IsConst)) {
                    long fieldValue = (long)CSharpPrimitiveCast.Cast(TypeCode.Int64, field.ConstantValue, false);
                    if (fieldValue == 0)
                        continue;    // skip None enum value

                    if ((fieldValue & enumValue) == fieldValue) {
                        var fieldExpression = ConvertType(type).Member(field.Name);
                        if (expr == null)
                            expr = fieldExpression;
                        else
                            expr = new BinaryOperatorExpression(expr, BinaryOperatorType.BitwiseOr, fieldExpression);

                        enumValue &= ~fieldValue;
                    }
                    if ((fieldValue & negatedEnumValue) == fieldValue) {
                        var fieldExpression = ConvertType(type).Member(field.Name);
                        if (negatedExpr == null)
                            negatedExpr = fieldExpression;
                        else
                            negatedExpr = new BinaryOperatorExpression(negatedExpr, BinaryOperatorType.BitwiseOr, fieldExpression);

                        negatedEnumValue &= ~fieldValue;
                    }
                }
                if (enumValue == 0 && expr != null) {
                    if (!(negatedEnumValue == 0 && negatedExpr != null && negatedExpr.Descendants.Count() < expr.Descendants.Count())) {
                        return expr;
                    }
                }
                if (negatedEnumValue == 0 && negatedExpr != null) {
                    return new UnaryOperatorExpression(UnaryOperatorType.BitNot, negatedExpr);
                }
            }
            return new PrimitiveExpression(CSharpPrimitiveCast.Cast(enumBaseTypeCode, val, false)).CastTo(ConvertType(type));
        }

        #endregion

        #region Convert Parameter
        public ParameterDeclaration ConvertParameter(IParameter parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException("parameter");
            ParameterDeclaration decl = new ParameterDeclaration();
            if (parameter.IsRef) {
                decl.ParameterModifier = ParameterModifier.Ref;
            } else if (parameter.IsOut) {
                decl.ParameterModifier = ParameterModifier.Out;
            } else if (parameter.IsParams) {
                decl.ParameterModifier = ParameterModifier.Params;
            }
            if (ShowAttributes) {
                decl.Attributes.AddRange (parameter.Attributes.Select ((a) => new AttributeSection (ConvertAttribute (a))));
            }
            decl.Type = ConvertType(parameter.Type);
            if (this.ShowParameterNames) {
                decl.Name = parameter.Name;
            }
            if (parameter.IsOptional && this.ShowConstantValues) {
                decl.DefaultExpression = ConvertConstantValue(parameter.Type, parameter.ConstantValue);
            }
            return decl;
        }
        #endregion

        #region Convert Entity
        public AstNode ConvertSymbol(ISymbol symbol)
        {
            if (symbol == null)
                throw new ArgumentNullException("symbol");
            switch (symbol.SymbolKind) {
                case SymbolKind.Namespace:
                    return ConvertNamespaceDeclaration((INamespace)symbol);
                case SymbolKind.Variable:
                    return ConvertVariable((IVariable)symbol);
                case SymbolKind.Parameter:
                    return ConvertParameter((IParameter)symbol);
                case SymbolKind.TypeParameter:
                    return ConvertTypeParameter((ITypeParameter)symbol);
                default:
                    if (symbol is IEntity entity)
                        return ConvertEntity(entity);
                    throw new ArgumentException("Invalid value for SymbolKind: " + symbol.SymbolKind);
            }
        }

        public EntityDeclaration ConvertEntity(IEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            switch (entity.SymbolKind) {
                case SymbolKind.TypeDefinition:
                    return ConvertTypeDefinition((ITypeDefinition)entity);
                case SymbolKind.Field:
                    return ConvertField((IField)entity);
                case SymbolKind.Property:
                    return ConvertProperty((IProperty)entity);
                case SymbolKind.Indexer:
                    return ConvertIndexer((IProperty)entity);
                case SymbolKind.Event:
                    return ConvertEvent((IEvent)entity);
                case SymbolKind.Method:
                    return ConvertMethod((IMethod)entity);
                case SymbolKind.Operator:
                    return ConvertOperator((IMethod)entity);
                case SymbolKind.Constructor:
                    return ConvertConstructor((IMethod)entity);
                case SymbolKind.Destructor:
                    return ConvertDestructor((IMethod)entity);
                case SymbolKind.Accessor:
                    IMethod accessor = (IMethod)entity;
                    return ConvertAccessor(accessor, accessor.AccessorOwner != null ? accessor.AccessorOwner.Accessibility : Accessibility.None, false);
                default:
                    throw new ArgumentException("Invalid value for SymbolKind: " + entity.SymbolKind);
            }
        }

        EntityDeclaration ConvertTypeDefinition(ITypeDefinition typeDefinition)
        {
            Modifiers modifiers = Modifiers.None;
            if (this.ShowAccessibility) {
                modifiers |= ModifierFromAccessibility(typeDefinition.Accessibility);
            }
            if (this.ShowModifiers) {
                if (typeDefinition.IsStatic) {
                    modifiers |= Modifiers.Static;
                } else if (typeDefinition.IsAbstract) {
                    modifiers |= Modifiers.Abstract;
                } else if (typeDefinition.IsSealed) {
                    modifiers |= Modifiers.Sealed;
                }
                if (typeDefinition.IsShadowing) {
                    modifiers |= Modifiers.New;
                }
            }

            ClassType classType;
            switch (typeDefinition.Kind) {
                case TypeKind.Struct:
                    classType = ClassType.Struct;
                    modifiers &= ~Modifiers.Sealed;
                    break;
                case TypeKind.Enum:
                    classType = ClassType.Enum;
                    modifiers &= ~Modifiers.Sealed;
                    break;
                case TypeKind.Interface:
                    classType = ClassType.Interface;
                    modifiers &= ~Modifiers.Abstract;
                    break;
                case TypeKind.Delegate:
                    IMethod invoke = typeDefinition.GetDelegateInvokeMethod();
                    if (invoke != null) {
                        return ConvertDelegate(invoke, modifiers);
                    } else {
                        goto default;
                    }
                default:
                    classType = ClassType.Class;
                    break;
            }

            var decl = new TypeDeclaration();
            decl.ClassType = classType;
            decl.Modifiers = modifiers;
            if (ShowAttributes) {
                decl.Attributes.AddRange (typeDefinition.Attributes.Select ((a) => new AttributeSection (ConvertAttribute (a))));
            }
            if (AddResolveResultAnnotations) {
                decl.AddAnnotation(new TypeResolveResult(typeDefinition));
            }
            decl.Name = typeDefinition.Name;

            int outerTypeParameterCount = (typeDefinition.DeclaringTypeDefinition == null) ? 0 : typeDefinition.DeclaringTypeDefinition.TypeParameterCount;

            if (this.ShowTypeParameters) {
                foreach (ITypeParameter tp in typeDefinition.TypeParameters.Skip(outerTypeParameterCount)) {
                    decl.TypeParameters.Add(ConvertTypeParameter(tp));
                }
            }

            if (this.ShowBaseTypes) {
                foreach (IType baseType in typeDefinition.DirectBaseTypes) {
                    if (baseType.IsKnownType (KnownTypeCode.Enum)) {
                        if (!typeDefinition.EnumUnderlyingType.IsKnownType (KnownTypeCode.Int32)) {
                            decl.BaseTypes.Add (ConvertType (typeDefinition.EnumUnderlyingType));
                        }
                    } else if (!baseType.IsKnownType (KnownTypeCode.Object) &&
                         !baseType.IsKnownType (KnownTypeCode.ValueType)) {
                        decl.BaseTypes.Add (ConvertType (baseType));
                    }
                }
            }

            if (this.ShowTypeParameters && this.ShowTypeParameterConstraints) {
                foreach (ITypeParameter tp in typeDefinition.TypeParameters.Skip(outerTypeParameterCount)) {
                    var constraint = ConvertTypeParameterConstraint(tp);
                    if (constraint != null)
                        decl.Constraints.Add(constraint);
                }
            }
            return decl;
        }

        DelegateDeclaration ConvertDelegate(IMethod invokeMethod, Modifiers modifiers)
        {
            ITypeDefinition d = invokeMethod.DeclaringTypeDefinition;

            DelegateDeclaration decl = new DelegateDeclaration();
            decl.Modifiers = modifiers & ~Modifiers.Sealed;
            if (ShowAttributes) {
                decl.Attributes.AddRange (d.Attributes.Select (a => new AttributeSection (ConvertAttribute (a))));
                decl.Attributes.AddRange (invokeMethod.ReturnTypeAttributes.Select ((a) => new AttributeSection (ConvertAttribute (a)) {
                    AttributeTarget = "return"
                }));
            }
            decl.ReturnType = ConvertType(invokeMethod.ReturnType);
            decl.Name = d.Name;

            if (this.ShowTypeParameters) {
                foreach (ITypeParameter tp in d.TypeParameters) {
                    decl.TypeParameters.Add(ConvertTypeParameter(tp));
                }
            }

            foreach (IParameter p in invokeMethod.Parameters) {
                decl.Parameters.Add(ConvertParameter(p));
            }

            if (this.ShowTypeParameters && this.ShowTypeParameterConstraints) {
                foreach (ITypeParameter tp in d.TypeParameters) {
                    var constraint = ConvertTypeParameterConstraint(tp);
                    if (constraint != null)
                        decl.Constraints.Add(constraint);
                }
            }
            return decl;
        }

        FieldDeclaration ConvertField(IField field)
        {
            FieldDeclaration decl = new FieldDeclaration();
            if (ShowModifiers) {
                Modifiers m = GetMemberModifiers(field);
                if (field.IsConst) {
                    m &= ~Modifiers.Static;
                    m |= Modifiers.Const;
                } else if (field.IsReadOnly) {
                    m |= Modifiers.Readonly;
                } else if (field.IsVolatile) {
                    m |= Modifiers.Volatile;
                }
                decl.Modifiers = m;
            }
            if (ShowAttributes) {
                decl.Attributes.AddRange (field.Attributes.Select ((a) => new AttributeSection (ConvertAttribute (a))));
            }
            if (AddResolveResultAnnotations) {
                decl.AddAnnotation(new MemberResolveResult(null, field));
            }
            decl.ReturnType = ConvertType(field.ReturnType);
            Expression initializer = null;
            if (field.IsConst && this.ShowConstantValues)
                initializer = ConvertConstantValue(field.Type, field.ConstantValue);
            decl.Variables.Add(new VariableInitializer(field.Name, initializer));
            return decl;
        }

        BlockStatement GenerateBodyBlock()
        {
            if (GenerateBody) {
                return new BlockStatement {
                    new ThrowStatement(new ObjectCreateExpression(ConvertType(new TopLevelTypeName("System", "NotImplementedException", 0))))
                };
            } else {
                return BlockStatement.Null;
            }
        }

        Accessor ConvertAccessor(IMethod accessor, Accessibility ownerAccessibility, bool addParamterAttribute)
        {
            if (accessor == null)
                return Accessor.Null;
            Accessor decl = new Accessor();
            if (this.ShowAccessibility && accessor.Accessibility != ownerAccessibility)
                decl.Modifiers = ModifierFromAccessibility(accessor.Accessibility);
            if (ShowAttributes) {
                decl.Attributes.AddRange (accessor.Attributes.Select ((a) => new AttributeSection (ConvertAttribute (a))));
                decl.Attributes.AddRange (accessor.ReturnTypeAttributes.Select ((a) => new AttributeSection (ConvertAttribute (a)) {
                    AttributeTarget = "return"
                }));
                if (addParamterAttribute && accessor.Parameters.Count > 0) {
                    decl.Attributes.AddRange (accessor.Parameters.Last ().Attributes.Select ((a) => new AttributeSection (ConvertAttribute (a)) {
                        AttributeTarget = "param"
                    }));
                }
            }
            if (AddResolveResultAnnotations) {
                decl.AddAnnotation(new MemberResolveResult(null, accessor));
            }
            decl.Body = GenerateBodyBlock();
            return decl;
        }

        PropertyDeclaration ConvertProperty(IProperty property)
        {
            PropertyDeclaration decl = new PropertyDeclaration();
            decl.Modifiers = GetMemberModifiers(property);
            if (ShowAttributes) {
                decl.Attributes.AddRange (property.Attributes.Select ((a) => new AttributeSection (ConvertAttribute (a))));
            }
            if (AddResolveResultAnnotations) {
                decl.AddAnnotation(new MemberResolveResult(null, property));
            }
            decl.ReturnType = ConvertType(property.ReturnType);
            decl.Name = property.Name;
            decl.Getter = ConvertAccessor(property.Getter, property.Accessibility, false);
            decl.Setter = ConvertAccessor(property.Setter, property.Accessibility, true);
            decl.PrivateImplementationType = GetExplicitInterfaceType (property);
            return decl;
        }

        IndexerDeclaration ConvertIndexer(IProperty indexer)
        {
            IndexerDeclaration decl = new IndexerDeclaration();
            decl.Modifiers = GetMemberModifiers(indexer);
            if (ShowAttributes) {
                decl.Attributes.AddRange (indexer.Attributes.Select ((a) => new AttributeSection (ConvertAttribute (a))));
            }
            if (AddResolveResultAnnotations) {
                decl.AddAnnotation(new MemberResolveResult(null, indexer));
            }
            decl.ReturnType = ConvertType(indexer.ReturnType);
            foreach (IParameter p in indexer.Parameters) {
                decl.Parameters.Add(ConvertParameter(p));
            }
            decl.Getter = ConvertAccessor(indexer.Getter, indexer.Accessibility, false);
            decl.Setter = ConvertAccessor(indexer.Setter, indexer.Accessibility, true);
            decl.PrivateImplementationType = GetExplicitInterfaceType (indexer);
            return decl;
        }

        EntityDeclaration ConvertEvent(IEvent ev)
        {
            if (this.UseCustomEvents) {
                CustomEventDeclaration decl = new CustomEventDeclaration();
                decl.Modifiers = GetMemberModifiers(ev);
                if (ShowAttributes) {
                    decl.Attributes.AddRange (ev.Attributes.Select ((a) => new AttributeSection (ConvertAttribute (a))));
                }
                if (AddResolveResultAnnotations) {
                    decl.AddAnnotation(new MemberResolveResult(null, ev));
                }
                decl.ReturnType = ConvertType(ev.ReturnType);
                decl.Name = ev.Name;
                decl.AddAccessor    = ConvertAccessor(ev.AddAccessor, ev.Accessibility, true);
                decl.RemoveAccessor = ConvertAccessor(ev.RemoveAccessor, ev.Accessibility, true);
                return decl;
            } else {
                EventDeclaration decl = new EventDeclaration();
                decl.Modifiers = GetMemberModifiers(ev);
                if (ShowAttributes) {
                    decl.Attributes.AddRange (ev.Attributes.Select ((a) => new AttributeSection (ConvertAttribute (a))));
                }
                if (AddResolveResultAnnotations) {
                    decl.AddAnnotation(new MemberResolveResult(null, ev));
                }
                decl.ReturnType = ConvertType(ev.ReturnType);
                decl.Variables.Add(new VariableInitializer(ev.Name));
                return decl;
            }
        }

        MethodDeclaration ConvertMethod(IMethod method)
        {
            MethodDeclaration decl = new MethodDeclaration();
            decl.Modifiers = GetMemberModifiers(method);
            if (method.IsAsync && ShowModifiers)
                decl.Modifiers |= Modifiers.Async;
            if (ShowAttributes) {
                decl.Attributes.AddRange (method.Attributes.Select ((a) => new AttributeSection (ConvertAttribute (a))));
                decl.Attributes.AddRange (method.ReturnTypeAttributes.Select ((a) => new AttributeSection (ConvertAttribute (a)) {
                    AttributeTarget = "return"
                }));
            }
            if (AddResolveResultAnnotations) {
                decl.AddAnnotation(new MemberResolveResult(null, method));
            }
            decl.ReturnType = ConvertType(method.ReturnType);
            decl.Name = method.Name;

            if (this.ShowTypeParameters) {
                foreach (ITypeParameter tp in method.TypeParameters) {
                    decl.TypeParameters.Add(ConvertTypeParameter(tp));
                }
            }

            foreach (IParameter p in method.Parameters) {
                decl.Parameters.Add(ConvertParameter(p));
            }
            if (method.IsExtensionMethod && method.ReducedFrom == null && decl.Parameters.Any() && decl.Parameters.First().ParameterModifier == ParameterModifier.None)
                decl.Parameters.First().ParameterModifier = ParameterModifier.This;

            if (this.ShowTypeParameters && this.ShowTypeParameterConstraints && !method.IsOverride && !method.IsExplicitInterfaceImplementation) {
                foreach (ITypeParameter tp in method.TypeParameters) {
                    var constraint = ConvertTypeParameterConstraint(tp);
                    if (constraint != null)
                        decl.Constraints.Add(constraint);
                }
            }
            decl.Body = GenerateBodyBlock();
            decl.PrivateImplementationType = GetExplicitInterfaceType (method);
            return decl;
        }

        EntityDeclaration ConvertOperator(IMethod op)
        {
            OperatorType? opType = OperatorDeclaration.GetOperatorType(op.Name);
            if (opType == null)
                return ConvertMethod(op);

            OperatorDeclaration decl = new OperatorDeclaration();
            decl.Modifiers = GetMemberModifiers(op);
            decl.OperatorType = opType.Value;
            decl.ReturnType = ConvertType(op.ReturnType);
            foreach (IParameter p in op.Parameters) {
                decl.Parameters.Add(ConvertParameter(p));
            }
            if (AddResolveResultAnnotations) {
                decl.AddAnnotation(new MemberResolveResult(null, op));
            }
            decl.Body = GenerateBodyBlock();
            return decl;
        }

        ConstructorDeclaration ConvertConstructor(IMethod ctor)
        {
            ConstructorDeclaration decl = new ConstructorDeclaration();
            decl.Modifiers = GetMemberModifiers(ctor);
            if (ShowAttributes)
                decl.Attributes.AddRange (ctor.Attributes.Select ((a) => new AttributeSection (ConvertAttribute (a))));
            if (ctor.DeclaringTypeDefinition != null)
                decl.Name = ctor.DeclaringTypeDefinition.Name;
            foreach (IParameter p in ctor.Parameters) {
                decl.Parameters.Add(ConvertParameter(p));
            }
            if (AddResolveResultAnnotations) {
                decl.AddAnnotation(new MemberResolveResult(null, ctor));
            }
            decl.Body = GenerateBodyBlock();
            return decl;
        }

        DestructorDeclaration ConvertDestructor(IMethod dtor)
        {
            DestructorDeclaration decl = new DestructorDeclaration();
            if (dtor.DeclaringTypeDefinition != null)
                decl.Name = dtor.DeclaringTypeDefinition.Name;
            if (AddResolveResultAnnotations) {
                decl.AddAnnotation(new MemberResolveResult(null, dtor));
            }
            decl.Body = GenerateBodyBlock();
            return decl;
        }
        #endregion

        #region Convert Modifiers
        public static Modifiers ModifierFromAccessibility(Accessibility accessibility)
        {
            switch (accessibility) {
                case Accessibility.Private:
                    return Modifiers.Private;
                case Accessibility.Public:
                    return Modifiers.Public;
                case Accessibility.Protected:
                    return Modifiers.Protected;
                case Accessibility.Internal:
                    return Modifiers.Internal;
                case Accessibility.ProtectedOrInternal:
                case Accessibility.ProtectedAndInternal:
                    return Modifiers.Protected | Modifiers.Internal;
                default:
                    return Modifiers.None;
            }
        }

        bool NeedsAccessibility(IMember member)
        {
            var declaringType = member.DeclaringType;
            if ((declaringType != null && declaringType.Kind == TypeKind.Interface) || member.IsExplicitInterfaceImplementation)
                return false;
            switch (member.SymbolKind) {
                case SymbolKind.Constructor:
                    return !member.IsStatic;
                case SymbolKind.Destructor:
                    return false;
                default:
                    return true;
            }
        }

        Modifiers GetMemberModifiers(IMember member)
        {
            Modifiers m = Modifiers.None;
            if (this.ShowAccessibility && NeedsAccessibility(member)) {
                m |= ModifierFromAccessibility (member.Accessibility);
            }
            if (this.ShowModifiers) {
                if (member.IsStatic) {
                    m |= Modifiers.Static;
                } else {
                    var declaringType = member.DeclaringType;
                    if (member.IsAbstract && declaringType != null && declaringType.Kind != TypeKind.Interface)
                        m |= Modifiers.Abstract;
                    if (member.IsOverride)
                        m |= Modifiers.Override;
                    if (member.IsVirtual && !member.IsAbstract && !member.IsOverride)
                        m |= Modifiers.Virtual;
                    if (member.IsSealed)
                        m |= Modifiers.Sealed;
                }
                if (member.IsShadowing)
                    m |= Modifiers.New;
            }
            return m;
        }
        #endregion

        #region Convert Type Parameter
        TypeParameterDeclaration ConvertTypeParameter(ITypeParameter tp)
        {
            TypeParameterDeclaration decl = new TypeParameterDeclaration();
            decl.Variance = tp.Variance;
            decl.Name = tp.Name;
            if (ShowAttributes)
                decl.Attributes.AddRange (tp.Attributes.Select ((a) => new AttributeSection (ConvertAttribute (a))));
            return decl;
        }

        Constraint ConvertTypeParameterConstraint(ITypeParameter tp)
        {
            if (!tp.HasDefaultConstructorConstraint && !tp.HasReferenceTypeConstraint && !tp.HasValueTypeConstraint && tp.DirectBaseTypes.All(IsObjectOrValueType)) {
                return null;
            }
            Constraint c = new Constraint();
            c.TypeParameter = new SimpleType (tp.Name);
            if (tp.HasReferenceTypeConstraint) {
                c.BaseTypes.Add(new PrimitiveType("class"));
            } else if (tp.HasValueTypeConstraint) {
                c.BaseTypes.Add(new PrimitiveType("struct"));
            }
            foreach (IType t in tp.DirectBaseTypes) {
                if (!IsObjectOrValueType(t))
                    c.BaseTypes.Add(ConvertType(t));
            }
            if (tp.HasDefaultConstructorConstraint && !tp.HasValueTypeConstraint) {
                c.BaseTypes.Add(new PrimitiveType("new"));
            }
            return c;
        }

        static bool IsObjectOrValueType(IType type)
        {
            ITypeDefinition d = type.GetDefinition();
            return d != null && (d.KnownTypeCode == KnownTypeCode.Object || d.KnownTypeCode == KnownTypeCode.ValueType);
        }
        #endregion

        #region Convert Variable
        public VariableDeclarationStatement ConvertVariable(IVariable v)
        {
            VariableDeclarationStatement decl = new VariableDeclarationStatement();
            decl.Modifiers = v.IsConst ? Modifiers.Const : Modifiers.None;
            decl.Type = ConvertType(v.Type);
            Expression initializer = null;
            if (v.IsConst)
                initializer = ConvertConstantValue(v.Type, v.ConstantValue);
            decl.Variables.Add(new VariableInitializer(v.Name, initializer));
            return decl;
        }
        #endregion

        NamespaceDeclaration ConvertNamespaceDeclaration(INamespace ns)
        {
            return new NamespaceDeclaration(ns.FullName);
        }

        AstType GetExplicitInterfaceType (IMember member)
        {
            if (member.IsExplicitInterfaceImplementation) {
                var baseMember = member.ImplementedInterfaceMembers.FirstOrDefault ();
                if (baseMember != null)
                    return ConvertType (baseMember.DeclaringType);
            }
            return null;
        }
    }
}
