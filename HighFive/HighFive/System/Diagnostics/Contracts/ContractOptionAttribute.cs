namespace System.Diagnostics.Contracts
{
    /// <summary>
    /// Allows setting contract and tool options at assembly, type, or method granularity.
    /// </summary>
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    [Conditional("CONTRACTS_FULL")]
    [HighFive.External]
    public sealed class ContractOptionAttribute : Attribute
    {
        public extern ContractOptionAttribute(String category, String setting, bool enabled);

        public extern ContractOptionAttribute(String category, String setting, String value);

        public extern String Category
        {
            get;
            private set;
        }

        public extern String Setting
        {
            get;
            private set;
        }

        public extern bool Enabled
        {
            get;
            private set;
        }

        public extern String Value
        {
            get;
            private set;
        }
    }
}