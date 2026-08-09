using System.Xml.Linq;
using Microsoft.CodeAnalysis.CSharp;

namespace H5.Translator.Roslyn.CLI;

/// <summary>
/// Minimal resolver for an H5 project: gathers the C# sources (the SDK's default glob),
/// and resolves the package references — transitively — to their assemblies in the NuGet
/// global-packages cache. H5 projects reference H5.dll as their whole BCL plus a few h5.*
/// packages (h5.core, h5.Newtonsoft.Json), all of which live in the cache once restored.
/// </summary>
internal sealed class ResolvedProject
{
    public required string CsprojPath { get; init; }
    public required string ProjectDir { get; init; }
    public required string AssemblyName { get; init; }
    public required List<(string path, string text)> Sources { get; init; }
    public required List<string> ReferencePaths { get; init; }
    public required List<string> DefineConstants { get; init; }
    public required LanguageVersion LanguageVersion { get; init; }

    /// <summary>Directories of every project in the closure — the root first, then the
    /// referenced projects it pulls in (each may contribute h5.json resources).</summary>
    public required List<string> ProjectDirs { get; init; }

    /// <summary>In separate-assembly mode, the built output DLLs of referenced projects — the
    /// consumer extracts their embedded JS/resources instead of recompiling their source.</summary>
    public List<string> ReferencedProjectDlls { get; init; } = new();
}

internal static class ProjectResolver
{
    public static ResolvedProject Resolve(string csprojPath, string configuration = "Debug", bool separateAssemblies = false)
    {
        csprojPath = Path.GetFullPath(csprojPath);
        var projectDir = Path.GetDirectoryName(csprojPath)!;
        var doc = XDocument.Load(csprojPath);

        var assemblyName = Property(doc, "AssemblyName") ?? Path.GetFileNameWithoutExtension(csprojPath);

        var defines = (Property(doc, "DefineConstants") ?? "")
            .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim())
            .Where(s => s.Length > 0)
            .ToList();
        if (!defines.Contains("H5")) defines.Add("H5"); // H5 projects always compile the H5 branch

        var lang = ParseLangVersion(Property(doc, "LangVersion"));

        // Default (bundle) mode: translate the whole closure of source projects into one JS
        // output. Separate-assembly mode: compile only this project's own sources and reference
        // each project dependency as its built DLL (its JS is embedded and extracted, not recompiled).
        var sources = new List<(string, string)>();
        var references = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var visitedProjects = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var projectDirs = new List<string>();
        var projectDlls = new List<string>();
        var roots = NuGetRoots().Where(Directory.Exists).ToList();
        CollectProject(csprojPath, sources, references, visitedProjects, projectDirs, roots,
            separateAssemblies, projectDlls, configuration, isRoot: true);

