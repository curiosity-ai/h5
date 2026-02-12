using H5.Compiler;
using H5.Compiler.Hosted;
using H5.Translator;
using NuGet.Versioning;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

namespace H5.Compiler.IntegrationTests
{
    public static class H5Compiler
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private static NuGetVersion? _cachedLatestVersion;

        private static async Task<NuGetVersion> GetLatestVersionAsync()
        {
            if (_cachedLatestVersion != null)
            {
                return _cachedLatestVersion;
            }

            try
            {
                // Fetch the list of versions from the official NuGet API
                var json = await _httpClient.GetStringAsync("https://api.nuget.org/v3-flatcontainer/h5/index.json");
                var versions = new List<NuGetVersion>();

                using (var doc = JsonDocument.Parse(json))
                {
                    if (doc.RootElement.TryGetProperty("versions", out var versionsProp) && versionsProp.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var v in versionsProp.EnumerateArray())
                        {
                            if (v.ValueKind == JsonValueKind.String)
                            {
                                var versionString = v.GetString();
                                if (!string.IsNullOrEmpty(versionString) && NuGetVersion.TryParse(versionString, out var version))
                                {
                                    versions.Add(version);
                                }
                            }
                        }
                    }
                }

                if (versions.Count == 0)
                {
                    throw new Exception("No versions found for h5 package.");
                }

                // Get the latest version (Max)
                _cachedLatestVersion = versions.Max();
                Console.WriteLine($"Resolved latest H5 version: {_cachedLatestVersion}");

                // Ensure the package is available in the global NuGet cache
                await EnsurePackageRestored(_cachedLatestVersion!);

                return _cachedLatestVersion!;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch or restore latest h5 version.", ex);
            }
        }

        private static async Task EnsurePackageRestored(NuGetVersion version)
        {
            var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var packagePath = Path.Combine(userProfile, ".nuget", "packages", "h5", version.ToString());

            if (Directory.Exists(packagePath))
            {
                return;
            }

            Console.WriteLine($"Package h5 version {version} not found in cache. Restoring...");

            // Create a temporary project to trigger restoration
            var tempDir = Path.Combine(Path.GetTempPath(), "H5_Restore_" + Guid.NewGuid());
            Directory.CreateDirectory(tempDir);

            try
            {
                var csprojContent = $@"
<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <TargetFramework>netstandard2.0</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include=""h5"" Version=""{version}"" />
  </ItemGroup>
</Project>";

                await File.WriteAllTextAsync(Path.Combine(tempDir, "Restore.csproj"), csprojContent);

                var startInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = "restore",
                    WorkingDirectory = tempDir,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(startInfo))
                {
                    if (process == null) throw new Exception("Failed to start dotnet restore process.");

                    var output = await process.StandardOutput.ReadToEndAsync();
                    var error = await process.StandardError.ReadToEndAsync();

                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        throw new Exception($"dotnet restore failed with exit code {process.ExitCode}.\nOutput: {output}\nError: {error}");
                    }
                }

                Console.WriteLine($"Successfully restored h5 version {version}.");
            }
            finally
            {
                try
                {
                    Directory.Delete(tempDir, true);
                }
                catch { /* Ignore cleanup errors */ }
            }
        }

        public static async Task<string> CompileToJs(string csharpCode)
        {
            var latestVersion = await GetLatestVersionAsync();

            var settings = new H5DotJson_AssemblySettings()
            {
                Reflection = new ReflectionConfig()
                {
                    Disabled = false,
                    Target = Contract.MetadataTarget.Inline, 
                }
            }
            ;
            var request = new CompilationRequest("App", settings)
                            //.NoPackageResources() // Comment this out to get resources
                            .NoHTML()
                            .WithPackageReference("h5", latestVersion)
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
