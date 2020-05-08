namespace Tesserae
{
    public interface IObservable
    {
        void OnChange(Observable.Changed changed);
        void Unobserve(Observable.Changed changed);
    }
}