        return new ResolvedProject
        {
            CsprojPath = csprojPath,
            ProjectDir = projectDir,
            AssemblyName = assemblyName,
            Sources = sources,
            ReferencePaths = references.Values.ToList(),
            DefineConstants = defines,
            LanguageVersion = lang,
            ProjectDirs = projectDirs,
            ReferencedProjectDlls = projectDlls,
        };
    }

    /// <summary>Adds a project's sources and package references, then handles its ProjectReferences —
    /// recursing into their source (bundle mode) or referencing their built DLL (separate mode).</summary>
    private static void CollectProject(
        string csprojPath,
        List<(string, string)> sources,
        Dictionary<string, string> references,
        HashSet<string> visited,
        List<string> projectDirs,
        List<string> roots,
        bool separate,
        List<string> projectDlls,
        string configuration,
        bool isRoot)
    {
        csprojPath = Path.GetFullPath(csprojPath);
        if (!visited.Add(csprojPath) || !File.Exists(csprojPath)) return;

        var doc = XDocument.Load(csprojPath);
        var projectDir = Path.GetDirectoryName(csprojPath)!;

        // In separate mode only the root contributes source + h5.json resources; a referenced
        // project contributes its DLL (below) and its own package references for binding.
        if (!separate || isRoot)
        {
            projectDirs.Add(projectDir);
            sources.AddRange(ResolveSources(doc, projectDir));
        }
        foreach (var (name, path) in ResolvePackageReferenceDlls(doc, roots))
            references.TryAdd(name, path);

        foreach (var pr in doc.Descendants().Where(e => e.Name.LocalName == "ProjectReference"))
        {
            var include = pr.Attribute("Include")?.Value;
            if (string.IsNullOrWhiteSpace(include)) continue;
            var refPath = Path.GetFullPath(Path.Combine(projectDir, include!.Replace('\\', '/')));

            if (separate)
            {
                // Reference the dependency's built DLL; still gather its package refs (and its own
                // project-ref DLLs) so types it exposes bind, but not its source or resources.
                var dll = ProjectOutputDll(refPath, configuration);
                if (dll is not null)
                {
                    references[Path.GetFileNameWithoutExtension(dll)] = dll;
                    if (!projectDlls.Contains(dll)) projectDlls.Add(dll);
                }
                if (visited.Add(refPath) && File.Exists(refPath))
                {
                    var refDoc = XDocument.Load(refPath);
                    var refDir = Path.GetDirectoryName(refPath)!;
                    foreach (var (name, path) in ResolvePackageReferenceDlls(refDoc, roots))
                        references.TryAdd(name, path);
                    foreach (var nested in refDoc.Descendants().Where(e => e.Name.LocalName == "ProjectReference"))
                    {
                        var ninc = nested.Attribute("Include")?.Value;
                        if (string.IsNullOrWhiteSpace(ninc)) continue;
                        var ndll = ProjectOutputDll(Path.GetFullPath(Path.Combine(refDir, ninc!.Replace('\\', '/'))), configuration);
                        if (ndll is not null)
                        {
                            references[Path.GetFileNameWithoutExtension(ndll)] = ndll;
                            if (!projectDlls.Contains(ndll)) projectDlls.Add(ndll);
                        }
                    }
                }
            }
            else
            {
                CollectProject(refPath, sources, references, visited, projectDirs, roots,
                    separate, projectDlls, configuration, isRoot: false);
            }
        }
    }

    /// <summary>
    /// The referenced projects of <paramref name="rootCsproj"/> in build order — every transitive
    /// ProjectReference, dependencies before dependents (post-order DFS), excluding the root. So a
    /// caller can build each one (as a package) before the project that consumes it.
    /// </summary>
    public static List<string> ReferencedProjectsInBuildOrder(string rootCsproj)
    {
        rootCsproj = Path.GetFullPath(rootCsproj);
        var order = new List<string>();
        var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        void Visit(string csproj, bool isRoot)
        {
            csproj = Path.GetFullPath(csproj);
            if (!visited.Add(csproj) || !File.Exists(csproj)) return;
            var dir = Path.GetDirectoryName(csproj)!;
            var doc = XDocument.Load(csproj);
            foreach (var pr in doc.Descendants().Where(e => e.Name.LocalName == "ProjectReference"))
            {
                var include = pr.Attribute("Include")?.Value;
                if (string.IsNullOrWhiteSpace(include)) continue;
                Visit(Path.GetFullPath(Path.Combine(dir, include!.Replace('\\', '/'))), isRoot: false);
            }
            if (!isRoot) order.Add(csproj);   // post-order → dependencies precede this project
        }

        Visit(rootCsproj, isRoot: true);
        return order;
    }

    /// <summary>The built output DLL path of a project, or null when the .csproj is missing.</summary>
    public static string? OutputDll(string csprojPath, string configuration) => ProjectOutputDll(csprojPath, configuration);

    /// <summary>
    /// True if <paramref name="csprojPath"/>'s package DLL is present, carries embedded H5 resources
    /// (i.e. was built by the translator, not a plain csc build), and is newer than the project's
    /// .csproj, all its source files, and every referenced project's DLL — the same incremental
    /// check the MSBuild-driven compiler relies on. When false, the project must be rebuilt.
    /// </summary>
    public static bool IsPackageUpToDate(string csprojPath, string configuration)
    {
        csprojPath = Path.GetFullPath(csprojPath);
        var dll = ProjectOutputDll(csprojPath, configuration);
        if (dll is null || !File.Exists(dll)) return false;
        if (!ResourceEmbedder.HasManifest(dll)) return false;   // a plain build with no embedded JS

        var dllTime = File.GetLastWriteTimeUtc(dll);
        var dir = Path.GetDirectoryName(csprojPath)!;

        if (File.GetLastWriteTimeUtc(csprojPath) > dllTime) return false;

        foreach (var src in Directory.EnumerateFiles(dir, "*.cs", SearchOption.AllDirectories))
        {
            if (IsUnderBuildOutput(src, dir)) continue;
            if (File.GetLastWriteTimeUtc(src) > dllTime) return false;
        }

        // A dependency rebuilt more recently invalidates this project too.
        var doc = XDocument.Load(csprojPath);
        foreach (var pr in doc.Descendants().Where(e => e.Name.LocalName == "ProjectReference"))
        {
            var include = pr.Attribute("Include")?.Value;
            if (string.IsNullOrWhiteSpace(include)) continue;
            var depDll = ProjectOutputDll(Path.GetFullPath(Path.Combine(dir, include!.Replace('\\', '/'))), configuration);
            if (depDll is null || !File.Exists(depDll) || File.GetLastWriteTimeUtc(depDll) > dllTime) return false;
        }

        return true;
    }

    /// <summary>The built output DLL path of a referenced project: bin/&lt;config&gt;/&lt;tfm&gt;/&lt;asm&gt;.dll.</summary>
    private static string? ProjectOutputDll(string csprojPath, string configuration)
    {
        if (!File.Exists(csprojPath)) return null;
        var doc = XDocument.Load(csprojPath);
        var dir = Path.GetDirectoryName(csprojPath)!;
        var asm = Property(doc, "AssemblyName") ?? Path.GetFileNameWithoutExtension(csprojPath);
        var tfm = Property(doc, "TargetFramework")
                  ?? Property(doc, "TargetFrameworks")?.Split(';', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault()
                  ?? "netstandard2.0";
        var binBase = Path.Combine(dir, "bin", configuration, tfm);
        var dll = Path.Combine(binBase, asm + ".dll");
        return dll;
    }

    private static List<(string path, string text)> ResolveSources(XDocument doc, string projectDir)
    {
        // SDK default glob: every .cs under the project directory, minus the build output.
        var all = Directory.EnumerateFiles(projectDir, "*.cs", SearchOption.AllDirectories)
            .Where(f => !IsUnderBuildOutput(f, projectDir))
            .ToList();

        // Honour explicit <Compile Remove="..."/> (glob or exact, relative to the project dir).
        var removed = doc.Descendants().Where(e => e.Name.LocalName == "Compile")
            .Select(e => e.Attribute("Remove")?.Value)
            .Where(v => !string.IsNullOrWhiteSpace(v))
            .Select(v => NormalizeGlob(projectDir, v!))
            .ToList();

        var result = new List<(string, string)>();
        foreach (var file in all)
        {
            var full = Path.GetFullPath(file);
            if (removed.Any(r => MatchesGlob(full, r))) continue;
            result.Add((full, File.ReadAllText(full)));
        }
        return result;
    }

    private static bool IsUnderBuildOutput(string file, string projectDir)
    {
        var rel = Path.GetRelativePath(projectDir, file).Replace('\\', '/');
        return rel.StartsWith("obj/") || rel.StartsWith("bin/")
            || rel.Contains("/obj/") || rel.Contains("/bin/");
    }

    private static string NormalizeGlob(string projectDir, string pattern)
        => Path.GetFullPath(Path.Combine(projectDir, pattern.Replace('\\', '/'))).Replace('\\', '/');

    private static bool MatchesGlob(string fullPath, string pattern)
    {
        fullPath = fullPath.Replace('\\', '/');
        if (!pattern.Contains('*')) return string.Equals(fullPath, pattern, StringComparison.OrdinalIgnoreCase);
        // Support the common **/*.cs / *.cs shapes with a simple regex translation.
        var rx = "^" + System.Text.RegularExpressions.Regex.Escape(pattern)
            .Replace(@"\*\*/", "(.*/)?").Replace(@"\*\*", ".*").Replace(@"\*", "[^/]*") + "$";
        return System.Text.RegularExpressions.Regex.IsMatch(fullPath, rx, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
    }

    // ---- reference resolution (NuGet global-packages cache) -----------------

    private static IEnumerable<(string name, string path)> ResolvePackageReferenceDlls(XDocument doc, List<string> roots)
    {
        var resolved = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase); // asmName → dll path
        var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);             // pkgId@version

        foreach (var pr in doc.Descendants().Where(e => e.Name.LocalName == "PackageReference"))
        {
            var id = pr.Attribute("Include")?.Value ?? pr.Attribute("Update")?.Value;
            var version = pr.Attribute("Version")?.Value ?? pr.Element(pr.Name.Namespace + "Version")?.Value;
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(version)) continue;
            ResolvePackage(roots, id!, version!, resolved, visited);
        }

        return resolved.Select(kv => (kv.Key, kv.Value));
    }

    private static void ResolvePackage(
        List<string> roots, string id, string version,
        Dictionary<string, string> resolved, HashSet<string> visited)
    {
        var key = id + "@" + version;
        if (!visited.Add(key)) return;

        var pkgDir = roots
            .Select(r => Path.Combine(r, id.ToLowerInvariant(), version))
            .FirstOrDefault(Directory.Exists);
        if (pkgDir is null) return;

        var libDir = BestLibDir(pkgDir);
        if (libDir is not null)
        {
            foreach (var dll in Directory.GetFiles(libDir, "*.dll"))
            {
                var name = Path.GetFileNameWithoutExtension(dll);
                if (!resolved.ContainsKey(name)) resolved[name] = dll;
            }
        }

        // Follow the package's declared dependencies so transitive BCL/interop types resolve.
        var nuspec = Path.Combine(pkgDir, id.ToLowerInvariant() + ".nuspec");
        if (File.Exists(nuspec))
        {
            try
            {
                var ndoc = XDocument.Load(nuspec);
                foreach (var dep in ndoc.Descendants().Where(e => e.Name.LocalName == "dependency"))
                {
                    var depId = dep.Attribute("id")?.Value;
                    var depVer = dep.Attribute("version")?.Value?.Trim('[', ']', '(', ')').Split(',')[0];
                    if (!string.IsNullOrWhiteSpace(depId) && !string.IsNullOrWhiteSpace(depVer))
                        ResolvePackage(roots, depId!, depVer!, resolved, visited);
                }
            }
            catch { /* best-effort transitive resolution */ }
        }
    }

    private static string? BestLibDir(string pkgDir)
    {
        var lib = Path.Combine(pkgDir, "lib");
        if (!Directory.Exists(lib)) return null;
        var tfms = Directory.GetDirectories(lib);
        // Prefer netstandard2.0 (what the h5 packages ship), else any netstandard, else the first.
        return tfms.FirstOrDefault(d => Path.GetFileName(d).Equals("netstandard2.0", StringComparison.OrdinalIgnoreCase))
            ?? tfms.FirstOrDefault(d => Path.GetFileName(d).StartsWith("netstandard", StringComparison.OrdinalIgnoreCase))
            ?? tfms.FirstOrDefault();
    }

    private static IEnumerable<string> NuGetRoots()
    {
        var env = Environment.GetEnvironmentVariable("NUGET_PACKAGES");
        if (!string.IsNullOrEmpty(env)) yield return env;
        var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        yield return Path.Combine(home, ".nuget", "packages");
        yield return "/root/.nuget/packages";
    }

    private static string? Property(XDocument doc, string name)
        => doc.Descendants().FirstOrDefault(e => e.Name.LocalName == name)?.Value;

    private static LanguageVersion ParseLangVersion(string? v)
    {
        if (string.IsNullOrWhiteSpace(v)) return LanguageVersion.Latest;
        return v.Trim().ToLowerInvariant() switch
        {
            "latest" or "latestmajor" or "preview" or "default" => LanguageVersion.Latest,
            _ => LanguageVersionFacts.TryParse(v, out var parsed) ? parsed : LanguageVersion.Latest,
        };
    }
}
