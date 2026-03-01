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
        private SemanticModel _semanticModel;

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

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            var symbol = _semanticModel.GetDeclaredSymbol(node);
            if (symbol == null) return;

            string className = symbol.Name;
            string namespaceName = symbol.ContainingNamespace?.ToString() ?? "";

            // Emit basic H5.define structure (simplified for HelloWorld)
            _sb.AppendLine($"H5.define('{namespaceName}.{className}', {{");

            var methods = node.Members.OfType<MethodDeclarationSyntax>().ToList();
            if (methods.Any(m => m.Modifiers.Any(SyntaxKind.StaticKeyword)))
            {
                _sb.AppendLine("    statics: {");
                _sb.AppendLine("        methods: {");
                foreach (var method in methods.Where(m => m.Modifiers.Any(SyntaxKind.StaticKeyword)))
                {
                    EmitMethod(method);
                }
                _sb.AppendLine("        }");
                _sb.AppendLine("    },");
            }
            // non static methods here...
            _sb.AppendLine("});");
        }

        private void EmitMethod(MethodDeclarationSyntax node)
        {
            var symbol = _semanticModel.GetDeclaredSymbol(node);
            if (symbol == null) return;

            string methodName = symbol.Name;
            // Simplified parameter emission
            var parameters = string.Join(", ", node.ParameterList.Parameters.Select(p => p.Identifier.Text));

            _sb.AppendLine($"            {methodName}: function ({parameters}) {{");
            if (node.Body != null)
            {
                foreach (var statement in node.Body.Statements)
                {
                    _sb.Append("                ");
                    Visit(statement);
                    _sb.AppendLine();
                }
            }
            _sb.AppendLine($"            }}");
        }

        public override void VisitExpressionStatement(ExpressionStatementSyntax node)
        {
            Visit(node.Expression);
            _sb.Append(";");
        }

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            var symbol = _semanticModel.GetSymbolInfo(node).Symbol as IMethodSymbol;
            // A more robust check for Console.WriteLine for the test
            if (node.Expression is MemberAccessExpressionSyntax memberAccess &&
                memberAccess.Expression is IdentifierNameSyntax id && id.Identifier.Text == "Console" &&
                memberAccess.Name.Identifier.Text == "WriteLine")
            {
                _sb.Append("console.log(");
            }
            else
            {
                 // Generic invocation
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
    }
}
