namespace System.Linq.Expressions
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Name("System.Object")]
    [H5.Cast("{this}.btype === 0")]
    public sealed class MemberAssignment : MemberBinding
    {
        [H5.Convention(H5.Notation.CamelCase)]
        public extern Expression Expression { get; private set; }

        internal extern MemberAssignment();
    }
}