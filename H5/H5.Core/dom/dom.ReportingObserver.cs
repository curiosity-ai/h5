using H5;
using H5.Core;

namespace H5.Core
{
    public static partial class dom
    {
        [CombinedClass]
        [FormerInterface]
        public class ReportingObserver : IObject
        {
            public extern ReportingObserver(dom.ReportingObserverCallback callback);

            public extern ReportingObserver(dom.ReportingObserverCallback callback, dom.ReportingObserverOptions options);

            public static dom.ReportingObserver prototype { get; set; }

            public virtual extern void observe();

            public virtual extern void disconnect();

            public virtual extern dom.Report[] takeRecords();
        }

        [CombinedClass]
        [FormerInterface]
        public class Report : IObject
        {
            public static dom.Report prototype { get; set; }

            public virtual dom.ReportBody body { get; }

            public virtual string type { get; }

            public virtual string url { get; }
        }

        [CombinedClass]
        [FormerInterface]
        public class ReportBody : IObject
        {
            public static dom.ReportBody prototype { get; set; }

            public virtual extern object toJSON();
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class ReportingObserverOptions : IObject
        {
            public string[] types { get; set; }
            public bool? buffered { get; set; }
        }

        [Generated]
        public delegate void ReportingObserverCallback(dom.Report[] reports, dom.ReportingObserver observer);

        [Virtual]
        public abstract class ReportingObserverTypeConfig : IObject
        {
             public virtual dom.ReportingObserver prototype { get; set; }

             [Template("new {this}({0})")]
             public abstract dom.ReportingObserver New(dom.ReportingObserverCallback callback);

             [Template("new {this}({0}, {1})")]
             public abstract dom.ReportingObserver New(dom.ReportingObserverCallback callback, dom.ReportingObserverOptions options);
        }
    }
}
