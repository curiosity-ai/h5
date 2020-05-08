namespace System.Linq.Expressions
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Name("System.Object")]
    [HighFive.Cast("{this}.ntype === 8")]
    public sealed class ConditionalExpression : Expression
    {
        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern Expression Test { get; private set; }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern Expression IfTrue { get; private set; }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern Expression IfFalse { get; private set; }

        internal extern ConditionalExpression();
    }
}