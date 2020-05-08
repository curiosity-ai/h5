using System.Reflection;

namespace System.Linq.Expressions
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Name("System.Object")]
    [HighFive.Cast("{this}.ntype === 23")]
    public sealed class MemberExpression : Expression
    {
        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern MemberInfo Member { get; private set; }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern Expression Expression { get; private set; }

        internal extern MemberExpression();
    }
}