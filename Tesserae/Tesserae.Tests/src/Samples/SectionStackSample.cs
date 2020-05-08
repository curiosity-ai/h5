using System;
using Tesserae.Components;
using static H5.dom;
using static Tesserae.UI;
using static Tesserae.Tests.Samples.SamplesHelper;

namespace Tesserae.Tests.Samples
{
    public class SectionStackSample : IComponent
    {
        private IComponent _content;

        public SectionStackSample()
        {
            var stack = SectionStack();

            _content = Stack().Children(SectionStack().Title(SampleHeader(nameof(SectionStackSample)))
                                            .Section(Stack().Children(
                                                    SampleTitle("Overview"),
                                                    TextBlock("A Session Stack is a container-type component that abstracts the implementation of a flexbox in order to define the layout of its children components.")))
                                            .Section(Stack().Children(
                                                    SampleTitle("Usage"),
                                                    Label("Number of items:").SetContent(Slider(5, 0, 10, 1).OnInput((s, e) => SetChildren(stack, s.Value))))),
                                        stack);
            SetChildren(stack, 5);
        }

        private void SetChildren(SectionStack stack, int count)
        {
            stack.Clear();
            for (int i = 0; i < count; i++)
            {
                stack.Section(Stack().Children(
                TextBlock($"Section {i}").MediumPlus().SemiBold(),
                TextBlock("Wrap (Default)").SmallPlus(),
                TextBlock("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.").Width(50.percent()),
                TextBlock("No Wrap").SmallPlus(),
                TextBlock("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.").NoWrap().Width(50.percent())
                ));
            }
        }

        public HTMLElement Render()
        {
            return _content.Render();
        }
    }
}
