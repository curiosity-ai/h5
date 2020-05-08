using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Linq.Expressions
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Name("System.Object")]
    [HighFive.Cast("{this}.ntype === 59")]
    public sealed class SwitchExpression : Expression
    {
        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern Expression SwitchValue { get; private set; }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern ReadOnlyCollection<SwitchCase> Cases { get; private set; }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern Expression DefaultBody { get; private set; }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern MethodInfo Comparison { get; private set; }

        internal extern SwitchExpression();
    }
}