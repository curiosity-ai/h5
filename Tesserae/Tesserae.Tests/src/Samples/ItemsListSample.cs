using System.Collections.Generic;
using System.Linq;
using Tesserae.Components;
using static Tesserae.Tests.Samples.SamplesHelper;
using static Tesserae.UI;
using static H5.Core.dom;

namespace Tesserae.Tests.Samples
{
    public class ItemsListSample : IComponent
    {
        private readonly IComponent _content;

        public ItemsListSample()
        {
            var obsList = new ObservableList<IComponent>();

            var vs = VisibilitySensor((v) =>
            {
                obsList.Remove(v);
                obsList.AddRange(GetSomeItems(20));
                v.Reset();
                obsList.Add(v);
            });

            obsList.AddRange(GetSomeItems(10));
            obsList.Add(vs);
            _content = SectionStack().WidthStretch()
                        .Title(SampleHeader(nameof(ItemsListSample)))
                    .Section(
                        Stack()
                            .Children(
                                SampleTitle("Overview"),
                                TextBlock("List provides a base component for rendering small sets of items. " +
                                          "It is agnostic of the tile component used, and selection " +
                                          "management. These concerns can be layered separately.")
                                    .PaddingBottom(16.px()),
                                TextBlock("Performance is adequate for smaller lists, for large number of items use VirtualizedList.")
                                    .PaddingBottom(16.px())))
                    .Section(
                        Stack()
                            .Children(
                                SampleTitle("Usage"),
                                TextBlock("Basic List")
                                    .Medium()
                                    .PaddingBottom(16.px()),
                                ItemsList(GetSomeItems(10)).PaddingBottom(16.px()).Height(500.px()).PaddingBottom(32.px()),
                                TextBlock("Basic List with columns")
                                    .Medium()
                                    .PaddingBottom(16.px()),
                                ItemsList(GetSomeItems(100), 25.percent(), 25.percent(), 25.percent(), 25 .percent()).Height(500.px()).PaddingBottom(32.px()),
                                TextBlock("Basic List with VisibilitySensor")
                                    .Medium()
                                    .PaddingBottom(16.px()),
                                ItemsList(obsList, 25.percent(), 25.percent(), 25.percent(), 25.percent()).Height(500.px()).PaddingBottom(32.px()),
                                TextBlock("Basic List with Empty List Message ")
                                    .Medium()
                                    .PaddingBottom(16.px()),
                                ItemsList(new IComponent[0], 25.percent(), 25.percent(), 25.percent(), 25.percent())
                                    .WithEmptyMessage(() => BackgroundArea(Card(TextBlock("Empty list").Padding(16.px()))).WidthStretch().HeightStretch().MinHeight(100.px()))
                                    .Height(500.px())));
        }

        public HTMLElement Render()
        {
            return _content.Render();
        }

        private IComponent[] GetSomeItems(int count)
        {
            return Enumerable
                .Range(1, count)
                .Select(number => Card(TextBlock($"Lorem Ipsum {number}").NonSelectable()).MinWidth(200.px()))
                .ToArray();
        }
    }
}
