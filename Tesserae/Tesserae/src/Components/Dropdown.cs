using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tesserae.HTML;
using static H5.Core.dom;
using static Tesserae.UI;

namespace Tesserae.Components
{
    public sealed class Dropdown : Layer<Dropdown>, IContainer<Dropdown, Dropdown.Item>, ICanValidate<Dropdown>, IObservableListComponent<Dropdown.Item>
    {
        private readonly HTMLElement _childContainer;

        private readonly HTMLDivElement _container;
        private readonly HTMLSpanElement _errorSpan;

        private HTMLDivElement _spinner;

        private bool _isChanged;
        private bool _callSelectOnAdd = true;
        private Func<Task<Item[]>> _itemsSource;
        private ObservableList<Item> _selectedChildren;

        public Dropdown()
        {
            InnerElement = Div(_("tss-dropdown"));
            _errorSpan = Span(_("tss-dropdown-error"));

            _container = Div(_("tss-dropdown-container"), InnerElement, _errorSpan);

            _childContainer = Div(_());

            InnerElement.onclick = (e) =>
            {
                StopEvent(e);
                if (!IsVisible) Show();
            };

            _container.onclick = (e) =>
            {
                StopEvent(e);
                if (!IsVisible) Show();
            };

            _selectedChildren = new ObservableList<Item>();
        }

        public Dropdown SuppressSelectedOnAddingItem()
        {
            _callSelectOnAdd = false;
            return this;
        }

        public SelectMode Mode
        {
            get => _childContainer.classList.contains("tss-dropdown-multi") ? SelectMode.Multi : SelectMode.Single;
            set
            {
                if (value == SelectMode.Single)
                {
                    _childContainer.classList.remove("tss-dropdown-multi");
                }
                else
                {
                    _childContainer.classList.add("tss-dropdown-multi");
                }
            }
        }

        public Item[] SelectedItems => _selectedChildren.ToArray();

        public string SelectedText
        {
            get
            {
                return string.Join(", ", _selectedChildren.Select(x => x.Text));
            }
        }

        public string Error
        {
            get => _errorSpan.innerText;
            set => _errorSpan.innerText = value;
        }

        public bool HasBorder
        {
            get => !_container.classList.contains("tss-noborder");
            set
            {
                if (value)
                {
                    _container.classList.remove("tss-noborder");
                }
                else
                {
                    _container.classList.add("tss-noborder");
                }
            }
        }

        public bool IsInvalid
        {
            get => _container.classList.contains("tss-invalid");
            set
            {
                if (value)
                {
                    _container.classList.add("tss-invalid");
                }
                else
                {
                    _container.classList.remove("tss-invalid");
                }
            }
        }

        public bool IsEnabled
        {
            get => !_container.classList.contains("tss-disabled");
            set
            {
                if (value)
                {
                    _container.classList.remove("tss-disabled");
                }
                else
                {
                    _container.classList.add("tss-disabled");
                }
            }
        }

        public bool IsRequired
        {
            get => _container.classList.contains("tss-required");
            set
            {
                if (value)
                {
                    _container.classList.add("tss-required");
                }
                else
                {
                    _container.classList.remove("tss-required");
                }
            }
        }


        public void Clear()
        {
            ClearChildren(ScrollBar.GetCorrectContainer(_childContainer));
        }

        public void Replace(Item newComponent, Item oldComponent)
        {
            ScrollBar.GetCorrectContainer(_childContainer).replaceChild(newComponent.Render(), oldComponent.Render());
        }

        public void Add(Item component)
        {
            ScrollBar.GetCorrectContainer(_childContainer).appendChild(component.Render());
            component.onSelectedChange += OnItemSelected;

            if (component.IsSelected)
            {
                OnItemSelected(null, component);
            }
        }

        public override HTMLElement Render()
        {
            DomObserver.WhenMounted(_container, () =>
            {
                DomObserver.WhenRemoved(_container, () =>
                {
                    Hide();
                });
            });
            return _container;
        }

