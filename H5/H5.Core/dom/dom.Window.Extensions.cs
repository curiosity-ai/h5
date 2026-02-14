using H5;
using H5.Core;
using System;

namespace H5.Core
{
    public static partial class dom
    {
        [CombinedClass]
        [FormerInterface]
        public class IdleDeadline : IObject
        {
            public virtual bool didTimeout { get; }
            public virtual extern double timeRemaining();
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class IdleRequestOptions : IObject
        {
            public int timeout { get; set; }
        }

        [Generated]
        public delegate void IdleRequestCallback(dom.IdleDeadline deadline);

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class StructuredSerializeOptions : IObject
        {
            public object[] transfer { get; set; }
        }

        public partial class Window
        {
            public virtual string origin
            {
                get;
            }

            public virtual dom.VisualViewport visualViewport
            {
                get;
            }

            public virtual bool crossOriginIsolated
            {
                get;
            }

            public virtual extern void queueMicrotask(Action callback);

            public virtual extern void reportError(object error);

            public virtual extern uint requestIdleCallback(dom.IdleRequestCallback callback);

            public virtual extern uint requestIdleCallback(dom.IdleRequestCallback callback, dom.IdleRequestOptions options);

            public virtual extern void cancelIdleCallback(uint handle);

            public virtual extern object structuredClone(object value);

            public virtual extern object structuredClone(object value, dom.StructuredSerializeOptions options);
        }
    }
}
