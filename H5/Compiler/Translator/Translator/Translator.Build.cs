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
            var helper = new ConfigHelper<AssemblyInfo>(this.Log);
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

                    var processor = new TranslatorProcessor(new H5Options
                    {
                        Rebuild = Rebuild,
                        ProjectLocation = projectRef,
                        H5Location = H5Location,
                        ProjectProperties = new Contract.ProjectProperties
                        {
                            BuildProjects = ProjectProperties.BuildProjects,
                            Configuration = ProjectProperties.Configuration
                        }
                    }, new Logger(null, false, LoggerLevel.Info, true, new ConsoleLoggerWriter(), new FileLoggerWriter()));

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

        public virtual void BuildAssembly()
        {
            this.Log.Info($"Building assembly '{ProjectProperties?.AssemblyName}' for location '{Location}'");

            var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

            var parseOptions = new CSharpParseOptions(LanguageVersion.CSharp7_2, Microsoft.CodeAnalysis.DocumentationMode.Parse, SourceCodeKind.Regular, DefineConstants);

            var files = SourceFiles;
            IList<string> referencesPathes = null;
            IList<PackageReference> referencedPackages = null;
            var baseDir = Path.GetDirectoryName(Location);

            XDocument projDefinition = XDocument.Load(Location);
            XNamespace rootNs = projDefinition.Root.Name.Namespace;
            var helper = new ConfigHelper<AssemblyInfo>(this.Log);
            var tokens = ProjectProperties.GetValues();

            referencesPathes = projDefinition
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

                    var processor = new TranslatorProcessor(new H5Options
                    {
                        Rebuild = Rebuild,
                        ProjectLocation = projectRef,
                        H5Location = H5Location,
                        ProjectProperties = new Contract.ProjectProperties
                        {
                            BuildProjects = ProjectProperties.BuildProjects,
                            Configuration = ProjectProperties.Configuration
                        }
                    }, new Logger(null, false, LoggerLevel.Info, true, new ConsoleLoggerWriter(), new FileLoggerWriter()));

                    processor.PreProcess();

                    var projectAssembly = processor.Translator.AssemblyLocation;

                    if (!File.Exists(projectAssembly) || Rebuild && !isBuilt)
                    {
                        processor.Process();
                        processor.PostProcess();
                    }

                    referencesPathes.Add(projectAssembly);
                }
            }
   
            //RFO: need to do the package discover first so it populates the 
            var referencesFromPackages = new List<MetadataReference>();

            if (referencedPackages is object && referencedPackages.Any())
            {
                string packagePath = GetPackagesCacheFolder();

                var outputFolder = Path.GetDirectoryName(AssemblyLocation);

                foreach (var rp in referencedPackages.GroupBy(p => p.PackageIdentity).OrderByDescending(p => p.Key.Version).Select(g => g.First()))
                {
                    var pp = Path.Combine(packagePath, rp.PackageIdentity.Id, rp.PackageIdentity.Version.ToString());
                    if (Directory.Exists(pp))
                    {
                        Log.Info($"NuGet: Importing package {rp.PackageIdentity.Id} version {rp.PackageIdentity.Version}");

                        var foundLibs = new List<string>();
                        foreach (var file in Directory.EnumerateFiles(Path.Combine(pp, "lib", rp.TargetFramework.GetShortFolderName()), "*.dll", SearchOption.AllDirectories))
                        {
                            referencesFromPackages.Add(MetadataReference.CreateFromFile(file));
                            _packagedFiles[Path.GetFileName(file)] = file;
                            foundLibs.Add(file);
                        }

                        foreach (var source in Directory.EnumerateFiles(Path.Combine(pp, "lib", rp.TargetFramework.GetShortFolderName()), "*.*", SearchOption.AllDirectories))
                        {
                            var target = Path.Combine(outputFolder, Path.GetFileName(source));
                            File.Copy(source, target, overwrite: true);
                            Log.Info($"NuGet: Copying lib file '{source}' to '{target}'");
                        }

                        var contentFolder = Path.Combine(pp, "content");
                        if (Directory.Exists(contentFolder))
                        {
                            foreach (var source in Directory.EnumerateFiles(contentFolder, "*.*", SearchOption.AllDirectories))
                            {
                                var target = Path.Combine(outputFolder, Path.GetFileName(source));
                                File.Copy(source, target, overwrite: true);
                                Log.Info($"NuGet: Copying content file '{source}' to '{target}'");
                            }
                        }

                        PackageReferencesDiscoveredPaths[rp.PackageIdentity.Id] = foundLibs.First();

                        if (string.Equals(rp.PackageIdentity.Id, CS.NS.H5, StringComparison.InvariantCultureIgnoreCase))
                        {
                            H5Location = foundLibs.Single(); //H5 should be the single dll in the file
                        }
                    }
                    else
                    {
                        Log.Warn($"NuGet package not found: {rp.PackageIdentity.Id} version {rp.PackageIdentity.Version}");
                        throw new Exception($"NuGet package not found: {rp.PackageIdentity.Id} version {rp.PackageIdentity.Version}");
                    }
                }

                //var resolver = new NuGet.Resolver.PackageResolver();
                //var packages = resolver.Resolve(new NuGet.Resolver.PackageResolverContext(NuGet.Resolver.DependencyBehavior.Highest,
                //                                    referencedPackages.Select(p => p.PackageIdentity.Id), 
                //                                    referencedPackages.Select(p => p.PackageIdentity.Id),
                //                                    referencedPackages, referencedPackages.Select(p => p.PackageIdentity),
                //                                    Enumerable.Empty<PackageIdentity>(),
                //packages,
                //Enumerable.Empty<PackageSource>()));

            }

            //var trustedAssembliesPaths = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")).Split(Path.PathSeparator);
            //var referencesFromTrustedPath = trustedAssembliesPaths
            //    .Where(p => neededAssemblies.Contains(Path.GetFileNameWithoutExtension(p)))
            //    .Select(p => MetadataReference.CreateFromFile(p))
            //    .ToList();


            referencesPathes.AddRange(PackageReferencesDiscoveredPaths.Values);

            var arr = referencesPathes.ToArray();
            foreach (var refPath in arr)
            {
                AddNestedReferences(referencesPathes, refPath);
            }

            IList<SyntaxTree> trees = new List<SyntaxTree>(files.Count);
            foreach (var file in files)
            {
                var filePath = Path.IsPathRooted(file) ? file : Path.GetFullPath((new Uri(Path.Combine(baseDir, file))).LocalPath);
                var syntaxTree = SyntaxFactory.ParseSyntaxTree(File.ReadAllText(filePath), parseOptions, filePath, Encoding.Default);
                trees.Add(syntaxTree);
            }

            var references = new List<MetadataReference>();
            var outputDir = Path.GetDirectoryName(AssemblyLocation);
            var di = new DirectoryInfo(outputDir);
            if (!di.Exists)
            {
                di.Create();
            }

            var updateH5Location = string.IsNullOrWhiteSpace(H5Location) || !File.Exists(H5Location);

            foreach (var path in referencesPathes)
            {
                var newPath = Path.GetFullPath(new Uri(Path.Combine(outputDir, Path.GetFileName(path))).LocalPath);
                if (string.Compare(newPath, path, true) != 0)
                {
                    File.Copy(path, newPath, true);
                }

                if (updateH5Location && string.Compare(Path.GetFileName(path), "h5.dll", true) == 0)
                {
                    H5Location = path;
                }

                references.Add(MetadataReference.CreateFromFile(path, new MetadataReferenceProperties(MetadataImageKind.Assembly, ImmutableArray.Create("global"))));
            }


            var compilation = CSharpCompilation.Create(ProjectProperties.AssemblyName ?? new DirectoryInfo(Location).Name, trees, null, compilationOptions)
                                .AddReferences(references)
                                .AddReferences(referencesFromPackages);

            Microsoft.CodeAnalysis.Emit.EmitResult emitResult;

            Log.Info($"Begin compiling {AssemblyLocation}");
            using (var outputStream = new FileStream(AssemblyLocation, FileMode.Create))
            {
                emitResult = compilation.Emit(outputStream, options: new Microsoft.CodeAnalysis.Emit.EmitOptions(false, Microsoft.CodeAnalysis.Emit.DebugInformationFormat.Embedded, runtimeMetadataVersion: RuntimeMetadataVersion, includePrivateMembers: true));
                outputStream.Flush();
                outputStream.Close();
            }
            Log.Info($"Finished compiling {AssemblyLocation}");

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

                throw new H5.Translator.TranslatorException(sb.ToString());
            }

            this.Log.Info($"Finished building assembly '{ProjectProperties?.AssemblyName}' for location '{Location}'");
        }

        private static string GetPackagesCacheFolder()
        {
            var overridePath = Environment.GetEnvironmentVariable("NUGET_PACKAGES");

            if (!string.IsNullOrWhiteSpace(overridePath))
            {
                overridePath = Environment.ExpandEnvironmentVariables(overridePath);
                if (Directory.Exists(overridePath))
                {
                    return overridePath;
                }
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return Environment.ExpandEnvironmentVariables(@"%userprofile%\.nuget\packages");
            }
            else
            {
                return "~/.nuget/packages";
            }
        }

        private void AddPackageAssembly(List<string> list, string packageDir)
        {
            if (Directory.Exists(packageDir))
            {
                var packageLib = Path.Combine(packageDir, "lib");

                if (Directory.Exists(packageLib))
                {
                    var libsFolders = Directory.GetDirectories(packageLib, "net*", SearchOption.TopDirectoryOnly);
                    var libFolder = libsFolders.Length > 0 ? (libsFolders.Contains("net40") ? "net40" : libsFolders[0]) : null;

                    if (libFolder != null)
                    {
                        var assemblies = Directory.GetFiles(libFolder, "*.dll", SearchOption.TopDirectoryOnly);

                        foreach (var assembly in assemblies)
                        {
                            list.Add(assembly);
                            _packagedFiles[Path.GetFileName(assembly)] = assembly;
                        }
                    }
                }
            }
        }

        private void AddNestedReferences(IList<string> referencesPathes, string refPath)
        {
            Log.Info($"Loading references from assembly {refPath}");

            if (!File.Exists(refPath))
            {
                var assemblyFileName = Path.GetFileName(refPath);
                if (_packagedFiles.TryGetValue(assemblyFileName, out var assemblyInPackagePath) && File.Exists(assemblyInPackagePath))
                {
                    Log.Info($"Redirecting assembly {refPath} to assembly in package {assemblyInPackagePath}");
                    refPath = assemblyInPackagePath;
                }
                else
                {
                    Log.Error($"Failed to locate assembly {refPath}");
                }
            }

            var asm = Mono.Cecil.AssemblyDefinition.ReadAssembly(refPath, new ReaderParameters()
            {
                ReadingMode = ReadingMode.Deferred,
                AssemblyResolver = new CecilAssemblyResolver(this.Log, AssemblyLocation)
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
    }
}