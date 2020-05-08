using System.Collections.Generic;
using System.Linq;
using Tesserae.Components;
using static Tesserae.Tests.Samples.SamplesHelper;
using static Tesserae.UI;
using static H5.Core.dom;

namespace Tesserae.Tests.Samples
{
    public class VirtualizedListSample : IComponent
    {
        private readonly IComponent _content;

        public VirtualizedListSample()
        {
            _content =
                SectionStack()
                    .Title(SampleHeader(nameof(VirtualizedListSample)))
                    .Section(
                        Stack()
                            .Children(
                                SampleTitle("Overview"),
                                TextBlock("List provides a base component for rendering large sets of items. " +
                                          "It is agnostic of the tile component used, and selection " +
                                          "management. These concerns can be layered separately.")
                                    .PaddingBottom(16.px()),
                                TextBlock("Performance is important, and DOM content is expensive. Therefore, " +
                                          "limit what you render. List applies this principle by using UI " +
                                          "virtualization. Unlike a simple for loop that renders all items in " +
                                          "a set, a List only renders a subset of items, and as you scroll around, " +
                                          "the subset of rendered content is shifted. This gives a much " +
                                          "better experience for large sets, especially when the " +
                                          "per-item components are complex/render-intensive/network-intensive.")
                                    .PaddingBottom(16.px()),
                                TextBlock("List breaks down the set of items passed in into pages. Only pages " +
                                          "within a 'materialized window' are actually rendered. As that window " +
                                          "changes due to scroll events, pages that fall outside that window are " +
                                          "removed, and their layout space is remembered and pushed into spacer " +
                                          "elements. This gives the user the experience of browsing massive amounts " +
                                          "of content but only using a small number of actual elements. " +
                                          "This gives the browser much less layout to resolve.")))
                    .Section(
                        Stack()
                            .Children(
                                SampleTitle("Usage"),
                                TextBlock("Virtualized List")
                                    .Medium()
                                    .PaddingBottom(16.px()),
                                VirtualizedList().WithListItems(GetALotOfItems()).PaddingBottom(32.px()),
                                TextBlock("Virtualized List with Empty List Message")
                                    .Medium()
                                    .PaddingBottom(16.px()),
                                VirtualizedList().WithEmptyMessage(() => TextBlock("No List Items")).WithListItems(Enumerable.Empty<IComponent>())));
        }

        public HTMLElement Render()
        {
            return _content.Render();
        }

        private IEnumerable<SampleVirtualizedItem> GetALotOfItems()
        {
            return Enumerable
                .Range(1, 5000)
                .Select(number => new SampleVirtualizedItem($"Lorem Ipsum {number}"));
        }

        public sealed class SampleVirtualizedItem : IComponent
        {
            private readonly HTMLElement _innerElement;

            public SampleVirtualizedItem(string text)
            {
                _innerElement =
                    Div(_(text: text, styles: s =>
                    {
                        s.display = "block";
                        s.textAlign = "center";
                        s.height = "63px";
                    }));
            }

            public HTMLElement Render() => _innerElement;
        }
    }
}
