using Tesserae.HTML;
using static H5.Core.dom;
using static Tesserae.UI;

namespace Tesserae.Components
{
    public class Image : ComponentBase<Image, HTMLImageElement>
    {
        public Image(string source)
        {
            InnerElement = UI.Image(_(src:source));
        }

        public override HTMLElement Render()
        {
            return InnerElement;
        }
    }
}
