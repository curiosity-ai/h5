using System;
using Tesserae.Components;
using static H5.Core.dom;
using static Tesserae.UI;
using static Tesserae.Tests.Samples.SamplesHelper;

namespace Tesserae.Tests.Samples
{
    public class FileSelectorAndDropAreaSample : IComponent
    {
        private IComponent _content;

        public FileSelectorAndDropAreaSample()
        {
            _content = SectionStack()
            .Title(SampleHeader(nameof(FileSelectorAndDropAreaSample)))
            .Section(Stack().Children(
                SampleTitle("Overview"),
                TextBlock("Use the file selector to allow users to select a single file. Use the file dropdown area to allow drag-and-drop for one or multiple files.")))
            .Section(Stack().Children(
                SampleTitle("Best Practices"),
                Stack().Horizontal().Children(
                Stack().Width(40.percent()).Children(
                    SampleSubTitle("Do"),
                    SampleDo("Filter files by supported types"),
                    SampleDo("Provide a message for the file drop area"),
                    SampleDo("Attach the OnUpload event handler")
                    ),
            Stack().Width(40.percent()).Children(
                SampleSubTitle("Don't"),
                SampleDont("TODO")))))
                .Section(Stack().Children(
                    SampleTitle("Usage"),
                    SampleSubTitle("File Selector"),
                    Label("Selected file size: ").Inline().SetContent(TextBlock("").Var(out var size)),
                    FileSelector().OnFileSelected((fs, e) => size.Text = fs.SelectedFile.size.ToString() + " bytes"),
                    FileSelector().SetPlaceholder("You must select a zip file").Required().SetAccepts(".zip").OnFileSelected((fs,e) => size.Text = fs.SelectedFile.size.ToString() + " bytes"),
                    FileSelector().SetPlaceholder("Please select any image").SetAccepts("image/*").OnFileSelected((fs, e) => size.Text = fs.SelectedFile.size.ToString() + " bytes"),
                    SampleSubTitle("File Drop Area"),
                    Label("Dropped Files: ").SetContent(Stack().Var(out var droppedFiles)),
                    FileDropArea().OnFileDropped((s, e) => droppedFiles.Add(TextBlock(e.name).Small())).Multiple()
                    ));
        }

        public HTMLElement Render()
        {
            return _content.Render();
        }
    }
}
