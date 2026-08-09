using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace H5.Translator.Roslyn;

/// <summary>
/// Walks the Roslyn syntax tree (guided by the semantic model) and emits JavaScript
/// in the H5 runtime format (H5.assembly + H5.define), so the output runs against
/// the real h5.js / h5.core runtime.
/// </summary>
public sealed partial class Emitter
{
    private readonly CSharpCompilation _compilation;
    private JsWriter _w = new();
    private readonly NameMangler _names = new();
    private readonly TreeModel _model;
    private readonly string _assemblyName;

    /// <summary>While emitting a primary constructor's own body, its parameters are the JS
    /// function parameters (raw names); elsewhere captured params read from the instance.</summary>
    private bool _inPrimaryCtorBody;

    /// <summary>How many loop bodies (for/foreach/while/do) currently enclose the emission point.
    /// A local declared inside a loop is emitted with `let` (a fresh per-iteration binding) so a
    /// closure created in the loop captures that iteration's value — C# block scoping. Outside a
    /// loop (loop depth 0) locals stay `var` (function scope), which tolerates the same-name
    /// redeclarations across flattened scopes that some code relies on.</summary>
    private int _loopDepth;

    /// <summary>The type whose define body is currently being emitted. Its own type parameters are
    /// the ones actually bound as JS function parameters of the define, so <c>default(T)</c> may
    /// safely reference them via <c>H5.getDefaultValue(T)</c>; a type parameter from an enclosing
    /// type (accessible in C# but not emitted as a parameter here) is not in scope.</summary>
    private INamedTypeSymbol? _currentEmitType;

    /// <summary>
    /// Active goto label-dispatch contexts. When non-empty a statement body is being lowered
    /// into a `for(;;) switch($state)` machine: `goto L` sets the state and continues the loop.
    /// The top entry maps each label name to its case index and names the dispatch loop.
    /// </summary>
    private readonly Stack<(System.Collections.Generic.Dictionary<string, int> labels, string loopLabel, string stateVar)> _gotoContexts = new();

    /// <summary>Whether reflection metadata is emitted at all (h5.json reflection.disabled).</summary>
    public bool ReflectionEnabled { get; set; } = true;

    /// <summary>Where reflection metadata goes — inline in the assembly, or a separate file.</summary>
    public MetadataTarget MetadataTarget { get; set; } = MetadataTarget.Inline;

    /// <summary>Assembly version string emitted into a separate metadata file's header.</summary>
    public string AssemblyVersion { get; set; } = "1.0.0.0";

    /// <summary>When <see cref="MetadataTarget"/> is File/Assembly, the standalone metadata
    /// script (a full H5.assembly wrapper) produced by the last <see cref="Emit"/>; else null.</summary>
    public string? MetadataScript { get; private set; }

    public Emitter(CSharpCompilation compilation, string assemblyName = CompilationBuilder.DefaultAssemblyName)
    {
        _compilation = compilation;
        _assemblyName = assemblyName;
        _model = new TreeModel(compilation);
    }

    public string Emit()
    {
        _w.WriteLine("/**");
        _w.WriteLine(" * H5.Translator.Roslyn generated output.");
        _w.WriteLine(" */");
        var types = CollectTypes();

        // Reflection metadata: either woven into this assembly function (inline target) or
        // collected into a standalone metadata script (file target), never both.
        var inlineMeta = ReflectionEnabled && MetadataTarget is MetadataTarget.Inline or MetadataTarget.Type;
        var fileMeta = ReflectionEnabled && MetadataTarget is MetadataTarget.File or MetadataTarget.Assembly;

        _w.Write($"H5.assembly(\"{_assemblyName}\", function ($asm, globals) ");
        _w.Block(() =>
        {
            _w.WriteLine("\"use strict\";");
            _w.WriteLine();

            foreach (var type in types)
            {
                EmitType(type);
                _w.WriteLine();
            }

            if (inlineMeta) EmitReflectionMetadata(types);
        });
        _w.WriteLine(");");

        MetadataScript = fileMeta ? BuildMetadataFile(types) : null;
        return _w.ToString();
    }

