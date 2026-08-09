using System.Reflection;
using System.Text;
using System.Text.Json;

namespace H5.Translator.Roslyn.CLI;

/// <summary>
/// Assembles a runnable H5 site from a translation: writes the compiled bundle, extracts the
/// JavaScript each referenced package embeds (h5.js, newtonsoft.json.js, … — listed in the
/// assembly's <c>H5.Resources.json</c>), copies the h5.json resource files (CSS/images), and
/// generates index.html — mirroring what the existing h5 compiler produces.
/// </summary>
internal static class OutputBuilder
{
    // Matches the legacy compiler's HtmlGenerator template.
    private const string HtmlTemplate =
@"<!doctype html>
<html lang=en>
<head>
    <meta charset=""utf-8"" />
    {META}
    <title>{TITLE}</title>
    {CSS}
    {SCRIPT}
    {HEAD}
</head>
<body>
{BODY}
</body>
</html>";

    public static string Build(ResolvedProject project, H5Json config, string javascript, string outputDir, bool minified = false, string? metadataJavascript = null)
    {
        Directory.CreateDirectory(outputDir);

        var runtimeScripts = new List<string>();   // h5.js, newtonsoft.json.js, …
        var libraryScripts = new List<string>();    // tss.js, tss.meta.js, tss-dep.js from referenced projects
        var resourceScripts = new List<string>();   // tss-dep.js, … (this project's own h5.json resources)
        var cssLinks = new List<string>();

        // In separate-assembly mode, referenced *projects* are consumed as DLLs (their JS is
        // extracted, not recompiled) — exclude them from the runtime-package JS loop below.
        var projectDlls = new HashSet<string>(project.ReferencedProjectDlls, StringComparer.OrdinalIgnoreCase);

        // 1. Runtime JS embedded in referenced packages, in dependency order (H5 core first).
        foreach (var dll in OrderRuntimeAssemblies(project.ReferencePaths.Where(p => !projectDlls.Contains(p))))
        {
            foreach (var (fileName, text) in ExtractEmbeddedJs(dll))
            {
                File.WriteAllText(Path.Combine(outputDir, fileName), text);
                runtimeScripts.Add(fileName);
            }
        }

        // The H5R shim (the translator's language-level helpers over h5.js) loads right after
        // the H5 runtime and before any generated code that calls into it.
        File.WriteAllText(Path.Combine(outputDir, "h5r.shim.js"), RoslynTranslator.RuntimeShim);
        runtimeScripts.Add("h5r.shim.js");

        // 1b. Referenced project assemblies: extract their embedded JS/CSS/resources (deepest
        //     dependency first) so a library loads before the app that uses it.
        foreach (var dll in Enumerable.Reverse(project.ReferencedProjectDlls))
            ExtractProjectDllResources(dll, outputDir, libraryScripts, cssLinks, minified);

        // 2. h5.json resource files from every project in the closure — referenced projects
        //    first (a library's JS deps load before the app that uses them). A resource group
        //    whose name is a .js/.css file concatenates its files into that one bundle; other
        //    groups (globbed images, etc.) copy each file through.
        foreach (var projectDir in Enumerable.Reverse(project.ProjectDirs))
        {
            var cfg = projectDir == project.ProjectDir ? config : H5Json.TryLoad(projectDir);
            if (cfg is null) continue;
            // The bundle group names whose files actually resolve in this project — used to
            // decide, for a minified/non-minified pair, which variant the current build takes.
            var bundleNames = cfg.Resources
                .Where(g => g.Files.SelectMany(p => ExpandGlob(projectDir, p)).Any())
                .Select(g => g.Name ?? "")
                .ToHashSet(StringComparer.OrdinalIgnoreCase);
            foreach (var group in cfg.Resources)
                ProcessResourceGroup(projectDir, outputDir, group, resourceScripts, cssLinks, bundleNames, minified);
        }

        // 3. The compiled bundle — loads last, after runtime + library deps are in place.
        File.WriteAllText(Path.Combine(outputDir, config.FileName), javascript);

        var appScripts = new List<string> { config.FileName };

        // 3b. Reflection metadata as a separate file (reflection.target: "file") — loads right
        //     after the bundle whose types it describes, matching the existing compiler.
        if (metadataJavascript is not null)
        {
            var metaName = Path.GetFileNameWithoutExtension(config.FileName) + ".meta.js";
            File.WriteAllText(Path.Combine(outputDir, metaName), metadataJavascript);
            appScripts.Add(metaName);
        }

        var scripts = runtimeScripts.Concat(libraryScripts).Concat(resourceScripts).Concat(appScripts).ToList();

        // 4. index.html.
        if (!config.HtmlDisabled)
            WriteHtml(project, config, outputDir, scripts, cssLinks);

        return outputDir;
    }

