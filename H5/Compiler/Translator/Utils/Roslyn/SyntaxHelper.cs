using H5.Contract;
using H5.Contract.Constants;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis.Text;

namespace H5.Translator
{
    /// <summary>
    /// Contains useful helper methods to generate Roslyn syntax tree parts.
    /// </summary>
    static internal class SyntaxHelper
    {
        public static bool IsNumeric(Type type)
        {
            if (type == null)
            {
                return false;
            }

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
                case TypeCode.Object:
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        return IsNumeric(Nullable.GetUnderlyingType(type));
                    }
                    return false;
            }

            return false;
        }

        public static bool IsChildOf(SyntaxNode node, SyntaxNode parent)
        {
            return parent.FullSpan.Contains(node.FullSpan);
        }

        public static bool RequireReturnStatement(SemanticModel model, SyntaxNode lambda)
        {
            var typeInfo = model.GetTypeInfo(lambda);
            var type = typeInfo.ConvertedType ?? typeInfo.Type;
            if (type == null || !type.IsDelegateType())
            {
                return false;
            }

            var returnType = type.GetDelegateInvokeMethod().GetReturnType();
            return returnType != null && returnType.SpecialType != SpecialType.System_Void;
        }

        public static ITypeSymbol GetReturnType(this ISymbol symbol)
        {
            if (symbol == null)
                throw new ArgumentNullException("symbol");
            switch (symbol.Kind)
            {
                case SymbolKind.Field:
                    var field = (IFieldSymbol)symbol;
                    return field.Type;
                case SymbolKind.Method:
                    var method = (IMethodSymbol)symbol;
                    if (method.MethodKind == MethodKind.Constructor)
                        return method.ContainingType;
                    return method.ReturnType;
                case SymbolKind.Property:
                    var property = (IPropertySymbol)symbol;
                    return property.Type;
                case SymbolKind.Event:
                    var evt = (IEventSymbol)symbol;
                    return evt.Type;
                case SymbolKind.Parameter:
                    var param = (IParameterSymbol)symbol;
                    return param.Type;
                case SymbolKind.Local:
                    var local = (ILocalSymbol)symbol;
                    return local.Type;
            }
            return null;
        }

        public static bool IsDelegateType(this ITypeSymbol symbol)
        {
            return symbol?.TypeKind == TypeKind.Delegate;
        }

        public static IMethodSymbol GetDelegateInvokeMethod(this ITypeSymbol type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (type.TypeKind == TypeKind.Delegate)
                return type.GetMembers("Invoke").OfType<IMethodSymbol>().FirstOrDefault(m => m.MethodKind == MethodKind.DelegateInvoke);
            return null;
        }

        /// <summary>
        /// Generates the static method call.
        /// </summary>
        public static ExpressionStatementSyntax GenerateStaticMethodCall(string methodName, string className, ArgumentSyntax[] arguments = null, ITypeSymbol[] typeArguments = null)
        {
            var methodIdentifier = GenerateMethodIdentifier(methodName, className, typeArguments);
            return SyntaxFactory.ExpressionStatement(
                SyntaxFactory.InvocationExpression(methodIdentifier,
                SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(arguments ?? new ArgumentSyntax[] { })))
            );
        }

        public static InvocationExpressionSyntax GenerateInvocation(string methodName, string targetIdentifier, ArgumentSyntax[] arguments = null, ITypeSymbol[] typeArguments = null)
        {
            var methodIdentifier = GenerateMethodIdentifier(methodName, targetIdentifier, typeArguments);
            return SyntaxFactory.InvocationExpression(methodIdentifier, SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(arguments ?? new ArgumentSyntax[] { })));
        }

        public static InvocationExpressionSyntax GenerateInvocation(string methodName, ExpressionSyntax targetIdentifier, ArgumentSyntax[] arguments = null, ITypeSymbol[] typeArguments = null)
        {
            var methodIdentifier = GenerateMethodIdentifier(methodName, targetIdentifier, typeArguments);
            return SyntaxFactory.InvocationExpression(methodIdentifier, SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(arguments ?? new ArgumentSyntax[] { })));
        }

        /// <summary>
        /// Generates the method call.
        /// </summary>
        public static ExpressionStatementSyntax GenerateMethodCall(string methodName, string targetIdentifier, ArgumentSyntax[] arguments = null, ITypeSymbol[] typeArguments = null)
        {
            var methodIdentifier = GenerateMethodIdentifier(methodName, targetIdentifier, typeArguments);
            return SyntaxFactory.ExpressionStatement(
                SyntaxFactory.InvocationExpression(methodIdentifier,
                SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(arguments ?? new ArgumentSyntax[] { })))
            );
        }

        /// <summary>
        /// Generates the method call.
        /// </summary>
        public static ExpressionStatementSyntax GenerateMethodCall(string methodName, ExpressionSyntax targetIdentifier, ArgumentSyntax[] arguments = null, ITypeSymbol[] typeArguments = null)
        {
            var methodIdentifier = GenerateMethodIdentifier(methodName, targetIdentifier, typeArguments);
            return SyntaxFactory.ExpressionStatement(
                SyntaxFactory.InvocationExpression(methodIdentifier,
                SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(arguments ?? new ArgumentSyntax[] { })))
            );
        }

        /// <summary>
        /// Generates the method identifier.
        /// </summary>
        public static ExpressionSyntax GenerateMethodIdentifier(string methodName, string targetIdentifierOrTypeName, ITypeSymbol[] typeArguments = null)
        {
            ExpressionSyntax methodIdentifier = SyntaxFactory.IdentifierName(targetIdentifierOrTypeName + "." + methodName);
            if (typeArguments != null && typeArguments.Length > 0)
            {
                methodIdentifier = SyntaxFactory.GenericName(SyntaxFactory.Identifier(targetIdentifierOrTypeName + "." + methodName),
                    SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(typeArguments.Select(GenerateTypeSyntax))));
            }
            return methodIdentifier;
        }

        /// <summary>
        /// Generates the method identifier.
        /// </summary>
        public static ExpressionSyntax GenerateMethodIdentifier(string methodName, ExpressionSyntax targetIdentifierOrTypeName, ITypeSymbol[] typeArguments = null)
        {
            return SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, targetIdentifierOrTypeName, SyntaxFactory.IdentifierName(methodName));
        }

        /// <summary>
        /// Generates the variable declaration and object creation statement.
        /// </summary>
        public static LocalDeclarationStatementSyntax GenerateVariableDeclarationAndObjectCreationStatement(string variableName, string typeName)
        {
            return GenerateVariableDeclarationAndObjectCreationStatement(variableName, () => SyntaxFactory.ParseTypeName(typeName));
        }

        /// <summary>
        /// Generates the variable declaration and object creation statement.
        /// </summary>
        public static LocalDeclarationStatementSyntax GenerateVariableDeclarationAndObjectCreationStatement(string variableName, Type type, SemanticModel model, int pos)
        {
            return GenerateVariableDeclarationAndObjectCreationStatement(variableName, () => GenerateTypeSyntax(type, model, pos));
        }

        /// <summary>
        /// Generates the variable declaration and object creation statement.
        /// </summary>
        private static LocalDeclarationStatementSyntax GenerateVariableDeclarationAndObjectCreationStatement(string variableName, Func<TypeSyntax> typeSyntaxFactory)
        {
            return SyntaxFactory.LocalDeclarationStatement(
                SyntaxFactory.VariableDeclaration(
                    typeSyntaxFactory(),
                    SyntaxFactory.SeparatedList(new[] {
                        SyntaxFactory.VariableDeclarator(
                            SyntaxFactory.Identifier(variableName),
                            null,
                            SyntaxFactory.EqualsValueClause(
                                SyntaxFactory.ObjectCreationExpression(typeSyntaxFactory(), SyntaxFactory.ArgumentList(), null)
                            )
                        )
                    })
                )
            );
        }

        /// <summary>
        /// Generates the extension method.
        /// </summary>
        public static MethodDeclarationSyntax GenerateExtensionMethod(string methodName, string returnTypeName, ParameterSyntax[] parameters, AttributeSyntax[] attributes = null)
        {
            var methodDeclaration = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName(returnTypeName ?? "void"), methodName)
                .WithModifiers(SyntaxTokenList.Create(SyntaxFactory.Token(SyntaxKind.PublicKeyword)).Add(SyntaxFactory.Token(SyntaxKind.StaticKeyword)))
                .WithParameterList(
                    SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList(parameters))
                );

            if (attributes != null)
            {
                methodDeclaration = methodDeclaration.WithAttributeLists(SyntaxFactory.List(new[]
                {
                    SyntaxFactory.AttributeList(SyntaxFactory.SeparatedList(attributes))
                }));
            }
            return methodDeclaration;
        }

        /// <summary>
        /// Generates the method parameter.
        /// </summary>
        public static ParameterSyntax GenerateMethodParameter(string parameterName, string typeName, bool isExtensionMethodFirstParameter)
        {
            return SyntaxFactory.Parameter(
                SyntaxFactory.List<AttributeListSyntax>(),
                isExtensionMethodFirstParameter ? SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.ThisKeyword)) : SyntaxFactory.TokenList(),
                SyntaxFactory.ParseTypeName(typeName),
                SyntaxFactory.Identifier(parameterName),
                null
            );
        }

        /// <summary>
        /// Generates the class.
        /// </summary>
        public static ClassDeclarationSyntax GenerateClass(string className, SyntaxKind[] modifiers, MemberDeclarationSyntax[] methods, AttributeSyntax[] attributes = null)
        {
            var list = new SyntaxTokenList();
            list = list.AddRange(modifiers.Select(SyntaxFactory.Token).ToArray());
            var classDeclaration = SyntaxFactory.ClassDeclaration(className)
                .WithModifiers(
                    list
                )
                .WithMembers(
                    SyntaxFactory.List(methods)
                );

            if (attributes != null)
            {
                classDeclaration = classDeclaration.WithAttributeLists(SyntaxFactory.List(new[] {
                    SyntaxFactory.AttributeList(SyntaxFactory.SeparatedList(attributes))
                }));
            }
            return classDeclaration;
        }

        /// <summary>
        /// Generates the namespace.
        /// </summary>
        public static NamespaceDeclarationSyntax GenerateNamespace(string namespaceName, MemberDeclarationSyntax[] members, IEnumerable<string> usings = null)
        {
            var builtinUsings = new List<string>() { "System", "System.Collections.Generic" };
            if (usings != null)
            {
                builtinUsings.AddRange(usings);
            }

            return SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(namespaceName))
                .WithUsings(SyntaxFactory.List(builtinUsings.Distinct().Where(u => namespaceName != u).Select(u => SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(u))).ToArray()))
                .WithMembers(SyntaxFactory.List(members));
        }

        /// <summary>
        /// Generates the type syntax.
        /// </summary>
        public static TypeSyntax GenerateTypeSyntax(Type type, SemanticModel model, int pos)
        {
            var name = GetTypeName(type);

            if (type.IsGenericType)
            {
                var genericArguments = type.GetGenericArguments();
                return GenerateGenericName(name, genericArguments, model, pos);
            }

            return SyntaxFactory.ParseTypeName(name);
        }

        public static TypeSyntax GenerateTypeSyntax(ITypeSymbol type)
        {
            if (type.IsTupleType)
            {
                var elements = ((INamedTypeSymbol)type).TupleElements;
                var types = new List<TypeSyntax>();
                foreach (var el in elements)
                {
                    types.Add(GenerateTypeSyntax(el.Type));
                }

                return SyntaxFactory.GenericName(SyntaxFactory.Identifier("System.ValueTuple"), SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(types)));
            }

            if (type.OriginalDefinition != null && type.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T)
            {
                var underlying = ((INamedTypeSymbol)type).TypeArguments[0];
                return SyntaxFactory.NullableType(GenerateTypeSyntax(underlying));
            }

            if (type is INamedTypeSymbol namedType && namedType.IsGenericType)
            {
                var elements = namedType.TypeArguments;
                var types = new List<TypeSyntax>();
                foreach (var el in elements)
                {
                    types.Add(GenerateTypeSyntax(el));
                }

                var format = new SymbolDisplayFormat(
                    globalNamespaceStyle: SymbolDisplayGlobalNamespaceStyle.Omitted,
                    typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
                    genericsOptions: SymbolDisplayGenericsOptions.None,
                    miscellaneousOptions: SymbolDisplayMiscellaneousOptions.EscapeKeywordIdentifiers
                );
                var openName = type.ToDisplayString(format);
                var openTypeSyntax = SyntaxFactory.ParseTypeName(openName);

                if (openTypeSyntax is QualifiedNameSyntax qns)
                {
                    var right = qns.Right;
                    var genericRight = SyntaxFactory.GenericName(right.Identifier, SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(types)));
                    return qns.WithRight(genericRight);
                }
                else if (openTypeSyntax is AliasQualifiedNameSyntax aqns)
                {
                    var right = aqns.Name;
                    if (right is IdentifierNameSyntax id)
                    {
                        var genericRight = SyntaxFactory.GenericName(id.Identifier, SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(types)));
                        return aqns.WithName(genericRight);
                    }
                }
                else if (openTypeSyntax is IdentifierNameSyntax id)
                {
                    return SyntaxFactory.GenericName(id.Identifier, SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(types)));
                }

                return SyntaxFactory.GenericName(SyntaxFactory.Identifier(openName), SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(types)));
            }

            return SyntaxFactory.ParseTypeName(type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)).WithoutTrivia();
        }

        public static TypeSyntax GenerateTypeSyntax(ITypeSymbol type, SemanticModel model, int pos, SharpSixRewriter rewriter)
        {
            if (type.IsTupleType)
            {
                var elements = ((INamedTypeSymbol)type).TupleElements;
                var types = new List<TypeSyntax>();
                foreach (var el in elements)
                {
                    types.Add(GenerateTypeSyntax(el.Type, model, pos, rewriter));
                }

                return SyntaxFactory.GenericName(SyntaxFactory.Identifier("System.ValueTuple"), SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(types)));
            }

            var typeName = type.FullyQualifiedName(false);
            if (rewriter.usingStaticNames.Any(n => typeName.StartsWith(n + '.')))
            {
                return SyntaxFactory.ParseTypeName(type.ToDisplayString());
            }
            else if (type is INamedTypeSymbol namedType && namedType.IsGenericType)
            {
                var elements = namedType.TypeArguments;
                var types = new List<TypeSyntax>();
                foreach (var el in elements)
                {
                    types.Add(GenerateTypeSyntax(el, model, pos, rewriter));
                }

                if (type.OriginalDefinition != null && type.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T)
                {
                    return SyntaxFactory.IdentifierName(type.ToMinimalDisplayString(
                            model,
                            pos,
                            new SymbolDisplayFormat(
                                genericsOptions: SymbolDisplayGenericsOptions.None
                            )
                        ));
                }

                if (types.Count > 0)
                {
                    string gtypeName;
                    if (type.ContainingType != null)
                    {
                        var parent = GenerateTypeSyntax(type.ContainingType, model, pos, rewriter);
                        var name = type.Name;
                        gtypeName = SyntaxFactory.QualifiedName((NameSyntax)parent, SyntaxFactory.IdentifierName(name)).ToString();
                    }
                    else
                    {
                        gtypeName = type.ToMinimalDisplayString(
                              model,
                              pos,
                              new SymbolDisplayFormat(
                                  genericsOptions: SymbolDisplayGenericsOptions.None
                              )
                          );
                    }

                    if (model != null)
                    {
                        var ai = model.GetSpeculativeAliasInfo(pos, SyntaxFactory.IdentifierName(gtypeName), SpeculativeBindingOption.BindAsTypeOrNamespace);

                        if (ai != null && ai.Name == gtypeName)
                        {
                            return SyntaxFactory.ParseTypeName(gtypeName);
                        }
                    }                    

                    return SyntaxFactory.GenericName(
                        SyntaxFactory.Identifier(gtypeName),
                        SyntaxFactory.TypeArgumentList(
                            SyntaxFactory.SeparatedList(types)
                        )
                    );
                }
            }

            if (type.ContainingType != null && type.Kind != SymbolKind.TypeParameter)
            {
                var parent = GenerateTypeSyntax(type.ContainingType, model, pos, rewriter);
                var name = type.Name;
                return SyntaxFactory.QualifiedName((NameSyntax)parent, SyntaxFactory.IdentifierName(name));
            }

            return SyntaxFactory.ParseTypeName(type.ToMinimalDisplayString(model, pos));
        }

        /// <summary>
        /// Generates the name of the generic.
        /// </summary>
        public static GenericNameSyntax GenerateGenericName(string name, IEnumerable<Type> types, SemanticModel model, int pos)
        {
            return SyntaxFactory.GenericName(SyntaxFactory.Identifier(name),
                SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(types.Select((type) => GenerateTypeSyntax(type, model, pos))))
            );
        }

        public static GenericNameSyntax GenerateGenericName(SyntaxToken name, IEnumerable<ITypeSymbol> types, SemanticModel model, int pos, SharpSixRewriter rewriter)
        {
            return SyntaxFactory.GenericName(name, SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(types.Select((type) => GenerateTypeSyntax(type, model, pos, rewriter)))));
        }

        /// <summary>
        /// Gets the C# representation of System.Type with respect to generics.
        /// </summary>
        private static string GetTypeName(Type type)
        {
            var name = type.Name.Replace("+", ".");

            if (name.Contains("`"))
            {
                name = name.Substring(0, name.IndexOf("`"));
            }

            return CS.NS.GLOBAL + name;
        }

        /// <summary>
        /// Generates the assignment statement.
        /// </summary>
        public static ExpressionStatementSyntax GenerateAssignmentStatement(ExpressionSyntax leftSide, ExpressionSyntax rightSide)
        {
            return SyntaxFactory.ExpressionStatement(
                SyntaxFactory.BinaryExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    leftSide,
                    rightSide
                )
            );
        }

        /// <summary>
        /// Generates the attribute on the class or method.
        /// </summary>
        public static AttributeSyntax GenerateAttribute(Type type, params ExpressionSyntax[] parameters)
        {
            return SyntaxFactory.Attribute(SyntaxFactory.ParseName(type.FullName),
                SyntaxFactory.AttributeArgumentList(SyntaxFactory.SeparatedList(parameters.Select(SyntaxFactory.AttributeArgument)))
            );
        }

        public static bool IsCSharpKeyword(this string name)
        {
            switch (name)
            {
                case "bool":
                case "byte":
                case "sbyte":
                case "short":
                case "ushort":
                case "int":
                case "uint":
                case "long":
                case "ulong":
                case "double":
                case "float":
                case "decimal":
                case "string":
                case "char":
                case "object":
                case "typeof":
                case "sizeof":
                case "null":
                case "true":
                case "false":
                case "if":
                case "else":
                case "while":
                case "for":
                case "foreach":
                case "do":
                case "switch":
                case "case":
                case "default":
                case "lock":
                case "try":
                case "throw":
                case "catch":
                case "finally":
                case "goto":
                case "break":
                case "continue":
                case "return":
                case "public":
                case "private":
                case "internal":
                case "protected":
                case "static":
                case "readonly":
                case "sealed":
                case "const":
                case "new":
                case "override":
                case "abstract":
                case "virtual":
                case "partial":
                case "ref":
                case "out":
                case "in":
                case "where":
                case "params":
                case "this":
                case "base":
                case "namespace":
                case "using":
                case "class":
                case "struct":
                case "interface":
                case "delegate":
                case "checked":
                case "get":
                case "set":
                case "add":
                case "remove":
                case "operator":
                case "implicit":
                case "explicit":
                case "fixed":
                case "extern":
                case "event":
                case "enum":
                case "unsafe":
                    return true;
                default:
                    return false;
            }
        }

        public static string GetFullyQualifiedNameAndValidate(this ISymbol symbol, SemanticModel semanticModel, int position, bool appenTypeArgs = true)
        {
            var name = symbol.FullyQualifiedName(appenTypeArgs);

            if (symbol is ITypeSymbol)
            {
                var ti = semanticModel.GetSpeculativeTypeInfo(position, SyntaxFactory.ParseTypeName(name), SpeculativeBindingOption.BindAsTypeOrNamespace);
                var type = ti.Type ?? ti.ConvertedType;

            }
            else
            {
                var si = semanticModel.GetSpeculativeSymbolInfo(position, SyntaxFactory.ParseExpression(name), SpeculativeBindingOption.BindAsExpression);

            }

            return name;
        }

        public static string FullyQualifiedName(this ISymbol symbol, bool appenTypeArgs = true)
        {
            if (symbol is IArrayTypeSymbol at)
            {
                string result = at.ElementType.FullyQualifiedName() + "[";

                for (int i = 0; i < at.Rank - 1; i++)
                {
                    result += ",";
                }

                result += "]";

                return result;
            }

            if (symbol is INamedTypeSymbol typeSymbol && typeSymbol.IsTupleType)
            {
                symbol = typeSymbol.TupleUnderlyingType;
            }

            var localName = symbol.Name;

            if (IsCSharpKeyword(localName))
            {
                localName = "@" + localName;
            }

            if (symbol is ITypeParameterSymbol)
            {
                return localName;
            }

            if (appenTypeArgs)
            {
                if (symbol is INamedTypeSymbol)
                {
                    localName = AppendTypeArguments(localName, ((INamedTypeSymbol)symbol).TypeArguments);
                }
                else if (symbol is IMethodSymbol)
                {
                    localName = AppendTypeArguments(localName, ((IMethodSymbol)symbol).TypeArguments);
                }
            }

            if (symbol.ContainingType != null)
            {
                return symbol.ContainingType.FullyQualifiedName() + "." + localName;
            }
            else if (symbol.ContainingNamespace != null)
            {
                if (symbol.ContainingNamespace.IsGlobalNamespace)
                {
                    //return CS.NS.GLOBAL + localName;
                    return localName;
                }

                return symbol.ContainingNamespace.FullyQualifiedName() + "." + localName;
            }
            else
            {
                return localName;
            }
        }

        public static bool IsAnonymous(ITypeSymbol type)
        {
            if (type.IsAnonymousType)
            {
                return true;
            }

            if (type is INamedTypeSymbol namedType && namedType.IsGenericType)
            {
                return namedType.TypeArguments.Any(IsAnonymous);
            }

            return false;
        }


        private static string AppendTypeArguments(string localName, IReadOnlyCollection<ITypeSymbol> typeArguments)
        {
            if (typeArguments.Count > 0 && !typeArguments.Any(IsAnonymous))
            {
                bool first = true;

                foreach (var ta in typeArguments)
                {
                    localName += (first ? "<" : ", ") + ta.FullyQualifiedName();
                    first = false;
                }

                localName += ">";
            }

            return localName;
        }

        public static bool IsAutoProperty(this PropertyDeclarationSyntax propertyDeclaration)
        {
            if (propertyDeclaration.Modifiers.Any(m => m.IsKind(SyntaxKind.AbstractKeyword) || m.IsKind(SyntaxKind.ExternKeyword)))
            {
                return false;
            }

            if (propertyDeclaration.AccessorList == null)
            {
                return false;
            }

            var getter = propertyDeclaration.AccessorList.Accessors.SingleOrDefault(a => a.Keyword.IsKind(SyntaxKind.GetKeyword));

            if (getter == null || getter.Body != null)
            {
                return false;
            }

            var setter = propertyDeclaration.AccessorList.Accessors.SingleOrDefault(a => a.Keyword.IsKind(SyntaxKind.SetKeyword));

            if (setter != null && setter.Body != null)
            {
                return false;
            }

            return true;
        }

        public static T RemoveSemicolon<T>(T node, SyntaxToken semicolonToken, Func<SyntaxToken, T> withSemicolonToken) where T : SyntaxNode
        {
            if (!semicolonToken.IsKind(SyntaxKind.None))
            {
                var leadingTrivia = semicolonToken.LeadingTrivia;
                var trailingTrivia = semicolonToken.TrailingTrivia;

                SyntaxToken newToken = SyntaxFactory.Token(
                  leadingTrivia,
                  SyntaxKind.None,
                  trailingTrivia);

                bool addNewline = semicolonToken.HasTrailingTrivia
                  && trailingTrivia.Count() == 1
                  && trailingTrivia.First().IsKind(SyntaxKind.EndOfLineTrivia);

                var newNode = withSemicolonToken(newToken);

                if (addNewline)
                {
                    return newNode.WithTrailingTrivia(SyntaxFactory.Whitespace(Environment.NewLine));
                }

                return newNode;
            }
            return node;
        }

        public static SyntaxNode Wrap(this SyntaxNode node, SyntaxNode oldNode)
        {
            var leadingTrivia = oldNode.GetLeadingTrivia();
            var trailingTrivia = oldNode.GetTrailingTrivia();

            node = node.WithLeadingTrivia(leadingTrivia);
            node = node.WithTrailingTrivia(trailingTrivia);

            return node;
        }

        public static PropertyDeclarationSyntax ToStatementBody(PropertyDeclarationSyntax property)
        {
            var accessor = SyntaxFactory
                               .AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                               .WithBody(SyntaxFactory.Block(SyntaxFactory.ReturnStatement(property.ExpressionBody.Expression.WithLeadingTrivia(SyntaxFactory.Space))));

            var accessorDeclList = new SyntaxList<AccessorDeclarationSyntax>();
            accessorDeclList = accessorDeclList.Add(accessor);

            return property
                .WithAccessorList(SyntaxFactory.AccessorList(accessorDeclList))
                .WithExpressionBody(null)
                .WithSemicolonToken(SyntaxFactory.MissingToken(SyntaxKind.SemicolonToken))
                .WithTrailingTrivia(property.GetTrailingTrivia());
        }

        public static MethodDeclarationSyntax ToStatementBody(MethodDeclarationSyntax method)
        {
            var isVoid = false;
            if (method.ReturnType is PredefinedTypeSyntax predefined && predefined.Keyword.IsKind(SyntaxKind.VoidKeyword))
            {
                isVoid = true;
            }

            var body = method.ExpressionBody.Expression.WithLeadingTrivia(SyntaxFactory.Space);

            return method.WithBody(SyntaxFactory.Block(isVoid ? SyntaxFactory.ExpressionStatement(body) : (StatementSyntax)SyntaxFactory.ReturnStatement(body)))
                         .WithExpressionBody(null)
                         .WithSemicolonToken(SyntaxFactory.MissingToken(SyntaxKind.SemicolonToken))
                         .WithTrailingTrivia(method.GetTrailingTrivia());
        }

        public static ConstructorDeclarationSyntax ToStatementBody(ConstructorDeclarationSyntax method)
        {
            var body = method.ExpressionBody.Expression.WithLeadingTrivia(SyntaxFactory.Space);

            return method.WithBody(SyntaxFactory.Block(SyntaxFactory.ExpressionStatement(body)))
                         .WithExpressionBody(null)
                         .WithSemicolonToken(SyntaxFactory.MissingToken(SyntaxKind.SemicolonToken))
                         .WithTrailingTrivia(method.GetTrailingTrivia());
        }

        public static DestructorDeclarationSyntax ToStatementBody(DestructorDeclarationSyntax method)
        {
            var body = method.ExpressionBody.Expression.WithLeadingTrivia(SyntaxFactory.Space);

            return method.WithBody(SyntaxFactory.Block(SyntaxFactory.ExpressionStatement(body)))
                         .WithExpressionBody(null)
                         .WithSemicolonToken(SyntaxFactory.MissingToken(SyntaxKind.SemicolonToken))
                         .WithTrailingTrivia(method.GetTrailingTrivia());
        }

        public static AccessorDeclarationSyntax ToStatementBody(AccessorDeclarationSyntax method)
        {
            var needReturn = method.Keyword.IsKind(SyntaxKind.GetKeyword);

            var body = method.ExpressionBody.Expression.WithLeadingTrivia(SyntaxFactory.Space);

            return method.WithBody(SyntaxFactory.Block(needReturn ? (StatementSyntax)SyntaxFactory.ReturnStatement(body) : SyntaxFactory.ExpressionStatement(body)))
                         .WithExpressionBody(null)
                         .WithSemicolonToken(SyntaxFactory.MissingToken(SyntaxKind.SemicolonToken))
                         .WithTrailingTrivia(method.GetTrailingTrivia());
        }

        public static OperatorDeclarationSyntax ToStatementBody(OperatorDeclarationSyntax method)
        {
            var isVoid = false;
            if (method.ReturnType is PredefinedTypeSyntax predefined && predefined.Keyword.IsKind(SyntaxKind.VoidKeyword))
            {
                isVoid = true;
            }

            var body = method.ExpressionBody.Expression.WithLeadingTrivia(SyntaxFactory.Space);

            return method.WithBody(SyntaxFactory.Block(isVoid ? SyntaxFactory.ExpressionStatement(body) : (StatementSyntax)SyntaxFactory.ReturnStatement(body)))
                         .WithExpressionBody(null)
                         .WithSemicolonToken(SyntaxFactory.MissingToken(SyntaxKind.SemicolonToken))
                         .WithTrailingTrivia(method.GetTrailingTrivia());
        }

        public static ConversionOperatorDeclarationSyntax ToStatementBody(ConversionOperatorDeclarationSyntax method)
        {
            var body = method.ExpressionBody.Expression.WithLeadingTrivia(SyntaxFactory.Space);

            return method.WithBody(SyntaxFactory.Block((StatementSyntax)SyntaxFactory.ReturnStatement(body)))
                         .WithExpressionBody(null)
                         .WithSemicolonToken(SyntaxFactory.MissingToken(SyntaxKind.SemicolonToken))
                         .WithTrailingTrivia(method.GetTrailingTrivia());
        }

        public static IndexerDeclarationSyntax ToStatementBody(IndexerDeclarationSyntax property)
        {
            var accessor = SyntaxFactory
                               .AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                               .WithBody(SyntaxFactory.Block(SyntaxFactory.ReturnStatement(property.ExpressionBody.Expression.WithLeadingTrivia(SyntaxFactory.Space))));

            var accessorDeclList = new SyntaxList<AccessorDeclarationSyntax>();
            accessorDeclList = accessorDeclList.Add(accessor);

            return property
                .WithAccessorList(SyntaxFactory.AccessorList(accessorDeclList))
                .WithExpressionBody(null)
                .WithSemicolonToken(SyntaxFactory.MissingToken(SyntaxKind.SemicolonToken))
                .WithTrailingTrivia(property.GetTrailingTrivia());
        }

        public static string GetSymbolName(InvocationExpressionSyntax node, SymbolInfo si, string name, SemanticModel semanticModel)
        {
            var symbol = si.Symbol;

            if (symbol == null && si.CandidateSymbols.Any())
            {
                symbol = si.CandidateSymbols.First();
            }

            if (symbol != null && symbol.Kind != SymbolKind.Namespace)
            {
                //bool preserveMemberChange = !(symbol.Kind == SymbolKind.Method || symbol.Kind == SymbolKind.Property);

                int enumMode = -1;

                if (symbol.ContainingType != null && symbol.ContainingType.TypeKind == TypeKind.Enum && symbol is IFieldSymbol)
                {
                    string enumAttr = Translator.H5_ASSEMBLY + ".EnumAttribute";
                    enumMode = 7;

                    foreach (var attr in symbol.ContainingType.GetAttributes())
                    {
                        if (attr.AttributeClass != null && attr.AttributeClass.FullyQualifiedName() == enumAttr && attr.ConstructorArguments.Any())
                        {
                            enumMode = (int)attr.ConstructorArguments.First().Value;
                            break;
                        }
                    }
                }

                var nameAttr = GetInheritedAttribute(symbol, Translator.H5_ASSEMBLY + ".NameAttribute");
                bool isIgnore = symbol.ContainingType != null && IsExternalType(symbol.ContainingType);

                name = symbol.Name;

                if (nameAttr != null)
                {
                    var value = nameAttr.ConstructorArguments.First().Value;
                    if (value is string)
                    {
                        name = value.ToString();
                        name = Helpers.ConvertNameTokens(name, symbol.Name);
                        if (!isIgnore && symbol.IsStatic && Helpers.IsReservedStaticName(name))
                        {
                            name = Helpers.ChangeReservedWord(name);
                        }
                        return name;
                    }

                    //preserveMemberChange = !(bool)value;
                    enumMode = -1;
                }

                if (enumMode > 6)
                {
                    switch (enumMode)
                    {
                        case 7:
                            break;

                        case 8:
                            name = name.ToLowerInvariant();
                            break;

                        case 9:
                            name = name.ToUpperInvariant();
                            break;
                    }
                }
                /*else
                {
                    name = !preserveMemberChange ? Object.Net.Utilities.StringUtils.ToLowerCamelCase(name) : name;
                }*/

                if (!isIgnore && symbol.IsStatic && Helpers.IsReservedStaticName(name))
                {
                    name = Helpers.ChangeReservedWord(name);
                }
            }

            return name;
        }

        private static bool IsExternalType(INamedTypeSymbol symbol)
        {
            string externalAttr = Translator.H5_ASSEMBLY + ".ExternalAttribute";
            string virtualAttr = Translator.H5_ASSEMBLY + ".VirtualAttribute";
            string objectLiteralAttr = Translator.H5_ASSEMBLY + ".ObjectLiteralAttribute";

            var result = HasAttribute(symbol.GetAttributes(), externalAttr)
                   || HasAttribute(symbol.GetAttributes(), objectLiteralAttr)
                   || HasAttribute(symbol.ContainingAssembly.GetAttributes(), externalAttr);

            if (result)
            {
                return true;
            }

            var attr = symbol.GetAttributes().FirstOrDefault(a => a.AttributeClass != null && a.AttributeClass.FullyQualifiedName() == virtualAttr);

            if (attr == null)
            {
                if (Validator.IsTypeFromH5Core(symbol.ContainingAssembly.Name)) return true;

                attr = symbol.ContainingAssembly.GetAttributes().FirstOrDefault(a => a.AttributeClass != null && a.AttributeClass.FullyQualifiedName() == virtualAttr);
            }

            if (attr != null)
            {
                if (attr.ConstructorArguments.Length == 0)
                {
                    return true;
                }

                var value = (int)attr.ConstructorArguments[0].Value;

                switch (value)
                {
                    case 0:
                        return true;
                    case 1:
                        return symbol.TypeKind != TypeKind.Interface;
                    case 2:
                        return symbol.TypeKind == TypeKind.Interface;
                }
            }

            return false;
        }

        private static bool HasAttribute(ImmutableArray<AttributeData> attributes, string attrName)
        {
            foreach (var attr in attributes)
            {
                if (attr.AttributeClass != null && attr.AttributeClass.FullyQualifiedName() == attrName)
                {
                    return true;
                }
            }

            return false;
        }

        private static AttributeData GetInheritedAttribute(ISymbol symbol, string attrName)
        {
            foreach (var attr in symbol.GetAttributes())
            {
                if (attr.AttributeClass.FullyQualifiedName() == attrName)
                {
                    return attr;
                }
            }

            if (symbol is IMethodSymbol method && method.OverriddenMethod != null)
            {
                return GetInheritedAttribute(method.OverriddenMethod, attrName);
            }

            if (symbol is IPropertySymbol property && property.OverriddenProperty != null)
            {
                return GetInheritedAttribute(property.OverriddenProperty, attrName);
            }

            return null;
        }

        public static bool IsExpressionOfT(this ITypeSymbol type)
        {
            return type is INamedTypeSymbol && type.OriginalDefinition.MetadataName == typeof(System.Linq.Expressions.Expression<>).Name && type.ContainingNamespace.FullyQualifiedName() == typeof(System.Linq.Expressions.Expression<>).Namespace;
        }

        public static IEnumerable<ITypeSymbol> GetBaseTypesAndThis(this ITypeSymbol type)
        {
            var current = type;
            while (current != null)
            {
                yield return current;
                current = current.BaseType;
            }
        }

        public static bool InheritsFromOrEquals(this ITypeSymbol type, ITypeSymbol baseType)
        {
            return type.GetBaseTypesAndThis().Contains(baseType);
        }

        public static T GetParent<T>(this SyntaxNode node, Type stop = null) where T : SyntaxNode
        {
            var p = node.Parent;
            while (p != null && !(p is T) && (stop == null || stop.IsAssignableFrom(p.GetType())))
            {
                p = p.Parent;
            }

            return p as T;
        }

        public static SyntaxTriviaList ExcludeDirectivies(this SyntaxTriviaList list)
        {
            return SyntaxFactory.TriviaList(list.Where(t => !t.IsDirective));
        }

        public static bool IsAccessibleIn(this ITypeSymbol type, ITypeSymbol currentType)
        {
            var list = new List<ITypeSymbol>();

            while (currentType != null && currentType.ContainingType != null)
            {
                var nested = currentType.GetTypeMembers();

                if (nested != null && nested.Length > 0)
                {
                    list.AddRange(nested);
                }

                list.Add(currentType.ContainingType);
                currentType = currentType.ContainingType;
            }

            if (currentType != null)
            {
                var nested1 = currentType.GetTypeMembers();

                if (nested1 != null && nested1.Length > 0)
                {
                    list.AddRange(nested1);
                }
            }

            return list.Contains(type);
        }
    }
}