        public async Task LoadItemsAsync()
        {
            if (_itemsSource is null) throw new InvalidOperationException("Only valid with async items");
            var itemsSourceLocal = _itemsSource;
            _itemsSource = null; //Clear so we don't call this twice
            _spinner = Div(_("tss-spinner"));
            _container.appendChild(_spinner);
            _container.style.pointerEvents = "none";
            var items = await itemsSourceLocal();
            Clear();
            Items(items);
            _container.removeChild(_spinner);
            _container.style.pointerEvents = "unset";
        }

        public override void Show()
        {
            if (_contentHtml == null)
            {
                _contentHtml = Div(_("tss-dropdown-popup"), _childContainer);
                if (_itemsSource is object)
                {
                    LoadItemsAsync().ContinueWith(t => Show()).FireAndForget();
                    return;
                }
            }

            _contentHtml.style.height = "unset";
            _contentHtml.style.left = "-1000px";
            _contentHtml.style.top = "-1000px";

            base.Show();

            _isChanged = false;

            if (!_contentHtml.classList.contains("tss-no-focus")) _contentHtml.classList.add("tss-no-focus");

            ClientRect rect = (ClientRect)_container.getBoundingClientRect();
            var contentRect = (ClientRect)_contentHtml.getBoundingClientRect();
            _contentHtml.style.top = rect.bottom - 1 + "px";
            _contentHtml.style.minWidth = rect.width + "px";

            var finalLeft = rect.left;
            if(rect.left + contentRect.width + 1 > window.innerWidth)
            {
                finalLeft = window.innerWidth - contentRect.width - 1;
            }

            _contentHtml.style.left = finalLeft + "px";

            if (window.innerHeight - rect.bottom - 1 < contentRect.height)
            {
                var top = rect.top - contentRect.height;
                if (top < 0)
                {
                    if (rect.top > window.innerHeight - rect.bottom - 1)
                    {
                        _contentHtml.style.top = "1px";
                        _contentHtml.style.height = rect.top - 1 + "px";
                    }
                    else
                    {
                        _contentHtml.style.height = window.innerHeight - rect.bottom - 1 + "px";
                    }
                }
                else
                {
                    _contentHtml.style.top = top + "px";
                }
            }

            window.setTimeout((e) =>
            {
                document.addEventListener("click", OnWindowClick);
                document.addEventListener("dblclick", OnWindowClick);
                document.addEventListener("contextmenu", OnWindowClick);
                document.addEventListener("wheel", OnWindowClick);
                document.addEventListener("keydown", OnPopupKeyDown);
                if (_selectedChildren.Count > 0) _selectedChildren[_selectedChildren.Count - 1].Render().focus();
            }, 100);
        }

        public override void Hide(Action onHidden = null)
        {
            document.removeEventListener("click", OnWindowClick);
            document.removeEventListener("dblclick", OnWindowClick);
            document.removeEventListener("contextmenu", OnWindowClick);
            document.removeEventListener("wheel", OnWindowClick);
            document.removeEventListener("keydown", OnPopupKeyDown);
            base.Hide(onHidden);
            if (_isChanged) RaiseOnChange(this);
        }

        public void Attach(EventHandler<Event> handler, Validation.Mode mode)
        {
            if (mode == Validation.Mode.OnBlur)
            {
                onChange += (s, e) => handler(this, e);
            }
            else
            {
                onInput += (s, e) => handler(this, e);
            }
        }

        public Dropdown Single()
        {
            Mode = SelectMode.Single;
            return this;
        }

        public Dropdown Multi()
        {
            Mode = SelectMode.Multi;
            return this;
        }

        public Dropdown NoArrow()
        {
            InnerElement.classList.add("tss-dropdown-noarrow");
            return this;
        }

        public Dropdown Items(params Item[] children)
        {
            children.ForEach(x => Add(x));
            return this;
        }
        public Dropdown Items(Func<Task<Item[]>> itemsSource)
        {
            _itemsSource = itemsSource;
            return this;
        }

        public Dropdown Disabled(bool value = true)
        {
            IsEnabled = !value;
            return this;
        }
        
