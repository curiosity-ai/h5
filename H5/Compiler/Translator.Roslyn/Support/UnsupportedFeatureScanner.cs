using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace H5.Translator.Roslyn;

/// <summary>
/// Walks the syntax trees and reports language features that cannot run in a
/// browser environment as compilation errors (H5R0001). Detection is by syntax
/// for language constructs and by bound symbol namespace for runtime APIs.
/// </summary>
internal sealed class UnsupportedFeatureScanner : CSharpSyntaxWalker
{
    private readonly SemanticModel _model;
    private readonly List<Diagnostic> _diagnostics;

    private UnsupportedFeatureScanner(SemanticModel model, List<Diagnostic> diagnostics)
    {
        _model = model;
        _diagnostics = diagnostics;
    }

    public static IReadOnlyList<Diagnostic> Scan(CSharpCompilation compilation)
    {
        var diagnostics = new List<Diagnostic>();
        foreach (var tree in compilation.SyntaxTrees)
        {
            var model = compilation.GetSemanticModel(tree);
            var scanner = new UnsupportedFeatureScanner(model, diagnostics);
            scanner.Visit(tree.GetRoot());
        }
        return diagnostics;
    }

    private void Report(SyntaxNode node, string message)
        => _diagnostics.Add(Diagnostics.Create(Diagnostics.Unsupported, node.GetLocation(), message));

    // ---- Pointers ----------------------------------------------------------

    public override void VisitPointerType(PointerTypeSyntax node)
    {
        Report(node, "Pointers are not supported in the browser environment.");
        base.VisitPointerType(node);
    }

    public override void VisitGlobalStatement(GlobalStatementSyntax node)
    {
        Report(node, "Top-level statements are not supported; use an explicit class with a Main method.");
        base.VisitGlobalStatement(node);
    }

    public override void VisitUsingDirective(UsingDirectiveSyntax node)
    {
        if (node.GlobalKeyword.RawKind != 0)
            Report(node, "Global usings are not supported; add per-file using directives instead.");
        base.VisitUsingDirective(node);
    }

