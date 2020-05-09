using static Tesserae.UI;
using static H5.Core.dom;
using System.Linq;
using System;
using Tesserae.HTML;
using System.Threading.Tasks;

namespace Tesserae.Components
{
    public class Button : ComponentBase<Button, HTMLButtonElement>, IHasTextSize, IHasBackgroundColor, IHasForegroundColor
    {
        private readonly HTMLSpanElement _textSpan;
        private HTMLElement _iconSpan;
        private HTMLElement _beforeReplace;

        public Button(string text = string.Empty)
        {
            _textSpan    = Span(_(text: text));
            InnerElement = Button(_("tss-btn tss-btn-default"), _textSpan);
            Weight       = TextWeight.SemiBold;
            Size         = TextSize.Small;

            AttachClick();
            AttachFocus();
            AttachBlur();

            if (string.IsNullOrEmpty(text))
            {
                InnerElement.style.minWidth = "unset";
            }
        }

        public string Background
        {
            get => InnerElement.style.background;
            set => InnerElement.style.background = value;
        }

        public string Foreground
        {
            get => InnerElement.style.color;
            set => InnerElement.style.color = value;
        }

        /// <summary>
        /// Gets or sets button text
        /// </summary>
        public string Text
        {
            get => _textSpan.innerText;
            set
            {
                _textSpan.innerText         = value;
                InnerElement.style.minWidth = string.IsNullOrEmpty(value) ? "unset" : string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets button title
        /// </summary>
        public string Title
        {
            get => InnerElement.title;
            set => InnerElement.title = value;
        }

        /// <summary>
        /// Gets or sets button icon (icon class)
        /// </summary>
        public string Icon
        {
            get => _iconSpan?.className;
            set
            {
                if (string.IsNullOrEmpty(value) && _iconSpan != null)
                {
                    InnerElement.removeChild(_iconSpan);
                    _iconSpan = null;

                    return;
                }

                if (_iconSpan == null)
                {
                    _iconSpan = I(_());
                    InnerElement.insertBefore(_iconSpan, _textSpan);
                }

                _iconSpan.className = value;
            }
        }

        /// <summary>
        /// Gets or set whenever button is rendered in a compact form
        /// </summary>
        public bool IsCompact
        {
            get => InnerElement.classList.contains("tss-small");
            set => InnerElement.UpdateClassIf(value, "tss-small");
        }

        /// <summary>
        /// Gets or set whenever button is rendered like a link
        /// </summary>
        public bool IsLink
        {
            get => InnerElement.classList.contains("tss-btn-link");
            set => InnerElement.UpdateClassIf(value, "tss-btn-link");
        }

        /// <summary>
        /// Gets or set whenever button is primary
        /// </summary>
        public bool IsPrimary
        {
            get => InnerElement.classList.contains("tss-btn-primary");
            set
            {
                if (value)
                {
                    InnerElement.classList.add("tss-btn-primary");
                    InnerElement.classList.remove("tss-btn-default", "tss-btn-success", "tss-btn-danger");
                }
                else
                {
                    InnerElement.classList.add("tss-btn-default");
                    InnerElement.classList.remove("tss-btn-default", "tss-btn-success", "tss-btn-danger", "tss-btn-primary");
                }
            }
        }

        /// <summary>
        /// Gets or set whenever button is primary
        /// </summary>
        public bool IsSuccess
        {
            get => InnerElement.classList.contains("tss-btn-success");
            set
            {
                if (value)
                {
                    InnerElement.classList.add("tss-btn-success");
                    InnerElement.classList.remove("tss-btn-default", "tss-btn-primary", "tss-btn-danger");
                }
                else
                {
                    InnerElement.classList.add("tss-btn-default");
                    InnerElement.classList.remove("tss-btn-default", "tss-btn-success", "tss-btn-danger", "tss-btn-primary");
                }
            }
        }

        /// <summary>
        /// Gets or set whenever button is danger
        /// </summary>
        public bool IsDanger
        {
            get => InnerElement.classList.contains("tss-btn-danger");
            set
            {
                if (value)
                {
                    InnerElement.classList.add("tss-btn-danger");
                    InnerElement.classList.remove("tss-btn-default", "tss-btn-primary", "tss-btn-success");
                }
                else
                {
                    InnerElement.classList.add("tss-btn-default");
                    InnerElement.classList.remove("tss-btn-default", "tss-btn-success", "tss-btn-danger", "tss-btn-primary");
                }
            }
        }

        /// <summary>
        /// Gets or sets whenever button is enabled
        /// </summary>
        public bool IsEnabled
        {
            get => !InnerElement.classList.contains("tss-disabled");
            set => InnerElement.UpdateClassIfNot(value, "tss-disabled");
        }

        public bool CanWrap
        {
            get => !InnerElement.classList.contains("tss-btn-nowrap");
            set => InnerElement.UpdateClassIfNot(value, "tss-btn-nowrap");
        }

        public bool EnableEllipsis
        {
            get => !InnerElement.classList.contains("tss-text-ellipsis");
            set => InnerElement.UpdateClassIf(value, "tss-text-ellipsis");
        }

        public TextSize Size
        {
            get => InnerElement.GetTextSize().textSize ?? TextSize.Small;
            set
            {
                var (textSize, textSizeCssClass) = InnerElement.GetTextSize();

                InnerElement.RemoveClassIf(textSize.HasValue, textSizeCssClass);

                InnerElement.classList.add($"tss-fontsize-{value.ToString().ToLower()}");
            }
        }

        public TextWeight Weight
        {
            get => InnerElement.GetTextWeight().textWeight ?? TextWeight.Regular;
            set
            {
                var (textWeight, textWeightCssClass) = InnerElement.GetTextWeight();

                InnerElement.RemoveClassIf(textWeight.HasValue, textWeightCssClass);

                InnerElement.classList.add($"tss-fontweight-{value.ToString().ToLower()}");
            }
        }

        public TextAlign TextAlign
        {
            get => InnerElement.GetTextAlign().textAlign ?? TextAlign.Center;
            set
            {
                var (textAlign, textAlignCssClass) = InnerElement.GetTextAlign();

                InnerElement.RemoveClassIf(textAlign.HasValue, textAlignCssClass);

                InnerElement.classList.add($"tss-textalign-{value.ToString().ToLower()}");
            }
        }

        public override HTMLElement Render()
        {
            return InnerElement;
        }


        public Button Compact()
        {
            IsCompact = true;
            return this;
        }

        public Button Link()
        {
            IsLink = true;
            return this;
        }

        public Button DefaultLink()
        {
            IsLink = true;
            InnerElement.classList.add("tss-dark");
            return this;
        }

        public Button DangerLink()
        {
            IsLink = true;
            InnerElement.classList.add("tss-danger");
            return this;
        }

        public void ToSpinner(string text = null)
        {
            if (_beforeReplace is null)
            {
                _beforeReplace = (HTMLElement)InnerElement.cloneNode(true);
                var rect = (DOMRect)InnerElement.getBoundingClientRect();
                ClearChildren(InnerElement);
                InnerElement.appendChild(Spinner(text).Medium().Render());
                InnerElement.style.minHeight = rect.height.px().ToString();
                IsEnabled = false;
                InnerElement.classList.add("tss-btn-nominsize");
            }
        }

        public void UndoSpinner()
        {
            if (_beforeReplace is object)
            {
                ClearChildren(InnerElement);
                foreach(var el in _beforeReplace.children) { InnerElement.appendChild(el); }
                IsEnabled = true;
                InnerElement.classList.remove("tss-btn-nominsize");
                _beforeReplace = null;
            }
        }

        public void SpinWhile(Func<Task> action, string text = null)
        {
            Task.Run(async () =>
            {
                ToSpinner(text);
                await action();
                UndoSpinner();
            }).FireAndForget();
        }

        public Button Primary()
        {
            IsPrimary = true;
            return this;
        }

        public Button Success()
        {
            IsSuccess = true;
            return this;
        }

        public Button Danger()
        {
            IsDanger = true;
            return this;
        }

        public Button Disabled(bool value = true)
        {
            IsEnabled = !value;
            return this;
        }

        public Button NoBorder()
        {
            InnerElement.classList.add("tss-btn-noborder");
            return this;
        }
        public Button NoMinSize()
        {
            InnerElement.classList.add("tss-btn-nominsize");
            return this;
        }

        public Button NoBackground()
        {
            InnerElement.classList.add("tss-btn-nobg");
            return this;
        }

        public Button LinkOnHover()
        {
            InnerElement.classList.add("tss-btn-linkonhover");
            return this;
        }

        public Button Color(string background, string textColor = "white", string borderColor = "white", string iconColor = "")
        {
            InnerElement.classList.add("tss-btn-nobg");
            InnerElement.style.background = background;
            InnerElement.style.color = textColor;
            InnerElement.style.borderColor = borderColor;
            if(_iconSpan is object)
            {
                _iconSpan.style.color = iconColor;
            }
            return this;
        }

        public Button SetText(string text)
        {
            Text = text;
            return this;
        }

        public Button SetTitle(string title)
        {
            Title = title;
            return this;
        }

        public Button SetIcon(string icon, string color = "")
        {
            Icon = icon;
            if (_iconSpan is object)
            {
                _iconSpan.style.color = color;
            }
            return this;
        }

        public Button SetIcon(LineAwesome icon, TextSize size = TextSize.Medium, LineAwesomeWeight weight = LineAwesomeWeight.Light , string color = "")
        {
            Icon = $"{weight} {icon} tss-fontsize-{size.ToString().ToLower()}";
            if (_iconSpan is object)
            {
                _iconSpan.style.color = color;
            }
            return this;
        }

        public Button ReplaceContent(IComponent content)
        {
            ClearChildren(InnerElement);
            InnerElement.appendChild(content.Render());
            return this;
        }

        public Button Wrap()
        {
            CanWrap = true;
            return this;
        }

        public Button Ellipsis()
        {
            EnableEllipsis = true;
            if (string.IsNullOrEmpty(Title))
            {
                Title = Text;
            }
            return this;
        }

        public Button NoWrap()
        {
            CanWrap = false;
            return this;
        }

        private void RaiseOnClick(Event e , Hotkeys.Handler handler)
        {
            if (IsEnabled)
            {
                base.RaiseOnClick(this);
            }
        }

        public Button Focus()
        {
            //When mounted triggers immediatelly if already mounted
            DomObserver.WhenMounted(InnerElement, () => window.setTimeout((_) => InnerElement.focus(), 500));
            return this;
        }

        public Button WithHotKey(string keys, Hotkeys.Option options = null)
        {
            DomObserver.WhenMounted(InnerElement, () =>
            {
                if(options is null)
                {
                    Hotkeys.BindGlobal(keys, RaiseOnClick);
                }
                else
                {
                    Hotkeys.Bind(keys, options, RaiseOnClick);
                }

                DomObserver.WhenRemoved(InnerElement, () =>
                {
                    if (options is null)
                    {
                        Hotkeys.UnbindGlobal(keys, RaiseOnClick);
                    }
                    else
                    {
                        Hotkeys.Unbind(keys, options, RaiseOnClick);
                    }
                });
            });
            return this;
        }
    }
}
