namespace System.Collections.Generic
{
    using System;

    public class SortedList<TKey, TValue> :
        IDictionary<TKey, TValue>, System.Collections.IDictionary, IReadOnlyDictionary<TKey, TValue>
    {
        private TKey[] keys;
        private TValue[] values;
        private int _size;
        private int version;
        private IComparer<TKey> comparer;
        private KeyList keyList;
        private ValueList valueList;

        static TKey[] emptyKeys = new TKey[0];
        static TValue[] emptyValues = new TValue[0];

        private const int _defaultCapacity = 4;

        // Constructs a new sorted list. The sorted list is initially empty and has
        // a capacity of zero. Upon adding the first element to the sorted list the
        // capacity is increased to _defaultCapacity, and then increased in multiples of two as
        // required. The elements of the sorted list are ordered according to the
        // IComparable interface, which must be implemented by the keys of
        // all entries added to the sorted list.
        public SortedList()
        {
            keys = emptyKeys;
            values = emptyValues;
            _size = 0;
            comparer = Comparer<TKey>.Default;
        }

        // Constructs a new sorted list. The sorted list is initially empty and has
        // a capacity of zero. Upon adding the first element to the sorted list the
        // capacity is increased to 16, and then increased in multiples of two as
        // required. The elements of the sorted list are ordered according to the
        // IComparable interface, which must be implemented by the keys of
        // all entries added to the sorted list.
        //
        public SortedList(int capacity)
        {
            if (capacity < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.capacity);
            keys = new TKey[capacity];
            values = new TValue[capacity];
            comparer = Comparer<TKey>.Default;
        }

        // Constructs a new sorted list with a given IComparer
        // implementation. The sorted list is initially empty and has a capacity of
        // zero. Upon adding the first element to the sorted list the capacity is
        // increased to 16, and then increased in multiples of two as required. The
        // elements of the sorted list are ordered according to the given
        // IComparer implementation. If comparer is null, the
        // elements are compared to each other using the IComparable
        // interface, which in that case must be implemented by the keys of all
        // entries added to the sorted list.
        //
        public SortedList(IComparer<TKey> comparer)
            : this()
        {
            if (comparer != null)
            {
                this.comparer = comparer;
            }
        }

        // Constructs a new sorted dictionary with a given IComparer
        // implementation and a given initial capacity. The sorted list is
        // initially empty, but will have room for the given number of elements
        // before any reallocations are required. The elements of the sorted list
        // are ordered according to the given IComparer implementation. If
        // comparer is null, the elements are compared to each other using
        // the IComparable interface, which in that case must be implemented
        // by the keys of all entries added to the sorted list.
        //
        public SortedList(int capacity, IComparer<TKey> comparer)
            : this(comparer)
        {
            Capacity = capacity;
        }

        // Constructs a new sorted list containing a copy of the entries in the
        // given dictionary. The elements of the sorted list are ordered according
        // to the IComparable interface, which must be implemented by the
        // keys of all entries in the the given dictionary as well as keys
        // subsequently added to the sorted list.
        //
        public SortedList(IDictionary<TKey, TValue> dictionary)
            : this(dictionary, null)
        {
        }

        // Constructs a new sorted list containing a copy of the entries in the
        // given dictionary. The elements of the sorted list are ordered according
        // to the given IComparer implementation. If comparer is
        // null, the elements are compared to each other using the
        // IComparable interface, which in that case must be implemented
        // by the keys of all entries in the the given dictionary as well as keys
        // subsequently added to the sorted list.
        //
        public SortedList(IDictionary<TKey, TValue> dictionary, IComparer<TKey> comparer)
            : this((dictionary != null ? dictionary.Count : 0), comparer)
        {
            if (dictionary == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.dictionary);

            dictionary.Keys.CopyTo(keys, 0);
            dictionary.Values.CopyTo(values, 0);
            Array.Sort<TKey, TValue>(keys, values, this.comparer);
            _size = dictionary.Count;
        }

        // Adds an entry with the given key and value to this sorted list. An
        // ArgumentException is thrown if the key is already present in the sorted list.
        //
        public void Add(TKey key, TValue value)
        {
            if (key == null) ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
            int i = Array.BinarySearch<TKey>(keys, 0, _size, key, comparer);
            if (i >= 0)
                ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_AddingDuplicate);
            Insert(~i, key, value);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> keyValuePair)
        {
            Add(keyValuePair.Key, keyValuePair.Value);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> keyValuePair)
        {
            int index = IndexOfKey(keyValuePair.Key);
            if (index >= 0 && EqualityComparer<TValue>.Default.Equals(values[index], keyValuePair.Value))
            {
                return true;
            }
            return false;
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> keyValuePair)
        {
            int index = IndexOfKey(keyValuePair.Key);
            if (index >= 0 && EqualityComparer<TValue>.Default.Equals(values[index], keyValuePair.Value))
            {
                RemoveAt(index);
                return true;
            }
            return false;
        }

