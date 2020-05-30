namespace System.Diagnostics.Contracts
{
    /// <summary>
    /// Allows setting contract and tool options at assembly, type, or method granularity.
    /// </summary>
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    [Conditional("CONTRACTS_FULL")]
    [H5.External]
    public sealed class ContractOptionAttribute : Attribute
    {
        public extern ContractOptionAttribute(string category, string setting, bool enabled);

        public extern ContractOptionAttribute(string category, string setting, string value);

        public extern string Category
        {
            get;
            private set;
        }

        public extern string Setting
        {
            get;
            private set;
        }

        public extern bool Enabled
        {
            get;
            private set;
        }

        public extern string Value
        {
            get;
            private set;
        }
    }
}