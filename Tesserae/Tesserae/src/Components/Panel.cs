using System;
using static H5.dom;
using static Tesserae.UI;

namespace Tesserae.Components
{
    public class Panel : Layer<Panel>
    {
        private IComponent _footer;
        private readonly HTMLElement _panel;
        private readonly HTMLElement _panelOverlay;
        private readonly HTMLElement _panelContent;
        private readonly HTMLElement _panelFooter;
        private readonly HTMLElement _panelCommand;
        private readonly HTMLElement _closeButton;

        public delegate void OnHideHandler(Panel sender);

        public event OnHideHandler onHide;

        public Panel() : base()
        {
            _closeButton = Button(_("las la-times", el: el => el.onclick = (e) => Hide()));
            _panelCommand = Div(_("tss-panel-command"), _closeButton);
            _panelContent = Div(_("tss-panel-content"));
            _panelFooter = Div(_("tss-panel-footer"));
            _panel = Div(_("tss-panel tss-panelSize-small tss-panelSide-far"), _panelCommand, Div(_("tss-panel-inner"), _panelContent, _panelFooter));
            _panelOverlay = Div(_("tss-panel-overlay"));
            _contentHtml = Div(_("tss-panel-container"), _panelOverlay, _panel);
        }

        public override IComponent Content
        {
            get => _content;
            set
            {
                ClearChildren(_panelContent); ;
                _content = value;
                if (_content != null)
                {
                    _panelContent.appendChild(_content.Render());
                }
            }
        }

        public IComponent Footer
        {
            get => _footer;
            set
            {
                ClearChildren(_panelFooter); ;
                _footer = value;
                if (_footer != null)
                {
                    _panelFooter.appendChild(_footer.Render());
                }
            }
        }

        public PanelSize Size
        {
            get
            {
                if (Enum.TryParse<PanelSize>(_panel.classList[1].Substring(_panel.classList[1].LastIndexOf('-') + 1), true, out var result))
                {
                    return result;
                }

                return PanelSize.Small;
            }
            set => _panel.classList.replace(_panel.classList[1], $"tss-panelSize-{value.ToString().ToLower()}");
        }

        public PanelSide Side
        {
            get
            {
                if (Enum.TryParse<PanelSide>(_panel.classList[2].Substring(_panel.classList[2].LastIndexOf('-') + 1), true, out var result))
                {
                    return result;
                }

                return PanelSide.Far;
            }
            set => _panel.classList.replace(_panel.classList[2], $"tss-panelSide-{value.ToString().ToLower()}");
        }

        public bool CanLightDismiss
        {
            get => _panelOverlay.classList.contains("tss-panel-lightDismiss");
            set
            {
                if (value)
                {
                    _panelOverlay.classList.add("tss-panel-lightDismiss");
                    _panelOverlay.addEventListener("click", OnCloseClick);
                }
                else
                {
                    _panelOverlay.classList.remove("tss-panel-lightDismiss");
                    _panelOverlay.removeEventListener("click", OnCloseClick);
                }
            }
        }

        public bool IsDark
        {
            get => _contentHtml.classList.contains("tss-dark");
            set
            {
                if (value)
                {
                    _contentHtml.classList.add("tss-dark");
                }
                else
                {
                    _contentHtml.classList.remove("tss-dark");
                }
            }
        }

        public bool IsNonBlocking
        {
            get => _contentHtml.classList.contains("tss-panel-modeless");
            set
            {
                if (value)
                {
                    _contentHtml.classList.add("tss-panel-modeless");
                }
                else
                {
                    _contentHtml.classList.remove("tss-panel-modeless");
                }
            }
        }

        public bool ShowCloseButton
        {
            get => _closeButton.style.display != "none";
            set
            {
                if (value) _closeButton.style.display = "";
                else _closeButton.style.display = "none";
            }

        }

        protected override HTMLElement BuildRenderedContent()
        {
            return _contentHtml;
        }

        public override void Show()
        {
            if (!IsNonBlocking) document.body.style.overflowY = "hidden";
            base.Show();
        }

        public Panel OnHide(OnHideHandler onHide)
        {
            this.onHide += onHide;
            return this;
        }

        public override void Hide(Action onHidden = null)
        {
            onHide?.Invoke(this);

            base.Hide(() =>
            {
                if (!IsNonBlocking) document.body.style.overflowY = "";
                onHidden?.Invoke();
            });
        }
        private void OnCloseClick(object ev)
        {
            Hide();
        }

        public enum PanelSize
        {
            Small,
            Medium,
            Large,
            LargeFixed,
            ExtraLarge,
            FullWidth
        }

        public enum PanelSide
        {
            Far,
            Near
        }

        public Panel HideCloseButton()
        {
            ShowCloseButton = false;
            return this;
        }

        public Panel SetFooter(IComponent footer)
        {
            Footer = footer;
            return this;
        }

        public Panel Small()
        {
            Size = PanelSize.Small;
            return this;
        }
        public Panel Medium()
        {
            Size = PanelSize.Medium;
            return this;
        }
        public Panel Large()
        {
            Size = PanelSize.Large;
            return this;
        }
        public Panel LargeFixed()
        {
            Size = PanelSize.LargeFixed;
            return this;
        }
        public Panel ExtraLarge()
        {
            Size = PanelSize.ExtraLarge;
            return this;
        }
        public Panel FullWidth()
        {
            Size = PanelSize.FullWidth;
            return this;
        }

        public Panel Far()
        {
            Side = PanelSide.Far;
            return this;
        }

        public Panel Near()
        {
            Side = PanelSide.Near;
            return this;
        }

        public Panel LightDismiss()
        {
            CanLightDismiss = true;
            return this;
        }

        public Panel NoLightDismiss()
        {
            CanLightDismiss = false;
            return this;
        }

        public Panel Dark()
        {
            IsDark = true;
            return this;
        }

        public Panel NonBlocking()
        {
            IsNonBlocking = true;
            return this;
        }
        public Panel Blocking()
        {
            IsNonBlocking = false;
            return this;
        }
    }
}
