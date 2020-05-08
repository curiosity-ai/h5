using System;
using System.Linq;
using static H5.dom;
using static Tesserae.UI;

namespace Tesserae.Components
{
    public class EditableArea : ComponentBase<EditableArea, HTMLTextAreaElement>, IHasTextSize, IObservableComponent<string>
    {
        protected readonly HTMLDivElement _container;

        protected readonly HTMLSpanElement _labelText;

        protected          HTMLElement    _editIcon;
        protected          HTMLElement    _cancelEditIcon;
        protected readonly HTMLDivElement _editView;
        protected readonly HTMLDivElement _labelView;

        private readonly SettableObservable<string> _observable = new SettableObservable<string>();

        public delegate bool SaveEditHandler(EditableArea sender, string newValue);

        public event SaveEditHandler onSave;

        private bool _isCanceling = false;

        public TextSize Size
        {
            get => TextSizeExtensions.FromClassList(InnerElement, TextSize.Small);
            set
            {
                string current = Size.ToClassName();
                InnerElement.classList.remove(current);
                _labelText.classList.remove(current);
                _editIcon.classList.remove(current);
                _cancelEditIcon.classList.remove(current);

                string newValue = value.ToClassName();
                InnerElement.classList.add(newValue);
                _labelText.classList.add(newValue);
                _editIcon.classList.add(newValue);
                _cancelEditIcon.classList.add(newValue);
            }
        }

        public TextWeight Weight
        {
            get => TextSizeExtensions.FromClassList(InnerElement, TextWeight.Regular);
            set
            {
                InnerElement.classList.remove(Weight.ToClassName());
                _labelText.classList.remove(Weight.ToClassName());
                InnerElement.classList.add(value.ToClassName());
                _labelText.classList.add(value.ToClassName());
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

        public EditableArea(string text = string.Empty)
        {
            _labelText  = Span(_("tss-editablelabel-textspan", text: text, title: "Click to edit"));
            _editIcon   = I(_("tss-editablelabel-edit-icon las la-edit"));
            _labelView  = Div(_("tss-editablelabel-displaybox"), _labelText, _editIcon);

            InnerElement     = TextArea(_("tss-editablelabel-textbox", type: "text"));
            _cancelEditIcon  = Div(_("tss-editablelabel-cancel-icon", title:"Cancel edit"), I(_("las la-times")));
            _editView        = Div(_("tss-editablelabel-editbox"), InnerElement, _cancelEditIcon);

            _container = Div(_("tss-editablelabel"), _labelView, _editView);

            AttachChange();
            AttachInput();
            AttachFocus();
            AttachBlur();
            AttachKeys();

            _labelView.addEventListener("click",      BeginEditing);
            _cancelEditIcon.addEventListener("click", CancelEditing);

            OnKeyUp(KeyUp);
            OnBlur(BeginSaveEditing);
        }

        private void KeyUp(EditableArea sender, KeyboardEvent e)
        {
            if(e.key == "Escape")
            {
                CancelEditing(sender);
            }
        }

        public bool IsEditingMode
        {
            get => _container.classList.contains("tss-editing");
            set
            {
                if (value)
                {
                    var labelRect = (DOMRect)_labelText.getBoundingClientRect();
                    InnerElement.style.minWidth = (labelRect.width * 1.2) + "px";
                    InnerElement.style.minHeight = (labelRect.height * 1.2) + "px";
                    _container.classList.add("tss-editing");
                }
                else
                {
                    _container.classList.remove("tss-editing");
                }
            }
        }

        public EditableArea OnSave(SaveEditHandler onSave)
        {
            this.onSave += onSave;
            return this;
        }

        protected void BeginEditing(object sender)
        {
            InnerElement.value = _labelText.textContent;
            IsEditingMode = true;
            _isCanceling = false;
            InnerElement.focus();
        }

        protected void CancelEditing(object sender)
        {
            _isCanceling = true;
            IsEditingMode = false;
            InnerElement.blur();
        }

        private void BeginSaveEditing(EditableArea sender, Event e)
        {
            //We need to do this on a timeout, because clicking on the Cancel would trigger this method first,
            //with no opportunity to cancel
            window.setTimeout(SaveEditing, 150);
        }

        private void SaveEditing(object e)
        {
            if (_isCanceling) return;

            var newValue = InnerElement.value;

            if (newValue != _labelText.textContent)
            {
                if (onSave is null || onSave(this, newValue))
                {
                    _labelText.textContent = newValue;
                    _observable.Value = newValue;
                    IsEditingMode = false;
                }
                else
                {
                    InnerElement.focus();
                }
            }
        }

        public EditableArea  SetText(string text)
        {
            if (IsEditingMode)
            {
                InnerElement.value = text;
            }
            else
            {
                _labelText.textContent = text;
            }

            _observable.Value = text;

            return this;
        }

        public override HTMLElement Render()
        {
            return _container;
        }

        public IObservable<string> AsObservable()
        {
            return _observable;
        }
    }
}
