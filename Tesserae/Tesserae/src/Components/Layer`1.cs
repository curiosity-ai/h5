using System;
using static H5.dom;
using static Tesserae.UI;

namespace Tesserae.Components
{
    // This generic version of Layer should only be used to create derived classes from, so that we know what the return type of methods such as SetContent should be. If you really want JUST a Layer, with no custom behaviour, use the non-generic class (which is derived from this)
    public abstract class Layer<T> : ComponentBase<T, HTMLDivElement> where T : Layer<T>
    {
        protected IComponent _content;
        protected HTMLDivElement _contentHtml;
        protected HTMLElement _renderedContent;
        private LayerHost _host;
        private bool _isVisible;
        public Layer()
        {
            InnerElement = Div(_("tss-layer-base"));
        }

        public LayerHost Host
        {
            get => _host;
            set
            {
                if (IsVisible) Hide();
                _host = value;
                if (IsVisible) Show();
            }
        }

        public virtual IComponent Content
        {
            get => _content;
            set
            {
                _content = value;
                if (IsVisible)
                {
                    Hide();
                    Show();
                }
            }
        }

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (value) Show();
                else Hide();
            }
        }

        public override HTMLElement Render() => InnerElement;

        public virtual void Show()
        {
            if (_content is object || _contentHtml is object)
            {
                if (_host is null)
                {
                    _renderedContent = Div(_("tss-layer tss-fade"), BuildRenderedContent());
                    _renderedContent.style.zIndex = Layers.PushLayer(_renderedContent);
                    document.body.appendChild(_renderedContent);
                    window.requestAnimationFrame((_) => _renderedContent?.classList.add("tss-show"));
                }
                else
                {
                    _renderedContent = BuildRenderedContent();
                    _host.InnerElement.appendChild(_renderedContent);
                }

                _isVisible = true;
            }
        }

        public virtual void Hide(Action onHidden = null)
        {
            if (_renderedContent is object)
            {
                if (_host == null)
                {
                    Layers.PopLayer(_renderedContent);
                    _renderedContent.classList.remove("tss-show");
                    var tr = _renderedContent;
                    window.setTimeout((_) => { document.body.removeChild(tr); onHidden?.Invoke(); }, 150);
                }
                else
                {
                    _host.InnerElement.removeChild(_renderedContent);
                }
                _renderedContent = null;
                _isVisible = false;
            }
        }

        protected virtual HTMLElement BuildRenderedContent()
        {
            if (_contentHtml is object) return _contentHtml;
            return Div(_("tss-layer-content"), _content.Render());
        }
    }
}