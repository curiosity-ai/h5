namespace System.Linq.Expressions
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Name("System.Object")]
    [H5.Cast("{this}.ntype == 50")]
    public abstract class DynamicExpression : Expression
    {
        [H5.Name("dtype")]
        public extern DynamicExpressionType DynamicType { get; private set; }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern Expression Expression { get; private set; }

        internal extern DynamicExpression();
    }
}