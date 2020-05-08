namespace System.Collections.Generic
{
    [H5.External]
    [H5.Reflectable]
    public interface IReadOnlyDictionary<TKey, TValue> : IReadOnlyCollection<KeyValuePair<TKey, TValue>>,
        IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
    {
        [H5.AccessorsIndexer]
        TValue this[TKey key]
        {
            [H5.Name("getItem")]
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