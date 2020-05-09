namespace Newtonsoft.Json
{
    /// <summary>
    /// Specifies type name handling options for the <see cref="JsonConvert"/>.
    /// </summary>
    public enum Formatting
    {
        /// <summary>
        /// No special formatting is applied. This is the default.
        /// </summary>
        None = 0,

        /// <summary>
        /// Causes child objects to be indented according to the Indentation and IndentChar settings.
        /// </summary>
        Indented = 1
    }
}