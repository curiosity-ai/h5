namespace System.Diagnostics.Contracts
{
    /// <summary>
    /// Types marked with this attribute specify that a separate type contains the contracts for this type.
    /// </summary>
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [Conditional("CONTRACTS_FULL")]
    [Conditional("DEBUG")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
    [H5.External]
    public sealed class ContractClassAttribute : Attribute
    {
        public extern ContractClassAttribute(Type typeContainingContracts);

        public extern Type TypeContainingContracts
        {
            get;
            private set;
        }
    }
}