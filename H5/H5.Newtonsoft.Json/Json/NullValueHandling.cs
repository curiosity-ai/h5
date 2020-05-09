namespace Newtonsoft.Json
{
    /// <summary>
    /// Specifies null value handling options for the <see cref="JsonConvert"/>.
    /// </summary>
    public enum NullValueHandling
    {
        /// <summary>
        /// Include null values when serializing and deserializing objects.
        /// </summary>
        Include = 0,

        /// <summary>
        /// Ignore null values when serializing and deserializing objects.
        /// </summary>
        Ignore = 1
    }
}