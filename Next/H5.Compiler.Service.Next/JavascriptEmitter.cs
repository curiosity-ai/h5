using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace H5.Compiler.Service.Next
{
    public class JavascriptEmitter : CSharpSyntaxWalker
    {
        private readonly CSharpCompilation _compilation;
        private readonly StringBuilder _sb;
        private SemanticModel? _semanticModel;

        public JavascriptEmitter(CSharpCompilation compilation)
        {
            _compilation = compilation;
            _sb = new StringBuilder();
        }

        public string Emit()
        {
            _sb.Clear();
            foreach (var tree in _compilation.SyntaxTrees)
            {
                _semanticModel = _compilation.GetSemanticModel(tree);
                Visit(tree.GetRoot());
            }

            return _sb.ToString();
        }

        public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
        {
            foreach (var member in node.Members)
            {
                Visit(member);
            }
        }

        public override void VisitFileScopedNamespaceDeclaration(FileScopedNamespaceDeclarationSyntax node)
        {
            foreach (var member in node.Members)
            {
                Visit(member);
            }
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            EmitTypeDeclaration(node);
        }

        public override void VisitStructDeclaration(StructDeclarationSyntax node)
        {
            EmitTypeDeclaration(node);
        }

        private void EmitTypeDeclaration(TypeDeclarationSyntax node)
        {
            if (_semanticModel == null) return;
            var symbol = _semanticModel.GetDeclaredSymbol(node) as INamedTypeSymbol;
            if (symbol == null) return;

            string className = symbol.Name;
            string namespaceName = symbol.ContainingNamespace?.ToString() ?? "";
            string fullTypeName = string.IsNullOrEmpty(namespaceName) || namespaceName == "<global namespace>" ? className : $"{namespaceName}.{className}";

            bool isGeneric = node.TypeParameterList != null && node.TypeParameterList.Parameters.Any();
            if (isGeneric)
            {
                var typeParams = string.Join(", ", node.TypeParameterList!.Parameters.Select(p => p.Identifier.Text));
                _sb.AppendLine($"H5.define('{fullTypeName}', function ({typeParams}) {{ return {{");
            }
            else
            {
                _sb.AppendLine($"H5.define('{fullTypeName}', {{");
            }

            var members = node.Members;
            var fields = members.OfType<FieldDeclarationSyntax>().ToList();
            var properties = members.OfType<PropertyDeclarationSyntax>().ToList();
            var methods = members.OfType<MethodDeclarationSyntax>().ToList();
            var constructors = members.OfType<ConstructorDeclarationSyntax>().ToList();

            if (node is StructDeclarationSyntax)
            {
                _sb.AppendLine($"    $kind: \"struct\",");
            }

            var staticFields = fields.Where(f => f.Modifiers.Any(SyntaxKind.StaticKeyword)).ToList();
            var staticProps = properties.Where(p => p.Modifiers.Any(SyntaxKind.StaticKeyword)).ToList();
            var staticMethods = methods.Where(m => m.Modifiers.Any(SyntaxKind.StaticKeyword)).ToList();

            if (staticFields.Any() || staticProps.Any() || staticMethods.Any())
            {
                _sb.AppendLine("    statics: {");

                if (staticFields.Any())
                {
                    _sb.AppendLine("        fields: {");
                    foreach (var field in staticFields) EmitField(field);
                    _sb.AppendLine("        },");
                }

                if (staticProps.Any())
                {
                     _sb.AppendLine("        props: {");
                     foreach (var prop in staticProps) EmitProperty(prop);
                     _sb.AppendLine("        },");
                }

                if (staticMethods.Any())
                {
                    _sb.AppendLine("        methods: {");
                    foreach (var method in staticMethods) EmitMethod(method);
                    _sb.AppendLine("        }");
                }

                _sb.AppendLine("    },");
            }

            var instanceFields = fields.Where(f => !f.Modifiers.Any(SyntaxKind.StaticKeyword)).ToList();
            var instanceProps = properties.Where(p => !p.Modifiers.Any(SyntaxKind.StaticKeyword)).ToList();
            var instanceMethods = methods.Where(m => !m.Modifiers.Any(SyntaxKind.StaticKeyword)).ToList();

            if (instanceFields.Any())
            {
                _sb.AppendLine("    fields: {");
                foreach (var field in instanceFields) EmitField(field);
                _sb.AppendLine("    },");
            }

            if (instanceProps.Any())
            {
                _sb.AppendLine("    props: {");
                foreach (var prop in instanceProps) EmitProperty(prop);
                _sb.AppendLine("    },");
            }

            if (constructors.Any())
            {
                _sb.AppendLine("    ctors: {");
                foreach (var ctor in constructors) EmitConstructor(ctor);
                _sb.AppendLine("    },");
            }

            if (instanceMethods.Any())
            {
                _sb.AppendLine("    methods: {");
                foreach (var method in instanceMethods) EmitMethod(method);
                _sb.AppendLine("    }");
            }

            if (isGeneric)
            {
                _sb.AppendLine("}; });");
            }
            else
            {
                _sb.AppendLine("});");
            }
        }

        private void EmitField(FieldDeclarationSyntax node)
        {
            foreach (var variable in node.Declaration.Variables)
            {
                string fieldName = variable.Identifier.Text;
                _sb.Append($"            {fieldName}: ");
                if (variable.Initializer != null)
                {
                    Visit(variable.Initializer.Value);
                }
                else
                {
                    _sb.Append("null");
                }
                _sb.AppendLine(",");
            }
        }

        private void EmitProperty(PropertyDeclarationSyntax node)
        {
            string propName = node.Identifier.Text;
            _sb.AppendLine($"            {propName}: null, // Simplified property backing");
        }

        private void EmitConstructor(ConstructorDeclarationSyntax node)
        {
            var parameters = string.Join(", ", node.ParameterList.Parameters.Select(p => p.Identifier.Text));

            _sb.AppendLine($"        init: function ({parameters}) {{");
            if (node.Body != null)
            {
                Visit(node.Body);
            }
            _sb.AppendLine($"        }},");
        }

        private void EmitMethod(MethodDeclarationSyntax node)
        {
            if (_semanticModel == null) return;
            var symbol = _semanticModel.GetDeclaredSymbol(node);
            if (symbol == null) return;

            string methodName = symbol.Name;
            var parameters = string.Join(", ", node.ParameterList.Parameters.Select(p => p.Identifier.Text));

            _sb.AppendLine($"            {methodName}: function ({parameters}) {{");
            if (node.Body != null)
            {
                Visit(node.Body);
            }
            else if (node.ExpressionBody != null)
            {
                 _sb.Append("                return ");
                 Visit(node.ExpressionBody.Expression);
                 _sb.AppendLine(";");
            }
            _sb.AppendLine($"            }},");
        }

        public override void VisitReturnStatement(ReturnStatementSyntax node)
        {
            _sb.Append("return ");
            if (node.Expression != null)
            {
                Visit(node.Expression);
            }
            _sb.Append(";");
        }

        public override void VisitExpressionStatement(ExpressionStatementSyntax node)
        {
            Visit(node.Expression);
            _sb.Append(";");
        }

        public override void VisitBinaryExpression(BinaryExpressionSyntax node)
        {
            Visit(node.Left);
            _sb.Append($" {node.OperatorToken.Text} ");
            Visit(node.Right);
        }

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            if (_semanticModel == null) return;
            var symbol = _semanticModel.GetSymbolInfo(node).Symbol as IMethodSymbol;
            if (node.Expression is MemberAccessExpressionSyntax memberAccess &&
                memberAccess.Expression is IdentifierNameSyntax id && id.Identifier.Text == "Console" &&
                memberAccess.Name.Identifier.Text == "WriteLine")
            {
                _sb.Append("console.log(");
            }
            else
            {
                 Visit(node.Expression);
                 _sb.Append("(");
            }

            bool first = true;
            foreach (var arg in node.ArgumentList.Arguments)
            {
                if (!first) _sb.Append(", ");
                Visit(arg.Expression);
                first = false;
            }
            _sb.Append(")");
        }

        public override void VisitAssignmentExpression(AssignmentExpressionSyntax node)
        {
             Visit(node.Left);
             _sb.Append(" = ");
             Visit(node.Right);
        }

        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            if (_semanticModel == null) return;
            var symbol = _semanticModel.GetSymbolInfo(node).Symbol;
            if (symbol is IPropertySymbol || symbol is IFieldSymbol)
            {
                if (!symbol.IsStatic && symbol.ContainingType != null)
                {
                    // Only prefix with this. if it is not part of a member access expression
                    // e.g. other.Count should not become other.this.Count
                    if (!(node.Parent is MemberAccessExpressionSyntax memberAccess && memberAccess.Name == node))
                    {
                        _sb.Append("this.");
                    }
                }
            }
            _sb.Append(node.Identifier.Text);
        }

        public override void VisitLiteralExpression(LiteralExpressionSyntax node)
        {
            if (node.IsKind(SyntaxKind.StringLiteralExpression))
            {
                _sb.Append($"'{node.Token.ValueText}'");
            }
            else
            {
                 _sb.Append(node.Token.Text);
            }
        }

        public override void VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            _sb.Append("var ");
            bool first = true;
            foreach (var variable in node.Declaration.Variables)
            {
                if (!first) _sb.Append(", ");
                _sb.Append(variable.Identifier.Text);
                if (variable.Initializer != null)
                {
                    _sb.Append(" = ");
                    Visit(variable.Initializer.Value);
                }
                first = false;
            }
            _sb.Append(";");
        }

        public override void VisitIfStatement(IfStatementSyntax node)
        {
            _sb.Append("if (");
            Visit(node.Condition);
            _sb.Append(") ");
            Visit(node.Statement);

            if (node.Else != null)
            {
                _sb.Append(" else ");
                Visit(node.Else.Statement);
            }
        }

        public override void VisitBlock(BlockSyntax node)
        {
            _sb.AppendLine("{");
            foreach (var statement in node.Statements)
            {
                _sb.Append("                ");
                Visit(statement);
                _sb.AppendLine();
            }
            _sb.Append("            }");
        }

        public override void VisitForStatement(ForStatementSyntax node)
        {
            _sb.Append("for (");
            if (node.Declaration != null)
            {
                _sb.Append("var ");
                bool first = true;
                foreach (var variable in node.Declaration.Variables)
                {
                    if (!first) _sb.Append(", ");
                    _sb.Append(variable.Identifier.Text);
                    if (variable.Initializer != null)
                    {
                        _sb.Append(" = ");
                        Visit(variable.Initializer.Value);
                    }
                    first = false;
                }
            }
            else
            {
                bool first = true;
                foreach (var initializer in node.Initializers)
                {
                    if (!first) _sb.Append(", ");
                    Visit(initializer);
                    first = false;
                }
            }
            _sb.Append("; ");

            if (node.Condition != null)
            {
                Visit(node.Condition);
            }
            _sb.Append("; ");

            bool firstInc = true;
            foreach (var incrementor in node.Incrementors)
            {
                if (!firstInc) _sb.Append(", ");
                Visit(incrementor);
                firstInc = false;
            }
            _sb.Append(") ");

            Visit(node.Statement);
        }

        public override void VisitWhileStatement(WhileStatementSyntax node)
        {
            _sb.Append("while (");
            Visit(node.Condition);
            _sb.Append(") ");
            Visit(node.Statement);
        }
        public override void VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            Visit(node.Expression);
            _sb.Append(".");
            Visit(node.Name);
        }
    }
}
