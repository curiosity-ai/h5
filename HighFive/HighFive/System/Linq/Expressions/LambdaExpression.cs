using System.Collections.ObjectModel;

namespace System.Linq.Expressions
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Name("System.Object")]
    [HighFive.Cast("{this}.ntype === 18")]
    public abstract class LambdaExpression : Expression
    {
        [HighFive.Name("p")]
        public extern ReadOnlyCollection<ParameterExpression> Parameters { get; private set; }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern Expression Body { get; private set; }

        [HighFive.Name("rt")]
        public extern Expression ReturnType { get; private set; }

        internal extern LambdaExpression();
    }
}