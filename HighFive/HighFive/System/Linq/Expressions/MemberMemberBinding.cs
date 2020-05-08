using System.Collections.ObjectModel;

namespace System.Linq.Expressions
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Name("System.Object")]
    [HighFive.Cast("{this}.btype === 1")]
    public sealed class MemberMemberBinding : MemberBinding
    {
        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern ReadOnlyCollection<MemberBinding> Bindings { get; private set; }

        internal extern MemberMemberBinding();
    }
}