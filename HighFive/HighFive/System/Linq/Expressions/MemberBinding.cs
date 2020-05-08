using System.Reflection;

namespace System.Linq.Expressions
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Name("System.Object")]
    public abstract class MemberBinding
    {
        [HighFive.Name("btype")]
        public extern MemberBindingType BindingType { get; private set; }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern MemberInfo Member { get; private set; }

        internal extern MemberBinding();
    }
}