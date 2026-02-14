using System.Collections;
using System.Collections.Generic;

namespace H5.Core
{
    public static partial class es5
    {
        [IgnoreCast]
        [Name("Map")]
        public class Map<K, V> : IEnumerable<object[]>, IEnumerable, IH5Class, IObject
        {
            public extern Map();
            public extern Map(object[][] entries);
            public extern Map(IEnumerable<object[]> entries);

            public virtual int size { get; }

            public virtual extern void clear();
            public virtual extern bool delete(K key);
            public virtual extern void forEach(MapCallback<K, V> callback);
            public virtual extern void forEach(MapCallback<K, V> callback, object thisArg);
            public virtual extern V get(K key);
            public virtual extern bool has(K key);
            public virtual extern es5.Map<K, V> set(K key, V value);

            [Name("entries")]
            public virtual extern es5.Iterator<object[]> entries();
            [Name("keys")]
            public virtual extern es5.Iterator<K> keys();
            [Name("values")]
            public virtual extern es5.Iterator<V> values();

            public virtual extern IEnumerator<object[]> GetEnumerator();
            extern IEnumerator IEnumerable.GetEnumerator();
        }

        [Generated]
        public delegate void MapCallback<K, V>(V value, K key, es5.Map<K, V> map);

        [IgnoreCast]
        [Name("Iterator")]
        public class Iterator<T> : IObject
        {
            public virtual extern IteratorResult<T> next();
        }

        [IgnoreCast]
        [ObjectLiteral]
        public class IteratorResult<T> : IObject
        {
            public bool done { get; set; }
            public T value { get; set; }
        }
    }
}
