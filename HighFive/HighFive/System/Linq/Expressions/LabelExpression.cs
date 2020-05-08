namespace System.Linq.Expressions
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Name("System.Object")]
    [H5.Cast("{this}.ntype === 56")]
    public sealed class LabelExpression : Expression
    {
        [H5.Name("dv")]
        public extern Expression DefaultValue { get; private set; }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern LabelTarget Target { get; private set; }

        internal extern LabelExpression();
    }
}