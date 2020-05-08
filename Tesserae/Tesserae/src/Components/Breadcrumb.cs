using System;
using static Tesserae.UI;
using static H5.dom;
using Tesserae.HTML;
using System.Collections.Generic;
using System.Linq;

namespace Tesserae.Components
{
    public class Breadcrumb : IComponent, IContainer<Breadcrumb, IComponent>
    {
        private HTMLElement _childContainer;
        private ResizeObserver _resizeObserver;
        private int _maximumItemsToDisplay = 10;
        private int _overflowIndex = 0;
        private bool _cacheSizes;
        private double _cachedFullWidth = 0;
        private HTMLElement _chevronToUseAsButton = null;

        private string _chevronIcon = "la-chevron-right";

        private Dictionary<HTMLElement, double> _cachedSizes = new Dictionary<HTMLElement, double>();

        public int MaximumItemsToDisplay
        {
            get => _maximumItemsToDisplay;
            set
            {
                _maximumItemsToDisplay = value;
                Recompute();
            }
        }

        public int OverflowIndex
        {
            get => _overflowIndex;
            set
            {
                _overflowIndex = value;
                Recompute();
            }
        }

        public bool IsSmall
        {
            get => _childContainer.classList.contains("tss-small");
            set
            {
                if (value) { _childContainer.classList.add("tss-small"); }
                else { _childContainer.classList.remove("tss-small"); }
            }
        }


        public Breadcrumb()
        {
            _childContainer = Div(_("tss-breadcrumb"));
            DomObserver.WhenMounted(_childContainer, Recompute);
            _resizeObserver = new ResizeObserver();
            _resizeObserver.Observe(document.body);
            _resizeObserver.OnResize = Recompute;
        }

        private void Recompute()
        {

            int childElementCount = (int)_childContainer.childElementCount;
            if (childElementCount == 0) return;


            if (_chevronToUseAsButton is object)
            {
                //Reset modified chevron if any
                _chevronToUseAsButton.classList.add(_chevronIcon, "tss-breadcrumb-collapse");
                _chevronToUseAsButton.classList.remove("la-ellipsis-h", "tss-breadcrumb-opencolapsed");

                _chevronToUseAsButton.onclick = null;
                _chevronToUseAsButton = null;
            }

            UpdateChildrenSizes();

            bool isChevron(HTMLElement e) => e.classList.contains("tss-breadcrumb-chevron");

            var keep = new int[childElementCount];

            const int KEEP = 2;
            const int COLLAPSE = 1;
            const int NOTMEASURED = 0;

            if (_overflowIndex >= 0)
            {
                for (int i = 0; i <= Math.Min(keep.Length - 1, ((_overflowIndex) * 2)); i++)
                {
                    keep[i] = KEEP;
                    if ((i+1 < _overflowIndex-2))
                    {
                        var child = (HTMLElement)_childContainer.children[(uint)i+1];
                        if (isChevron(child))
                        {
                            keep[i + 1] = KEEP;
                        }
                    }
                }
            }

            keep[keep.Length-1] = KEEP;

            var debt = _cachedFullWidth - _cachedSizes.Values.Sum() - 64;
            while(debt < 0)
            {
                var candidate = Array.IndexOf(keep, NOTMEASURED);
                if(candidate >= 0)
                {
                    keep[candidate] = COLLAPSE;
                    var child = (HTMLElement)_childContainer.children[(uint)candidate];
                    debt += _cachedSizes[child];
                }
                else
                {
                    break;
                }
            }

            var hidden = new List<HTMLElement>();

            for (uint i = 0; i < _childContainer.childElementCount; i++)
            {
                var child = (HTMLElement)_childContainer.children[i];
                if (keep[i] == COLLAPSE)
                {
                    if(_chevronToUseAsButton is null)
                    {
                        if (isChevron(child))
                        {
                            _chevronToUseAsButton = child;
                            continue; //Don't collapse this, instead keep for menu button
                        }
                        else if (i > 0)
                        {
                            //previous element is a chevron, so use it instead
                            _chevronToUseAsButton = (HTMLElement)_childContainer.children[i - 1];
                        }
                    }

                    if (!isChevron(child)) hidden.Add(child);
                    child.classList.add("tss-breadcrumb-collapse");
                }
                else
                {
                    child.classList.remove("tss-breadcrumb-collapse");
                }
            }


            IComponent clone(Node node)
            {
                var c = (HTMLElement)(node.cloneNode(true));
                c.classList.remove("tss-breadcrumb-collapse");
                return Raw(c);
            }

            if (_chevronToUseAsButton is object)
            {
                _chevronToUseAsButton.classList.add("la-ellipsis-h", "tss-breadcrumb-opencolapsed");
                _chevronToUseAsButton.classList.remove(_chevronIcon, "tss-breadcrumb-collapse");
                _chevronToUseAsButton.onclick = (e) =>
                {
                    StopEvent(e);
                    var clones = hidden.Select(element => ContextMenuItem(clone(element)).OnClick((s2, e2) => element.click())).ToArray();
                    ContextMenu().Items(clones).ShowFor(_chevronToUseAsButton);
                };
            }

        }

        private void UpdateChildrenSizes()
        {
            if (!_cacheSizes)
            {
                _cachedSizes.Clear();

                for (uint i = 0; i < _childContainer.childElementCount; i++)
                {
                    var child = (HTMLElement)_childContainer.children[i];
                    child.classList.remove("tss-breadcrumb-collapse");
                }

                var rect = (DOMRect)_childContainer.getBoundingClientRect();
                _cachedFullWidth = rect.width;
            }



            foreach (HTMLElement child in _childContainer.children)
            {
                if (!_cachedSizes.ContainsKey(child))
                {
                    var childRect = (DOMRect)child.getBoundingClientRect();
                    _cachedSizes[child] = childRect.width;
                }
            }
        }

        public void Clear()
        {
            ClearChildren(_childContainer);
        }

        public void Replace(IComponent newComponent, IComponent oldComponent)
        {
            _childContainer.replaceChild(newComponent.Render(), oldComponent.Render());
        }

        public void Add(IComponent component)
        {
            if(_childContainer.childElementCount > 0)
            {
                _childContainer.appendChild(I(_("tss-breadcrumb-chevron las " + _chevronIcon)));
            }
            _childContainer.appendChild(component.Render());
        }

        public Breadcrumb Items(params IComponent[] children)
        {
            children.ForEach(x => Add(x));
            return this;
        }

        public Breadcrumb DisableSizeCache()
        {
            _cacheSizes = false;
            return this;
        }

        public Breadcrumb SetOverflowIndex(int i)
        {
            _overflowIndex = i;
            return this;
        }

        public Breadcrumb SetChevron(string icon)
        {
            _chevronIcon = icon;
            return this;
        }

        public Breadcrumb Small()
        {
            IsSmall = true;
            return this;
        }

        public HTMLElement Render()
        {
            return _childContainer;
        }
    }
}
