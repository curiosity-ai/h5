using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tesserae.HTML;
using static H5.Core.dom;
using static Tesserae.UI;

namespace Tesserae.Components
{
    public class Nav : ComponentBase<Nav, HTMLUListElement>, IContainer<Nav.NavLink, Nav.NavLink>, IHasBackgroundColor
    {
        public Nav()
        {
            InnerElement = Ul(_("tss-nav"));
        }

        public NavLink SelectedLink { get; private set; }

        public string Background { get => InnerElement.style.background; set => InnerElement.style.background = value; }

        public override HTMLElement Render()
        {
            return InnerElement;
        }

        public void Add(NavLink component)
        {
            ScrollBar.GetCorrectContainer(InnerElement).appendChild(component.Render());
            component.internalOnSelected += OnNavLinkSelected;
            if (component.IsSelected)
            {
                if (SelectedLink != null) SelectedLink.IsSelected = false;
                RaiseOnChange(component);
                SelectedLink = component;
            }

            if (component.SelectedChild != null)
            {
                if (SelectedLink != null) SelectedLink.IsSelected = false;
                RaiseOnChange(component.SelectedChild);
                SelectedLink = component.SelectedChild;
            }
        }

        public void Clear()
        {
            ClearChildren(ScrollBar.GetCorrectContainer(InnerElement));
        }

        public void Replace(NavLink newComponent, NavLink oldComponent)
        {
            ScrollBar.GetCorrectContainer(InnerElement).replaceChild(newComponent.Render(), oldComponent.Render());

            newComponent.internalOnSelected += OnNavLinkSelected;
            if (newComponent.IsSelected)
            {
                if (SelectedLink != null) SelectedLink.IsSelected = false;
                RaiseOnChange(newComponent);
                SelectedLink = newComponent;
            }
        }
        public Nav Links(params Nav.NavLink[] children)
        {
            children.ForEach(x => Add(x));
            return this;
        }

        public Nav InlineContent(IComponent content)
        {
            Add(new Nav.ComponentInNavLink(content));
            return this;
        }

        private void OnNavLinkSelected(object sender, NavLink e)
        {
            if (SelectedLink != null) SelectedLink.IsSelected = false;
            RaiseOnChange(e);
            SelectedLink = e;
        }

        public Nav Compact()
        {
            InnerElement.classList.add("tss-nav-small");
            return this;
        }

        public Nav SelectMarkerOnRight()
        {
            InnerElement.classList.add("tss-nav-right");
            return this;
        }

        public class ComponentInNavLink : NavLink
        {
            private IComponent Content;

            private bool AlreadyRendered = false;

            public ComponentInNavLink(IComponent content) : base()
            {
                Content = content;
            }

            public override HTMLElement Render()
            {
                if (!AlreadyRendered)
                {
                    AlreadyRendered = true;
                    ClearChildren(_headerDiv);
                    _headerDiv.onclick -= ClickHandler;
                    _headerDiv.appendChild(Content.Render());
                }

                return InnerElement;
            }
        }

        public class NavLink : ComponentBase<NavLink, HTMLLIElement>, IContainer<NavLink, NavLink>, IHasTextSize, IHasBackgroundColor
        {
            protected readonly HTMLSpanElement _textSpan;
            protected HTMLElement _iconSpan;
            protected readonly HTMLDivElement _headerDiv;
            protected readonly HTMLUListElement _childContainer;
            protected readonly HTMLButtonElement _expandButton;

            private bool _canSelectAndExpand = false;
            private int _Level;
            private bool _shouldExpandOnFirstAdd;
            private readonly List<NavLink> Children = new List<NavLink>();


            public NavLink(string text = null, string icon = null)
            {
                _textSpan = Span(_(text: text));
                _childContainer = Ul(_("tss-nav-link-container"));
                _expandButton = Button(_("tss-nav-link-button"));
                _headerDiv = Div(_("tss-nav-link-header"), _expandButton, _textSpan);
                _headerDiv.onclick += ClickHandler;
                _expandButton.onclick += ExpandHandler;
                InnerElement = Li(_("tss-nav-link"), _headerDiv, _childContainer);
                Size = TextSize.Small;
                Weight = TextWeight.Regular;
            }

