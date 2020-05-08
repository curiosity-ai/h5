using static Tesserae.UI;
using static H5.dom;
using System.Linq;
using System;

namespace Tesserae.Components
{
    public class Spinner : ComponentBase<Spinner, HTMLDivElement>
    {
        private readonly HTMLElement _container;
        private readonly HTMLElement _label;

        public Spinner(string text = string.Empty)
        {
            InnerElement = Div(_("tss-spinner"));
            _label = Label(_("tss-spinner-label", text: text));
            _container = Div(_("tss-spinner-container tss-spinner-position-right tss-spinner-size-small"), InnerElement, _label);
        }

        public LabelPosition Position
        {
            get
            {
                var s = _container.classList.FirstOrDefault(x => x.StartsWith("tss-spinner-position-"));
                if (s != null && Enum.TryParse<LabelPosition>(s, true, out LabelPosition result)) return result;
                return LabelPosition.Right;
            }
            set
            {
                var s = _container.classList.FirstOrDefault(x => x.StartsWith("tss-spinner-position-"));
                if (s != null) _container.classList.remove(s);
                _container.classList.add($"tss-spinner-position-{value.ToString().ToLower()}");
            }
        }

        public CircleSize Size
        {
            get
            {
                var s = _container.classList.FirstOrDefault(x => x.StartsWith("tss-spinner-size-"));
                if (s != null && Enum.TryParse<CircleSize>(s, true, out CircleSize result)) return result;
                return CircleSize.Small;
            }
            set
            {
                var s = _container.classList.FirstOrDefault(x => x.StartsWith("tss-spinner-size-"));
                if (s != null) _container.classList.remove(s);
                _container.classList.add($"tss-spinner-size-{value.ToString().ToLower()}");
            }
        }

        public string Text
        {
            get => _label.innerText;
            set => _label.innerText = value;
        }

        public override HTMLElement Render()
        {
            return _container;
        }

        public Spinner Left()
        {
            Position = LabelPosition.Left;
            return this;
        }
        public Spinner Right()
        {
            Position = LabelPosition.Right;
            return this;
        }
        public Spinner Above()
        {
            Position = LabelPosition.Above;
            return this;
        }
        public Spinner Below()
        {
            Position = LabelPosition.Below;
            return this;
        }

        public Spinner XSmall()
        {
            Size = CircleSize.XSmall;
            return this;
        }
        public Spinner Small()
        {
            Size = CircleSize.Small;
            return this;
        }
        public Spinner Medium()
        {
            Size = CircleSize.Medium;
            return this;
        }
        public Spinner Large()
        {
            Size = CircleSize.Large;
            return this;
        }

        public Spinner SetText(string text)
        {
            Text = text;
            return this;
        }

        public enum LabelPosition
        {
            Above,
            Below,
            Left,
            Right
        }

        public enum CircleSize
        {
            XSmall,
            Small,
            Medium,
            Large
        }
    }
}
