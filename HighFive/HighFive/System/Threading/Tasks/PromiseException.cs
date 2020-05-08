namespace System.Threading.Tasks
{
    /// <summary>
    /// This exception is used as the exception for a task created from a promise when the underlying promise fails.
    /// </summary>
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Namespace("HighFive")]
    public class PromiseException : Exception
    {
        public extern PromiseException(object[] arguments);

        public extern PromiseException(object[] arguments, string message);

        public extern PromiseException(object[] arguments, string message, Exception innerException);

        /// <summary>
        /// Arguments supplied to the promise onError() callback.
        /// </summary>
        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern object[] Arguments
        {
            get;
        }
    }
}