            public NavLink(IComponent content)
            {
                _childContainer = Ul(_("tss-nav-link-container"));
                _expandButton = Button(_("tss-nav-link-button"));
                _headerDiv = Div(_("tss-nav-link-header"), _expandButton, content.Render());
                _headerDiv.onclick += ClickHandler;
                _expandButton.onclick+= ExpandHandler;
                InnerElement = Li(_("tss-nav-link"), _headerDiv, _childContainer);
                Size = TextSize.Small;
                Weight = TextWeight.Regular;
            }


            public event EventHandler<NavLink> onExpanded;

            internal NavLink SelectedChild { get; private set; }

            internal event EventHandler<NavLink> internalOnSelected;
            public event EventHandler<NavLink> onSelected;

            private void ThrowIfUsingComponent(string method)
            {
                if (_textSpan is null) throw new Exception($"Not allowed to call {method} when using a custom component for rendering the Navlink");
            }

            /// <summary>
            /// Gets or sets NavLink text
            /// </summary>
            public string Text
            {
                get { ThrowIfUsingComponent(nameof(Text));  return _textSpan?.innerText ; }
                set { ThrowIfUsingComponent(nameof(Text)); _textSpan.innerText = value; }
            }

            /// <summary>
            /// Gets or sets NavLink icon (icon class)
            /// </summary>
            public string Icon
            {
                get { ThrowIfUsingComponent(nameof(Icon)); return _iconSpan?.className; }
                set
                {
                    ThrowIfUsingComponent(nameof(Icon));
                    if (string.IsNullOrEmpty(value))
                    {
                        if (_iconSpan != null)
                        {
                            _headerDiv.removeChild(_iconSpan);
                            _iconSpan = null;
                        }

                        return;
                    }

                    if (_iconSpan == null)
                    {
                        _iconSpan = I(_());
                        _headerDiv.insertBefore(_iconSpan, _textSpan);
                    }

                    _iconSpan.className = value;
                }
            }

            public bool IsExpanded
            {
                get => InnerElement.classList.contains("tss-expanded");
                set
                {
                    if (value)
                    {
                        if (!IsExpanded)
                        {
                            onExpanded?.Invoke(this, this);
                            ScrollIntoView();
                        }
                        InnerElement.classList.add("tss-expanded");
                    }
                    else InnerElement.classList.remove("tss-expanded");
                }
            }

            public bool IsSelected
            {
                get => _headerDiv.classList.contains("tss-selected");
                set
                {
                    if (value && !IsSelected)
                    {
                        internalOnSelected?.Invoke(this, this);
                        onSelected?.Invoke(this, this);
                        ScrollIntoView();
                    }
                    UpdateSelectedClass(value);
                }
            }

            private void ScrollIntoView()
            {
                DomObserver.WhenMounted(InnerElement, () => InnerElement.scrollIntoView(new ScrollIntoViewOptions() { block = ScrollLogicalPosition.nearest, inline = ScrollLogicalPosition.nearest , behavior = ScrollBehavior.smooth }));
            }

            private void UpdateSelectedClass(bool isSelected)
            {
                if (isSelected)
                    _headerDiv.classList.add("tss-selected");
                else
                    _headerDiv.classList.remove("tss-selected");
            }

            public bool HasChildren => _childContainer.hasChildNodes();

            internal int Level
            {
                get => _Level;
                set
                {
                    _Level = value;
                    foreach (var c in Children)
                    {
                        c.Level = Level + 1;
                    }
                }
            }

            public TextSize Size
            {
                get => TextSizeExtensions.FromClassList(InnerElement, TextSize.Small);
                set
                {
                    InnerElement.classList.remove(Size.ToClassName());
                    InnerElement.classList.add(value.ToClassName());
                }
            }

            public TextWeight Weight
            {
                get => TextSizeExtensions.FromClassList(InnerElement, TextWeight.Regular);
                set
                {
                    InnerElement.classList.remove(Weight.ToClassName());
                    InnerElement.classList.add(value.ToClassName());
                }
            }

            public TextAlign TextAlign
            {
                get
                {
                    var curFontSize = InnerElement.classList.FirstOrDefault(t => t.StartsWith("tss-textalign-"));
                    if (curFontSize is object && Enum.TryParse<TextAlign>(curFontSize.Substring("tss-textalign-".Length), true, out var result))
                    {
                        return result;
                    }
                    else
                    {
                        return TextAlign.Left;
                    }
                }
                set
                {
                    var curFontSize = InnerElement.classList.FirstOrDefault(t => t.StartsWith("tss-textalign-"));
                    if (curFontSize is object)
                    {
                        InnerElement.classList.remove(curFontSize);
                    }
                    InnerElement.classList.add($"tss-textalign-{value.ToString().ToLower()}");
                }
            }

