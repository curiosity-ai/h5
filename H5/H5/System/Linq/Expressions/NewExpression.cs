using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Linq.Expressions
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Name("System.Object")]
    [H5.Cast("{this}.ntype === 31")]
    public sealed class NewExpression : Expression
    {
        [H5.Convention(H5.Notation.CamelCase)]
        public new extern ConstructorInfo Constructor { get; private set; }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern ReadOnlyCollection<Expression> Arguments { get; private set; }

        [H5.Name("m")]
        public extern ReadOnlyCollection<MemberInfo> Members { get; private set; }

        internal extern NewExpression();
    }
}