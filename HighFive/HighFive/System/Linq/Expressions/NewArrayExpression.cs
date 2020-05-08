using System.Collections.ObjectModel;

namespace System.Linq.Expressions
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Name("System.Object")]
    [HighFive.Cast("{this}.ntype === 32 || {this}.ntype === 33")]
    public sealed class NewArrayExpression : Expression
    {
        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern ReadOnlyCollection<Expression> Expressions { get; private set; }

        internal extern NewArrayExpression();
    }
}