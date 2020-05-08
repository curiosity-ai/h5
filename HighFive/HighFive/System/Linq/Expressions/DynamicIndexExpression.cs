namespace System.Linq.Expressions
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Name("System.Object")]
    [HighFive.Cast("{this}.ntype == 50 && {this}.dtype === 2")]
    public sealed class DynamicIndexExpression : DynamicExpression
    {
        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern Expression Argument { get; private set; }

        internal extern DynamicIndexExpression();
    }
}