    /// <summary>
    /// The resources a library assembly embeds so a referencing project can extract them: the
    /// compiled JS (and its .meta.js) plus every h5.json resource group (bundled or copied),
    /// each tagged with its output subdirectory. Both minified and non-minified resource-group
    /// variants are embedded — the consumer picks per build configuration.
    /// </summary>
    public static List<EmbeddedItem> CollectEmbeddableItems(
        string projectDir, H5Json config, string mainJsName, string javascript, string? metadataJavascript)
    {
        var items = new List<EmbeddedItem>();
        var utf8 = new UTF8Encoding(false);

        items.Add(new EmbeddedItem(mainJsName, utf8.GetBytes(javascript), null));
        if (metadataJavascript is not null)
        {
            var metaName = Path.GetFileNameWithoutExtension(mainJsName) + ".meta.js";
            items.Add(new EmbeddedItem(metaName, utf8.GetBytes(metadataJavascript), null));
        }

        foreach (var group in config.Resources)
        {
            var name = group.Name ?? "";
            var destSub = string.IsNullOrEmpty(group.Output) ? null : group.Output!.Replace('\\', '/');
            var files = group.Files.SelectMany(p => ExpandGlob(projectDir, p)).ToList();
            if (files.Count == 0) continue;

            var isBundle = name.EndsWith(".js", StringComparison.OrdinalIgnoreCase)
                           || name.EndsWith(".css", StringComparison.OrdinalIgnoreCase);
            if (isBundle)
            {
                var bytes = utf8.GetBytes(string.Join("\n", files.Select(File.ReadAllText)));
                items.Add(new EmbeddedItem(name, bytes, destSub));
            }
            else
            {
                // Copy-through group (images, fonts): embed each file under its own name.
                foreach (var src in files)
                    items.Add(new EmbeddedItem(Path.GetFileName(src), File.ReadAllBytes(src), destSub));
            }
        }
        return items;
    }

    /// <summary>
    /// Extracts every resource a referenced project assembly embedded (via its H5.Resources.json
    /// manifest) into the output folder, honouring each entry's output subdirectory and the
    /// Debug/Release minified-variant selection. Adds scripts/CSS links (in manifest order) to the
    /// supplied lists. This is the consuming half of the package protocol.
    /// </summary>
    private static void ExtractProjectDllResources(
        string dllPath, string outputDir, List<string> scripts, List<string> cssLinks, bool minified)
    {
        if (!File.Exists(dllPath)) return;
        Assembly asm;
        try { asm = Assembly.LoadFrom(dllPath); } catch { return; }

        var names = asm.GetManifestResourceNames();
        var manifestName = names.FirstOrDefault(n => n.EndsWith("H5.Resources.json", StringComparison.OrdinalIgnoreCase));
        if (manifestName is null) return;

        List<(string fileName, string? path)> entries;
        using (var ms = asm.GetManifestResourceStream(manifestName)!)
        using (var sr = new StreamReader(ms))
        using (var doc = JsonDocument.Parse(sr.ReadToEnd(), new JsonDocumentOptions { AllowTrailingCommas = true }))
        {
            entries = doc.RootElement.EnumerateArray()
                .Select(e => (
                    fileName: e.TryGetProperty("FileName", out var f) ? f.GetString() : null,
                    path: e.TryGetProperty("Path", out var p) && p.ValueKind == JsonValueKind.String ? p.GetString() : null))
                .Where(x => !string.IsNullOrEmpty(x.fileName))
                .Select(x => (x.fileName!, x.path))
                .ToList();
        }

        // For a minified/non-minified pair, keep only the variant this configuration wants.
        var present = entries.Select(e => e.fileName).ToHashSet(StringComparer.OrdinalIgnoreCase);
        foreach (var (fileName, path) in entries)
        {
            if (IsMinifiedName(fileName) == !minified && present.Contains(CounterpartName(fileName)))
                continue;

            var resName = names.FirstOrDefault(n => n.Equals(fileName, StringComparison.OrdinalIgnoreCase))
                          ?? names.FirstOrDefault(n => n.EndsWith(fileName, StringComparison.OrdinalIgnoreCase));
            if (resName is null) continue;

            var rel = string.IsNullOrEmpty(path) ? fileName : path!.Replace('\\', '/') + "/" + fileName;
            var dest = Path.Combine(outputDir, rel);
            Directory.CreateDirectory(Path.GetDirectoryName(dest)!);
            using (var s = asm.GetManifestResourceStream(resName)!)
            using (var fs = File.Create(dest))
                s.CopyTo(fs);

            if (rel.EndsWith(".css", StringComparison.OrdinalIgnoreCase)) cssLinks.Add(rel);
            else if (rel.EndsWith(".js", StringComparison.OrdinalIgnoreCase)) scripts.Add(rel);
        }
    }

