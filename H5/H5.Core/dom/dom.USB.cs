using H5;
using H5.Core;

namespace H5.Core
{
    public static partial class dom
    {
        [CombinedClass]
        [FormerInterface]
        public class USB : dom.EventTarget
        {
            public static dom.USB prototype { get; set; }

            public virtual extern es5.Promise<dom.USBDevice[]> getDevices();

            public virtual extern es5.Promise<dom.USBDevice> requestDevice(dom.USBDeviceRequestOptions options);
        }

        [CombinedClass]
        [FormerInterface]
        public class USBDevice : IObject
        {
            public static dom.USBDevice prototype { get; set; }

            public virtual byte usbVersionMajor { get; }
            public virtual byte usbVersionMinor { get; }
            public virtual byte usbVersionSubminor { get; }
            public virtual byte deviceClass { get; }
            public virtual byte deviceSubclass { get; }
            public virtual byte deviceProtocol { get; }
            public virtual ushort vendorId { get; }
            public virtual ushort productId { get; }
            public virtual byte deviceVersionMajor { get; }
            public virtual byte deviceVersionMinor { get; }
            public virtual byte deviceVersionSubminor { get; }
            public virtual string manufacturerName { get; }
            public virtual string productName { get; }
            public virtual string serialNumber { get; }
            public virtual dom.USBConfiguration configuration { get; }
            public virtual dom.USBConfiguration[] configurations { get; }
            public virtual bool opened { get; }

            public virtual extern es5.Promise<H5.Core.Void> open();
            public virtual extern es5.Promise<H5.Core.Void> close();
            public virtual extern es5.Promise<H5.Core.Void> selectConfiguration(byte configurationValue);
            public virtual extern es5.Promise<H5.Core.Void> claimInterface(byte interfaceNumber);
            public virtual extern es5.Promise<H5.Core.Void> releaseInterface(byte interfaceNumber);
            public virtual extern es5.Promise<H5.Core.Void> selectAlternateInterface(byte interfaceNumber, byte alternateSetting);
            public virtual extern es5.Promise<dom.USBInTransferResult> controlTransferIn(dom.USBControlTransferParameters setup, ushort length);
            public virtual extern es5.Promise<dom.USBOutTransferResult> controlTransferOut(dom.USBControlTransferParameters setup);
            public virtual extern es5.Promise<dom.USBOutTransferResult> controlTransferOut(dom.USBControlTransferParameters setup, es5.ArrayBufferView data);
            public virtual extern es5.Promise<H5.Core.Void> clearHalt(dom.USBDirection direction, byte endpointNumber);
            public virtual extern es5.Promise<dom.USBInTransferResult> transferIn(byte endpointNumber, double length);
            public virtual extern es5.Promise<dom.USBOutTransferResult> transferOut(byte endpointNumber, es5.ArrayBufferView data);
            public virtual extern es5.Promise<dom.USBIsochronousInTransferResult> isochronousTransferIn(byte endpointNumber, double[] packetLengths);
            public virtual extern es5.Promise<dom.USBIsochronousOutTransferResult> isochronousTransferOut(byte endpointNumber, es5.ArrayBufferView data, double[] packetLengths);
            public virtual extern es5.Promise<H5.Core.Void> reset();
        }

        [CombinedClass]
        [FormerInterface]
        public class USBConfiguration : IObject
        {
            public static dom.USBConfiguration prototype { get; set; }
            public virtual byte configurationValue { get; }
            public virtual string configurationName { get; }
            public virtual dom.USBInterface[] interfaces { get; }
        }

        [CombinedClass]
        [FormerInterface]
        public class USBInterface : IObject
        {
            public static dom.USBInterface prototype { get; set; }
            public virtual byte interfaceNumber { get; }
            public virtual dom.USBAlternateInterface alternate { get; }
            public virtual dom.USBAlternateInterface[] alternates { get; }
            public virtual bool claimed { get; }
        }

        [CombinedClass]
        [FormerInterface]
        public class USBAlternateInterface : IObject
        {
            public static dom.USBAlternateInterface prototype { get; set; }
            public virtual byte alternateSetting { get; }
            public virtual byte interfaceClass { get; }
            public virtual byte interfaceSubclass { get; }
            public virtual byte interfaceProtocol { get; }
            public virtual string interfaceName { get; }
            public virtual dom.USBEndpoint[] endpoints { get; }
        }

