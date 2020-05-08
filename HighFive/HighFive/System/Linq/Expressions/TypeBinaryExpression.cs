namespace System.Linq.Expressions
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Name("System.Object")]
    [HighFive.Cast("{this}.ntype === 45 || {this}.ntype === 81")]
    public sealed class TypeBinaryExpression : Expression
    {
        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern Expression Expression { get; private set; }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern Type TypeOperand { get; private set; }

        internal extern TypeBinaryExpression();
    }
}