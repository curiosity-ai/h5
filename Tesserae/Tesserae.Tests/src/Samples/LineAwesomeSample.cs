using System;
using Tesserae.Components;
using static H5.Core.dom;
using static Tesserae.UI;
using static Tesserae.Tests.Samples.SamplesHelper;
using System.Collections.Generic;
using System.Linq;

namespace Tesserae.Tests.Samples
{
    public class LineAwesomeSample : IComponent
    {
        private IComponent _content;

        public LineAwesomeSample()
        {
            //TODO: Add dropwdown to select icon weight

            var searchAsYouType = TextBlock("search for icons");
            _content = SectionStack()
            .Title(SampleHeader(nameof(LineAwesomeSample)))
            .Section(Stack().Children(
                SampleTitle("Overview"),
                TextBlock("Tesserae integrates the LineAwesome icons as part of the package, with an auto-generated strongly typed enum for them.")))
            .Section(Stack().Children(
                SampleTitle("Best Practices"),
                Stack().Horizontal().Children(
                Stack().Width(40.percent()).Children(
                    SampleSubTitle("Do"),
                    SampleDo("TODO")
                ),
                Stack().Width(40.percent()).Children(
                    SampleSubTitle("Don't"),
                    SampleDont("TODO")
                )
            )))
            .Section(Stack().Children(
                SampleTitle("Usage:"),
                TextBlock($"enum {nameof(LineAwesome)}:").Medium(),
                SearchableList(GetAllIcons().ToArray(), 25.percent(), 25.percent(), 25.percent(), 25.percent()))).PaddingBottom(32.px()).MaxHeight(100.vh());
        }

        public HTMLElement Render()
        {
            return _content.Render();
        }

        private IEnumerable<IconItem> GetAllIcons()
        {
            var names = Enum.GetNames(typeof(LineAwesome));
            LineAwesome[] values = (LineAwesome[])Enum.GetValues(typeof(LineAwesome));

            for (int i = 0; i < values.Length; i++)
            {
                yield return new IconItem(values[i], names[i]);
            }
        }

        private class IconItem : ISearchableItem
        {
            private string _value;
            private IComponent component;
            public IconItem(LineAwesome icon, string name)
            {
                name = ToValidName(name.Substring(3));
                _value = name + " " + icon.ToString();
                component = Stack().Horizontal().Children(Icon(icon, size: TextSize.Large).MinWidth(36.px()).AlignCenter(), TextBlock($"{name}").Title(icon.ToString()).Wrap().AlignCenter()).PaddingBottom(4.px());
            }

            public bool IsMatch(string searchTerm) => _value.Contains(searchTerm);

            public IComponent Render() => component;
        }


        //Copy of the logic in the generator code, as we don't have the enum names anymore on  Enum.GetNames(typeof(LineAwesome))
        private static string ToValidName(string icon)
        {
            var words = icon.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(i => i.Substring(0, 1).ToUpper() + i.Substring(1))
                            .ToArray();

            var name = string.Join("", words);
            if (char.IsDigit(name[0]))
            {
                return "_" + name;
            }
            else
            {
                return name;
            }
        }
    }
}
