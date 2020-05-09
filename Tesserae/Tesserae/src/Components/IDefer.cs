namespace Tesserae.Components
{
    public interface IDefer : IComponent
    {
        IDefer Debounce(int milliseconds);
        void Refresh();
    }
}