namespace System.Linq.Expressions
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Name("System.Object")]
    public sealed class LabelTarget
    {
        [H5.Name("n")]
        public extern string Name { get; }

        [H5.Name("t")]
        public extern Type Type { get; }

        internal extern LabelTarget();
    }
}