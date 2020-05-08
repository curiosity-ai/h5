using System;
using Tesserae.Components;
using static H5.dom;
using static Tesserae.UI;
using static Tesserae.Tests.Samples.SamplesHelper;

namespace Tesserae.Tests.Samples
{
    public class ToggleSample : IComponent
    {
        private IComponent _content;

        public ToggleSample()
        {
            _content = SectionStack()
                        .Title(SampleHeader(nameof(ToggleSample)))
                        .Section(Stack().Children(
                            SampleTitle("Overview"),
                            TextBlock("Toggles represent a physical switch that allows users to turn things on or off. Use Toggles to present users with two mutually exclusive options (like on/off), where choosing an option results in an immediate action. Use a Toggle for binary operations that take effect right after the user flips the Toggle. For example, use a Toggle to turn services or hardware components on or off. In other words, if a physical switch would work for the action, a Toggle is probably the best control to use."),
                            TextBlock("Choosing between Toggle and Checkbox").Medium(),
                            TextBlock("For some actions, either a Toggle or a Checkbox might work. To decide which control would work better, follow these tips:"),
                            TextBlock("Use a Toggle for binary settings when changes become effective immediately after the user changes them."),
                            TextBlock("In the above example, it's clear with the Toggle that the wireless is set to \"On.\" But with the Checkbox, the user needs to think about whether the wireless is on now or whether they need to check the box to turn wireless on."),
                            TextBlock("Use a Checkbox when the user has to perform extra steps for changes to be effective. For example, if the user must click a \"Submit\", \"Next\", \"Ok\" button to apply changes, use a Checkbox.")))
                        .Section(Stack().Children(
                            SampleTitle("Best Practices"),
                            Stack().Horizontal().Children(
                            Stack().Width(40.percent()).Children(
                            SampleSubTitle("Do"),
                            SampleDo("Only replace the On and Off labels if there are more specific labels for the setting. If there are short (3-4 characters) labels that represent binary opposites that are more appropriate for a particular setting, use them. ")
                        ),
                        Stack().Width(40.percent()).Children(
                            SampleSubTitle("Don't"),
                            SampleDont("Don’t use a Toggle if the user will have to do something else or go somewhere else in order to experience its effect. If any extra step is required for changes to be effective, you should use a checkbox and corresponding \"Apply\" button instead of a Toggle.")
                        )
                        )))
                        .Section(Stack().Children(
                            SampleTitle("Usage"),
                            TextBlock("Basic Toggles").Medium(),
                            Label("Enabled and checked").SetContent(Toggle().Checked()),
                            Label("Enabled and unchecked").SetContent(Toggle()),
                            Label("Disabled and checked").SetContent(Toggle().Checked().Disabled()),
                            Label("Disabled and unchecked").SetContent(Toggle().Disabled()),
                            Label("With inline label").Inline().SetContent(Toggle()),
                            Label("Disabled with inline label").Inline().SetContent(Toggle().Disabled()).Disabled(),
                            Toggle("With inline label and without onText and offText"),
                            Toggle("Disabled with inline label and without onText and offText").Disabled()));
        }

        public HTMLElement Render()
        {
            return _content.Render();
        }
    }
}
