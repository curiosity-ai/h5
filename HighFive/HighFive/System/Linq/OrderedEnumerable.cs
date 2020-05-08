using System.Collections.Generic;

namespace System.Linq
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.IgnoreGeneric]
    [HighFive.Convention(Target = HighFive.ConventionTarget.Member, Member = HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    public interface IOrderedEnumerable<TSource> : IEnumerable<TSource>
    {
        [HighFive.Template("thenBy({keySelector})")]
        IOrderedEnumerable<TSource> ThenBy<TKey>(Func<TSource, TKey> keySelector);

        [HighFive.Template("thenBy({keySelector}, {comparer})")]
        IOrderedEnumerable<TSource> ThenBy<TKey>(Func<TSource, TKey> keySelector, IComparer<TKey> comparer);

        [HighFive.Template("thenByDescending({keySelector})")]
        IOrderedEnumerable<TSource> ThenByDescending<TKey>(Func<TSource, TKey> keySelector);

        [HighFive.Template("thenByDescending({keySelector}, {comparer})")]
        IOrderedEnumerable<TSource> ThenByDescending<TKey>(Func<TSource, TKey> keySelector, IComparer<TKey> comparer);
    }

    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.IgnoreGeneric]
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