using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace H5.Compiler.IntegrationTests
{
    public static class RoslynCompiler
    {
        public static string CompileAndRun(string csharpCode)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(csharpCode);

            var references = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic && !string.IsNullOrWhiteSpace(a.Location))
                .Select(a => MetadataReference.CreateFromFile(a.Location))
                .Cast<MetadataReference>()
                .ToList();

            var compilation = CSharpCompilation.Create(
                "RoslynCompiledAssembly",
                new[] { syntaxTree },
                references,
                new CSharpCompilationOptions(OutputKind.ConsoleApplication));

            using var ms = new MemoryStream();
            var result = compilation.Emit(ms);

            if (!result.Success)
            {
                var failures = result.Diagnostics.Where(diagnostic =>
                    diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error);

                var errorMsg = string.Join("\n", failures.Select(f => $"{f.Id}: {f.GetMessage()}"));
                throw new Exception($"Roslyn compilation failed:\n{errorMsg}");
            }

            ms.Seek(0, SeekOrigin.Begin);

            var assemblyLoadContext = new AssemblyLoadContext("RoslynContext", isCollectible: true);
            var assembly = assemblyLoadContext.LoadFromStream(ms);

            var entryPoint = assembly.EntryPoint;
            if (entryPoint == null)
            {
                throw new Exception("No entry point found in the compiled code. Ensure the code has a Main method.");
            }

            var originalConsoleOut = Console.Out;
            using var sw = new StringWriter();
            try
            {
                Console.SetOut(sw);

                var parameters = entryPoint.GetParameters().Length > 0 ? new object[] { new string[0] } : null;
                entryPoint.Invoke(null, parameters);
            }
            finally
            {
                Console.SetOut(originalConsoleOut);
                assemblyLoadContext.Unload();
            }

            return sw.ToString();
        }
    }
}
