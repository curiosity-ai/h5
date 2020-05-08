namespace System.Linq.Expressions
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Name("System.Object")]
    public sealed class CatchBlock
    {
        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern ParameterExpression Variable { get; private set; }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern Type Test { get; private set; }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern Expression Body { get; private set; }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern Expression Filter { get; private set; }

        internal extern CatchBlock();
    }
}