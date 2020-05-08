using System;
using Tesserae.Components;
using static H5.Core.dom;
using static Tesserae.UI;
using static Tesserae.Tests.Samples.SamplesHelper;

namespace Tesserae.Tests.Samples
{
    public class HorizontalSeparatorSample : IComponent
    {
        private IComponent _content;

        public HorizontalSeparatorSample()
        {
            _content = SectionStack()
            .Title(SampleHeader(nameof(HorizontalSeparatorSample)))
            .Section(Stack().Children(
                SampleTitle("Overview"),
                TextBlock("A separator visually separates content into groups."),
                TextBlock("You can render content in the separator by specifying the component's children. The component's children can be plain text or a component like Icon. The content is center-aligned by default.")))
            .Section(Stack().Children(
                SampleTitle("Best Practices"),
                Stack().Horizontal().Children(
                Stack().Width(40.percent()).Children(
                    SampleSubTitle("Do"),
                    SampleDo("Explain what is the group this separator introduces"),
                    SampleDo("Be short and concise.")
                    ),
                Stack().Width(40.percent()).Children(
                    SampleSubTitle("Don't"),
                    SampleDont("Use long group names")))))
            .Section(Stack().Children(
                    SampleTitle("Usage"),
                    HorizontalSeparator("Center"),
                    HorizontalSeparator("Left").Left(),
                    HorizontalSeparator("Right").Right(),
                    SampleTitle("Custom Separators"),
                    HorizontalSeparator(Stack().Horizontal().Children(Icon("las la-plane").AlignCenter().PaddingRight(8.px()), TextBlock("Custom Center").SemiBold().MediumPlus().AlignCenter())).Primary(),
                    HorizontalSeparator(Stack().Horizontal().Children(Icon("las la-plane").AlignCenter().PaddingRight(8.px()), TextBlock("Custom Left").SemiBold().MediumPlus().AlignCenter())).Primary().Left(),
                    HorizontalSeparator(Stack().Horizontal().Children(Icon("las la-plane").AlignCenter().PaddingRight(8.px()), TextBlock("Custom Right").SemiBold().MediumPlus().AlignCenter())).Primary().Right()));
        }

        public HTMLElement Render()
        {
            return _content.Render();
        }
    }
}
