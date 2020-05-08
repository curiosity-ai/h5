namespace System.Linq.Expressions
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Name("System.Object")]
    [HighFive.Cast("{this}.btype === 0")]
    public sealed class MemberAssignment : MemberBinding
    {
        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern Expression Expression { get; private set; }

        internal extern MemberAssignment();
    }
}