        public Dropdown NoBorder()
        {
            HasBorder = false;
            return this;
        }

        public Dropdown Required()
        {
            IsRequired = true;
            return this;
        }

        private void OnWindowClick(Event e)
        {
            if (e.srcElement != _childContainer && !_childContainer.contains(e.srcElement)) Hide();
        }

        private void OnItemSelected(object sender, Item e)
        {
            if (Mode == SelectMode.Single && !e.IsSelected) return;

            if (Mode == SelectMode.Single)
            {
                if (_selectedChildren.Count > 0)
                {
                    foreach (var selectedChild in _selectedChildren.ToArray()) //Need to copy as setting IsSelected will get back here
                    {
                        if(selectedChild != e)
                        {
                            selectedChild.IsSelected = false;
                        }
                    }

                    _selectedChildren.Clear();
                }

                if (!_selectedChildren.Contains(e))
                {
                    _selectedChildren.Add(e);
                }

                Hide();
            }
            else
            {
                if(e.IsSelected)
                {
                    if (!_selectedChildren.Contains(e))
                    {
                        _selectedChildren.Add(e);
                    }
                }
                else
                {
                    if (_selectedChildren.Contains(e))
                    {
                        _selectedChildren.Remove(e);
                    }
                }

                _isChanged = true;
            }

            RenderSelected();

            if (_callSelectOnAdd)
            {
                RaiseOnInput(this);
            }
        }

        private void RenderSelected()
        {
            ClearChildren(InnerElement);

            for (int i = 0; i < SelectedItems.Length; i++)
            {
                Item sel = SelectedItems[i];
                var clone = (HTMLElement)(sel.RenderSelected());
                clone.classList.remove("tss-dropdown-item");
                clone.classList.remove("tss-selected");
                clone.classList.add("tss-dropdown-item-on-box");
                InnerElement.appendChild(clone);
            }
        }

        private void OnPopupKeyDown(Event e)
        {
            var ev = e as KeyboardEvent;
            if (ev.key == "ArrowUp")
            {
                if (_contentHtml.classList.contains("tss-no-focus")) _contentHtml.classList.remove("tss-no-focus");
                if (document.activeElement != null && _childContainer.contains(document.activeElement))
                {
                    var el = (_childContainer.children.TakeWhile(x => !x.Equals(document.activeElement)).LastOrDefault(x => (x as HTMLElement).tabIndex != -1) as HTMLElement);
                    if (el != null) el.focus();
                    else (_childContainer.children.Last(x => (x as HTMLElement).tabIndex != -1) as HTMLElement).focus();
                }
                else
                {
                    (_childContainer.children.Last(x => (x as HTMLElement).tabIndex != -1) as HTMLElement).focus();
                }
            }
            else if (ev.key == "ArrowDown")
            {
                if (_contentHtml.classList.contains("tss-no-focus")) _contentHtml.classList.remove("tss-no-focus");
                if (document.activeElement != null && _childContainer.contains(document.activeElement))
                {
                    var el = (_childContainer.children.SkipWhile(x => !x.Equals(document.activeElement)).Skip(1).FirstOrDefault(x => (x as HTMLElement).tabIndex != -1) as HTMLElement);
                    if (el != null) el.focus();
                    else (_childContainer.children.First(x => (x as HTMLElement).tabIndex != -1) as HTMLElement).focus();
                }
                else
                {
                    (_childContainer.children.First(x => (x as HTMLElement).tabIndex != -1) as HTMLElement).focus();
                }
            }
        }

        public IObservable<IReadOnlyList<Item>> AsObservable()
        {
            return _selectedChildren;
        }

        public enum SelectMode
        {
            Single,
            Multi
        }

        public enum ItemType
        {
            Item,
            Header,
            Divider
        }

        public class Item : IComponent
        {

            private readonly HTMLElement InnerElement;
            private readonly HTMLElement SelectedElement;
            private dynamic _data;
            public dynamic Data => _data;
            public Item(string text, string selectedText = null) : this(TextBlock(text), TextBlock(string.IsNullOrEmpty(selectedText) ? text : selectedText) )
            {
            }

