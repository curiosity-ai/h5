using System;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Xml.Linq;
using System.Linq;
using System.IO;
using Microsoft.Extensions.Logging;
using H5.Contract;
using System.Collections.Generic;
using System.Collections.Immutable;
using ICSharpCode.NRefactory.Documentation;
using System.Text;
using System.Globalization;
using Mono.Cecil;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Frameworks;
using NuGet.Versioning;
using ICSharpCode.NRefactory.Semantics;
using H5.Contract.Constants;
using System.Runtime.InteropServices;
using System.Collections.Concurrent;
using Mosaik.Core;
using ZLogger;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.CodeAnalysis.Emit;

namespace H5.Translator
{
    public partial class Translator
    {
        private const string RuntimeMetadataVersion = "v4.0.30319";

        private ConcurrentDictionary<string, string> _packagedFiles = new ConcurrentDictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        public virtual string[] GetProjectReferenceAssemblies()
        {
            var baseDir = Path.GetDirectoryName(Location);

            XDocument projDefinition = XDocument.Load(Location);
            XNamespace rootNs = projDefinition.Root.Name.Namespace;
            var helper = new ConfigHelper<AssemblyInfo>();
            var tokens = ProjectProperties.GetValues();

            var referencesPathes = projDefinition
                .Element(rootNs + "Project")
                .Elements(rootNs + "ItemGroup")
                .Elements(rootNs + "Reference")
                .Where(el => (el.Attribute("Include")?.Value != "System") && (el.Attribute("Condition") == null || el.Attribute("Condition").Value.ToLowerInvariant() != "false"))
                .Select(refElem => (refElem.Element(rootNs + "HintPath") == null ? (refElem.Attribute("Include") == null ? "" : refElem.Attribute("Include").Value) : refElem.Element(rootNs + "HintPath").Value))
                .Select(path => helper.ApplyPathTokens(tokens, Path.IsPathRooted(path) ? path : Path.GetFullPath((new Uri(Path.Combine(baseDir, path))).LocalPath)))
                .ToList();

            var projectReferences = projDefinition
                .Element(rootNs + "Project")
                .Elements(rootNs + "ItemGroup")
                .Elements(rootNs + "ProjectReference")
                .Where(el => el.Attribute("Condition") == null || el.Attribute("Condition").Value.ToLowerInvariant() != "false")
                .Select(refElem => (refElem.Element(rootNs + "HintPath") == null ? (refElem.Attribute("Include") == null ? "" : refElem.Attribute("Include").Value) : refElem.Element(rootNs + "HintPath").Value))
                .Select(path => helper.ApplyPathTokens(tokens, Path.IsPathRooted(path) ? path : Path.GetFullPath((new Uri(Path.Combine(baseDir, path))).LocalPath)))
                .ToArray();

            if (projectReferences.Length > 0)
            {
                if (ProjectProperties.BuildProjects == null)
                {
                    ProjectProperties.BuildProjects = new List<string>();
                }

                foreach (var projectRef in projectReferences)
                {
                    var isBuilt = ProjectProperties.BuildProjects.Contains(projectRef);

                    if (!isBuilt)
                    {
                        ProjectProperties.BuildProjects.Add(projectRef);
                    }

                    var processor = new TranslatorProcessor(new CompilationOptions
                    {
                        Rebuild = Rebuild,
                        ProjectLocation = projectRef,
                        H5Location = H5Location,
                        ProjectProperties = new ProjectProperties
                        {
                            BuildProjects = ProjectProperties.BuildProjects,
                            Configuration = ProjectProperties.Configuration
                        }
                    }, default);

                    processor.PreProcess();

                    var projectAssembly = processor.Translator.AssemblyLocation;

                    if (File.Exists(projectAssembly))
                    {
                        referencesPathes.Add(projectAssembly);
                    }
                }
            }

            return referencesPathes.ToArray();

        }

