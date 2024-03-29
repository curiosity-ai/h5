namespace System.Diagnostics.Contracts
{
    /// <summary>
    /// Allows a field f to be used in the method contracts for a method m when f has less visibility than m.
    /// For instance, if the method is public, but the field is private.
    /// </summary>
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [Conditional("CONTRACTS_FULL")]
    [AttributeUsage(AttributeTargets.Field)]
    [H5.External]
    public sealed class ContractPublicPropertyNameAttribute : Attribute
    {
        public extern ContractPublicPropertyNameAttribute(string name);

        public extern string Name
        {
            get;
            private set;
        }
    }
}