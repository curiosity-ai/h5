using System.Collections.ObjectModel;

namespace System.Linq.Expressions
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Name("System.Object")]
    [H5.Cast("{this}.btype === 1")]
    public sealed class MemberMemberBinding : MemberBinding
    {
        [H5.Convention(H5.Notation.CamelCase)]
        public extern ReadOnlyCollection<MemberBinding> Bindings { get; private set; }

        internal extern MemberMemberBinding();
    }
}