using System;
using HighFive;
using static Tesserae.UI;
using static H5.dom;

namespace Tesserae.Components
{
    public class LayerHost : ComponentBase<Layer, HTMLDivElement>
    {
        public LayerHost()
        {
            InnerElement = Div(_("tss-layer-host"));
        }

        public override HTMLElement Render()
        {
            return InnerElement;
        }
    }
}