    /// <summary>Runs <paramref name="emit"/> against a temporary writer and returns its text.</summary>
    private string Capture(Action emit)
    {
        var saved = _w;
        _w = new JsWriter();
        try
        {
            emit();
            return _w.ToString();
        }
        finally
        {
            _w = saved;
        }
    }

    private List<INamedTypeSymbol> CollectTypes()
    {
        var declared = new List<INamedTypeSymbol>();
        var seen = new HashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);

        foreach (var tree in _compilation.SyntaxTrees)
        {
            var model = _compilation.GetSemanticModel(tree);
            foreach (var node in tree.GetRoot().DescendantNodes().OfType<BaseTypeDeclarationSyntax>())
            {
                if (model.GetDeclaredSymbol(node) is INamedTypeSymbol sym && seen.Add(sym))
                {
                    declared.Add(sym);
                }
            }
        }

        // Emit each type after every source type it depends on (base class + implemented/
        // extended interfaces), so the runtime's H5.define never sees an undefined reference
        // in `inherits`. Dependency depth gives such an order (the graph is acyclic).
        var depthCache = new Dictionary<INamedTypeSymbol, int>(SymbolEqualityComparer.Default);
        return declared
            .OrderBy(t => DependencyDepth(t, depthCache))
            .ThenBy(t => t.TypeKind == TypeKind.Interface ? 0 : 1)
            .ToList();
    }

    /// <summary>
    /// The longest chain of source-type dependencies (base class and implemented/extended
    /// interfaces) below <paramref name="type"/>. Types with a greater depth are emitted later,
    /// guaranteeing a type's dependencies are defined first. Non-source dependencies live in
    /// the runtime (loaded before the bundle) and contribute no ordering constraint.
    /// </summary>
    private int DependencyDepth(INamedTypeSymbol type, Dictionary<INamedTypeSymbol, int> cache)
    {
        type = (INamedTypeSymbol)type.OriginalDefinition;
        if (cache.TryGetValue(type, out var cached)) return cached;
        cache[type] = 0; // guard against unexpected cycles

        var depth = 0;
        foreach (var dep in Dependencies(type))
            depth = Math.Max(depth, DependencyDepth(dep, cache) + 1);

        cache[type] = depth;
        return depth;
    }

    private static IEnumerable<INamedTypeSymbol> Dependencies(INamedTypeSymbol type)
    {
        // A type's `inherits` names its base class and interfaces *including their generic
        // type arguments* (e.g. LayerHost : ComponentBase<Layer, …> references Layer), and all
        // of those source types must already be defined when the lazy inherits runs.
        if (type.BaseType is { } bt)
            foreach (var d in SourceTypesIn(bt)) yield return d;
        foreach (var iface in type.Interfaces)
            foreach (var d in SourceTypesIn(iface)) yield return d;
    }

    /// <summary>The source named types within a type reference — the type itself and, recursively,
    /// its generic type arguments.</summary>
    private static IEnumerable<INamedTypeSymbol> SourceTypesIn(ITypeSymbol type)
    {
        if (type is not INamedTypeSymbol named) yield break;
        if (named.Locations.Any(l => l.IsInSource))
            yield return (INamedTypeSymbol)named.OriginalDefinition;
        foreach (var arg in named.TypeArguments)
            foreach (var d in SourceTypesIn(arg)) yield return d;
    }

    // ---- helpers -----------------------------------------------------------

    private bool IsTaskType(ITypeSymbol type)
    {
        var name = type.OriginalDefinition.ToDisplayString();
        return name is "System.Threading.Tasks.Task" or "System.Threading.Tasks.Task<TResult>"
            or "System.Threading.Tasks.ValueTask" or "System.Threading.Tasks.ValueTask<TResult>";
    }

    private void Unsupported(SyntaxNode node, string what)
        => throw new TranslationException(
            $"Translation of this construct is not supported yet: {what}", node.GetLocation());
}
