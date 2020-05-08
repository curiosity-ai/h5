namespace System.Linq.Expressions
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Name("System.Object")]
    [HighFive.Cast("{this}.ntype === 53")]
    public sealed class GotoExpression : Expression
    {
        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern GotoExpressionKind Kind { get; private set; }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern Expression Value { get; private set; }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern LabelTarget Target { get; private set; }

        internal extern GotoExpression();
    }
}