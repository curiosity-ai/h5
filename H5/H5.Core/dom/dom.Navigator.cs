// Decompiled with JetBrains decompiler
// Type: H5.dom
// Assembly: H5.dom, Version=2.8.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 57CCBF73-D494-47BA-ACF8-95E65E795865
// Assembly location: C:\work\curiosity\tesserae\Tesserae\bin\Debug\net461\H5.dom.dll

using H5;
using H5.Core;
using System;
using System.Collections;
using System.Collections.Generic;

namespace H5.Core
{
    public static partial class dom
    {

        [CombinedClass]
        [FormerInterface]
        public class Clipboard : IObject
        {
            public static dom.Clipboard prototype
            {
                get;
                set;
            }

            public virtual extern es5.Promise<dom.ClipboardItem> read();
            public virtual extern es5.Promise<string> readText();

            public virtual extern es5.Promise<H5.Core.Void> write(dom.ClipboardItem data);
            public virtual extern es5.Promise<H5.Core.Void> writeText(string newClipText);

        }

        [CombinedClass]
        [FormerInterface]
        public class ClipboardItem : IObject
        {
            public static dom.ClipboardItem prototype
            {
                get;
                set;
            }
            public extern ClipboardItem(object clipboardItemData);

            public virtual es5.ReadonlyArray<string> types
            {
                get;
            }

            public virtual string presentationStyle
            {
                get;
            }

            public virtual extern es5.Promise<dom.Blob> getType(string mimeType);
        }



        [CombinedClass]
        [FormerInterface]
        public partial class Navigator : dom.NavigatorID, dom.NavigatorOnLine.Interface, IObject, dom.NavigatorContentUtils.Interface, dom.NavigatorStorageUtils.Interface, dom.MSNavigatorDoNotTrack.Interface, dom.MSFileSaver.Interface, dom.NavigatorBeacon.Interface, dom.NavigatorConcurrentHardware.Interface, dom.NavigatorUserMedia.Interface, dom.NavigatorLanguage.Interface
        {
            public static dom.Navigator prototype
            {
                get;
                set;
            }

            public virtual es5.ReadonlyArray<dom.VRDisplay> activeVRDisplays
            {
                get;
            }

            public virtual dom.WebAuthentication authentication
            {
                get;
            }

            public virtual dom.Clipboard clipboard
            {
                get;
            }

            public virtual bool cookieEnabled
            {
                get;
            }

            public virtual string doNotTrack
            {
                get;
            }

            public virtual dom.GamepadInputEmulationType gamepadInputEmulation
            {
                get;
                set;
            }

            public virtual dom.Geolocation geolocation
            {
                get;
            }

            public virtual double maxTouchPoints
            {
                get;
            }

            public virtual dom.MimeTypeArray mimeTypes
            {
                get;
            }

            public virtual bool msManipulationViewsEnabled
            {
                get;
            }

            public virtual double msMaxTouchPoints
            {
                get;
            }

            public virtual bool msPointerEnabled
            {
                get;
            }

            public virtual dom.PluginArray plugins
            {
                get;
            }

            public virtual bool pointerEnabled
            {
                get;
            }

            public virtual dom.ServiceWorkerContainer serviceWorker
            {
                get;
            }

            public virtual bool webdriver
            {
                get;
            }

            public virtual extern dom.Gamepad[] getGamepads();

            public virtual extern es5.Promise<dom.VRDisplay[]> getVRDisplays();

            public virtual extern bool javaEnabled();

            public virtual extern void msLaunchUri(string uri);

            public virtual extern void msLaunchUri(string uri, dom.MSLaunchUriCallback successCallback);

            public virtual extern void msLaunchUri(
              string uri,
              dom.MSLaunchUriCallback successCallback,
              dom.MSLaunchUriCallback noHandlerCallback);

            public virtual extern es5.Promise<dom.MediaKeySystemAccess> requestMediaKeySystemAccess(
              string keySystem,
              dom.MediaKeySystemConfiguration[] supportedConfigurations);

            public virtual extern bool vibrate(Union<double, double[]> pattern);

            public virtual extern bool vibrate(double pattern);

            public virtual extern bool vibrate(double[] pattern);

            public virtual bool onLine
            {
                get;
            }

            public virtual extern bool confirmSiteSpecificTrackingException(
              dom.ConfirmSiteSpecificExceptionsInformation args);

            public virtual extern bool confirmWebWideTrackingException(dom.ExceptionInformation args);

            public virtual extern void removeSiteSpecificTrackingException(dom.ExceptionInformation args);

            public virtual extern void removeWebWideTrackingException(dom.ExceptionInformation args);

            public virtual extern void storeSiteSpecificTrackingException(
              dom.StoreSiteSpecificExceptionsInformation args);

            public virtual extern void storeWebWideTrackingException(dom.StoreExceptionsInformation args);

            public virtual extern bool msSaveBlob(object blob);

            public virtual extern bool msSaveBlob(object blob, string defaultName);

            public virtual extern bool msSaveOrOpenBlob(object blob);

            public virtual extern bool msSaveOrOpenBlob(object blob, string defaultName);

            public virtual extern bool sendBeacon(string url);

            public virtual extern bool sendBeacon(
              string url,
              Union<dom.Blob, es5.Int8Array, es5.Int16Array, es5.Int32Array, es5.Uint8Array, es5.Uint16Array, es5.Uint32Array, es5.Uint8ClampedArray, es5.Float32Array, es5.Float64Array, es5.DataView, es5.ArrayBuffer, dom.FormData, string, Null> data);

            public virtual extern bool sendBeacon(string url, dom.Blob data);

            public virtual extern bool sendBeacon(string url, es5.Int8Array data);

            public virtual extern bool sendBeacon(string url, es5.Int16Array data);

            public virtual extern bool sendBeacon(string url, es5.Int32Array data);

            public virtual extern bool sendBeacon(string url, es5.Uint8Array data);

            public virtual extern bool sendBeacon(string url, es5.Uint16Array data);

            public virtual extern bool sendBeacon(string url, es5.Uint32Array data);

            public virtual extern bool sendBeacon(string url, es5.Uint8ClampedArray data);

            public virtual extern bool sendBeacon(string url, es5.Float32Array data);

            public virtual extern bool sendBeacon(string url, es5.Float64Array data);

            public virtual extern bool sendBeacon(string url, es5.DataView data);

            public virtual extern bool sendBeacon(string url, es5.ArrayBuffer data);

            public virtual extern bool sendBeacon(string url, dom.FormData data);

            public virtual extern bool sendBeacon(string url, string data);

            public virtual extern bool sendBeacon(string url, Null data);

            public virtual ulong hardwareConcurrency
            {
                get;
            }

            public virtual dom.MediaDevices mediaDevices
            {
                get;
            }

            public virtual extern es5.Promise<dom.MediaStream> getDisplayMedia(
              dom.MediaStreamConstraints constraints);

            public virtual extern void getUserMedia(
              dom.MediaStreamConstraints constraints,
              dom.NavigatorUserMediaSuccessCallback successCallback,
              dom.NavigatorUserMediaErrorCallback errorCallback);

            public virtual string language
            {
                get;
            }

            public virtual es5.ReadonlyArray<string> languages
            {
                get;
            }
        }
    }
}
