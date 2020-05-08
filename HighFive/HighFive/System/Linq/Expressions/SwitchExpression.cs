using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Linq.Expressions
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Name("System.Object")]
    [H5.Cast("{this}.ntype === 59")]
    public sealed class SwitchExpression : Expression
    {
        [H5.Convention(H5.Notation.CamelCase)]
        public extern Expression SwitchValue { get; private set; }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern ReadOnlyCollection<SwitchCase> Cases { get; private set; }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern Expression DefaultBody { get; private set; }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern MethodInfo Comparison { get; private set; }

        internal extern SwitchExpression();
    }
}