using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Linq.Expressions
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Name("System.Object")]
    [HighFive.Cast("{this}.ntype === 55")]
    public sealed class IndexExpression : Expression
    {
        [HighFive.Name("obj")]
        public extern Expression Object { get; private set; }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern PropertyInfo Indexer { get; private set; }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern ReadOnlyCollection<Expression> Arguments { get; private set; }

        internal extern IndexExpression();
    }
}