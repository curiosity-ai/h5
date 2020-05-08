using System.Collections.ObjectModel;

namespace System.Linq.Expressions
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Name("System.Object")]
    public sealed class SwitchCase
    {
        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern ReadOnlyCollection<Expression> TestValues { get; private set; }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern Expression Body { get; private set; }

        internal extern SwitchCase();
    }
}