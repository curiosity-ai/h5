using System;
using System.Collections.Generic;
using System.Linq;
using Tesserae.Components;
using static H5.dom;
using static Tesserae.UI;
using static Tesserae.Tests.Samples.SamplesHelper;

namespace Tesserae.Tests.Samples
{
    public class PickerSample : IComponent
    {
        private readonly IComponent _content;

        public PickerSample()
        {
            _content =
                SectionStack()
                    .Title(SampleHeader(nameof(PickerSample)))
                    .Section(
                        Stack()
                            .Children(
                                SampleTitle("Overview"),
                                TextBlock("Pickers are used to pick recipients.")))
                    .Section(
                        Stack()
                            .Width(40.percent())
                            .Children(
                                SampleTitle("Usage"),
                                TextBlock("Picker with text suggestions and tag-like selections")
                                    .Medium()
                                    .PaddingBottom(16.px()),
                                Picker<PickerSampleItem>(suggestionsTitleText: "Suggested Tags").WithItems(GetPickerItems())
                                    .PaddingBottom(32.px()),
                                TextBlock("Picker with single selection")
                                    .Medium()
                                    .PaddingBottom(16.px()),
                                Picker<PickerSampleItem>(suggestionsTitleText: "Suggested Tags", maximumAllowedSelections:1).WithItems(GetPickerItems())
                                    .PaddingBottom(32.px()),
                                TextBlock("Picker with icon and text suggestions and component based selections")
                                    .Medium()
                                    .PaddingBottom(16.px()),
                                Picker<PickerSampleItemWithComponents>(suggestionsTitleText: "Suggested Items", renderSelectionsInline: false).WithItems(GetComponentPickerItems())));
        }

        public HTMLElement Render()
        {
            return _content.Render();
        }

        private PickerSampleItem[] GetPickerItems()
        {
            return new []
            {
                new PickerSampleItem("Bob"),
                new PickerSampleItem("BOB"),
                new PickerSampleItem("Donuts by J Dilla"),
                new PickerSampleItem("Donuts"),
                new PickerSampleItem("Coffee"),
                new PickerSampleItem("Chicken Coop"),
                new PickerSampleItem("Cherry Pie"),
                new PickerSampleItem("Chess"),
                new PickerSampleItem("Cooper")
            };
        }

        private PickerSampleItemWithComponents[] GetComponentPickerItems()
        {
            return new []
            {
                new PickerSampleItemWithComponents("Bob", LineAwesome.Bomb),
                new PickerSampleItemWithComponents("BOB", LineAwesome.Blender),
                new PickerSampleItemWithComponents("Donuts by J Dilla", LineAwesome.Carrot),
                new PickerSampleItemWithComponents("Donuts", LineAwesome.CarBattery),
                new PickerSampleItemWithComponents("Coffee", LineAwesome.Coffee),
                new PickerSampleItemWithComponents("Chicken Coop", LineAwesome.Hamburger),
                new PickerSampleItemWithComponents("Cherry Pie", LineAwesome.ChartPie),
                new PickerSampleItemWithComponents("Chess", LineAwesome.Chess),
                new PickerSampleItemWithComponents("Cooper", LineAwesome.QuestionCircle)
            };
        }
    }

    public class PickerSampleItem : IPickerItem
    {
        public PickerSampleItem(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public bool IsSelected { get; set; }

        public IComponent Render()
        {
            return TextBlock(Name);
        }
    }

    public class PickerSampleItemWithComponents : IPickerItem
    {
        private LineAwesome _icon;

        public PickerSampleItemWithComponents(string name, LineAwesome icon)
        {
            Name = name;
            _icon = icon;
        }

        public string Name { get; }

        public bool IsSelected { get; set; }

        public IComponent Render()
        {
            return Stack().Horizontal().AlignContent(ItemAlign.Center).Children(Icon(_icon).MinWidth(16.px()), TextBlock(Name));
        }
    }
}
