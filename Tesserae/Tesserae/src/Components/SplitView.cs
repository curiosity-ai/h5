using System;
using static Tesserae.UI;
using static H5.Core.dom;
using System.Threading.Tasks;

namespace Tesserae.Components
{
    public class SplitView : IComponent
    {
        private HTMLElement InnerElement;
        private Raw         LeftComponent;
        private Raw         SplitterComponent;
        private Raw         RightComponent;

        public SplitView(UnitSize splitterSize = null)
        {
            LeftComponent     = Raw(Div(_()));
            SplitterComponent = Raw(Div(_("tss-splitter")));
            RightComponent    = Raw(Div(_()));
            SplitterComponent.Width = (splitterSize is object && splitterSize.Unit != Unit.Auto && splitterSize.Unit != Unit.Inherit)
                                          ? splitterSize.ToString()
                                          : "8px";
            LeftComponent.Height     = "100%";
            SplitterComponent.Height = "100%";
            RightComponent.Height    = "100%";
            SplitInMiddle();
            InnerElement = Div(_("tss-splitview"), LeftComponent.Render(), SplitterComponent.Render(), RightComponent.Render());
        }

        public SplitView Left(IComponent component, string background = "")
        {
            LeftComponent.Content(component);

            LeftComponent.Background = background;
            return this;
        }

        public SplitView Right(IComponent component, string background = "")
        {
            RightComponent.Content(component);

            RightComponent.Background = background;
            return this;
        }

        public SplitView Splitter(IComponent component)
        {
            SplitterComponent.Content(component);
            return this;
        }

        public SplitView NoSplitter()
        {
            SplitterComponent.Width = "0px";
            return this;
        }

        public SplitView SplitInMiddle()
        {
            LeftComponent.Width  = $"calc(50% - {SplitterComponent.Width})";
            RightComponent.Width = "50%";
            return this;
        }

        public SplitView LeftIsSmaller(UnitSize leftSize)
        {
            LeftComponent.Width  = $"calc({leftSize.ToString()} - {SplitterComponent.Width})";
            RightComponent.Width = $"calc(100% - {leftSize.ToString()})";

            return this;
        }

        public SplitView RightIsSmaller(UnitSize rightSize)
        {
            RightComponent.Width = $"calc({rightSize.ToString()} - {SplitterComponent.Width})";
            LeftComponent.Width = $"calc(100% - {rightSize.ToString()})";

            return this;
        }

        public HTMLElement Render()
        {
            return InnerElement;
        }
    }

    public enum SizeMode
    {
        Pixels,
        Percent
    }
}
