using System.Collections.ObjectModel;

namespace System.Linq.Expressions
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Name("System.Object")]
    [HighFive.Cast("{this}.ntype === 24")]
    public sealed class MemberInitExpression : Expression
    {
        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern NewExpression NewExpression { get; private set; }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern ReadOnlyCollection<MemberBinding> Bindings { get; private set; }

        internal extern MemberInitExpression();
    }
}