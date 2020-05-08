using System;
using System.Collections.Generic;
using System.Linq;
using Tesserae.Components;
using static Tesserae.Tests.Samples.SamplesHelper;
using static Tesserae.UI;
using static H5.dom;

namespace Tesserae.Tests.Samples
{
    public class DetailsListSample : IComponent
    {
        private IComponent _content;

        public DetailsListSample()
        {
            _content =
                SectionStack()
                    .Title(SampleHeader(nameof(DetailsListSample)))
                    .Section(
                        Stack()
                            .Children(
                                SampleTitle("Overview"),
                                TextBlock("DetailsList is a derivative of the List component. It is a robust way to " +
                                          "display an information rich collection of items. It can support powerful " +
                                          "ways to aid a user in finding content with sorting, grouping and " +
                                          "filtering.  Lists are a great way to handle large amounts of content, " +
                                          "but poorly designed Lists can be difficult to parse.")
                                    .PaddingBottom(16.px()),
                                TextBlock("Use a DetailsList when density of information is critical. Lists can " +
                                          "support single and multiple selection, as well as drag and drop and " +
                                          "marquee selection. They are composed of a column header, which " +
                                          "contains the metadata fields which are attached to the list items, " +
                                          "and provide the ability to sort, filter and even group the list. " +
                                          "List items are composed of selection, icon, and name columns at " +
                                          "minimum. One can also include other columns such as Date Modified, or " +
                                          "any other metadata field associated with the collection. Place the most " +
                                          "important columns from left to right for ease of recall and comparison.")
                                    .PaddingBottom(16.px()),
                                TextBlock("DetailsList is classically used to display files, but is also used to " +
                                          "render custom lists that can be purely metadata. Avoid using file type " +
                                          "icon overlays to denote status of a file as it can make the entire icon " +
                                          "unclear. Be sure to leave ample width for each column’s data. " +
                                          "If there are multiple lines of text in a column, " +
                                          "consider the variable row height variant.")))
                    .Section(
                        Stack()
                            .Children(
                                SampleTitle("Best Practices"),
                                Stack()
                                    .Horizontal()
                                    .Children(
                                        Stack()
                                            .Width(40.percent())
                                            .Children(
                                                SampleSubTitle("Do"),
                                                SampleDo("Use them to display content."),
                                                SampleDo("Provide useful columns of metadata."),
                                                SampleDo("Display columns in order of importance left to right or " +
                                                         "right to left depending on the standards of the culture."),
                                                SampleDo("Give columns ample default width to display information.")),
                                        Stack()
                                            .Width(40.percent())
                                            .Children(
                                                SampleSubTitle("Don't"),
                                                SampleDont("Use them to display commands or settings."),
                                                SampleDont("Overload the view with too many columns that require " +
                                                         "excessive horizontal scrolling."),
                                                SampleDont("Make columns so narrow that it truncates the information " +
                                                         "in typical cases.")))))
                    .Section(
                        Stack()
                            .Children(
                                SampleTitle("Usage"),
                                TextBlock("Details List With Textual Rows")
                                    .Medium()
                                    .PaddingBottom(16.px()),
                                DetailsList<DetailsListSampleFileItem>(
                                        IconColumn(Icon(LineAwesome.File), width: 32.px(),  enableColumnSorting: true, sortingKey: "FileIcon"),
                                        DetailsListColumn(title: "File Name",         width: 350.px(), enableColumnSorting: true, sortingKey: "FileName", isRowHeader: true),
                                        DetailsListColumn(title: "Date Modified",     width: 170.px(), enableColumnSorting: true, sortingKey: "DateModified"),
                                        DetailsListColumn(title: "Modified By",       width: 150.px(), enableColumnSorting: true, sortingKey: "ModifiedBy"),
                                        DetailsListColumn(title: "File Size",         width: 120.px(), enableColumnSorting: true, sortingKey: "FileSize"))
                                    .Height(500.px())
                                    .WithListItems(GetDetailsListItems())
                                    .SortedBy("FileName")
                            .PaddingBottom(32.px()),
                                TextBlock("Details List With Component Rows")
                                    .Medium()
                                    .PaddingBottom(16.px()),
                                DetailsList<DetailsListSampleItemWithComponents>(
                                    IconColumn(Icon(LineAwesome.Apple), width: 32.px(), enableColumnSorting: true, sortingKey: "Icon"),
                                    DetailsListColumn(title: "CheckBox", width: 120.px()),
                                    DetailsListColumn(title: "Name", width: 250.px(), isRowHeader: true),
                                    DetailsListColumn(title: "Button", width: 150.px()),
                                    DetailsListColumn(title: "ChoiceGroup", width: 400.px()),
                                    DetailsListColumn(title: "Dropdown", width: 250.px()),
                                    DetailsListColumn(title: "Toggle", width: 100.px()))
                                    .Compact()
                                    .Height(500.px())
                                    .WithListItems(GetComponentDetailsListItems())
                                    .SortedBy("Name")
                            .PaddingBottom(32.px()),
                                TextBlock("Details List With Empty List Message")
                                    .Medium()
                                    .PaddingBottom(16.px()),
                                DetailsList<DetailsListSampleFileItem>(
                                        IconColumn(Icon(LineAwesome.File), width: 32.px(),  enableColumnSorting: true, sortingKey: "FileIcon"),
                                        DetailsListColumn(title: "File Name",         width: 350.px(), enableColumnSorting: true, sortingKey: "FileName", isRowHeader: true),
                                        DetailsListColumn(title: "Date Modified",     width: 170.px(), enableColumnSorting: true, sortingKey: "DateModified"),
                                        DetailsListColumn(title: "Modified By",       width: 150.px(), enableColumnSorting: true, sortingKey: "ModifiedBy"),
                                        DetailsListColumn(title: "File Size",         width: 120.px(), enableColumnSorting: true, sortingKey: "FileSize"))
                                    .Compact()
                                    .WithEmptyMessage(() => BackgroundArea(Card(TextBlock("Empty list").Padding(16.px()))).WidthStretch().HeightStretch() )
                                    .Height(500.px())
                                    .WithListItems(new DetailsListSampleFileItem[0])
                                    .SortedBy("Name")));
        }

