using H5;
using H5.Core;

namespace H5.Core
{
    public static partial class dom
    {
        [CombinedClass]
        [FormerInterface]
        public class Bluetooth : dom.EventTarget
        {
            public static dom.Bluetooth prototype { get; set; }

            public virtual extern es5.Promise<dom.BluetoothDevice> requestDevice(dom.RequestDeviceOptions options);

            public virtual extern es5.Promise<bool> getAvailability();
        }

        [CombinedClass]
        [FormerInterface]
        public class BluetoothDevice : dom.EventTarget
        {
            public static dom.BluetoothDevice prototype { get; set; }

            public virtual string id { get; }
            public virtual string name { get; }
            public virtual dom.BluetoothRemoteGATTServer gatt { get; }
        }

        [CombinedClass]
        [FormerInterface]
        public class BluetoothRemoteGATTServer : IObject
        {
            public static dom.BluetoothRemoteGATTServer prototype { get; set; }

            public virtual dom.BluetoothDevice device { get; }
            public virtual bool connected { get; }

            public virtual extern es5.Promise<dom.BluetoothRemoteGATTServer> connect();
            public virtual extern void disconnect();
            public virtual extern es5.Promise<dom.BluetoothRemoteGATTService> getPrimaryService(Union<string, double> service);
        }

        [CombinedClass]
        [FormerInterface]
        public class BluetoothRemoteGATTService : dom.EventTarget
        {
            public static dom.BluetoothRemoteGATTService prototype { get; set; }

            public virtual string uuid { get; }
            public virtual bool isPrimary { get; }
            public virtual dom.BluetoothDevice device { get; }

            public virtual extern es5.Promise<dom.BluetoothRemoteGATTCharacteristic> getCharacteristic(Union<string, double> characteristic);
        }

        [CombinedClass]
        [FormerInterface]
        public class BluetoothRemoteGATTCharacteristic : dom.EventTarget
        {
            public static dom.BluetoothRemoteGATTCharacteristic prototype { get; set; }

            public virtual string uuid { get; }
            public virtual dom.BluetoothRemoteGATTService service { get; }
            public virtual es5.DataView value { get; }

            public virtual extern es5.Promise<es5.DataView> readValue();
            public virtual extern es5.Promise<H5.Core.Void> writeValue(es5.ArrayBufferView value);
            public virtual extern es5.Promise<H5.Core.Void> startNotifications();
            public virtual extern es5.Promise<H5.Core.Void> stopNotifications();
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class RequestDeviceOptions : IObject
        {
            public dom.BluetoothLEScanFilterInit[] filters { get; set; }
            public string[] optionalServices { get; set; }
            public bool? acceptAllDevices { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class BluetoothLEScanFilterInit : IObject
        {
            public string[] services { get; set; }
            public string name { get; set; }
            public string namePrefix { get; set; }
        }

        [Virtual]
        public abstract class BluetoothTypeConfig : IObject
        {
             public virtual dom.Bluetooth prototype { get; set; }
        }
    }
}
