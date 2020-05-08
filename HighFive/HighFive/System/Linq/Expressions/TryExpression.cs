using System.Collections.ObjectModel;

namespace System.Linq.Expressions
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Name("System.Object")]
    [H5.Cast("{this}.ntype === 61")]
    public sealed class TryExpression : Expression
    {
        [H5.Convention(H5.Notation.CamelCase)]
        public extern Expression Body { get; private set; }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern ReadOnlyCollection<CatchBlock> Handlers { get; private set; }

        [H5.Name("finallyExpr")]
        public extern Expression Finally { get; private set; }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern Expression Fault { get; private set; }

        internal extern TryExpression();
    }
}