            public Item(IComponent content, IComponent selectedContent)
            {
                InnerElement = Button(_("tss-dropdown-item"));
                InnerElement.appendChild(content.Render());

                if(selectedContent is null || selectedContent == content)
                {
                    SelectedElement = (HTMLElement)InnerElement.cloneNode(true);
                }
                else
                {
                    SelectedElement = Button(_("tss-dropdown-item"));
                    SelectedElement.appendChild(selectedContent.Render());
                }

                InnerElement.addEventListener("click", OnItemClick);
                InnerElement.addEventListener("mouseover", OnItemMouseOver);
            }


            public event BeforeSelectEventHandler<Item> onBeforeSelected;
            internal event EventHandler<Item> onSelectedChange;

            public ItemType Type
            {
                get
                {
                    if (InnerElement.classList.contains("tss-dropdown-item")) return ItemType.Item;
                    if (InnerElement.classList.contains("tss-dropdown-header")) return ItemType.Header;
                    return ItemType.Divider;
                }

                set
                {
                    InnerElement.classList.remove($"tss-dropdown-{Type.ToString().ToLower()}");
                    InnerElement.classList.add($"tss-dropdown-{value.ToString().ToLower()}");

                    if (value == ItemType.Item) InnerElement.tabIndex = 0;
                    else InnerElement.tabIndex = -1;
                }
            }

            public bool IsEnabled
            {
                get => !InnerElement.classList.contains("tss-disabled");
                set
                {
                    if (value)
                    {
                        InnerElement.classList.remove("tss-disabled");
                        if (Type == ItemType.Item) InnerElement.tabIndex = 0;
                    }
                    else
                    {
                        InnerElement.classList.add("tss-disabled");
                        InnerElement.tabIndex = -1;
                    }
                }
            }

            public bool IsSelected
            {
                get => InnerElement.classList.contains("tss-selected");
                set
                {
                    var changed = value != IsSelected;
                    if (changed)
                    {
                        if (value && onBeforeSelected is object)
                        {
                            var shouldSelect = onBeforeSelected(this);
                            if (!shouldSelect) return;
                        }
                    }

                    if (value)
                    {
                        InnerElement.classList.add("tss-selected");
                    }
                    else
                    {
                        InnerElement.classList.remove("tss-selected");
                    }

                    if (changed)
                    {
                        onSelectedChange?.Invoke(this, this);
                    }
                }
            }

            public string Text
            {
                get => InnerElement.innerText;
                set => InnerElement.innerText = value;
            }

            public HTMLElement Render()
            {
                return InnerElement;
            }

            public HTMLElement RenderSelected()
            {
                return SelectedElement;
            }

            public Item Header()
            {
                Type = ItemType.Header;
                return this;
            }
            public Item Divider()
            {
                Type = ItemType.Divider;
                return this;
            }
            public Item Disabled(bool value = true)
            {
                IsEnabled = !value;
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

            public Item SetData(dynamic data)
            {
                _data = data;
                return this;
            }

            public Item OnSelected(EventHandler<Item> onSelected, EventHandler<Item> onDeselected = null)
            {
                onSelectedChange += (s,e) =>
                {
                    if(e.IsSelected)
                    {
                        onSelected?.Invoke(s, e);
                    }
                    else
                    {
                        onDeselected?.Invoke(s, e);
                    }
                };
                return this;
            }

            public Item OnBeforeSelected(BeforeSelectEventHandler<Item> onBeforeSelect)
            {
                onBeforeSelected += onBeforeSelect;
                return this;
            }

            private void OnItemClick(Event e)
            {
                if (Type == ItemType.Item)
                {
                    if (InnerElement.parentElement.classList.contains("tss-dropdown-multi")) IsSelected = !IsSelected;
                    else IsSelected = true;
                }
            }

            private void OnItemMouseOver(Event ev)
            {
                if (Type == ItemType.Item) InnerElement.focus();
            }
        }
    }
}
