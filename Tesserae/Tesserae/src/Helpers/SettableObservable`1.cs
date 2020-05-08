using System;

namespace Tesserae
{
    /// <summary>
    /// Encapsulates a variable of type T, and enables monitoring for changes as well as the ability to update that value (which will trigger a ValueChanged event)
    /// </summary>
    /// <typeparam name="T">An immutable type to be observed. Be careful with non-imutable types, as you can change them in ways that will not be visible here</typeparam>
    public class SettableObservable<T> : Observable<T>
    {
        public SettableObservable(T value = default) : base(value) { }

        public new T Value
        {
            get => base.Value;
            set => base.Value = value;
        }

        public void Update(Func<T, T> action)
        {
            Value = action(Value);
        }
    }
}