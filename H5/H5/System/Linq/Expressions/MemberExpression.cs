using System.Reflection;

namespace System.Linq.Expressions
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Name("System.Object")]
    [H5.Cast("{this}.ntype === 23")]
    public sealed class MemberExpression : Expression
    {
        [H5.Convention(H5.Notation.CamelCase)]
        public extern MemberInfo Member { get; private set; }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern Expression Expression { get; private set; }

        internal extern MemberExpression();
    }
}