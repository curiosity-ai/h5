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
        public class HTMLAllCollection : IObject
        {

            public static dom.HTMLAllCollection prototype
            {
                get;
                set;
            }

            public virtual uint length
            {
                get;
            }

            public virtual extern Union<dom.HTMLCollection, dom.Element, Null> item();

            public virtual extern Union<dom.HTMLCollection, dom.Element, Null> item(
              string nameOrIndex);

            public virtual extern Union<dom.HTMLCollection, dom.Element, Null> namedItem(
              string name);

            public virtual extern dom.Element this[double index] { get; set; }
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLAnchorElement : dom.HTMLElement, dom.HTMLHyperlinkElementUtils.Interface, IObject
        {









            [Template("document.createElement(\"a\")")]
            public extern HTMLAnchorElement();

            public static dom.HTMLAnchorElement prototype
            {
                get;
                set;
            }

            public virtual string Methods
            {
                get;
                set;
            }

            public virtual string charset
            {
                get;
                set;
            }

            public virtual string coords
            {
                get;
                set;
            }

            public virtual string download
            {
                get;
                set;
            }

            public virtual string hreflang
            {
                get;
                set;
            }

            public virtual string mimeType
            {
                get;
            }

            public virtual string name
            {
                get;
                set;
            }

            public virtual string nameProp
            {
                get;
            }

            public virtual string protocolLong
            {
                get;
            }

            public virtual string rel
            {
                get;
                set;
            }

            public virtual string rev
            {
                get;
                set;
            }

            public virtual string shape
            {
                get;
                set;
            }

            public virtual string target
            {
                get;
                set;
            }

            public virtual string text
            {
                get;
                set;
            }

            public virtual string type
            {
                get;
                set;
            }

            public virtual string urn
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLAnchorElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLAnchorElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLAnchorElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLAnchorElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLAnchorElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLAnchorElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLAnchorElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLAnchorElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual string hash
            {
                get;
                set;
            }

            public virtual string host
            {
                get;
                set;
            }

            public virtual string hostname
            {
                get;
                set;
            }

            public virtual string href
            {
                get;
                set;
            }

            public virtual string origin
            {
                get;
                set;
            }

            public virtual string pathname
            {
                get;
                set;
            }

            public virtual string port
            {
                get;
                set;
            }

            public virtual string protocol
            {
                get;
                set;
            }

            public virtual string search
            {
                get;
                set;
            }

            public virtual extern string toString();

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLAppletElement : dom.HTMLElement
        {




            [Template("document.createElement(\"applet\")")]
            public extern HTMLAppletElement();

            public static dom.HTMLAppletElement prototype
            {
                get;
                set;
            }

            public virtual string align
            {
                get;
                set;
            }

            public virtual string alt
            {
                get;
                set;
            }

            public virtual string archive
            {
                get;
                set;
            }

            public virtual string code
            {
                get;
                set;
            }

            public virtual string codeBase
            {
                get;
                set;
            }

            public virtual dom.HTMLFormElement form
            {
                get;
            }

            public virtual string height
            {
                get;
                set;
            }

            public virtual uint hspace
            {
                get;
                set;
            }

            public virtual string name
            {
                get;
                set;
            }

            [Name("object")]
            public virtual string @object
            {
                get;
                set;
            }

            public virtual uint vspace
            {
                get;
                set;
            }

            public virtual string width
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLAppletElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLAppletElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLAppletElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLAppletElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLAppletElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLAppletElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLAppletElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLAppletElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLAreaElement : dom.HTMLElement, dom.HTMLHyperlinkElementUtils.Interface, IObject
        {






            [Template("document.createElement(\"area\")")]
            public extern HTMLAreaElement();

            public static dom.HTMLAreaElement prototype
            {
                get;
                set;
            }

            public virtual string alt
            {
                get;
                set;
            }

            public virtual string coords
            {
                get;
                set;
            }

            public virtual string download
            {
                get;
                set;
            }

            public virtual bool noHref
            {
                get;
                set;
            }

            public virtual string rel
            {
                get;
                set;
            }

            public virtual string shape
            {
                get;
                set;
            }

            public virtual string target
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLAreaElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLAreaElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLAreaElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLAreaElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLAreaElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLAreaElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLAreaElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLAreaElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual string hash
            {
                get;
                set;
            }

            public virtual string host
            {
                get;
                set;
            }

            public virtual string hostname
            {
                get;
                set;
            }

            public virtual string href
            {
                get;
                set;
            }

            public virtual string origin
            {
                get;
                set;
            }

            public virtual string pathname
            {
                get;
                set;
            }

            public virtual string port
            {
                get;
                set;
            }

            public virtual string protocol
            {
                get;
                set;
            }

            public virtual string search
            {
                get;
                set;
            }

            public virtual extern string toString();

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLAreasCollection : dom.HTMLCollectionBase
        {
            public static dom.HTMLAreasCollection prototype
            {
                get;
                set;
            }

            public override extern uint length { get; }

            public override extern dom.Element item(uint index);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLAudioElement : dom.HTMLMediaElement
        {
            [Template("document.createElement(\"audio\")")]
            public extern HTMLAudioElement();

            public static dom.HTMLAudioElement prototype
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLMediaElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLAudioElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLMediaElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLAudioElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLMediaElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLAudioElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLMediaElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLAudioElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLMediaElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLAudioElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLMediaElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLAudioElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLMediaElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLAudioElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLMediaElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLAudioElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLMediaElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLMediaElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLBRElement : dom.HTMLElement
        {

            [Template("document.createElement(\"br\")")]
            public extern HTMLBRElement();

            public static dom.HTMLBRElement prototype
            {
                get;
                set;
            }

            public virtual string clear
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLBRElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLBRElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLBRElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLBRElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLBRElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLBRElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLBRElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLBRElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLBaseElement : dom.HTMLElement
        {


            [Template("document.createElement(\"base\")")]
            public extern HTMLBaseElement();

            public static dom.HTMLBaseElement prototype
            {
                get;
                set;
            }

            public virtual string href
            {
                get;
                set;
            }

            public virtual string target
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLBaseElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLBaseElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLBaseElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLBaseElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLBaseElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLBaseElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLBaseElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLBaseElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLBaseFontElement : dom.HTMLElement, dom.DOML2DeprecatedColorProperty.Interface, IObject
        {

            [Template("document.createElement(\"basefont\")")]
            public extern HTMLBaseFontElement();

            public static dom.HTMLBaseFontElement prototype
            {
                get;
                set;
            }

            public virtual string face
            {
                get;
                set;
            }

            public virtual double size
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLBaseFontElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLBaseFontElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLBaseFontElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLBaseFontElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLBaseFontElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLBaseFontElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLBaseFontElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLBaseFontElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual string color
            {
                get;
                set;
            }

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class HTMLBodyElementEventMap : dom.HTMLElementEventMap, dom.WindowEventHandlersEventMap.Interface, IObject
        {





            public dom.Event orientationchange
            {
                get;
                set;
            }

            public dom.UIEvent resize
            {
                get;
                set;
            }

            public dom.Event afterprint
            {
                get;
                set;
            }

            public dom.Event beforeprint
            {
                get;
                set;
            }

            public dom.BeforeUnloadEvent beforeunload
            {
                get;
                set;
            }

            public dom.HashChangeEvent hashchange
            {
                get;
                set;
            }

            public dom.MessageEvent message
            {
                get;
                set;
            }

            public dom.Event offline
            {
                get;
                set;
            }

            public dom.Event online
            {
                get;
                set;
            }

            public dom.PageTransitionEvent pagehide
            {
                get;
                set;
            }

            public dom.PageTransitionEvent pageshow
            {
                get;
                set;
            }

            public dom.PopStateEvent popstate
            {
                get;
                set;
            }

            public dom.StorageEvent storage
            {
                get;
                set;
            }

            public dom.Event unload
            {
                get;
                set;
            }

            [Generated]
            public new static class KeyOf
            {
                [Template("\"orientationchange\"")]
                public static readonly KeyOf<dom.HTMLBodyElementEventMap> orientationchange;
                [Template("\"resize\"")]
                public static readonly KeyOf<dom.HTMLBodyElementEventMap> resize;
                [Template("\"afterprint\"")]
                public static readonly KeyOf<dom.HTMLBodyElementEventMap> afterprint;
                [Template("\"beforeprint\"")]
                public static readonly KeyOf<dom.HTMLBodyElementEventMap> beforeprint;
                [Template("\"beforeunload\"")]
                public static readonly KeyOf<dom.HTMLBodyElementEventMap> beforeunload;
                [Template("\"hashchange\"")]
                public static readonly KeyOf<dom.HTMLBodyElementEventMap> hashchange;
                [Template("\"message\"")]
                public static readonly KeyOf<dom.HTMLBodyElementEventMap> message;
                [Template("\"offline\"")]
                public static readonly KeyOf<dom.HTMLBodyElementEventMap> offline;
                [Template("\"online\"")]
                public static readonly KeyOf<dom.HTMLBodyElementEventMap> online;
                [Template("\"pagehide\"")]
                public static readonly KeyOf<dom.HTMLBodyElementEventMap> pagehide;
                [Template("\"pageshow\"")]
                public static readonly KeyOf<dom.HTMLBodyElementEventMap> pageshow;
                [Template("\"popstate\"")]
                public static readonly KeyOf<dom.HTMLBodyElementEventMap> popstate;
                [Template("\"storage\"")]
                public static readonly KeyOf<dom.HTMLBodyElementEventMap> storage;
                [Template("\"unload\"")]
                public static readonly KeyOf<dom.HTMLBodyElementEventMap> unload;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLBodyElement : dom.HTMLElement, dom.WindowEventHandlers.Interface, IObject
        {








            [Template("document.createElement(\"body\")")]
            public extern HTMLBodyElement();

            public static dom.HTMLBodyElement prototype
            {
                get;
                set;
            }

            public virtual string aLink
            {
                get;
                set;
            }

            public virtual string background
            {
                get;
                set;
            }

            public virtual string bgColor
            {
                get;
                set;
            }

            public virtual string bgProperties
            {
                get;
                set;
            }

            public virtual string link
            {
                get;
                set;
            }

            public virtual bool noWrap
            {
                get;
                set;
            }

            public virtual dom.HTMLBodyElement.onorientationchangeFn onorientationchange
            {
                get;
                set;
            }

            public virtual dom.HTMLBodyElement.onresizeFn onresize
            {
                get;
                set;
            }

            public virtual string text
            {
                get;
                set;
            }

            public virtual string vLink
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLBodyElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLBodyElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLBodyElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLBodyElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLBodyElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLBodyElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLBodyElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLBodyElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLBodyElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLBodyElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLBodyElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLBodyElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLBodyElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLBodyElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLBodyElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLBodyElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual dom.WindowEventHandlers.onafterprintFn onafterprint
            {
                get;
                set;
            }

            public virtual dom.WindowEventHandlers.onafterprintFn onbeforeprint
            {
                get;
                set;
            }

            public virtual dom.WindowEventHandlers.onbeforeunloadFn onbeforeunload
            {
                get;
                set;
            }

            public virtual dom.WindowEventHandlers.onhashchangeFn onhashchange
            {
                get;
                set;
            }

            public virtual dom.WindowEventHandlers.onmessageFn onmessage
            {
                get;
                set;
            }

            public virtual dom.WindowEventHandlers.onafterprintFn onoffline
            {
                get;
                set;
            }

            public virtual dom.WindowEventHandlers.onafterprintFn ononline
            {
                get;
                set;
            }

            public virtual dom.WindowEventHandlers.onpagehideFn onpagehide
            {
                get;
                set;
            }

            public virtual dom.WindowEventHandlers.onpagehideFn onpageshow
            {
                get;
                set;
            }

            public virtual dom.WindowEventHandlers.onpopstateFn onpopstate
            {
                get;
                set;
            }

            public virtual dom.WindowEventHandlers.onstorageFn onstorage
            {
                get;
                set;
            }

            public virtual dom.WindowEventHandlers.onafterprintFn onunload
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.WindowEventHandlersEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.WindowEventHandlers.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.WindowEventHandlersEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.WindowEventHandlers.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.WindowEventHandlersEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.WindowEventHandlers.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.WindowEventHandlersEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.WindowEventHandlers.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.WindowEventHandlersEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.WindowEventHandlers.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.WindowEventHandlersEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.WindowEventHandlers.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.WindowEventHandlersEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.WindowEventHandlers.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.WindowEventHandlersEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.WindowEventHandlers.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            public delegate void onorientationchangeFn(dom.Event ev);

            [Generated]
            public delegate void onresizeFn(dom.UIEvent ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLBodyElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLBodyElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLButtonElement : dom.HTMLElement
        {





            [Template("document.createElement(\"button\")")]
            public extern HTMLButtonElement();

            public static dom.HTMLButtonElement prototype
            {
                get;
                set;
            }

            public virtual bool autofocus
            {
                get;
                set;
            }

            public virtual bool disabled
            {
                get;
                set;
            }

            public virtual dom.HTMLFormElement form
            {
                get;
            }

            public virtual string formAction
            {
                get;
                set;
            }

            public virtual string formEnctype
            {
                get;
                set;
            }

            public virtual string formMethod
            {
                get;
                set;
            }

            public virtual bool formNoValidate
            {
                get;
                set;
            }

            public virtual string formTarget
            {
                get;
                set;
            }

            public virtual string name
            {
                get;
                set;
            }

            public virtual object status
            {
                get;
                set;
            }

            public virtual string type
            {
                get;
                set;
            }

            public virtual string validationMessage
            {
                get;
            }

            public virtual dom.ValidityState validity
            {
                get;
            }

            public virtual string value
            {
                get;
                set;
            }

            public virtual bool willValidate
            {
                get;
            }

            public virtual extern bool checkValidity();

            public virtual extern void setCustomValidity(string error);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLButtonElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLButtonElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLButtonElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLButtonElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLButtonElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLButtonElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLButtonElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLButtonElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLCanvasElement : dom.HTMLElement
        {


            [Template("document.createElement(\"canvas\")")]
            public extern HTMLCanvasElement();

            public static dom.HTMLCanvasElement prototype
            {
                get;
                set;
            }

            public virtual uint height
            {
                get;
                set;
            }

            public virtual uint width
            {
                get;
                set;
            }

            public virtual extern dom.CanvasRenderingContext2D getContext(
              dom.Literals.Types._2d contextId);

            public virtual extern dom.CanvasRenderingContext2D getContext(
              dom.Literals.Types._2d contextId,
              dom.Canvas2DContextAttributes contextAttributes);

            public virtual extern dom.WebGLRenderingContext getContext(
              dom.Literals.Options.contextId contextId);

            public virtual extern dom.WebGLRenderingContext getContext(
              dom.Literals.Options.contextId contextId,
              dom.WebGLContextAttributes contextAttributes);

            public virtual extern Union<dom.CanvasRenderingContext2D, dom.WebGLRenderingContext, Null> getContext(
              string contextId);

            public virtual extern Union<dom.CanvasRenderingContext2D, dom.WebGLRenderingContext, Null> getContext(
              string contextId,
              object contextAttributes);

            public virtual extern dom.Blob msToBlob();

            public virtual extern void toBlob(dom.HTMLCanvasElement.toBlobFn callback);

            [ExpandParams]
            public virtual extern void toBlob(
              dom.HTMLCanvasElement.toBlobFn callback,
              string type,
              params object[] arguments);

            public virtual extern string toDataURL();

            [ExpandParams]
            public virtual extern string toDataURL(string type, params object[] args);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLCanvasElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLCanvasElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLCanvasElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLCanvasElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLCanvasElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLCanvasElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLCanvasElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLCanvasElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            public delegate void toBlobFn(dom.Blob result);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [IgnoreCast]
        [Virtual]
        [FormerInterface]
        public abstract class HTMLCollectionBase : IEnumerable<dom.Element>, IEnumerable, IH5Class, IObject
        {
            public abstract uint length { get; }

            public abstract dom.Element item(uint index);

            public virtual extern dom.Element this[uint index] { get; set; }

            [Template("H5.getEnumerator({this}, {T})")]
            extern IEnumerator<dom.Element> IEnumerable<dom.Element>.GetEnumerator();

            [Template("H5.getEnumerator({this})")]
            extern IEnumerator IEnumerable.GetEnumerator();
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLCollection : dom.HTMLCollectionBase
        {
            public static dom.HTMLCollection prototype
            {
                get;
                set;
            }

            public virtual extern dom.Element namedItem(string name);

            public override extern uint length { get; }

            public override extern dom.Element item(uint index);
        }

        [IgnoreCast]
        [IgnoreGeneric(AllowInTypeScript = true)]
        [Virtual]
        [FormerInterface]
        [Where("T", typeof(dom.Element), EnableImplicitConversion = true)]
        public abstract class HTMLCollectionOf<T> : dom.HTMLCollectionBase, IEnumerable<T>, IEnumerable, IH5Class
        {
            public abstract T item(double index);

            public abstract T namedItem(string name);

            public virtual extern T this[double index] { get; set; }

            [Template("H5.getEnumerator({this}, {T})")]
            extern IEnumerator<T> IEnumerable<T>.GetEnumerator();
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLDListElement : dom.HTMLElement
        {

            [Template("document.createElement(\"dl\")")]
            public extern HTMLDListElement();

            public static dom.HTMLDListElement prototype
            {
                get;
                set;
            }

            public virtual bool compact
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLDListElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLDListElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLDListElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLDListElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLDListElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLDListElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLDListElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLDListElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLDataElement : dom.HTMLElement
        {

            [Template("document.createElement(\"data\")")]
            public extern HTMLDataElement();

            public static dom.HTMLDataElement prototype
            {
                get;
                set;
            }

            public virtual string value
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLDataElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLDataElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLDataElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLDataElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLDataElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLDataElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLDataElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLDataElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLDataListElement : dom.HTMLElement
        {

            [Template("document.createElement(\"datalist\")")]
            public extern HTMLDataListElement();

            public static dom.HTMLDataListElement prototype
            {
                get;
                set;
            }

            public virtual dom.HTMLCollectionOf<dom.HTMLOptionElement> options
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLDataListElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLDataListElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLDataListElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLDataListElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLDataListElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLDataListElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLDataListElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLDataListElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLDetailsElement : dom.HTMLElement
        {

            public static dom.HTMLDetailsElement prototype
            {
                get;
                set;
            }

            public virtual bool open
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLDetailsElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLDetailsElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLDetailsElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLDetailsElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLDetailsElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLDetailsElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLDetailsElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLDetailsElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLDialogElement : dom.HTMLElement
        {


            public static dom.HTMLDialogElement prototype
            {
                get;
                set;
            }

            public virtual bool open
            {
                get;
                set;
            }

            public virtual string returnValue
            {
                get;
                set;
            }

            public virtual extern void close();

            public virtual extern void close(string returnValue);

            public virtual extern void show();

            public virtual extern void showModal();

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLDialogElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLDialogElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLDialogElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLDialogElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLDialogElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLDialogElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLDialogElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLDialogElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLDirectoryElement : dom.HTMLElement
        {

            [Template("document.createElement(\"dir\")")]
            public extern HTMLDirectoryElement();

            public static dom.HTMLDirectoryElement prototype
            {
                get;
                set;
            }

            public virtual bool compact
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLDirectoryElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLDirectoryElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLDirectoryElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLDirectoryElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLDirectoryElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLDirectoryElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLDirectoryElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLDirectoryElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLDivElement : dom.HTMLElement
        {


            [Template("document.createElement(\"div\")")]
            public extern HTMLDivElement();

            public static dom.HTMLDivElement prototype
            {
                get;
                set;
            }

            public virtual string align
            {
                get;
                set;
            }

            public virtual bool noWrap
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLDivElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLDivElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLDivElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLDivElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLDivElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLDivElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLDivElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLDivElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLDocument : dom.Document
        {
            public static dom.HTMLDocument prototype
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.DocumentEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLDocument.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.DocumentEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLDocument.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.DocumentEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLDocument.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.DocumentEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLDocument.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.DocumentEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLDocument.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.DocumentEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLDocument.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.DocumentEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLDocument.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.DocumentEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLDocument.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.DocumentEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.DocumentEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class HTMLElementEventMap : dom.ElementEventMap
        {






















            public dom.UIEvent abort
            {
                get;
                set;
            }

            public dom.Event activate
            {
                get;
                set;
            }

            public dom.Event beforeactivate
            {
                get;
                set;
            }

            public dom.Event beforecopy
            {
                get;
                set;
            }

            public dom.Event beforecut
            {
                get;
                set;
            }

            public dom.Event beforedeactivate
            {
                get;
                set;
            }

            public dom.Event beforepaste
            {
                get;
                set;
            }

            public dom.FocusEvent blur
            {
                get;
                set;
            }

            public dom.Event canplay
            {
                get;
                set;
            }

            public dom.Event canplaythrough
            {
                get;
                set;
            }

            public dom.Event change
            {
                get;
                set;
            }

            public dom.MouseEvent click
            {
                get;
                set;
            }

            public dom.PointerEvent contextmenu
            {
                get;
                set;
            }

            public dom.ClipboardEvent copy
            {
                get;
                set;
            }

            public dom.Event cuechange
            {
                get;
                set;
            }

            public dom.ClipboardEvent cut
            {
                get;
                set;
            }

            public dom.MouseEvent dblclick
            {
                get;
                set;
            }

            public dom.Event deactivate
            {
                get;
                set;
            }

            public dom.DragEvent drag
            {
                get;
                set;
            }

            public dom.DragEvent dragend
            {
                get;
                set;
            }

            public dom.DragEvent dragenter
            {
                get;
                set;
            }

            public dom.DragEvent dragleave
            {
                get;
                set;
            }

            public dom.DragEvent dragover
            {
                get;
                set;
            }

            public dom.DragEvent dragstart
            {
                get;
                set;
            }

            public dom.DragEvent drop
            {
                get;
                set;
            }

            public dom.Event durationchange
            {
                get;
                set;
            }

            public dom.Event emptied
            {
                get;
                set;
            }

            public dom.Event ended
            {
                get;
                set;
            }

            public dom.ErrorEvent error
            {
                get;
                set;
            }

            public dom.FocusEvent focus
            {
                get;
                set;
            }

            public dom.Event input
            {
                get;
                set;
            }

            public dom.Event invalid
            {
                get;
                set;
            }

            public dom.KeyboardEvent keydown
            {
                get;
                set;
            }

            public dom.KeyboardEvent keypress
            {
                get;
                set;
            }

            public dom.KeyboardEvent keyup
            {
                get;
                set;
            }

            public dom.Event load
            {
                get;
                set;
            }

            public dom.Event loadeddata
            {
                get;
                set;
            }

            public dom.Event loadedmetadata
            {
                get;
                set;
            }

            public dom.Event loadstart
            {
                get;
                set;
            }

            public dom.MouseEvent mousedown
            {
                get;
                set;
            }

            public dom.MouseEvent mouseenter
            {
                get;
                set;
            }

            public dom.MouseEvent mouseleave
            {
                get;
                set;
            }

            public dom.MouseEvent mousemove
            {
                get;
                set;
            }

            public dom.MouseEvent mouseout
            {
                get;
                set;
            }

            public dom.MouseEvent mouseover
            {
                get;
                set;
            }

            public dom.MouseEvent mouseup
            {
                get;
                set;
            }

            public dom.WheelEvent mousewheel
            {
                get;
                set;
            }

            public dom.Event MSContentZoom
            {
                get;
                set;
            }

            public dom.Event MSManipulationStateChanged
            {
                get;
                set;
            }

            public dom.ClipboardEvent paste
            {
                get;
                set;
            }

            public dom.Event pause
            {
                get;
                set;
            }

            public dom.Event play
            {
                get;
                set;
            }

            public dom.Event playing
            {
                get;
                set;
            }

            public dom.ProgressEvent progress
            {
                get;
                set;
            }

            public dom.Event ratechange
            {
                get;
                set;
            }

            public dom.Event reset
            {
                get;
                set;
            }

            public dom.UIEvent scroll
            {
                get;
                set;
            }

            public dom.Event seeked
            {
                get;
                set;
            }

            public dom.Event seeking
            {
                get;
                set;
            }

            public dom.UIEvent select
            {
                get;
                set;
            }

            public dom.Event selectstart
            {
                get;
                set;
            }

            public dom.Event stalled
            {
                get;
                set;
            }

            public dom.Event submit
            {
                get;
                set;
            }

            public dom.Event suspend
            {
                get;
                set;
            }

            public dom.Event timeupdate
            {
                get;
                set;
            }

            public dom.Event volumechange
            {
                get;
                set;
            }

            public dom.Event waiting
            {
                get;
                set;
            }

            [Generated]
            public new static class KeyOf
            {
                [Template("\"abort\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> abort;
                [Template("\"activate\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> activate;
                [Template("\"beforeactivate\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> beforeactivate;
                [Template("\"beforecopy\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> beforecopy;
                [Template("\"beforecut\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> beforecut;
                [Template("\"beforedeactivate\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> beforedeactivate;
                [Template("\"beforepaste\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> beforepaste;
                [Template("\"blur\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> blur;
                [Template("\"canplay\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> canplay;
                [Template("\"canplaythrough\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> canplaythrough;
                [Template("\"change\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> change;
                [Template("\"click\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> click;
                [Template("\"contextmenu\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> contextmenu;
                [Template("\"copy\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> copy;
                [Template("\"cuechange\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> cuechange;
                [Template("\"cut\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> cut;
                [Template("\"dblclick\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> dblclick;
                [Template("\"deactivate\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> deactivate;
                [Template("\"drag\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> drag;
                [Template("\"dragend\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> dragend;
                [Template("\"dragenter\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> dragenter;
                [Template("\"dragleave\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> dragleave;
                [Template("\"dragover\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> dragover;
                [Template("\"dragstart\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> dragstart;
                [Template("\"drop\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> drop;
                [Template("\"durationchange\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> durationchange;
                [Template("\"emptied\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> emptied;
                [Template("\"ended\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> ended;
                [Template("\"error\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> error;
                [Template("\"focus\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> focus;
                [Template("\"input\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> input;
                [Template("\"invalid\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> invalid;
                [Template("\"keydown\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> keydown;
                [Template("\"keypress\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> keypress;
                [Template("\"keyup\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> keyup;
                [Template("\"load\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> load;
                [Template("\"loadeddata\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> loadeddata;
                [Template("\"loadedmetadata\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> loadedmetadata;
                [Template("\"loadstart\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> loadstart;
                [Template("\"mousedown\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> mousedown;
                [Template("\"mouseenter\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> mouseenter;
                [Template("\"mouseleave\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> mouseleave;
                [Template("\"mousemove\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> mousemove;
                [Template("\"mouseout\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> mouseout;
                [Template("\"mouseover\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> mouseover;
                [Template("\"mouseup\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> mouseup;
                [Template("\"mousewheel\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> mousewheel;
                [Template("\"MSContentZoom\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> MSContentZoom;
                [Template("\"MSManipulationStateChanged\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> MSManipulationStateChanged;
                [Template("\"paste\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> paste;
                [Template("\"pause\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> pause;
                [Template("\"play\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> play;
                [Template("\"playing\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> playing;
                [Template("\"progress\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> progress;
                [Template("\"ratechange\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> ratechange;
                [Template("\"reset\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> reset;
                [Template("\"scroll\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> scroll;
                [Template("\"seeked\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> seeked;
                [Template("\"seeking\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> seeking;
                [Template("\"select\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> select;
                [Template("\"selectstart\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> selectstart;
                [Template("\"stalled\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> stalled;
                [Template("\"submit\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> submit;
                [Template("\"suspend\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> suspend;
                [Template("\"timeupdate\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> timeupdate;
                [Template("\"volumechange\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> volumechange;
                [Template("\"waiting\"")]
                public static readonly KeyOf<dom.HTMLElementEventMap> waiting;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLElement : dom.Element, dom.ElementCSSInlineStyle.Interface, IObject
        {





























            public static dom.HTMLElement prototype
            {
                get;
                set;
            }

            public virtual string accessKey
            {
                get;
                set;
            }

            public virtual string contentEditable
            {
                get;
                set;
            }

            public virtual dom.DOMStringMap dataset
            {
                get;
            }

            public virtual string dir
            {
                get;
                set;
            }

            public virtual bool draggable
            {
                get;
                set;
            }

            public virtual bool hidden
            {
                get;
                set;
            }

            public virtual bool hideFocus
            {
                get;
                set;
            }

            public virtual string innerText
            {
                get;
                set;
            }

            public virtual bool isContentEditable
            {
                get;
            }

            public virtual string lang
            {
                get;
                set;
            }

            public virtual int offsetHeight
            {
                get;
            }

            public virtual int offsetLeft
            {
                get;
            }

            public virtual dom.Element offsetParent
            {
                get;
            }

            public virtual int offsetTop
            {
                get;
            }

            public virtual int offsetWidth
            {
                get;
            }

            public virtual dom.HTMLElement.onabortFn onabort
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onactivateFn onactivate
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onactivateFn onbeforeactivate
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onactivateFn onbeforecopy
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onactivateFn onbeforecut
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onactivateFn onbeforedeactivate
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onactivateFn onbeforepaste
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onblurFn onblur
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onactivateFn oncanplay
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onactivateFn oncanplaythrough
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onactivateFn onchange
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onclickFn onclick
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onclickFn onauxclick
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.oncontextmenuFn oncontextmenu
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.oncopyFn oncopy
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onactivateFn oncuechange
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.oncopyFn oncut
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onclickFn ondblclick
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onactivateFn ondeactivate
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.ondragFn ondrag
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.ondragFn ondragend
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.ondragFn ondragenter
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.ondragFn ondragleave
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.ondragFn ondragover
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.ondragFn ondragstart
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.ondragFn ondrop
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onactivateFn ondurationchange
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onactivateFn onemptied
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onactivateFn onended
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onerrorFn onerror
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onblurFn onfocus
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onactivateFn oninput
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onactivateFn oninvalid
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onkeydownFn onkeydown
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onkeydownFn onkeypress
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onkeydownFn onkeyup
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onactivateFn onload
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onactivateFn onloadeddata
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onactivateFn onloadedmetadata
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onactivateFn onloadstart
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onclickFn onmousedown
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onclickFn onmouseenter
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onclickFn onmouseleave
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onclickFn onmousemove
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onclickFn onmouseout
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onclickFn onmouseover
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onclickFn onmouseup
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onmousewheelFn onmousewheel
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onactivateFn onmscontentzoom
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onactivateFn onmsmanipulationstatechanged
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.oncopyFn onpaste
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onactivateFn onpause
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onactivateFn onplay
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onactivateFn onplaying
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onprogressFn onprogress
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onactivateFn onratechange
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onactivateFn onreset
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onabortFn onscroll
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onactivateFn onseeked
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onactivateFn onseeking
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onabortFn onselect
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onactivateFn onselectstart
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onactivateFn onstalled
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onactivateFn onsubmit
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onactivateFn onsuspend
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onactivateFn ontimeupdate
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onactivateFn onvolumechange
            {
                get;
                set;
            }

            public virtual dom.HTMLElement.onactivateFn onwaiting
            {
                get;
                set;
            }

            public virtual string outerText
            {
                get;
                set;
            }

            public virtual bool spellcheck
            {
                get;
                set;
            }

            public virtual int tabIndex
            {
                get;
                set;
            }

            public virtual string title
            {
                get;
                set;
            }

            public virtual extern dom.Animation animate(
              Union<dom.AnimationKeyFrame, dom.AnimationKeyFrame[]> keyframes,
              Union<double, dom.AnimationOptions> options);

            public virtual extern dom.Animation animate(
              dom.AnimationKeyFrame keyframes,
              double options);

            public virtual extern dom.Animation animate(
              dom.AnimationKeyFrame keyframes,
              dom.AnimationOptions options);

            public virtual extern dom.Animation animate(
              dom.AnimationKeyFrame[] keyframes,
              double options);

            public virtual extern dom.Animation animate(
              dom.AnimationKeyFrame[] keyframes,
              dom.AnimationOptions options);

            public virtual extern void blur();

            public virtual extern void click();

            public virtual extern bool dragDrop();

            public virtual extern void focus();

            public virtual extern dom.MSInputMethodContext msGetInputContext();

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual extern void addEventListener(string type, Action listenerFn);

            [Where("T", typeof(dom.Event), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<T>(string type, Action<T> listenerFn);

            public virtual dom.CSSStyleDeclaration style
            {
                get;
            }

            [Generated]
            public delegate void onabortFn(dom.UIEvent ev);

            [Generated]
            public delegate void onactivateFn(dom.Event ev);

            [Generated]
            public delegate void onblurFn(dom.FocusEvent ev);

            [Generated]
            public delegate void onclickFn(dom.MouseEvent ev);

            [Generated]
            public delegate void oncontextmenuFn(dom.PointerEvent ev);

            [Generated]
            public delegate void oncopyFn(dom.ClipboardEvent ev);

            [Generated]
            public delegate void ondragFn(dom.DragEvent ev);

            [Generated]
            public delegate void onerrorFn(dom.ErrorEvent ev);

            [Generated]
            public delegate void onkeydownFn(dom.KeyboardEvent ev);

            [Generated]
            public delegate void onmousewheelFn(dom.WheelEvent ev);

            [Generated]
            public delegate void onprogressFn(dom.ProgressEvent ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLEmbedElement : dom.HTMLElement, dom.GetSVGDocument.Interface, IObject
        {





            [Template("document.createElement(\"embed\")")]
            public extern HTMLEmbedElement();

            public static dom.HTMLEmbedElement prototype
            {
                get;
                set;
            }

            public virtual string height
            {
                get;
                set;
            }

            public virtual object hidden
            {
                get;
                set;
            }

            public virtual bool msPlayToDisabled
            {
                get;
                set;
            }

            public virtual string msPlayToPreferredSourceUri
            {
                get;
                set;
            }

            public virtual bool msPlayToPrimary
            {
                get;
                set;
            }

            public virtual object msPlayToSource
            {
                get;
            }

            public virtual string name
            {
                get;
                set;
            }

            public virtual string palette
            {
                get;
            }

            public virtual string pluginspage
            {
                get;
            }

            public virtual string readyState
            {
                get;
            }

            public virtual string src
            {
                get;
                set;
            }

            public virtual string units
            {
                get;
                set;
            }

            public virtual string width
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLEmbedElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLEmbedElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLEmbedElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLEmbedElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLEmbedElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLEmbedElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLEmbedElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLEmbedElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual extern dom.Document getSVGDocument();

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLFieldSetElement : dom.HTMLElement
        {



            [Template("document.createElement(\"fieldset\")")]
            public extern HTMLFieldSetElement();

            public static dom.HTMLFieldSetElement prototype
            {
                get;
                set;
            }

            public virtual string align
            {
                get;
                set;
            }

            public virtual bool disabled
            {
                get;
                set;
            }

            public virtual dom.HTMLFormElement form
            {
                get;
            }

            public virtual string name
            {
                get;
                set;
            }

            public virtual string validationMessage
            {
                get;
            }

            public virtual dom.ValidityState validity
            {
                get;
            }

            public virtual bool willValidate
            {
                get;
            }

            public virtual extern bool checkValidity();

            public virtual extern void setCustomValidity(string error);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLFieldSetElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLFieldSetElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLFieldSetElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLFieldSetElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLFieldSetElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLFieldSetElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLFieldSetElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLFieldSetElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLFontElement : dom.HTMLElement, dom.DOML2DeprecatedColorProperty.Interface, IObject, dom.DOML2DeprecatedSizeProperty.Interface
        {

            [Template("document.createElement(\"font\")")]
            public extern HTMLFontElement();

            public static dom.HTMLFontElement prototype
            {
                get;
                set;
            }

            public virtual string face
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLFontElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLFontElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLFontElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLFontElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLFontElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLFontElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLFontElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLFontElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual string color
            {
                get;
                set;
            }

            public virtual double size
            {
                get;
                set;
            }

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLFormControlsCollection : dom.HTMLCollectionBase
        {
            public static dom.HTMLFormControlsCollection prototype
            {
                get;
                set;
            }

            public virtual extern Union<dom.HTMLCollection, dom.Element, Null> namedItem(
              string name);

            public override extern uint length { get; }

            public override extern dom.Element item(uint index);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLFormElement : dom.HTMLElement
        {





            [Template("document.createElement(\"form\")")]
            public extern HTMLFormElement();

            public static dom.HTMLFormElement prototype
            {
                get;
                set;
            }

            public virtual string acceptCharset
            {
                get;
                set;
            }

            public virtual string action
            {
                get;
                set;
            }

            public virtual string autocomplete
            {
                get;
                set;
            }

            public virtual dom.HTMLFormControlsCollection elements
            {
                get;
            }

            public virtual string encoding
            {
                get;
                set;
            }

            public virtual string enctype
            {
                get;
                set;
            }

            public virtual uint length
            {
                get;
            }

            public virtual string method
            {
                get;
                set;
            }

            public virtual string name
            {
                get;
                set;
            }

            public virtual bool noValidate
            {
                get;
                set;
            }

            public virtual string target
            {
                get;
                set;
            }

            public virtual extern bool checkValidity();

            public virtual extern object item();

            public virtual extern object item(object name);

            public virtual extern object item(object name, object index);

            public virtual extern object namedItem(string name);

            public virtual extern bool reportValidity();

            public virtual extern void reset();

            public virtual extern void submit();

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLFormElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLFormElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLFormElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLFormElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLFormElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLFormElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLFormElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLFormElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class HTMLFrameElementEventMap : dom.HTMLElementEventMap
        {
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLFrameElement : dom.HTMLElement, dom.GetSVGDocument.Interface, IObject
        {





            [Template("document.createElement(\"frame\")")]
            public extern HTMLFrameElement();

            public static dom.HTMLFrameElement prototype
            {
                get;
                set;
            }

            public virtual string border
            {
                get;
                set;
            }

            public virtual object borderColor
            {
                get;
                set;
            }

            public virtual dom.Document contentDocument
            {
                get;
            }

            public virtual dom.Window contentWindow
            {
                get;
            }

            public virtual string frameBorder
            {
                get;
                set;
            }

            public virtual object frameSpacing
            {
                get;
                set;
            }

            public virtual Union<string, double> height
            {
                get;
                set;
            }

            public virtual string longDesc
            {
                get;
                set;
            }

            public virtual string marginHeight
            {
                get;
                set;
            }

            public virtual string marginWidth
            {
                get;
                set;
            }

            public virtual string name
            {
                get;
                set;
            }

            public virtual bool noResize
            {
                get;
                set;
            }

            public virtual string scrolling
            {
                get;
                set;
            }

            public virtual string src
            {
                get;
                set;
            }

            public virtual Union<string, double> width
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLFrameElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLFrameElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLFrameElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLFrameElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLFrameElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLFrameElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLFrameElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLFrameElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLFrameElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLFrameElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLFrameElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLFrameElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLFrameElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLFrameElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLFrameElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLFrameElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual extern dom.Document getSVGDocument();

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLFrameElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLFrameElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class HTMLFrameSetElementEventMap : dom.HTMLElementEventMap, dom.WindowEventHandlersEventMap.Interface, IObject
        {





            public dom.Event orientationchange
            {
                get;
                set;
            }

            public dom.UIEvent resize
            {
                get;
                set;
            }

            public dom.Event afterprint
            {
                get;
                set;
            }

            public dom.Event beforeprint
            {
                get;
                set;
            }

            public dom.BeforeUnloadEvent beforeunload
            {
                get;
                set;
            }

            public dom.HashChangeEvent hashchange
            {
                get;
                set;
            }

            public dom.MessageEvent message
            {
                get;
                set;
            }

            public dom.Event offline
            {
                get;
                set;
            }

            public dom.Event online
            {
                get;
                set;
            }

            public dom.PageTransitionEvent pagehide
            {
                get;
                set;
            }

            public dom.PageTransitionEvent pageshow
            {
                get;
                set;
            }

            public dom.PopStateEvent popstate
            {
                get;
                set;
            }

            public dom.StorageEvent storage
            {
                get;
                set;
            }

            public dom.Event unload
            {
                get;
                set;
            }

            [Generated]
            public new static class KeyOf
            {
                [Template("\"orientationchange\"")]
                public static readonly KeyOf<dom.HTMLFrameSetElementEventMap> orientationchange;
                [Template("\"resize\"")]
                public static readonly KeyOf<dom.HTMLFrameSetElementEventMap> resize;
                [Template("\"afterprint\"")]
                public static readonly KeyOf<dom.HTMLFrameSetElementEventMap> afterprint;
                [Template("\"beforeprint\"")]
                public static readonly KeyOf<dom.HTMLFrameSetElementEventMap> beforeprint;
                [Template("\"beforeunload\"")]
                public static readonly KeyOf<dom.HTMLFrameSetElementEventMap> beforeunload;
                [Template("\"hashchange\"")]
                public static readonly KeyOf<dom.HTMLFrameSetElementEventMap> hashchange;
                [Template("\"message\"")]
                public static readonly KeyOf<dom.HTMLFrameSetElementEventMap> message;
                [Template("\"offline\"")]
                public static readonly KeyOf<dom.HTMLFrameSetElementEventMap> offline;
                [Template("\"online\"")]
                public static readonly KeyOf<dom.HTMLFrameSetElementEventMap> online;
                [Template("\"pagehide\"")]
                public static readonly KeyOf<dom.HTMLFrameSetElementEventMap> pagehide;
                [Template("\"pageshow\"")]
                public static readonly KeyOf<dom.HTMLFrameSetElementEventMap> pageshow;
                [Template("\"popstate\"")]
                public static readonly KeyOf<dom.HTMLFrameSetElementEventMap> popstate;
                [Template("\"storage\"")]
                public static readonly KeyOf<dom.HTMLFrameSetElementEventMap> storage;
                [Template("\"unload\"")]
                public static readonly KeyOf<dom.HTMLFrameSetElementEventMap> unload;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLFrameSetElement : dom.HTMLElement, dom.WindowEventHandlers.Interface, IObject
        {







            [Template("document.createElement(\"frameset\")")]
            public extern HTMLFrameSetElement();

            public static dom.HTMLFrameSetElement prototype
            {
                get;
                set;
            }

            public virtual string cols
            {
                get;
                set;
            }

            public virtual string name
            {
                get;
                set;
            }

            public virtual dom.HTMLFrameSetElement.onorientationchangeFn onorientationchange
            {
                get;
                set;
            }

            public virtual dom.HTMLFrameSetElement.onresizeFn onresize
            {
                get;
                set;
            }

            public virtual string rows
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLFrameSetElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLFrameSetElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLFrameSetElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLFrameSetElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLFrameSetElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLFrameSetElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLFrameSetElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLFrameSetElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLFrameSetElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLFrameSetElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLFrameSetElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLFrameSetElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLFrameSetElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLFrameSetElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLFrameSetElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLFrameSetElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual dom.WindowEventHandlers.onafterprintFn onafterprint
            {
                get;
                set;
            }

            public virtual dom.WindowEventHandlers.onafterprintFn onbeforeprint
            {
                get;
                set;
            }

            public virtual dom.WindowEventHandlers.onbeforeunloadFn onbeforeunload
            {
                get;
                set;
            }

            public virtual dom.WindowEventHandlers.onhashchangeFn onhashchange
            {
                get;
                set;
            }

            public virtual dom.WindowEventHandlers.onmessageFn onmessage
            {
                get;
                set;
            }

            public virtual dom.WindowEventHandlers.onafterprintFn onoffline
            {
                get;
                set;
            }

            public virtual dom.WindowEventHandlers.onafterprintFn ononline
            {
                get;
                set;
            }

            public virtual dom.WindowEventHandlers.onpagehideFn onpagehide
            {
                get;
                set;
            }

            public virtual dom.WindowEventHandlers.onpagehideFn onpageshow
            {
                get;
                set;
            }

            public virtual dom.WindowEventHandlers.onpopstateFn onpopstate
            {
                get;
                set;
            }

            public virtual dom.WindowEventHandlers.onstorageFn onstorage
            {
                get;
                set;
            }

            public virtual dom.WindowEventHandlers.onafterprintFn onunload
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.WindowEventHandlersEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.WindowEventHandlers.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.WindowEventHandlersEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.WindowEventHandlers.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.WindowEventHandlersEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.WindowEventHandlers.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.WindowEventHandlersEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.WindowEventHandlers.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.WindowEventHandlersEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.WindowEventHandlers.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.WindowEventHandlersEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.WindowEventHandlers.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.WindowEventHandlersEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.WindowEventHandlers.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.WindowEventHandlersEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.WindowEventHandlers.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            public delegate void onorientationchangeFn(dom.Event ev);

            [Generated]
            public delegate void onresizeFn(dom.UIEvent ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLFrameSetElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLFrameSetElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLHRElement : dom.HTMLElement, dom.DOML2DeprecatedColorProperty.Interface, IObject, dom.DOML2DeprecatedSizeProperty.Interface
        {



            [Template("document.createElement(\"hr\")")]
            public extern HTMLHRElement();

            public static dom.HTMLHRElement prototype
            {
                get;
                set;
            }

            public virtual string align
            {
                get;
                set;
            }

            public virtual bool noShade
            {
                get;
                set;
            }

            public virtual string width
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLHRElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLHRElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLHRElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLHRElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLHRElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLHRElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLHRElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLHRElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual string color
            {
                get;
                set;
            }

            public virtual double size
            {
                get;
                set;
            }

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLHeadElement : dom.HTMLElement
        {

            [Template("document.createElement(\"head\")")]
            public extern HTMLHeadElement();

            public static dom.HTMLHeadElement prototype
            {
                get;
                set;
            }

            public virtual string profile
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLHeadElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLHeadElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLHeadElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLHeadElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLHeadElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLHeadElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLHeadElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLHeadElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLHeadingElement : dom.HTMLElement
        {

            [Template("document.createElement(\"h1\")")]
            public extern HTMLHeadingElement();

            public static dom.HTMLHeadingElement prototype
            {
                get;
                set;
            }

            public virtual string align
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLHeadingElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLHeadingElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLHeadingElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLHeadingElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLHeadingElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLHeadingElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLHeadingElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLHeadingElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLHtmlElement : dom.HTMLElement
        {

            [Template("document.createElement(\"html\")")]
            public extern HTMLHtmlElement();

            public static dom.HTMLHtmlElement prototype
            {
                get;
                set;
            }

            public virtual string version
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLHtmlElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLHtmlElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLHtmlElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLHtmlElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLHtmlElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLHtmlElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLHtmlElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLHtmlElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [IgnoreCast]
        [Virtual]
        [InterfaceWrapper]
        public abstract class HTMLHyperlinkElementUtils : dom.HTMLHyperlinkElementUtils.Interface, IObject
        {




            public virtual string hash
            {
                get;
                set;
            }

            public virtual string host
            {
                get;
                set;
            }

            public virtual string hostname
            {
                get;
                set;
            }

            public virtual string href
            {
                get;
                set;
            }

            public virtual string origin
            {
                get;
                set;
            }

            public virtual string pathname
            {
                get;
                set;
            }

            public virtual string port
            {
                get;
                set;
            }

            public virtual string protocol
            {
                get;
                set;
            }

            public virtual string search
            {
                get;
                set;
            }

            public abstract string toString();

            [Generated]
            [IgnoreCast]
            public interface Interface : IObject
            {
                string hash { get; set; }

                string host { get; set; }

                string hostname { get; set; }

                string href { get; set; }

                string origin { get; set; }

                string pathname { get; set; }

                string port { get; set; }

                string protocol { get; set; }

                string search { get; set; }

                string toString();
            }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class HTMLIFrameElementEventMap : dom.HTMLElementEventMap
        {
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLIFrameElement : dom.HTMLElement, dom.GetSVGDocument.Interface, IObject
        {






            [Template("document.createElement(\"iframe\")")]
            public extern HTMLIFrameElement();

            public static dom.HTMLIFrameElement prototype
            {
                get;
                set;
            }

            public virtual string align
            {
                get;
                set;
            }

            public virtual bool allowFullscreen
            {
                get;
                set;
            }

            public virtual bool allowPaymentRequest
            {
                get;
                set;
            }

            public virtual dom.Document contentDocument
            {
                get;
            }

            public virtual dom.Window contentWindow
            {
                get;
            }

            public virtual string frameBorder
            {
                get;
                set;
            }

            public virtual string height
            {
                get;
                set;
            }

            public virtual string longDesc
            {
                get;
                set;
            }

            public virtual string marginHeight
            {
                get;
                set;
            }

            public virtual string marginWidth
            {
                get;
                set;
            }

            public virtual string name
            {
                get;
                set;
            }

            public virtual dom.DOMTokenList sandbox
            {
                get;
            }

            public virtual string scrolling
            {
                get;
                set;
            }

            public virtual string src
            {
                get;
                set;
            }

            public virtual string srcdoc
            {
                get;
                set;
            }

            public virtual string width
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLIFrameElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLIFrameElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLIFrameElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLIFrameElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLIFrameElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLIFrameElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLIFrameElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLIFrameElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLIFrameElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLIFrameElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLIFrameElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLIFrameElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLIFrameElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLIFrameElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLIFrameElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLIFrameElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual extern dom.Document getSVGDocument();

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLIFrameElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLIFrameElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLImageElement : dom.HTMLElement
        {









            [Template("document.createElement(\"img\")")]
            public extern HTMLImageElement();

            public static dom.HTMLImageElement prototype
            {
                get;
                set;
            }

            public virtual string align
            {
                get;
                set;
            }

            public virtual string alt
            {
                get;
                set;
            }

            public virtual string border
            {
                get;
                set;
            }

            public virtual bool complete
            {
                get;
            }

            public virtual string crossOrigin
            {
                get;
                set;
            }

            public virtual string currentSrc
            {
                get;
            }

            public virtual dom.Literals.Options.decoding decoding
            {
                get;
                set;
            }

            public virtual uint height
            {
                get;
                set;
            }

            public virtual uint hspace
            {
                get;
                set;
            }

            public virtual bool isMap
            {
                get;
                set;
            }

            public virtual string longDesc
            {
                get;
                set;
            }

            public virtual string lowsrc
            {
                get;
                set;
            }

            public virtual bool msPlayToDisabled
            {
                get;
                set;
            }

            public virtual string msPlayToPreferredSourceUri
            {
                get;
                set;
            }

            public virtual bool msPlayToPrimary
            {
                get;
                set;
            }

            public virtual object msPlayToSource
            {
                get;
            }

            public virtual string name
            {
                get;
                set;
            }

            public virtual uint naturalHeight
            {
                get;
            }

            public virtual uint naturalWidth
            {
                get;
            }

            public virtual string sizes
            {
                get;
                set;
            }

            public virtual string src
            {
                get;
                set;
            }

            public virtual string srcset
            {
                get;
                set;
            }

            public virtual string useMap
            {
                get;
                set;
            }

            public virtual uint vspace
            {
                get;
                set;
            }

            public virtual uint width
            {
                get;
                set;
            }

            public virtual int x
            {
                get;
            }

            public virtual int y
            {
                get;
            }

            public virtual extern object msGetAsCastingSource();

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLImageElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLImageElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLImageElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLImageElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLImageElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLImageElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLImageElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLImageElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLInputElement : dom.HTMLElement
        {















            [Template("document.createElement(\"input\")")]
            public extern HTMLInputElement();

            public static dom.HTMLInputElement prototype
            {
                get;
                set;
            }

            public virtual string accept
            {
                get;
                set;
            }

            public virtual string align
            {
                get;
                set;
            }

            public virtual string alt
            {
                get;
                set;
            }

            public virtual string autocomplete
            {
                get;
                set;
            }

            public virtual bool autofocus
            {
                get;
                set;
            }

            [Name("checked")]
            public virtual bool @checked
            {
                get;
                set;
            }

            public virtual bool defaultChecked
            {
                get;
                set;
            }

            public virtual string defaultValue
            {
                get;
                set;
            }

            public virtual bool disabled
            {
                get;
                set;
            }

            public virtual dom.FileList files
            {
                get;
            }

            public virtual dom.HTMLFormElement form
            {
                get;
            }

            public virtual string formAction
            {
                get;
                set;
            }

            public virtual string formEnctype
            {
                get;
                set;
            }

            public virtual string formMethod
            {
                get;
                set;
            }

            public virtual bool formNoValidate
            {
                get;
                set;
            }

            public virtual string formTarget
            {
                get;
                set;
            }

            public virtual uint height
            {
                get;
                set;
            }

            public virtual bool indeterminate
            {
                get;
                set;
            }

            public virtual dom.HTMLElement list
            {
                get;
            }

            public virtual string max
            {
                get;
                set;
            }

            public virtual int maxLength
            {
                get;
                set;
            }

            public virtual string min
            {
                get;
                set;
            }

            public virtual int minLength
            {
                get;
                set;
            }

            public virtual bool multiple
            {
                get;
                set;
            }

            public virtual string name
            {
                get;
                set;
            }

            public virtual string pattern
            {
                get;
                set;
            }

            public virtual string placeholder
            {
                get;
                set;
            }

            public virtual bool readOnly
            {
                get;
                set;
            }

            public virtual bool required
            {
                get;
                set;
            }

            public virtual string selectionDirection
            {
                get;
                set;
            }

            public virtual uint? selectionEnd
            {
                get;
                set;
            }

            public virtual uint? selectionStart
            {
                get;
                set;
            }

            public virtual uint size
            {
                get;
                set;
            }

            public virtual string src
            {
                get;
                set;
            }

            public virtual string step
            {
                get;
                set;
            }

            public virtual string type
            {
                get;
                set;
            }

            public virtual string useMap
            {
                get;
                set;
            }

            public virtual string validationMessage
            {
                get;
            }

            public virtual dom.ValidityState validity
            {
                get;
            }

            public virtual string value
            {
                get;
                set;
            }

            public virtual object valueAsDate
            {
                get;
                set;
            }

            public virtual double valueAsNumber
            {
                get;
                set;
            }

            public virtual bool webkitdirectory
            {
                get;
                set;
            }

            public virtual uint width
            {
                get;
                set;
            }

            public virtual bool willValidate
            {
                get;
            }

            public virtual extern bool checkValidity();

            public virtual extern void select();

            public virtual extern void setCustomValidity(string error);

            public virtual extern void setSelectionRange(uint start, uint end);

            public virtual extern void setSelectionRange(
              uint start,
              uint end,
              dom.Literals.Options.direction2 direction);

            public virtual extern void stepDown();

            public virtual extern void stepDown(int n);

            public virtual extern void stepUp();

            public virtual extern void stepUp(int n);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLInputElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLInputElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLInputElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLInputElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLInputElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLInputElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLInputElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLInputElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLLIElement : dom.HTMLElement
        {


            [Template("document.createElement(\"li\")")]
            public extern HTMLLIElement();

            public static dom.HTMLLIElement prototype
            {
                get;
                set;
            }

            public virtual string type
            {
                get;
                set;
            }

            public virtual int value
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLLIElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLLIElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLLIElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLLIElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLLIElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLLIElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLLIElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLLIElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLLabelElement : dom.HTMLElement
        {

            [Template("document.createElement(\"label\")")]
            public extern HTMLLabelElement();

            public static dom.HTMLLabelElement prototype
            {
                get;
                set;
            }

            public virtual dom.HTMLInputElement control
            {
                get;
            }

            public virtual dom.HTMLFormElement form
            {
                get;
            }

            public virtual string htmlFor
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLLabelElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLLabelElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLLabelElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLLabelElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLLabelElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLLabelElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLLabelElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLLabelElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLLegendElement : dom.HTMLElement
        {


            [Template("document.createElement(\"legend\")")]
            public extern HTMLLegendElement();

            public static dom.HTMLLegendElement prototype
            {
                get;
                set;
            }

            public virtual string align
            {
                get;
                set;
            }

            public virtual dom.HTMLFormElement form
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLLegendElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLLegendElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLLegendElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLLegendElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLLegendElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLLegendElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLLegendElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLLegendElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLLinkElement : dom.HTMLElement, dom.LinkStyle.Interface, IObject
        {





            [Template("document.createElement(\"link\")")]
            public extern HTMLLinkElement();

            public static dom.HTMLLinkElement prototype
            {
                get;
                set;
            }

            public virtual string charset
            {
                get;
                set;
            }

            public virtual string crossOrigin
            {
                get;
                set;
            }

            public virtual bool disabled
            {
                get;
                set;
            }

            public virtual string href
            {
                get;
                set;
            }

            public virtual string hreflang
            {
                get;
                set;
            }

            public virtual dom.Document import
            {
                get;
                set;
            }

            public virtual string integrity
            {
                get;
                set;
            }

            public virtual string media
            {
                get;
                set;
            }

            public virtual string rel
            {
                get;
                set;
            }

            public virtual string rev
            {
                get;
                set;
            }

            public virtual string target
            {
                get;
                set;
            }

            public virtual string type
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLLinkElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLLinkElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLLinkElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLLinkElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLLinkElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLLinkElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLLinkElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLLinkElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual dom.StyleSheet sheet
            {
                get;
            }

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLMainElement : dom.HTMLElement
        {
            public static dom.HTMLMainElement prototype
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLMainElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLMainElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLMainElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLMainElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLMainElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLMainElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLMainElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLMainElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLMapElement : dom.HTMLElement
        {


            [Template("document.createElement(\"map\")")]
            public extern HTMLMapElement();

            public static dom.HTMLMapElement prototype
            {
                get;
                set;
            }

            public virtual dom.HTMLAreasCollection areas
            {
                get;
            }

            public virtual string name
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLMapElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLMapElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLMapElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLMapElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLMapElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLMapElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLMapElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLMapElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class HTMLMarqueeElementEventMap : dom.HTMLElementEventMap
        {


            public dom.Event bounce
            {
                get;
                set;
            }

            public dom.Event finish
            {
                get;
                set;
            }

            public dom.Event start
            {
                get;
                set;
            }

            [Generated]
            public new static class KeyOf
            {
                [Template("\"bounce\"")]
                public static readonly KeyOf<dom.HTMLMarqueeElementEventMap> bounce;
                [Template("\"finish\"")]
                public static readonly KeyOf<dom.HTMLMarqueeElementEventMap> finish;
                [Template("\"start\"")]
                public static readonly KeyOf<dom.HTMLMarqueeElementEventMap> start;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLMarqueeElement : dom.HTMLElement
        {






            [Template("document.createElement(\"marquee\")")]
            public extern HTMLMarqueeElement();

            public static dom.HTMLMarqueeElement prototype
            {
                get;
                set;
            }

            public virtual string behavior
            {
                get;
                set;
            }

            public virtual string bgColor
            {
                get;
                set;
            }

            public virtual string direction
            {
                get;
                set;
            }

            public virtual string height
            {
                get;
                set;
            }

            public virtual uint hspace
            {
                get;
                set;
            }

            public virtual int loop
            {
                get;
                set;
            }

            public virtual dom.HTMLMarqueeElement.onbounceFn onbounce
            {
                get;
                set;
            }

            public virtual dom.HTMLMarqueeElement.onbounceFn onfinish
            {
                get;
                set;
            }

            public virtual dom.HTMLMarqueeElement.onbounceFn onstart
            {
                get;
                set;
            }

            public virtual uint scrollAmount
            {
                get;
                set;
            }

            public virtual uint scrollDelay
            {
                get;
                set;
            }

            public virtual bool trueSpeed
            {
                get;
                set;
            }

            public virtual uint vspace
            {
                get;
                set;
            }

            public virtual string width
            {
                get;
                set;
            }

            public virtual extern void start();

            public virtual extern void stop();

            [Where("K", typeof(KeyOf<dom.HTMLMarqueeElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLMarqueeElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLMarqueeElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLMarqueeElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLMarqueeElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLMarqueeElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLMarqueeElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLMarqueeElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLMarqueeElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLMarqueeElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLMarqueeElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLMarqueeElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLMarqueeElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLMarqueeElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLMarqueeElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLMarqueeElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            public delegate void onbounceFn(dom.Event ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLMarqueeElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLMarqueeElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class HTMLMediaElementEventMap : dom.HTMLElementEventMap
        {

            public dom.MediaEncryptedEvent encrypted
            {
                get;
                set;
            }

            public dom.Event msneedkey
            {
                get;
                set;
            }

            [Generated]
            public new static class KeyOf
            {
                [Template("\"encrypted\"")]
                public static readonly KeyOf<dom.HTMLMediaElementEventMap> encrypted;
                [Template("\"msneedkey\"")]
                public static readonly KeyOf<dom.HTMLMediaElementEventMap> msneedkey;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLMediaElement : dom.HTMLElement
        {



















            public static dom.HTMLMediaElement prototype
            {
                get;
                set;
            }

            [Name("HAVE_CURRENT_DATA")]
            public static ushort HAVE_CURRENT_DATA_Static
            {
                get;
            }

            [Name("HAVE_ENOUGH_DATA")]
            public static ushort HAVE_ENOUGH_DATA_Static
            {
                get;
            }

            [Name("HAVE_FUTURE_DATA")]
            public static ushort HAVE_FUTURE_DATA_Static
            {
                get;
            }

            [Name("HAVE_METADATA")]
            public static ushort HAVE_METADATA_Static
            {
                get;
            }

            [Name("HAVE_NOTHING")]
            public static ushort HAVE_NOTHING_Static
            {
                get;
            }

            [Name("NETWORK_EMPTY")]
            public static ushort NETWORK_EMPTY_Static
            {
                get;
            }

            [Name("NETWORK_IDLE")]
            public static ushort NETWORK_IDLE_Static
            {
                get;
            }

            [Name("NETWORK_LOADING")]
            public static ushort NETWORK_LOADING_Static
            {
                get;
            }

            [Name("NETWORK_NO_SOURCE")]
            public static ushort NETWORK_NO_SOURCE_Static
            {
                get;
            }

            public virtual dom.AudioTrackList audioTracks
            {
                get;
            }

            public virtual bool autoplay
            {
                get;
                set;
            }

            public virtual dom.TimeRanges buffered
            {
                get;
            }

            public virtual bool controls
            {
                get;
                set;
            }

            public virtual string crossOrigin
            {
                get;
                set;
            }

            public virtual string currentSrc
            {
                get;
            }

            public virtual double currentTime
            {
                get;
                set;
            }

            public virtual bool defaultMuted
            {
                get;
                set;
            }

            public virtual double defaultPlaybackRate
            {
                get;
                set;
            }

            public virtual double duration
            {
                get;
            }

            public virtual bool ended
            {
                get;
            }

            public virtual dom.MediaError error
            {
                get;
            }

            public virtual bool loop
            {
                get;
                set;
            }

            public virtual dom.MediaKeys mediaKeys
            {
                get;
            }

            public virtual string msAudioCategory
            {
                get;
                set;
            }

            public virtual string msAudioDeviceType
            {
                get;
                set;
            }

            public virtual dom.MSGraphicsTrust msGraphicsTrustStatus
            {
                get;
            }

            public virtual dom.MSMediaKeys msKeys
            {
                get;
            }

            public virtual bool msPlayToDisabled
            {
                get;
                set;
            }

            public virtual string msPlayToPreferredSourceUri
            {
                get;
                set;
            }

            public virtual bool msPlayToPrimary
            {
                get;
                set;
            }

            public virtual object msPlayToSource
            {
                get;
            }

            public virtual bool msRealTime
            {
                get;
                set;
            }

            public virtual bool muted
            {
                get;
                set;
            }

            public virtual ushort networkState
            {
                get;
            }

            public virtual dom.HTMLMediaElement.onencryptedFn onencrypted
            {
                get;
                set;
            }

            public virtual dom.HTMLMediaElement.onmsneedkeyFn onmsneedkey
            {
                get;
                set;
            }

            public virtual bool paused
            {
                get;
            }

            public virtual double playbackRate
            {
                get;
                set;
            }

            public virtual dom.TimeRanges played
            {
                get;
            }

            public virtual string preload
            {
                get;
                set;
            }

            public virtual ushort readyState
            {
                get;
            }

            public virtual dom.TimeRanges seekable
            {
                get;
            }

            public virtual bool seeking
            {
                get;
            }

            public virtual string src
            {
                get;
                set;
            }

            public virtual Union<dom.MediaStream, dom.MediaSource, dom.Blob, Null> srcObject
            {
                get;
                set;
            }

            public virtual dom.TextTrackList textTracks
            {
                get;
            }

            public virtual dom.VideoTrackList videoTracks
            {
                get;
            }

            public virtual double volume
            {
                get;
                set;
            }

            public virtual extern dom.TextTrack addTextTrack(dom.TextTrackKind kind);

            public virtual extern dom.TextTrack addTextTrack(dom.TextTrackKind kind, string label);

            public virtual extern dom.TextTrack addTextTrack(
              dom.TextTrackKind kind,
              string label,
              string language);

            public virtual extern dom.CanPlayTypeResult canPlayType(string type);

            public virtual extern void load();

            public virtual extern void msClearEffects();

            public virtual extern object msGetAsCastingSource();

            public virtual extern void msInsertAudioEffect(string activatableClassId, bool effectRequired);

            public virtual extern void msInsertAudioEffect(
              string activatableClassId,
              bool effectRequired,
              object config);

            public virtual extern void msSetMediaKeys(dom.MSMediaKeys mediaKeys);

            public virtual extern void msSetMediaProtectionManager();

            public virtual extern void msSetMediaProtectionManager(object mediaProtectionManager);

            public virtual extern void pause();

            public virtual extern es5.Promise<H5.Core.Void> play();

            public virtual extern es5.Promise<H5.Core.Void> setMediaKeys(
              dom.MediaKeys mediaKeys);

            public virtual ushort HAVE_CURRENT_DATA
            {
                get;
            }

            public virtual ushort HAVE_ENOUGH_DATA
            {
                get;
            }

            public virtual ushort HAVE_FUTURE_DATA
            {
                get;
            }

            public virtual ushort HAVE_METADATA
            {
                get;
            }

            public virtual ushort HAVE_NOTHING
            {
                get;
            }

            public virtual ushort NETWORK_EMPTY
            {
                get;
            }

            public virtual ushort NETWORK_IDLE
            {
                get;
            }

            public virtual ushort NETWORK_LOADING
            {
                get;
            }

            public virtual ushort NETWORK_NO_SOURCE
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.HTMLMediaElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLMediaElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLMediaElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLMediaElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLMediaElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLMediaElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLMediaElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLMediaElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLMediaElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLMediaElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLMediaElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLMediaElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLMediaElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLMediaElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLMediaElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLMediaElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            public delegate void onencryptedFn(dom.MediaEncryptedEvent ev);

            [Generated]
            public delegate void onmsneedkeyFn(dom.Event ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLMediaElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLMediaElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLMenuElement : dom.HTMLElement
        {


            [Template("document.createElement(\"menu\")")]
            public extern HTMLMenuElement();

            public static dom.HTMLMenuElement prototype
            {
                get;
                set;
            }

            public virtual bool compact
            {
                get;
                set;
            }

            public virtual string type
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLMenuElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLMenuElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLMenuElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLMenuElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLMenuElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLMenuElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLMenuElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLMenuElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLMetaElement : dom.HTMLElement
        {


            [Template("document.createElement(\"meta\")")]
            public extern HTMLMetaElement();

            public static dom.HTMLMetaElement prototype
            {
                get;
                set;
            }

            public virtual string charset
            {
                get;
                set;
            }

            public virtual string content
            {
                get;
                set;
            }

            public virtual string httpEquiv
            {
                get;
                set;
            }

            public virtual string name
            {
                get;
                set;
            }

            public virtual string scheme
            {
                get;
                set;
            }

            public virtual string url
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLMetaElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLMetaElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLMetaElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLMetaElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLMetaElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLMetaElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLMetaElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLMetaElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLMeterElement : dom.HTMLElement
        {


            [Template("document.createElement(\"meter\")")]
            public extern HTMLMeterElement();

            public static dom.HTMLMeterElement prototype
            {
                get;
                set;
            }

            public virtual double high
            {
                get;
                set;
            }

            public virtual double low
            {
                get;
                set;
            }

            public virtual double max
            {
                get;
                set;
            }

            public virtual double min
            {
                get;
                set;
            }

            public virtual double optimum
            {
                get;
                set;
            }

            public virtual double value
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLMeterElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLMeterElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLMeterElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLMeterElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLMeterElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLMeterElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLMeterElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLMeterElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLModElement : dom.HTMLElement
        {


            [Template("document.createElement(\"del\")")]
            public extern HTMLModElement();

            public static dom.HTMLModElement prototype
            {
                get;
                set;
            }

            public virtual string cite
            {
                get;
                set;
            }

            public virtual string dateTime
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLModElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLModElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLModElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLModElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLModElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLModElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLModElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLModElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLOListElement : dom.HTMLElement
        {

            [Template("document.createElement(\"ol\")")]
            public extern HTMLOListElement();

            public static dom.HTMLOListElement prototype
            {
                get;
                set;
            }

            public virtual bool compact
            {
                get;
                set;
            }

            public virtual int start
            {
                get;
                set;
            }

            public virtual string type
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLOListElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLOListElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLOListElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLOListElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLOListElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLOListElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLOListElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLOListElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLObjectElement : dom.HTMLElement, dom.GetSVGDocument.Interface, IObject
        {










            [Template("document.createElement(\"object\")")]
            public extern HTMLObjectElement();

            public static dom.HTMLObjectElement prototype
            {
                get;
                set;
            }

            public virtual string BaseHref
            {
                get;
            }

            public virtual string align
            {
                get;
                set;
            }

            public virtual string archive
            {
                get;
                set;
            }

            public virtual string border
            {
                get;
                set;
            }

            public virtual string code
            {
                get;
                set;
            }

            public virtual string codeBase
            {
                get;
                set;
            }

            public virtual string codeType
            {
                get;
                set;
            }

            public virtual dom.Document contentDocument
            {
                get;
            }

            public virtual string data
            {
                get;
                set;
            }

            public virtual bool declare
            {
                get;
                set;
            }

            public virtual dom.HTMLFormElement form
            {
                get;
            }

            public virtual string height
            {
                get;
                set;
            }

            public virtual uint hspace
            {
                get;
                set;
            }

            public virtual bool msPlayToDisabled
            {
                get;
                set;
            }

            public virtual string msPlayToPreferredSourceUri
            {
                get;
                set;
            }

            public virtual bool msPlayToPrimary
            {
                get;
                set;
            }

            public virtual object msPlayToSource
            {
                get;
            }

            public virtual string name
            {
                get;
                set;
            }

            public virtual double readyState
            {
                get;
            }

            public virtual string standby
            {
                get;
                set;
            }

            public virtual string type
            {
                get;
                set;
            }

            public virtual bool typemustmatch
            {
                get;
                set;
            }

            public virtual string useMap
            {
                get;
                set;
            }

            public virtual string validationMessage
            {
                get;
            }

            public virtual dom.ValidityState validity
            {
                get;
            }

            public virtual uint vspace
            {
                get;
                set;
            }

            public virtual string width
            {
                get;
                set;
            }

            public virtual bool willValidate
            {
                get;
            }

            public virtual extern bool checkValidity();

            public virtual extern void setCustomValidity(string error);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLObjectElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLObjectElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLObjectElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLObjectElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLObjectElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLObjectElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLObjectElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLObjectElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual extern dom.Document getSVGDocument();

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLOptGroupElement : dom.HTMLElement
        {

            [Template("document.createElement(\"optgroup\")")]
            public extern HTMLOptGroupElement();

            public static dom.HTMLOptGroupElement prototype
            {
                get;
                set;
            }

            public virtual bool disabled
            {
                get;
                set;
            }

            public virtual dom.HTMLFormElement form
            {
                get;
            }

            public virtual string label
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLOptGroupElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLOptGroupElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLOptGroupElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLOptGroupElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLOptGroupElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLOptGroupElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLOptGroupElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLOptGroupElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLOptionElement : dom.HTMLElement
        {




            [Template("document.createElement(\"option\")")]
            public extern HTMLOptionElement();

            public static dom.HTMLOptionElement prototype
            {
                get;
                set;
            }

            public virtual bool defaultSelected
            {
                get;
                set;
            }

            public virtual bool disabled
            {
                get;
                set;
            }

            public virtual dom.HTMLFormElement form
            {
                get;
            }

            public virtual int index
            {
                get;
            }

            public virtual string label
            {
                get;
                set;
            }

            public virtual bool selected
            {
                get;
                set;
            }

            public virtual string text
            {
                get;
                set;
            }

            public virtual string value
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLOptionElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLOptionElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLOptionElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLOptionElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLOptionElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLOptionElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLOptionElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLOptionElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLOptionsCollection : dom.HTMLCollectionOf<dom.HTMLOptionElement>
        {

            public static dom.HTMLOptionsCollection prototype
            {
                get;
                set;
            }

            public virtual int selectedIndex
            {
                get;
                set;
            }

            public virtual extern void add(
              Union<dom.HTMLOptionElement, dom.HTMLOptGroupElement> element);

            public virtual extern void add(dom.HTMLOptionElement element);

            public virtual extern void add(dom.HTMLOptGroupElement element);

            public virtual extern void add(
              Union<dom.HTMLOptionElement, dom.HTMLOptGroupElement> element,
              Union<dom.HTMLElement, double, Null> before);

            public virtual extern void add(dom.HTMLOptionElement element, dom.HTMLElement before);

            public virtual extern void add(dom.HTMLOptionElement element, double before);

            public virtual extern void add(dom.HTMLOptionElement element, Null before);

            public virtual extern void add(dom.HTMLOptGroupElement element, dom.HTMLElement before);

            public virtual extern void add(dom.HTMLOptGroupElement element, double before);

            public virtual extern void add(dom.HTMLOptGroupElement element, Null before);

            public virtual extern void remove(int index);

            public override extern dom.HTMLOptionElement item(double index);

            public override extern dom.HTMLOptionElement namedItem(string name);

            public override extern uint length { get; }

            public override extern dom.Element item(uint index);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLOutputElement : dom.HTMLElement
        {



            [Template("document.createElement(\"output\")")]
            public extern HTMLOutputElement();

            public static dom.HTMLOutputElement prototype
            {
                get;
                set;
            }

            public virtual string defaultValue
            {
                get;
                set;
            }

            public virtual dom.HTMLFormElement form
            {
                get;
            }

            public virtual dom.DOMTokenList htmlFor
            {
                get;
            }

            public virtual string name
            {
                get;
                set;
            }

            public virtual string type
            {
                get;
            }

            public virtual string validationMessage
            {
                get;
            }

            public virtual dom.ValidityState validity
            {
                get;
            }

            public virtual string value
            {
                get;
                set;
            }

            public virtual bool willValidate
            {
                get;
            }

            public virtual extern bool checkValidity();

            public virtual extern bool reportValidity();

            public virtual extern void setCustomValidity(string error);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLOutputElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLOutputElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLOutputElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLOutputElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLOutputElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLOutputElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLOutputElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLOutputElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLParagraphElement : dom.HTMLElement
        {


            [Template("document.createElement(\"p\")")]
            public extern HTMLParagraphElement();

            public static dom.HTMLParagraphElement prototype
            {
                get;
                set;
            }

            public virtual string align
            {
                get;
                set;
            }

            public virtual string clear
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLParagraphElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLParagraphElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLParagraphElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLParagraphElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLParagraphElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLParagraphElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLParagraphElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLParagraphElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLParamElement : dom.HTMLElement
        {


            [Template("document.createElement(\"param\")")]
            public extern HTMLParamElement();

            public static dom.HTMLParamElement prototype
            {
                get;
                set;
            }

            public virtual string name
            {
                get;
                set;
            }

            public virtual string type
            {
                get;
                set;
            }

            public virtual string value
            {
                get;
                set;
            }

            public virtual string valueType
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLParamElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLParamElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLParamElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLParamElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLParamElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLParamElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLParamElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLParamElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLPictureElement : dom.HTMLElement
        {
            [Template("document.createElement(\"picture\")")]
            public extern HTMLPictureElement();

            public static dom.HTMLPictureElement prototype
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLPictureElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLPictureElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLPictureElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLPictureElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLPictureElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLPictureElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLPictureElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLPictureElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLPreElement : dom.HTMLElement
        {

            [Template("document.createElement(\"pre\")")]
            public extern HTMLPreElement();

            public static dom.HTMLPreElement prototype
            {
                get;
                set;
            }

            public virtual int width
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLPreElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLPreElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLPreElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLPreElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLPreElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLPreElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLPreElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLPreElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLProgressElement : dom.HTMLElement
        {


            [Template("document.createElement(\"progress\")")]
            public extern HTMLProgressElement();

            public static dom.HTMLProgressElement prototype
            {
                get;
                set;
            }

            public virtual dom.HTMLFormElement form
            {
                get;
            }

            public virtual double max
            {
                get;
                set;
            }

            public virtual double position
            {
                get;
            }

            public virtual double value
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLProgressElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLProgressElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLProgressElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLProgressElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLProgressElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLProgressElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLProgressElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLProgressElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLQuoteElement : dom.HTMLElement
        {

            [Template("document.createElement(\"blockquote\")")]
            public extern HTMLQuoteElement();

            public static dom.HTMLQuoteElement prototype
            {
                get;
                set;
            }

            public virtual string cite
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLQuoteElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLQuoteElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLQuoteElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLQuoteElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLQuoteElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLQuoteElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLQuoteElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLQuoteElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLScriptElement : dom.HTMLElement
        {





            [Template("document.createElement(\"script\")")]
            public extern HTMLScriptElement();

            public static dom.HTMLScriptElement prototype
            {
                get;
                set;
            }

            public virtual bool async
            {
                get;
                set;
            }

            public virtual string charset
            {
                get;
                set;
            }

            public virtual string crossOrigin
            {
                get;
                set;
            }

            public virtual bool defer
            {
                get;
                set;
            }

            [Name("event")]
            public virtual string @event
            {
                get;
                set;
            }

            public virtual string htmlFor
            {
                get;
                set;
            }

            public virtual string integrity
            {
                get;
                set;
            }

            public virtual bool noModule
            {
                get;
                set;
            }

            public virtual string src
            {
                get;
                set;
            }

            public virtual string text
            {
                get;
                set;
            }

            public virtual string type
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLScriptElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLScriptElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLScriptElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLScriptElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLScriptElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLScriptElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLScriptElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLScriptElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLSelectElement : dom.HTMLElement
        {






            [Template("document.createElement(\"select\")")]
            public extern HTMLSelectElement();

            public static dom.HTMLSelectElement prototype
            {
                get;
                set;
            }

            public virtual bool autofocus
            {
                get;
                set;
            }

            public virtual bool disabled
            {
                get;
                set;
            }

            public virtual dom.HTMLFormElement form
            {
                get;
            }

            public virtual uint length
            {
                get;
                set;
            }

            public virtual bool multiple
            {
                get;
                set;
            }

            public virtual string name
            {
                get;
                set;
            }

            public virtual dom.HTMLOptionsCollection options
            {
                get;
            }

            public virtual bool required
            {
                get;
                set;
            }

            public virtual int selectedIndex
            {
                get;
                set;
            }

            public virtual dom.HTMLCollectionOf<dom.HTMLOptionElement> selectedOptions
            {
                get;
            }

            public virtual uint size
            {
                get;
                set;
            }

            public virtual string type
            {
                get;
            }

            public virtual string validationMessage
            {
                get;
            }

            public virtual dom.ValidityState validity
            {
                get;
            }

            public virtual string value
            {
                get;
                set;
            }

            public virtual bool willValidate
            {
                get;
            }

            public virtual extern void add(
              Union<dom.HTMLOptionElement, dom.HTMLOptGroupElement> element);

            public virtual extern void add(dom.HTMLOptionElement element);

            public virtual extern void add(dom.HTMLOptGroupElement element);

            public virtual extern void add(
              Union<dom.HTMLOptionElement, dom.HTMLOptGroupElement> element,
              Union<dom.HTMLElement, double, Null> before);

            public virtual extern void add(dom.HTMLOptionElement element, dom.HTMLElement before);

            public virtual extern void add(dom.HTMLOptionElement element, double before);

            public virtual extern void add(dom.HTMLOptionElement element, Null before);

            public virtual extern void add(dom.HTMLOptGroupElement element, dom.HTMLElement before);

            public virtual extern void add(dom.HTMLOptGroupElement element, double before);

            public virtual extern void add(dom.HTMLOptGroupElement element, Null before);

            public virtual extern bool checkValidity();

            public virtual extern dom.Element item();

            public virtual extern dom.Element item(object name);

            public virtual extern dom.Element item(object name, object index);

            public virtual extern object namedItem(string name);

            public override extern void remove();

            public virtual extern void remove(int index);

            public virtual extern void setCustomValidity(string error);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLSelectElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLSelectElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLSelectElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLSelectElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLSelectElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLSelectElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLSelectElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLSelectElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [IgnoreCast]
        [Virtual]
        [FormerInterface]
        public abstract class HTMLSlotElement : dom.HTMLElement
        {
            public virtual string name
            {
                get;
                set;
            }

            public abstract dom.Node[] assignedNodes();

            public abstract dom.Node[] assignedNodes(dom.AssignedNodesOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public abstract void addEventListener<K>(
              K type,
              dom.HTMLSlotElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public abstract void addEventListener<K>(
              K type,
              dom.HTMLSlotElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public abstract void addEventListener<K>(
              K type,
              dom.HTMLSlotElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public abstract void addEventListener<K>(
              K type,
              dom.HTMLSlotElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public abstract void removeEventListener<K>(
              K type,
              dom.HTMLSlotElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public abstract void removeEventListener<K>(
              K type,
              dom.HTMLSlotElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public abstract void removeEventListener<K>(
              K type,
              dom.HTMLSlotElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public abstract void removeEventListener<K>(
              K type,
              dom.HTMLSlotElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            [Generated]
            public new delegate void addEventListenerFn<K>(object ev);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            [Generated]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLSourceElement : dom.HTMLElement
        {


            [Template("document.createElement(\"source\")")]
            public extern HTMLSourceElement();

            public static dom.HTMLSourceElement prototype
            {
                get;
                set;
            }

            public virtual string media
            {
                get;
                set;
            }

            public virtual string msKeySystem
            {
                get;
                set;
            }

            public virtual string sizes
            {
                get;
                set;
            }

            public virtual string src
            {
                get;
                set;
            }

            public virtual string srcset
            {
                get;
                set;
            }

            public virtual string type
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLSourceElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLSourceElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLSourceElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLSourceElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLSourceElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLSourceElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLSourceElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLSourceElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLSpanElement : dom.HTMLElement
        {
            [Template("document.createElement(\"span\")")]
            public extern HTMLSpanElement();

            public static dom.HTMLSpanElement prototype
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLSpanElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLSpanElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLSpanElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLSpanElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLSpanElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLSpanElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLSpanElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLSpanElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLStyleElement : dom.HTMLElement, dom.LinkStyle.Interface, IObject
        {


            [Template("document.createElement(\"style\")")]
            public extern HTMLStyleElement();

            public static dom.HTMLStyleElement prototype
            {
                get;
                set;
            }

            public virtual bool disabled
            {
                get;
                set;
            }

            public virtual string media
            {
                get;
                set;
            }

            public virtual string type
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLStyleElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLStyleElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLStyleElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLStyleElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLStyleElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLStyleElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLStyleElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLStyleElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual dom.StyleSheet sheet
            {
                get;
            }

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLSummaryElement : dom.HTMLElement
        {
            public static dom.HTMLSummaryElement prototype
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLSummaryElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLSummaryElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLSummaryElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLSummaryElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLSummaryElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLSummaryElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLSummaryElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLSummaryElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLTableCaptionElement : dom.HTMLElement
        {

            [Template("document.createElement(\"caption\")")]
            public extern HTMLTableCaptionElement();

            public static dom.HTMLTableCaptionElement prototype
            {
                get;
                set;
            }

            public virtual string align
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTableCaptionElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTableCaptionElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTableCaptionElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTableCaptionElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTableCaptionElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTableCaptionElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTableCaptionElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTableCaptionElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLTableCellElement : dom.HTMLElement
        {





            public static dom.HTMLTableCellElement prototype
            {
                get;
                set;
            }

            public virtual string abbr
            {
                get;
                set;
            }

            public virtual string align
            {
                get;
                set;
            }

            public virtual string axis
            {
                get;
                set;
            }

            public virtual string bgColor
            {
                get;
                set;
            }

            public virtual int cellIndex
            {
                get;
            }

            public virtual string ch
            {
                get;
                set;
            }

            public virtual string chOff
            {
                get;
                set;
            }

            public virtual uint colSpan
            {
                get;
                set;
            }

            public virtual string headers
            {
                get;
                set;
            }

            public virtual string height
            {
                get;
                set;
            }

            public virtual bool noWrap
            {
                get;
                set;
            }

            public virtual uint rowSpan
            {
                get;
                set;
            }

            public virtual string scope
            {
                get;
                set;
            }

            public virtual string vAlign
            {
                get;
                set;
            }

            public virtual string width
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTableCellElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTableCellElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTableCellElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTableCellElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTableCellElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTableCellElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTableCellElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTableCellElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLTableColElement : dom.HTMLElement
        {


            [Template("document.createElement(\"col\")")]
            public extern HTMLTableColElement();

            public static dom.HTMLTableColElement prototype
            {
                get;
                set;
            }

            public virtual string align
            {
                get;
                set;
            }

            public virtual string ch
            {
                get;
                set;
            }

            public virtual string chOff
            {
                get;
                set;
            }

            public virtual uint span
            {
                get;
                set;
            }

            public virtual string vAlign
            {
                get;
                set;
            }

            public virtual string width
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTableColElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTableColElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTableColElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTableColElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTableColElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTableColElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTableColElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTableColElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLTableDataCellElement : dom.HTMLTableCellElement
        {
            [Template("document.createElement(\"td\")")]
            public extern HTMLTableDataCellElement();

            public static dom.HTMLTableDataCellElement prototype
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTableDataCellElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTableDataCellElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTableDataCellElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTableDataCellElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTableDataCellElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTableDataCellElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTableDataCellElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTableDataCellElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLTableElement : dom.HTMLElement
        {






            [Template("document.createElement(\"table\")")]
            public extern HTMLTableElement();

            public static dom.HTMLTableElement prototype
            {
                get;
                set;
            }

            public virtual string align
            {
                get;
                set;
            }

            public virtual string bgColor
            {
                get;
                set;
            }

            public virtual string border
            {
                get;
                set;
            }

            public virtual dom.HTMLTableCaptionElement caption
            {
                get;
                set;
            }

            public virtual string cellPadding
            {
                get;
                set;
            }

            public virtual string cellSpacing
            {
                get;
                set;
            }

            public virtual string frame
            {
                get;
                set;
            }

            public virtual dom.HTMLCollectionOf<dom.HTMLTableRowElement> rows
            {
                get;
            }

            public virtual string rules
            {
                get;
                set;
            }

            public virtual string summary
            {
                get;
                set;
            }

            public virtual dom.HTMLCollectionOf<dom.HTMLTableSectionElement> tBodies
            {
                get;
            }

            public virtual dom.HTMLTableSectionElement tFoot
            {
                get;
                set;
            }

            public virtual dom.HTMLTableSectionElement tHead
            {
                get;
                set;
            }

            public virtual string width
            {
                get;
                set;
            }

            public virtual extern dom.HTMLTableCaptionElement createCaption();

            public virtual extern dom.HTMLTableSectionElement createTBody();

            public virtual extern dom.HTMLTableSectionElement createTFoot();

            public virtual extern dom.HTMLTableSectionElement createTHead();

            public virtual extern void deleteCaption();

            public virtual extern void deleteRow();

            public virtual extern void deleteRow(int index);

            public virtual extern void deleteTFoot();

            public virtual extern void deleteTHead();

            public virtual extern dom.HTMLTableRowElement insertRow();

            public virtual extern dom.HTMLTableRowElement insertRow(int index);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTableElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTableElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTableElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTableElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTableElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTableElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTableElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTableElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLTableHeaderCellElement : dom.HTMLTableCellElement
        {
            [Template("document.createElement(\"th\")")]
            public extern HTMLTableHeaderCellElement();

            public static dom.HTMLTableHeaderCellElement prototype
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTableHeaderCellElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTableHeaderCellElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTableHeaderCellElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTableHeaderCellElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTableHeaderCellElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTableHeaderCellElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTableHeaderCellElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTableHeaderCellElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLTableRowElement : dom.HTMLElement
        {




            [Template("document.createElement(\"tr\")")]
            public extern HTMLTableRowElement();

            public static dom.HTMLTableRowElement prototype
            {
                get;
                set;
            }

            public virtual string align
            {
                get;
                set;
            }

            public virtual string bgColor
            {
                get;
                set;
            }

            public virtual dom.HTMLCollectionOf<dom.HTMLTableCellElement> cells
            {
                get;
            }

            public virtual string ch
            {
                get;
                set;
            }

            public virtual string chOff
            {
                get;
                set;
            }

            public virtual int rowIndex
            {
                get;
            }

            public virtual int sectionRowIndex
            {
                get;
            }

            public virtual string vAlign
            {
                get;
                set;
            }

            public virtual extern void deleteCell();

            public virtual extern void deleteCell(int index);

            public virtual extern dom.HTMLTableDataCellElement insertCell();

            public virtual extern dom.HTMLTableDataCellElement insertCell(int index);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTableRowElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTableRowElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTableRowElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTableRowElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTableRowElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTableRowElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTableRowElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTableRowElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLTableSectionElement : dom.HTMLElement
        {



            [Template("document.createElement(\"tbody\")")]
            public extern HTMLTableSectionElement();

            public static dom.HTMLTableSectionElement prototype
            {
                get;
                set;
            }

            public virtual string align
            {
                get;
                set;
            }

            public virtual string ch
            {
                get;
                set;
            }

            public virtual string chOff
            {
                get;
                set;
            }

            public virtual dom.HTMLCollectionOf<dom.HTMLTableRowElement> rows
            {
                get;
            }

            public virtual string vAlign
            {
                get;
                set;
            }

            public virtual extern void deleteRow();

            public virtual extern void deleteRow(int index);

            public virtual extern dom.HTMLTableRowElement insertRow();

            public virtual extern dom.HTMLTableRowElement insertRow(int index);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTableSectionElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTableSectionElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTableSectionElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTableSectionElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTableSectionElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTableSectionElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTableSectionElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTableSectionElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLTemplateElement : dom.HTMLElement
        {

            [Template("document.createElement(\"template\")")]
            public extern HTMLTemplateElement();

            public static dom.HTMLTemplateElement prototype
            {
                get;
                set;
            }

            public virtual dom.DocumentFragment content
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTemplateElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTemplateElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTemplateElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTemplateElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTemplateElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTemplateElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTemplateElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTemplateElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLTextAreaElement : dom.HTMLElement
        {








            [Template("document.createElement(\"textarea\")")]
            public extern HTMLTextAreaElement();

            public static dom.HTMLTextAreaElement prototype
            {
                get;
                set;
            }

            public virtual bool autofocus
            {
                get;
                set;
            }

            public virtual uint cols
            {
                get;
                set;
            }

            public virtual string defaultValue
            {
                get;
                set;
            }

            public virtual bool disabled
            {
                get;
                set;
            }

            public virtual dom.HTMLFormElement form
            {
                get;
            }

            public virtual int maxLength
            {
                get;
                set;
            }

            public virtual int minLength
            {
                get;
                set;
            }

            public virtual string name
            {
                get;
                set;
            }

            public virtual string placeholder
            {
                get;
                set;
            }

            public virtual bool readOnly
            {
                get;
                set;
            }

            public virtual bool required
            {
                get;
                set;
            }

            public virtual uint rows
            {
                get;
                set;
            }

            public virtual uint selectionEnd
            {
                get;
                set;
            }

            public virtual uint selectionStart
            {
                get;
                set;
            }

            public virtual string type
            {
                get;
            }

            public virtual string validationMessage
            {
                get;
            }

            public virtual dom.ValidityState validity
            {
                get;
            }

            public virtual string value
            {
                get;
                set;
            }

            public virtual bool willValidate
            {
                get;
            }

            public virtual string wrap
            {
                get;
                set;
            }

            public virtual extern bool checkValidity();

            public virtual extern void select();

            public virtual extern void setCustomValidity(string error);

            public virtual extern void setSelectionRange(uint start, uint end);

            public virtual extern void setSelectionRange(
              uint start,
              uint end,
              dom.Literals.Options.direction3 direction);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTextAreaElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTextAreaElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTextAreaElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTextAreaElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTextAreaElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTextAreaElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTextAreaElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTextAreaElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLTimeElement : dom.HTMLElement
        {

            [Template("document.createElement(\"time\")")]
            public extern HTMLTimeElement();

            public static dom.HTMLTimeElement prototype
            {
                get;
                set;
            }

            public virtual string dateTime
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTimeElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTimeElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTimeElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTimeElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTimeElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTimeElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTimeElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTimeElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLTitleElement : dom.HTMLElement
        {

            [Template("document.createElement(\"title\")")]
            public extern HTMLTitleElement();

            public static dom.HTMLTitleElement prototype
            {
                get;
                set;
            }

            public virtual string text
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTitleElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTitleElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTitleElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTitleElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTitleElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTitleElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTitleElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTitleElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLTrackElement : dom.HTMLElement
        {





            [Template("document.createElement(\"track\")")]
            public extern HTMLTrackElement();

            public static dom.HTMLTrackElement prototype
            {
                get;
                set;
            }

            [Name("ERROR")]
            public static ushort ERROR_Static
            {
                get;
            }

            [Name("LOADED")]
            public static ushort LOADED_Static
            {
                get;
            }

            [Name("LOADING")]
            public static ushort LOADING_Static
            {
                get;
            }

            [Name("NONE")]
            public static ushort NONE_Static
            {
                get;
            }

            [Name("default")]
            public virtual bool @default
            {
                get;
                set;
            }

            public virtual string kind
            {
                get;
                set;
            }

            public virtual string label
            {
                get;
                set;
            }

            public virtual ushort readyState
            {
                get;
            }

            public virtual string src
            {
                get;
                set;
            }

            public virtual string srclang
            {
                get;
                set;
            }

            public virtual dom.TextTrack track
            {
                get;
            }

            public virtual ushort ERROR
            {
                get;
            }

            public virtual ushort LOADED
            {
                get;
            }

            public virtual ushort LOADING
            {
                get;
            }

            public virtual ushort NONE
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTrackElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTrackElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTrackElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLTrackElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTrackElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTrackElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTrackElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLTrackElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLUListElement : dom.HTMLElement
        {


            [Template("document.createElement(\"ul\")")]
            public extern HTMLUListElement();

            public static dom.HTMLUListElement prototype
            {
                get;
                set;
            }

            public virtual bool compact
            {
                get;
                set;
            }

            public virtual string type
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLUListElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLUListElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLUListElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLUListElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLUListElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLUListElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLUListElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLUListElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLUnknownElement : dom.HTMLElement
        {
            [Template("document.createElement(\"isindex\")")]
            public extern HTMLUnknownElement();

            public static dom.HTMLUnknownElement prototype
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLUnknownElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLUnknownElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLUnknownElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLUnknownElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLUnknownElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLUnknownElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLUnknownElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLUnknownElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class HTMLVideoElementEventMap : dom.HTMLMediaElementEventMap
        {


            public dom.Event MSVideoFormatChanged
            {
                get;
                set;
            }

            public dom.Event MSVideoFrameStepCompleted
            {
                get;
                set;
            }

            public dom.Event MSVideoOptimalLayoutChanged
            {
                get;
                set;
            }

            [Generated]
            public new static class KeyOf
            {
                [Template("\"MSVideoFormatChanged\"")]
                public static readonly KeyOf<dom.HTMLVideoElementEventMap> MSVideoFormatChanged;
                [Template("\"MSVideoFrameStepCompleted\"")]
                public static readonly KeyOf<dom.HTMLVideoElementEventMap> MSVideoFrameStepCompleted;
                [Template("\"MSVideoOptimalLayoutChanged\"")]
                public static readonly KeyOf<dom.HTMLVideoElementEventMap> MSVideoOptimalLayoutChanged;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public partial class HTMLVideoElement : dom.HTMLMediaElement
        {






            [Template("document.createElement(\"video\")")]
            public extern HTMLVideoElement();

            public static dom.HTMLVideoElement prototype
            {
                get;
                set;
            }

            public virtual uint height
            {
                get;
                set;
            }

            public virtual bool msHorizontalMirror
            {
                get;
                set;
            }

            public virtual bool msIsLayoutOptimalForPlayback
            {
                get;
            }

            public virtual bool msIsStereo3D
            {
                get;
            }

            public virtual string msStereo3DPackingMode
            {
                get;
                set;
            }

            public virtual string msStereo3DRenderMode
            {
                get;
                set;
            }

            public virtual bool msZoom
            {
                get;
                set;
            }

            public virtual dom.HTMLVideoElement.onMSVideoFormatChangedFn onMSVideoFormatChanged
            {
                get;
                set;
            }

            public virtual dom.HTMLVideoElement.onMSVideoFormatChangedFn onMSVideoFrameStepCompleted
            {
                get;
                set;
            }

            public virtual dom.HTMLVideoElement.onMSVideoFormatChangedFn onMSVideoOptimalLayoutChanged
            {
                get;
                set;
            }

            public virtual string poster
            {
                get;
                set;
            }

            public virtual uint videoHeight
            {
                get;
            }

            public virtual uint videoWidth
            {
                get;
            }

            public virtual bool webkitDisplayingFullscreen
            {
                get;
            }

            public virtual bool webkitSupportsFullscreen
            {
                get;
            }

            public virtual uint width
            {
                get;
                set;
            }

            public virtual extern dom.VideoPlaybackQuality getVideoPlaybackQuality();

            public virtual extern void msFrameStep(bool forward);

            public virtual extern void msInsertVideoEffect(string activatableClassId, bool effectRequired);

            public virtual extern void msInsertVideoEffect(
              string activatableClassId,
              bool effectRequired,
              object config);

            public virtual extern void msSetVideoRectangle(
              double left,
              double top,
              double right,
              double bottom);

            public virtual extern void webkitEnterFullScreen();

            public virtual extern void webkitEnterFullscreen();

            public virtual extern void webkitExitFullScreen();

            public virtual extern void webkitExitFullscreen();

            [Where("K", typeof(KeyOf<dom.HTMLVideoElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLVideoElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLVideoElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLVideoElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLVideoElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLVideoElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLVideoElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.HTMLVideoElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.HTMLVideoElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLVideoElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.HTMLVideoElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLVideoElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.HTMLVideoElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLVideoElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.HTMLVideoElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.HTMLVideoElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            public delegate void onMSVideoFormatChangedFn(dom.Event ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLVideoElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.HTMLVideoElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class HTMLegendElement : IObject
        {

            public static dom.HTMLegendElement prototype
            {
                get;
                set;
            }

            public virtual dom.HTMLFormElement form
            {
                get;
            }
        }
    }
}
