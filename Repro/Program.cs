using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

class TestRewriter : CSharpSyntaxRewriter
{
    public override SyntaxNode VisitIdentifierName(IdentifierNameSyntax node)
    {
        if (node.Identifier.IsKind(SyntaxKind.GlobalKeyword))
        {
             // Simulate hypothetical SharpSixRewriter logic that reconstructs identifier
             return SyntaxFactory.IdentifierName("global");
        }
        return base.VisitIdentifierName(node);
    }
}

class Program
{
    static void Main()
    {
        var node = SyntaxFactory.AliasQualifiedName(
            SyntaxFactory.IdentifierName(SyntaxFactory.Token(SyntaxKind.GlobalKeyword)),
            SyntaxFactory.IdentifierName("Foo")
        );

        Console.WriteLine($"Original: {node}");

        var rewriter = new TestRewriter();
        var result = rewriter.Visit(node);

        Console.WriteLine($"Result: {result}");
    }
}
