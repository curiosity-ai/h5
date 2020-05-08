using System.Collections.ObjectModel;

namespace System.Linq.Expressions
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Name("System.Object")]
    public sealed class SwitchCase
    {
        [H5.Convention(H5.Notation.CamelCase)]
        public extern ReadOnlyCollection<Expression> TestValues { get; private set; }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern Expression Body { get; private set; }

        internal extern SwitchCase();
    }
}