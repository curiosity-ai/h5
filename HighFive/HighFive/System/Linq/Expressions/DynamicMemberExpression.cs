namespace System.Linq.Expressions
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Name("System.Object")]
    [HighFive.Cast("{this}.ntype == 50 && {this}.dtype === 0")]
    public sealed class DynamicMemberExpression : DynamicExpression
    {
        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern string Member { get; private set; }

        internal extern DynamicMemberExpression();
    }
}