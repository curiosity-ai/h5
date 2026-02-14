using H5;
using H5.Core;

namespace H5.Core
{
    public static partial class dom
    {
        [CombinedClass]
        [FormerInterface]
        public class HID : dom.EventTarget
        {
            public static dom.HID prototype { get; set; }

            public virtual extern es5.Promise<dom.HIDDevice[]> getDevices();

            public virtual extern es5.Promise<dom.HIDDevice[]> requestDevice(dom.HIDDeviceRequestOptions options);

            public virtual dom.HID.onconnectFn onconnect { get; set; }
            public virtual dom.HID.ondisconnectFn ondisconnect { get; set; }

            [Generated]
            public delegate void onconnectFn(dom.HIDConnectionEvent ev);
            [Generated]
            public delegate void ondisconnectFn(dom.HIDConnectionEvent ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HIDDevice : dom.EventTarget
        {
            public static dom.HIDDevice prototype { get; set; }

            public virtual bool opened { get; }
            public virtual ushort vendorId { get; }
            public virtual ushort productId { get; }
            public virtual string productName { get; }
            public virtual dom.HIDCollectionInfo[] collections { get; }

            public virtual extern es5.Promise<H5.Core.Void> open();
            public virtual extern es5.Promise<H5.Core.Void> close();
            public virtual extern es5.Promise<H5.Core.Void> sendReport(byte reportId, es5.ArrayBufferView data);
            public virtual extern es5.Promise<H5.Core.Void> sendFeatureReport(byte reportId, es5.ArrayBufferView data);
            public virtual extern es5.Promise<es5.DataView> receiveFeatureReport(byte reportId);

            public virtual dom.HIDDevice.oninputreportFn oninputreport { get; set; }

            [Generated]
            public delegate void oninputreportFn(dom.HIDInputReportEvent ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HIDConnectionEvent : dom.Event
        {
            public extern HIDConnectionEvent(string type, dom.HIDConnectionEventInit eventInitDict);
            public static dom.HIDConnectionEvent prototype { get; set; }
            public virtual dom.HIDDevice device { get; }
        }

        [CombinedClass]
        [FormerInterface]
        public class HIDInputReportEvent : dom.Event
        {
            public extern HIDInputReportEvent(string type, dom.HIDInputReportEventInit eventInitDict);
            public static dom.HIDInputReportEvent prototype { get; set; }
            public virtual dom.HIDDevice device { get; }
            public virtual byte reportId { get; }
            public virtual es5.DataView data { get; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class HIDDeviceRequestOptions : IObject
        {
            public dom.HIDDeviceFilter[] filters { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class HIDDeviceFilter : IObject
        {
            public ushort? vendorId { get; set; }
            public ushort? productId { get; set; }
            public ushort? usagePage { get; set; }
            public ushort? usage { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class HIDConnectionEventInit : dom.EventInit
        {
            public dom.HIDDevice device { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class HIDInputReportEventInit : dom.EventInit
        {
            public dom.HIDDevice device { get; set; }
            public byte reportId { get; set; }
            public es5.DataView data { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class HIDCollectionInfo : IObject
        {
            public ushort? usagePage { get; set; }
            public ushort? usage { get; set; }
            public dom.HIDCollectionInfo[] children { get; set; }
        }

        [Virtual]
        public abstract class HIDTypeConfig : IObject
        {
             public virtual dom.HID prototype { get; set; }
        }
    }
}
