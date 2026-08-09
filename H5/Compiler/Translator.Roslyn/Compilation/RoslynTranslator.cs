using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace H5.Translator.Roslyn;

/// <summary>
/// Public entry point for the Roslyn-only C# → JavaScript translator.
///
/// Pipeline: parse + compile (Roslyn) → surface errors → scan for browser-incompatible
/// features → walk syntax tree guided by the semantic model → emit JavaScript.
/// </summary>
public sealed class RoslynTranslator
{
    /// <summary>
    /// Roslyn errors that are artifacts of compiling against the H5 BCL (which targets a
    /// runtime feature-set narrower than the language) but are harmless when the output is
    /// untyped JavaScript. CS8830: covariant return types in overrides — no runtime type
    /// check exists in JS, so the override simply works.
    /// </summary>
    private static readonly HashSet<string> BenignForJs = new()
    {
        "CS8830", // covariant return types in overrides — no runtime type check in JS
        "CS5001", // no static Main — library-style snippets simply emit no entry point
        // async ValueTask: H5.dll's ValueTask lacks the async method-builder attribute, so
        // Roslyn (against the H5 BCL) rejects it as a task-like return type and then reports
        // a missing return. We emit ValueTask exactly like Task (a Promise → h5.js Task), so
        // both are harmless. (The native comparison compiles against the real BCL, unaffected.)
        "CS1983", // return type of an async method must be void/Task/task-like…
        "CS0161", // not all code paths return a value (fallout of the above; JS returns undefined)
        "CS4032", // 'await' in a method with a non-task-like return (same ValueTask fallout)
    };

    /// <summary>Translate a single source file.</summary>
    public TranslationResult Translate(string source, string path = "App.cs", string assemblyName = CompilationBuilder.DefaultAssemblyName)
        => Translate(new[] { (path, source) }, assemblyName);

    /// <summary>Translate multiple source files into a single JS bundle.</summary>
    public TranslationResult Translate(IEnumerable<(string path, string text)> sources, string assemblyName = CompilationBuilder.DefaultAssemblyName)
        => Translate(sources, assemblyName, null);

    /// <summary>
    /// Translate multiple source files into a single JS bundle, referencing extra assemblies
    /// (e.g. h5.core, h5.Newtonsoft.Json) alongside H5.dll — used when compiling a real project.
    /// </summary>
    public TranslationResult Translate(
        IEnumerable<(string path, string text)> sources,
        string assemblyName,
        IEnumerable<string>? extraReferencePaths,
        IEnumerable<string>? preprocessorSymbols = null,
        LanguageVersion languageVersion = LanguageVersion.Latest,
        bool reflectionEnabled = true,
        MetadataTarget metadataTarget = MetadataTarget.Inline)
    {
        var r = BuildAssembly(sources, assemblyName, extraReferencePaths, preprocessorSymbols,
            languageVersion, reflectionEnabled, metadataTarget, emitAssembly: false);
        return new TranslationResult(r.Javascript, r.Diagnostics, r.MetadataJavascript);
    }

