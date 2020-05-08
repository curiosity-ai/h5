namespace System.Diagnostics.Contracts
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    public sealed class ContractException : Exception
    {
        public extern ContractFailureKind Kind
        {
            get;
        }

        public extern string Failure
        {
            get;
        }

        public extern string UserMessage
        {
            get;
        }

        public extern string Condition
        {
            get;
        }

        public extern ContractException(ContractFailureKind kind, string failure, string userMessage, string condition, Exception innerException);
    }
}