    /// <summary>A resource group name for a minified bundle (ends in <c>.min.js</c>/<c>.min.css</c>).</summary>
    private static bool IsMinifiedName(string name)
        => name.EndsWith(".min.js", StringComparison.OrdinalIgnoreCase)
           || name.EndsWith(".min.css", StringComparison.OrdinalIgnoreCase);

    /// <summary>The paired variant of a bundle name: <c>x.js</c> ↔ <c>x.min.js</c> (and .css).</summary>
    private static string CounterpartName(string name)
    {
        foreach (var ext in new[] { ".js", ".css" })
        {
            if (IsMinifiedName(name) && name.EndsWith(".min" + ext, StringComparison.OrdinalIgnoreCase))
                return name.Substring(0, name.Length - (".min" + ext).Length) + ext;
            if (!IsMinifiedName(name) && name.EndsWith(ext, StringComparison.OrdinalIgnoreCase))
                return name.Substring(0, name.Length - ext.Length) + ".min" + ext;
        }
        return name;
    }

    private static void ProcessResourceGroup(
        string projectDir, string outputDir, H5Json.ResourceGroup group,
        List<string> resourceScripts, List<string> cssLinks, HashSet<string> bundleNames, bool minified)
    {
        var destSub = (group.Output ?? "").Replace('\\', '/');
        var files = group.Files.SelectMany(p => ExpandGlob(projectDir, p)).ToList();
        if (files.Count == 0) return;

        var name = group.Name ?? "";

        // H5 emits resource groups in minified/non-minified pairs (e.g. tss-dep.js and
        // tss-dep.min.js — see the "outputFormatting": "Both" note in Tesserae's h5.json). When
        // BOTH variants of a bundle are available, a referencing project built in Debug takes the
        // non-minified one and a Release build takes the .min.js one. When only one variant
        // exists, it is used regardless of configuration — otherwise both variants of a matched
        // pair would load and the dependency bundle would run twice.
        if (IsMinifiedName(name) == !minified && bundleNames.Contains(CounterpartName(name)))
            return;

        var isBundle = name.EndsWith(".js", StringComparison.OrdinalIgnoreCase)
                       || name.EndsWith(".css", StringComparison.OrdinalIgnoreCase);

        if (isBundle)
        {
            // Concatenate the group's files into a single named output (e.g. tss-dep.js).
            var rel = string.IsNullOrEmpty(destSub) ? name : destSub + "/" + name;
            var dest = Path.Combine(outputDir, rel);
            Directory.CreateDirectory(Path.GetDirectoryName(dest)!);
            File.WriteAllText(dest, string.Join("\n", files.Select(File.ReadAllText)));
            if (name.EndsWith(".css", StringComparison.OrdinalIgnoreCase)) cssLinks.Add(rel);
            else resourceScripts.Add(rel);
        }
        else
        {
            // Copy each file through (images, or JS/CSS referenced by their own file name).
            foreach (var src in files)
            {
                var rel = string.IsNullOrEmpty(destSub) ? Path.GetFileName(src) : destSub + "/" + Path.GetFileName(src);
                var dest = Path.Combine(outputDir, rel);
                Directory.CreateDirectory(Path.GetDirectoryName(dest)!);
                File.Copy(src, dest, overwrite: true);
                if (rel.EndsWith(".css", StringComparison.OrdinalIgnoreCase)) cssLinks.Add(rel);
                else if (rel.EndsWith(".js", StringComparison.OrdinalIgnoreCase)) resourceScripts.Add(rel);
            }
        }
    }

