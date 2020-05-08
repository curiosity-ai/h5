using System.Collections.ObjectModel;

namespace System.Linq.Expressions
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Name("System.Object")]
    [HighFive.Cast("{this}.ntype === 17")]
    public sealed class InvocationExpression : Expression
    {
        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern Expression Expression { get; private set; }

        [HighFive.Name("args")]
        public extern ReadOnlyCollection<Expression> Arguments { get; private set; }

        internal extern InvocationExpression();
    }
}