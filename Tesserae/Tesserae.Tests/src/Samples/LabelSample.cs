using System;
using Tesserae.Components;
using static H5.Core.dom;
using static Tesserae.UI;
using static Tesserae.Tests.Samples.SamplesHelper;

namespace Tesserae.Tests.Samples
{
    public class LabelSample : IComponent
    {
        private IComponent _content;

        public LabelSample()
        {
            _content = SectionStack()
                        .Title(SampleHeader(nameof(LabelSample)))
                        .Section(Stack().Children(
                            SampleTitle("Overview"),
                            TextBlock("Labels give a name or title to a component or group of components. Labels should be in close proximity to the component or group they are paired with. Some components, such as TextField, Dropdown, or Toggle, already have Labels incorporated, but other components may optionally add a Label if it helps inform the user of the component’s purpose.")))
                        .Section(Stack().Children(
                            SampleTitle("Best Practices"),
                            Stack().Horizontal().Children(
                            Stack().Width(40.percent()).Children(
                                SampleSubTitle("Do"),
                                SampleDo("Use sentence casing, e.g. “First name”."),
                                SampleDo("Be short and concise."),
                                SampleDo("When adding a Label to components, use the text as a noun or short noun phrase.")
                                ),
                        Stack().Width(40.percent()).Children(
                            SampleSubTitle("Don't"),
                            SampleDo("Use Labels as instructional text, e.g. “Click to get started”."),
                            SampleDo("Don’t use full sentences or complex punctuation (colons, semicolons, etc.).")))))
                            .Section(Stack().Children(
                                SampleTitle("Usage"),
                                Label("I'm Label"),
                                Label("I'm a disabled Label").Disabled(),
                                Label("I'm a required Label").Required(),
                                Label("I'm a primary Label").Primary(),
                                Label("I'm a secondary Label").Secondary(),
                                Label("I'm a tiny Label").Regular().Tiny(),
                                Label("A Label for An Input").SetContent(TextBox()),

                                TextBlock("Inline without auto-width").Medium().PaddingTop(16.px()).PaddingBottom(8.px()),
                                Stack().Children(
                                    Label("Lbl").Inline().SetContent(TextBox()),
                                    Label("Label").Inline().SetContent(TextBox()),
                                    Label("Bigger Label").Inline().SetContent(TextBox()),
                                    Label("The Biggest Label").Inline().SetContent(TextBox())
                                ),

                                TextBlock("Inline with auto-width").Medium().PaddingTop(16.px()).PaddingBottom(8.px()),
                                Stack().Children(
                                    Label("Lbl").Inline().AutoWidth().SetContent(TextBox()),
                                    Label("Label").Inline().AutoWidth().SetContent(TextBox()),
                                    Label("Bigger Label").Inline().AutoWidth().SetContent(TextBox()),
                                    Label("The Biggest Label").Inline().AutoWidth().SetContent(TextBox())
                                ),

                                TextBlock("Inline with auto-width, aligned right").Medium().PaddingTop(16.px()).PaddingBottom(8.px()),
                                Stack().Children(
                                    Label("Lbl").Inline().AutoWidth(alignRight:true).SetContent(TextBox()),
                                    Label("Label").Inline().AutoWidth(alignRight: true).SetContent(TextBox()),
                                    Label("Bigger Label").Inline().AutoWidth(alignRight: true).SetContent(TextBox()),
                                    Label("The Biggest Label").Inline().AutoWidth(alignRight: true).SetContent(TextBox())
                                )

                                ));
        }

        public HTMLElement Render()
        {
            return _content.Render();
        }
    }
}
