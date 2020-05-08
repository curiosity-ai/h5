using System;
using System.Threading;
using System.Threading.Tasks;
using H5;
using Tesserae.Components;
using static H5.dom;
using static Tesserae.UI;
using static Tesserae.Tests.Samples.SamplesHelper;

namespace Tesserae.Tests.Samples
{
    public class ContextMenuSample : IComponent
    {
        private IComponent _content;

        public ContextMenuSample()
        {
            var d = ContextMenu();
            var msg = TextBlock();
            _content = SectionStack()
                        .Title(SampleHeader(nameof(ContextMenuSample)))
                        .Section(Stack().Children(SampleTitle("Overview"),
                                                 TextBlock("ContextualMenus are lists of commands that are based on the context of selection, mouse hover or keyboard focus. They are one of the most effective and highly used command surfaces, and can be used in a variety of places.")))
                        .Section(Stack().Children(SampleTitle("Best Practices"),
                                                  Stack().Horizontal().Children(Stack().Width(40.percent()).Children(
                                                    SampleSubTitle("Do"),
                                                    SampleDo("Use to display commands."),
                                                    SampleDo("Divide groups of commands with rules."),
                                                    SampleDo("Use selection checks without icons."),
                                                    SampleDo("Provide submenus for sets of related commands that aren’t as critical as others.")),
                                                  Stack().Width(40.percent()).Children(
                                                    SampleSubTitle("Don't"),
                                                    SampleDont("Use them to display content."),
                                                    SampleDont("Show commands as one large group."),
                                                    SampleDont("Mix checks and icons."),
                                                    SampleDont("Create submenus of submenus.")))))
                        .Section(Stack().Children(SampleTitle("Usage"),
                                                  TextBlock("Basic ContextMenus").Medium(),
                                                  Stack().Width(40.percent()).Children(
                                                    Label("Standard with Headers").SetContent(
                                                        Button("Open").Var(out var btn2).OnClick((s, e) =>
                                                            ContextMenu().Items(
                                                            ContextMenuItem("New").OnClick((s2,e2) => msg.Text("Clicked: New")),
                                                            ContextMenuItem().Divider(),
                                                            ContextMenuItem("Edit").OnClick((s2, e2) => msg.Text("Clicked: Edit")),
                                                            ContextMenuItem("Properties").OnClick((s2, e2) => msg.Text("Clicked: Properties")),
                                                            ContextMenuItem("Header").Header(),
                                                            ContextMenuItem("Disabled").Disabled(),
                                                            ContextMenuItem("Link").OnClick((s2, e2) => msg.Text("Clicked: Link"))
                                                            ).ShowFor(btn2)
                                            )), msg)));
        }

        public HTMLElement Render()
        {
            return _content.Render();
        }
    }
}