    public override void VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax node)
    {
        if (node.IsKind(SyntaxKind.AddressOfExpression) || node.IsKind(SyntaxKind.PointerIndirectionExpression))
        {
            Report(node, "Pointers are not supported in the browser environment.");
        }
        base.VisitPrefixUnaryExpression(node);
    }

    // ---- unsafe / fixed ----------------------------------------------------

    public override void VisitUnsafeStatement(UnsafeStatementSyntax node)
    {
        Report(node, "Unsafe code is not supported in the browser environment.");
        base.VisitUnsafeStatement(node);
    }

    public override void VisitFixedStatement(FixedStatementSyntax node)
    {
        Report(node, "Unsafe code is not supported in the browser environment.");
        base.VisitFixedStatement(node);
    }

    public override void VisitStackAllocArrayCreationExpression(StackAllocArrayCreationExpressionSyntax node)
    {
        Report(node, "stackalloc is not supported in the browser environment.");
        base.VisitStackAllocArrayCreationExpression(node);
    }

    private void CheckUnsafeModifier(SyntaxTokenList modifiers, SyntaxNode node)
    {
        if (modifiers.Any(SyntaxKind.UnsafeKeyword))
        {
            Report(node, "Unsafe code is not supported in the browser environment.");
        }
    }

    /// <summary>An `extern` member is only unsupported when it is real native interop —
    /// not when it carries an H5 codegen attribute ([Template]/[Name]/[External]/[Script])
    /// that maps it to JavaScript.</summary>
    private void CheckExtern(SyntaxTokenList modifiers, SyntaxList<AttributeListSyntax> attributes, SyntaxNode node)
    {
        if (!modifiers.Any(SyntaxKind.ExternKeyword)) return;
        var jsMapped = attributes.SelectMany(a => a.Attributes).Any(a =>
        {
            var n = a.Name.ToString();
            return n is "Template" or "Name" or "External" or "Script"
                or "H5.Template" or "H5.Name" or "H5.External" or "H5.Script"
                or "TemplateAttribute" or "NameAttribute" or "ExternalAttribute" or "ScriptAttribute";
        });
        if (!jsMapped)
            Report(node, "Native interop (extern methods) is not supported in the browser environment.");
    }

    public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        CheckUnsafeModifier(node.Modifiers, node);
        CheckExtern(node.Modifiers, node.AttributeLists, node);
        CheckDllImport(node);
        base.VisitMethodDeclaration(node);
    }

    public override void VisitClassDeclaration(ClassDeclarationSyntax node)
    {
        CheckUnsafeModifier(node.Modifiers, node);
        base.VisitClassDeclaration(node);
    }

    public override void VisitStructDeclaration(StructDeclarationSyntax node)
    {
        CheckUnsafeModifier(node.Modifiers, node);
        base.VisitStructDeclaration(node);
    }

    public override void VisitFieldDeclaration(FieldDeclarationSyntax node)
    {
        CheckUnsafeModifier(node.Modifiers, node);
        base.VisitFieldDeclaration(node);
    }

    private void CheckDllImport(MethodDeclarationSyntax node)
    {
        foreach (var attr in node.AttributeLists.SelectMany(a => a.Attributes))
        {
            var symbol = _model.GetSymbolInfo(attr).Symbol?.ContainingType;
            var name = symbol?.ToDisplayString();
            if (name is "System.Runtime.InteropServices.DllImportAttribute"
                     or "System.Runtime.InteropServices.LibraryImportAttribute")
            {
                Report(node, "Native interop (P/Invoke) is not supported in the browser environment.");
            }
        }
    }

    // ---- Runtime APIs that cannot exist in the browser ---------------------

    private static readonly (string ns, string message)[] DeniedNamespaces =
    {
        ("System.IO", "File I/O ({0}) is not supported in the browser environment."),
        ("System.Net.Sockets", "Sockets ({0}) are not supported in the browser environment."),
        ("System.Threading", "Threading primitives ({0}) are not supported in the browser environment."),
    };

    // Threading types that are allowed because they are modeled (Task-based async, etc.)
    private static readonly HashSet<string> AllowedThreadingTypes = new()
    {
        "System.Threading.Tasks.Task",
        "System.Threading.Tasks.Task`1",
        "System.Threading.Tasks.ValueTask",
        "System.Threading.Tasks.ValueTask`1",
        "System.Threading.Tasks.TaskCompletionSource",
        "System.Threading.Tasks.TaskCompletionSource`1",
        "System.Threading.CancellationToken",
        "System.Threading.CancellationTokenSource",
    };

    public override void VisitIdentifierName(IdentifierNameSyntax node)
    {
        var symbol = _model.GetSymbolInfo(node).Symbol;
        var type = symbol as INamedTypeSymbol ?? symbol?.ContainingType;
        if (type is not null)
        {
            CheckDeniedType(type, node);
        }
        base.VisitIdentifierName(node);
    }

    private readonly HashSet<Location> _reportedApiLocations = new();

    private void CheckDeniedType(INamedTypeSymbol type, SyntaxNode node)
    {
        var full = type.ConstructedFrom.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
            .Replace("global::", string.Empty);

        var metadataName = type.ConstructUnboundGenericTypeSafeName();
        if (AllowedThreadingTypes.Contains(metadataName)) return;

        var ns = type.ContainingNamespace?.ToDisplayString() ?? string.Empty;
        foreach (var (deniedNs, message) in DeniedNamespaces)
        {
            if (ns == deniedNs || ns.StartsWith(deniedNs + ".", System.StringComparison.Ordinal))
            {
                if (_reportedApiLocations.Add(node.GetLocation()))
                {
                    Report(node, string.Format(message, full));
                }
                return;
            }
        }
    }
}

internal static class SymbolExtensions
{
    /// <summary>Metadata-style name including arity marker, e.g. Task`1.</summary>
    public static string ConstructUnboundGenericTypeSafeName(this INamedTypeSymbol type)
    {
        var ns = type.ContainingNamespace?.ToDisplayString();
        var name = type.MetadataName; // already includes `1 arity
        return string.IsNullOrEmpty(ns) ? name : ns + "." + name;
    }
}
