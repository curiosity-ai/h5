﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace System.Collections.ObjectModel
{
    public class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IDictionary, IReadOnlyDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> m_dictionary;
        private const string NotSupported_ReadOnlyCollection = "Collection is read-only.";

        [NonSerialized]
        private KeyCollection _keys;
        [NonSerialized]
        private ValueCollection _values;

        public ReadOnlyDictionary(IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }
            Contract.EndContractBlock();
            m_dictionary = dictionary;
        }

        protected IDictionary<TKey, TValue> Dictionary
        {
            get { return m_dictionary; }
        }

        public KeyCollection Keys
        {
            get
            {
                Contract.Ensures(Contract.Result<KeyCollection>() != null);
                if (_keys == null)
                {
                    _keys = new KeyCollection(m_dictionary.Keys);
                }
                return _keys;
            }
        }

        public ValueCollection Values
        {
            get
            {
                Contract.Ensures(Contract.Result<ValueCollection>() != null);
                if (_values == null)
                {
                    _values = new ValueCollection(m_dictionary.Values);
                }
                return _values;
            }
        }

        #region IDictionary<TKey, TValue> Members

        public bool ContainsKey(TKey key)
        {
            return m_dictionary.ContainsKey(key);
        }

        ICollection<TKey> IDictionary<TKey, TValue>.Keys
        {
            get
            {
                return Keys;
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return m_dictionary.TryGetValue(key, out value);
        }

        ICollection<TValue> IDictionary<TKey, TValue>.Values
        {
            get
            {
                return Values;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                return m_dictionary[key];
            }
        }

        void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            throw new NotSupportedException(NotSupported_ReadOnlyCollection);
        }

        bool IDictionary<TKey, TValue>.Remove(TKey key)
        {
            throw new NotSupportedException(NotSupported_ReadOnlyCollection);
        }

        TValue IDictionary<TKey, TValue>.this[TKey key]
        {
            get
            {
                return m_dictionary[key];
            }
            set
            {
                throw new NotSupportedException(NotSupported_ReadOnlyCollection);
            }
        }

        #endregion

        #region ICollection<KeyValuePair<TKey, TValue>> Members

        public int Count
        {
            get { return m_dictionary.Count; }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return m_dictionary.Contains(item);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            m_dictionary.CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get { return true; }
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException(NotSupported_ReadOnlyCollection);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Clear()
        {
            throw new NotSupportedException(NotSupported_ReadOnlyCollection);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException(NotSupported_ReadOnlyCollection);
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey, TValue>> Members

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return m_dictionary.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)m_dictionary).GetEnumerator();
        }

        #endregion

        #region IDictionary Members

        private static bool IsCompatibleKey(object key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return key is TKey;
        }

        void IDictionary.Add(object key, object value)
        {
            throw new NotSupportedException(NotSupported_ReadOnlyCollection);
        }

        void IDictionary.Clear()
        {
            throw new NotSupportedException(NotSupported_ReadOnlyCollection);
        }

        bool IDictionary.Contains(object key)
        {
            return IsCompatibleKey(key) && ContainsKey((TKey)key);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            if (m_dictionary is IDictionary d)
            {
                return d.GetEnumerator();
            }
            return new DictionaryEnumerator(m_dictionary);
        }

        bool IDictionary.IsFixedSize
        {
            get { return true; }
        }

        bool IDictionary.IsReadOnly
        {
            get { return true; }
        }

        ICollection IDictionary.Keys
        {
            get
            {
                return Keys;
            }
        }

        void IDictionary.Remove(object key)
        {
            throw new NotSupportedException(NotSupported_ReadOnlyCollection);
        }

        ICollection IDictionary.Values
        {
            get
            {
                return Values;
            }
        }

        object IDictionary.this[object key]
        {
            get
            {
                if (IsCompatibleKey(key))
                {
                    return this[(TKey)key];
                }
                return null;
            }
            set
            {
                throw new NotSupportedException(NotSupported_ReadOnlyCollection);
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (array.Rank != 1)
            {
                throw new ArgumentException("Only single dimensional arrays are supported for the requested action.");
            }

            if (array.GetLowerBound(0) != 0)
            {
                throw new ArgumentException("The lower bound of target array must be zero.");
            }

            if (index < 0 || index > array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Non-negative number required.");
            }

            if (array.Length - index < Count)
            {
                throw new ArgumentException("Destination array is not long enough to copy all the items in the collection. Check array index and length.");
            }

            if (array is KeyValuePair<TKey, TValue>[] pairs)
            {
                m_dictionary.CopyTo(pairs, index);
            }
            else
            {
                if (array is DictionaryEntry[] dictEntryArray)
                {
                    foreach (var item in m_dictionary)
                    {
                        dictEntryArray[index++] = new DictionaryEntry(item.Key, item.Value);
                    }
                }
                else
                {
                    if (!(array is object[] objects))
                    {
                        throw new ArgumentException("Target array type is not compatible with the type of items in the collection.");
                    }

                    try
                    {
                        foreach (var item in m_dictionary)
                        {
                            objects[index++] = new KeyValuePair<TKey, TValue>(item.Key, item.Value);
                        }
                    }
                    catch (ArrayTypeMismatchException)
                    {
                        throw new ArgumentException("Target array type is not compatible with the type of items in the collection.");
                    }
                }
            }
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return null;
            }
        }

        private struct DictionaryEnumerator : IDictionaryEnumerator
        {
            private readonly IDictionary<TKey, TValue> _dictionary;
            private IEnumerator<KeyValuePair<TKey, TValue>> _enumerator;

            public DictionaryEnumerator(IDictionary<TKey, TValue> dictionary)
            {
                _dictionary = dictionary;
                _enumerator = _dictionary.GetEnumerator();
            }

            public DictionaryEntry Entry
            {
                get { return new DictionaryEntry(_enumerator.Current.Key, _enumerator.Current.Value); }
            }

            public object Key
            {
                get { return _enumerator.Current.Key; }
            }

            public object Value
            {
                get { return _enumerator.Current.Value; }
            }

            public object Current
            {
                get { return Entry; }
            }

            public bool MoveNext()
            {
                return _enumerator.MoveNext();
            }

            public void Reset()
            {
                _enumerator.Reset();
            }
        }

        #endregion

        #region IReadOnlyDictionary members

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
        {
            get
            {
                return Keys;
            }
        }

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
        {
            get
            {
                return Values;
            }
        }

        #endregion IReadOnlyDictionary members

        public sealed class KeyCollection : ICollection<TKey>, ICollection, IReadOnlyCollection<TKey>
        {
            private readonly ICollection<TKey> _collection;

            internal KeyCollection(ICollection<TKey> collection)
            {
                if (collection == null)
                {
                    throw new ArgumentNullException(nameof(collection));
                }
                _collection = collection;
            }

            #region ICollection<T> Members

            void ICollection<TKey>.Add(TKey item)
            {
                throw new NotSupportedException(NotSupported_ReadOnlyCollection);
            }

            void ICollection<TKey>.Clear()
            {
                throw new NotSupportedException(NotSupported_ReadOnlyCollection);
            }

            bool ICollection<TKey>.Contains(TKey item)
            {
                return _collection.Contains(item);
            }

            public void CopyTo(TKey[] array, int arrayIndex)
            {
                _collection.CopyTo(array, arrayIndex);
            }

            public int Count
            {
                get { return _collection.Count; }
            }

            bool ICollection<TKey>.IsReadOnly
            {
                get { return true; }
            }

            bool ICollection<TKey>.Remove(TKey item)
            {
                throw new NotSupportedException(NotSupported_ReadOnlyCollection);
            }

            #endregion

            #region IEnumerable<T> Members

            public IEnumerator<TKey> GetEnumerator()
            {
                return _collection.GetEnumerator();
            }

            #endregion

            #region IEnumerable Members

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return ((IEnumerable)_collection).GetEnumerator();
            }

            #endregion

            #region ICollection Members

            void ICollection.CopyTo(Array array, int index)
            {
                ReadOnlyDictionaryHelpers.CopyToNonGenericICollectionHelper<TKey>(_collection, array, index);
            }

            bool ICollection.IsSynchronized
            {
                get { return false; }
            }

            object ICollection.SyncRoot
            {
                get
                {
                    return null;
                }
            }
            #endregion
        }

        public sealed class ValueCollection : ICollection<TValue>, ICollection, IReadOnlyCollection<TValue>
        {
            private readonly ICollection<TValue> _collection;

            internal ValueCollection(ICollection<TValue> collection)
            {
                if (collection == null)
                {
                    throw new ArgumentNullException(nameof(collection));
                }
                _collection = collection;
            }

            #region ICollection<T> Members

            void ICollection<TValue>.Add(TValue item)
            {
                throw new NotSupportedException(NotSupported_ReadOnlyCollection);
            }

            void ICollection<TValue>.Clear()
            {
                throw new NotSupportedException(NotSupported_ReadOnlyCollection);
            }

            bool ICollection<TValue>.Contains(TValue item)
            {
                return _collection.Contains(item);
            }

            public void CopyTo(TValue[] array, int arrayIndex)
            {
                _collection.CopyTo(array, arrayIndex);
            }

            public int Count
            {
                get { return _collection.Count; }
            }

            bool ICollection<TValue>.IsReadOnly
            {
                get { return true; }
            }

            bool ICollection<TValue>.Remove(TValue item)
            {
                throw new NotSupportedException(NotSupported_ReadOnlyCollection);
            }

            #endregion

            #region IEnumerable<T> Members

            public IEnumerator<TValue> GetEnumerator()
            {
                return _collection.GetEnumerator();
            }

            #endregion

            #region IEnumerable Members

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return ((IEnumerable)_collection).GetEnumerator();
            }

            #endregion

            #region ICollection Members

            void ICollection.CopyTo(Array array, int index)
            {
                ReadOnlyDictionaryHelpers.CopyToNonGenericICollectionHelper<TValue>(_collection, array, index);
            }

            bool ICollection.IsSynchronized
            {
                get { return false; }
            }

            object ICollection.SyncRoot
            {
                get
                {
                    return null;
                }
            }
            #endregion ICollection Members
        }
    }

    // To share code when possible, use a non-generic class to get rid of irrelevant type parameters.
    internal static class ReadOnlyDictionaryHelpers
    {
        #region Helper method for our KeyCollection and ValueCollection

        // Abstracted away to avoid redundant implementations.
        internal static void CopyToNonGenericICollectionHelper<T>(ICollection<T> collection, Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (array.Rank != 1)
            {
                throw new ArgumentException("Only single dimensional arrays are supported for the requested action.");
            }

            if (array.GetLowerBound(0) != 0)
            {
                throw new ArgumentException("The lower bound of target array must be zero.");
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index is less than zero.");
            }

            if (array.Length - index < collection.Count)
            {
                throw new ArgumentException("Destination array is not long enough to copy all the items in the collection. Check array index and length.");
            }

            // Easy out if the ICollection<T> implements the non-generic ICollection
            if (collection is ICollection nonGenericCollection)
            {
                nonGenericCollection.CopyTo(array, index);
                return;
            }

            if (array is T[] items)
            {
                collection.CopyTo(items, index);
            }
            else
            {
                /*
                    FxOverRh: Type.IsAssignableNot() not an api on that platform.

                //
                // Catch the obvious case assignment will fail.
                // We can found all possible problems by doing the check though.
                // For example, if the element type of the Array is derived from T,
                // we can't figure out if we can successfully copy the element beforehand.
                //
                Type targetType = array.GetType().GetElementType();
                Type sourceType = typeof(T);
                if (!(targetType.IsAssignableFrom(sourceType) || sourceType.IsAssignableFrom(targetType))) {
                    throw new ArgumentException(SR.Argument_InvalidArrayType);
                }
                */

                //
                // We can't cast array of value type to object[], so we don't support
                // widening of primitive types here.
                //
                if (!(array is object[] objects))
                {
                    throw new ArgumentException("Target array type is not compatible with the type of items in the collection.");
                }

                try
                {
                    foreach (var item in collection)
                    {
                        objects[index++] = item;
                    }
                }
                catch (ArrayTypeMismatchException)
                {
                    throw new ArgumentException("Target array type is not compatible with the type of items in the collection.");
                }
            }
        }
        #endregion Helper method for our KeyCollection and ValueCollection
    }
}
