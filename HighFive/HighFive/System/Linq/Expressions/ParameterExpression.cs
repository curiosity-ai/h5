namespace System.Linq.Expressions
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Name("System.Object")]
    [H5.Cast("{this}.ntype === 38")]
    public sealed class ParameterExpression : Expression
    {
        [H5.Name("n")]
        public extern string Name { get; private set; }

        internal extern ParameterExpression();
    }
}