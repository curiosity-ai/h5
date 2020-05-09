using System;
using static Tesserae.UI;
using static H5.Core.dom;
using Tesserae.HTML;
using System.Collections.Generic;
using System.Linq;

namespace Tesserae.Components
{
    public class VisibilitySensor : IComponent
    {
        private HTMLElement InnerElement;
        private double _debounceTimeout = 50;
        private double _debounce;
        private Action<VisibilitySensor> _onVisible;
        private int _maxCalls;

        public VisibilitySensor(Action<VisibilitySensor> onVisible, bool singleCall = true, IComponent message = null)
        {
            InnerElement = DIV();
            if(message is object)
            {
                InnerElement.appendChild(message.Render());
            }
            _onVisible = onVisible;
            _maxCalls = singleCall ? 1 : int.MaxValue;

            DomObserver.WhenMounted(InnerElement, HookCheck);
        }

        public HTMLElement Render() => InnerElement;

        public void Reset()
        {
            DomObserver.WhenMounted(InnerElement, HookCheck);
            if(_maxCalls < 1) //will only reach 0 if it was single call
            {
                _maxCalls = 1;
            }
        }
        private void HookCheck()
        {
            document.addEventListener("scroll", OnScroll, true);
            window.addEventListener("resize", OnScroll, true);
            DomObserver.WhenRemoved(InnerElement, UnHookCheck);
            //Trigger one time on first render, to force check if visible
            OnScroll(null);
        }

        private void UnHookCheck()
        {
            document.removeEventListener("scroll", OnScroll);
            window.removeEventListener("resize", OnScroll);
        }

        private void OnScroll(Event ev)
        {
            window.clearTimeout(_debounce);
            _debounce = window.setTimeout(CheckVisibility, _debounceTimeout);
        }

        private void CheckVisibility(object t)
        {
            var viewport_top = window.scrollY;
            var viewport_bottom = window.scrollY + window.innerHeight;
            var rect = (DOMRect)InnerElement.getBoundingClientRect();
            if(rect.top > viewport_top && rect.bottom < viewport_bottom)
            {
                if(_maxCalls > 0)
                {
                    _maxCalls--;
                    _onVisible(this);
                }

                if(_maxCalls == 0)
                {
                    UnHookCheck();
                }
            }
        }
    }
}