        [CombinedClass]
        [FormerInterface]
        public class USBEndpoint : IObject
        {
            public static dom.USBEndpoint prototype { get; set; }
            public virtual byte endpointNumber { get; }
            public virtual dom.USBDirection direction { get; }
            public virtual dom.USBEndpointType type { get; }
            public virtual double packetSize { get; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class USBDeviceRequestOptions : IObject
        {
            public dom.USBDeviceFilter[] filters { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class USBDeviceFilter : IObject
        {
            public ushort? vendorId { get; set; }
            public ushort? productId { get; set; }
            public byte? classCode { get; set; }
            public byte? subclassCode { get; set; }
            public byte? protocolCode { get; set; }
            public string serialNumber { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class USBControlTransferParameters : IObject
        {
            public dom.USBRequestType requestType { get; set; }
            public dom.USBRecipient recipient { get; set; }
            public byte request { get; set; }
            public ushort value { get; set; }
            public ushort index { get; set; }
        }

        [CombinedClass]
        [FormerInterface]
        public class USBInTransferResult : IObject
        {
            public virtual es5.DataView data { get; }
            public virtual dom.USBTransferStatus status { get; }
        }

        [CombinedClass]
        [FormerInterface]
        public class USBOutTransferResult : IObject
        {
            public virtual double bytesWritten { get; }
            public virtual dom.USBTransferStatus status { get; }
        }

        [CombinedClass]
        [FormerInterface]
        public class USBIsochronousInTransferResult : IObject
        {
            public virtual es5.DataView data { get; }
            public virtual dom.USBIsochronousInTransferPacket[] packets { get; }
        }

        [CombinedClass]
        [FormerInterface]
        public class USBIsochronousOutTransferResult : IObject
        {
            public virtual dom.USBIsochronousOutTransferPacket[] packets { get; }
        }

        [CombinedClass]
        [FormerInterface]
        public class USBIsochronousInTransferPacket : IObject
        {
            public virtual es5.DataView data { get; }
            public virtual dom.USBTransferStatus status { get; }
        }

        [CombinedClass]
        [FormerInterface]
        public class USBIsochronousOutTransferPacket : IObject
        {
            public virtual double bytesWritten { get; }
            public virtual dom.USBTransferStatus status { get; }
        }

        [Name("System.String")]
        public class USBDirection : LiteralType<string>
        {
            [Template("<self>\"in\"")]
            public static readonly dom.USBDirection @in;
            [Template("<self>\"out\"")]
            public static readonly dom.USBDirection @out;
            private extern USBDirection();
            public static extern implicit operator dom.USBDirection(string value);
        }

        [Name("System.String")]
        public class USBEndpointType : LiteralType<string>
        {
            [Template("<self>\"bulk\"")]
            public static readonly dom.USBEndpointType bulk;
            [Template("<self>\"interrupt\"")]
            public static readonly dom.USBEndpointType interrupt;
            [Template("<self>\"isochronous\"")]
            public static readonly dom.USBEndpointType isochronous;
            private extern USBEndpointType();
            public static extern implicit operator dom.USBEndpointType(string value);
        }

        [Name("System.String")]
        public class USBRequestType : LiteralType<string>
        {
            [Template("<self>\"standard\"")]
            public static readonly dom.USBRequestType standard;
            [Template("<self>\"class\"")]
            public static readonly dom.USBRequestType @class;
            [Template("<self>\"vendor\"")]
            public static readonly dom.USBRequestType vendor;
            private extern USBRequestType();
            public static extern implicit operator dom.USBRequestType(string value);
        }

        [Name("System.String")]
        public class USBRecipient : LiteralType<string>
        {
            [Template("<self>\"device\"")]
            public static readonly dom.USBRecipient device;
            [Template("<self>\"interface\"")]
            public static readonly dom.USBRecipient @interface;
            [Template("<self>\"endpoint\"")]
            public static readonly dom.USBRecipient endpoint;
            [Template("<self>\"other\"")]
            public static readonly dom.USBRecipient other;
            private extern USBRecipient();
            public static extern implicit operator dom.USBRecipient(string value);
        }

        [Name("System.String")]
        public class USBTransferStatus : LiteralType<string>
        {
            [Template("<self>\"ok\"")]
            public static readonly dom.USBTransferStatus ok;
            [Template("<self>\"stall\"")]
            public static readonly dom.USBTransferStatus stall;
            [Template("<self>\"babble\"")]
            public static readonly dom.USBTransferStatus babble;
            private extern USBTransferStatus();
            public static extern implicit operator dom.USBTransferStatus(string value);
        }

        [Virtual]
        public abstract class USBTypeConfig : IObject
        {
             public virtual dom.USB prototype { get; set; }
        }
    }
}
