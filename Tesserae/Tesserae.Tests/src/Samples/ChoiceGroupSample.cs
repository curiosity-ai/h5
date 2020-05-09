using System;
using Tesserae.Components;
using static H5.Core.dom;
using static Tesserae.UI;
using static Tesserae.Tests.Samples.SamplesHelper;

namespace Tesserae.Tests.Samples
{
    public class ChoiceGroupSample : IComponent
    {
        private IComponent _content;

        public ChoiceGroupSample()
        {
            _content = SectionStack()
            .Title(SampleHeader(nameof(ChoiceGroupSample)))
            .Section(Stack().Children(
                SampleTitle("Overview"),
                TextBlock("The ChoiceGroup component, also known as radio buttons, let users select one option from two or more choices. Each option is represented by one ChoiceGroup button; a user can select only one ChoiceGroup in a button group."),
                TextBlock("ChoiceGroup emphasize all options equally, and that may draw more attention to the options than necessary. Consider using other controls, unless the options deserve extra attention from the user. For example, if the default option is recommended for most users in most situations, use a Dropdown component instead."),
                TextBlock("If there are only two mutually exclusive options, combine them into a single Checkbox or Toggle switch. For example, use a Checkbox for \"I agree\" instead of ChoiceGroup buttons for \"I agree\" and \"I don't agree.\"")))
            .Section(Stack().Children(
                SampleTitle("Best Practices"),
                Stack().Horizontal().Children(
                    Stack().Width(40.percent()).Children(
                        SampleSubTitle("Do"),
                        SampleDo("Use when there are 2-7 options, if you have enough screen space and the options are important enough to be a good use of that screen space. Otherwise, use a Checkbox or Dropdown list."),
                        SampleDo("Use on wizard pages to make the alternatives clear, even if a Checkbox is otherwise acceptable."),
                        SampleDo("List the options in a logical order, such as most likely to be selected to least, simplest operation to most complex, or least risk to most. Alphabetical ordering is not recommended because it is language dependent and therefore not localizable."),
                        SampleDo("If none of the options is a valid choice, add another option to reflect this choice, such as \"None\" or \"Does not apply\"."),
                        SampleDo("Select the safest (to prevent loss of data or system access) and most secure and private option as the default. If safety and security aren't factors, select the most likely or convenient option."),
                        SampleDo("Align radio buttons vertically instead of horizontally, if possible. Horizontal alignment is harder to read and localize.")),
                    Stack().Width(40.percent()).Children(
                        SampleSubTitle("Don't"),
                        SampleDont("Use when the options are numbers that have fixed steps, like 10, 20, 30. Use a Slider component instead."),
                        SampleDont("Use if there are more than 7 options, use a Dropdown instead."),
                        SampleDont("Nest with other ChoiceGroup or CheckBoxes. If possible, keep all the options at the same level.")))))
            .Section(
                Stack().Children(
                    SampleTitle("Usage"),
                    TextBlock("Default ChoiceGroup").Medium(),
                    ChoiceGroup().Choices(
                        Choice("Option A"),
                        Choice("Option B"),
                        Choice("Option C").Disabled(),
                        Choice("Option D")),
                    TextBlock("Required ChoiceGroup with a custom label").Medium(),
                    ChoiceGroup("Custom label").Required().Choices(
                        Choice("Option A"),
                        Choice("Option B"),
                        Choice("Option C").Disabled(),
                        Choice("Option D")),
                    TextBlock("Horizontal ChoiceGroup").Medium(),
                    ChoiceGroup().Horizontal().Choices(
                        Choice("Option A"),
                        Choice("Option B"),
                        Choice("Option C").Disabled(),
                        Choice("Option D"))));
        }

        public HTMLElement Render()
        {
            return _content.Render();
        }
    }
}
