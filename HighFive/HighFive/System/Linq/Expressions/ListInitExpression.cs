using System.Collections.ObjectModel;

namespace System.Linq.Expressions
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Name("System.Object")]
    [HighFive.Cast("{this}.ntype === 22")]
    public sealed class ListInitExpression : Expression
    {
        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern NewExpression NewExpression { get; private set; }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern ReadOnlyCollection<ElementInit> Initializers { get; private set; }

        internal extern ListInitExpression();
    }
}