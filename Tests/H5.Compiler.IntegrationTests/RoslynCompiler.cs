using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests
{
    public static class RoslynCompiler
    {
        private static readonly SemaphoreSlim _consoleLock = new SemaphoreSlim(1, 1);

        public static string CompileAndRun(string csharpCode)
        {
            return CompileAndRunAsync(csharpCode).GetAwaiter().GetResult();
        }

        public static async Task<string> CompileAndRunAsync(string csharpCode)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(csharpCode);

            var references = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),        // System.Private.CoreLib
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),       // System.Console
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),    // System.Linq
                MetadataReference.CreateFromFile(typeof(List<>).Assembly.Location),        // System.Collections
                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location) // System.Runtime
            };

            var compilation = CSharpCompilation.Create(
                $"TestAssembly_{Guid.NewGuid()}",
                new[] { syntaxTree },
                references,
                new CSharpCompilationOptions(OutputKind.ConsoleApplication));

            using (var ms = new MemoryStream())
            {
                var result = compilation.Emit(ms);

                if (!result.Success)
                {
                    var failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error);

                    var errorMsg = string.Join("\n", failures.Select(d => d.ToString()));
                    throw new Exception($"Roslyn compilation failed:\n{errorMsg}");
                }

                ms.Seek(0, SeekOrigin.Begin);
                var assembly = Assembly.Load(ms.ToArray());

                // Find the entry point (Program.Main)
                var entryPoint = assembly.EntryPoint;
                if (entryPoint == null)
                {
                     // Try to find Program.Main manually if EntryPoint is not set (e.g. library output, but we set ConsoleApp)
                     // Or if Main is not static void Main(string[]) exactly.
                     var programType = assembly.GetType("Program");
                     if (programType != null)
                     {
                         entryPoint = programType.GetMethod("Main", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                     }
                }

                if (entryPoint == null)
                {
                    throw new Exception("Could not find entry point (Program.Main).");
                }

                var originalOut = Console.Out;
                await _consoleLock.WaitAsync();
                try
                {
                    using (var sw = new StringWriter())
                    {
                        Console.SetOut(sw);

                        var parameters = entryPoint.GetParameters();
                        object?[] args = parameters.Length == 0 ? null : new object[] { new string[0] };

                        var invokeResult = entryPoint.Invoke(null, args);

                        if (invokeResult is Task task)
                        {
                            await task;
                        }

                        return sw.ToString();
                    }
                }
                finally
                {
                    Console.SetOut(originalOut);
                    _consoleLock.Release();
                }
            }
        }
    }
}
