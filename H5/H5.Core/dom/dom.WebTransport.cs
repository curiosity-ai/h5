using H5;
using H5.Core;

namespace H5.Core
{
    public static partial class dom
    {
        [CombinedClass]
        [FormerInterface]
        public class WebTransport : IObject
        {
            public extern WebTransport(string url);
            public extern WebTransport(string url, dom.WebTransportOptions options);
            public static dom.WebTransport prototype { get; set; }

            public virtual es5.Promise<H5.Core.Void> ready { get; }
            public virtual es5.Promise<dom.WebTransportCloseInfo> closed { get; }
            public virtual dom.WebTransportDatagramDuplexStream datagrams { get; }

            public virtual extern void close();
            public virtual extern void close(dom.WebTransportCloseInfo closeInfo);
            public virtual extern es5.Promise<dom.WebTransportBidirectionalStream> createBidirectionalStream();
            public virtual extern es5.Promise<dom.WritableStream> createUnidirectionalStream();
            public virtual dom.ReadableStream incomingBidirectionalStreams { get; }
            public virtual dom.ReadableStream incomingUnidirectionalStreams { get; }
        }

        [CombinedClass]
        [FormerInterface]
        public class WebTransportDatagramDuplexStream : IObject
        {
            public static dom.WebTransportDatagramDuplexStream prototype { get; set; }
            public virtual dom.ReadableStream readable { get; }
            public virtual dom.WritableStream writable { get; }
            public virtual double maxDatagramSize { get; }
            public virtual double? incomingMaxAge { get; set; }
            public virtual double? outgoingMaxAge { get; set; }
            public virtual double? incomingHighWaterMark { get; set; }
            public virtual double? outgoingHighWaterMark { get; set; }
        }

        [CombinedClass]
        [FormerInterface]
        public class WebTransportBidirectionalStream : IObject
        {
            public static dom.WebTransportBidirectionalStream prototype { get; set; }
            public virtual dom.ReadableStream readable { get; }
            public virtual dom.WritableStream writable { get; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class WebTransportOptions : IObject
        {
            public bool? allowPooling { get; set; }
            public bool? requireUnreliable { get; set; }
            public dom.WebTransportHash[] serverCertificateHashes { get; set; }
            public dom.WebTransportCongestionControl congestionControl { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class WebTransportHash : IObject
        {
            public string algorithm { get; set; }
            public es5.ArrayBufferView value { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class WebTransportCloseInfo : IObject
        {
            public uint? closeCode { get; set; }
            public string reason { get; set; }
        }

        [Name("System.String")]
        public class WebTransportCongestionControl : LiteralType<string>
        {
            [Template("<self>\"default\"")]
            public static readonly dom.WebTransportCongestionControl @default;
            [Template("<self>\"throughput\"")]
            public static readonly dom.WebTransportCongestionControl throughput;
            [Template("<self>\"low-latency\"")]
            public static readonly dom.WebTransportCongestionControl lowLatency;
            private extern WebTransportCongestionControl();
            public static extern implicit operator dom.WebTransportCongestionControl(string value);
        }

        [Virtual]
        public abstract class WebTransportTypeConfig : IObject
        {
             public virtual dom.WebTransport prototype { get; set; }
             [Template("new {this}({0})")]
             public abstract dom.WebTransport New(string url);
             [Template("new {this}({0}, {1})")]
             public abstract dom.WebTransport New(string url, dom.WebTransportOptions options);
        }
    }
}
