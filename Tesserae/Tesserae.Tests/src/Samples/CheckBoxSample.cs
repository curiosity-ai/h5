using System;
using Tesserae.Components;
using static H5.dom;
using static Tesserae.UI;
using static Tesserae.Tests.Samples.SamplesHelper;

namespace Tesserae.Tests.Samples
{
    public class CheckBoxSample : IComponent
    {
        private IComponent _content;

        public CheckBoxSample()
        {
            _content = SectionStack()
            .Title(SampleHeader(nameof(CheckBoxSample)))
                .Section(Stack().Children(
                    SampleTitle("Overview"),
                    TextBlock("A CheckBox is a UI element that allows users to switch between two mutually exclusive options (checked or unchecked, on or off) through a single click or tap. It can also be used to indicate a subordinate setting or preference when paired with another control."),
                    TextBlock("A CheckBox is used to select or deselect action items. It can be used for a single item or for a list of multiple items that a user can choose from. The control has two selection states: unselected and selected."),
                    TextBlock("Use a single CheckBox for a subordinate setting, such as with a \"Remember me ? \" login scenario or with a terms of service agreement."),
                    TextBlock("For a binary choice, the main difference between a CheckBox and a toggle switch is that the CheckBox is for status and the toggle switch is for action. You can delay committing a CheckBox interaction (as part of a form submit, for example), while you should immediately commit a toggle switch interaction. Also, only CheckBoxes allow for multi-selection."),
                    TextBlock("Use multiple CheckBoxes for multi-select scenarios in which a user chooses one or more items from a group of choices that are not mutually exclusive.")
                    ))
                .Section(Stack().Children(
                    SampleTitle("Best Practices"),
                    Stack().Horizontal().Children(
                        Stack().Width(40.percent()).Children(
                            SampleSubTitle("Do"),
                            SampleDo("Allow users to choose any combination of options when several CheckBoxes are grouped together.")
                        ),
                        Stack().Width(40.percent()).Children(
                            SampleSubTitle("Don't"),
                            SampleDont("Don't use a CheckBox as an on/off control. Instead use a toggle switch."),
                            SampleDont("Don’t use a CheckBox when the user can choose only one option from the group, use radio buttons instead."),
                            SampleDont("Don't put two groups of CheckBoxes next to each other. Separate the two groups with labels.")
                        ))))
                .Section(Stack().Children(
                    SampleTitle("Usage"),
                    TextBlock("Basic CheckBoxes").Medium(),
                    CheckBox("Unchecked checkbox"),
                    CheckBox("Checked checkbox").Checked(),
                    CheckBox("Disabled checkbox").Disabled(),
                    CheckBox("Disabled checked checkbox").Checked().Disabled()
                    ));
        }

        public HTMLElement Render()
        {
            return _content.Render();
        }
    }
}
