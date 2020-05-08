namespace System.Collections.Generic
{
    [HighFive.External]
    [HighFive.Reflectable]
    public interface IReadOnlyDictionary<TKey, TValue> : IReadOnlyCollection<KeyValuePair<TKey, TValue>>,
        IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
    {
        [HighFive.AccessorsIndexer]
        TValue this[TKey key]
        {
            [HighFive.Name("getItem")]
            get;
        }

        IEnumerable<TKey> Keys
        {
            get;
        }

        IEnumerable<TValue> Values
        {
            get;
        }

        bool ContainsKey(TKey key);

        bool TryGetValue(TKey key, out TValue value);
    }
}