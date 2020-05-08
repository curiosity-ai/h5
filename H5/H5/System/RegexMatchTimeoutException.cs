namespace System
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    public class RegexMatchTimeoutException : TimeoutException
    {
        public extern string Pattern
        {
            [H5.Template("getPattern()")]
            get;
        }

        public extern string Input
        {
            [H5.Template("getInput()")]
            get;
        }

        public extern TimeSpan MatchTimeout
        {
            [H5.Template("getMatchTimeout()")]
            get;
        }

        public extern RegexMatchTimeoutException();

        public extern RegexMatchTimeoutException(string message);

        public extern RegexMatchTimeoutException(string message, Exception innerException);

        public extern RegexMatchTimeoutException(string regexInput, string regexPattern, TimeSpan matchTimeout);
    }
}