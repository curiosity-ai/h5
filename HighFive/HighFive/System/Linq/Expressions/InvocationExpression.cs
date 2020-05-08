using System.Collections.ObjectModel;

namespace System.Linq.Expressions
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Name("System.Object")]
    [H5.Cast("{this}.ntype === 17")]
    public sealed class InvocationExpression : Expression
    {
        [H5.Convention(H5.Notation.CamelCase)]
        public extern Expression Expression { get; private set; }

        [H5.Name("args")]
        public extern ReadOnlyCollection<Expression> Arguments { get; private set; }

        internal extern InvocationExpression();
    }
}