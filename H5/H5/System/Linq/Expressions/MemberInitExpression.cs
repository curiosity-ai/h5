using System.Collections.ObjectModel;

namespace System.Linq.Expressions
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Name("System.Object")]
    [H5.Cast("{this}.ntype === 24")]
    public sealed class MemberInitExpression : Expression
    {
        [H5.Convention(H5.Notation.CamelCase)]
        public extern NewExpression NewExpression { get; private set; }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern ReadOnlyCollection<MemberBinding> Bindings { get; private set; }

        internal extern MemberInitExpression();
    }
}