    /// <summary>Orders reference assemblies so the H5 runtime core loads first.</summary>
    private static IEnumerable<string> OrderRuntimeAssemblies(IEnumerable<string> dlls)
        => dlls.OrderBy(d => Path.GetFileName(d).Equals("H5.dll", StringComparison.OrdinalIgnoreCase) ? 0 : 1)
               .ThenBy(d => Path.GetFileName(d), StringComparer.OrdinalIgnoreCase);

    /// <summary>The JS files a package embeds, in the order its H5.Resources.json lists them.</summary>
    private static IEnumerable<(string fileName, string text)> ExtractEmbeddedJs(string dllPath)
    {
        Assembly asm;
        try { asm = Assembly.LoadFrom(dllPath); }
        catch { yield break; }

        var resourceNames = asm.GetManifestResourceNames();
        var manifest = resourceNames.FirstOrDefault(n => n.EndsWith("H5.Resources.json", StringComparison.OrdinalIgnoreCase));
        List<string> order;
        if (manifest is not null)
        {
            using var ms = asm.GetManifestResourceStream(manifest)!;
            using var sr = new StreamReader(ms);
            using var doc = JsonDocument.Parse(sr.ReadToEnd(), new JsonDocumentOptions { AllowTrailingCommas = true });
            order = doc.RootElement.EnumerateArray()
                .Select(e => e.TryGetProperty("FileName", out var f) ? f.GetString() : null)
                .Where(n => !string.IsNullOrEmpty(n)).Select(n => n!).ToList();
        }
        else
        {
            order = resourceNames.Where(n => n.EndsWith(".js", StringComparison.OrdinalIgnoreCase)).ToList();
        }

        foreach (var fileName in order)
        {
            var resName = resourceNames.FirstOrDefault(n => n.Equals(fileName, StringComparison.OrdinalIgnoreCase))
                          ?? resourceNames.FirstOrDefault(n => n.EndsWith(fileName, StringComparison.OrdinalIgnoreCase));
            if (resName is null) continue;
            using var s = asm.GetManifestResourceStream(resName)!;
            using var reader = new StreamReader(s);
            yield return (Path.GetFileName(fileName), reader.ReadToEnd());
        }
    }

    private static IEnumerable<string> ExpandGlob(string baseDir, string pattern)
    {
        pattern = pattern.Replace('\\', '/');
        var dir = Path.GetDirectoryName(pattern) ?? "";
        var file = Path.GetFileName(pattern);
        var searchDir = Path.Combine(baseDir, dir);
        if (!Directory.Exists(searchDir)) return Enumerable.Empty<string>();
        return file.Contains('*')
            ? Directory.EnumerateFiles(searchDir, file, SearchOption.TopDirectoryOnly)
            : File.Exists(Path.Combine(searchDir, file)) ? new[] { Path.Combine(searchDir, file) } : Enumerable.Empty<string>();
    }

    private static void WriteHtml(ResolvedProject project, H5Json config, string outputDir, List<string> scripts, List<string> cssLinks)
    {
        var css = new StringBuilder();
        foreach (var link in cssLinks)
            css.Append("\n    ").Append($"<link rel=\"stylesheet\" href=\"{link}\">");

        var js = new StringBuilder();
        foreach (var script in scripts)
            js.Append("\n    ").Append($"<script src=\"{script}\" defer></script>");

        var html = HtmlTemplate
            .Replace("{META}", config.HtmlMeta)
            .Replace("{TITLE}", config.HtmlTitle ?? project.AssemblyName)
            .Replace("{CSS}", css.ToString().TrimStart())
            .Replace("{SCRIPT}", js.ToString().TrimStart())
            .Replace("{HEAD}", config.HtmlHead)
            .Replace("{BODY}", config.HtmlBody);

        File.WriteAllText(Path.Combine(outputDir, "index.html"), html);
    }
}
