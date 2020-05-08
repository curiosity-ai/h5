namespace System.Text.RegularExpressions
{
    /// <summary>
    /// Represents the results from a single successful subexpression capture.
    /// </summary>
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Reflectable]
    public class Capture
    {
        internal extern Capture(string text, int i, int l);

        /// <summary>
        /// The position in the original string where the first character of the captured substring is found.
        /// </summary>
        public extern int Index
        {
            [H5.Template("getIndex()")]
            get;
        }

        /// <summary>
        /// Gets the length of the captured substring.
        /// </summary>
        public extern int Length
        {
            [H5.Template("getLength()")]
            get;
        }

        /// <summary>
        /// Gets the captured substring from the input string.
        /// </summary>
        public extern string Value
        {
            [H5.Template("getValue()")]
            get;
        }

        /// <summary>
        /// Retrieves the captured substring from the input string by calling the Value property. (Overrides Object.ToString().)
        /// </summary>
        public extern override string ToString();
    }
}