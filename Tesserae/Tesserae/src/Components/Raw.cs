using System;
using static H5.dom;
using static Tesserae.UI;

namespace Tesserae.Components
{
    public class Raw : IComponent, IHasMarginPadding, IHasBackgroundColor
    {
        private HTMLElement InnerElement;
        private byte _hasProperties;

        public Raw(HTMLElement content = null)
        {
            InnerElement = content ?? DIV();
        }

        public Raw(IComponent component)
        {
            InnerElement = component.Render();
        }

        public Raw Content(IComponent component) => Content(component.Render());

        public Raw Content(HTMLElement element)
        {
            if(_hasProperties > 0)
            {
                CopyPropertiesTo(element);
            }

            if(InnerElement is object && InnerElement.parentElement is object)
            {
                InnerElement.parentElement.replaceChild(element, InnerElement);
            }

            InnerElement = element;

            return this;
        }

        private void CopyPropertiesTo(HTMLElement element)
        {
            if ((_hasProperties & 0b00001) == 0b00001) element.style.background = InnerElement.style.background;
            if ((_hasProperties & 0b00010) == 0b00010) element.style.margin = InnerElement.style.margin;
            if ((_hasProperties & 0b00100) == 0b00100) element.style.padding = InnerElement.style.padding;
            if ((_hasProperties & 0b01000) == 0b01000) element.style.width = InnerElement.style.width;
            if ((_hasProperties & 0b10000) == 0b10000) element.style.height = InnerElement.style.height;
        }

        public string Background { get => InnerElement.style.background; set { _hasProperties |= 0b00001; InnerElement.style.background = value; } }
        public string Margin { get => InnerElement.style.margin; set { _hasProperties         |= 0b00010; InnerElement.style.margin = value; } }
        public string Padding { get => InnerElement.style.padding; set { _hasProperties       |= 0b00100; InnerElement.style.padding = value; } }
        public string Width { get => InnerElement.style.width; set { _hasProperties           |= 0b01000; InnerElement.style.width = value; } }
        public string Height { get => InnerElement.style.height; set { _hasProperties         |= 0b10000; InnerElement.style.height = value; } }

        public HTMLElement Render()
        {
            return InnerElement;
        }
    }
}