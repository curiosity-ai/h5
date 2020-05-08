using System;
using Tesserae.HTML;
using static H5.dom;
using static Tesserae.UI;
namespace Tesserae.Components
{
    public class TextBox : ComponentBase<TextBox, HTMLInputElement>, ICanValidate<TextBox>, IObservableComponent<string>
    {
        private readonly HTMLDivElement _container;
        private readonly HTMLSpanElement _errorSpan;
        private readonly SettableObservable<string> _observable = new SettableObservable<string>();

        public TextBox(string text = string.Empty)
        {
            InnerElement = TextBox(_("tss-textbox", type: "text", value: text));
            _errorSpan = Span(_("tss-textbox-error"));
            _container = Div(_("tss-textbox-container"), InnerElement, _errorSpan);
            AttachChange();
            AttachInput();
            AttachFocus();
            AttachBlur();

            OnChange((_, __) => _observable.Value = Text);

            OnInput((_, __) => _observable.Value = Text);
        }

        public bool IsEnabled
        {
            get => !InnerElement.classList.contains("tss-disabled");
            set
            {
                if (value)
                {
                    InnerElement.classList.remove("tss-disabled");
                }
                else
                {
                    InnerElement.classList.add("tss-disabled");
                }
            }
        }

        public bool IsReadOnly
        {
            get => InnerElement.hasAttribute("readonly");
            set
            {
                if (value) InnerElement.setAttribute("readonly", "");
                else InnerElement.removeAttribute("readonly");
            }
        }

        public bool IsPassword
        {
            get => InnerElement.type == "password";
            set
            {
                if (value) InnerElement.type = "password";
                else InnerElement.type = "";
            }
        }


        public string Text
        {
            get => InnerElement.value;
            set
            {
                InnerElement.value = value;
                _observable.Value = value;
                RaiseOnInput(null);
            }
        }

        public string Placeholder
        {
            get => InnerElement.placeholder;
            set => InnerElement.placeholder = value;
        }

        public string Error
        {
            get => _errorSpan.innerText;
            set => _errorSpan.innerText = value;
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

        public override HTMLElement Render() => _container;

        public void Attach(EventHandler<Event> handler, Validation.Mode mode)
        {
            if (mode == Validation.Mode.OnBlur)
            {
                onChange += (s,e) => handler(s,e);
            }
            else
            {
                onInput += (s, e) => handler(s, e);
            }
        }

        public TextBox SetText(string text)
        {
            Text = text;
            return this;
        }

        public TextBox ClearText()
        {
            SetText(string.Empty);
            return this;
        }

        public TextBox SetPlaceholder(string placeholder)
        {
            Placeholder = placeholder;
            return this;
        }

        public TextBox Disabled(bool value = true)
        {
            IsEnabled = !value;
            return this;
        }

        public TextBox ReadOnly()
        {
            IsReadOnly = true;
            return this;
        }

        public TextBox Password()
        {
            IsPassword = true;
            return this;
        }

        public TextBox Required()
        {
            IsRequired = true;
            return this;
        }
        public TextBox Focus()
        {
            //When mounted triggers immediatelly if already mounted
            DomObserver.WhenMounted(InnerElement, () => window.setTimeout((_) => InnerElement.focus(), 500));
            return this;
        }

        public IObservable<string> AsObservable() => _observable;
    }
}