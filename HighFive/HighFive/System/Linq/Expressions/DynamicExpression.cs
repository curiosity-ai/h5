namespace System.Linq.Expressions
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Name("System.Object")]
    [HighFive.Cast("{this}.ntype == 50")]
    public abstract class DynamicExpression : Expression
    {
        [HighFive.Name("dtype")]
        public extern DynamicExpressionType DynamicType { get; private set; }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern Expression Expression { get; private set; }

        internal extern DynamicExpression();
    }
}