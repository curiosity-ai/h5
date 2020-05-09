namespace Newtonsoft.Json
{
    /// <summary>
    /// Specifies how object creation is handled.
    /// </summary>
    public enum ObjectCreationHandling
    {
        /// <summary>
        /// Reuse existing objects, create new objects when needed.
        /// </summary>
        Auto = 0,

        /// <summary>
        /// Only reuse existing objects.
        /// </summary>
        Reuse = 1,

        /// <summary>
        /// Always create new objects.
        /// </summary>
        Replace = 2
    }
}