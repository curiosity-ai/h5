using System.Collections;
using System.Collections.Generic;
using static H5.dom;

namespace Tesserae
{
    public class ObservableDictionary<TKey, TValue> : Observable, IDictionary<TKey, TValue>, IObservable<IReadOnlyDictionary<TKey, TValue>>
    {
        public event ObservableEvent.ValueChanged<IReadOnlyDictionary<TKey, TValue>> onValueChanged;
        public IReadOnlyDictionary<TKey, TValue> Value => _dictionary;
        
        private readonly Dictionary<TKey, TValue> _dictionary;
        private bool _valueIsObservable;
        private double _refreshTimeout;

        public ObservableDictionary()
        {
            _dictionary = new Dictionary<TKey, TValue>();
            _valueIsObservable = typeof(IObservable).IsAssignableFrom(typeof(TValue));
        }

        public ObservableDictionary(Dictionary<TKey, TValue> dictionary)
        {
            _dictionary = dictionary;
            _valueIsObservable = typeof(IObservable).IsAssignableFrom(typeof(TValue));
            if (_valueIsObservable)
            {
                foreach (var kv in _dictionary)
                {
                    HookValue(kv.Value);
                }
            }
        }

        private void HookValue(TValue v)
        {
            if (_valueIsObservable && v is object)
            {
                ((IObservable)v).OnChange(RaiseOnValueChanged);
            }
        }

        private void UnhookValue(TValue v)
        {
            if (_valueIsObservable && v is object)
            {
                ((IObservable)v).Unobserve(RaiseOnValueChanged);
            }
        }



        public void Observe(ObservableEvent.ValueChanged<IReadOnlyDictionary<TKey, TValue>> valueGetter)
        {
            onValueChanged += valueGetter;
            valueGetter(_dictionary);
        }

        private void RaiseOnValueChanged()
        {
            window.clearTimeout(_refreshTimeout);
            _refreshTimeout = window.setTimeout(raise, 1);
            void raise(object t)
            {
                onValueChanged?.Invoke(_dictionary);
                RaiseOnChanged();
            }
        }

        public TValue this[TKey key] 
        { 
            get => _dictionary[key]; 
            set 
            {
                if (_dictionary.TryGetValue(key, out var prev)) 
                {
                    UnhookValue(prev); 
                }   
                _dictionary[key] = value; 
                HookValue(value);
                RaiseOnValueChanged();
            }
        }

        public ICollection<TKey> Keys => _dictionary.Keys;

        public ICollection<TValue> Values => _dictionary.Values;

        public int Count => _dictionary.Count;

        public bool IsReadOnly => false;

        public void Add(TKey key, TValue value)
        {
            _dictionary.Add(key, value);
            HookValue(value);
            RaiseOnValueChanged();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            _dictionary.Add(item.Key, item.Value);
            HookValue(item.Value);
            RaiseOnValueChanged();
        }

        public void Clear()
        {
            if (_valueIsObservable)
            {
                foreach (var kv in _dictionary)
                {
                    UnhookValue(kv.Value);
                }
            }

            _dictionary.Clear();
            RaiseOnValueChanged();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((IDictionary<TKey, TValue>)_dictionary).Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((IDictionary<TKey, TValue>)_dictionary).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        public bool Remove(TKey key)
        {
            if (_dictionary.TryGetValue(key, out var prev))
            {
                _dictionary.Remove(key);
                UnhookValue(prev);
                RaiseOnValueChanged();
                return true;
            }
            return false;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (_dictionary.TryGetValue(item.Key, out var prev) && Equals(item.Value, prev))
            {
                _dictionary.Remove(item.Key);
                UnhookValue(prev);
                RaiseOnValueChanged();
                return true;
            }
            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }
    }
}