        public virtual Dictionary<string, string> BuildAssembly(CancellationToken cancellationToken)
        {
            using (new Measure(Logger, $"Building assembly '{ProjectProperties?.AssemblyName}' for location '{Location}'"))
            {
                var baseDir = Path.GetDirectoryName(Location);

                ProcessProjectReferences(baseDir, out var pathToReferencesInProject, out var referencedPackages, out var projectReferences);

                BuildReferencedProjectsIfNeeded(pathToReferencesInProject, projectReferences, cancellationToken);

                ProcessPackagedReferences(referencedPackages, out var referencesFromPackages, out var packageDiscoveredPaths, cancellationToken);

                pathToReferencesInProject.AddRange(packageDiscoveredPaths.Values);

                foreach (var refPath in pathToReferencesInProject.ToArray()) //ToArray because we modify the list in AddNestedReferences
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    AddNestedReferences(pathToReferencesInProject, refPath);
                }

                IList<SyntaxTree> trees = new List<SyntaxTree>(SourceFiles.Count);
                foreach (var file in SourceFiles)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var filePath = Path.IsPathRooted(file) ? file : Path.GetFullPath((new Uri(Path.Combine(baseDir, file))).LocalPath);
                    var syntaxTree = SyntaxFactory.ParseSyntaxTree(File.ReadAllText(filePath), new CSharpParseOptions(LanguageVersion.CSharp7_2, Microsoft.CodeAnalysis.DocumentationMode.Parse, SourceCodeKind.Regular, DefineConstants), filePath, Encoding.Default);
                    trees.Add(syntaxTree);
                }

                var references = new List<MetadataReference>();
                var outputDir = Path.GetDirectoryName(AssemblyLocation);
                var di = new DirectoryInfo(outputDir);

                if (!di.Exists) { di.Create(); }

                var updateH5Location = string.IsNullOrWhiteSpace(H5Location) || !File.Exists(H5Location);

                foreach (var path in pathToReferencesInProject)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var newPath = Path.GetFullPath(new Uri(Path.Combine(outputDir, Path.GetFileName(path))).LocalPath);
                    if (string.Compare(newPath, path, true) != 0)
                    {
                        CopyFileAsync(path, newPath).Wait();
                    }

                    if (updateH5Location && string.Compare(Path.GetFileName(path), "h5.dll", true) == 0)
                    {
                        H5Location = path;
                    }

                    references.Add(MetadataReference.CreateFromFile(path, new MetadataReferenceProperties(MetadataImageKind.Assembly, ImmutableArray.Create("global"))));
                }

                var emitResult = CompileAndEmit(referencesFromPackages, trees, references, cancellationToken);

                HandleBuildErrorsIfAny(baseDir, emitResult);

