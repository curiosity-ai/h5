namespace Tesserae.Components
{
    public static class ICanValidateExtensions
    {
        public static T Error<T>(this T component, string error) where T : ICanValidate
        {
            component.Error = error;
            return component;
        }

        public static T IsInvalid<T>(this T component, bool isInvalid = true) where T : ICanValidate
        {
            component.IsInvalid = isInvalid;
            return component;
        }
    }
}