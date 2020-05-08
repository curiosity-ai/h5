namespace System.Linq.Expressions
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Name("System.Object")]
    [HighFive.Cast("{this}.ntype === 9")]
    public sealed class ConstantExpression : Expression
    {
        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern object Value { get; private set; }

        internal extern ConstantExpression();
    }
}