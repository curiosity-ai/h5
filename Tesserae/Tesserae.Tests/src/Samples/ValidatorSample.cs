using Tesserae.Components;
using static H5.dom;
using static Tesserae.UI;
using static Tesserae.Tests.Samples.SamplesHelper;

namespace Tesserae.Tests.Samples
{
    public class ValidatorSample : IComponent
    {
        private IComponent content;

        public ValidatorSample()
        {
            var isAllValid = TextBlock("?");
            var validator = Validator().OnValidation((valid) => isAllValid.Text = valid ? "Valid ✔" : "Invalid ❌");

            var tb1 = TextBox();
            var tb2 = TextBox();
            tb1.Validation((tb) => tb.Text.Length == 0 ? "Empty" : ((tb1.Text == tb2.Text) ? "Duplicated  values" : null), validator);
            tb2.Validation((tb) => Validation.NonZeroPositiveInteger(tb) ?? ((tb1.Text == tb2.Text) ? "Duplicated values" : null), validator);

            content = SectionStack()
                        .Title(SampleHeader(nameof(ValidatorSample)))
                        .Section(Stack().Children(
                            SampleTitle("Overview"),
                            TextBlock("The validator helper allows you to capture the state of multiple components registered on it.")))
                        .Section(Stack().Children(
                            SampleTitle("Best Practices"),
                            Stack().Horizontal().Children(
                                Stack().Width(40.percent()).Children(
                                    SampleSubTitle("Do"),
                                    SampleDo("TODO")),
                                Stack().Width(40.percent()).Children(
                                    SampleSubTitle("Don't"),
                                    SampleDont("TODO")))))
                        .Section(
                            Stack().Children(
                                SampleTitle("Usage"),
                                TextBlock("Basic TextBox").Medium(),
                                Stack().Width(40.percent()).Children(
                                    Label("Non-empty").SetContent(tb1),
                                    Label("Integer > 0").SetContent(tb2),
                                    Label("Is all valid").SetContent(isAllValid),
                                    Label("Test revalidation (will fail if repeated)").SetContent(Button("Revalidate").OnClick((s,b) => validator.Revalidate())))));
        }

        public HTMLElement Render()
        {
            return content.Render();
        }
    }
}
