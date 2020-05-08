using System;
using Tesserae.Components;
using static H5.Core.dom;
using static Tesserae.UI;
using static Tesserae.Tests.Samples.SamplesHelper;

namespace Tesserae.Tests.Samples
{
    public class SpinnerSample : IComponent
    {
        private IComponent _content;

        public SpinnerSample()
        {
            _content = SectionStack()
                .Title(SampleHeader(nameof(SpinnerSample)))
                .Section(Stack().Children(
                    SampleTitle("Overview"),
                    TextBlock(
                        "A Spinner is an outline of a circle which animates around itself indicating to the user that things are processing. A Spinner is shown when it's unsure how long a task will take making it the indeterminate version of a ProgressIndicator. They can be various sizes, located inline with content or centered. They generally appear after an action is being processed or committed. They are subtle and generally do not take up much space, but are transitions from the completed task."))
                )
                .Section(Stack().WidthStretch().Children(
                    SampleTitle("Best Practices"),
                    Stack().WidthStretch().Horizontal().Children(
                        Stack().WidthStretch().Children(
                            SampleSubTitle("Do"),
                            SampleDo("Use a Spinner when a task is not immediate."),
                            SampleDo("Use one Spinner at a time."),
                            SampleDo("Descriptive verbs are appropriate under a Spinner to help the user understand what's happening. Ie: Saving, processing, updating."),
                            SampleDo("Use a Spinner when confirming a change has been made or a task is being processed.")
                        ),
                        Stack().WidthStretch().Children(
                            SampleSubTitle("Don't"),
                            SampleDont("Don’t use a Spinner when performing immediate tasks."),
                            SampleDont("Don't show multiple Spinners at the same time."),
                            SampleDont("Don't include more than a few words when paired with a Spinner.")
                        ))
                ))
                .Section(
                    Stack().Width(400.px()).Children(
                        SampleTitle("Usage"),
                        TextBlock("Spinner sizes").Medium(),
                        Label("Extra small spinner").SetContent(Spinner().XSmall()).AlignCenter(),
                        Label("Small spinner").SetContent(Spinner().Small()).AlignCenter(),
                        Label("Medium spinner").SetContent(Spinner().Medium()).AlignCenter(),
                        Label("Large spinner").SetContent(Spinner().Large()).AlignCenter()
                    ))
                .Section(
                    Stack().Width(400.px()).Children(
                        TextBlock("Spinner label positioning").Medium(),
                        Label("Spinner with label positioned below").SetContent(Spinner("I am definitely loading...").Below()),
                        Label("Spinner with label positioned above").SetContent(Spinner("Seriously, still loading...").Above()),
                        Label("Spinner with label positioned to right").SetContent(Spinner("Wait, wait...").Right()),
                        Label("Spinner with label positioned to left").SetContent(Spinner("Nope, still loading...").Left())
                    ));
        }

        public HTMLElement Render()
        {
            return _content.Render();
        }
    }
}
