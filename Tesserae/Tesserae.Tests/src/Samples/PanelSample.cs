using Tesserae.Components;
using static H5.dom;
using static Tesserae.Tests.Samples.SamplesHelper;
using static Tesserae.UI;
using Panel = Tesserae.Components.Panel;

namespace Tesserae.Tests.Samples
{
    public class PanelSample : IComponent
    {
        private IComponent _content;

        public PanelSample()
        {
            var panel = Panel().LightDismiss();
            panel.Content(
                Stack().Children(
                    TextBlock("Sample panel").MediumPlus().SemiBold(),
                    ChoiceGroup("Side:").Choices(
                        Choice("Far").Selected().OnSelected((x, e) => panel.Side = Panel.PanelSide.Far),
                        Choice("Near").OnSelected((x, e) => panel.Side = Panel.PanelSide.Near)
                    ),
                    Toggle("Light Dismiss").OnChange((s, e) => panel.CanLightDismiss = s.IsChecked).Checked(panel.CanLightDismiss),
                    ChoiceGroup("Size:").Choices(
                        Choice("Small").Selected().OnSelected((x, e) => panel.Size = Panel.PanelSize.Small),
                        Choice("Medium").OnSelected((x, e) => panel.Size = Panel.PanelSize.Medium),
                        Choice("Large").OnSelected((x, e) => panel.Size = Panel.PanelSize.Large),
                        Choice("LargeFixed").OnSelected((x, e) => panel.Size = Panel.PanelSize.LargeFixed),
                        Choice("ExtraLarge").OnSelected((x, e) => panel.Size = Panel.PanelSize.ExtraLarge),
                        Choice("FullWidth").OnSelected((x, e) => panel.Size = Panel.PanelSize.FullWidth)
                    ),
                    Toggle("Is non-blocking").OnChange((s, e) => panel.IsNonBlocking = s.IsChecked).Checked(panel.IsNonBlocking),
                    Toggle("Is dark overlay").OnChange((s, e) => panel.IsDark = s.IsChecked).Checked(panel.IsDark),
                    Toggle("Hide close button").OnChange((s, e) => panel.ShowCloseButton = !s.IsChecked).Checked(!panel.ShowCloseButton)
                    )).SetFooter(Stack().Horizontal().Children(Button("Footer Button 1").Primary(), Button("Footer Button 2")));

            _content = SectionStack()
                        .Title(SampleHeader(nameof(PanelSample)))
                        .Section(Stack().Children(
                            SampleTitle("Overview"),
                            TextBlock("Panels are modal UI overlays that provide contextual app information. They often request some kind of creation or management action from the user. Panels are paired with the Overlay component, also known as a Light Dismiss. The Overlay blocks interactions with the app view until dismissed either through clicking or tapping on the Overlay or by selecting a close or completion action within the Panel."),
                            SampleSubTitle("Examples of experiences that use Panels"),
                            TextBlock("Member or group list creation or management"),
                            TextBlock("Document list creation or management"),
                            TextBlock("Permissions creation or management"),
                            TextBlock("Settings creation or management"),
                            TextBlock("Multi-field forms")))
                        .Section(Stack().Children(
                            SampleTitle("Best Practices"),
                            Stack().Horizontal().Children(
                            Stack().Width(40.percent()).Children(
                                SampleSubTitle("Do"),
                                SampleDo("Use for self-contained experiences where the user does not need to interact with the app view to complete the task."),
                                SampleDo("Use for complex creation, edit or management experiences."),
                                SampleDo("Consider how the panel and its contained contents will scale across Fabric’s responsive web breakpoints.")
                        ),
                        Stack().Width(40.percent()).Children(
                            SampleSubTitle("Don't"),
                            SampleDont("Don't use for experiences where the user needs to interact with the app view.")))))
                        .Section(Stack().Children(
                            SampleTitle("Usage"),
                            Button("Open panel").OnClick((s, e) => panel.Show())));
        }

        public HTMLElement Render()
        {
            return _content.Render();
        }
    }
}
