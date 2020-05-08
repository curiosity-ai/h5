using System;
using System.Threading.Tasks;
using Tesserae.Components;
using static H5.dom;
using static Tesserae.UI;
using static Tesserae.Tests.Samples.SamplesHelper;

namespace Tesserae.Tests.Samples
{
    public class DeferSample: IComponent
    {
        private IComponent content;

        public DeferSample()
        {
            var stack = SectionStack();
            var countSlider = Slider(5, 0, 10, 1);
            content = SectionStack()
                .Title(SampleHeader(nameof(DeferSample)))
                .Section(Stack().Children(
                    SampleTitle("Overview"),
                    TextBlock("Use Defer() to render asyncronous components. The asyncronous task is only triggered on the first render of the Defer component")))
                .Section(Stack().Children(
                    SampleTitle("Usage"),
                Stack().Children(
                    Stack().Horizontal().Children(
                        Stack().Children(
                            Label("Number of items:").SetContent(countSlider.OnInput((s, e) => SetChildren(stack, s.Value)))
                            )
                        )
                    ),
                    stack.HeightAuto()
            ));
            SetChildren(stack, 5);
        }

        private void SetChildren(SectionStack stack, int count)
        {
            stack.Clear();
            for (int i = 0; i < count; i++)
            {
                var delay = (i + 1) * 1_000;
                stack.Section(Stack().Children(
                TextBlock($"Section {i} - delayed {i + 1} seconds").MediumPlus().SemiBold(),

                Defer(async () =>
                {
                    await Task.Delay(delay);
                    return Stack().Children(
                        TextBlock("Wrap (Default)").SmallPlus(),
                        TextBlock("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.").Width(50.percent()),
                        TextBlock("No Wrap").SmallPlus(),
                        TextBlock("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.").NoWrap().Width(50.percent())
                    );
                }
                )));
            }
        }

        public HTMLElement Render()
        {
            return content.Render();
        }
    }
}
