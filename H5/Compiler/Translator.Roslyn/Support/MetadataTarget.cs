namespace H5.Translator.Roslyn;

/// <summary>
/// Where reflection metadata (H5.setMetadata registrations) is written, mirroring the
/// existing compiler's <c>reflection.target</c> h5.json setting.
/// </summary>
public enum MetadataTarget
{
    /// <summary>A separate <c>&lt;name&gt;.meta.js</c> file (the h5 default for libraries).</summary>
    File,

    /// <summary>Inline, inside the same assembly function as the generated types.</summary>
    Inline,

    /// <summary>Reserved (per-type metadata) — treated as Inline here.</summary>
    Type,

    /// <summary>Reserved (single assembly metadata) — treated as File here.</summary>
    Assembly,
}
