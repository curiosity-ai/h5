using System.Reflection;

namespace System.Linq.Expressions
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Name("System.Object")]
    public abstract class MemberBinding
    {
        [H5.Name("btype")]
        public extern MemberBindingType BindingType { get; private set; }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern MemberInfo Member { get; private set; }

        internal extern MemberBinding();
    }
}