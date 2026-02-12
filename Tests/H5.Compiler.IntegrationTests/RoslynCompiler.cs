using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests
{
    public class TestConsole
    {
        private readonly StringWriter _writer = new StringWriter();

        public void Write(object? value) => _writer.Write(value);
        public void Write(string? value) => _writer.Write(value);
        public void Write(string format, params object?[] arg) => _writer.Write(format, arg);
        public void WriteLine() => _writer.WriteLine();
        public void WriteLine(object? value) => _writer.WriteLine(value);
        public void WriteLine(string? value) => _writer.WriteLine(value);
        public void WriteLine(string format, params object?[] arg) => _writer.WriteLine(format, arg);

        public override string ToString() => _writer.ToString();
    }

    public class ScriptGlobals
    {
        public TestConsole GlobalConsole { get; } = new TestConsole();
    }

    public static class RoslynCompiler
    {
        public static string CompileAndRun(string csharpCode)
        {
            // Since the interface is synchronous but ScriptRunner is async, we block.
            // Ideally we should propagate async, but for now we follow the existing signature if possible,
            // or update it. The previous implementation was synchronous (CompileAndRun).
            // BasicTests.cs calls it with 'await RunTest' but RunTest calls CompileAndRun synchronously?
            // Let's check IntegrationTestBase.cs
            return CompileAndRunAsync(csharpCode).GetAwaiter().GetResult();
        }

        public static async Task<string> CompileAndRunAsync(string csharpCode)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(csharpCode);
            var root = await syntaxTree.GetRootAsync();

            // 1. Separate Usings from the rest of the code
            var usings = root.DescendantNodes().OfType<UsingDirectiveSyntax>().ToList();
            var otherCode = root.RemoveNodes(usings, SyntaxRemoveOptions.KeepNoTrivia);

            var usingsCode = string.Join("\n", usings.Select(u => u.ToFullString()));
            var bodyCode = otherCode?.ToFullString() ?? "";

            // 2. Define the Shadow Console Class
            // We use a unique name for the impl field to avoid conflicts, though unlikely in a script.
            var shadowConsoleCode = @"
public class Console
{
    public static H5.Compiler.IntegrationTests.TestConsole Impl;

    public static void Write(object value) => Impl?.Write(value);
    public static void Write(string value) => Impl?.Write(value);
    public static void Write(string format, params object[] arg) => Impl?.Write(format, arg);
    public static void WriteLine() => Impl?.WriteLine();
    public static void WriteLine(object value) => Impl?.WriteLine(value);
    public static void WriteLine(string value) => Impl?.WriteLine(value);
    public static void WriteLine(string format, params object[] arg) => Impl?.WriteLine(format, arg);
}
";

            // 3. Initialization Code
            // GlobalConsole is a property of ScriptGlobals
            var initCode = "Console.Impl = GlobalConsole;";

            // 4. Invocation Code
            // We need to find the entry point (Program.Main)
            var programClass = root.DescendantNodes().OfType<ClassDeclarationSyntax>()
                .FirstOrDefault(c => c.Identifier.Text == "Program");

            string invocationCode = "";
            if (programClass != null)
            {
                var mainMethod = programClass.Members.OfType<MethodDeclarationSyntax>()
                    .FirstOrDefault(m => m.Identifier.Text == "Main");

                if (mainMethod != null)
                {
                    bool isAsync = mainMethod.Modifiers.Any(m => m.IsKind(SyntaxKind.AsyncKeyword));
                    string args = mainMethod.ParameterList.Parameters.Count > 0 ? "new string[0]" : "";

                    if (isAsync)
                    {
                        invocationCode = $"await Program.Main({args});";
                    }
                    else
                    {
                        invocationCode = $"Program.Main({args});";
                    }
                }
            }

            // Assemble the final script
            // Usings must be first.
            // Then the Shadow Console class.
            // Then the Init code (top level statement).
            // Then the User Code (Class definitions).
            // Then the Invocation (top level statement).

            var finalScript = $@"
{usingsCode}
{shadowConsoleCode}
{initCode}
{bodyCode}
{invocationCode}
";

            var globals = new ScriptGlobals();

            // Add necessary references
            // We need System.Runtime, System.Console (for types), and H5.Compiler.IntegrationTests (for TestConsole type)
            var references = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location), // System.Private.CoreLib
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location), // System.Console
                MetadataReference.CreateFromFile(typeof(TestConsole).Assembly.Location), // This assembly
                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location), // System.Linq
                MetadataReference.CreateFromFile(typeof(List<>).Assembly.Location) // System.Collections
                MetadataReference.CreateFromFile(Assembly.Load("Microsoft.CSharp").Location), // Dynamic support
                MetadataReference.CreateFromFile(Assembly.Load("System.Dynamic.Runtime").Location)
            };

            // Add standard imports
            var options = ScriptOptions.Default
                .WithReferences(references)
                .WithImports("System"); // Default imports

            var script = CSharpScript.Create(finalScript, options, globalsType: typeof(ScriptGlobals));

            try
            {
                var runner = script.CreateDelegate();
                await runner(globals, CancellationToken.None);
            }
            catch (CompilationErrorException e)
            {
                 throw new Exception($"Script compilation failed:\n{string.Join("\n", e.Diagnostics)}");
            }

            return globals.GlobalConsole.ToString();
        }
    }
}
