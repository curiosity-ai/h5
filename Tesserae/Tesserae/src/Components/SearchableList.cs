using H5;
using System;
using System.Linq;
using System.Threading.Tasks;
using static H5.dom;
using static Tesserae.UI;

namespace Tesserae.Components
{
    public class SearchableList<T> : IComponent, ISpecialCaseStyling where T : ISearchableItem
    {
        private readonly IDefer _defered;
        private readonly Stack _stack;
        private readonly SearchBox _searchBox;
        private readonly ItemsList _list;
        public HTMLElement StylingContainer => _stack.InnerElement;
        public bool PropagateToStackItemParent => true;
        public ObservableList<T> Items { get; }

        public SearchableList(T[] items, UnitSize[] columns) : this(new ObservableList<T>(items ?? new T[0]), columns)
        {
        }

        public SearchableList(ObservableList<T> items, UnitSize[] columns)
        {
            Items = items ?? new ObservableList<T>();
            _searchBox = new SearchBox().Underlined().SetPlaceholder("Type to search").SearchAsYouType().WidthStretch();
            _list = ItemsList(new IComponent[0], columns);
            _defered = Defer(Items, item =>
            {
                var searchTerm = _searchBox.Text;
                var filteredItems = Items.Where(i => string.IsNullOrWhiteSpace(searchTerm) || i.IsMatch(searchTerm)).Select(i => i.Render()).ToArray();

                _list.Items.Clear();

                if (filteredItems.Any())
                {
                    _list.Items.AddRange(filteredItems);
                }

                return _list.Stretch().AsTask();
            }).WidthStretch().Height(100.px()).Grow(1);

            _searchBox.OnSearch((_, __) => _defered.Refresh());

            _stack = Stack().Children(_searchBox, _defered.Scroll()).WidthStretch().MaxHeight(100.percent());
        }

        public SearchableList<T> WithNoResultsMessage(Func<IComponent> emptyListMessageGenerator)
        {
            _list.WithEmptyMessage(emptyListMessageGenerator ?? throw new ArgumentNullException(nameof(emptyListMessageGenerator)));
            _defered.Refresh();
            return this;
        }

        public SearchableList<T> SearchBox(Action<SearchBox> sb)
        {
            sb(_searchBox);
            return this;
        }

        public dom.HTMLElement Render() => _stack.Render();
    }

    public interface ISearchableItem
    {
        bool IsMatch(string searchTerm);
        IComponent Render();
    }
}
