using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace H5.Translator.Roslyn;

/// <summary>
/// Builds a <see cref="CSharpCompilation"/> from source, wiring up the reference
/// assemblies so the semantic model can bind the BCL. Targets C# Latest.
/// </summary>
public static class CompilationBuilder
{
    public const string DefaultAssemblyName = "App";

    public static CSharpCompilation Build(
        IEnumerable<(string path, string text)> sources,
        string assemblyName = DefaultAssemblyName,
        LanguageVersion languageVersion = LanguageVersion.Latest,
        IEnumerable<string>? extraReferencePaths = null,
        IEnumerable<string>? preprocessorSymbols = null)
    {
        var parseOptions = new CSharpParseOptions(languageVersion)
            .WithFeatures(new[] { new KeyValuePair<string, string>("strict", "false") });
        if (preprocessorSymbols is not null)
            parseOptions = parseOptions.WithPreprocessorSymbols(preprocessorSymbols);

        // Give each source text an explicit encoding: emitting the assembly with embedded debug
        // information (as the package build does) requires it — Roslyn otherwise reports CS8055
        // ("Cannot emit debug information for a source text without encoding").
        var trees = sources
            .Select(s => CSharpSyntaxTree.ParseText(
                Microsoft.CodeAnalysis.Text.SourceText.From(s.text, System.Text.Encoding.UTF8),
                parseOptions, path: s.path))
            .ToList();

        // Nullable reference types only exist from C# 8; enabling the annotations context
        // under an earlier language version (e.g. a project pinned to 7.2) is a hard error.
        var effectiveLang = languageVersion == LanguageVersion.Latest ? LanguageVersion.Latest : languageVersion;
        var nullable = effectiveLang != LanguageVersion.Latest && effectiveLang < LanguageVersion.CSharp8
            ? NullableContextOptions.Disable
            : NullableContextOptions.Annotations;

        var options = new CSharpCompilationOptions(
            OutputKind.ConsoleApplication,
            optimizationLevel: OptimizationLevel.Debug,
            allowUnsafe: true, // allowed by the compiler so we can detect+report it ourselves
            nullableContextOptions: nullable,
            // Import non-public members of referenced assemblies. A referenced H5-compiled package
            // (built with --emit-package) numbers its overloads (e.g. $ctorN) over its FULL member
            // set including private ones; the consumer must see the same set for its call sites to
            // resolve to the same JS names.
            metadataImportOptions: MetadataImportOptions.All);

        return CSharpCompilation.Create(
            assemblyName,
            syntaxTrees: trees,
            references: GetReferenceAssemblies(extraReferencePaths),
            options: options);
    }

    /// <summary>
    /// References the H5 assembly (H5.dll) as the sole BCL, exactly like the H5
    /// compiler. H5.dll redefines System.* with the [External]/[Name]/[Template]
    /// attributes that drive JavaScript emission and interop with the h5.js runtime.
    /// Any <paramref name="extraReferencePaths"/> (e.g. h5.core, h5.Newtonsoft.Json for a
    /// real project) are added alongside it.
    /// </summary>
    private static IReadOnlyList<MetadataReference> GetReferenceAssemblies(IEnumerable<string>? extraReferencePaths)
    {
        var refs = new List<MetadataReference> { MetadataReference.CreateFromFile(H5Assemblies.H5DllPath) };
        if (extraReferencePaths is not null)
        {
            var h5Dll = Path.GetFullPath(H5Assemblies.H5DllPath);
            foreach (var path in extraReferencePaths)
            {
                if (string.IsNullOrWhiteSpace(path) || !File.Exists(path)) continue;
                if (string.Equals(Path.GetFullPath(path), h5Dll, StringComparison.OrdinalIgnoreCase)) continue;
                refs.Add(MetadataReference.CreateFromFile(path));
            }
        }
        return refs;
    }
}