            public string Background { get => _headerDiv.style.background; set => _headerDiv.style.background = value; }

            public override HTMLElement Render()
            {
                return InnerElement;
            }

            public void Add(NavLink component)
            {
                Children.Add(component);
                ScrollBar.GetCorrectContainer(_childContainer).appendChild(component.Render());
                _headerDiv.classList.add("tss-expandable");
                component.Level = Level + 1;
                component.internalOnSelected += OnChildSelected;
                if (component.IsSelected)
                {
                    internalOnSelected?.Invoke(this, component);

                    if (SelectedChild != null) SelectedChild.IsSelected = false;
                    SelectedChild = component;
                }

                if (component.SelectedChild != null)
                {
                    internalOnSelected?.Invoke(component, component.SelectedChild);

                    if (SelectedChild != null) SelectedChild.IsSelected = false;
                    SelectedChild = component.SelectedChild;
                }

                if(HasChildren && _shouldExpandOnFirstAdd)
                {
                    IsExpanded = true;
                }
            }

            private void OnChildSelected(object sender, NavLink e)
            {
                internalOnSelected?.Invoke(this, e);
            }

            public void Clear()
            {
                ClearChildren(ScrollBar.GetCorrectContainer(_childContainer));
                Children.Clear();
                _headerDiv.classList.remove("tss-expandable");
            }

            public void Replace(NavLink newComponent, NavLink oldComponent)
            {
                ScrollBar.GetCorrectContainer(_childContainer).replaceChild(newComponent.Render(), oldComponent.Render());
                newComponent.internalOnSelected += OnChildSelected;
                if (newComponent.IsSelected) internalOnSelected?.Invoke(this, newComponent);
            }

            public void Remove(NavLink oldComponent)
            {
                _childContainer.removeChild(oldComponent.Render());
            }
            public NavLink InlineContent(IComponent content)
            {
                Add(new Nav.ComponentInNavLink(content));
                return this;
            }

            public NavLink Selected()
            {
                IsSelected = true;
                return this;
            }
            
            public NavLink CanSelectAndExpand()
            {
                _canSelectAndExpand = true;
                return this;
            }

            public NavLink SelectedOrExpandedIf(bool shouldSelect)
            {
                if (shouldSelect)
                {
                    if (HasChildren)
                    {
                        IsExpanded = true;

                        if(_canSelectAndExpand)
                        {
                            IsSelected = true;
                        }
                    }
                    else
                    {
                        IsSelected = true;
                    }

                    _shouldExpandOnFirstAdd = true;
                }
                return this;
            }

            public NavLink Expanded()
            {
                IsExpanded = true;
                return this;
            }

            public NavLink SetText(string text)
            {
                Text = text;
                return this;
            }

            public NavLink SetIcon(string icon)
            {
                Icon = icon;
                return this;
            }

            public NavLink OnSelected(EventHandler<Nav.NavLink> onSelected)
            {
                this.onSelected += onSelected;
                return this;
            }

            public NavLink OnExpanded(EventHandler<Nav.NavLink> onExpanded)
            {
                this.onExpanded += onExpanded;
                return this;
            }

            public NavLink Links(params Nav.NavLink[] children)
            {
                children.ForEach(x => Add(x));
                return this;
            }

            public NavLink LinksAsync(Func<Task<Nav.NavLink[]>> childrenAsync)
            {
                bool alreadyRun = false;
                var dummy = new Nav.NavLink("loading...");
                Add(dummy);
                onExpanded += (s, e) =>
                {
                    if (!alreadyRun)
                    {
                        alreadyRun = true;
                        Task.Run(async () =>
                        {
                            var children = await childrenAsync();
                            Remove(dummy);
                            children.ForEach(x => Add(x));
                        }).FireAndForget();
                    }
                };
                return this;
            }

            protected void ClickHandler(MouseEvent e)
            {
                StopEvent(e);
                if (HasChildren)
                {
                    if(_canSelectAndExpand && !IsSelected)
                    {
                        IsSelected = true;
                    }
                    else
                    {
                        IsExpanded = !IsExpanded;
                    }
                }
                else
                {
                    IsSelected = true;
                }
            }

            protected void ExpandHandler(MouseEvent e)
            {
                if(HasChildren)
                {
                    IsExpanded = !IsExpanded;
                    StopEvent(e);
                }
            }
        }
    }
}
