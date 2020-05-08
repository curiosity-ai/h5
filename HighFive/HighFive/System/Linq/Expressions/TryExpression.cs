using System.Collections.ObjectModel;

namespace System.Linq.Expressions
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Name("System.Object")]
    [HighFive.Cast("{this}.ntype === 61")]
    public sealed class TryExpression : Expression
    {
        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern Expression Body { get; private set; }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern ReadOnlyCollection<CatchBlock> Handlers { get; private set; }

        [HighFive.Name("finallyExpr")]
        public extern Expression Finally { get; private set; }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern Expression Fault { get; private set; }

        internal extern TryExpression();
    }
}