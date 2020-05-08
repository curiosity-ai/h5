namespace System.Linq.Expressions
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Name("System.Object")]
    [HighFive.Cast("{this}.ntype === 56")]
    public sealed class LabelExpression : Expression
    {
        [HighFive.Name("dv")]
        public extern Expression DefaultValue { get; private set; }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern LabelTarget Target { get; private set; }

        internal extern LabelExpression();
    }
}