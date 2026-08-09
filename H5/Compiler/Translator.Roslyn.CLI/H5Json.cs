using System.Text.Json;

namespace H5.Translator.Roslyn.CLI;

/// <summary>
/// The subset of a project's <c>h5.json</c> the CLI acts on: where output goes, the bundle
/// file name, HTML generation, and the resource files (CSS/images) copied into the output
/// and linked from index.html. JSONC (comments + trailing commas) is tolerated.
/// </summary>
internal sealed class H5Json
{
    public string? Output { get; init; }
    public string FileName { get; init; } = "app.js";

    /// <summary>The h5.json <c>fileName</c> exactly as written (null when unset). A library with
    /// no explicit fileName outputs &lt;AssemblyName&gt;.js.</summary>
    public string? ExplicitFileName { get; init; }
    public bool HtmlDisabled { get; init; }
    public string? HtmlTitle { get; init; }
    public string HtmlBody { get; init; } = "";
    public string HtmlHead { get; init; } = "";
    public string HtmlMeta { get; init; } = "";
    public List<ResourceGroup> Resources { get; init; } = new();

    /// <summary>reflection.disabled — when true, no reflection metadata is emitted.</summary>
    public bool ReflectionDisabled { get; init; }

    /// <summary>reflection.target — "inline" or "file" (default). Others map to the closest of the two.</summary>
    public string ReflectionTarget { get; init; } = "file";

    internal sealed class ResourceGroup
    {
        public string? Name { get; init; }
        public List<string> Files { get; init; } = new();
        public string? Output { get; init; }
    }

    public static H5Json? TryLoad(string projectDir)
    {
        var path = Path.Combine(projectDir, "h5.json");
        if (!File.Exists(path)) return null;

        var options = new JsonDocumentOptions { CommentHandling = JsonCommentHandling.Skip, AllowTrailingCommas = true };
        using var doc = JsonDocument.Parse(File.ReadAllText(path), options);
        var root = doc.RootElement;

        string? Str(JsonElement e, string name)
            => e.TryGetProperty(name, out var v) && v.ValueKind == JsonValueKind.String ? v.GetString() : null;

        var html = root.TryGetProperty("html", out var h) && h.ValueKind == JsonValueKind.Object ? h : default;
        var reflection = root.TryGetProperty("reflection", out var rfl) && rfl.ValueKind == JsonValueKind.Object ? rfl : default;
        var resources = new List<ResourceGroup>();
        if (root.TryGetProperty("resources", out var res) && res.ValueKind == JsonValueKind.Array)
        {
            foreach (var g in res.EnumerateArray())
            {
                if (g.ValueKind != JsonValueKind.Object) continue;
                var files = new List<string>();
                if (g.TryGetProperty("files", out var fs) && fs.ValueKind == JsonValueKind.Array)
                    files.AddRange(fs.EnumerateArray().Where(x => x.ValueKind == JsonValueKind.String).Select(x => x.GetString()!));
                resources.Add(new ResourceGroup { Name = Str(g, "name"), Files = files, Output = Str(g, "output") });
            }
        }

        return new H5Json
        {
            Output = Str(root, "output"),
            FileName = Str(root, "fileName") ?? "app.js",
            ExplicitFileName = Str(root, "fileName"),
            HtmlDisabled = html.ValueKind == JsonValueKind.Object && html.TryGetProperty("disabled", out var d) && d.ValueKind == JsonValueKind.True,
            HtmlTitle = html.ValueKind == JsonValueKind.Object ? Str(html, "title") : null,
            HtmlBody = (html.ValueKind == JsonValueKind.Object ? Str(html, "body") : null) ?? "",
            HtmlHead = (html.ValueKind == JsonValueKind.Object ? Str(html, "head") : null) ?? "",
            HtmlMeta = (html.ValueKind == JsonValueKind.Object ? Str(html, "meta") : null) ?? "",
            Resources = resources,
            ReflectionDisabled = reflection.ValueKind == JsonValueKind.Object
                && reflection.TryGetProperty("disabled", out var rd) && rd.ValueKind == JsonValueKind.True,
            ReflectionTarget = (reflection.ValueKind == JsonValueKind.Object ? Str(reflection, "target") : null) ?? "file",
        };
    }
}
