using System;
using System.Collections.Generic;
using System.Linq;
using Tesserae.HTML;
using static System.Math;
using static H5.Core.dom;
using static Tesserae.UI;

namespace Tesserae.Components
{
    public class VirtualizedList : IComponent
    {
        private const int PagesToVirtualize    = 5;
        private const int InitialPagesToCreate = PagesToVirtualize;

        private readonly ListPageCache<IComponent> _listPageCache;
        private readonly int _pagesToVirtualizeUpperBoundary;
        private readonly int _pagesToVirtualizeLowerBoundary;
        private readonly string _componentHeightInPercentage;
        private readonly string _componentWidthInPercentage;
        private readonly HTMLElement _innerElement;
        private readonly HTMLDivElement _basicListContainer;
        private readonly HTMLDivElement _topSpacingDiv;
        private readonly HTMLDivElement _bottomSpacingDiv;

        private bool _initialPagesCreated;
        private Func<IComponent> _emptyListMessageGenerator;
        private int _currentPage;
        private UnitSize _componentHeight;
        private UnitSize _pageHeight;
        private double _currentScrollPosition;

        public VirtualizedList(int rowsPerPage = 4, int columnsPerRow = 4)
        {
            if (rowsPerPage <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(rowsPerPage));
            }

            if (columnsPerRow <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(columnsPerRow));
            }

            _listPageCache = new ListPageCache<IComponent>(rowsPerPage, columnsPerRow, CreatePageHtmlElement, CreateComponentContainerHtmlElement);

            _pagesToVirtualizeUpperBoundary = (int)Floor((double)PagesToVirtualize / 2);
            _pagesToVirtualizeLowerBoundary = (int)Ceiling((double)PagesToVirtualize / 2);

            _componentHeightInPercentage = GetComponentSize(rowsPerPage);
            _componentWidthInPercentage  = GetComponentSize(columnsPerRow);

            _innerElement       = CreateInnerElementHtmlDivElement();
            _basicListContainer = CreateBasicListContainerHtmlDivElement();
            _topSpacingDiv      = CreateTopSpacingHtmlDivElement();
            _bottomSpacingDiv   = CreateBottomSpacingHtmlDivElement();

