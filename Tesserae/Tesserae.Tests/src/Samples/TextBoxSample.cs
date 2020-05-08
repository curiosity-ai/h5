using System;
using Tesserae.Components;
using static H5.Core.dom;
using static Tesserae.UI;
using static Tesserae.Tests.Samples.SamplesHelper;

namespace Tesserae.Tests.Samples
{
    public class TextBoxSample : IComponent
    {
        private IComponent _content;

        public TextBoxSample()
        {
            _content = SectionStack()
            .Title(SampleHeader(nameof(TextBoxSample)))
            .Section(Stack().Children(
                SampleTitle("Overview"),
                TextBlock("The TextBox component enables a user to type text into an app. The text displays on the screen in a simple, uniform format.")))
            .Section(Stack().Children(
                SampleTitle("Best Practices"),
                Stack().Horizontal().Children(
                Stack().Width(40.percent()).Children(
                    SampleSubTitle("Do"),
                    SampleDo("Use the TextBox to accept data input on a form or page."),
                    SampleDo("Label the TextBox with a helpful name."),
                    SampleDo("Provide concise helper text that specifies what content is expected to be entered."),
                    SampleDo("When part of a form, provide clear designations for which TextBox are required vs. optional."),
                    SampleDo("Provide all appropriate methods for submitting provided data (e.g. dedicated ‘Submit’ button)."),
                    SampleDo("Provide all appropriate methods of clearing provided data (‘X’ or something similar)."),
                    SampleDo("Allow for selection, copy and paste of field data."),
                    SampleDo("Ensure that the TextBox is functional through use of mouse/keyboard or touch when available.")
                ),
                Stack().Width(40.percent()).Children(
                    SampleSubTitle("Don't"),
                    SampleDont("Don't use a TextBox to render basic copy as part of a body element of a page."),
                    SampleDont("Don't provide an unlabeled TextBox and expect that users will know what to do with it."),
                    SampleDont("Don't place a TextBox inline with body copy."),
                    SampleDont("Don't be overly verbose with helper text."),
                    SampleDont("Don't occlude the entry or allow entry when the active content is not visible.")
                )
            )))
            .Section(Stack().Children(
                SampleTitle("Usage"),
                TextBlock("Basic TextBox").Medium(),
                Stack().Width(40.percent()).Children(
                    Label("Standard").SetContent(TextBox()),
                    Label("Disabled").Disabled().SetContent(TextBox("I am disabled").Disabled()),
                    Label("Read-only").SetContent(TextBox("I am read-only").ReadOnly()),
                    Label("Password").SetContent(TextBox("I am a password box").Password()),
                    Label("Required").Required().SetContent(TextBox("")),
                    TextBox("").Required(),
                    Label("With error message").SetContent(TextBox().Error("Error message").IsInvalid()),
                    Label("With placeholder").SetContent(TextBox().SetPlaceholder("Please enter text here")),
                    Label("With validation").SetContent(TextBox().Validation((tb) => tb.Text.Length == 0 ? "Empty" : null)),
                    Label("With validation on type").SetContent(TextBox().Validation(Validation.NonZeroPositiveInteger)),
                    Label("Disabled with placeholder").Disabled().SetContent(TextBox().SetPlaceholder("I am disabled").Disabled()))));
        }

        public HTMLElement Render()
        {
            return _content.Render();
        }
    }
}
