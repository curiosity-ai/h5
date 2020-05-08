using System.Collections.ObjectModel;

namespace System.Linq.Expressions
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Name("System.Object")]
    [H5.Cast("{this}.ntype === 32 || {this}.ntype === 33")]
    public sealed class NewArrayExpression : Expression
    {
        [H5.Convention(H5.Notation.CamelCase)]
        public extern ReadOnlyCollection<Expression> Expressions { get; private set; }

        internal extern NewArrayExpression();
    }
}