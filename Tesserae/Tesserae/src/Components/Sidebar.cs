using System;
using System.Collections.Generic;
using System.Linq;
using Tesserae.HTML;
using static H5.Core.dom;
using static Tesserae.UI;

namespace Tesserae.Components
{
    public class Sidebar : IComponent
    {
        public enum Size
        {
            Small,
            Medium,
            Large
        }

        private HTMLElement _sidebarContainer;
        private HTMLElement _contentContainer;
        private HTMLElement _container;
        private List<Item> _items = new List<Item>();
        private ResizeObserver _resizeObserver;

        public event OnBeforeSelectHandler onBeforeSelect;
        public delegate bool OnBeforeSelectHandler(Item willBeSelected, Item currentlySelected);

        public bool IsLight
        {
            get => _sidebarContainer.classList.contains("tss-light");
            set
            {
                if (value) _sidebarContainer.classList.add("tss-light");
                else _sidebarContainer.classList.remove("tss-light");
            }
        }

        public Size Width
        {
            get
            {

                if (_sidebarContainer.classList.contains("tss-small"))
                {
                    return Size.Small;
                }
                else if (_sidebarContainer.classList.contains("tss-medium"))
                {
                    return Size.Medium;
                }
                else
                {
                    return Size.Large;
                }
            }
            set
            {
                if (value == Size.Small)
                {
                    _sidebarContainer.classList.add("tss-small");
                    _sidebarContainer.classList.remove("tss-medium");
                }
                else if (value == Size.Medium)
                {
                    _sidebarContainer.classList.add("tss-medium");
                    _sidebarContainer.classList.remove("tss-small");
                }
                else
                {
                    _sidebarContainer.classList.remove("tss-small");
                    _sidebarContainer.classList.remove("tss-medium");
                }
            }
        }

        public bool IsVisible
        {
            get => !_container.classList.contains("tss-hidden");
            set
            {
                if (value) _container.classList.remove("tss-hidden");
                else _container.classList.add("tss-hidden");
            }
        }

        public bool IsAlwaysOpen
        {
            get => _container.classList.contains("tss-open");
            set
            {
                if (value)
                {
                    _container.classList.add("tss-open");
                    EnableResizeMonitor();
                }
                else
                {
                    _container.classList.remove("tss-open");
                }
                RecomputeContainerMargin();
            }
        }


        public Sidebar()
        {
            _sidebarContainer = Div(_("tss-sidebar"));
            _contentContainer = Div(_("tss-sidebar-content"));
            _container = Div(_("tss-sidebar-host"), _sidebarContainer, _contentContainer);
            Width = Size.Medium;
        }

        public Sidebar SetContent(IComponent content)
        {
            ClearChildren(_contentContainer);
            _contentContainer.appendChild(content.Render());
            return this;
        }

        public Sidebar Light()
        {
            IsLight = true;
            return this;
        }
        public Sidebar Small()
        {
            Width = Size.Small;
            return this;
        }

        public Sidebar Large()
        {
            Width = Size.Large;
            return this;
        }

        public Sidebar AlwaysOpen()
        {
            IsAlwaysOpen = true;
            return this;
        }

        public Sidebar Clear()
        {
            _items.Clear();
            ClearChildren(_sidebarContainer);
            return this;
        }

        public Sidebar Brand(IComponent brand)
        {
            if(_sidebarContainer.childElementCount == 0)
            {
                _sidebarContainer.appendChild(brand.Render());
            }
            else
            {
                _sidebarContainer.insertBefore(brand.Render(), _sidebarContainer.firstElementChild);
            }
            return this;
        }

        public Sidebar Add(Item item)
        {
            item.parent = this;
            _items.Add(item);
            _sidebarContainer.appendChild(item.Render());
            return this;
        }

        public Sidebar OnBeforeSelect(OnBeforeSelectHandler onBeforeSelect)
        {
            this.onBeforeSelect += onBeforeSelect;
            return this;
        }

        public HTMLElement Render()
        {
            return _container;
        }

        private void SelectItem(Item item)
        {
            foreach(var i in _items)
            {
                if(i != item)
                {
                    i.IsSelected = false;
                }
            }
        }

        private void EnableResizeMonitor()
        {
            if (_resizeObserver is null)
            {
                _resizeObserver = new ResizeObserver();
                _resizeObserver.Observe(_sidebarContainer);
                _resizeObserver.OnResize = RecomputeContainerMargin;
            }
        }

