using System;
using Tesserae.Components;
using static H5.Core.dom;
using static Tesserae.UI;
using static Tesserae.Tests.Samples.SamplesHelper;

namespace Tesserae.Tests.Samples
{
    public class TextBlockSample : IComponent
    {
        private IComponent _content;

        public TextBlockSample()
        {
            _content = SectionStack()
                        .Title(SampleHeader(nameof(TextBlockSample)))
                        .Section(Stack().Children(
                            SampleTitle("Overview"),
                            TextBlock("Text is a component for displaying text. You can use Text to standardize text across your web app.")))
                        .Section(Stack().Children(
                            SampleTitle("Usage"),
                            TextBlock("TextBox Ramp Example").Medium(),
                            Stack().Horizontal().Children(TextBlock("Variant").Width(200.px()).SemiBold(), TextBlock("Example").SemiBold()),
                            Stack().Horizontal().Children(TextBlock("tiny").Width(200.px()), TextBlock("The quick brown fox jumped over the lazy dog.").Tiny()),
                            Stack().Horizontal().Children(TextBlock("xSmall").Width(200.px()), TextBlock("The quick brown fox jumped over the lazy dog.").XSmall()),
                            Stack().Horizontal().Children(TextBlock("small").Width(200.px()), TextBlock("The quick brown fox jumped over the lazy dog.").Small()),
                            Stack().Horizontal().Children(TextBlock("smallPlus").Width(200.px()), TextBlock("The quick brown fox jumped over the lazy dog.").SmallPlus()),
                            Stack().Horizontal().Children(TextBlock("medium").Width(200.px()), TextBlock("The quick brown fox jumped over the lazy dog.").Medium()),
                            Stack().Horizontal().Children(TextBlock("mediumPlus").Width(200.px()), TextBlock("The quick brown fox jumped over the lazy dog.").MediumPlus()),
                            Stack().Horizontal().Children(TextBlock("large").Width(200.px()), TextBlock("The quick brown fox jumped over the lazy dog.").Large()),
                            Stack().Horizontal().Children(TextBlock("xLarge").Width(200.px()), TextBlock("The quick brown fox jumped over the lazy dog.").XLarge()),
                            Stack().Horizontal().Children(TextBlock("xxLarge").Width(200.px()), TextBlock("The quick brown fox jumped over the lazy dog.").XXLarge()),
                            Stack().Horizontal().Children(TextBlock("mega").Width(200.px()), TextBlock("The quick brown fox jumped over the lazy dog.").Mega()),
                            TextBlock("TextBox Wrap Example").Medium(),
                            TextBlock("Wrap (Default)").SmallPlus(),
                            TextBlock("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.").Width(50.percent()),
                            TextBlock("No Wrap").SmallPlus(),
                            TextBlock("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.").NoWrap().Width(50.percent())));
        }

        public HTMLElement Render()
        {
            return _content.Render();
        }
    }
}
