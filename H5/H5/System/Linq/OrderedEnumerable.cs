using System.Collections.Generic;

namespace System.Linq
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.IgnoreGeneric]
    [H5.Convention(Target = H5.ConventionTarget.Member, Member = H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    public interface IOrderedEnumerable<TSource> : IEnumerable<TSource>
    {
        [H5.Template("thenBy({keySelector})")]
        IOrderedEnumerable<TSource> ThenBy<TKey>(Func<TSource, TKey> keySelector);

        [H5.Template("thenBy({keySelector}, {comparer})")]
        IOrderedEnumerable<TSource> ThenBy<TKey>(Func<TSource, TKey> keySelector, IComparer<TKey> comparer);

        [H5.Template("thenByDescending({keySelector})")]
        IOrderedEnumerable<TSource> ThenByDescending<TKey>(Func<TSource, TKey> keySelector);

        [H5.Template("thenByDescending({keySelector}, {comparer})")]
        IOrderedEnumerable<TSource> ThenByDescending<TKey>(Func<TSource, TKey> keySelector, IComparer<TKey> comparer);
    }

    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.IgnoreGeneric]
    public class OrderedEnumerable<TElement> : EnumerableInstance<TElement>, IOrderedEnumerable<TElement>
    {
        internal extern OrderedEnumerable();

        extern IOrderedEnumerable<TElement> IOrderedEnumerable<TElement>.ThenBy<TKey>(Func<TElement, TKey> keySelector);

        extern IOrderedEnumerable<TElement> IOrderedEnumerable<TElement>.ThenBy<TKey>(Func<TElement, TKey> keySelector, IComparer<TKey> comparer);

        extern IOrderedEnumerable<TElement> IOrderedEnumerable<TElement>.ThenByDescending<TKey>(Func<TElement, TKey> keySelector);

        extern IOrderedEnumerable<TElement> IOrderedEnumerable<TElement>.ThenByDescending<TKey>(Func<TElement, TKey> keySelector, IComparer<TKey> comparer);

        public extern OrderedEnumerable<TElement> ThenBy<TKey>(Func<TElement, TKey> keySelector);

        public extern OrderedEnumerable<TElement> ThenBy<TKey>(Func<TElement, TKey> keySelector, IComparer<TKey> comparer);

        public extern OrderedEnumerable<TElement> ThenByDescending<TKey>(Func<TElement, TKey> keySelector);

        public extern OrderedEnumerable<TElement> ThenByDescending<TKey>(Func<TElement, TKey> keySelector, IComparer<TKey> comparer);
    }
}