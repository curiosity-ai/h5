using System.Collections.ObjectModel;

namespace System.Linq.Expressions
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Name("System.Object")]
    [HighFive.Cast("{this}.ntype == 50 && {this}.dtype === 1")]
    public sealed class DynamicInvocationExpression : DynamicExpression
    {
        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern ReadOnlyCollection<Expression> Arguments { get; private set; }

        internal extern DynamicInvocationExpression();
    }
}