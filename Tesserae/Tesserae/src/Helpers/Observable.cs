namespace Tesserae
{
    public class Observable
    {
        public delegate void Changed();
        
        private event Changed OnChanged;

        public void OnChange(Changed changed) => OnChanged += changed; // TODO [2020-03-05 DWR]: Why does this method exist if we already have a public event that can be listened to?

        public void Unobserve(Changed changed) => OnChanged -= changed; // TODO [2020-03-05 DWR]: Why does this method exist if we already have a public event that listeners can be removed from?

        protected void RaiseOnChanged() => OnChanged?.Invoke();
    }
}