    /// <summary>
    /// Compiles a project as a distributable assembly: builds the Roslyn compilation once, then
    /// (optionally) emits the real .NET DLL AND translates the sources to JavaScript. This mirrors
    /// the existing compiler, where <c>h5</c> both produces the assembly and the JS that later
    /// gets embedded into it — so the assembly can be referenced by another project which extracts
    /// the JS back out.
    /// </summary>
    public AssemblyBuildResult BuildAssembly(
        IEnumerable<(string path, string text)> sources,
        string assemblyName,
        IEnumerable<string>? extraReferencePaths,
        IEnumerable<string>? preprocessorSymbols = null,
        LanguageVersion languageVersion = LanguageVersion.Latest,
        bool reflectionEnabled = true,
        MetadataTarget metadataTarget = MetadataTarget.Inline,
        bool emitAssembly = true)
    {
        var compilation = CompilationBuilder.Build(
            sources, assemblyName, languageVersion,
            extraReferencePaths: extraReferencePaths,
            preprocessorSymbols: preprocessorSymbols);

        var diagnostics = new List<Diagnostic>();
        diagnostics.AddRange(compilation.GetDiagnostics()
            .Where(d => d.Severity == DiagnosticSeverity.Error && !BenignForJs.Contains(d.Id)));
        if (diagnostics.Count > 0)
            return new AssemblyBuildResult(null, null, null, diagnostics);

        var unsupported = UnsupportedFeatureScanner.Scan(compilation);
        diagnostics.AddRange(unsupported);
        if (unsupported.Any(d => d.Severity == DiagnosticSeverity.Error))
            return new AssemblyBuildResult(null, null, null, diagnostics);

        // Emit the real .NET assembly (as a library) so referencing projects can bind to its
        // types. Emitted before translation so an emit failure is reported like any other error.
        byte[]? assemblyBytes = null;
        if (emitAssembly)
        {
            var asmCompilation = compilation.WithOptions(
                compilation.Options.WithOutputKind(OutputKind.DynamicallyLinkedLibrary));
            using var ms = new MemoryStream();
            // Include private members so a referencing project sees the full member set — the
            // overload numbering (e.g. $ctorN) must match what this assembly emits for itself,
            // and that numbering counts private overloads too.
            var emit = asmCompilation.Emit(ms, options: new Microsoft.CodeAnalysis.Emit.EmitOptions(
                metadataOnly: false,
                includePrivateMembers: true,
                debugInformationFormat: Microsoft.CodeAnalysis.Emit.DebugInformationFormat.Embedded));
            if (!emit.Success)
            {
                diagnostics.AddRange(emit.Diagnostics
                    .Where(d => d.Severity == DiagnosticSeverity.Error && !BenignForJs.Contains(d.Id)));
                return new AssemblyBuildResult(null, null, null, diagnostics);
            }
            assemblyBytes = ms.ToArray();
        }

        try
        {
            var emitter = new Emitter(compilation, assemblyName)
            {
                ReflectionEnabled = reflectionEnabled,
                MetadataTarget = metadataTarget,
            };
            var js = emitter.Emit();
            return new AssemblyBuildResult(js, emitter.MetadataScript, assemblyBytes, diagnostics);
        }
        catch (TranslationException ex)
        {
            diagnostics.Add(Diagnostics.Create(Diagnostics.Unsupported, ex.Location, ex.Message));
            return new AssemblyBuildResult(null, null, null, diagnostics);
        }
    }

    /// <summary>
    /// Convenience: translate and throw with a readable message if anything failed.
    /// Mirrors the behavior tests expect (compilation failures throw).
    /// </summary>
    public string TranslateOrThrow(string source, string path = "App.cs")
    {
        var result = Translate(source, path);
        if (!result.Success)
        {
            var messages = string.Join("\n", result.Errors.Select(d => d.GetMessage()));
            throw new TranslationException(messages.Length > 0 ? messages : "Translation failed.");
        }
        return result.Javascript!;
    }

    private static string? _shim;

    /// <summary>
    /// The full runtime prelude: the real h5.js followed by the thin H5R shim that
    /// adapts the emitter's language-level helpers onto h5.js primitives.
    /// </summary>
    public static string LoadRuntime()
    {
        return H5Assemblies.RuntimeJs + "\n" + RuntimeShim;
    }

    /// <summary>The thin H5R shim (the emitter's language-level helpers over h5.js primitives),
    /// loaded once. A site build ships this as its own script after h5.js and before the bundle.</summary>
    public static string RuntimeShim => _shim ??= ReadShim();

    private static string ReadShim()
    {
        var asm = typeof(RoslynTranslator).Assembly;
        var name = asm.GetManifestResourceNames().First(n => n.EndsWith("h5roslyn.shim.js", StringComparison.Ordinal));
        using var stream = asm.GetManifestResourceStream(name)!;
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
