using static Tesserae.UI;
using static H5.Core.dom;

namespace Tesserae.Components
{
    public class Icon : IComponent, IHasForegroundColor, IHasTextSize
    {
        private readonly HTMLElement InnerElement;

        public Icon(string icon) => InnerElement = I(_(icon));

        public string Foreground
        {
            get => InnerElement.style.color;
            set => InnerElement.style.color = value;
        }

        public TextSize Size
        {
            get => InnerElement.GetTextSize().textSize ?? TextSize.Small;
            set
            {
                var (textSize, textSizeCssClass) = InnerElement.GetTextSize();

                InnerElement.RemoveClassIf(textSize.HasValue, textSizeCssClass);

                InnerElement.classList.add($"tss-fontsize-{value.ToString().ToLower()}");
            }
        }

        public TextWeight Weight
        {
            get => InnerElement.GetTextWeight().textWeight ?? TextWeight.Regular;
            set
            {
                var (textWeight, textWeightCssClass) = InnerElement.GetTextWeight();

                InnerElement.RemoveClassIf(textWeight.HasValue, textWeightCssClass);

                InnerElement.classList.add($"tss-fontweight-{value.ToString().ToLower()}");
            }
        }

        public TextAlign TextAlign
        {
            get => InnerElement.GetTextAlign().textAlign ?? TextAlign.Center;
            set
            {
                var (textAlign, textAlignCssClass) = InnerElement.GetTextAlign();

                InnerElement.RemoveClassIf(textAlign.HasValue, textAlignCssClass);

                InnerElement.classList.add($"tss-textalign-{value.ToString().ToLower()}");
            }
        }

        public string Title
        {
            get => InnerElement.title;
            set => InnerElement.title = value;
        }

        public Icon SetTitle(string title)
        {
            Title = title;
            return this;
        }

        public HTMLElement Render() => InnerElement;
    }
}
