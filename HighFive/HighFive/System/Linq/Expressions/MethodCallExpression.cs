using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Linq.Expressions
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Name("System.Object")]
    [H5.Cast("{this}.ntype === 6")]
    public sealed class MethodCallExpression : Expression
    {
        [H5.Convention(H5.Notation.CamelCase)]
        public extern MethodInfo Method { get; private set; }

        [H5.Name("obj")]
        public extern Expression Object { get; private set; }

        [H5.Name("args")]
        public extern ReadOnlyCollection<Expression> Arguments { get; private set; }

        internal extern MethodCallExpression();
    }
}