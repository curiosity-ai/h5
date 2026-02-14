using H5.Compiler;
using H5.Compiler.Hosted;
using H5.Contract;
using H5.Translator;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Reflection;

namespace H5.Compiler.Service.Tests
{
    public class TestCompiler : IDisposable
    {
        public string WorkingDirectory { get; }
        private string SourceDir => Path.Combine(WorkingDirectory, "src");
        private string OutDir => Path.Combine(WorkingDirectory, "bin");

        public TestCompiler()
        {
            WorkingDirectory = Path.Combine(Path.GetTempPath(), "H5_Test_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(WorkingDirectory);
            Directory.CreateDirectory(SourceDir);
            Directory.CreateDirectory(OutDir);
        }

        public void Dispose()
        {
            try
            {
                if (Directory.Exists(WorkingDirectory))
                {
                    Directory.Delete(WorkingDirectory, true);
                }
            }
            catch { }
        }

        public CompilationOutput Compile(Dictionary<string, string> sources, bool rebuild = true)
        {
            var assemblyName = "TestApp";

            var settings = new H5DotJson_AssemblySettings();
            settings.EnableCache = true;
            var request = new CompilationRequest(assemblyName, settings);
            request.WithLanguageVersion("Latest");

            foreach(var kvp in sources)
            {
                request.WithSourceFile(kvp.Key, kvp.Value);
            }

            // Generate options and files
            var options = request.ToOptions(SourceDir, NuGetVersion.Parse("10.0.0"));

            // Find H5.dll
            var h5Location = Path.GetFullPath("lib/H5.dll");
            if (!File.Exists(h5Location))
            {
                 // Try to find it relative to execution
                 var assemblyLoc = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                 // Assuming we are in Tests/H5.Compiler.Service.Tests/bin/Debug/net10.0/
                 // and H5.dll is in root/lib/
                 h5Location = Path.GetFullPath(Path.Combine(assemblyLoc, "../../../../../lib/H5.dll"));
            }

            options.H5Location = h5Location;
            options.Rebuild = rebuild;
            options.ProjectProperties.OutDir = OutDir;
            options.ProjectProperties.OutputPath = OutDir;

            // Modify csproj to reference H5.dll locally and avoid NuGet restore if possible
            var csprojPath = options.ProjectLocation;
            var csprojContent = File.ReadAllText(csprojPath);

            // Remove PackageReference to h5 if any (CompilationRequest adds it if we asked, but we didn't)
            // But we need to add Reference to H5.dll

            var referenceXml = $"<ItemGroup><Reference Include=\"H5\"><HintPath>{h5Location}</HintPath></Reference></ItemGroup>";
            csprojContent = csprojContent.Replace("</Project>", referenceXml + "\n</Project>");

            // Replace h5.Target SDK with Microsoft.NET.Sdk as we don't have the package installed in this environment
            csprojContent = System.Text.RegularExpressions.Regex.Replace(csprojContent, @"Sdk=""h5\.Target/[^""]+""", "Sdk=\"Microsoft.NET.Sdk\"");

            File.WriteAllText(csprojPath, csprojContent);

            var processor = new TranslatorProcessor(options, CancellationToken.None);
            processor.PreProcess(settings);
            processor.Process();
            processor.PostProcess();

            // Replicate CompilationOutput.FromOutputLocation logic via reflection because it is internal
            var output = (CompilationOutput)Activator.CreateInstance(typeof(CompilationOutput));
            var outputProp = output.GetType().GetProperty("Output");

            var outputLocation = processor.Translator.AssemblyInfo.Output;
            // processor.PostProcess calls GetOutputFolder which handles absolute paths.
            // But Translator.AssemblyInfo.Output might be relative.
            // Let's use the directory where we expect output.
            var absoluteOutputLocation = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(options.ProjectLocation), outputLocation));

            var dict = new Dictionary<string, string>();
            if (Directory.Exists(absoluteOutputLocation))
            {
                dict = Directory.EnumerateFiles(absoluteOutputLocation, "*", SearchOption.AllDirectories)
                    .Where(f => File.Exists(f))
                    .ToDictionary(f => f.Replace(absoluteOutputLocation, "").TrimStart(Path.DirectorySeparatorChar), f => File.ReadAllText(f));
            }

            outputProp.SetValue(output, dict);

            // Populate Stats if available
            if (processor.Translator.Stats != null)
            {
                output.Stats = processor.Translator.Stats;
            }

            return output;
        }
    }
}
