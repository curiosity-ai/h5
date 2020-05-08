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
    public class DropdownSample : IComponent
    {
        private IComponent _content;

        public DropdownSample()
        {
            var d = Dropdown();
            _content = SectionStack()
            .Title(SampleHeader(nameof(DropdownSample)))
            .Section(Stack().Children(
                SampleTitle("Overview"),
                TextBlock("A Dropdown is a list in which the selected item is always visible, and the others are visible on demand by clicking a drop-down button. They are used to simplify the design and make a choice within the UI. When closed, only the selected item is visible. When users click the drop-down button, all the options become visible. To change the value, users open the list and click another value or use the arrow keys (up and down) to select a new value.")))
            .Section(Stack().Children(SampleTitle("Best Practices"),
                Stack().Horizontal().Children(Stack().Width(40.percent()).Children(
                    SampleSubTitle("Do"),
                    SampleDo("Use a Dropdown when there are multiple choices that can be collapsed under one title. Or if the list of items is long or when space is constrained."),
                    SampleDo("Use shortened statements or single words as options."),
                    SampleDo("Use a Dropdown when the selected option is more important than the alternatives (in contrast to radio buttons where all the choices are visible putting more emphasis on the other options).")
                    ),
            Stack().Width(40.percent()).Children(
                SampleSubTitle("Don't"),
                SampleDo("Use if there are less than 7 options, use a ChoiceGroup instead.")))))
                .Section(Stack().Children(
                SampleTitle("Usage"),
                TextBlock("Basic Dropdowns").Medium(),
            Stack().Width(40.percent()).Children(
                Label("Standard").SetContent(Dropdown().Items(
                DropdownItem("1-1").Selected(),
                DropdownItem("1-2")
                ))),
            Stack().Width(40.percent()).Children(
                Label("Standard with Headers").SetContent(Dropdown().Items(
                DropdownItem("Header 1").Header(),
                DropdownItem("1-1"),
                DropdownItem("1-2"),
                DropdownItem("1-3"),
                DropdownItem("1-4").Disabled(),
                DropdownItem("1-5"),
                DropdownItem().Divider(),
                DropdownItem("Header 2").Header(),
                DropdownItem("2-1"),
                DropdownItem("2-2"),
                DropdownItem("2-3").Selected(),
                DropdownItem("2-4"),
                DropdownItem("2-5")
                )),
            Label("Multi-select with custom on-selected text").SetContent(Dropdown().Multi().Items(
                DropdownItem("Header 1").Header(),
                DropdownItem("1-1", "H1-1-1"),
                DropdownItem("1-2", "H1-1-2").Selected(),
                DropdownItem("1-3", "H1-1-3"),
                DropdownItem("1-4", "H1-1-4").Disabled(),
                DropdownItem("1-5", "H1-1-5"),
                DropdownItem().Divider(),
                DropdownItem("Header 2").Header(),
                DropdownItem("2-1", "H2-2-1"),
                DropdownItem("2-2", "H2-2-2"),
                DropdownItem("2-3", "H2-2-3"),
                DropdownItem("2-4", "H2-2-4").Selected(),
                DropdownItem("2-5", "H2-2-5")
                )),
            Label("Validation").SetContent(d.Items(
                DropdownItem("Header 1").Header(),
                DropdownItem("1-1").Selected(),
                DropdownItem("1-2"),
                DropdownItem("1-3"),
                DropdownItem("1-4").Disabled(),
                DropdownItem("1-5"),
                DropdownItem().Divider(),
                DropdownItem("Header 2").Header(),
                DropdownItem("2-1"),
                DropdownItem("2-2"),
                DropdownItem("2-3"),
                DropdownItem("2-4"),
                DropdownItem("2-5")
                )),
            Label("Disabled").SetContent(Dropdown().Disabled().Items(
                DropdownItem("Header 1").Header(),
                DropdownItem("1-1").Selected(),
                DropdownItem("1-2"),
                DropdownItem("1-3"),
                DropdownItem("1-4").Disabled(),
                DropdownItem("1-5"),
                DropdownItem().Divider(),
                DropdownItem("Header 2").Header(),
                DropdownItem("2-1"),
                DropdownItem("2-2"),
                DropdownItem("2-3"),
                DropdownItem("2-4"),
                DropdownItem("2-5")
            )),
            Label("Required").SetContent(Dropdown().Required().Items(
                DropdownItem("Header 1").Header(),
                DropdownItem("1-1").Selected(),
                DropdownItem("1-2"),
                DropdownItem("1-3"),
                DropdownItem("1-4").Disabled(),
                DropdownItem("1-5"),
                DropdownItem().Divider(),
                DropdownItem("Header 2").Header(),
                DropdownItem("2-1"),
                DropdownItem("2-2"),
                DropdownItem("2-3"),
                DropdownItem("2-4"),
                DropdownItem("2-5")
            )),
            Label("Async 5 seconds delay").SetContent(Dropdown().Items(GetItemsAsync)),
            Label("Async wait Google.com (need CORS)").SetContent(Dropdown().Items(GetGoogleItemsAsync)))));
            d.Attach((e, _) =>
            {
                var dd = (Dropdown) e;
                if (dd.SelectedItems.Length != 1 || dd.SelectedItems[0].Text != "1-1")
                {
                    dd.IsInvalid = true;
                    dd.Error = "Some error happens, need 1-1";
                }
                else dd.IsInvalid = false;
            }, Validation.Mode.OnInput);
        }

        private async Task<Dropdown.Item[]> GetItemsAsync()
        {
            await Task.Delay(5000);

            return new[]
            {
                DropdownItem("Header 1").Header(),
                DropdownItem("1-1"),
                DropdownItem("1-2"),
                DropdownItem("1-3"),
                DropdownItem("1-4").Disabled(),
                DropdownItem("1-5"),
                DropdownItem().Divider(),
                DropdownItem("Header 2").Header(),
                DropdownItem("2-1"),
                DropdownItem("2-2"),
                DropdownItem("2-3"),
                DropdownItem("2-4"),
                DropdownItem("2-5")
            };
        }

        private async Task<Dropdown.Item[]> GetGoogleItemsAsync()
        {
            try
            {
                await GetAsync("http://google.com");
            }
            catch
            {
                await Task.Delay(1000);
            }

            return new[]
            {
                DropdownItem("Header 1").Header(),
                DropdownItem("1-1"),
                DropdownItem("1-2"),
                DropdownItem("1-3"),
                DropdownItem("1-4").Disabled(),
                DropdownItem("1-5"),
                DropdownItem().Divider(),
                DropdownItem("Header 2").Header(),
                DropdownItem("2-1"),
                DropdownItem("2-2"),
                DropdownItem("2-3"),
                DropdownItem("2-4"),
                DropdownItem("2-5")
            };
        }

        public HTMLElement Render()
        {
            return _content.Render();
        }

        private async Task<string> GetAsync(string url)
        {
            var xmlHttp = new XMLHttpRequest();
            xmlHttp.open("GET", url, true);

            xmlHttp.setRequestHeader("Access-Control-Allow-Origin", "*");

            var tcs = new TaskCompletionSource<string>();

            xmlHttp.onreadystatechange = (e) =>
            {
                if (xmlHttp.readyState == 0)
                {
                    tcs.SetException(new Exception("Request aborted"));
                }
                else if (xmlHttp.readyState == 4)
                {
                    if (xmlHttp.status == 200 || xmlHttp.status == 201 || xmlHttp.status == 304)
                    {
                        tcs.SetResult(xmlHttp.responseText);
                    }
                    else tcs.SetException(new Exception("Request failed"));
                }
            };

            xmlHttp.send();

            var tcsTask = tcs.Task;

            while (true)
            {
                await Task.WhenAny(tcsTask, Task.Delay(150));

                if (tcsTask.IsCompleted)
                {
                    if (tcsTask.IsFaulted)
                    {
                        throw tcsTask.Exception;
                    }
                    else
                    {
                        return tcsTask.Result;
                    }
                }
            }
        }
    }
}
