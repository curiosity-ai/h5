using H5;
using H5.Core;

namespace H5.Core
{
    public static partial class dom
    {
        [CombinedClass]
        [FormerInterface]
        public class ResizeObserver : IObject
        {
            public extern ResizeObserver(dom.ResizeObserverCallback callback);

            public static dom.ResizeObserver prototype { get; set; }

            public virtual extern void disconnect();

            public virtual extern void observe(dom.Element target);

            public virtual extern void observe(dom.Element target, dom.ResizeObserverOptions options);

            public virtual extern void unobserve(dom.Element target);
        }

        [CombinedClass]
        [FormerInterface]
        public class ResizeObserverEntry : IObject
        {
            public static dom.ResizeObserverEntry prototype { get; set; }

            public virtual dom.Element target { get; }

            public virtual dom.DOMRectReadOnly contentRect { get; }

            public virtual dom.ResizeObserverSize[] borderBoxSize { get; }

            public virtual dom.ResizeObserverSize[] contentBoxSize { get; }

            public virtual dom.ResizeObserverSize[] devicePixelContentBoxSize { get; }
        }

        [CombinedClass]
        [FormerInterface]
        public class ResizeObserverSize : IObject
        {
            public static dom.ResizeObserverSize prototype { get; set; }

            public virtual double inlineSize { get; }

            public virtual double blockSize { get; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class ResizeObserverOptions : IObject
        {
            public Union<string, dom.ResizeObserverBoxOptions> box { get; set; }
        }

        [Generated]
        public delegate void ResizeObserverCallback(dom.ResizeObserverEntry[] entries, dom.ResizeObserver observer);

        [Virtual]
        public abstract class ResizeObserverTypeConfig : IObject
        {
             public virtual dom.ResizeObserver prototype { get; set; }

             [Template("new {this}({0})")]
             public abstract dom.ResizeObserver New(dom.ResizeObserverCallback callback);
        }

        [Name("System.String")]
        public class ResizeObserverBoxOptions : LiteralType<string>
        {
            [Template("<self>\"content-box\"")]
            public static readonly dom.ResizeObserverBoxOptions contentBox;

            [Template("<self>\"border-box\"")]
            public static readonly dom.ResizeObserverBoxOptions borderBox;

            [Template("<self>\"device-pixel-content-box\"")]
            public static readonly dom.ResizeObserverBoxOptions devicePixelContentBox;

            private extern ResizeObserverBoxOptions();

            public static extern implicit operator dom.ResizeObserverBoxOptions(string value);
        }
    }
}
