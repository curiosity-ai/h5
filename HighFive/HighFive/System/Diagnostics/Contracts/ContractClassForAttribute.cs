namespace System.Diagnostics.Contracts
{
    /// <summary>
    /// Types marked with this attribute specify that they are a contract for the type that is the argument of the constructor.
    /// </summary>
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [Conditional("CONTRACTS_FULL")]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    [H5.External]
    public sealed class ContractClassForAttribute : Attribute
    {
        public extern ContractClassForAttribute(Type typeContractsAreFor);

        public extern Type TypeContractsAreFor
        {
            get;
            private set;
        }
    }
}