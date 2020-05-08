using System;
using static Tesserae.UI;
using static H5.Core.dom;

namespace Tesserae.Components
{
    public class DetailsListIconColumn : IDetailsListColumn
    {
        private readonly Action _onColumnClick;
        private HTMLElement InnerElement;

        public DetailsListIconColumn(Icon icon, UnitSize width, bool enableColumnSorting = false, string sortingKey = null, Action onColumnClick = null)
        {
            if (enableColumnSorting && string.IsNullOrWhiteSpace(sortingKey))
            {
                throw new ArgumentException(nameof(sortingKey));
            }

            Icon     = icon;
            Width               = width      ?? throw new ArgumentNullException(nameof(width));
            SortingKey          = sortingKey ?? string.Empty;
            EnableColumnSorting = enableColumnSorting;

            if (onColumnClick != null)
            {
                _onColumnClick           = onColumnClick;
                EnableOnColumnClickEvent = true;
            }

            InnerElement = Div(_()).appendChild(Icon.Render());
        }

        public string SortingKey               { get; }

        public Icon Icon                       { get; }

        public UnitSize Width                  { get; }

        public LineAwesomeWeight IconWeight    { get; }

        public bool IsRowHeader                => false;

        public bool EnableColumnSorting        { get; }

        public bool EnableOnColumnClickEvent   { get; }

        public void OnColumnClick()            => _onColumnClick?.Invoke();

        public HTMLElement Render()            => InnerElement;
    }
}
