using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.IgnoreGeneric]
    [HighFive.Convention(Target = HighFive.ConventionTarget.Member, Member = HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    public interface ILookup<TKey, TElement> : IEnumerable<Grouping<TKey, TElement>>
    {
        int Count
        {
            [HighFive.Template("count()")]
            get;
        }

        [HighFive.AccessorsIndexer]
        EnumerableInstance<TElement> this[TKey key]
        {
            [HighFive.Template("get({0})")]
            get;
        }

        [HighFive.Template("contains({key})")]
        bool Contains(TKey key);
    }

    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.IgnoreGeneric]
    public class Lookup<TKey, TElement> : ILookup<TKey, TElement>
    {
        internal extern Lookup();

        public extern int Count
        {
            [HighFive.Template("count()")]
            get;
        }

        [HighFive.AccessorsIndexer]
        public extern EnumerableInstance<TElement> this[TKey key]
        {
            [HighFive.Template("get({0})")]
            get;
        }

        public extern bool Contains(TKey key);

        [HighFive.Convention(HighFive.Notation.None)]
        public extern IEnumerator<Grouping<TKey, TElement>> GetEnumerator();

        [HighFive.Convention(HighFive.Notation.None)]
        extern IEnumerator IEnumerable.GetEnumerator();
    }
}