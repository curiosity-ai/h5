using H5;
using H5.Core;

namespace H5.Core
{
    public static partial class dom
    {
        [CombinedClass]
        [FormerInterface]
        public class VisualViewport : dom.EventTarget
        {
            public static dom.VisualViewport prototype
            {
                get;
                set;
            }

            public virtual double offsetLeft
            {
                get;
            }

            public virtual double offsetTop
            {
                get;
            }

            public virtual double pageLeft
            {
                get;
            }

            public virtual double pageTop
            {
                get;
            }

            public virtual double width
            {
                get;
            }

            public virtual double height
            {
                get;
            }

            public virtual double scale
            {
                get;
            }

            public virtual dom.VisualViewport.onresizeFn onresize
            {
                get;
                set;
            }

            public virtual dom.VisualViewport.onscrollFn onscroll
            {
                get;
                set;
            }

            [Generated]
            public delegate void onresizeFn(dom.Event ev);

            [Generated]
            public delegate void onscrollFn(dom.Event ev);
        }
    }
}
