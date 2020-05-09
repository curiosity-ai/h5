using System;
using Tesserae.Components;
using static H5.Core.dom;
using static Tesserae.UI;
using static Tesserae.Tests.Samples.SamplesHelper;

namespace Tesserae.Tests.Samples
{
    public class StackSample : IComponent
    {
        private IComponent _content;

        public StackSample()
        {
            var stack = Stack();
            var countSlider = Slider(5, 0, 10, 1);
            _content = SectionStack()
            .Title(SampleHeader(nameof(StackSample)))
            .Section(Stack().Children(
                SampleTitle("Overview"),
                TextBlock("A Stack is a container-type component that abstracts the implementation of a flexbox in order to define the layout of its children components.")))
            .Section(Stack().Children(
                SampleTitle("Usage"),
                Stack().Children(
                    Stack().Horizontal().Children(
                    Stack().Children(
                    Label("Number of items:").SetContent(countSlider.OnInput((s, e) => SetChildren(stack, s.Value))),
                    Stack().Horizontal().Children(
                    ChoiceGroup("Orientation:").Horizontal().Choices(Choice("Vertical").Selected(), Choice("Horizontal"), Choice("Vertical Reverse"), Choice("Horizontal Reverse")).OnChange(
                        (s, e) =>
                        {
                            if (s.SelectedOption.Text == "Horizontal")
                                stack.Horizontal();
                            else if (s.SelectedOption.Text == "Vertical")
                                stack.Vertical();
                            else if (s.SelectedOption.Text == "Horizontal Reverse")
                                stack.HorizontalReverse();
                            else if (s.SelectedOption.Text == "Vertical Reverse")
                                stack.VerticalReverse();
                        })
                    )
                    )
                ),
                stack.HeightAuto())));
                SetChildren(stack, 5);
        }

        private void SetChildren(Stack stack, int count)
        {
            stack.Clear();
            for (int i = 0; i < count; i++)
            {
                stack.Add(Button(i.ToString()));
            }
        }

        public HTMLElement Render()
        {
            return _content.Render();
        }
    }
}
