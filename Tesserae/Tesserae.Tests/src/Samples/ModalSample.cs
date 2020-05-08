using Tesserae.Components;
using static H5.Core.dom;
using static Tesserae.Tests.Samples.SamplesHelper;
using static Tesserae.UI;

namespace Tesserae.Tests.Samples
{
    public class ModalSample : IComponent
    {
        private IComponent _content;

        public ModalSample()
        {
            var container = Raw();

            Modal("Lorem Ipsum")
                .Var(out var modal)
                .LightDismiss()
                .Width(60.vw())
                .Height(60.vh())
                .SetFooter(TextBlock("This is a footer note").SemiBold().MediumPlus())
                .Content(Stack().Children(
                            TextBlock("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas lorem nulla, malesuada ut sagittis sit amet, vulputate in leo. Maecenas vulputate congue sapien eu tincidunt. Etiam eu sem turpis. Fusce tempor sagittis nunc, ut interdum ipsum vestibulum non. Proin dolor elit, aliquam eget tincidunt non, vestibulum ut turpis. In hac habitasse platea dictumst. In a odio eget enim porttitor maximus. Aliquam nulla nibh, ullamcorper aliquam placerat eu, viverra et dui. Phasellus ex lectus, maximus in mollis ac, luctus vel eros. Vivamus ultrices, turpis sed malesuada gravida, eros ipsum venenatis elit, et volutpat eros dui et ante. Quisque ultricies mi nec leo ultricies mollis. Vivamus egestas volutpat lacinia. Quisque pharetra eleifend efficitur. "),
                            TextBlock("Mauris at nunc eget lectus lobortis facilisis et eget magna. Vestibulum venenatis augue sapien, rhoncus faucibus magna semper eget. Proin rutrum libero sagittis sapien aliquet auctor. Suspendisse tristique a magna at facilisis. Duis rhoncus feugiat magna in rutrum. Suspendisse semper, dolor et vestibulum lacinia, nunc felis malesuada ex, nec hendrerit justo ex et massa. Quisque quis mollis nulla. Nam commodo est ornare, rhoncus odio eu, pharetra tellus. Nunc sed velit mi."),
                            TextBlock("Sed condimentum ultricies turpis convallis pharetra. Sed sagittis quam pharetra luctus porttitor. Cras vel consequat lectus. Sed nec fringilla urna, a aliquet libero. Aenean sed nisl purus. Vivamus vulputate felis et odio efficitur suscipit. Ut volutpat dictum lectus, ac rutrum massa accumsan at. Sed pharetra auctor finibus. In augue libero, commodo vitae nisi non, sagittis convallis ante. Phasellus malesuada eleifend mollis. Curabitur ultricies leo ac metus venenatis elementum."),
                            TextBlock("Aenean egestas quam ut erat commodo blandit. Mauris ante nisl, pellentesque sed venenatis nec, aliquet sit amet enim. Praesent vitae diam non diam aliquet tristique non ut arcu. Pellentesque et ultrices eros. Fusce diam metus, mattis eu luctus nec, facilisis vel erat. Nam a lacus quis tellus gravida euismod. Nulla sed sem eget tortor cursus interdum. Sed vehicula tristique ultricies. Aenean libero purus, mollis quis massa quis, eleifend dictum massa. Fusce eu sapien sit amet odio lacinia placerat. Mauris varius risus sed aliquet cursus. Aenean lectus magna, tincidunt sit amet sodales a, volutpat ac leo. Morbi nisl sapien, tincidunt sit amet mauris quis, sollicitudin auctor est."),
                            TextBlock("Nam id mi justo. Nam vehicula vulputate augue, ac pretium enim rutrum ultricies. Sed aliquet accumsan varius. Quisque ac auctor ligula. Fusce fringilla, odio et dignissim iaculis, est lacus ultrices risus, vitae condimentum enim urna eu nunc. In risus est, mattis non suscipit at, mattis ut ante. Maecenas consectetur urna vel erat maximus, non molestie massa consequat. Duis a feugiat nibh. Sed a hendrerit diam, a mattis est. In augue dolor, faucibus vel metus at, convallis rhoncus dui."),
                            Label("Light Dismiss").Inline().AutoWidth().SetContent(Toggle().OnChange((s, e) => modal.CanLightDismiss = s.IsChecked).Checked(modal.CanLightDismiss)),
                            Label("Is draggable").Inline().AutoWidth().SetContent(Toggle().OnChange((s, e) => modal.IsDraggable = s.IsChecked).Checked(modal.IsDraggable)),
                            Label("Is dark overlay").Inline().AutoWidth().SetContent(Toggle().OnChange((s, e) => modal.IsDark = s.IsChecked).Checked(modal.IsDark)),
                            Label("Is non-blocking").Inline().AutoWidth().SetContent(Toggle().OnChange((s, e) => modal.IsNonBlocking = s.IsChecked).Checked(modal.IsNonBlocking)),
                            Label("Hide close button").Inline().AutoWidth().SetContent(Toggle().OnChange((s, e) => modal.ShowCloseButton = !s.IsChecked).Checked(!modal.ShowCloseButton)),
                            Label("Open a dialog from here").Var(out var lbl).SetContent(Button("Open").OnClick((s, e) => Dialog("Dialog over Modal").Content(TextBlock("Hello World!")).YesNo(() => lbl.Text = "Yes", () => lbl.Text = "No")))));

            _content = SectionStack()
                        .Title(SampleHeader(nameof(ModalSample)))
                        .Section(Stack().Children(
                            SampleTitle("Overview"),
                            TextBlock("Modals are temporary, modal UI overlay that generally provide contextual app information or require user confirmation/input, or can be used to advertise new app features. In some cases, Modals block interactions with the web page or application until being explicitly dismissed. They can be used for lightweight creation or edit tasks and simple management tasks, or for hosting heavier temporary content."),
                            TextBlock("For usage requiring a quick choice from the user, Dialog may be a more appropriate control.")))
                        .Section(Stack().Children(
                            SampleTitle("Best Practices"),
                            Stack().Horizontal().Children(
                                Stack().Width(40.percent()).Children(
                                    SampleSubTitle("Do"),
                                    SampleDo("Use Modals for actionable interactions, such as needing the user to provide information or change settings."),
                                    SampleDo("When possible, try a non-blocking Modal before resorting to a blocking Modal."),
                                    SampleDo("Always have at least one focusable element inside a Modal.")),
                            Stack().Width(40.percent()).Children(
                                SampleSubTitle("Don't"),
                                SampleDont("Don’t overuse Modals. In some cases they can be perceived as interrupting workflow, and too many can be a bad user experience.")))))
                        .Section(Stack().Children(
                            SampleTitle("Usage"),
                            Button("Open Modal").OnClick((s, e) => modal.Show()),
                            Button("Open Modal from top right").OnClick((s, e) => modal.ShowAt(fromRight: 16.px(), fromTop: 16.px())),
                            Button("Open Modal with minimum size").OnClick((s, e) => Modal().CenterContent().LightDismiss().Dark().Content(TextBlock("small content").Tiny()).MinHeight(50.vh()).MinWidth(50.vw()).Show()),
                            
                            SampleTitle("Embedded Modal"),
                            Button("Open Modal Below").OnClick((s, e) => container.Content(Modal("Embedded Modal").CenterContent().LightDismiss().Dark().Content(TextBlock("hosted small content").Tiny()).MinHeight(30.vh()).MinWidth(50.vw()).ShowEmbedded())),
                            container
                            ));
        }

        public HTMLElement Render()
        {
            return _content.Render();
        }
    }
}
