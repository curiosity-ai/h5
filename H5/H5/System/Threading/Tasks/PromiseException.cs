namespace System.Threading.Tasks
{
    /// <summary>
    /// This exception is used as the exception for a task created from a promise when the underlying promise fails.
    /// </summary>
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Namespace("H5")]
    public class PromiseException : Exception
    {
        public extern PromiseException(object[] arguments);

        public extern PromiseException(object[] arguments, string message);

        public extern PromiseException(object[] arguments, string message, Exception innerException);

        /// <summary>
        /// Arguments supplied to the promise onError() callback.
        /// </summary>
        [H5.Convention(H5.Notation.CamelCase)]
        public extern object[] Arguments
        {
            get;
        }
    }
}