        public HTMLElement Render()
        {
            return _content.Render();
        }

        private DetailsListSampleFileItem[] GetDetailsListItems()
        {
            return Enumerable
                .Range(1, 100)
                .SelectMany(number => new List<DetailsListSampleFileItem>
                {
                    new DetailsListSampleFileItem(
                        fileIcon: LineAwesome.FileWord,
                        fileName: "Interesting File Name, quite long as you can see. In fact, let's make it " +
                                  "longer to see how the padding looks.",
                        dateModified: DateTime.Today.AddDays(-10),
                        modifiedBy: "Dale Cooper",
                        fileSize: 10),
                    new DetailsListSampleFileItem(
                        fileIcon: LineAwesome.FileExcel,
                        fileName: "File Name 2",
                        dateModified: DateTime.Today.AddDays(-20),
                        modifiedBy: "Rusty",
                        fileSize: 12),
                    new DetailsListSampleFileItem(
                        fileIcon: LineAwesome.FilePowerpoint,
                        fileName: "File Name 3",
                        dateModified: DateTime.Today.AddDays(-30),
                        modifiedBy: "Cole",
                        fileSize: 15)
            }).ToArray();
        }

        private DetailsListSampleItemWithComponents[] GetComponentDetailsListItems()
        {
            return Enumerable
                .Range(1, 100)
                .SelectMany(number => new List<DetailsListSampleItemWithComponents>
                {
                    new DetailsListSampleItemWithComponents()
                        .WithIcon(LineAwesome.Code)
                        .WithCheckBox(
                            CheckBox("CheckBox"))
                        .WithName("Component Details List Item")
                        .WithButton(
                            Button()
                                .SetText("Primary")
                                .Primary()
                                .OnClick(
                                    (s, e) => alert("Clicked!")))
                        .WithChoiceGroup(
                            ChoiceGroup()
                                .Horizontal()
                                .Choices(
                                     Choice("Option A"),
                                     Choice("Option B").Disabled(),
                                     Choice("Option C")))
                        .WithDropdown(
                            Dropdown()
                                .Multi()
                                .Items(
                                    DropdownItem("Header 1").Header(),
                                    DropdownItem("1-1"),
                                    DropdownItem("1-2").Selected(),
                                    DropdownItem("1-3"),
                                    DropdownItem("1-4").Disabled(),
                                    DropdownItem("1-5"),
                                    DropdownItem("2-1"),
                                    DropdownItem("2-2"),
                                    DropdownItem("2-3"),
                                    DropdownItem("2-4").Selected(),
                                    DropdownItem("2-5")))
                        .WithToggle(Toggle())
            }).ToArray();
        }

    }

    public class DetailsListSampleFileItem : IDetailsListItem<DetailsListSampleFileItem>
    {
        public DetailsListSampleFileItem(LineAwesome fileIcon, string fileName, DateTime dateModified, string modifiedBy, int fileSize)
        {
            FileIcon = fileIcon;
            FileName = fileName;
            DateModified = dateModified;
            ModifiedBy = modifiedBy;
            FileSize = fileSize;
        }

