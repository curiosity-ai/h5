using System;
using System.Collections.Generic;
using System.Linq;
using Tesserae.Components;
using static H5.Core.dom;

namespace Tesserae
{
    public sealed class ListPageCache<TComponent> : ComponentCacheBase<TComponent>
        where TComponent : class
    {
        private readonly Func<int, HTMLElement> _createPageHtmlElementExpression;
        private readonly Func<(int Key, TComponent Component), HTMLElement> _afterComponentRetrievedExpression;
        private readonly Dictionary<int, HTMLElement> _pageCache;
        private readonly List<List<(int Key, TComponent Component)>> _pages;

        public ListPageCache(
            int rowsPerPage,
            int columnsPerRow,
            Func<int, HTMLElement> createPageHtmlElementExpression,
            Func<(int key, TComponent component), HTMLElement> afterComponentRetrievedExpression)
        {
            if (rowsPerPage <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(rowsPerPage));
            }

            if (columnsPerRow <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(columnsPerRow));
            }

            RowsPerPage       = rowsPerPage;
            ComponentsPerPage = RowsPerPage * columnsPerRow;

            _createPageHtmlElementExpression =
                createPageHtmlElementExpression ??
                    throw new ArgumentNullException(nameof(createPageHtmlElementExpression));

            _afterComponentRetrievedExpression =
                afterComponentRetrievedExpression ??
                    throw new ArgumentNullException(nameof(afterComponentRetrievedExpression));

            _pageCache  = new Dictionary<int, HTMLElement>();
            _pages      = new List<List<(int key, TComponent component)>>();
        }

        public int RowsPerPage       { get; }

        public int ComponentsPerPage { get; }

        public int PagesCount        => _pages.Count;

        public int RowsCount         => RowsPerPage * PagesCount;

        public ListPageCache<TComponent> AddComponents(IEnumerable<TComponent> components)
        {
            var currentComponentsCount = ComponentsCount;

            AddToComponents(components);
            AddPages(currentComponentsCount);

            return this;
        }

        public HTMLElement RetrievePageFromCache(int pageNumberToRetrieve)
        {
            if (_pageCache.ContainsKey(pageNumberToRetrieve))
            {
                console.log($"Retrieved page number {pageNumberToRetrieve} from cache");
                return _pageCache.GetValueOrDefault(pageNumberToRetrieve);
            }

            var page = _createPageHtmlElementExpression(pageNumberToRetrieve);

            page.AppendChildren(
                GetComponentsForPage(pageNumberToRetrieve)
                    .Select(_afterComponentRetrievedExpression)
                    .ToArray());

            _pageCache.Add(pageNumberToRetrieve, page);

            return page;
        }

        public IEnumerable<HTMLElement> RetrievePagesFromCache(IEnumerable<int> rangeOfPageNumbersToRetrieve)
        {
            return rangeOfPageNumbersToRetrieve.Select(RetrievePageFromCache);
        }

        public IEnumerable<HTMLElement> RetrieveAllPagesFromCache()
        {
            return Enumerable.Range(1, PagesCount).Select(RetrievePageFromCache);
        }

        public ListPageCache<TComponent> Clear()
        {
            _componentsAndKeys.Clear();
            _pages.Clear();
            _pageCache.Clear();

            return this;
        }

        private void AddPages(int componentNumberToPageFrom)
        {
            var pagesToAdd =
                _componentsAndKeys.Skip(componentNumberToPageFrom).InGroupsOf(ComponentsPerPage);

            _pages.AddRange(pagesToAdd);
        }

        private List<(int key, TComponent component)> GetComponentsForPage(int pageNumber)
        {
            return _pages.ElementAt(pageNumber - 1);
        }
    }
}
