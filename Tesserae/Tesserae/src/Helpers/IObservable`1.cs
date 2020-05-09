namespace Tesserae
{
    public static class ObservableEvent
    {
        public delegate void ValueChanged<T>(T value);
    }

    public interface IObservable<T> : IObservable
    {
        event ObservableEvent.ValueChanged<T> onValueChanged;
        T Value { get; }
        void Observe(ObservableEvent.ValueChanged<T> onChange);
    }
}