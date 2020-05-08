namespace System.Linq.Expressions
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Name("System.Object")]
    public sealed class CatchBlock
    {
        [H5.Convention(H5.Notation.CamelCase)]
        public extern ParameterExpression Variable { get; private set; }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern Type Test { get; private set; }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern Expression Body { get; private set; }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern Expression Filter { get; private set; }

        internal extern CatchBlock();
    }
}