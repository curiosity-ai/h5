using HighFive;
using System;
using System.Collections.Generic;
using Tesserae.Components;
using static H5.Core.dom;

namespace Tesserae.HTML
{
    public class ResizeObserver
    {
        public Action<Event> OnResizeElement { get; set; }
        public Action OnResize { get; set; }
        private object resizeObserver;
        private List<Action> pending;
        public ResizeObserver()
        {
            try
            {
                CreateRO();
            }
            catch
            {
                pending = new List<Action>();
                pending.Add(CreateRO);
                Require.LoadScriptAsync("./assets/js/resizeobserver.js").ContinueWith(t =>
                {
                    if (t.IsCompleted)
                    {
                        var p = pending;
                        pending = null;
                        foreach (var a in p)
                        {
                            a();
                        }
                    }
                }).FireAndForget();
            }
        }

        private void CreateRO()
        {
            Action<Event[]> resize = DoResize;
            resizeObserver = Script.Write<object>("new ResizeObserver(entries => {0}(entries));", resize);
        }

        public void Observe(HTMLElement element)
        {
            if (pending is null)
            {
                Script.Write("{0}.observe(element)", resizeObserver);
            }
            else
            {
                pending.Add(() => Observe(element));
            }
        }

        public void Unobserve(HTMLElement element)
        {
            if (pending is null)
            {
                Script.Write("{0}.unobserve(element)", resizeObserver);
            }
            else
            {
                pending.Add(() => Unobserve(element));
            }
        }

        public void Disconnect()
        {
            if (pending is null)
            {
                Script.Write("{0}.disconnect()", resizeObserver);
            }
            else
            {
                pending.Add(Disconnect);
            }
        }

        private void DoResize(Event[] entries)
        {
            if (OnResizeElement != null)
            {
                for (int i = 0; i < entries.Length; i++)
                {
                    Event entry = entries[i];
                    OnResizeElement(entry);
                }
            }

            OnResize?.Invoke();
        }

        public float GetHeight(HTMLElement element)
        {
            var height = window.getComputedStyle(element).height;
            if (height == "")
            {
                // 2019-10-04 DWR: I've seen height be returned as a blank string, which will fail at float.parse, so return zero instead
                return 0;
            }
            return float.Parse(height.Replace("px", ""));
        }

        public float GetWidth(HTMLElement element)
        {
            var width = window.getComputedStyle(element).width;
            if (width == "")
            {
                // 2019-10-04 DWR: I presume that if height can be blank (see GetHeight) then width can be too, so include the same safety check
                return 0;
            }
            return float.Parse(width.Replace("px", ""));
        }
    }
}
