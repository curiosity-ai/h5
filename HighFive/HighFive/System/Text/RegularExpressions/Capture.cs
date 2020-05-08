namespace System.Text.RegularExpressions
{
    /// <summary>
    /// Represents the results from a single successful subexpression capture.
    /// </summary>
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Reflectable]
    public class Capture
    {
        internal extern Capture(string text, int i, int l);

        /// <summary>
        /// The position in the original string where the first character of the captured substring is found.
        /// </summary>
        public extern int Index
        {
            [HighFive.Template("getIndex()")]
            get;
        }

        /// <summary>
        /// Gets the length of the captured substring.
        /// </summary>
        public extern int Length
        {
            [HighFive.Template("getLength()")]
            get;
        }

        /// <summary>
        /// Gets the captured substring from the input string.
        /// </summary>
        public extern string Value
        {
            [HighFive.Template("getValue()")]
            get;
        }

        /// <summary>
        /// Retrieves the captured substring from the input string by calling the Value property. (Overrides Object.ToString().)
        /// </summary>
        public extern override string ToString();
    }
}