using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace H5.Translator.Roslyn.Tests;

/// <summary>
/// Compiles and runs C# natively (Roslyn → in-memory assembly → invoke entry point),
/// capturing Console output. This produces the "expected" output that the emitted
/// JavaScript is diffed against.
/// </summary>
public static class RoslynNativeRunner
{
    public static string CompileAndRun(string source)
    {
        var tree = CSharpSyntaxTree.ParseText(source, new CSharpParseOptions(LanguageVersion.Latest));

        var refs = ((AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES") as string) ?? "")
            .Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries)
            .Where(p => p.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) && File.Exists(p))
            .Select(p => (MetadataReference)MetadataReference.CreateFromFile(p))
            .ToList();

        var compilation = CSharpCompilation.Create(
            "NativeRun_" + Guid.NewGuid().ToString("N"),
            new[] { tree },
            refs,
            new CSharpCompilationOptions(OutputKind.ConsoleApplication, optimizationLevel: OptimizationLevel.Debug));

        using var peStream = new MemoryStream();
        var result = compilation.Emit(peStream);
        if (!result.Success)
        {
            var errors = string.Join("\n", result.Diagnostics
                .Where(d => d.Severity == DiagnosticSeverity.Error)
                .Select(d => d.ToString()));
            throw new InvalidOperationException("Native compilation failed:\n" + errors);
        }

        peStream.Seek(0, SeekOrigin.Begin);
        var context = new AssemblyLoadContext("NativeRun", isCollectible: true);
        try
        {
            var assembly = context.LoadFromStream(peStream);
            var entry = assembly.EntryPoint
                ?? throw new InvalidOperationException("No entry point found.");

            var sb = new StringBuilder();
            var writer = new StringWriter(sb);
            var previous = Console.Out;
            Console.SetOut(writer);
            try
            {
                var args = entry.GetParameters().Length == 0 ? null : new object[] { Array.Empty<string>() };
                var invokeResult = entry.Invoke(null, args);
                if (invokeResult is System.Threading.Tasks.Task task)
                {
                    task.GetAwaiter().GetResult();
                }
            }
            finally
            {
                Console.SetOut(previous);
                writer.Flush();
            }

            return sb.ToString();
        }
        finally
        {
            context.Unload();
        }
    }
}
