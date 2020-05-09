using System.Collections.ObjectModel;

namespace System.Linq.Expressions
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Name("System.Object")]
    [H5.Cast("{this}.btype === 2")]
    public sealed class MemberListBinding : MemberBinding
    {
        [H5.Convention(H5.Notation.CamelCase)]
        public extern ReadOnlyCollection<ElementInit> Initializers { get; private set; }

        internal extern MemberListBinding();
    }
}