            _innerElement.appendChild(_basicListContainer);
            AppendChildrenToBasicListContainerHtmlDivElement(_topSpacingDiv, _bottomSpacingDiv);
        }

        public VirtualizedList WithEmptyMessage(Func<IComponent> emptyListMessageGenerator)
        {
            _emptyListMessageGenerator = emptyListMessageGenerator ?? throw new ArgumentNullException(nameof(emptyListMessageGenerator));

            return this;
        }

        public VirtualizedList WithListItems(IEnumerable<IComponent> listItems)
        {
            if (listItems == null)
            {
                throw new ArgumentNullException(nameof(listItems));
            }

            _listPageCache.AddComponents(listItems);

            if (_listPageCache.HasComponents && !_initialPagesCreated)
            {
                CreatePagesDownwards(GetInitialPages());

                AttachOnLastComponentMountedEvent();
                AttachBasicListContainerOnScrollEvent();

                _initialPagesCreated = true;
            }
            else if (_emptyListMessageGenerator != null)
            {
                _basicListContainer.appendChild(_emptyListMessageGenerator().Render());
            }

            return this;
        }

        public HTMLElement Render() => _innerElement;

        private static string GetComponentSize(int itemsCount) => (100 / itemsCount).percent().ToString();

        private static HTMLDivElement CreateInnerElementHtmlDivElement() => Div(_());

        private static HTMLDivElement CreateSpacingHtmlDivElement(string className) => Div(_(className));

        private static void SetHtmlElementHeight(HTMLElement htmlElement, UnitSize height)
        {
            htmlElement.SetStyle(cssStyleDeclaration =>
            {
                cssStyleDeclaration.height = height.ToString();
            });
        }

        private static void CreatePage(HTMLElement page, Action<HTMLElement> renderingAction) => renderingAction(page);

        private HTMLDivElement CreateBasicListContainerHtmlDivElement() => Div(_("tss-basiclist").WithRole("list"));

        private void AppendChildrenToBasicListContainerHtmlDivElement(params HTMLElement[] htmlElements)
        {
            _basicListContainer.AppendChildren(htmlElements);
        }

        private HTMLDivElement CreateTopSpacingHtmlDivElement()
        {
            return CreateSpacingHtmlDivElement("tss-basiclist-top-spacing");
        }

        private HTMLDivElement CreateBottomSpacingHtmlDivElement()
        {
            return CreateSpacingHtmlDivElement("tss-basiclist-bottom-spacing");
        }

        private void SetBasicListContainerHeight() => SetHtmlElementHeight(_basicListContainer, _pageHeight);

        private void SetTopSpacingDivHeight(UnitSize height) => SetHtmlElementHeight(_topSpacingDiv, height);

        private void SetBottomSpacingDivHeight(UnitSize height)
        {
            SetHtmlElementHeight(_bottomSpacingDiv, height);
        }

        private IEnumerable<HTMLElement> GetInitialPages()
        {
            return RetrievePagesFromCache(Enumerable.Range(1, InitialPagesToCreate));
        }

        private IEnumerable<HTMLElement> RetrievePagesFromCache(IEnumerable<int> rangeOfPageNumbersToRetrieve)
        {
            return _listPageCache.RetrievePagesFromCache(rangeOfPageNumbersToRetrieve);
        }

        private HTMLElement RetrievePageFromCache(int pageNumberToRetrieve)
        {
            return _listPageCache.RetrievePageFromCache(pageNumberToRetrieve);
        }

        private HTMLElement CreatePageHtmlElement(int pageNumber)
        {
            return Div(
                _("tss-basiclist-page")
                    .WithRole("presentation")
                    .WithData("tss-basiclist-pagenumber", pageNumber.ToString()));
        }

        private HTMLElement CreateComponentContainerHtmlElement((int key, IComponent component) componentAndKey)
        {
            var (key, component) = componentAndKey;

            return Div(
                _("tss-basiclist-item",
                    styles: cssStyleDeclaration =>
                    {
                        cssStyleDeclaration.height = _componentHeightInPercentage;
                        cssStyleDeclaration.width  = _componentWidthInPercentage;
                    })
                    .WithRole("listitems")
                    .WithData("tss-basiclist-componentnumber", key.ToString()),
                component.Render());
        }

        private void CreatePageUpwards(HTMLElement page)
        {
            CreatePage(page, pageToCreate =>
            {
                _topSpacingDiv.insertAdjacentElement(InsertPosition.afterend, pageToCreate);
            });
        }

        private void CreatePagesDownwards(IEnumerable<HTMLElement> pages)
        {
            foreach (var page in pages)
            {
                CreatePageDownwards(page);
            }
        }

        private void CreatePageDownwards(HTMLElement page)
        {
            CreatePage(page, pageToCreate =>
            {
                _basicListContainer.insertBefore(page, _bottomSpacingDiv);
            });
        }

        private NodeListOf<Element> GetRenderedPages()
        {
            return _basicListContainer.getElementsByClassName("tss-basiclist-page");
        }

        private void RemoveFirstPageFromBasicListContainer()
        {
            RemovePageFromBasicListContainer(GetRenderedPages(), 0);
        }

        private void RemoveLastPageFromBasicListContainer()
        {
            var pages = GetRenderedPages();

            RemovePageFromBasicListContainer(pages, (int)pages.length - 1);
        }

        private void RemovePageFromBasicListContainer(NodeListOf<Element> pages, int index)
        {
            _basicListContainer.removeChild(pages[index]);
        }

        private void AttachOnLastComponentMountedEvent()
        {
            var lastComponentMounted =
                (HTMLElement)_basicListContainer.lastElementChild.previousElementSibling.lastChild;

            DomObserver.WhenMounted(lastComponentMounted,
                () => OnLastComponentMounted(lastComponentMounted.clientHeight));
        }

        private void AttachBasicListContainerOnScrollEvent()
        {
            _basicListContainer.addEventListener("scroll", OnBasicListContainerScroll);
        }

        private void OnLastComponentMounted(int lastComponentMountedClientHeight)
        {
            if (lastComponentMountedClientHeight <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(lastComponentMountedClientHeight));
            }

            _componentHeight = lastComponentMountedClientHeight.px();
            _pageHeight      = (_componentHeight.Size * _listPageCache.RowsPerPage).px();

            SetBasicListContainerHeight();
            SetTopSpacingDivHeight(0.px());

            var initialBottomSpacingDivHeight =
                ((_listPageCache.PagesCount - InitialPagesToCreate) * _pageHeight.Size).px();

            SetBottomSpacingDivHeight(initialBottomSpacingDivHeight);
        }

        private void OnBasicListContainerScroll(object listener)
        {
            var scrollTop       = _basicListContainer.scrollTop;
            var scrollDirection = GetScrollDirection(scrollTop);

            if (scrollDirection == ScrollDirection.Neutral)
            {
                console.log("Scroll neutral");
            }

            var scrollPosition = scrollTop;

            var newPage = (int)Round(scrollPosition / _pageHeight.Size, MidpointRounding.AwayFromZero);

            if ((newPage != _currentPage) && newPage > _pagesToVirtualizeLowerBoundary)
            {
                if (scrollDirection == ScrollDirection.Down)
                {
                    console.log($"Scroll down - new page: {newPage}");

                    RemoveFirstPageFromBasicListContainer();

                    var newTopSpacingDivHeight = ((newPage - _pagesToVirtualizeLowerBoundary) * _pageHeight.Size).px();
                    SetTopSpacingDivHeight(newTopSpacingDivHeight);

                    var pageNumberToAdd = newPage + _pagesToVirtualizeUpperBoundary;
                    CreatePageDownwards(RetrievePageFromCache(pageNumberToAdd));

                    var newBottomSpacingDivHeight =
                        ((_listPageCache.PagesCount - (newPage + _pagesToVirtualizeUpperBoundary)) * _pageHeight.Size).px();

                    SetBottomSpacingDivHeight(newBottomSpacingDivHeight);
                }
                else if (scrollDirection == ScrollDirection.Up)
                {
                    console.log($"Scroll up - new page: {newPage}");

                    RemoveLastPageFromBasicListContainer();

                    var newTopSpacingDivHeight =
                        ((newPage - (_pagesToVirtualizeUpperBoundary - 1)) * _pageHeight.Size).px();
                    SetTopSpacingDivHeight(newTopSpacingDivHeight);

                    var pageNumberToAdd = newPage - _pagesToVirtualizeUpperBoundary;
                    CreatePageUpwards(RetrievePageFromCache(pageNumberToAdd));

                    var newBottomSpacingDivHeight =
                        ((_listPageCache.PagesCount - (newPage + _pagesToVirtualizeLowerBoundary)) * _pageHeight.Size).px();

                    SetBottomSpacingDivHeight(newBottomSpacingDivHeight);
                }
            }

            _currentPage            = newPage;
            _currentScrollPosition  = scrollPosition;
        }

        private ScrollDirection GetScrollDirection(double scrollTop)
        {
            if (scrollTop > _currentScrollPosition)
            {
                return ScrollDirection.Down;
            }

            if (scrollTop < _currentScrollPosition)
            {
                return ScrollDirection.Up;
            }

            return  ScrollDirection.Neutral;
        }

        private enum ScrollDirection
        {
            Neutral = 0,
            Up,
            Down
        }
    }
}
