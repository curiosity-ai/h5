using H5;
using H5.Core;

namespace H5.Core
{
    public static partial class dom
    {
        [CombinedClass]
        [FormerInterface]
        public class PerformanceObserver : IObject
        {
            public extern PerformanceObserver(dom.PerformanceObserverCallback callback);

            public static dom.PerformanceObserver prototype { get; set; }

            public static es5.ReadonlyArray<string> supportedEntryTypes { get; }

            public virtual extern void observe(dom.PerformanceObserverInit options);

            public virtual extern void disconnect();

            public virtual extern dom.PerformanceEntry[] takeRecords();
        }

        [CombinedClass]
        [FormerInterface]
        public class PerformanceObserverEntryList : IObject
        {
            public static dom.PerformanceObserverEntryList prototype { get; set; }

            public virtual extern dom.PerformanceEntry[] getEntries();

            public virtual extern dom.PerformanceEntry[] getEntriesByType(string type);

            public virtual extern dom.PerformanceEntry[] getEntriesByName(string name);

            public virtual extern dom.PerformanceEntry[] getEntriesByName(string name, string type);
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class PerformanceObserverInit : IObject
        {
            public string[] entryTypes { get; set; }
            public string type { get; set; }
            public bool? buffered { get; set; }
        }

        [Generated]
        public delegate void PerformanceObserverCallback(dom.PerformanceObserverEntryList list, dom.PerformanceObserver observer);

        [Virtual]
        public abstract class PerformanceObserverTypeConfig : IObject
        {
             public virtual dom.PerformanceObserver prototype { get; set; }

             [Template("new {this}({0})")]
             public abstract dom.PerformanceObserver New(dom.PerformanceObserverCallback callback);
        }
    }
}
