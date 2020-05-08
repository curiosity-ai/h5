namespace System.Linq.Expressions
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Name("System.Object")]
    [H5.Cast("{this}.ntype === 8")]
    public sealed class ConditionalExpression : Expression
    {
        [H5.Convention(H5.Notation.CamelCase)]
        public extern Expression Test { get; private set; }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern Expression IfTrue { get; private set; }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern Expression IfFalse { get; private set; }

        internal extern ConditionalExpression();
    }
}