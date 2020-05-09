using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Linq.Expressions
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Name("System.Object")]
    [H5.Cast("{this}.ntype === 55")]
    public sealed class IndexExpression : Expression
    {
        [H5.Name("obj")]
        public extern Expression Object { get; private set; }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern PropertyInfo Indexer { get; private set; }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern ReadOnlyCollection<Expression> Arguments { get; private set; }

        internal extern IndexExpression();
    }
}