        private void RecomputeContainerMargin()
        {
            if(IsAlwaysOpen)
            {
                var rect = (DOMRect)_sidebarContainer.getBoundingClientRect();
                _contentContainer.style.marginLeft = rect.width.px().ToString();
            }
            else
            {
                _contentContainer.style.marginLeft = "";
            }
        }

        private bool OnBeforeSelect(Item willBeSelected)
        {
            var currentlySelected = _items.Where(i => i.IsSelected).FirstOrDefault();

            if (onBeforeSelect is object)
            {
                return onBeforeSelect(willBeSelected, currentlySelected);
            }
            else
            {
                return true;
            }
        }

        public class Item : IComponent
        {
            protected HTMLElement _container;
            private HTMLSpanElement _label;
            private HTMLElement _icon;
            private bool _isSelectable = true;
            private bool _hasOnClick = false;
            private bool _hasOnSelect = false;
            internal Sidebar parent;

            public bool IsEnabled
            {
                get => !_container.classList.contains("tss-disabled");
                set { if(value) _container.classList.add("tss-disabled"); else _container.classList.remove("tss-disabled"); }
            }

            public bool IsLarge
            {
                get => !_container.classList.contains("tss-extrapadding");
                set { if(value) _container.classList.add("tss-extrapadding"); else _container.classList.remove("tss-extrapadding"); }
            }

            public bool IsSelectable
            {
                get => _isSelectable;
                set
                {
                    _isSelectable = value;
                    if(!_isSelectable)
                    {
                        _container.classList.remove("tss-selected");
                        _container.classList.add("tss-nonselectable");
                    }
                    else
                    {
                        _container.classList.remove("tss-nonselectable");
                    }
                }
            }

            public event SidebarItemHandler onClick;
            public event SidebarItemHandler onSelected;

            public delegate void SidebarItemHandler(Item sender);

            public bool IsSelected
            {
                get => IsSelectable ? _container.classList.contains("tss-selected") : false;
                set
                {
                    if (!IsSelectable) return;

                    var changed = value != IsSelected;

                    if (value)
                    {
                        _container.classList.add("tss-selected");
                        if (changed)
                        {
                            parent?.SelectItem(this);
                        }
                    }
                    else
                    {
                        _container.classList.remove("tss-selected");
                    }
                }
            }

            public Item(string text, IComponent icon, string href = null)
            {
                _icon = icon.Render();
                CreateSelf(text, href);
            }

            public Item(string text, string icon, string href = null)
            {
                _icon = I(_(icon));
                CreateSelf(text, href);
            }

            private void CreateSelf(string text, string href)
            {
                _label = Span(_("tss-sidebar-label", text: text));

                if (string.IsNullOrEmpty(href))
                {
                    _container = Div(_("tss-sidebar-item"), Div(_("tss-sidebar-icon"), _icon), _label);
                }
                else
                {
                    _container = A(_("tss-sidebar-item" ,href:href), Div(_("tss-sidebar-icon"), _icon), _label);
                }

                _container.onclick = (e) =>
                {
                    if (_hasOnClick || _hasOnSelect)
                    {
                        StopEvent(e);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(href))
                        {
                            StopEvent(e);
                            Router.Navigate(href);
                        }
                    }
                        
                    onClick?.Invoke(this);

                    if (!IsSelectable)
                    {
                        return;
                    }

                    if (parent is object)
                    {
                        if (!parent.OnBeforeSelect(this))
                        {
                            return;
                        }
                    }

                    IsSelected = true;

                    onSelected?.Invoke(this);
                };
            }

            public Item SetIcon(string icon)
            {
                if (_icon is null) return this;
                _icon.className = icon;
                return this;
            }

            public Item SetText(string text)
            {
                if (_label is null) return this;
                _label.textContent = text;
                return this;
            }

            public Item Selected()
            {
                IsSelected = true;
                return this;
            }

            public Item SelectedIf(bool shouldSelect)
            {
                if (shouldSelect)
                {
                    IsSelected = true;
                }
                return this;
            }

            public Item Large()
            {
                IsLarge = true;
                return this;
            }

            public Item NonSelectable()
            {
                IsSelectable = false;
                return this;
            }

            public Item ShowOnBottom()
            {
                _container.style.position = "absolute";
                _container.style.bottom = "16px";
                return this;
            }

            public Item OnSelect(SidebarItemHandler onSelect)
            {
                _hasOnSelect = true;
                onSelected += onSelect;
                return this;
            }
            
            public Item OnClick(SidebarItemHandler onClick)
            {
                _hasOnClick = true;
                this.onClick += onClick;
                return this;
            }

            public HTMLElement Render()
            {
                return _container;
            }
        }
    }
}
