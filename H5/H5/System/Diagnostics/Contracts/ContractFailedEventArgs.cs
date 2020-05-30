namespace System.Diagnostics.Contracts
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Name("System.Object")]
    public sealed class ContractFailedEventArgs : EventArgs
    {
        public extern ContractFailedEventArgs(ContractFailureKind failureKind, string message, string condition, Exception originalException);

        public extern string Message
        {
            get;
        }

        public extern string Condition
        {
            get;
        }

        public extern ContractFailureKind FailureKind
        {
            get;
        }

        public extern Exception OriginalException
        {
            get;
        }

        // Whether the event handler "handles" this contract failure, or to fail via escalation policy.
        public extern bool Handled
        {
            get;
        }

        public extern void SetHandled();

        public extern bool Unwind
        {
            get;
        }

        public extern void SetUnwind();
    }
}