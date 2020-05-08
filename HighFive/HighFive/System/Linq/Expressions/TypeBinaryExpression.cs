namespace System.Linq.Expressions
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Name("System.Object")]
    [H5.Cast("{this}.ntype === 45 || {this}.ntype === 81")]
    public sealed class TypeBinaryExpression : Expression
    {
        [H5.Convention(H5.Notation.CamelCase)]
        public extern Expression Expression { get; private set; }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern Type TypeOperand { get; private set; }

        internal extern TypeBinaryExpression();
    }
}