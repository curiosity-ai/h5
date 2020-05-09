namespace System.Linq.Expressions
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Name("System.Object")]
    [H5.Cast("{this}.ntype === 53")]
    public sealed class GotoExpression : Expression
    {
        [H5.Convention(H5.Notation.CamelCase)]
        public extern GotoExpressionKind Kind { get; private set; }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern Expression Value { get; private set; }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern LabelTarget Target { get; private set; }

        internal extern GotoExpression();
    }
}