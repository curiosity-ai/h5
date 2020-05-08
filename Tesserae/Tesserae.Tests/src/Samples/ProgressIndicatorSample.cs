using System;
using Tesserae.Components;
using static H5.Core.dom;
using static Tesserae.UI;
using static Tesserae.Tests.Samples.SamplesHelper;

namespace Tesserae.Tests.Samples
{
    public class ProgressIndicatorSample : IComponent
    {
        private IComponent _content;

        public ProgressIndicatorSample()
        {
            _content = SectionStack()
                .Title(SampleHeader(nameof(ProgressIndicatorSample)))
                .Section(Stack().Children(
                    SampleTitle("Overview"),
                    TextBlock(
                        "ProgressIndicators are used to show the completion status of an operation lasting more than 2 seconds. If the state of progress cannot be determined, use a Spinner instead. ProgressIndicators can appear in a new panel, a flyout, under the UI initiating the operation, or even replacing the initiating UI, as long as the UI can return if the operation is canceled or is stopped."))
                )
                .Section(Stack().WidthStretch().Children(
                    SampleTitle("Best Practices"),
                    Stack().WidthStretch().Horizontal().Children(
                        Stack().WidthStretch().Children(
                            SampleSubTitle("Do"),
                            SampleDo("Use a ProgressIndicator when the total units to completion is known"),
                            SampleDo("Display operation description"),
                            SampleDo("Show text above and/or below the bar"),
                            SampleDo("Combine steps of a single operation into one bar")
                        ),
                        Stack().WidthStretch().Children(
                            SampleSubTitle("Don't"),
                            SampleDont("Use a ProgressIndicator when the total units to completion is indeterminate."),
                            SampleDont("Show text to the right or left of the bar"),
                            SampleDont("Cause progress to “rewind” to show new steps")
                        ))
                ))
                .Section(
                    Stack().Children(
                        SampleTitle("Usage"),
                        TextBlock("States").Medium(),
                        Label("Empty").SetContent(ProgressIndicator().Progress(0).Width(400.px())).AlignCenter(),
                        Label("30%").SetContent(ProgressIndicator().Progress(30).Width(400.px())).AlignCenter(),
                        Label("60%").SetContent(ProgressIndicator().Progress(60).Width(400.px())).AlignCenter(),
                        Label("Full").SetContent(ProgressIndicator().Progress(100).Width(400.px())).AlignCenter(),
                        Label("Indeterminate").SetContent(ProgressIndicator().Indeterminated().Width(400.px())).AlignCenter()
                    ));
        }

        public HTMLElement Render()
        {
            return _content.Render();
        }
    }
}