        public LineAwesome FileIcon { get; }

        public string FileName { get; }

        public DateTime DateModified { get; }

        public string ModifiedBy { get; }

        public int FileSize { get; }

        public bool EnableOnListItemClickEvent => true;

        public void OnListItemClick(int listItemIndex)
        {
            alert($"You clicked me! List item index: {listItemIndex}, my name is {FileName}");
        }

        public int CompareTo(DetailsListSampleFileItem other, string columnSortingKey)
        {
            if (other == null)
            {
                throw new ArgumentException(nameof(other));
            }

            if (columnSortingKey.Equals(nameof(FileIcon)))
            {
                return FileIcon.CompareTo(other.FileIcon);
            }

            if (columnSortingKey.Equals(nameof(FileName)))
            {
                return string.Compare(FileName, other.FileName, StringComparison.Ordinal);
            }

            if (columnSortingKey.Equals(nameof(DateModified)))
            {
                return DateModified.CompareTo(other.DateModified);
            }

            if (columnSortingKey.Equals(nameof(ModifiedBy)))
            {
                return string.Compare(FileName, other.FileName, StringComparison.Ordinal);
            }

            if (columnSortingKey.Equals(nameof(FileSize)))
            {
                return FileSize.CompareTo(other.FileSize);
            }

            throw new InvalidOperationException($"Can not match {columnSortingKey} to current list item");
        }

        public IEnumerable<IComponent> Render(IList<IDetailsListColumn> columns, Func<IDetailsListColumn, Func<IComponent>, IComponent> createGridCellExpression)
        {
            yield return createGridCellExpression(columns[0], () => Icon(FileIcon));
            yield return createGridCellExpression(columns[1], () => TextBlock(FileName));
            yield return createGridCellExpression(columns[2], () => TextBlock(DateModified.ToShortDateString()));
            yield return createGridCellExpression(columns[3], () => TextBlock(ModifiedBy));
            yield return createGridCellExpression(columns[4], () => TextBlock(FileSize.ToString()));
        }
    }

    public class DetailsListSampleItemWithComponents : IDetailsListItem<DetailsListSampleItemWithComponents>
    {
        public LineAwesome Icon { get; private set; }

        public CheckBox CheckBox { get; private set; }

        public string Name { get; private set; }

        public Button Button { get; private set; }

        public ChoiceGroup ChoiceGroup { get; private set; }

        public Dropdown Dropdown { get; private set; }

        public Toggle Toggle { get; private set; }

        public bool EnableOnListItemClickEvent => false;

        public void OnListItemClick(int listItemIndex)
        {
        }

        public int CompareTo(DetailsListSampleItemWithComponents other, string columnSortingKey)
        {
            return 0;
        }

        public DetailsListSampleItemWithComponents WithIcon(LineAwesome icon)
        {
            Icon = icon;
            return this;
        }

        public DetailsListSampleItemWithComponents WithCheckBox(CheckBox checkBox)
        {
            CheckBox = checkBox;
            return this;
        }

        public DetailsListSampleItemWithComponents WithName(string name)
        {
            Name = name;
            return this;
        }

        public DetailsListSampleItemWithComponents WithButton(Button button)
        {
            Button = button;
            return this;
        }

        public DetailsListSampleItemWithComponents WithChoiceGroup(ChoiceGroup choiceGroup)
        {
            ChoiceGroup = choiceGroup;

            return this;
        }

        public DetailsListSampleItemWithComponents WithDropdown(Dropdown dropdown)
        {
            Dropdown = dropdown;
            return this;
        }

        public DetailsListSampleItemWithComponents WithToggle(Toggle toggle)
        {
            Toggle = toggle;
            return this;
        }

        public IEnumerable<IComponent> Render(IList<IDetailsListColumn> columns, Func<IDetailsListColumn, Func<IComponent>, IComponent> createGridCellExpression)
        {
            yield return createGridCellExpression(columns[0], () => Icon(Icon));
            yield return createGridCellExpression(columns[1], () => CheckBox);
            yield return createGridCellExpression(columns[2], () => TextBlock(Name));
            yield return createGridCellExpression(columns[3], () => Button);
            yield return createGridCellExpression(columns[4], () => ChoiceGroup);
            yield return createGridCellExpression(columns[5], () => Dropdown);
            yield return createGridCellExpression(columns[6], () => Toggle);
        }
    }
}
