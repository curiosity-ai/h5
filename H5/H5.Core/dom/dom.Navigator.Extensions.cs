using H5;
using H5.Core;
using System;

namespace H5.Core
{
    public static partial class dom
    {
        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class ShareData : IObject
        {
            public string title { get; set; }
            public string text { get; set; }
            public string url { get; set; }
            public object[] files { get; set; }
        }

        [CombinedClass]
        [FormerInterface]
        public class CredentialsContainer : IObject
        {
            public virtual extern es5.Promise<object> create(object options);
            public virtual extern es5.Promise<object> get(object options);
            public virtual extern es5.Promise<bool> preventSilentAccess();
            public virtual extern es5.Promise<H5.Core.Void> store(object credential);
        }

        [CombinedClass]
        [FormerInterface]
        public class StorageManager : IObject
        {
            public virtual extern es5.Promise<bool> persisted();
            public virtual extern es5.Promise<bool> persist();
            public virtual extern es5.Promise<object> estimate();
            public virtual extern es5.Promise<dom.FileSystemDirectoryHandle> getDirectory();
        }

        [CombinedClass]
        [FormerInterface]
        public class PermissionStatus : dom.EventTarget
        {
            public virtual string state { get; }
            public virtual dom.PermissionStatus.onchangeFn onchange { get; set; }
            [Generated]
            public delegate void onchangeFn(dom.Event ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class Permissions : IObject
        {
            public virtual extern es5.Promise<dom.PermissionStatus> query(object permissionDescriptor);
        }

        [CombinedClass]
        [FormerInterface]
        public class LockManager : IObject
        {
            public virtual extern es5.Promise<object> request(string name, Action<object> callback);
            public virtual extern es5.Promise<object> request(string name, object options, Action<object> callback);
            public virtual extern es5.Promise<object> query();
        }

        [CombinedClass]
        [FormerInterface]
        public class WakeLockSentinel : dom.EventTarget
        {
            public virtual bool released { get; }
            public virtual string type { get; }
            public virtual extern es5.Promise<H5.Core.Void> release();
            public virtual dom.WakeLockSentinel.onreleaseFn onrelease { get; set; }
            [Generated]
            public delegate void onreleaseFn(dom.Event ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class WakeLock : IObject
        {
            public virtual extern es5.Promise<dom.WakeLockSentinel> request(string type);
        }

        [CombinedClass]
        [FormerInterface]
        public class UserActivation : IObject
        {
            public virtual bool hasBeenActive { get; }
            public virtual bool isActive { get; }
        }

        [CombinedClass]
        [FormerInterface]
        public class NetworkInformation : dom.EventTarget
        {
            public virtual double downlink { get; }
            public virtual double downlinkMax { get; }
            public virtual string effectiveType { get; }
            public virtual double rtt { get; }
            public virtual bool saveData { get; }
            public virtual string type { get; }
            public virtual dom.NetworkInformation.onchangeFn onchange { get; set; }
            [Generated]
            public delegate void onchangeFn(dom.Event ev);
        }

        public partial class Navigator
        {
            public virtual dom.CredentialsContainer credentials { get; }
            public virtual dom.StorageManager storage { get; }
            public virtual dom.Permissions permissions { get; }
            public virtual dom.LockManager locks { get; }
            public virtual dom.WakeLock wakeLock { get; }
            public virtual dom.UserActivation userActivation { get; }
            public virtual dom.NetworkInformation connection { get; }
            // clipboard is already defined in dom.Navigator.cs
            public virtual dom.Bluetooth bluetooth { get; }
            public virtual dom.USB usb { get; }
            public virtual dom.XRSystem xr { get; }
            public virtual dom.HID hid { get; }
            public virtual dom.Serial serial { get; }

            public virtual extern es5.Promise<H5.Core.Void> share(dom.ShareData data);
            public virtual extern es5.Promise<H5.Core.Void> setAppBadge(double contents);
            public virtual extern es5.Promise<H5.Core.Void> setAppBadge();
            public virtual extern es5.Promise<H5.Core.Void> clearAppBadge();
            public virtual extern es5.Promise<dom.MIDIAccess> requestMIDIAccess();
            public virtual extern es5.Promise<dom.MIDIAccess> requestMIDIAccess(dom.MIDIOptions options);
            public virtual extern bool canShare(dom.ShareData data);
        }
    }
}
