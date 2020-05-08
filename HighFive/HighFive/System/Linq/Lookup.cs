using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.IgnoreGeneric]
    [H5.Convention(Target = H5.ConventionTarget.Member, Member = H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    public interface ILookup<TKey, TElement> : IEnumerable<Grouping<TKey, TElement>>
    {
        int Count
        {
            [H5.Template("count()")]
            get;
        }

        [H5.AccessorsIndexer]
        EnumerableInstance<TElement> this[TKey key]
        {
            [H5.Template("get({0})")]
            get;
        }

        [H5.Template("contains({key})")]
        bool Contains(TKey key);
    }

    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.IgnoreGeneric]
    public class Lookup<TKey, TElement> : ILookup<TKey, TElement>
    {
        internal extern Lookup();

        public extern int Count
        {
            [H5.Template("count()")]
            get;
        }

        [H5.AccessorsIndexer]
        public extern EnumerableInstance<TElement> this[TKey key]
        {
            [H5.Template("get({0})")]
            get;
        }

        public extern bool Contains(TKey key);

        [H5.Convention(H5.Notation.None)]
        public extern IEnumerator<Grouping<TKey, TElement>> GetEnumerator();

        [H5.Convention(H5.Notation.None)]
        extern IEnumerator IEnumerable.GetEnumerator();
    }
}