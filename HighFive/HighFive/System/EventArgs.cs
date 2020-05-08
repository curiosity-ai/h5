namespace System
{
    /// <summary>
    /// System.EventArgs is the base class for classes containing event data.
    /// </summary>
    [HighFive.External]
    [HighFive.Name("System.Object")]
    public class EventArgs
    {
        /// <summary>
        /// Represents an event with no event data.
        /// </summary>
        [HighFive.Template("{ }")]
        public static readonly EventArgs Empty;

        /// <summary>
        /// Initializes a new instance of the System.EventArgs class.
        /// </summary>
        [HighFive.Template("{ }")]
        public extern EventArgs();
    }
}