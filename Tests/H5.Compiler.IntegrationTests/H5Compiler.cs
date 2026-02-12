using H5.Compiler;
using H5.Compiler.Hosted;
using H5.Translator;
using NuGet.Versioning;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests
{
    public static class H5Compiler
    {
        public static async Task<string> CompileToJs(string csharpCode)
        {
            var settings = new H5DotJson_AssemblySettings();
            var request = new CompilationRequest("App", settings)
                            //.NoPackageResources() // Comment this out to get resources
                            .NoHTML()
                            .WithPackageReference("h5",      NuGetVersion.Parse("25.11.62743"))
                            .WithSourceFile("App.cs", csharpCode);

            var compiledJavascript = await CompilationProcessor.CompileAsync(request);

            if (compiledJavascript.Output == null || !compiledJavascript.Output.Any())
            {
                 throw new Exception("H5 compilation failed or produced no output.");
            }

            var jsFiles = compiledJavascript.Output
                .Where(f => f.Key.EndsWith(".js") && !f.Key.EndsWith(".min.js"))
                .ToList();

            if (!jsFiles.Any())
            {
                 // Fallback to minified if no non-minified found
                 jsFiles = compiledJavascript.Output
                    .Where(f => f.Key.EndsWith(".js"))
                    .ToList();
            }

            if (!jsFiles.Any())
            {
                throw new Exception("Could not find generated JavaScript file in output.");
            }

            // Sort files: h5.js/h5.core.js first, then others.
            // We rely on the order returned or name heuristics.

            var sortedFiles = jsFiles.OrderBy(f =>
            {
                var name = System.IO.Path.GetFileName(f.Key).ToLowerInvariant();
                if (name == "h5.js") return 0;
                if (name == "h5.core.js") return 1;
                if (name.StartsWith("h5.")) return 2;
                return 10;
            }).ToList();

            var sb = new System.Text.StringBuilder();
            foreach (var file in sortedFiles)
            {
                sb.AppendLine($"// File: {file.Key}");
                sb.AppendLine(file.Value);
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
