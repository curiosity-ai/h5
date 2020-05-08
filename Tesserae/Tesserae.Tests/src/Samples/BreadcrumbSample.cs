using System;
using Tesserae.Components;
using static H5.dom;
using static Tesserae.UI;
using static Tesserae.Tests.Samples.SamplesHelper;

namespace Tesserae.Tests.Samples
{
    public class BreadcrumbSample : IComponent
    {
        private IComponent _content;

        public BreadcrumbSample()
        {
            _content = SectionStack()
            .Title(SampleHeader(nameof(BreadcrumbSample)))
            .Section(Stack().Children(
                SampleTitle("Overview"),
                TextBlock("Breadcrumbs should be used as a navigational aid in your app or site. They indicate the current page’s location within a hierarchy and help the user understand where they are in relation to the rest of that hierarchy. They also afford one-click access to higher levels of that hierarchy."),
                TextBlock("Breadcrumbs are typically placed, in horizontal form, under the masthead or navigation of an experience, above the primary content area.")))
            .Section(Stack().Children(
                SampleTitle("Best Practices"),
                Stack().Horizontal().Children(
                Stack().Width(40.percent()).Children(
                    SampleSubTitle("Do"),
                    SampleDo("Place Breadcrumbs at the top of a page, above a list of items, or above the main content of a page.")
                    ),
            Stack().Width(40.percent()).Children(
                SampleSubTitle("Don't"),
                SampleDont("Don't use Breadcrumbs as a primary way to navigate an app or site.")))))
                .Section(Stack().Children(
                    SampleTitle("Usage"),
                    Label("Selected: ").SetContent(TextBlock().Var(out var msg)),
                    TextBlock("All Visible").Medium(),
                    Breadcrumb().PaddingTop(16.px()).PaddingBottom(16.px()).Items(
                        Crumb("Folder 1").OnClick((s,e) => msg.Text("Folder 1")),
                        Crumb("Folder 2").OnClick((s, e) => msg.Text("Folder 2")).Disabled(),
                        Crumb("Folder 3").OnClick((s, e) => msg.Text("Folder 3")),
                        Crumb("Folder 4").OnClick((s, e) => msg.Text("Folder 4")),
                        Crumb("Folder 5").OnClick((s, e) => msg.Text("Folder 5")),
                        Crumb("Folder 6").OnClick((s, e) => msg.Text("Folder 6"))),

                    TextBlock("All Visible, Small").Medium(),
                    Breadcrumb().Small().PaddingTop(16.px()).PaddingBottom(16.px()).Items(
                        Crumb("Folder 1").OnClick((s, e) => msg.Text("Folder 1")),
                        Crumb("Folder 2").OnClick((s, e) => msg.Text("Folder 2")).Disabled(),
                        Crumb("Folder 3").OnClick((s, e) => msg.Text("Folder 3")),
                        Crumb("Folder 4").OnClick((s, e) => msg.Text("Folder 4")),
                        Crumb("Folder 5").OnClick((s, e) => msg.Text("Folder 5")),
                        Crumb("Folder 6").OnClick((s, e) => msg.Text("Folder 6"))),

                    TextBlock("Collapse 200px").Medium(),
                    Breadcrumb().PaddingTop(16.px()).PaddingBottom(16.px()).MaxWidth(200.px()).Items(
                        Crumb("Folder 1").OnClick((s, e) => msg.Text("Folder 1")),
                        Crumb("Folder 2").OnClick((s, e) => msg.Text("Folder 2")),
                        Crumb("Folder 3").OnClick((s, e) => msg.Text("Folder 3")),
                        Crumb("Folder 4").OnClick((s, e) => msg.Text("Folder 4")),
                        Crumb("Folder 5").OnClick((s, e) => msg.Text("Folder 5")),
                        Crumb("Folder 6").OnClick((s, e) => msg.Text("Folder 6")))
                    ,

                    TextBlock("Collapse 200px, Small").Medium(),
                    Breadcrumb().PaddingTop(16.px()).PaddingBottom(16.px()).Small().MaxWidth(200.px()).Items(
                        Crumb("Folder 1").OnClick((s, e) => msg.Text("Folder 1")),
                        Crumb("Folder 2").OnClick((s, e) => msg.Text("Folder 2")),
                        Crumb("Folder 3").OnClick((s, e) => msg.Text("Folder 3")),
                        Crumb("Folder 4").OnClick((s, e) => msg.Text("Folder 4")),
                        Crumb("Folder 5").OnClick((s, e) => msg.Text("Folder 5")),
                        Crumb("Folder 6").OnClick((s, e) => msg.Text("Folder 6")))
                    ,
                    TextBlock("Collapse 300px").Medium(),
                    Breadcrumb().PaddingTop(16.px()).PaddingBottom(16.px()).MaxWidth(300.px()).Items(
                        Crumb("Folder 1").OnClick((s, e) => msg.Text("Folder 1")),
                        Crumb("Folder 2").OnClick((s, e) => msg.Text("Folder 2")),
                        Crumb("Folder 3").OnClick((s, e) => msg.Text("Folder 3")),
                        Crumb("Folder 4").OnClick((s, e) => msg.Text("Folder 4")),
                        Crumb("Folder 5").OnClick((s, e) => msg.Text("Folder 5")),
                        Crumb("Folder 6").OnClick((s, e) => msg.Text("Folder 6"))),

                    TextBlock("Collapse 300px, from second, custom chevron").Medium(),
                    Breadcrumb().PaddingTop(16.px()).PaddingBottom(16.px()).MaxWidth(300.px()).SetChevron("fa-plane").SetOverflowIndex(1).Items(
                        Crumb("Folder 1").OnClick((s, e) => msg.Text("Folder 1")),
                        Crumb("Folder 2").OnClick((s, e) => msg.Text("Folder 2")),
                        Crumb("Folder 3").OnClick((s, e) => msg.Text("Folder 3")),
                        Crumb("Folder 4").OnClick((s, e) => msg.Text("Folder 4")),
                        Crumb("Folder 5").OnClick((s, e) => msg.Text("Folder 5")),
                        Crumb("Folder 6").OnClick((s, e) => msg.Text("Folder 6")))

                    ));
        }

        public HTMLElement Render()
        {
            return _content.Render();
        }
    }
}
