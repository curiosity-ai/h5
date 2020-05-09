using System.Collections.ObjectModel;

namespace System.Linq.Expressions
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Name("System.Object")]
    [H5.Cast("{this}.ntype === 18")]
    public abstract class LambdaExpression : Expression
    {
        [H5.Name("p")]
        public extern ReadOnlyCollection<ParameterExpression> Parameters { get; private set; }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern Expression Body { get; private set; }

        [H5.Name("rt")]
        public extern Expression ReturnType { get; private set; }

        internal extern LambdaExpression();
    }
}