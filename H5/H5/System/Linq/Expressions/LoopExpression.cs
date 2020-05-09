namespace System.Linq.Expressions
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Name("System.Object")]
    [H5.Cast("{this}.ntype === 58")]
    public sealed class LoopExpression : Expression
    {
        [H5.Convention(H5.Notation.CamelCase)]
        public extern Expression Body { get; private set; }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern LabelTarget BreakLabel { get; private set; }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern LabelTarget ContinueLabel { get; private set; }

        internal extern LoopExpression();
    }
}