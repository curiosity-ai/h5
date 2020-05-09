namespace Tesserae.Components
{
    public static class UnitSizeExtensions
    {
        public static UnitSize percent(this int value) => new UnitSize(value, Unit.Percent);
        public static UnitSize px(this int value) => new UnitSize(value, Unit.Pixels);
        public static UnitSize vh(this int value) => new UnitSize(value, Unit.ViewportHeight);
        public static UnitSize vw(this int value) => new UnitSize(value, Unit.ViewportWidth);

        public static UnitSize percent(this double value) => new UnitSize(value, Unit.Percent);
        public static UnitSize px(this double value) => new UnitSize(value, Unit.Pixels);
        public static UnitSize vh(this double value) => new UnitSize(value, Unit.ViewportHeight);
        public static UnitSize vw(this double value) => new UnitSize(value, Unit.ViewportWidth);

        public static UnitSize percent(this float value) => new UnitSize(value, Unit.Percent);
        public static UnitSize px(this float value) => new UnitSize(value, Unit.Pixels);
        public static UnitSize vh(this float value) => new UnitSize(value, Unit.ViewportHeight);
        public static UnitSize vw(this float value) => new UnitSize(value, Unit.ViewportWidth);
    }
}
