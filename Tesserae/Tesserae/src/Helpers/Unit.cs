using H5;

namespace Tesserae
{
    [Enum(Emit.Value)]
    public enum Unit
    {
        Default,

        [Name("auto")]
        Auto,

        [Name("%")]
        Percent,

        [Name("px")]
        Pixels,

        [Name("vh")]
        ViewportHeight,

        [Name("vw")]
        ViewportWidth,

        [Name("inherit")]
        Inherit,
    }
}
