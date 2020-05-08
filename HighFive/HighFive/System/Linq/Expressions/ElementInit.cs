using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Linq.Expressions
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Name("System.Object")]
    public sealed class ElementInit
    {
        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern MethodInfo AddMethod { get; private set; }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern ReadOnlyCollection<Expression> Arguments { get; private set; }

        internal extern ElementInit();
    }
}