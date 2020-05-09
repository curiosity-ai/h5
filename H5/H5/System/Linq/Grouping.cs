using System.Collections.Generic;

namespace System.Linq
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.IgnoreGeneric]
    [H5.Convention(Target = H5.ConventionTarget.Member, Member = H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    public interface IGrouping<out TKey, out TElement> : IEnumerable<TElement>
    {
        TKey Key
        {
            [H5.Template("key()")]
            get;
        }
    }

    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.IgnoreGeneric]
    public class Grouping<TKey, TElement> : EnumerableInstance<TElement>, IGrouping<TKey, TElement>
    {
        internal extern Grouping();

        public extern TKey Key
        {
            [H5.Template("key()")]
            get;
        }
    }
}