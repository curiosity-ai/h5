namespace System.Linq.Expressions
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Name("System.Object")]
    [HighFive.Cast("{this}.ntype === 58")]
    public sealed class LoopExpression : Expression
    {
        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern Expression Body { get; private set; }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern LabelTarget BreakLabel { get; private set; }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern LabelTarget ContinueLabel { get; private set; }

        internal extern LoopExpression();
    }
}