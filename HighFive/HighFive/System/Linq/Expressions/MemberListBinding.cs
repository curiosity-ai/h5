using System.Collections.ObjectModel;

namespace System.Linq.Expressions
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Name("System.Object")]
    [HighFive.Cast("{this}.btype === 2")]
    public sealed class MemberListBinding : MemberBinding
    {
        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern ReadOnlyCollection<ElementInit> Initializers { get; private set; }

        internal extern MemberListBinding();
    }
}