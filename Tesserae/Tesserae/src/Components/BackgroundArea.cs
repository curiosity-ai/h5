using static Tesserae.UI;
using static H5.dom;

namespace Tesserae.Components
{
    public class BackgroundArea : IComponent, IHasBackgroundColor
    {
        private readonly Raw _raw;
        private readonly HTMLElement _container;

        public BackgroundArea(IComponent content)
        {
            _raw = Raw(content.Render());
            _container = Div(_("tss-background-area"), _raw.Render());
        }

        public BackgroundArea Content(IComponent content)
        {
            _raw.Content(content);
            return this;
        }
        public string Background { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public HTMLElement Render() => _container;
    }
}
