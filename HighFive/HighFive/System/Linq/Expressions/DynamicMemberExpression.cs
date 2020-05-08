namespace System.Linq.Expressions
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Name("System.Object")]
    [H5.Cast("{this}.ntype == 50 && {this}.dtype === 0")]
    public sealed class DynamicMemberExpression : DynamicExpression
    {
        [H5.Convention(H5.Notation.CamelCase)]
        public extern string Member { get; private set; }

        internal extern DynamicMemberExpression();
    }
}