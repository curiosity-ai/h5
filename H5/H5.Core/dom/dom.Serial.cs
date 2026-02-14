using H5;
using H5.Core;

namespace H5.Core
{
    public static partial class dom
    {
        [CombinedClass]
        [FormerInterface]
        public class Serial : dom.EventTarget
        {
            public static dom.Serial prototype { get; set; }

            public virtual extern es5.Promise<dom.SerialPort[]> getPorts();

            public virtual extern es5.Promise<dom.SerialPort> requestPort(dom.SerialPortRequestOptions options);

            public virtual dom.Serial.onconnectFn onconnect { get; set; }
            public virtual dom.Serial.ondisconnectFn ondisconnect { get; set; }

            [Generated]
            public delegate void onconnectFn(dom.Event ev); // SerialConnectionEvent usually just Event
            [Generated]
            public delegate void ondisconnectFn(dom.Event ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SerialPort : dom.EventTarget
        {
            public static dom.SerialPort prototype { get; set; }

            public virtual bool readable { get; }
            public virtual bool writable { get; }

            public virtual extern dom.SerialPortInfo getInfo();
            public virtual extern es5.Promise<H5.Core.Void> open(dom.SerialOptions options);
            public virtual extern es5.Promise<H5.Core.Void> close();
            public virtual extern es5.Promise<H5.Core.Void> setSignals(dom.SerialOutputSignals signals);
            public virtual extern es5.Promise<dom.SerialInputSignals> getSignals();

            // ReadableStream and WritableStream properties logic is complex in binding if types are generic in .NET but not in TS/JS binding context sometimes.
            // Using standard WritableStream/ReadableStream.
            public virtual dom.ReadableStream readableStream { get; }
            public virtual dom.WritableStream writableStream { get; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class SerialPortInfo : IObject
        {
            public ushort? usbVendorId { get; set; }
            public ushort? usbProductId { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class SerialOptions : IObject
        {
            public double baudRate { get; set; }
            public double? dataBits { get; set; }
            public double? stopBits { get; set; }
            public dom.ParityType parity { get; set; }
            public double? bufferSize { get; set; }
            public dom.FlowControlType flowControl { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class SerialOutputSignals : IObject
        {
            public bool? dataTerminalReady { get; set; }
            public bool? requestToSend { get; set; }
            public bool? breakSignal { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class SerialInputSignals : IObject
        {
            public bool? dataCarrierDetect { get; set; }
            public bool? clearToSend { get; set; }
            public bool? ringIndicator { get; set; }
            public bool? dataSetReady { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class SerialPortRequestOptions : IObject
        {
            public dom.SerialPortFilter[] filters { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class SerialPortFilter : IObject
        {
            public ushort? usbVendorId { get; set; }
            public ushort? usbProductId { get; set; }
        }

        [Name("System.String")]
        public class ParityType : LiteralType<string>
        {
            [Template("<self>\"none\"")]
            public static readonly dom.ParityType none;
            [Template("<self>\"even\"")]
            public static readonly dom.ParityType even;
            [Template("<self>\"odd\"")]
            public static readonly dom.ParityType odd;
            private extern ParityType();
            public static extern implicit operator dom.ParityType(string value);
        }

        [Name("System.String")]
        public class FlowControlType : LiteralType<string>
        {
            [Template("<self>\"none\"")]
            public static readonly dom.FlowControlType none;
            [Template("<self>\"hardware\"")]
            public static readonly dom.FlowControlType hardware;
            private extern FlowControlType();
            public static extern implicit operator dom.FlowControlType(string value);
        }

        [Virtual]
        public abstract class SerialTypeConfig : IObject
        {
             public virtual dom.Serial prototype { get; set; }
        }
    }
}
