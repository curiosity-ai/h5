using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static H5.dom;

namespace Tesserae
{
    public class ObservableList<T> : Observable, IList<T>, ICollection<T>, IObservable<IReadOnlyList<T>>
    {
        public event ObservableEvent.ValueChanged<IReadOnlyList<T>> onValueChanged;

        private readonly List<T> _list;
        private readonly bool _valueIsObservable;
        private double _refreshTimeout;

        public ObservableList()
        {
            _list = new List<T>();
            _valueIsObservable = typeof(IObservable).IsAssignableFrom(typeof(T));
        }

        public ObservableList(params T[] initialValues)
        {
            _list = initialValues.ToList();
            _valueIsObservable = typeof(IObservable).IsAssignableFrom(typeof(T));
            if (_valueIsObservable)
            {
                foreach (var i in _list)
                {
                    HookValue(i);
                }
            }
        }

        private void HookValue(T v)
        {
            if (_valueIsObservable && (v is IObservable observableV))
            {
                observableV.OnChange(RaiseOnValueChanged);
            }
        }

        private void UnhookValue(T v)
        {
            if (_valueIsObservable && (v is IObservable observableV))
            {
                observableV.Unobserve(RaiseOnValueChanged);
            }
        }

        public void Observe(ObservableEvent.ValueChanged<IReadOnlyList<T>> valueGetter)
        {
            onValueChanged += valueGetter;
            valueGetter(_list);
        }

        public T this[int index]
        {
            get => _list[index];
            set
            {
                if (_list.Count > index)
                {
                    UnhookValue(_list[index]);
                }
                _list[index] = value;
                RaiseOnValueChanged();
            }
        }

        private void RaiseOnValueChanged()
        {
            window.clearTimeout(_refreshTimeout);
            _refreshTimeout = window.setTimeout(raise, 1);
            void raise(object t)
            {
                onValueChanged?.Invoke(_list);
                RaiseOnChanged();
            }
        }

        public int Count => _list.Count;

        public bool IsReadOnly => false;

        public IReadOnlyList<T> Value => _list;

        public void Add(T item)
        {
            _list.Add(item);
            HookValue(item);
            RaiseOnValueChanged();
        }

        public void AddRange(IEnumerable<T> enumerable)
        {
            foreach (var item in enumerable)
            {
                _list.Add(item);
                HookValue(item);
            }
            RaiseOnValueChanged();
        }

        public void Clear()
        {
            if (_valueIsObservable)
            {
                foreach (var i in _list)
                {
                    UnhookValue(i);
                }
            }

            _list.Clear();
            RaiseOnValueChanged();
        }

        public bool Contains(T item) => _list.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int IndexOf(T item) => _list.IndexOf(item);

        public void Insert(int index, T item)
        {
            if (_list.Count > index)
            {
                UnhookValue(_list[index]);
            }

            _list.Insert(index, item);
            HookValue(item);
            RaiseOnValueChanged();
        }

        public bool Remove(T item)
        {
            var removed = _list.Remove(item);
            if (removed)
            {
                UnhookValue(item);
                RaiseOnValueChanged();
            }
            return removed;
        }

        public void RemoveAt(int index)
        {
            if (_list.Count > index)
            {
                UnhookValue(_list[index]);
            }

            _list.RemoveAt(index);
            RaiseOnValueChanged();
        }
    }
}