        // Returns the capacity of this sorted list. The capacity of a sorted list
        // represents the allocated length of the internal arrays used to store the
        // keys and values of the list, and thus also indicates the maximum number
        // of entries the list can contain before a reallocation of the internal
        // arrays is required.
        //
        public int Capacity
        {
            get
            {
                return keys.Length;
            }
            set
            {
                if (value != keys.Length)
                {
                    if (value < _size)
                    {
                        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.value, ExceptionResource.ArgumentOutOfRange_SmallCapacity);
                    }

                    if (value > 0)
                    {
                        TKey[] newKeys = new TKey[value];
                        TValue[] newValues = new TValue[value];
                        if (_size > 0)
                        {
                            Array.Copy(keys, 0, newKeys, 0, _size);
                            Array.Copy(values, 0, newValues, 0, _size);
                        }
                        keys = newKeys;
                        values = newValues;
                    }
                    else
                    {
                        keys = emptyKeys;
                        values = emptyValues;
                    }
                }
            }
        }

        public IComparer<TKey> Comparer
        {
            get
            {
                return comparer;
            }
        }

        void System.Collections.IDictionary.Add(object key, object value)
        {
            if (key == null)
            {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
            }

            ThrowHelper.IfNullAndNullsAreIllegalThenThrow<TValue>(value, ExceptionArgument.value);

            try
            {
                TKey tempKey = (TKey)key;

                try
                {
                    Add(tempKey, (TValue)value);
                }
                catch (InvalidCastException)
                {
                    ThrowHelper.ThrowWrongValueTypeArgumentException(value, typeof(TValue));
                }
            }
            catch (InvalidCastException)
            {
                ThrowHelper.ThrowWrongKeyTypeArgumentException(key, typeof(TKey));
            }
        }

        // Returns the number of entries in this sorted list.
        //
        public int Count
        {
            get
            {
                return _size;
            }
        }

        // Returns a collection representing the keys of this sorted list. This
        // method returns the same object as GetKeyList, but typed as an
        // ICollection instead of an IList.
        //
        public IList<TKey> Keys
        {
            get
            {
                return GetKeyListHelper();
            }
        }

        ICollection<TKey> IDictionary<TKey, TValue>.Keys
        {
            get
            {
                return GetKeyListHelper();
            }
        }

        System.Collections.ICollection System.Collections.IDictionary.Keys
        {
            get
            {
                return GetKeyListHelper();
            }
        }

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
        {
            get
            {
                return GetKeyListHelper();
            }
        }

        // Returns a collection representing the values of this sorted list. This
        // method returns the same object as GetValueList, but typed as an
        // ICollection instead of an IList.
        //
        public IList<TValue> Values
        {
            get
            {
                return GetValueListHelper();
            }
        }

        ICollection<TValue> IDictionary<TKey, TValue>.Values
        {
            get
            {
                return GetValueListHelper();
            }
        }

        System.Collections.ICollection System.Collections.IDictionary.Values
        {
            get
            {
                return GetValueListHelper();
            }
        }

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
        {
            get
            {
                return GetValueListHelper();
            }
        }

        private KeyList GetKeyListHelper()
        {
            if (keyList == null)
                keyList = new KeyList(this);
            return keyList;
        }

        private ValueList GetValueListHelper()
        {
            if (valueList == null)
                valueList = new ValueList(this);
            return valueList;
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get { return false; }
        }

        bool System.Collections.IDictionary.IsReadOnly
        {
            get { return false; }
        }

        bool System.Collections.IDictionary.IsFixedSize
        {
            get { return false; }
        }

        bool System.Collections.ICollection.IsSynchronized
        {
            get { return false; }
        }

        // Synchronization root for this object.
        object System.Collections.ICollection.SyncRoot
        {
            get
            {
                return null;
            }
        }

        // Removes all entries from this sorted list.
        public void Clear()
        {
            // clear does not change the capacity
            version++;
            // Don't need to doc this but we clear the elements so that the gc can reclaim the references.
            Array.Clear(keys, 0, _size);
            Array.Clear(values, 0, _size);
            _size = 0;
        }


        bool System.Collections.IDictionary.Contains(object key)
        {
            if (IsCompatibleKey(key))
            {
                return ContainsKey((TKey)key);
            }
            return false;
        }

        // Checks if this sorted list contains an entry with the given key.
        //
        public bool ContainsKey(TKey key)
        {
            return IndexOfKey(key) >= 0;
        }

        // Checks if this sorted list contains an entry with the given value. The
        // values of the entries of the sorted list are compared to the given value
        // using the Object.Equals method. This method performs a linear
        // search and is substantially slower than the Contains
        // method.
        //
        public bool ContainsValue(TValue value)
        {
            return IndexOfValue(value) >= 0;
        }

        // Copies the values in this SortedList to an array.
        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null)
            {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
            }

            if (arrayIndex < 0 || arrayIndex > array.Length)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.arrayIndex, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
            }

            if (array.Length - arrayIndex < Count)
            {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
            }

            for (int i = 0; i < Count; i++)
            {
                KeyValuePair<TKey, TValue> entry = new KeyValuePair<TKey, TValue>(keys[i], values[i]);
                array[arrayIndex + i] = entry;
            }
        }

        void System.Collections.ICollection.CopyTo(Array array, int arrayIndex)
        {
            if (array == null)
            {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
            }

            if (array.Rank != 1)
            {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
            }

            if (array.GetLowerBound(0) != 0)
            {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NonZeroLowerBound);
            }

            if (arrayIndex < 0 || arrayIndex > array.Length)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.arrayIndex, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
            }

            if (array.Length - arrayIndex < Count)
            {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
            }

            if (array is KeyValuePair<TKey, TValue>[] keyValuePairArray)
            {
                for (int i = 0; i < Count; i++)
                {
                    keyValuePairArray[i + arrayIndex] = new KeyValuePair<TKey, TValue>(keys[i], values[i]);
                }
            }
            else
            {
                object[] objects = array as object[];
                if (objects == null)
                {
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
                }

                try
                {
                    for (int i = 0; i < Count; i++)
                    {
                        objects[i + arrayIndex] = new KeyValuePair<TKey, TValue>(keys[i], values[i]);
                    }
                }
                catch (ArrayTypeMismatchException)
                {
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
                }

            }
        }

        private const int MaxArrayLength = 0X7FEFFFFF;

        // Ensures that the capacity of this sorted list is at least the given
        // minimum value. If the currect capacity of the list is less than
        // min, the capacity is increased to twice the current capacity or
        // to min, whichever is larger.
        private void EnsureCapacity(int min)
        {
            int newCapacity = keys.Length == 0 ? _defaultCapacity : keys.Length * 2;
            // Allow the list to grow to maximum possible capacity (~2G elements) before encountering overflow.
            // Note that this check works even when _items.Length overflowed thanks to the (uint) cast
            if ((uint)newCapacity > MaxArrayLength) newCapacity = MaxArrayLength;
            if (newCapacity < min) newCapacity = min;
            Capacity = newCapacity;
        }

        // Returns the value of the entry at the given index.
        //
        private TValue GetByIndex(int index)
        {
            if (index < 0 || index >= _size)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_Index);
            return values[index];
        }


        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new Enumerator(this, Enumerator.KeyValuePair);
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return new Enumerator(this, Enumerator.KeyValuePair);
        }

        System.Collections.IDictionaryEnumerator System.Collections.IDictionary.GetEnumerator()
        {
            return new Enumerator(this, Enumerator.DictEntry);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new Enumerator(this, Enumerator.KeyValuePair);
        }


        // Returns the key of the entry at the given index.
        //
        private TKey GetKey(int index)
        {
            if (index < 0 || index >= _size) ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_Index);
            return keys[index];
        }


        // Returns the value associated with the given key. If an entry with the
        // given key is not found, the returned value is null.
        //
        public TValue this[TKey key]
        {
            get
            {
                int i = IndexOfKey(key);
                if (i >= 0)
                    return values[i];

                throw new System.Collections.Generic.KeyNotFoundException();
            }
            set
            {
                if (((object)key) == null) ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
                int i = Array.BinarySearch<TKey>(keys, 0, _size, key, comparer);
                if (i >= 0)
                {
                    values[i] = value;
                    version++;
                    return;
                }
                Insert(~i, key, value);
            }
        }

        public virtual object this[object key]
        {
            get
            {
                if (IsCompatibleKey(key))
                {
                    int i = IndexOfKey((TKey)key);
                    if (i >= 0)
                    {
                        return values[i];
                    }
                }

                return null;
            }
            set
            {
                if (!IsCompatibleKey(key))
                {
                    ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
                }

                ThrowHelper.IfNullAndNullsAreIllegalThenThrow<TValue>(value, ExceptionArgument.value);

                try
                {
                    TKey tempKey = (TKey)key;
                    try
                    {
                        this[tempKey] = (TValue)value;
                    }
                    catch (InvalidCastException)
                    {
                        ThrowHelper.ThrowWrongValueTypeArgumentException(value, typeof(TValue));
                    }
                }
                catch (InvalidCastException)
                {
                    ThrowHelper.ThrowWrongKeyTypeArgumentException(key, typeof(TKey));
                }
            }
        }

        // Returns the index of the entry with a given key in this sorted list. The
        // key is located through a binary search, and thus the average execution
        // time of this method is proportional to Log2(size), where
        // size is the size of this sorted list. The returned value is -1 if
        // the given key does not occur in this sorted list. Null is an invalid
        // key value.
        //
        public int IndexOfKey(TKey key)
        {
            if (key == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
            int ret = Array.BinarySearch<TKey>(keys, 0, _size, key, comparer);
            return ret >= 0 ? ret : -1;
        }

        // Returns the index of the first occurrence of an entry with a given value
        // in this sorted list. The entry is located through a linear search, and
        // thus the average execution time of this method is proportional to the
        // size of this sorted list. The elements of the list are compared to the
        // given value using the Object.Equals method.
        //
        public int IndexOfValue(TValue value)
        {
            return Array.IndexOf(values, value, 0, _size);
        }

        // Inserts an entry with a given key and value at a given index.
        private void Insert(int index, TKey key, TValue value)
        {
            if (_size == keys.Length) EnsureCapacity(_size + 1);
            if (index < _size)
            {
                Array.Copy(keys, index, keys, index + 1, _size - index);
                Array.Copy(values, index, values, index + 1, _size - index);
            }
            keys[index] = key;
            values[index] = value;
            _size++;
            version++;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            int i = IndexOfKey(key);
            if (i >= 0)
            {
                value = values[i];
                return true;
            }

            value = default(TValue);
            return false;
        }

        // Removes the entry at the given index. The size of the sorted list is
        // decreased by one.
        //
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= _size) ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_Index);
            _size--;
            if (index < _size)
            {
                Array.Copy(keys, index + 1, keys, index, _size - index);
                Array.Copy(values, index + 1, values, index, _size - index);
            }
            keys[_size] = default(TKey);
            values[_size] = default(TValue);
            version++;
        }

        // Removes an entry from this sorted list. If an entry with the specified
        // key exists in the sorted list, it is removed. An ArgumentException is
        // thrown if the key is null.
        //
        public bool Remove(TKey key)
        {
            int i = IndexOfKey(key);
            if (i >= 0)
                RemoveAt(i);
            return i >= 0;
        }

        void System.Collections.IDictionary.Remove(object key)
        {
            if (IsCompatibleKey(key))
            {
                Remove((TKey)key);
            }
        }

        // Sets the capacity of this sorted list to the size of the sorted list.
        // This method can be used to minimize a sorted list's memory overhead once
        // it is known that no new elements will be added to the sorted list. To
        // completely clear a sorted list and release all memory referenced by the
        // sorted list, execute the following statements:
        //
        // SortedList.Clear();
        // SortedList.TrimExcess();
        //
        public void TrimExcess()
        {
            int threshold = (int)(((double)keys.Length) * 0.9);
            if (_size < threshold)
            {
                Capacity = _size;
            }
        }

        private static bool IsCompatibleKey(object key)
        {
            if (key == null)
            {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
            }

            return (key is TKey);
        }

        private struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, System.Collections.IDictionaryEnumerator
        {
            private SortedList<TKey, TValue> _sortedList;
            private TKey key;
            private TValue value;
            private int index;
            private int version;
            private int getEnumeratorRetType;  // What should Enumerator.Current return?

            internal const int KeyValuePair = 1;
            internal const int DictEntry = 2;

            internal Enumerator(SortedList<TKey, TValue> sortedList, int getEnumeratorRetType)
            {
                this._sortedList = sortedList;
                this.index = 0;
                version = _sortedList.version;
                this.getEnumeratorRetType = getEnumeratorRetType;
                key = default(TKey);
                value = default(TValue);
            }

            public void Dispose()
            {
                index = 0;
                key = default(TKey);
                value = default(TValue);
            }


            object System.Collections.IDictionaryEnumerator.Key
            {
                get
                {
                    if (index == 0 || (index == _sortedList.Count + 1))
                    {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
                    }

                    return key;
                }
            }

            public bool MoveNext()
            {
                if (version != _sortedList.version) ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);

                if ((uint)index < (uint)_sortedList.Count)
                {
                    key = _sortedList.keys[index];
                    value = _sortedList.values[index];
                    index++;
                    return true;
                }

                index = _sortedList.Count + 1;
                key = default(TKey);
                value = default(TValue);
                return false;
            }

            DictionaryEntry System.Collections.IDictionaryEnumerator.Entry
            {
                get
                {
                    if (index == 0 || (index == _sortedList.Count + 1))
                    {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
                    }

                    return new DictionaryEntry(key, value);
                }
            }

            public KeyValuePair<TKey, TValue> Current
            {
                get
                {
                    return new KeyValuePair<TKey, TValue>(key, value);
                }
            }

            object System.Collections.IEnumerator.Current
            {
                get
                {
                    if (index == 0 || (index == _sortedList.Count + 1))
                    {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
                    }

                    if (getEnumeratorRetType == DictEntry)
                    {
                        return new System.Collections.DictionaryEntry(key, value);
                    }
                    else
                    {
                        return new KeyValuePair<TKey, TValue>(key, value);
                    }
                }
            }

            object System.Collections.IDictionaryEnumerator.Value
            {
                get
                {
                    if (index == 0 || (index == _sortedList.Count + 1))
                    {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
                    }

                    return value;
                }
            }

            void System.Collections.IEnumerator.Reset()
            {
                if (version != _sortedList.version)
                {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                index = 0;
                key = default(TKey);
                value = default(TValue);
            }
        }

        private sealed class SortedListKeyEnumerator : IEnumerator<TKey>, System.Collections.IEnumerator
        {
            private SortedList<TKey, TValue> _sortedList;
            private int index;
            private int version;
            private TKey currentKey;

            internal SortedListKeyEnumerator(SortedList<TKey, TValue> sortedList)
            {
                _sortedList = sortedList;
                version = sortedList.version;
            }

            public void Dispose()
            {
                index = 0;
                currentKey = default(TKey);
            }

            public bool MoveNext()
            {
                if (version != _sortedList.version)
                {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                if ((uint)index < (uint)_sortedList.Count)
                {
                    currentKey = _sortedList.keys[index];
                    index++;
                    return true;
                }

                index = _sortedList.Count + 1;
                currentKey = default(TKey);
                return false;
            }

            public TKey Current
            {
                get
                {
                    return currentKey;
                }
            }

            object System.Collections.IEnumerator.Current
            {
                get
                {
                    if (index == 0 || (index == _sortedList.Count + 1))
                    {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
                    }

                    return currentKey;
                }
            }

            void System.Collections.IEnumerator.Reset()
            {
                if (version != _sortedList.version)
                {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }
                index = 0;
                currentKey = default(TKey);
            }
        }

        private sealed class SortedListValueEnumerator : IEnumerator<TValue>, System.Collections.IEnumerator
        {
            private SortedList<TKey, TValue> _sortedList;
            private int index;
            private int version;
            private TValue currentValue;

            internal SortedListValueEnumerator(SortedList<TKey, TValue> sortedList)
            {
                _sortedList = sortedList;
                version = sortedList.version;
            }

            public void Dispose()
            {
                index = 0;
                currentValue = default(TValue);
            }

            public bool MoveNext()
            {
                if (version != _sortedList.version)
                {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                if ((uint)index < (uint)_sortedList.Count)
                {
                    currentValue = _sortedList.values[index];
                    index++;
                    return true;
                }

                index = _sortedList.Count + 1;
                currentValue = default(TValue);
                return false;
            }

            public TValue Current
            {
                get
                {
                    return currentValue;
                }
            }

            object System.Collections.IEnumerator.Current
            {
                get
                {
                    if (index == 0 || (index == _sortedList.Count + 1))
                    {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
                    }

                    return currentValue;
                }
            }

            void System.Collections.IEnumerator.Reset()
            {
                if (version != _sortedList.version)
                {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }
                index = 0;
                currentValue = default(TValue);
            }
        }

        private sealed class KeyList : IList<TKey>, System.Collections.ICollection
        {
            private SortedList<TKey, TValue> _dict;

            internal KeyList(SortedList<TKey, TValue> dictionary)
            {
                this._dict = dictionary;
            }

            public int Count
            {
                get { return _dict._size; }
            }

            public bool IsReadOnly
            {
                get { return true; }
            }

            bool System.Collections.ICollection.IsSynchronized
            {
                get { return false; }
            }

            object System.Collections.ICollection.SyncRoot
            {
                get { return ((ICollection)_dict).SyncRoot; }
            }

            public void Add(TKey key)
            {
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_SortedListNestedWrite);
            }

            public void Clear()
            {
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_SortedListNestedWrite);
            }

            public bool Contains(TKey key)
            {
                return _dict.ContainsKey(key);
            }

            public void CopyTo(TKey[] array, int arrayIndex)
            {
                // defer error checking to Array.Copy
                Array.Copy(_dict.keys, 0, array, arrayIndex, _dict.Count);
            }

            void System.Collections.ICollection.CopyTo(Array array, int arrayIndex)
            {
                if (array != null && array.Rank != 1)
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);

                try
                {
                    // defer error checking to Array.Copy
                    Array.Copy(_dict.keys, 0, array, arrayIndex, _dict.Count);
                }
                catch (ArrayTypeMismatchException)
                {
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
                }
            }

            public void Insert(int index, TKey value)
            {
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_SortedListNestedWrite);
            }

            public TKey this[int index]
            {
                get
                {
                    return _dict.GetKey(index);
                }
                set
                {
                    ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_KeyCollectionSet);
                }
            }

            public IEnumerator<TKey> GetEnumerator()
            {
                return new SortedListKeyEnumerator(_dict);
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return new SortedListKeyEnumerator(_dict);
            }

            public int IndexOf(TKey key)
            {
                if (((object)key) == null)
                    ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);

                int i = Array.BinarySearch<TKey>(_dict.keys, 0,
                                          _dict.Count, key, _dict.comparer);
                if (i >= 0) return i;
                return -1;
            }

            public bool Remove(TKey key)
            {
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_SortedListNestedWrite);
                return false;
            }

            public void RemoveAt(int index)
            {
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_SortedListNestedWrite);
            }
        }

        private sealed class ValueList : IList<TValue>, System.Collections.ICollection
        {
            private SortedList<TKey, TValue> _dict;

            internal ValueList(SortedList<TKey, TValue> dictionary)
            {
                this._dict = dictionary;
            }

            public int Count
            {
                get { return _dict._size; }
            }

            public bool IsReadOnly
            {
                get { return true; }
            }

            bool System.Collections.ICollection.IsSynchronized
            {
                get { return false; }
            }

            object System.Collections.ICollection.SyncRoot
            {
                get { return ((ICollection)_dict).SyncRoot; }
            }

            public void Add(TValue key)
            {
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_SortedListNestedWrite);
            }

            public void Clear()
            {
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_SortedListNestedWrite);
            }

            public bool Contains(TValue value)
            {
                return _dict.ContainsValue(value);
            }

            public void CopyTo(TValue[] array, int arrayIndex)
            {
                // defer error checking to Array.Copy
                Array.Copy(_dict.values, 0, array, arrayIndex, _dict.Count);
            }

            void System.Collections.ICollection.CopyTo(Array array, int arrayIndex)
            {
                if (array != null && array.Rank != 1)
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);

                try
                {
                    // defer error checking to Array.Copy
                    Array.Copy(_dict.values, 0, array, arrayIndex, _dict.Count);
                }
                catch (ArrayTypeMismatchException)
                {
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
                }
            }

            public void Insert(int index, TValue value)
            {
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_SortedListNestedWrite);
            }

            public TValue this[int index]
            {
                get
                {
                    return _dict.GetByIndex(index);
                }
                set
                {
                    ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_SortedListNestedWrite);
                }
            }

            public IEnumerator<TValue> GetEnumerator()
            {
                return new SortedListValueEnumerator(_dict);
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return new SortedListValueEnumerator(_dict);
            }

            public int IndexOf(TValue value)
            {
                return Array.IndexOf(_dict.values, value, 0, _dict.Count);
            }

            public bool Remove(TValue value)
            {
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_SortedListNestedWrite);
                return false;
            }

            public void RemoveAt(int index)
            {
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_SortedListNestedWrite);
            }

        }
    }
}