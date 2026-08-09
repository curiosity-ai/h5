using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;

namespace H5.Translator.Roslyn;

/// <summary>
/// Locates the H5 reference assembly (H5.dll) and extracts the embedded H5
/// JavaScript runtime (h5.js) so emitted code can run against the real H5 runtime.
/// </summary>
public static class H5Assemblies
{
    private static string? _h5DllPath;
    private static string? _runtimeJs;
    private static HashSet<int>? _noBodyTokens;

    /// <summary>Path to H5.dll, discovered from the NuGet global-packages cache (overridable).</summary>
    public static string H5DllPath
    {
        get => _h5DllPath ??= Discover();
        set => _h5DllPath = value;
    }

    private static string Discover()
    {
        // Allow explicit override.
        var env = Environment.GetEnvironmentVariable("H5_DLL_PATH");
        if (!string.IsNullOrEmpty(env) && File.Exists(env)) return env;

        var candidates =
            from root in NuGetRoots()
            let pkg = Path.Combine(root, "h5")
            where Directory.Exists(pkg)
            from versionDir in Directory.GetDirectories(pkg)
            let dll = Path.Combine(versionDir, "lib", "netstandard2.0", "H5.dll")
            where File.Exists(dll)
            orderby versionDir
            select dll;

        var found = candidates.LastOrDefault();
        if (found is null)
        {
            throw new InvalidOperationException(
                "Could not locate H5.dll in the NuGet packages cache. Set the H5_DLL_PATH environment variable.");
        }
        return found;
    }

    private static System.Collections.Generic.IEnumerable<string> NuGetRoots()
    {
        var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        yield return Path.Combine(home, ".nuget", "packages");
        var env = Environment.GetEnvironmentVariable("NUGET_PACKAGES");
        if (!string.IsNullOrEmpty(env)) yield return env;
        yield return "/root/.nuget/packages";
    }

    /// <summary>
    /// Metadata tokens of the H5.dll methods that carry no IL body and are not abstract —
    /// i.e. C# <c>extern</c> methods/constructors whose behaviour is supplied by a
    /// hand-written JS runtime file (e.g. <c>Regex</c>). H5's <c>OverloadsCollection</c>
    /// excludes these from a non-external type's overload set, so they receive no
    /// <c>$N</c> suffix (matching the single dispatching name in the hand-written JS).
    /// </summary>
    public static HashSet<int> NoBodyMethodTokens
    {
        get
        {
            if (_noBodyTokens is not null) return _noBodyTokens;
            var set = new HashSet<int>();
            using (var fs = File.OpenRead(H5DllPath))
            using (var pe = new PEReader(fs))
            {
                var mr = pe.GetMetadataReader();
                foreach (var handle in mr.MethodDefinitions)
                {
                    var md = mr.GetMethodDefinition(handle);
                    var isAbstract = (md.Attributes & MethodAttributes.Abstract) != 0;
                    if (md.RelativeVirtualAddress == 0 && !isAbstract)
                        set.Add(MetadataTokens.GetToken(handle));
                }
            }
            return _noBodyTokens = set;
        }
    }

    /// <summary>The embedded H5 JavaScript runtime (h5.js), read once from H5.dll.</summary>
    public static string RuntimeJs
    {
        get
        {
            if (_runtimeJs is not null) return _runtimeJs;
            var asm = Assembly.LoadFrom(H5DllPath);
            var name = asm.GetManifestResourceNames().FirstOrDefault(n => n.Equals("h5.js", StringComparison.OrdinalIgnoreCase))
                ?? asm.GetManifestResourceNames().FirstOrDefault(n => n.EndsWith("h5.js", StringComparison.OrdinalIgnoreCase))
                ?? throw new InvalidOperationException("h5.js resource not found in H5.dll.");
            using var stream = asm.GetManifestResourceStream(name)!;
            using var reader = new StreamReader(stream);
            _runtimeJs = reader.ReadToEnd();
            return _runtimeJs;
        }
    }
}
