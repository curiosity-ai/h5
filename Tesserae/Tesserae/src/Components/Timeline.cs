using System;
using Tesserae.HTML;
using static H5.Core.dom;
using static Tesserae.UI;

namespace Tesserae.Components
{
    public class Timeline : IContainer<Timeline, IComponent>, IHasBackgroundColor, IHasMarginPadding
    {
        private readonly HTMLElement _timeline;
        private readonly HTMLElement _timelineOwner;

        public string Background { get => _timelineOwner.style.background; set => _timelineOwner.style.background = value; }
        public string Margin { get => _timelineOwner.style.margin; set => _timelineOwner.style.margin = value; }
        public string Padding { get => _timelineOwner.style.padding; set => _timelineOwner.style.padding = value; }
        public bool IsSameSide
        {
            get => _timelineOwner.classList.contains("tss-left");
            set => _timelineOwner.UpdateClassIf(value, "tss-left");
        }

        private bool left = true;
        public Timeline()
        {
            _timeline = Div(_("tss-timeline"));
            _timelineOwner = Div(_("tss-timeline-owner"), _timeline);
        }

        public void Add(IComponent component)
        {
            ScrollBar.GetCorrectContainer(_timeline).appendChild(Wrap(component));
            Rebase(false);
        }

        public Timeline SameSide()
        {
            IsSameSide = true;
            return this;
        }

        public Timeline SameSideIf(int minWidthPixels)
        {
            void Recompute()
            {
                var rect = (DOMRect)_timelineOwner.getBoundingClientRect();
                IsSameSide = rect.width <= minWidthPixels;
            }

            DomObserver.WhenMounted(_timelineOwner, () =>
            {
                var ro = new ResizeObserver();
                ro.Observe(document.body);
                ro.OnResize = Recompute;
                DomObserver.WhenRemoved(_timelineOwner, () =>
                {
                    ro.Unobserve(document.body);
                });
            });

            return this;
        }

        public Timeline TimelineWidth(UnitSize maxWidth)
        {
            _timeline.style.maxWidth = maxWidth.ToString();
            return this;
        }

        private void Rebase(bool rebaseAll)
        {
            var parent = ScrollBar.GetCorrectContainer(_timeline);

            if (rebaseAll)
            {
                left = true;
                foreach (var n in parent.children)
                {
                    n.classList.remove("tss-left", "tss-right");
                    n.classList.add(left ? "tss-left" : "tss-right");
                    left = !left;
                }
            }
            else
            {
                //just do the final one
                ScrollBar.GetCorrectContainer(_timeline).lastElementChild.classList.add(left ? "tss-left" : "tss-right");
                left = !left;
            }
        }

        private HTMLElement Wrap(IComponent component)
        {
            return Div(_("tss-timeline-container"), Div(_("tss-timeline-content"), component.Render()));
        }

        public void Clear()
        {
            ClearChildren(_timeline);
        }

        public HTMLElement Render() => _timelineOwner;

        public void Replace(IComponent newComponent, IComponent oldComponent)
        {
            ScrollBar.GetCorrectContainer(_timeline).replaceChild(Wrap(newComponent), Wrap(oldComponent));
            Rebase(true);
        }
    }
}
