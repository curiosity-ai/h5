using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace H5.Translator.Roslyn;

/// <summary>
/// A semantic-model facade that always queries the correct per-syntax-tree
/// <see cref="SemanticModel"/> for the node being asked about. A Roslyn semantic model is
/// bound to a single syntax tree, but a <c>partial</c> type's members span several files
/// (trees); routing each query by the node's own tree keeps multi-file projects working.
/// Models are cached per tree.
/// </summary>
internal sealed class TreeModel
{
    private readonly Compilation _compilation;
    private readonly Dictionary<SyntaxTree, SemanticModel> _cache = new();

    public TreeModel(Compilation compilation) => _compilation = compilation;

    private SemanticModel For(SyntaxNode node)
    {
        var tree = node.SyntaxTree;
        if (!_cache.TryGetValue(tree, out var model))
        {
            model = _compilation.GetSemanticModel(tree);
            _cache[tree] = model;
        }
        return model;
    }

    public TypeInfo GetTypeInfo(SyntaxNode node) => For(node).GetTypeInfo(node);
    public SymbolInfo GetSymbolInfo(SyntaxNode node) => For(node).GetSymbolInfo(node);
    public ISymbol? GetDeclaredSymbol(SyntaxNode node) => For(node).GetDeclaredSymbol(node);
    public Optional<object?> GetConstantValue(SyntaxNode node) => For(node).GetConstantValue(node);

    public ForEachStatementInfo GetForEachStatementInfo(CommonForEachStatementSyntax node)
        => For(node).GetForEachStatementInfo(node);

    public SymbolInfo GetCollectionInitializerSymbolInfo(ExpressionSyntax node)
        => For(node).GetCollectionInitializerSymbolInfo(node);

    /// <summary>The enclosing symbol at a node's position (the node identifies the tree).</summary>
    public ISymbol? GetEnclosingSymbol(SyntaxNode node)
        => For(node).GetEnclosingSymbol(node.SpanStart);
}
