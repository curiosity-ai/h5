using System;
using System.Collections.Generic;
using System.Linq;
using static H5.Core.dom;
using static Tesserae.UI;

namespace Tesserae.Components
{
    public sealed class Picker<TPickerItem> : IComponent, IObservableListComponent<TPickerItem>  where TPickerItem : class, IPickerItem
    {
        private readonly ObservableList<TPickerItem> _pickerItems = new ObservableList<TPickerItem>();
        private readonly HTMLElement _container;
        private readonly TextBox _textBox;
        private readonly SuggestionsLayer _suggestionsLayer;
        private readonly bool _renderSelectionsInline;
        private readonly HTMLElement _selectionsElement;

        private double _debounce;
        private double _debounceTimeout = 50;
        private double _hideSugestionsTimeout;

        private HTMLElement _textBoxElement;

        public Picker(int maximumAllowedSelections = int.MaxValue, bool duplicateSelectionsAllowed = false, int suggestionsTolerance = 0,  bool renderSelectionsInline = true, string suggestionsTitleText = null)
        {
            MaximumAllowedSelections   = maximumAllowedSelections;
            DuplicateSelectionsAllowed = duplicateSelectionsAllowed;
            SuggestionsTolerance       = suggestionsTolerance;
            _renderSelectionsInline    = renderSelectionsInline;
            _selectionsElement         = Div(_("tss-picker-selections"));

            var pickerContainer = Div(_("tss-picker-container"));

            if (_renderSelectionsInline)
            {
                pickerContainer.classList.add("tss-picker-container-inline-selections");
                _selectionsElement.classList.add("tss-picker-selections-inline");
            }

            _container            = DIV();
            _textBox              = TextBox();
            _suggestionsLayer     = new SuggestionsLayer(new Suggestions(suggestionsTitleText));

            CreatePicker(pickerContainer);
        }

        public IObservable<IReadOnlyList<TPickerItem>> AsObservable()
        {
            return _pickerItems;
        }

        public IEnumerable<TPickerItem> PickerItems           => _pickerItems;

        public IEnumerable<TPickerItem> SelectedPickerItems   => _pickerItems.Where(pickerItem => pickerItem.IsSelected);

        public IEnumerable<TPickerItem> UnselectedPickerItems => _pickerItems.Where(pickerItem => !pickerItem.IsSelected);

        public int? MaximumAllowedSelections                  { get; }

        public bool DuplicateSelectionsAllowed                { get; }

        public int SuggestionsTolerance                       { get; }

        public Picker<TPickerItem> WithItems(params TPickerItem[] items)
        {
            return WithItems(items.AsEnumerable());
        }

        public Picker<TPickerItem> WithItems(IEnumerable<TPickerItem> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            items = items.ToList();

            if (!items.Any())
            {
                throw new ArgumentException(nameof(items));
            }

            if (PickerItems.Any(pickerItem => items.Any(item => pickerItem.Name.Equals(item.Name))))
            {
                throw new ArgumentException("Can not add duplicate items");
            }

            _pickerItems.AddRange(items);

            return this;
        }

        public event EventHandler<TPickerItem> onItemSelected;

        public Picker<TPickerItem> OnItemSelected(EventHandler<TPickerItem> eventHandler)
        {
            onItemSelected += eventHandler;

            return this;
        }

        public HTMLElement Render() => _container;

        private void CreatePicker(HTMLElement pickerContainer)
        {
            _container.appendChild(pickerContainer);

            _textBox.OnInput(OnTextBoxInput);
            _textBox.OnFocus(OnTextBoxInput);
            _textBox.OnBlur(OnTextBoxBlur);

            _textBoxElement = _textBox.Render();

            pickerContainer.appendChild(_textBoxElement);

            if (_renderSelectionsInline)
            {
                pickerContainer.insertBefore(_selectionsElement, _textBoxElement);
            }
            else
            {
                pickerContainer.appendChild(_selectionsElement);
            }
        }

        private void OnTextBoxInput(TextBox textBox, Event @event)
        {
            window.clearTimeout(_hideSugestionsTimeout);
            window.clearTimeout(_debounce);
            _debounce = window.setTimeout(_ => ShowSuggestions(), _debounceTimeout);
        }

        private void ShowSuggestions()
        {
            ClearSuggestions();

            if (SuggestionsTolerance > 0 &&
                (string.IsNullOrWhiteSpace(_textBox.Text) || (_textBox.Text.Length < SuggestionsTolerance)))
            {
                _suggestionsLayer.Hide();
                return;
            }

            var suggestions = GetSuggestions(_textBox.Text);

            CreateSuggestions(suggestions);
        }

        private void OnTextBoxBlur(TextBox textBox, Event @event)
        {
            _hideSugestionsTimeout = window.setTimeout(_ =>
            {
                ClearSuggestions();
                _suggestionsLayer.Hide();
            }, 150);
        }

        private IEnumerable<TPickerItem> GetPickerItems()
        {
            if (!MaximumAllowedSelections.HasValue || SelectedPickerItems.Count() < MaximumAllowedSelections)
            {
                return DuplicateSelectionsAllowed ? PickerItems : UnselectedPickerItems;
            }

            return Enumerable.Empty<TPickerItem>();
        }

