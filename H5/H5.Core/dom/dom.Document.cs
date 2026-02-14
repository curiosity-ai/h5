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
        public partial class Document : dom.Node, dom.GlobalEventHandlers.Interface, IObject, dom.ParentNode.Interface, dom.DocumentEvent.Interface
        {

            public static dom.Document prototype
            {
                get;
                set;
            }

            public virtual string URL
            {
                get;
            }

            public virtual string URLUnencoded
            {
                get;
            }

            public virtual dom.Element activeElement
            {
                get;
            }

            public virtual string alinkColor
            {
                get;
                set;
            }

            public virtual dom.HTMLAllCollection all
            {
                get;
            }

            public virtual dom.HTMLCollectionOf<dom.HTMLAnchorElement> anchors
            {
                get;
            }

            public virtual dom.HTMLCollectionOf<dom.HTMLAppletElement> applets
            {
                get;
            }

            public virtual string bgColor
            {
                get;
                set;
            }

            public virtual dom.HTMLElement body
            {
                get;
                set;
            }

            public virtual string characterSet
            {
                get;
            }

            public virtual string charset
            {
                get;
                set;
            }

            public virtual string compatMode
            {
                get;
            }

            public virtual string cookie
            {
                get;
                set;
            }

            public virtual Union<dom.HTMLScriptElement, dom.SVGScriptElement, Null> currentScript
            {
                get;
            }

            public virtual dom.Window defaultView
            {
                get;
            }

            public virtual string designMode
            {
                get;
                set;
            }

            public virtual string dir
            {
                get;
                set;
            }

            public virtual dom.DocumentType doctype
            {
                get;
            }

            public virtual dom.HTMLElement documentElement
            {
                get;
            }

            public virtual string domain
            {
                get;
                set;
            }

            public virtual dom.HTMLCollectionOf<dom.HTMLEmbedElement> embeds
            {
                get;
            }

            public virtual string fgColor
            {
                get;
                set;
            }

            public virtual dom.HTMLCollectionOf<dom.HTMLFormElement> forms
            {
                get;
            }

            public virtual dom.Element fullscreenElement
            {
                get;
            }

            public virtual bool fullscreenEnabled
            {
                get;
            }

            public virtual dom.HTMLHeadElement head
            {
                get;
            }

            public virtual bool hidden
            {
                get;
            }

            public virtual dom.HTMLCollectionOf<dom.HTMLImageElement> images
            {
                get;
            }

            public virtual dom.DOMImplementation implementation
            {
                get;
            }

            public virtual string inputEncoding
            {
                get;
            }

            public virtual string lastModified
            {
                get;
            }

            public virtual string linkColor
            {
                get;
                set;
            }

            public virtual dom.HTMLCollectionOf<dom.HTMLElement> links
            {
                get;
            }

            public virtual dom.Location location
            {
                get;
                set;
            }

            public virtual bool msCSSOMElementFloatMetrics
            {
                get;
                set;
            }

            public virtual bool msCapsLockWarningOff
            {
                get;
                set;
            }

            public virtual dom.Document.onabortFn onabort
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onactivate
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onbeforeactivate
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onbeforedeactivate
            {
                get;
                set;
            }

            public virtual dom.Document.onblurFn onblur
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn oncanplay
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn oncanplaythrough
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onchange
            {
                get;
                set;
            }

            public virtual dom.Document.onclickFn onclick
            {
                get;
                set;
            }

            public virtual dom.Document.onclickFn onauxclick
            {
                get;
                set;
            }

            public virtual dom.Document.oncontextmenuFn oncontextmenu
            {
                get;
                set;
            }

            public virtual dom.Document.onclickFn ondblclick
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn ondeactivate
            {
                get;
                set;
            }

            public virtual dom.Document.ondragFn ondrag
            {
                get;
                set;
            }

            public virtual dom.Document.ondragFn ondragend
            {
                get;
                set;
            }

            public virtual dom.Document.ondragFn ondragenter
            {
                get;
                set;
            }

            public virtual dom.Document.ondragFn ondragleave
            {
                get;
                set;
            }

            public virtual dom.Document.ondragFn ondragover
            {
                get;
                set;
            }

            public virtual dom.Document.ondragFn ondragstart
            {
                get;
                set;
            }

            public virtual dom.Document.ondragFn ondrop
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn ondurationchange
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onemptied
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onended
            {
                get;
                set;
            }

            public virtual dom.Document.onerrorFn onerror
            {
                get;
                set;
            }

            public virtual dom.Document.onblurFn onfocus
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onfullscreenchange
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onfullscreenerror
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn oninput
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn oninvalid
            {
                get;
                set;
            }

            public virtual dom.Document.onkeydownFn onkeydown
            {
                get;
                set;
            }

            public virtual dom.Document.onkeydownFn onkeypress
            {
                get;
                set;
            }

            public virtual dom.Document.onkeydownFn onkeyup
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onload
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onloadeddata
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onloadedmetadata
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onloadstart
            {
                get;
                set;
            }

            public virtual dom.Document.onclickFn onmousedown
            {
                get;
                set;
            }

            public virtual dom.Document.onclickFn onmousemove
            {
                get;
                set;
            }

            public virtual dom.Document.onclickFn onmouseout
            {
                get;
                set;
            }

            public virtual dom.Document.onclickFn onmouseover
            {
                get;
                set;
            }

            public virtual dom.Document.onclickFn onmouseup
            {
                get;
                set;
            }

            public virtual dom.Document.onmousewheelFn onmousewheel
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onmscontentzoom
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onmsgesturechange
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onmsgesturedoubletap
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onmsgestureend
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onmsgesturehold
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onmsgesturestart
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onmsgesturetap
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onmsinertiastart
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onmsmanipulationstatechanged
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onmspointercancel
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onmspointerdown
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onmspointerenter
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onmspointerleave
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onmspointermove
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onmspointerout
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onmspointerover
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onmspointerup
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onmssitemodejumplistitemremoved
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onmsthumbnailclick
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onpause
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onplay
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onplaying
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onpointerlockchange
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onpointerlockerror
            {
                get;
                set;
            }

            public virtual dom.Document.onprogressFn onprogress
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onratechange
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onreadystatechange
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onreset
            {
                get;
                set;
            }

            public virtual dom.Document.onabortFn onscroll
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onseeked
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onseeking
            {
                get;
                set;
            }

            public virtual dom.Document.onabortFn onselect
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onselectionchange
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onselectstart
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onstalled
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onstop
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onsubmit
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onsuspend
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn ontimeupdate
            {
                get;
                set;
            }

            public virtual dom.Document.ontouchcancelFn ontouchcancel
            {
                get;
                set;
            }

            public virtual dom.Document.ontouchcancelFn ontouchend
            {
                get;
                set;
            }

            public virtual dom.Document.ontouchcancelFn ontouchmove
            {
                get;
                set;
            }

            public virtual dom.Document.ontouchcancelFn ontouchstart
            {
                get;
                set;
            }

            public virtual dom.Document.onvisibilitychangeFn onvisibilitychange
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onvolumechange
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onwaiting
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onwebkitfullscreenchange
            {
                get;
                set;
            }

            public virtual dom.Document.onactivateFn onwebkitfullscreenerror
            {
                get;
                set;
            }

            public virtual dom.HTMLCollectionOf<dom.HTMLEmbedElement> plugins
            {
                get;
            }

            public virtual dom.Element pointerLockElement
            {
                get;
            }

            public virtual string readyState
            {
                get;
            }

            public virtual string referrer
            {
                get;
            }

            public virtual dom.SVGSVGElement rootElement
            {
                get;
            }

            public virtual dom.HTMLCollectionOf<dom.HTMLScriptElement> scripts
            {
                get;
            }

            public virtual dom.Element scrollingElement
            {
                get;
            }

            public virtual dom.StyleSheetList styleSheets
            {
                get;
            }

            public virtual string title
            {
                get;
                set;
            }

            public virtual dom.VisibilityState visibilityState
            {
                get;
            }

            public virtual string vlinkColor
            {
                get;
                set;
            }

            public virtual dom.Element webkitCurrentFullScreenElement
            {
                get;
            }

            public virtual dom.Element webkitFullscreenElement
            {
                get;
            }

            public virtual bool webkitFullscreenEnabled
            {
                get;
            }

            public virtual bool webkitIsFullScreen
            {
                get;
            }

            public virtual string xmlEncoding
            {
                get;
            }

            public virtual bool xmlStandalone
            {
                get;
                set;
            }

            public virtual string xmlVersion
            {
                get;
                set;
            }

            [Where("T", typeof(dom.Node.Interface), EnableImplicitConversion = true)]
            public virtual extern T adoptNode<T>(T source);

            public virtual extern void captureEvents();

            public virtual extern dom.Range caretRangeFromPoint(double x, double y);

            public virtual extern void clear();

            public virtual extern void close();

            public virtual extern dom.Attr createAttribute(string name);

            public virtual extern dom.Attr createAttributeNS(string namespaceURI, string qualifiedName);

            public virtual extern dom.CDATASection createCDATASection(string data);

            public virtual extern dom.Comment createComment(string data);

            public virtual extern dom.DocumentFragment createDocumentFragment();

            [Where("K", typeof(KeyOf<dom.HTMLElementTagNameMap>), EnableImplicitConversion = true)]
            public virtual extern object createElement<K>(K tagName);

            [Where("K", typeof(KeyOf<dom.HTMLElementTagNameMap>), EnableImplicitConversion = true)]
            public virtual extern object createElement<K>(K tagName, dom.ElementCreationOptions options);

            public virtual extern dom.HTMLElement createElement(string tagName);

            public virtual extern dom.HTMLElement createElement(
              string tagName,
              dom.ElementCreationOptions options);

            public virtual extern dom.HTMLElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash1999Slashxhtml namespaceURI,
              string qualifiedName);

            public virtual extern dom.SVGAElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.a qualifiedName);

            public virtual extern dom.SVGCircleElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.circle qualifiedName);

            public virtual extern dom.SVGClipPathElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.clipPath qualifiedName);

            public virtual extern dom.SVGComponentTransferFunctionElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.componentTransferFunction qualifiedName);

            public virtual extern dom.SVGDefsElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.defs qualifiedName);

            public virtual extern dom.SVGDescElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.desc qualifiedName);

            public virtual extern dom.SVGEllipseElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.ellipse qualifiedName);

            public virtual extern dom.SVGFEBlendElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.feBlend qualifiedName);

            public virtual extern dom.SVGFEColorMatrixElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.feColorMatrix qualifiedName);

            public virtual extern dom.SVGFEComponentTransferElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.feComponentTransfer qualifiedName);

            public virtual extern dom.SVGFECompositeElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.feComposite qualifiedName);

            public virtual extern dom.SVGFEConvolveMatrixElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.feConvolveMatrix qualifiedName);

            public virtual extern dom.SVGFEDiffuseLightingElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.feDiffuseLighting qualifiedName);

            public virtual extern dom.SVGFEDisplacementMapElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.feDisplacementMap qualifiedName);

            public virtual extern dom.SVGFEDistantLightElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.feDistantLight qualifiedName);

            public virtual extern dom.SVGFEFloodElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.feFlood qualifiedName);

            public virtual extern dom.SVGFEFuncAElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.feFuncA qualifiedName);

            public virtual extern dom.SVGFEFuncBElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.feFuncB qualifiedName);

            public virtual extern dom.SVGFEFuncGElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.feFuncG qualifiedName);

            public virtual extern dom.SVGFEFuncRElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.feFuncR qualifiedName);

            public virtual extern dom.SVGFEGaussianBlurElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.feGaussianBlur qualifiedName);

            public virtual extern dom.SVGFEImageElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.feImage qualifiedName);

            public virtual extern dom.SVGFEMergeElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.feMerge qualifiedName);

            public virtual extern dom.SVGFEMergeNodeElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.feMergeNode qualifiedName);

            public virtual extern dom.SVGFEMorphologyElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.feMorphology qualifiedName);

            public virtual extern dom.SVGFEOffsetElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.feOffset qualifiedName);

            public virtual extern dom.SVGFEPointLightElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.fePointLight qualifiedName);

            public virtual extern dom.SVGFESpecularLightingElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.feSpecularLighting qualifiedName);

            public virtual extern dom.SVGFESpotLightElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.feSpotLight qualifiedName);

            public virtual extern dom.SVGFETileElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.feTile qualifiedName);

            public virtual extern dom.SVGFETurbulenceElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.feTurbulence qualifiedName);

            public virtual extern dom.SVGFilterElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.filter qualifiedName);

            public virtual extern dom.SVGForeignObjectElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.foreignObject qualifiedName);

            public virtual extern dom.SVGGElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.g qualifiedName);

            public virtual extern dom.SVGImageElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.image qualifiedName);

            public virtual extern dom.SVGGradientElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.gradient qualifiedName);

            public virtual extern dom.SVGLineElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.line qualifiedName);

            public virtual extern dom.SVGLinearGradientElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.linearGradient qualifiedName);

            public virtual extern dom.SVGMarkerElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.marker qualifiedName);

            public virtual extern dom.SVGMaskElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.mask qualifiedName);

            public virtual extern dom.SVGPathElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.path qualifiedName);

            public virtual extern dom.SVGMetadataElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.metadata qualifiedName);

            public virtual extern dom.SVGPatternElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.pattern qualifiedName);

            public virtual extern dom.SVGPolygonElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.polygon qualifiedName);

            public virtual extern dom.SVGPolylineElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.polyline qualifiedName);

            public virtual extern dom.SVGRadialGradientElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.radialGradient qualifiedName);

            public virtual extern dom.SVGRectElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.rect qualifiedName);

            public virtual extern dom.SVGSVGElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.svg qualifiedName);

            public virtual extern dom.SVGScriptElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.script qualifiedName);

            public virtual extern dom.SVGStopElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.stop qualifiedName);

            public virtual extern dom.SVGStyleElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.style qualifiedName);

            public virtual extern dom.SVGSwitchElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.@switch qualifiedName);

            public virtual extern dom.SVGSymbolElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.symbol qualifiedName);

            public virtual extern dom.SVGTSpanElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.tspan qualifiedName);

            public virtual extern dom.SVGTextContentElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.textContent qualifiedName);

            public virtual extern dom.SVGTextElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.text qualifiedName);

            public virtual extern dom.SVGTextPathElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.textPath qualifiedName);

            public virtual extern dom.SVGTextPositioningElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.textPositioning qualifiedName);

            public virtual extern dom.SVGTitleElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.title qualifiedName);

            public virtual extern dom.SVGUseElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.use qualifiedName);

            public virtual extern dom.SVGViewElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              dom.Literals.Types.view qualifiedName);

            public virtual extern dom.SVGElement createElementNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              string qualifiedName);

            public virtual extern dom.Element createElementNS(
              string namespaceURI,
              string qualifiedName);

            public virtual extern dom.XPathExpression createExpression(
              string expression,
              dom.XPathNSResolver resolver);

            public virtual extern dom.XPathNSResolver createNSResolver(dom.Node nodeResolver);

            public virtual extern dom.NodeIterator createNodeIterator(dom.Node root);

            public virtual extern dom.NodeIterator createNodeIterator(dom.Node root, uint whatToShow);

            public virtual extern dom.NodeIterator createNodeIterator(
              dom.Node root,
              uint whatToShow,
              dom.NodeFilter filter);

            public virtual extern dom.NodeIterator createNodeIterator(
              dom.Node root,
              uint whatToShow,
              dom.NodeFilter filter,
              bool entityReferenceExpansion);

            public virtual extern dom.ProcessingInstruction createProcessingInstruction(
              string target,
              string data);

            public virtual extern dom.Range createRange();

            public virtual extern dom.Text createTextNode(string data);

            public virtual extern dom.Touch createTouch(
              dom.Window view,
              dom.EventTarget target,
              double identifier,
              double pageX,
              double pageY,
              double screenX,
              double screenY);

            [ExpandParams]
            public virtual extern dom.TouchList createTouchList(params dom.Touch[] touches);

            public virtual extern dom.TreeWalker createTreeWalker(dom.Node root);

            public virtual extern dom.TreeWalker createTreeWalker(dom.Node root, uint whatToShow);

            public virtual extern dom.TreeWalker createTreeWalker(
              dom.Node root,
              uint whatToShow,
              dom.NodeFilter filter);

            public virtual extern dom.TreeWalker createTreeWalker(
              dom.Node root,
              uint whatToShow,
              dom.NodeFilter filter,
              bool entityReferenceExpansion);

            public virtual extern dom.Element elementFromPoint(double x, double y);

            public virtual extern dom.Element[] elementsFromPoint(double x, double y);

            public virtual extern dom.XPathResult evaluate(
              string expression,
              dom.Node contextNode,
              dom.XPathNSResolver resolver,
              double type,
              dom.XPathResult result);

            public virtual extern bool execCommand(string commandId);

            public virtual extern bool execCommand(string commandId, bool showUI);

            public virtual extern bool execCommand(string commandId, bool showUI, object value);

            public virtual extern bool execCommandShowHelp(string commandId);

            public virtual extern void exitFullscreen();

            public virtual extern void exitPointerLock();

            public virtual extern void focus();

            public virtual extern dom.HTMLElement getElementById(string elementId);

            public virtual extern dom.HTMLCollectionOf<dom.Element> getElementsByClassName(
              string classNames);

            public virtual extern dom.NodeListOf<dom.HTMLElement> getElementsByName(
              string elementName);

            [Where("K", typeof(KeyOf<dom.HTMLElementTagNameMap>), EnableImplicitConversion = true)]
            public virtual extern dom.NodeListOf<dom.Node> getElementsByTagName<K>(K tagname);

            [Name("getElementsByTagName")]
            [Where("K", typeof(KeyOf<dom.SVGElementTagNameMap>), EnableImplicitConversion = true)]
            public virtual extern dom.NodeListOf<dom.Node> getElementsByTagName2<K>(K tagname);

            public virtual extern dom.NodeListOf<dom.Element> getElementsByTagName(string tagname);

            public virtual extern dom.HTMLCollectionOf<dom.HTMLElement> getElementsByTagNameNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash1999Slashxhtml namespaceURI,
              string localName);

            public virtual extern dom.HTMLCollectionOf<dom.SVGElement> getElementsByTagNameNS(
              dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg namespaceURI,
              string localName);

            public virtual extern dom.HTMLCollectionOf<dom.Element> getElementsByTagNameNS(
              string namespaceURI,
              string localName);

            public virtual extern dom.Selection getSelection();

            public virtual extern bool hasFocus();

            [Where("T", typeof(dom.Node.Interface), EnableImplicitConversion = true)]
            public virtual extern T importNode<T>(T importedNode, bool deep);

            public virtual extern dom.NodeListOf<dom.Element> msElementsFromPoint(
              double x,
              double y);

            public virtual extern dom.NodeListOf<dom.Element> msElementsFromRect(
              double left,
              double top,
              double width,
              double height);

            public virtual extern dom.Document open();

            public virtual extern dom.Document open(string url);

            public virtual extern dom.Document open(string url, string name);

            public virtual extern dom.Document open(string url, string name, string features);

            public virtual extern dom.Document open(
              string url,
              string name,
              string features,
              bool replace);

            public virtual extern bool queryCommandEnabled(string commandId);

            public virtual extern bool queryCommandIndeterm(string commandId);

            public virtual extern bool queryCommandState(string commandId);

            public virtual extern bool queryCommandSupported(string commandId);

            public virtual extern string queryCommandText(string commandId);

            public virtual extern string queryCommandValue(string commandId);

            public virtual extern void releaseEvents();

            public virtual extern void webkitCancelFullScreen();

            public virtual extern void webkitExitFullscreen();

            [ExpandParams]
            public virtual extern void write(params string[] content);

            [ExpandParams]
            public virtual extern void writeln(params string[] content);

            [Where("K", typeof(KeyOf<dom.DocumentEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.Document.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.DocumentEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.Document.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.DocumentEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.Document.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.DocumentEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.Document.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            public override extern void addEventListener(
              string type,
              dom.EventListenerOrEventListenerObject listener);

            public override extern void addEventListener(
              string type,
              Union<dom.EventListener, dom.EventListenerObject> listener);

            public override extern void addEventListener(string type, dom.EventListener listener);

            public override extern void addEventListener(string type, dom.EventListenerObject listener);

            public virtual extern void addEventListener(string type, Action<dom.Event> listener);

            public override extern void addEventListener(
              string type,
              dom.EventListenerOrEventListenerObject listener,
              Union<bool, dom.AddEventListenerOptions> options);

            public override extern void addEventListener(
              string type,
              Union<dom.EventListener, dom.EventListenerObject> listener,
              bool options);

            public override extern void addEventListener(
              string type,
              Union<dom.EventListener, dom.EventListenerObject> listener,
              dom.AddEventListenerOptions options);

            public override extern void addEventListener(
              string type,
              dom.EventListener listener,
              bool options);

            public override extern void addEventListener(
              string type,
              dom.EventListener listener,
              dom.AddEventListenerOptions options);

            public override extern void addEventListener(
              string type,
              dom.EventListenerObject listener,
              bool options);

            public override extern void addEventListener(
              string type,
              dom.EventListenerObject listener,
              dom.AddEventListenerOptions options);

            public virtual extern void addEventListener(
              string type,
              Action<dom.Event> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            public virtual extern void addEventListener(
              string type,
              Action<dom.Event> listener,
              bool options);

            public virtual extern void addEventListener(
              string type,
              Action<dom.Event> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.DocumentEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.Document.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.DocumentEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.Document.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.DocumentEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.Document.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.DocumentEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.Document.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public override extern void removeEventListener(
              string type,
              dom.EventListenerOrEventListenerObject listener);

            public override extern void removeEventListener(
              string type,
              Union<dom.EventListener, dom.EventListenerObject> listener);

            public override extern void removeEventListener(string type, dom.EventListener listener);

            public override extern void removeEventListener(string type, dom.EventListenerObject listener);

            public virtual extern void removeEventListener(string type, Action<dom.Event> listener);

            public virtual extern void removeEventListener(
              string type,
              dom.EventListenerOrEventListenerObject listener,
              Union<bool, dom.EventListenerOptions> options);

            public override extern void removeEventListener(
              string type,
              Union<dom.EventListener, dom.EventListenerObject> listener,
              bool options);

            public override extern void removeEventListener(
              string type,
              Union<dom.EventListener, dom.EventListenerObject> listener,
              dom.EventListenerOptions options);

            public override extern void removeEventListener(
              string type,
              dom.EventListener listener,
              bool options);

            public override extern void removeEventListener(
              string type,
              dom.EventListener listener,
              dom.EventListenerOptions options);

            public override extern void removeEventListener(
              string type,
              dom.EventListenerObject listener,
              bool options);

            public override extern void removeEventListener(
              string type,
              dom.EventListenerObject listener,
              dom.EventListenerOptions options);

            public virtual extern void removeEventListener(
              string type,
              Action<dom.Event> listener,
              Union<bool, dom.EventListenerOptions> options);

            public virtual extern void removeEventListener(
              string type,
              Action<dom.Event> listener,
              bool options);

            public virtual extern void removeEventListener(
              string type,
              Action<dom.Event> listener,
              dom.EventListenerOptions options);

            public virtual dom.GlobalEventHandlers.onpointercancelFn onpointercancel
            {
                get;
                set;
            }

            public virtual dom.GlobalEventHandlers.onpointercancelFn onpointerdown
            {
                get;
                set;
            }

            public virtual dom.GlobalEventHandlers.onpointercancelFn onpointerenter
            {
                get;
                set;
            }

            public virtual dom.GlobalEventHandlers.onpointercancelFn onpointerleave
            {
                get;
                set;
            }

            public virtual dom.GlobalEventHandlers.onpointercancelFn onpointermove
            {
                get;
                set;
            }

            public virtual dom.GlobalEventHandlers.onpointercancelFn onpointerout
            {
                get;
                set;
            }

            public virtual dom.GlobalEventHandlers.onpointercancelFn onpointerover
            {
                get;
                set;
            }

            public virtual dom.GlobalEventHandlers.onpointercancelFn onpointerup
            {
                get;
                set;
            }

            public virtual dom.GlobalEventHandlers.onwheelFn onwheel
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.GlobalEventHandlersEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.GlobalEventHandlers.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.GlobalEventHandlersEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.GlobalEventHandlers.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.GlobalEventHandlersEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.GlobalEventHandlers.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.GlobalEventHandlersEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.GlobalEventHandlers.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.GlobalEventHandlersEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.GlobalEventHandlers.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.GlobalEventHandlersEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.GlobalEventHandlers.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.GlobalEventHandlersEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.GlobalEventHandlers.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.GlobalEventHandlersEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.GlobalEventHandlers.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual uint childElementCount
            {
                get;
            }

            public virtual dom.Element firstElementChild
            {
                get;
            }

            public virtual dom.Element lastElementChild
            {
                get;
            }

            public virtual dom.HTMLCollection children
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.HTMLElementTagNameMap>), EnableImplicitConversion = true)]
            public virtual extern object querySelector<K>(K selectors);

            [Name("querySelector")]
            [Where("K", typeof(KeyOf<dom.SVGElementTagNameMap>), EnableImplicitConversion = true)]
            public virtual extern object querySelector2<K>(K selectors);

            [Where("E", typeof(dom.Element), EnableImplicitConversion = true)]
            public virtual extern Union<E, Null> querySelector<E>(string selectors);

            [Where("K", typeof(KeyOf<dom.HTMLElementTagNameMap>), EnableImplicitConversion = true)]
            public virtual extern dom.NodeListOf<dom.Node> querySelectorAll<K>(K selectors);

            [Name("querySelectorAll")]
            [Where("K", typeof(KeyOf<dom.SVGElementTagNameMap>), EnableImplicitConversion = true)]
            public virtual extern dom.NodeListOf<dom.Node> querySelectorAll2<K>(K selectors);

            [Where("E", typeof(dom.Element), EnableImplicitConversion = true)]
            public virtual extern dom.NodeListOf<E> querySelectorAll<E>(string selectors);

            public virtual extern dom.AnimationEvent createEvent(
              dom.Literals.Types.AnimationEvent eventInterface);

            public virtual extern dom.AnimationPlaybackEvent createEvent(
              dom.Literals.Types.AnimationPlaybackEvent eventInterface);

            public virtual extern dom.AudioProcessingEvent createEvent(
              dom.Literals.Types.AudioProcessingEvent eventInterface);

            public virtual extern dom.BeforeUnloadEvent createEvent(
              dom.Literals.Types.BeforeUnloadEvent eventInterface);

            public virtual extern dom.ClipboardEvent createEvent(
              dom.Literals.Types.ClipboardEvent eventInterface);

            public virtual extern dom.CloseEvent createEvent(
              dom.Literals.Types.CloseEvent eventInterface);

            public virtual extern dom.CompositionEvent createEvent(
              dom.Literals.Types.CompositionEvent eventInterface);

            public virtual extern dom.CustomEvent<object> createEvent(
              dom.Literals.Types.CustomEvent eventInterface);

            public virtual extern dom.DeviceLightEvent createEvent(
              dom.Literals.Types.DeviceLightEvent eventInterface);

            public virtual extern dom.DeviceMotionEvent createEvent(
              dom.Literals.Types.DeviceMotionEvent eventInterface);

            public virtual extern dom.DeviceOrientationEvent createEvent(
              dom.Literals.Types.DeviceOrientationEvent eventInterface);

            public virtual extern dom.DragEvent createEvent(dom.Literals.Types.DragEvent eventInterface);

            public virtual extern dom.ErrorEvent createEvent(
              dom.Literals.Types.ErrorEvent eventInterface);

            public virtual extern dom.Event createEvent(dom.Literals.Types.Event eventInterface);

            public virtual extern dom.Event createEvent(dom.Literals.Types.Events eventInterface);

            public virtual extern dom.FocusEvent createEvent(
              dom.Literals.Types.FocusEvent eventInterface);

            public virtual extern dom.FocusNavigationEvent createEvent(
              dom.Literals.Types.FocusNavigationEvent eventInterface);

            public virtual extern dom.GamepadEvent createEvent(
              dom.Literals.Types.GamepadEvent eventInterface);

            public virtual extern dom.HashChangeEvent createEvent(
              dom.Literals.Types.HashChangeEvent eventInterface);

            public virtual extern dom.IDBVersionChangeEvent createEvent(
              dom.Literals.Types.IDBVersionChangeEvent eventInterface);

            public virtual extern dom.KeyboardEvent createEvent(
              dom.Literals.Types.KeyboardEvent eventInterface);

            public virtual extern dom.ListeningStateChangedEvent createEvent(
              dom.Literals.Types.ListeningStateChangedEvent eventInterface);

            public virtual extern dom.MSDCCEvent createEvent(
              dom.Literals.Types.MSDCCEvent eventInterface);

            public virtual extern dom.MSDSHEvent createEvent(
              dom.Literals.Types.MSDSHEvent eventInterface);

            public virtual extern dom.MSMediaKeyMessageEvent createEvent(
              dom.Literals.Types.MSMediaKeyMessageEvent eventInterface);

            public virtual extern dom.MSMediaKeyNeededEvent createEvent(
              dom.Literals.Types.MSMediaKeyNeededEvent eventInterface);

            public virtual extern dom.MediaEncryptedEvent createEvent(
              dom.Literals.Types.MediaEncryptedEvent eventInterface);

            public virtual extern dom.MediaKeyMessageEvent createEvent(
              dom.Literals.Types.MediaKeyMessageEvent eventInterface);

            public virtual extern dom.MediaStreamErrorEvent createEvent(
              dom.Literals.Types.MediaStreamErrorEvent eventInterface);

            public virtual extern dom.MediaStreamEvent createEvent(
              dom.Literals.Types.MediaStreamEvent eventInterface);

            public virtual extern dom.MediaStreamTrackEvent createEvent(
              dom.Literals.Types.MediaStreamTrackEvent eventInterface);

            public virtual extern dom.MessageEvent createEvent(
              dom.Literals.Types.MessageEvent eventInterface);

            public virtual extern dom.MouseEvent createEvent(
              dom.Literals.Types.MouseEvent eventInterface);

            public virtual extern dom.MouseEvent createEvent(
              dom.Literals.Types.MouseEvents eventInterface);

            public virtual extern dom.MutationEvent createEvent(
              dom.Literals.Types.MutationEvent eventInterface);

            public virtual extern dom.MutationEvent createEvent(
              dom.Literals.Types.MutationEvents eventInterface);

            public virtual extern dom.OfflineAudioCompletionEvent createEvent(
              dom.Literals.Types.OfflineAudioCompletionEvent eventInterface);

            public virtual extern dom.OverflowEvent createEvent(
              dom.Literals.Types.OverflowEvent eventInterface);

            public virtual extern dom.PageTransitionEvent createEvent(
              dom.Literals.Types.PageTransitionEvent eventInterface);

            public virtual extern dom.PaymentRequestUpdateEvent createEvent(
              dom.Literals.Types.PaymentRequestUpdateEvent eventInterface);

            public virtual extern dom.PermissionRequestedEvent createEvent(
              dom.Literals.Types.PermissionRequestedEvent eventInterface);

            public virtual extern dom.PointerEvent createEvent(
              dom.Literals.Types.PointerEvent eventInterface);

            public virtual extern dom.PopStateEvent createEvent(
              dom.Literals.Types.PopStateEvent eventInterface);

            public virtual extern dom.ProgressEvent createEvent(
              dom.Literals.Types.ProgressEvent eventInterface);

            public virtual extern dom.PromiseRejectionEvent createEvent(
              dom.Literals.Types.PromiseRejectionEvent eventInterface);

            public virtual extern dom.RTCDTMFToneChangeEvent createEvent(
              dom.Literals.Types.RTCDTMFToneChangeEvent eventInterface);

            public virtual extern dom.RTCDtlsTransportStateChangedEvent createEvent(
              dom.Literals.Types.RTCDtlsTransportStateChangedEvent eventInterface);

            public virtual extern dom.RTCIceCandidatePairChangedEvent createEvent(
              dom.Literals.Types.RTCIceCandidatePairChangedEvent eventInterface);

            public virtual extern dom.RTCIceGathererEvent createEvent(
              dom.Literals.Types.RTCIceGathererEvent eventInterface);

            public virtual extern dom.RTCIceTransportStateChangedEvent createEvent(
              dom.Literals.Types.RTCIceTransportStateChangedEvent eventInterface);

            public virtual extern dom.RTCPeerConnectionIceEvent createEvent(
              dom.Literals.Types.RTCPeerConnectionIceEvent eventInterface);

            public virtual extern dom.RTCSsrcConflictEvent createEvent(
              dom.Literals.Types.RTCSsrcConflictEvent eventInterface);

            public virtual extern dom.SVGZoomEvent createEvent(
              dom.Literals.Types.SVGZoomEvent eventInterface);

            public virtual extern dom.SVGZoomEvent createEvent(
              dom.Literals.Types.SVGZoomEvents eventInterface);

            public virtual extern dom.SecurityPolicyViolationEvent createEvent(
              dom.Literals.Types.SecurityPolicyViolationEvent eventInterface);

            public virtual extern dom.ServiceWorkerMessageEvent createEvent(
              dom.Literals.Types.ServiceWorkerMessageEvent eventInterface);

            public virtual extern dom.SpeechSynthesisEvent createEvent(
              dom.Literals.Types.SpeechSynthesisEvent eventInterface);

            public virtual extern dom.StorageEvent createEvent(
              dom.Literals.Types.StorageEvent eventInterface);

            public virtual extern dom.TextEvent createEvent(dom.Literals.Types.TextEvent eventInterface);

            public virtual extern dom.TouchEvent createEvent(
              dom.Literals.Types.TouchEvent eventInterface);

            public virtual extern dom.TrackEvent createEvent(
              dom.Literals.Types.TrackEvent eventInterface);

            public virtual extern dom.TransitionEvent createEvent(
              dom.Literals.Types.TransitionEvent eventInterface);

            public virtual extern dom.UIEvent createEvent(dom.Literals.Types.UIEvent eventInterface);

            public virtual extern dom.UIEvent createEvent(dom.Literals.Types.UIEvents eventInterface);

            public virtual extern dom.VRDisplayEvent createEvent(
              dom.Literals.Types.VRDisplayEvent eventInterface);

            public virtual extern dom.VRDisplayEvent createEvent(
              dom.Literals.Types.VRDisplayEvent_ eventInterface);

            public virtual extern dom.WebGLContextEvent createEvent(
              dom.Literals.Types.WebGLContextEvent eventInterface);

            public virtual extern dom.WheelEvent createEvent(
              dom.Literals.Types.WheelEvent eventInterface);

            public virtual extern dom.Event createEvent(string eventInterface);

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
            public delegate void ontouchcancelFn(dom.TouchEvent ev);

            [Generated]
            public delegate void onvisibilitychangeFn(dom.Event ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.DocumentEventMap>), EnableImplicitConversion = true)]
            public delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.DocumentEventMap>), EnableImplicitConversion = true)]
            public delegate void removeEventListenerFn<K>(object ev);
        }

    }
}
