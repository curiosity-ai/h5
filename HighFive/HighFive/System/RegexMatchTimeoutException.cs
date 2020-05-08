namespace System
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    public class RegexMatchTimeoutException : TimeoutException
    {
        public extern string Pattern
        {
            [HighFive.Template("getPattern()")]
            get;
        }

        public extern string Input
        {
            [HighFive.Template("getInput()")]
            get;
        }

        public extern TimeSpan MatchTimeout
        {
            [HighFive.Template("getMatchTimeout()")]
            get;
        }

        public extern RegexMatchTimeoutException();

        public extern RegexMatchTimeoutException(string message);

        public extern RegexMatchTimeoutException(string message, Exception innerException);

        public extern RegexMatchTimeoutException(string regexInput, string regexPattern, TimeSpan matchTimeout);
    }
}