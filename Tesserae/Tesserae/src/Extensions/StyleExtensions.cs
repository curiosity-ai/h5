namespace Tesserae.Components
{
    public static class StyleExtensions
    {
        public static T Background<T>(this T element, string color) where T : IHasBackgroundColor
        {
            element.Background = color;
            return element;
        }

        public static T Foreground<T>(this T element, string color) where T : IHasForegroundColor
        {
            element.Foreground = color;
            return element;
        }

        public static T Padding<T>(this T element, string padding) where T : IHasMarginPadding
        {
            element.Padding = padding;
            return element;
        }

        public static T Margin<T>(this T element, string margin) where T : IHasMarginPadding
        {
            element.Margin = margin;
            return element;
        }
    }
}
