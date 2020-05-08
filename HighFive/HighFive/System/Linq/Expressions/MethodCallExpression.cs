using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Linq.Expressions
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Name("System.Object")]
    [HighFive.Cast("{this}.ntype === 6")]
    public sealed class MethodCallExpression : Expression
    {
        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern MethodInfo Method { get; private set; }

        [HighFive.Name("obj")]
        public extern Expression Object { get; private set; }

        [HighFive.Name("args")]
        public extern ReadOnlyCollection<Expression> Arguments { get; private set; }

        internal extern MethodCallExpression();
    }
}