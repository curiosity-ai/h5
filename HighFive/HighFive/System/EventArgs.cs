namespace System
{
    /// <summary>
    /// System.EventArgs is the base class for classes containing event data.
    /// </summary>
    [H5.External]
    [H5.Name("System.Object")]
    public class EventArgs
    {
        /// <summary>
        /// Represents an event with no event data.
        /// </summary>
        [H5.Template("{ }")]
        public static readonly EventArgs Empty;

        /// <summary>
        /// Initializes a new instance of the System.EventArgs class.
        /// </summary>
        [H5.Template("{ }")]
        public extern EventArgs();
    }
}