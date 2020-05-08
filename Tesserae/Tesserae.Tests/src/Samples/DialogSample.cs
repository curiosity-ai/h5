using System;
using Tesserae.Components;
using static H5.Core.dom;
using static Tesserae.UI;
using static Tesserae.Tests.Samples.SamplesHelper;

namespace Tesserae.Tests.Samples
{
    public class DialogSample : IComponent
    {
        private IComponent _content;

        public DialogSample()
        {
            var dialog = Dialog("Lorem Ipsum");
            var response = TextBlock();

            _content = SectionStack()
                .Title(SampleHeader(nameof(DialogSample)))
            .Section(Stack().Children(
                SampleTitle("Overview"),
                TextBlock("Dialogs are temporary, modal UI overlay that generally provide contextual app information or require user confirmation/input. In most cases, Dialogs block interactions with the web page or application until being explicitly dismissed, and often request action from the user. They are primarily used for lightweight creation or edit tasks, and simple management tasks."),
                TextBlock("Best Practices").MediumPlus()))
            .Section(Stack().Children(
                Stack().Horizontal().Children(
                Stack().Width(40.percent()).Children(
                SampleSubTitle("Do"),
                SampleDo("Use Dialogs for quick, actionable interactions, such as making a choice or needing the user to provide information."),
                SampleDo("When possible, try a non-blocking Dialog before resorting to a blocking Dialog."),
                SampleDo("Only include information needed to help users make a decision."),
                SampleDo("Button text should reflect the actions available to the user (e.g. save, delete)."),
                SampleDo("Validate that the user's entries are acceptable before closing the Dialog. Show an inline validation error near the field they must correct.")),
            Stack().Width(40.percent()).Children(
                SampleSubTitle("Don't"),
                SampleDont("Don’t overuse Modals. In some cases they can be perceived as interrupting workflow, and too many can be a bad user experience."),
                SampleDont("Avoid \"Are you sure ?\" or confirmation Dialogs unless the user is making an irreversible or destructive choice."),
                SampleDont("Do not use a blocking Dialog unless absolutely necessary because they are very disruptive."),
                SampleDont("Don’t have long sentences or complicated choices."),
                SampleDont("Avoid generic button labels like \"Ok\" if you can be more specific about the action a user is about to complete."),
                SampleDont("Don't dismiss the Dialog if underlying problem is not fixed. Don't put the user back into a broken/error state."),
                SampleDont("Don't provide the user with more than 3 buttons.")))))
            .Section(Stack().Children(
                SampleTitle("Usage"),
                Button("Open Dialog").OnClick((c, ev) => dialog.Show()),
                Stack().Horizontal().Children(
                Button("Open YesNo").OnClick((c, ev)             => Dialog("Sample Dialog").YesNo(() => response.Text("Clicked Yes"), () => response.Text("Clicked No"))),
                Button("Open YesNoCancel").OnClick((c, ev)       => Dialog("Sample Dialog").YesNoCancel(() => response.Text("Clicked Yes"), () => response.Text("Clicked No"), () => response.Text("Clicked Cancel"))),
                Button("Open Ok").OnClick((c, ev)                => Dialog("Sample Dialog").Ok(() => response.Text("Clicked Ok"))),
                Button("Open RetryCancel").OnClick((c, ev)       => Dialog("Sample Dialog").RetryCancel(() => response.Text("Clicked Retry"), () => response.Text("Clicked Cancel")))),
                Button("Open YesNo with dark overlay").OnClick((c, ev)       => Dialog("Sample Dialog").Dark().YesNo(() => response.Text("Clicked Yes"), () => response.Text("Clicked No"), y => y.Success().SetText("Yes!"), n => n.Danger().SetText("Nope"))),
                Button("Open YesNoCancel with dark overlay").OnClick((c, ev) => Dialog("Sample Dialog").Dark().YesNoCancel(() => response.Text("Clicked Yes"), () => response.Text("Clicked No"), () => response.Text("Clicked Cancel"))),
                Button("Open Ok with dark overlay").OnClick((c, ev)          => Dialog("Sample Dialog").Dark().Ok(() => response.Text("Clicked Ok"))),
                Button("Open RetryCancel with dark overlay").OnClick((c, ev) => Dialog("Sample Dialog").Dark().RetryCancel(() => response.Text("Clicked Retry"), () => response.Text("Clicked Cancel"))),
                response));
                dialog.Content(Stack().Children(TextBlock("Lorem ipsum dolor sit amet, consectetur adipiscing elit."),
                                                Toggle("Is draggable").OnChange((c, ev) => dialog.IsDraggable = c.IsChecked),
                                                Toggle("Is dark overlay").OnChange((c, ev) => dialog.IsDark = c.IsChecked).Checked(dialog.IsDark)
                                                ))
                      .Commands(Button("Send").Primary().AlignEnd().OnClick((c, ev) => dialog.Hide()), Button("Don`t send").AlignEnd().OnClick((c, ev) => dialog.Hide()));
        }

        public HTMLElement Render()
        {
            return _content.Render();
        }
    }
}
