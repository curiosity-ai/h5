namespace System.Linq.Expressions
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Name("System.Object")]
    [H5.Cast("{this}.ntype === 9")]
    public sealed class ConstantExpression : Expression
    {
        [H5.Convention(H5.Notation.CamelCase)]
        public extern object Value { get; private set; }

        internal extern ConstantExpression();
    }
}