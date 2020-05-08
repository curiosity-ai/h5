using static H5.Core.dom;

namespace Tesserae
{
    /// <summary>
    /// Enables monitoring of changes for a variable of type T (this class is for listeners only, if updating the value is required then the SettableObserver should be used)
    /// </summary>
    /// <typeparam name="T">An immutable type to be observed. Be careful with non-imutable types, as they may be changed in ways that will not be repoted here</typeparam>
    public class Observable<T> : Observable, IObservable<T>
    {
        private T _value;
        private double _refreshTimeout;

        public Observable(T value = default) => _value = value;

        public event ObservableEvent.ValueChanged<T> onValueChanged;

        public T Value
        {
            get => _value;
            protected set
            {
                if (!_value.Equals(value))
                {
                    _value = value;
                    RaiseOnValueChanged();
                }
            }
        }

        private void RaiseOnValueChanged()
        {
            window.clearTimeout(_refreshTimeout);
            _refreshTimeout = window.setTimeout(raise, 1);
            void raise(object t)
            {
                onValueChanged?.Invoke(_value);
                RaiseOnChanged();
            }
        }

        
        public void Observe(ObservableEvent.ValueChanged<T> onChange)
        {
            onValueChanged += onChange;
            onChange(_value);
        }
    }

    public class CombinedObservable<T1, T2> : Observable, IObservable<(T1 first, T2 second)>
    {
        private readonly IObservable<T1> _first;
        private readonly IObservable<T2> _second;
        private double _refreshTimeout;

        public (T1 first, T2 second) Value => (_first.Value, _second.Value);

        public event ObservableEvent.ValueChanged<(T1 first, T2 second)> onValueChanged;

        public CombinedObservable(IObservable<T1> o1, IObservable<T2> o2)
        {
            o1.onValueChanged += FirstValueChanged;
            o2.onValueChanged += SecondValueChanged;
            _first = o1;
            _second = o2;
        }

        private void FirstValueChanged(T1 value)
        {
            RaiseOnValueChanged();
        }

        private void SecondValueChanged(T2 value)
        {
            RaiseOnValueChanged();
        }

        private void RaiseOnValueChanged()
        {
            window.clearTimeout(_refreshTimeout);
            _refreshTimeout = window.setTimeout(raise, 1);
            void raise(object t)
            {
                onValueChanged?.Invoke(Value);
                RaiseOnChanged();
            }
        }

        public void Observe(ObservableEvent.ValueChanged<(T1 first, T2 second)> onChange)
        {
            onValueChanged += onChange;
            onChange(Value);
        }
    }
}