using System.Collections.ObjectModel;

namespace System.Linq.Expressions
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Name("System.Object")]
    [HighFive.Cast("{this}.ntype === 47")]
    public sealed class BlockExpression : Expression
    {
        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern ReadOnlyCollection<Expression> Expressions { get; private set; }

        public extern ReadOnlyCollection<ParameterExpression> Variables
        {
            [HighFive.Template("({this}.variables || HighFive.toList([]))")] get;
            private set;
        }

        public extern Expression Result
        {
            [HighFive.Template("{this}.expressions.getItem({this}.expressions.Count - 1)")]
            get;
            private set;
        }

        internal extern BlockExpression();
    }
}