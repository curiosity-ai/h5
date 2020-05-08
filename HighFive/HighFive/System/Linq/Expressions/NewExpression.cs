using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Linq.Expressions
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Name("System.Object")]
    [HighFive.Cast("{this}.ntype === 31")]
    public sealed class NewExpression : Expression
    {
        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public new extern ConstructorInfo Constructor { get; private set; }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern ReadOnlyCollection<Expression> Arguments { get; private set; }

        [HighFive.Name("m")]
        public extern ReadOnlyCollection<MemberInfo> Members { get; private set; }

        internal extern NewExpression();
    }
}