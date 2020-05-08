namespace System.Linq.Expressions
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Name("System.Object")]
    public sealed class LabelTarget
    {
        [HighFive.Name("n")]
        public extern string Name { get; }

        [HighFive.Name("t")]
        public extern Type Type { get; }

        internal extern LabelTarget();
    }
}