using System.Collections.Generic;

namespace Tesserae
{
    public interface IObservableComponent<T>
    {
        IObservable<T> AsObservable();
    }

    public interface IObservableListComponent<T>
    {
        IObservable<IReadOnlyList<T>> AsObservable();
    }
}