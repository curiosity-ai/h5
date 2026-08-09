using System.Text;
using System.Text.Json;
using Mono.Cecil;

namespace H5.Translator.Roslyn.CLI;

/// <summary>One embedded H5 resource: the file bytes plus its manifest entry. <see cref="Output"/>
/// is the output subdirectory a consumer extracts it to (null for a top-level script).</summary>
internal sealed record EmbeddedItem(string Name, byte[] Content, string? Output);

/// <summary>
/// Embeds the compiled JavaScript (and h5.json resource files) into a .NET assembly as private
/// manifest resources, alongside an <c>H5.Resources.json</c> manifest listing them — exactly the
/// shape the existing compiler produces and that <see cref="OutputBuilder"/> extracts when the
/// assembly is referenced. This is the "produce a distributable package" half of the protocol.
/// </summary>
internal static class ResourceEmbedder
{
    private const string ManifestName = "H5.Resources.json";
    private static readonly UTF8Encoding Utf8NoBom = new(false);

    /// <summary>True if the assembly already carries the embedded H5 resource manifest — i.e. it
    /// was produced as an H5 package (with its JS embedded), not a plain csc build. Used to decide
    /// whether a referenced project's DLL needs to be (re)built by the translator.</summary>
    public static bool HasManifest(string assemblyPath)
    {
        try
        {
            using var asm = AssemblyDefinition.ReadAssembly(assemblyPath);
            return asm.MainModule.Resources.Any(r => r.Name == ManifestName);
        }
        catch { return false; }
    }

    public static void Embed(string assemblyPath, IReadOnlyList<EmbeddedItem> items)
    {
        // Read → modify → write back the assembly in place (ReadWrite so we can overwrite it).
        using var asm = AssemblyDefinition.ReadAssembly(assemblyPath, new ReaderParameters { ReadWrite = true });
        var resources = asm.MainModule.Resources;

        void Replace(string name, byte[] bytes)
        {
            for (var i = resources.Count - 1; i >= 0; i--)
                if (resources[i].Name == name) resources.RemoveAt(i);
            resources.Add(new EmbeddedResource(name, ManifestResourceAttributes.Private, bytes));
        }

        foreach (var item in items)
            Replace(item.Name, item.Content);

        // The manifest lists { FileName, Name, Path } per resource (Parts omitted — we don't
        // split combined bundles). Serialized with the same field names the compiler reads.
        var manifest = items.Select(i => new
        {
            FileName = i.Name,
            Name = i.Name,
            Path = i.Output,
            Parts = (object?)null,
        }).ToArray();
        var json = JsonSerializer.Serialize(manifest, new JsonSerializerOptions { WriteIndented = true });
        Replace(ManifestName, Utf8NoBom.GetBytes(json));

        asm.Write();
    }
}
