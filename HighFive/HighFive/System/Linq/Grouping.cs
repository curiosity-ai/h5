using System.Collections.Generic;

namespace System.Linq
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.IgnoreGeneric]
    [HighFive.Convention(Target = HighFive.ConventionTarget.Member, Member = HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    public interface IGrouping<out TKey, out TElement> : IEnumerable<TElement>
    {
        TKey Key
        {
            [HighFive.Template("key()")]
            get;
        }
    }

    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.IgnoreGeneric]
    public class Grouping<TKey, TElement> : EnumerableInstance<TElement>, IGrouping<TKey, TElement>
    {
        internal extern Grouping();

        public extern TKey Key
        {
            [HighFive.Template("key()")]
            get;
        }
    }
}