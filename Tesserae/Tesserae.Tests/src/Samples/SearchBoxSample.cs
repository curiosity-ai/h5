using System;
using Tesserae.Components;
using static H5.Core.dom;
using static Tesserae.UI;
using static Tesserae.Tests.Samples.SamplesHelper;

namespace Tesserae.Tests.Samples
{
    public class SearchBoxSample : IComponent
    {
        private IComponent _content;

        public SearchBoxSample()
        {
            var searchAsYouType = TextBlock("start typing");
            _content = SectionStack()
            .Title(SampleHeader(nameof(SearchBoxSample)))
            .Section(Stack().Children(
                SampleTitle("Overview"),
                TextBlock("SearchBoxes provide an input field for searching through content, allowing users to locate specific items within the website or app.")))
            .Section(Stack().Children(
                SampleTitle("Best Practices"),
                Stack().Horizontal().Children(
                Stack().Width(40.percent()).Children(
                    SampleSubTitle("Do"),
                    SampleDo("Use placeholder text in the SearchBox to describe what users can search for."),
                    SampleDo("Example: 'Search'; 'Search files'; 'Search site'"),
                    SampleDo("Once the user has clicked into the SearchBox but hasn’t entered input yet, use 'hint text' to communicate search scope."),
                    SampleDo("Examples: 'Try searching for a PDFs'; 'Search contacts list'; 'Type to find <content type> '"),
                    SampleDo("Use the Underlined SearchBox for CommandBars.")
                ),
                Stack().Width(40.percent()).Children(
                    SampleSubTitle("Don't"),
                    SampleDont("Don't leave the SearchBox blank because it's too ambiguous."),
                    SampleDont("Don't have lengthy and unclear hint text. It should be used to clasify and set expectations."),
                    SampleDont("Don't provide inaccurate matches or bad predictions, as it will make search seem unreliable and will result in user frustration."),
                    SampleDont("Don’t provide too much information or metadata in the suggestions list; it’s intended to be lightweight."),
                    SampleDont("Don't build a custom search control based on the default text box or any other control."),
                    SampleDont("Don't use SearchBox if you cannot reliably provide accurate results.")
                )
            )))
            .Section(Stack().Children(
                SampleTitle("Usage"),
                TextBlock("Basic TextBox").Medium(),
                Stack().Width(40.percent()).Children(
                    Label("Default").SetContent(SearchBox("Search").OnSearch((s,e) => alert($"Searched for {e}"))),
                    Label("Disabled").Disabled().SetContent(SearchBox("Search").Disabled()),
                    Label("Underline").SetContent(SearchBox("Search").Underlined().OnSearch((s, e) => alert($"Searched for {e}"))),
                    Label("Search as you type").SetContent(SearchBox("Search").Underlined().SearchAsYouType().OnSearch((s, e) => searchAsYouType.Text = $"Searched for {e}")),
                    searchAsYouType,
                    Label("Custom Icon").Required().SetContent(SearchBox("Filter").SetIcon("las la-filter").OnSearch((s, e) => alert($"Filter for {e}"))),
                    Label("No Icon").SetContent(SearchBox("Search").NoIcon().OnSearch((s, e) => alert($"Searched for {e}"))),
                    Label("Fixed Width").Required().SetContent(SearchBox("Small Search").Width(200.px()).OnSearch((s, e) => alert($"Searched for {e}"))))));
        }

        public HTMLElement Render()
        {
            return _content.Render();
        }
    }
}