        private IEnumerable<TPickerItem> GetSuggestions(string textBoxText)
        {
            if (string.IsNullOrWhiteSpace(textBoxText))
            {
                return GetPickerItems();
            }

            textBoxText = textBoxText.ToUpper();

            return GetPickerItems().Where(pickerItem => pickerItem.Name.ToUpper().Contains(textBoxText));
        }

        private void CreateSuggestions(IEnumerable<TPickerItem> suggestions)
        {
            suggestions = suggestions.ToList();

            if (!suggestions.Any())
            {
                _suggestionsLayer.Hide();
                return;
            }

            foreach (var suggestion in suggestions)
            {
                // TODO: Add to a component cache.
                var suggestionContainerElement = Div(_("tss-picker-suggestion"), suggestion.Render().Render());

                suggestionContainerElement.onclick += e =>
                {
                    StopEvent(e);
                    CreateSelection(suggestion);
                };

                _suggestionsLayer.SuggestionsContent.appendChild(suggestionContainerElement);
            }

            if (!_suggestionsLayer.IsVisible)
            {
                _suggestionsLayer.Show();
            }

            PositionSuggestions();
        }

        private void ClearSuggestions()
        {
            var suggestions = _suggestionsLayer.SuggestionsContent.getElementsByClassName("tss-picker-suggestion");

            while (suggestions.length > 0)
            {
                suggestions[0].parentNode.removeChild(suggestions[0]);
            }
        }

        private void ClearOnClick(HTMLElement suggestionElement)
        {
            if (suggestionElement.onclick is object)
            {
                foreach (Delegate d in suggestionElement.onclick.GetInvocationList())
                {
                    suggestionElement.onclick -= (HTMLElement.onclickFn)d;
                }
            }
        }

        private void CreateSelection(TPickerItem selectedItem)
        {
            UpdateSelection(selectedItem, true);

            var selectionContainerElement = Div(_("tss-picker-selection"));
            var selectionComponent = selectedItem.Render();
            var removeButton = Button()
                                .Link()
                                .SetIcon(LineAwesome.Times, color: "var(--tss-default-foreground-color)")
                                .OnClick((_,__) =>
                                {
                                    UpdateSelection(selectedItem, false);
                                    selectionContainerElement.remove();
                                }).Render();

            removeButton.classList.add("tss-picker-remove");

            selectionContainerElement.appendChild(selectionComponent.Render());
            selectionContainerElement.appendChild(removeButton);

            _selectionsElement.appendChild(selectionContainerElement);

            onItemSelected?.Invoke(this, selectedItem);
        }

        private void UpdateSelection(TPickerItem selectedItem, bool isSelected)
        {
            selectedItem.IsSelected = isSelected;
            _textBox.ClearText();
            _hideSugestionsTimeout = window.setTimeout(_ =>
            {
                ClearSuggestions();
                _suggestionsLayer.Hide();
            }, 150);
        }

        private void PositionSuggestions()
        {
            _suggestionsLayer.SuggestionsContainer.classList.add("tss-layer-picker-suggestions");

            var suggestionsContentClientHeight = _suggestionsLayer.SuggestionsContent.clientHeight;
            var textBoxClientRect              = (ClientRect)_textBoxElement.getBoundingClientRect();
            var bodyClientRect                 = (ClientRect)document.body.getBoundingClientRect();

            if (suggestionsContentClientHeight + textBoxClientRect.bottom + 10 >= bodyClientRect.height)
            {
                _suggestionsLayer.SuggestionsContainer.style.top = $"{(textBoxClientRect.bottom - suggestionsContentClientHeight - textBoxClientRect.height - 10).px()}";
            }
            else
            {
                _suggestionsLayer.SuggestionsContainer.style.top = $"{(textBoxClientRect.bottom + 10).px()}";
            }

            _suggestionsLayer.SuggestionsContainer.style.left  = textBoxClientRect.left.px().ToString();
            _suggestionsLayer.SuggestionsContainer.style.width = $"{(textBoxClientRect.width / 2).px()}";
        }

        private class SuggestionsLayer : Layer<SuggestionsLayer>
        {
            private readonly HTMLElement _suggestions;

            public SuggestionsLayer(IComponent suggestions)
            {
                _suggestions = suggestions.Render();
                _contentHtml = Div(_("tss-layer-content"), _suggestions);
            }

            public Element SuggestionsContent       => _suggestions;

            public HTMLElement SuggestionsContainer => _renderedContent;
        }

        private class Suggestions : IComponent
        {
            private readonly HTMLElement _suggestions;

            public Suggestions(string suggestionsTitleText)
            {
                _suggestions = Div(_("tss-picker-suggestions"));

                if (!string.IsNullOrWhiteSpace(suggestionsTitleText))
                {
                    var suggestionsLabel = Div(_("tss-picker-label tss-fontsize-medium tss-fontweight-semibold tss-fontcolor-primary", text: suggestionsTitleText));

                    _suggestions.appendChild(suggestionsLabel);
                }
            }

            public HTMLElement Render() => _suggestions;
        }
    }
}