                return packageDiscoveredPaths;
            }
        }

        private void ProcessProjectReferences(string baseDir, out List<string> pathToReferencesInProject, out List<PackageReference> referencedPackages, out string[] projectReferences)
        {
            var projDefinition = XDocument.Load(Location);
            var rootNs = projDefinition.Root.Name.Namespace;
            var helper = new ConfigHelper<AssemblyInfo>();
            var tokens = ProjectProperties.GetValues();

            pathToReferencesInProject = projDefinition
                .Element(rootNs + "Project")
                .Elements(rootNs + "ItemGroup")
                .Elements(rootNs + "Reference")
                .Where(el => (el.Attribute("Include")?.Value != "System") && (el.Attribute("Condition") == null || el.Attribute("Condition").Value.ToLowerInvariant() != "false"))
                .Select(refElem => (refElem.Element(rootNs + "HintPath") == null ? (refElem.Attribute("Include") == null ? "" : refElem.Attribute("Include").Value) : refElem.Element(rootNs + "HintPath").Value))
                .Select(path => helper.ApplyPathTokens(tokens, Path.IsPathRooted(path) ? path : Path.GetFullPath((new Uri(Path.Combine(baseDir, path))).LocalPath)))
                .ToList();

            referencedPackages = projDefinition
                .Element(rootNs + "Project")
                .Elements(rootNs + "ItemGroup")
                .Elements(rootNs + "PackageReference")
                .Where(el => (el.Attribute("Include")?.Value != "System") && (el.Attribute("Condition") == null || el.Attribute("Condition").Value.ToLowerInvariant() != "false"))
                .Select(refElem => new PackageReference(new PackageIdentity(refElem.Attribute("Include").Value, new NuGetVersion(refElem.Attribute("Version").Value)), NuGetFramework.Parse("netstandard2.0")))
                .ToList();

            projectReferences = projDefinition
                .Element(rootNs + "Project")
                .Elements(rootNs + "ItemGroup")
                .Elements(rootNs + "ProjectReference")
                .Where(el => el.Attribute("Condition") == null || el.Attribute("Condition").Value.ToLowerInvariant() != "false")
                .Select(refElem => (refElem.Element(rootNs + "HintPath") == null ? (refElem.Attribute("Include") == null ? "" : refElem.Attribute("Include").Value) : refElem.Element(rootNs + "HintPath").Value))
                .Select(path => helper.ApplyPathTokens(tokens, Path.IsPathRooted(path) ? path : Path.GetFullPath((new Uri(Path.Combine(baseDir, path))).LocalPath)))
                .ToArray();
        }

        private void BuildReferencedProjectsIfNeeded(List<string> pathToReferencesInProject, string[] projectReferences, CancellationToken cancellationToken)
        {
            if (projectReferences.Length > 0)
            {
                if (ProjectProperties.BuildProjects == null)
                {
                    ProjectProperties.BuildProjects = new List<string>();
                }

                foreach (var projectRef in projectReferences)
                {
                    var isBuilt = ProjectProperties.BuildProjects.Contains(projectRef);

                    if (!isBuilt)
                    {
                        ProjectProperties.BuildProjects.Add(projectRef);
                    }

                    var processor = new TranslatorProcessor(new CompilationOptions
                    {
                        Rebuild = Rebuild,
                        ProjectLocation = projectRef,
                        H5Location = H5Location,
                        ProjectProperties = new ProjectProperties
                        {
                            BuildProjects = ProjectProperties.BuildProjects,
                            Configuration = ProjectProperties.Configuration
                        }
                    }, cancellationToken);

                    processor.PreProcess();

                    var projectAssembly = processor.Translator.AssemblyLocation;

                    if (!File.Exists(projectAssembly) || Rebuild && !isBuilt)
                    {
                        processor.Process();
                        processor.PostProcess();
                    }

                    pathToReferencesInProject.Add(projectAssembly);
                }
            }
        }

        private void ProcessPackagedReferences(List<PackageReference> referencedPackages, out List<MetadataReference> referencesFromPackages, out Dictionary<string, string> packageDiscoveredPaths,  CancellationToken cancellationToken)
        {
            //RFO: need to do the package discover first so it populates the 
            referencesFromPackages = new List<MetadataReference>();

            packageDiscoveredPaths = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            if (referencedPackages is object && referencedPackages.Any())
            {
                string nugetLocation = GetPackagesCacheFolder();

                var outputFolder = Path.GetDirectoryName(AssemblyLocation);

                var filesToCopy = new List<(string source, string destination)>();

                foreach (var rp in referencedPackages.GroupBy(p => p.PackageIdentity).OrderByDescending(p => p.Key.Version).Select(g => g.First()))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    string packageBasePath;

                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        packageBasePath = Path.Combine(nugetLocation, rp.PackageIdentity.Id, rp.PackageIdentity.Version.ToString());
                    }
                    else
                    {
                        //Package path is lower-case on Linux platforms
                        packageBasePath = Path.Combine(nugetLocation, rp.PackageIdentity.Id.ToLowerInvariant(), rp.PackageIdentity.Version.ToString().ToLowerInvariant());
                    }

                    if (Directory.Exists(packageBasePath))
                    {
                        Logger.ZLogInformation($"NuGet: Importing package {rp.PackageIdentity.Id} version {rp.PackageIdentity.Version}");

                        var foundLibs = new List<string>();
                        foreach (var file in Directory.EnumerateFiles(Path.Combine(packageBasePath, "lib", rp.TargetFramework.GetShortFolderName()), "*.dll", SearchOption.AllDirectories))
                        {
                            referencesFromPackages.Add(MetadataReference.CreateFromFile(file));
                            _packagedFiles[Path.GetFileName(file)] = file;
                            foundLibs.Add(file);
                        }

                        foreach (var source in Directory.EnumerateFiles(Path.Combine(packageBasePath, "lib", rp.TargetFramework.GetShortFolderName()), "*.*", SearchOption.AllDirectories))
                        {
                            var target = Path.Combine(outputFolder, Path.GetFileName(source));
                            filesToCopy.Add((source, target));
                        }

                        var contentFolder = Path.Combine(packageBasePath, "content");
                        if (Directory.Exists(contentFolder))
                        {
                            foreach (var source in Directory.EnumerateFiles(contentFolder, "*.*", SearchOption.AllDirectories))
                            {
                                var target = Path.Combine(outputFolder, Path.GetFileName(source));
                                filesToCopy.Add((source, target));
                            }
                        }

                        packageDiscoveredPaths[rp.PackageIdentity.Id] = foundLibs.First();

                        if (string.Equals(rp.PackageIdentity.Id, CS.NS.H5, StringComparison.InvariantCultureIgnoreCase))
                        {
                            H5Location = foundLibs.Single(); //H5 should be the single dll in the file
                        }
                    }
                    else
                    {
                        Logger.ZLogError($"NuGet package not found on path '{packageBasePath}': {rp.PackageIdentity.Id} version {rp.PackageIdentity.Version}");
                        throw new Exception($"NuGet package not found on path '{packageBasePath}': {rp.PackageIdentity.Id} version {rp.PackageIdentity.Version}");
                    }
                }

                if (filesToCopy.Any())
                {
                    Task.WaitAll(filesToCopy.Select(async (copy) =>
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        Logger.ZLogInformation($"NuGet: Copying lib file '{copy.source}' to '{copy.destination}'");
                        await CopyFileAsync(copy.source, copy.destination).ConfigureAwait(false);
                    }).ToArray());
                }
            }
        }

        private EmitResult CompileAndEmit(List<MetadataReference> referencesFromPackages, IList<SyntaxTree> trees, List<MetadataReference> references, CancellationToken cancellationToken)
        {
            var compilation = CSharpCompilation.Create(ProjectProperties.AssemblyName ?? new DirectoryInfo(Location).Name, trees, null, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                                .AddReferences(references)
                                .AddReferences(referencesFromPackages);

            EmitResult emitResult;

            using (new Measure(Logger, $"Compiling {AssemblyLocation} with Roslyn"))
            {
                using (var outputStream = new FileStream(AssemblyLocation, FileMode.Create))
                {
                    emitResult = compilation.Emit(outputStream, options: new Microsoft.CodeAnalysis.Emit.EmitOptions(false, Microsoft.CodeAnalysis.Emit.DebugInformationFormat.Embedded, runtimeMetadataVersion: RuntimeMetadataVersion, includePrivateMembers: true), cancellationToken: cancellationToken);
                    outputStream.Flush();
                    outputStream.Close();
                }
            }

            return emitResult;
        }

        private void HandleBuildErrorsIfAny(string baseDir, EmitResult emitResult)
        {
            if (!emitResult.Success)
            {
                StringBuilder sb = new StringBuilder("C# Compilation Failed");
                sb.AppendLine();

                baseDir = File.Exists(Location) ? Path.GetDirectoryName(Location) : Path.GetFullPath(Location);

                foreach (var d in emitResult.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error))
                {
                    var filePath = d.Location?.SourceTree?.FilePath ?? "";
                    if (filePath.StartsWith(baseDir))
                    {
                        filePath = filePath.Substring(baseDir.Length + 1);
                    }

                    var mapped = d.Location != null ? d.Location.GetMappedLineSpan() : default(FileLinePositionSpan);
                    sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "\t{4}({0},{1}): {2}: {3}", mapped.StartLinePosition.Line + 1, mapped.StartLinePosition.Character + 1, d.Id, d.GetMessage(), filePath));
                    foreach (var l in d.AdditionalLocations)
                    {
                        filePath = l.SourceTree.FilePath ?? "";
                        if (filePath.StartsWith(baseDir))
                        {
                            filePath = filePath.Substring(baseDir.Length + 1);
                        }
                        mapped = l.GetMappedLineSpan();
                        sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "\t{2}({0},{1}): (Related location)", mapped.StartLinePosition.Line + 1, mapped.StartLinePosition.Character + 1, filePath));
                    }
                }

                throw new TranslatorException(sb.ToString());
            }
        }

        private static string GetPackagesCacheFolder()
        {
            var overridePath = Environment.GetEnvironmentVariable("NUGET_PACKAGES");

            if (!string.IsNullOrWhiteSpace(overridePath))
            {
                overridePath = Path.GetFullPath(Environment.ExpandEnvironmentVariables(overridePath));
                if (Directory.Exists(overridePath))
                {
                    return overridePath;
                }
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return Path.GetFullPath(Environment.ExpandEnvironmentVariables(@"%userprofile%\.nuget\packages\"));
            }
            else
            {
                return Path.GetFullPath("~/.nuget/packages/");
            }
        }

        //private void AddPackageAssembly(List<string> list, string packageDir)
        //{
        //    if (Directory.Exists(packageDir))
        //    {
        //        var packageLib = Path.Combine(packageDir, "lib");

        //        if (Directory.Exists(packageLib))
        //        {
        //            var libsFolders = Directory.GetDirectories(packageLib, "net*", SearchOption.TopDirectoryOnly);
        //            var libFolder = libsFolders.Length > 0 ? (libsFolders.Contains("net40") ? "net40" : libsFolders[0]) : null;

        //            if (libFolder != null)
        //            {
        //                var assemblies = Directory.GetFiles(libFolder, "*.dll", SearchOption.TopDirectoryOnly);

        //                foreach (var assembly in assemblies)
        //                {
        //                    list.Add(assembly);
        //                    _packagedFiles[Path.GetFileName(assembly)] = assembly;
        //                }
        //            }
        //        }
        //    }
        //}

        private void AddNestedReferences(List<string> referencesPathes, string refPath)
        {
            Logger.ZLogInformation($"Loading references from assembly {refPath}");

            if (!File.Exists(refPath))
            {
                var assemblyFileName = Path.GetFileName(refPath);
                if (_packagedFiles.TryGetValue(assemblyFileName, out var assemblyInPackagePath) && File.Exists(assemblyInPackagePath))
                {
                    Logger.ZLogInformation($"Redirecting assembly {refPath} to assembly in package {assemblyInPackagePath}");
                    refPath = assemblyInPackagePath;
                }
                else
                {
                    Logger.ZLogError($"Failed to locate assembly {refPath}");
                }
            }

            var asm = AssemblyDefinition.ReadAssembly(refPath, new ReaderParameters()
            {
                ReadingMode = ReadingMode.Deferred,
                AssemblyResolver = new CecilAssemblyResolver(AssemblyLocation)
            });

            foreach (AssemblyNameReference r in asm.MainModule.AssemblyReferences)
            {
                var name = r.Name;

                if (name == SystemAssemblyName || name == "System.Core")
                {
                    continue;
                }

                var path = Path.Combine(Path.GetDirectoryName(refPath), name) + ".dll";

                if (referencesPathes.Any(rp => Path.GetFileNameWithoutExtension(rp).Equals(name, StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }

                referencesPathes.Add(path);

                AddNestedReferences(referencesPathes, path);
            }
        }

        private static async Task CopyFileAsync(string sourceFile, string destinationFile)
        {
            var sfi = new FileInfo(sourceFile);
            var dfi = new FileInfo(destinationFile);

            if(!sfi.Exists)
            {
                throw new FileNotFoundException($"File {sourceFile} not found");
            }

            if (dfi.Exists)
            {
                if(dfi.Length == sfi.Length && dfi.LastWriteTimeUtc == sfi.LastWriteTimeUtc)
                {
                    //We can assume it's the same file, so skip copying
                    return;
                }
                else
                {
                    lock (_loadedAssembliesLock)
                    {
                        if (_loadedAssemblies.TryGetValue(destinationFile, out var previouslyLoaded))
                        {
                            _loadedAssemblies.Remove(destinationFile);
                            previouslyLoaded.assembly.Dispose();
                        }

                        if (_loadedAssemblieStreams.TryGetValue(destinationFile, out var stream))
                        {
                            _loadedAssemblieStreams.Remove(destinationFile);
                            stream.Close();
                            stream.Dispose();
                        }
                    }

                    await Task.Delay(50);
                }
            }

            var fileOptions = FileOptions.Asynchronous | FileOptions.SequentialScan;
            var bufferSize = 4096;
            using (var sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, fileOptions))
            using (var destinationStream = new FileStream(destinationFile, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, bufferSize, fileOptions))
            {
                destinationStream.SetLength(0);
                await sourceStream.CopyToAsync(destinationStream, bufferSize).ConfigureAwait(false);
                await destinationStream.FlushAsync().ConfigureAwait(false);
                sourceStream.Close();
                destinationStream.Close();
            }

            File.SetLastWriteTimeUtc(destinationFile, sfi.LastWriteTimeUtc);
        }
    }
}