namespace System.Linq.Expressions
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Name("System.Object")]
    [HighFive.Cast("{this}.ntype === 38")]
    public sealed class ParameterExpression : Expression
    {
        [HighFive.Name("n")]
        public extern string Name { get; private set; }

        internal extern ParameterExpression();
    }
}