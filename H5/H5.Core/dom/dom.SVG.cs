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
        [Virtual]
        public abstract class SVGAElementTypeConfig : IObject
        {
            public virtual dom.SVGAElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGAElement New();
        }

        [Virtual]
        public abstract class SVGAngleTypeConfig : IObject
        {
            public virtual dom.SVGAngle prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGAngle New();

            public abstract ushort SVG_ANGLETYPE_DEG { get; }

            public abstract ushort SVG_ANGLETYPE_GRAD { get; }

            public abstract ushort SVG_ANGLETYPE_RAD { get; }

            public abstract ushort SVG_ANGLETYPE_UNKNOWN { get; }

            public abstract ushort SVG_ANGLETYPE_UNSPECIFIED { get; }
        }

        [Virtual]
        public abstract class SVGAnimatedAngleTypeConfig : IObject
        {
            public virtual dom.SVGAnimatedAngle prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGAnimatedAngle New();
        }

        [Virtual]
        public abstract class SVGAnimatedBooleanTypeConfig : IObject
        {
            public virtual dom.SVGAnimatedBoolean prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGAnimatedBoolean New();
        }

        [Virtual]
        public abstract class SVGAnimatedEnumerationTypeConfig : IObject
        {
            public virtual dom.SVGAnimatedEnumeration prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGAnimatedEnumeration New();
        }

        [Virtual]
        public abstract class SVGAnimatedIntegerTypeConfig : IObject
        {
            public virtual dom.SVGAnimatedInteger prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGAnimatedInteger New();
        }

        [Virtual]
        public abstract class SVGAnimatedLengthTypeConfig : IObject
        {
            public virtual dom.SVGAnimatedLength prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGAnimatedLength New();
        }

        [Virtual]
        public abstract class SVGAnimatedLengthListTypeConfig : IObject
        {
            public virtual dom.SVGAnimatedLengthList prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGAnimatedLengthList New();
        }

        [Virtual]
        public abstract class SVGAnimatedNumberTypeConfig : IObject
        {
            public virtual dom.SVGAnimatedNumber prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGAnimatedNumber New();
        }

        [Virtual]
        public abstract class SVGAnimatedNumberListTypeConfig : IObject
        {
            public virtual dom.SVGAnimatedNumberList prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGAnimatedNumberList New();
        }

        [Virtual]
        public abstract class SVGAnimatedPreserveAspectRatioTypeConfig : IObject
        {
            public virtual dom.SVGAnimatedPreserveAspectRatio prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGAnimatedPreserveAspectRatio New();
        }

        [Virtual]
        public abstract class SVGAnimatedRectTypeConfig : IObject
        {
            public virtual dom.SVGAnimatedRect prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGAnimatedRect New();
        }

        [Virtual]
        public abstract class SVGAnimatedStringTypeConfig : IObject
        {
            public virtual dom.SVGAnimatedString prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGAnimatedString New();
        }

        [Virtual]
        public abstract class SVGAnimatedTransformListTypeConfig : IObject
        {
            public virtual dom.SVGAnimatedTransformList prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGAnimatedTransformList New();
        }

        [Virtual]
        public abstract class SVGCircleElementTypeConfig : IObject
        {
            public virtual dom.SVGCircleElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGCircleElement New();
        }

        [Virtual]
        public abstract class SVGClipPathElementTypeConfig : IObject
        {
            public virtual dom.SVGClipPathElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGClipPathElement New();
        }

        [Virtual]
        public abstract class SVGComponentTransferFunctionElementTypeConfig : IObject
        {
            public virtual dom.SVGComponentTransferFunctionElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGComponentTransferFunctionElement New();

            public abstract double SVG_FECOMPONENTTRANSFER_TYPE_DISCRETE { get; }

            public abstract double SVG_FECOMPONENTTRANSFER_TYPE_GAMMA { get; }

            public abstract double SVG_FECOMPONENTTRANSFER_TYPE_IDENTITY { get; }

            public abstract double SVG_FECOMPONENTTRANSFER_TYPE_LINEAR { get; }

            public abstract double SVG_FECOMPONENTTRANSFER_TYPE_TABLE { get; }

            public abstract double SVG_FECOMPONENTTRANSFER_TYPE_UNKNOWN { get; }
        }

        [Virtual]
        public abstract class SVGDefsElementTypeConfig : IObject
        {
            public virtual dom.SVGDefsElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGDefsElement New();
        }

        [Virtual]
        public abstract class SVGDescElementTypeConfig : IObject
        {
            public virtual dom.SVGDescElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGDescElement New();
        }

        [Virtual]
        public abstract class SVGElementTypeConfig : IObject
        {
            public virtual dom.SVGElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGElement New();
        }

        [Virtual]
        public abstract class SVGElementInstanceTypeConfig : IObject
        {
            public virtual dom.SVGElementInstance prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGElementInstance New();
        }

        [Virtual]
        public abstract class SVGElementInstanceListTypeConfig : IObject
        {
            public virtual dom.SVGElementInstanceList prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGElementInstanceList New();
        }

        [Virtual]
        public abstract class SVGEllipseElementTypeConfig : IObject
        {
            public virtual dom.SVGEllipseElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGEllipseElement New();
        }

        [Virtual]
        public abstract class SVGFEBlendElementTypeConfig : IObject
        {
            public virtual dom.SVGFEBlendElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGFEBlendElement New();

            public abstract double SVG_FEBLEND_MODE_COLOR { get; }

            public abstract double SVG_FEBLEND_MODE_COLOR_BURN { get; }

            public abstract double SVG_FEBLEND_MODE_COLOR_DODGE { get; }

            public abstract double SVG_FEBLEND_MODE_DARKEN { get; }

            public abstract double SVG_FEBLEND_MODE_DIFFERENCE { get; }

            public abstract double SVG_FEBLEND_MODE_EXCLUSION { get; }

            public abstract double SVG_FEBLEND_MODE_HARD_LIGHT { get; }

            public abstract double SVG_FEBLEND_MODE_HUE { get; }

            public abstract double SVG_FEBLEND_MODE_LIGHTEN { get; }

            public abstract double SVG_FEBLEND_MODE_LUMINOSITY { get; }

            public abstract double SVG_FEBLEND_MODE_MULTIPLY { get; }

            public abstract double SVG_FEBLEND_MODE_NORMAL { get; }

            public abstract double SVG_FEBLEND_MODE_OVERLAY { get; }

            public abstract double SVG_FEBLEND_MODE_SATURATION { get; }

            public abstract double SVG_FEBLEND_MODE_SCREEN { get; }

            public abstract double SVG_FEBLEND_MODE_SOFT_LIGHT { get; }

            public abstract double SVG_FEBLEND_MODE_UNKNOWN { get; }
        }

        [Virtual]
        public abstract class SVGFEColorMatrixElementTypeConfig : IObject
        {
            public virtual dom.SVGFEColorMatrixElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGFEColorMatrixElement New();

            public abstract double SVG_FECOLORMATRIX_TYPE_HUEROTATE { get; }

            public abstract double SVG_FECOLORMATRIX_TYPE_LUMINANCETOALPHA { get; }

            public abstract double SVG_FECOLORMATRIX_TYPE_MATRIX { get; }

            public abstract double SVG_FECOLORMATRIX_TYPE_SATURATE { get; }

            public abstract double SVG_FECOLORMATRIX_TYPE_UNKNOWN { get; }
        }

        [Virtual]
        public abstract class SVGFEComponentTransferElementTypeConfig : IObject
        {
            public virtual dom.SVGFEComponentTransferElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGFEComponentTransferElement New();
        }

        [Virtual]
        public abstract class SVGFECompositeElementTypeConfig : IObject
        {
            public virtual dom.SVGFECompositeElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGFECompositeElement New();

            public abstract double SVG_FECOMPOSITE_OPERATOR_ARITHMETIC { get; }

            public abstract double SVG_FECOMPOSITE_OPERATOR_ATOP { get; }

            public abstract double SVG_FECOMPOSITE_OPERATOR_IN { get; }

            public abstract double SVG_FECOMPOSITE_OPERATOR_OUT { get; }

            public abstract double SVG_FECOMPOSITE_OPERATOR_OVER { get; }

            public abstract double SVG_FECOMPOSITE_OPERATOR_UNKNOWN { get; }

            public abstract double SVG_FECOMPOSITE_OPERATOR_XOR { get; }
        }

        [Virtual]
        public abstract class SVGFEConvolveMatrixElementTypeConfig : IObject
        {
            public virtual dom.SVGFEConvolveMatrixElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGFEConvolveMatrixElement New();

            public abstract double SVG_EDGEMODE_DUPLICATE { get; }

            public abstract double SVG_EDGEMODE_NONE { get; }

            public abstract double SVG_EDGEMODE_UNKNOWN { get; }

            public abstract double SVG_EDGEMODE_WRAP { get; }
        }

        [Virtual]
        public abstract class SVGFEDiffuseLightingElementTypeConfig : IObject
        {
            public virtual dom.SVGFEDiffuseLightingElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGFEDiffuseLightingElement New();
        }

        [Virtual]
        public abstract class SVGFEDisplacementMapElementTypeConfig : IObject
        {
            public virtual dom.SVGFEDisplacementMapElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGFEDisplacementMapElement New();

            public abstract double SVG_CHANNEL_A { get; }

            public abstract double SVG_CHANNEL_B { get; }

            public abstract double SVG_CHANNEL_G { get; }

            public abstract double SVG_CHANNEL_R { get; }

            public abstract double SVG_CHANNEL_UNKNOWN { get; }
        }

        [Virtual]
        public abstract class SVGFEDistantLightElementTypeConfig : IObject
        {
            public virtual dom.SVGFEDistantLightElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGFEDistantLightElement New();
        }

        [Virtual]
        public abstract class SVGFEFloodElementTypeConfig : IObject
        {
            public virtual dom.SVGFEFloodElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGFEFloodElement New();
        }

        [Virtual]
        public abstract class SVGFEFuncAElementTypeConfig : IObject
        {
            public virtual dom.SVGFEFuncAElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGFEFuncAElement New();
        }

        [Virtual]
        public abstract class SVGFEFuncBElementTypeConfig : IObject
        {
            public virtual dom.SVGFEFuncBElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGFEFuncBElement New();
        }

        [Virtual]
        public abstract class SVGFEFuncGElementTypeConfig : IObject
        {
            public virtual dom.SVGFEFuncGElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGFEFuncGElement New();
        }

        [Virtual]
        public abstract class SVGFEFuncRElementTypeConfig : IObject
        {
            public virtual dom.SVGFEFuncRElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGFEFuncRElement New();
        }

        [Virtual]
        public abstract class SVGFEGaussianBlurElementTypeConfig : IObject
        {
            public virtual dom.SVGFEGaussianBlurElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGFEGaussianBlurElement New();
        }

        [Virtual]
        public abstract class SVGFEImageElementTypeConfig : IObject
        {
            public virtual dom.SVGFEImageElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGFEImageElement New();
        }

        [Virtual]
        public abstract class SVGFEMergeElementTypeConfig : IObject
        {
            public virtual dom.SVGFEMergeElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGFEMergeElement New();
        }

        [Virtual]
        public abstract class SVGFEMergeNodeElementTypeConfig : IObject
        {
            public virtual dom.SVGFEMergeNodeElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGFEMergeNodeElement New();
        }

        [Virtual]
        public abstract class SVGFEMorphologyElementTypeConfig : IObject
        {
            public virtual dom.SVGFEMorphologyElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGFEMorphologyElement New();

            public abstract double SVG_MORPHOLOGY_OPERATOR_DILATE { get; }

            public abstract double SVG_MORPHOLOGY_OPERATOR_ERODE { get; }

            public abstract double SVG_MORPHOLOGY_OPERATOR_UNKNOWN { get; }
        }

        [Virtual]
        public abstract class SVGFEOffsetElementTypeConfig : IObject
        {
            public virtual dom.SVGFEOffsetElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGFEOffsetElement New();
        }

        [Virtual]
        public abstract class SVGFEPointLightElementTypeConfig : IObject
        {
            public virtual dom.SVGFEPointLightElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGFEPointLightElement New();
        }

        [Virtual]
        public abstract class SVGFESpecularLightingElementTypeConfig : IObject
        {
            public virtual dom.SVGFESpecularLightingElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGFESpecularLightingElement New();
        }

        [Virtual]
        public abstract class SVGFESpotLightElementTypeConfig : IObject
        {
            public virtual dom.SVGFESpotLightElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGFESpotLightElement New();
        }

        [Virtual]
        public abstract class SVGFETileElementTypeConfig : IObject
        {
            public virtual dom.SVGFETileElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGFETileElement New();
        }

        [Virtual]
        public abstract class SVGFETurbulenceElementTypeConfig : IObject
        {
            public virtual dom.SVGFETurbulenceElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGFETurbulenceElement New();

            public abstract double SVG_STITCHTYPE_NOSTITCH { get; }

            public abstract double SVG_STITCHTYPE_STITCH { get; }

            public abstract double SVG_STITCHTYPE_UNKNOWN { get; }

            public abstract double SVG_TURBULENCE_TYPE_FRACTALNOISE { get; }

            public abstract double SVG_TURBULENCE_TYPE_TURBULENCE { get; }

            public abstract double SVG_TURBULENCE_TYPE_UNKNOWN { get; }
        }

        [Virtual]
        public abstract class SVGFilterElementTypeConfig : IObject
        {
            public virtual dom.SVGFilterElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGFilterElement New();
        }

        [Virtual]
        public abstract class SVGForeignObjectElementTypeConfig : IObject
        {
            public virtual dom.SVGForeignObjectElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGForeignObjectElement New();
        }

        [Virtual]
        public abstract class SVGGElementTypeConfig : IObject
        {
            public virtual dom.SVGGElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGGElement New();
        }

        [Virtual]
        public abstract class SVGGradientElementTypeConfig : IObject
        {
            public virtual dom.SVGGradientElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGGradientElement New();

            public abstract ushort SVG_SPREADMETHOD_PAD { get; }

            public abstract ushort SVG_SPREADMETHOD_REFLECT { get; }

            public abstract ushort SVG_SPREADMETHOD_REPEAT { get; }

            public abstract ushort SVG_SPREADMETHOD_UNKNOWN { get; }
        }

        [Virtual]
        public abstract class SVGGraphicsElementTypeConfig : IObject
        {
            public virtual dom.SVGGraphicsElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGGraphicsElement New();
        }

        [Virtual]
        public abstract class SVGImageElementTypeConfig : IObject
        {
            public virtual dom.SVGImageElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGImageElement New();
        }

        [Virtual]
        public abstract class SVGLengthTypeConfig : IObject
        {
            public virtual dom.SVGLength prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGLength New();

            public abstract ushort SVG_LENGTHTYPE_CM { get; }

            public abstract ushort SVG_LENGTHTYPE_EMS { get; }

            public abstract ushort SVG_LENGTHTYPE_EXS { get; }

            public abstract ushort SVG_LENGTHTYPE_IN { get; }

            public abstract ushort SVG_LENGTHTYPE_MM { get; }

            public abstract ushort SVG_LENGTHTYPE_NUMBER { get; }

            public abstract ushort SVG_LENGTHTYPE_PC { get; }

            public abstract ushort SVG_LENGTHTYPE_PERCENTAGE { get; }

            public abstract ushort SVG_LENGTHTYPE_PT { get; }

            public abstract ushort SVG_LENGTHTYPE_PX { get; }

            public abstract ushort SVG_LENGTHTYPE_UNKNOWN { get; }
        }

        [Virtual]
        public abstract class SVGLengthListTypeConfig : IObject
        {
            public virtual dom.SVGLengthList prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGLengthList New();
        }

        [Virtual]
        public abstract class SVGLineElementTypeConfig : IObject
        {
            public virtual dom.SVGLineElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGLineElement New();
        }

        [Virtual]
        public abstract class SVGLinearGradientElementTypeConfig : IObject
        {
            public virtual dom.SVGLinearGradientElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGLinearGradientElement New();
        }

        [Virtual]
        public abstract class SVGMarkerElementTypeConfig : IObject
        {
            public virtual dom.SVGMarkerElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGMarkerElement New();

            public abstract ushort SVG_MARKERUNITS_STROKEWIDTH { get; }

            public abstract ushort SVG_MARKERUNITS_UNKNOWN { get; }

            public abstract ushort SVG_MARKERUNITS_USERSPACEONUSE { get; }

            public abstract ushort SVG_MARKER_ORIENT_ANGLE { get; }

            public abstract ushort SVG_MARKER_ORIENT_AUTO { get; }

            public abstract ushort SVG_MARKER_ORIENT_UNKNOWN { get; }
        }

        [Virtual]
        public abstract class SVGMaskElementTypeConfig : IObject
        {
            public virtual dom.SVGMaskElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGMaskElement New();
        }

        [Virtual]
        public abstract class SVGMatrixTypeConfig : IObject
        {
            public virtual dom.SVGMatrix prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGMatrix New();
        }

        [Virtual]
        public abstract class SVGMetadataElementTypeConfig : IObject
        {
            public virtual dom.SVGMetadataElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGMetadataElement New();
        }

        [Virtual]
        public abstract class SVGNumberTypeConfig : IObject
        {
            public virtual dom.SVGNumber prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGNumber New();
        }

        [Virtual]
        public abstract class SVGNumberListTypeConfig : IObject
        {
            public virtual dom.SVGNumberList prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGNumberList New();
        }

        [Virtual]
        public abstract class SVGPathElementTypeConfig : IObject
        {
            public virtual dom.SVGPathElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGPathElement New();
        }

        [Virtual]
        public abstract class SVGPathSegTypeConfig : IObject
        {
            public virtual dom.SVGPathSeg prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGPathSeg New();

            public abstract double PATHSEG_ARC_ABS { get; }

            public abstract double PATHSEG_ARC_REL { get; }

            public abstract double PATHSEG_CLOSEPATH { get; }

            public abstract double PATHSEG_CURVETO_CUBIC_ABS { get; }

            public abstract double PATHSEG_CURVETO_CUBIC_REL { get; }

            public abstract double PATHSEG_CURVETO_CUBIC_SMOOTH_ABS { get; }

            public abstract double PATHSEG_CURVETO_CUBIC_SMOOTH_REL { get; }

            public abstract double PATHSEG_CURVETO_QUADRATIC_ABS { get; }

            public abstract double PATHSEG_CURVETO_QUADRATIC_REL { get; }

            public abstract double PATHSEG_CURVETO_QUADRATIC_SMOOTH_ABS { get; }

            public abstract double PATHSEG_CURVETO_QUADRATIC_SMOOTH_REL { get; }

            public abstract double PATHSEG_LINETO_ABS { get; }

            public abstract double PATHSEG_LINETO_HORIZONTAL_ABS { get; }

            public abstract double PATHSEG_LINETO_HORIZONTAL_REL { get; }

            public abstract double PATHSEG_LINETO_REL { get; }

            public abstract double PATHSEG_LINETO_VERTICAL_ABS { get; }

            public abstract double PATHSEG_LINETO_VERTICAL_REL { get; }

            public abstract double PATHSEG_MOVETO_ABS { get; }

            public abstract double PATHSEG_MOVETO_REL { get; }

            public abstract double PATHSEG_UNKNOWN { get; }
        }

        [Virtual]
        public abstract class SVGPathSegArcAbsTypeConfig : IObject
        {
            public virtual dom.SVGPathSegArcAbs prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGPathSegArcAbs New();
        }

        [Virtual]
        public abstract class SVGPathSegArcRelTypeConfig : IObject
        {
            public virtual dom.SVGPathSegArcRel prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGPathSegArcRel New();
        }

        [Virtual]
        public abstract class SVGPathSegClosePathTypeConfig : IObject
        {
            public virtual dom.SVGPathSegClosePath prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGPathSegClosePath New();
        }

        [Virtual]
        public abstract class SVGPathSegCurvetoCubicAbsTypeConfig : IObject
        {
            public virtual dom.SVGPathSegCurvetoCubicAbs prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGPathSegCurvetoCubicAbs New();
        }

        [Virtual]
        public abstract class SVGPathSegCurvetoCubicRelTypeConfig : IObject
        {
            public virtual dom.SVGPathSegCurvetoCubicRel prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGPathSegCurvetoCubicRel New();
        }

        [Virtual]
        public abstract class SVGPathSegCurvetoCubicSmoothAbsTypeConfig : IObject
        {
            public virtual dom.SVGPathSegCurvetoCubicSmoothAbs prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGPathSegCurvetoCubicSmoothAbs New();
        }

        [Virtual]
        public abstract class SVGPathSegCurvetoCubicSmoothRelTypeConfig : IObject
        {
            public virtual dom.SVGPathSegCurvetoCubicSmoothRel prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGPathSegCurvetoCubicSmoothRel New();
        }

        [Virtual]
        public abstract class SVGPathSegCurvetoQuadraticAbsTypeConfig : IObject
        {
            public virtual dom.SVGPathSegCurvetoQuadraticAbs prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGPathSegCurvetoQuadraticAbs New();
        }

        [Virtual]
        public abstract class SVGPathSegCurvetoQuadraticRelTypeConfig : IObject
        {
            public virtual dom.SVGPathSegCurvetoQuadraticRel prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGPathSegCurvetoQuadraticRel New();
        }

        [Virtual]
        public abstract class SVGPathSegCurvetoQuadraticSmoothAbsTypeConfig : IObject
        {
            public virtual dom.SVGPathSegCurvetoQuadraticSmoothAbs prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGPathSegCurvetoQuadraticSmoothAbs New();
        }

        [Virtual]
        public abstract class SVGPathSegCurvetoQuadraticSmoothRelTypeConfig : IObject
        {
            public virtual dom.SVGPathSegCurvetoQuadraticSmoothRel prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGPathSegCurvetoQuadraticSmoothRel New();
        }

        [Virtual]
        public abstract class SVGPathSegLinetoAbsTypeConfig : IObject
        {
            public virtual dom.SVGPathSegLinetoAbs prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGPathSegLinetoAbs New();
        }

        [Virtual]
        public abstract class SVGPathSegLinetoHorizontalAbsTypeConfig : IObject
        {
            public virtual dom.SVGPathSegLinetoHorizontalAbs prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGPathSegLinetoHorizontalAbs New();
        }

        [Virtual]
        public abstract class SVGPathSegLinetoHorizontalRelTypeConfig : IObject
        {
            public virtual dom.SVGPathSegLinetoHorizontalRel prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGPathSegLinetoHorizontalRel New();
        }

        [Virtual]
        public abstract class SVGPathSegLinetoRelTypeConfig : IObject
        {
            public virtual dom.SVGPathSegLinetoRel prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGPathSegLinetoRel New();
        }

        [Virtual]
        public abstract class SVGPathSegLinetoVerticalAbsTypeConfig : IObject
        {
            public virtual dom.SVGPathSegLinetoVerticalAbs prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGPathSegLinetoVerticalAbs New();
        }

        [Virtual]
        public abstract class SVGPathSegLinetoVerticalRelTypeConfig : IObject
        {
            public virtual dom.SVGPathSegLinetoVerticalRel prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGPathSegLinetoVerticalRel New();
        }

        [Virtual]
        public abstract class SVGPathSegListTypeConfig : IObject
        {
            public virtual dom.SVGPathSegList prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGPathSegList New();
        }

        [Virtual]
        public abstract class SVGPathSegMovetoAbsTypeConfig : IObject
        {
            public virtual dom.SVGPathSegMovetoAbs prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGPathSegMovetoAbs New();
        }

        [Virtual]
        public abstract class SVGPathSegMovetoRelTypeConfig : IObject
        {
            public virtual dom.SVGPathSegMovetoRel prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGPathSegMovetoRel New();
        }

        [Virtual]
        public abstract class SVGPatternElementTypeConfig : IObject
        {
            public virtual dom.SVGPatternElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGPatternElement New();
        }

        [Virtual]
        public abstract class SVGPointTypeConfig : IObject
        {
            public virtual dom.SVGPoint prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGPoint New();
        }

        [Virtual]
        public abstract class SVGPointListTypeConfig : IObject
        {
            public virtual dom.SVGPointList prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGPointList New();
        }

        [Virtual]
        public abstract class SVGPolygonElementTypeConfig : IObject
        {
            public virtual dom.SVGPolygonElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGPolygonElement New();
        }

        [Virtual]
        public abstract class SVGPolylineElementTypeConfig : IObject
        {
            public virtual dom.SVGPolylineElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGPolylineElement New();
        }

        [Virtual]
        public abstract class SVGPreserveAspectRatioTypeConfig : IObject
        {
            public virtual dom.SVGPreserveAspectRatio prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGPreserveAspectRatio New();

            public abstract ushort SVG_MEETORSLICE_MEET { get; }

            public abstract ushort SVG_MEETORSLICE_SLICE { get; }

            public abstract ushort SVG_MEETORSLICE_UNKNOWN { get; }

            public abstract ushort SVG_PRESERVEASPECTRATIO_NONE { get; }

            public abstract ushort SVG_PRESERVEASPECTRATIO_UNKNOWN { get; }

            public abstract ushort SVG_PRESERVEASPECTRATIO_XMAXYMAX { get; }

            public abstract ushort SVG_PRESERVEASPECTRATIO_XMAXYMID { get; }

            public abstract ushort SVG_PRESERVEASPECTRATIO_XMAXYMIN { get; }

            public abstract ushort SVG_PRESERVEASPECTRATIO_XMIDYMAX { get; }

            public abstract ushort SVG_PRESERVEASPECTRATIO_XMIDYMID { get; }

            public abstract ushort SVG_PRESERVEASPECTRATIO_XMIDYMIN { get; }

            public abstract ushort SVG_PRESERVEASPECTRATIO_XMINYMAX { get; }

            public abstract ushort SVG_PRESERVEASPECTRATIO_XMINYMID { get; }

            public abstract ushort SVG_PRESERVEASPECTRATIO_XMINYMIN { get; }
        }

        [Virtual]
        public abstract class SVGRadialGradientElementTypeConfig : IObject
        {
            public virtual dom.SVGRadialGradientElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGRadialGradientElement New();
        }

        [Virtual]
        public abstract class SVGRectTypeConfig : IObject
        {
            public virtual dom.SVGRect prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGRect New();
        }

        [Virtual]
        public abstract class SVGRectElementTypeConfig : IObject
        {
            public virtual dom.SVGRectElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGRectElement New();
        }

        [Virtual]
        public abstract class SVGSVGElementTypeConfig : IObject
        {
            public virtual dom.SVGSVGElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGSVGElement New();
        }

        [Virtual]
        public abstract class SVGScriptElementTypeConfig : IObject
        {
            public virtual dom.SVGScriptElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGScriptElement New();
        }

        [Virtual]
        public abstract class SVGStopElementTypeConfig : IObject
        {
            public virtual dom.SVGStopElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGStopElement New();
        }

        [Virtual]
        public abstract class SVGStringListTypeConfig : IObject
        {
            public virtual dom.SVGStringList prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGStringList New();
        }

        [Virtual]
        public abstract class SVGStylableTypeConfig : IObject
        {
            public virtual dom.SVGStylable prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGStylable New();
        }

        [Virtual]
        public abstract class SVGStyleElementTypeConfig : IObject
        {
            public virtual dom.SVGStyleElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGStyleElement New();
        }

        [Virtual]
        public abstract class SVGSwitchElementTypeConfig : IObject
        {
            public virtual dom.SVGSwitchElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGSwitchElement New();
        }

        [Virtual]
        public abstract class SVGSymbolElementTypeConfig : IObject
        {
            public virtual dom.SVGSymbolElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGSymbolElement New();
        }

        [Virtual]
        public abstract class SVGTSpanElementTypeConfig : IObject
        {
            public virtual dom.SVGTSpanElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGTSpanElement New();
        }

        [Virtual]
        public abstract class SVGTextContentElementTypeConfig : IObject
        {
            public virtual dom.SVGTextContentElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGTextContentElement New();

            public abstract ushort LENGTHADJUST_SPACING { get; }

            public abstract ushort LENGTHADJUST_SPACINGANDGLYPHS { get; }

            public abstract ushort LENGTHADJUST_UNKNOWN { get; }
        }

        [Virtual]
        public abstract class SVGTextElementTypeConfig : IObject
        {
            public virtual dom.SVGTextElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGTextElement New();
        }

        [Virtual]
        public abstract class SVGTextPathElementTypeConfig : IObject
        {
            public virtual dom.SVGTextPathElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGTextPathElement New();

            public abstract ushort TEXTPATH_METHODTYPE_ALIGN { get; }

            public abstract ushort TEXTPATH_METHODTYPE_STRETCH { get; }

            public abstract ushort TEXTPATH_METHODTYPE_UNKNOWN { get; }

            public abstract ushort TEXTPATH_SPACINGTYPE_AUTO { get; }

            public abstract ushort TEXTPATH_SPACINGTYPE_EXACT { get; }

            public abstract ushort TEXTPATH_SPACINGTYPE_UNKNOWN { get; }
        }

        [Virtual]
        public abstract class SVGTextPositioningElementTypeConfig : IObject
        {
            public virtual dom.SVGTextPositioningElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGTextPositioningElement New();
        }

        [Virtual]
        public abstract class SVGTitleElementTypeConfig : IObject
        {
            public virtual dom.SVGTitleElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGTitleElement New();
        }

        [Virtual]
        public abstract class SVGTransformTypeConfig : IObject
        {
            public virtual dom.SVGTransform prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGTransform New();

            public abstract ushort SVG_TRANSFORM_MATRIX { get; }

            public abstract ushort SVG_TRANSFORM_ROTATE { get; }

            public abstract ushort SVG_TRANSFORM_SCALE { get; }

            public abstract ushort SVG_TRANSFORM_SKEWX { get; }

            public abstract ushort SVG_TRANSFORM_SKEWY { get; }

            public abstract ushort SVG_TRANSFORM_TRANSLATE { get; }

            public abstract ushort SVG_TRANSFORM_UNKNOWN { get; }
        }

        [Virtual]
        public abstract class SVGTransformListTypeConfig : IObject
        {
            public virtual dom.SVGTransformList prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGTransformList New();
        }

        [Virtual]
        public abstract class SVGUseElementTypeConfig : IObject
        {
            public virtual dom.SVGUseElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGUseElement New();
        }

        [Virtual]
        public abstract class SVGViewElementTypeConfig : IObject
        {
            public virtual dom.SVGViewElement prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGViewElement New();
        }

        [Virtual]
        public abstract class SVGZoomAndPan2Config : IObject
        {
            public abstract ushort SVG_ZOOMANDPAN_DISABLE { get; }

            public abstract ushort SVG_ZOOMANDPAN_MAGNIFY { get; }

            public abstract ushort SVG_ZOOMANDPAN_UNKNOWN { get; }
        }

        [Virtual]
        public abstract class SVGZoomEventTypeConfig : IObject
        {
            public virtual dom.SVGZoomEvent prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract dom.SVGZoomEvent New();
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGAElement : dom.SVGGraphicsElement, dom.SVGURIReference.Interface, IObject
        {


            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"a\")")]
            public extern SVGAElement();

            public static dom.SVGAElement prototype
            {
                get;
                set;
            }

            public virtual dom.SVGAnimatedString target
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGAElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGAElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGAElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGAElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGAElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGAElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGAElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGAElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual dom.SVGAnimatedString href
            {
                get;
            }

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGAngle : IObject
        {






            public static dom.SVGAngle prototype
            {
                get;
                set;
            }

            [Name("SVG_ANGLETYPE_DEG")]
            public static ushort SVG_ANGLETYPE_DEG_Static
            {
                get;
            }

            [Name("SVG_ANGLETYPE_GRAD")]
            public static ushort SVG_ANGLETYPE_GRAD_Static
            {
                get;
            }

            [Name("SVG_ANGLETYPE_RAD")]
            public static ushort SVG_ANGLETYPE_RAD_Static
            {
                get;
            }

            [Name("SVG_ANGLETYPE_UNKNOWN")]
            public static ushort SVG_ANGLETYPE_UNKNOWN_Static
            {
                get;
            }

            [Name("SVG_ANGLETYPE_UNSPECIFIED")]
            public static ushort SVG_ANGLETYPE_UNSPECIFIED_Static
            {
                get;
            }

            public virtual ushort unitType
            {
                get;
            }

            public virtual float value
            {
                get;
                set;
            }

            public virtual string valueAsString
            {
                get;
                set;
            }

            public virtual float valueInSpecifiedUnits
            {
                get;
                set;
            }

            public virtual extern void convertToSpecifiedUnits(ushort unitType);

            public virtual extern void newValueSpecifiedUnits(
              ushort unitType,
              float valueInSpecifiedUnits);

            public virtual ushort SVG_ANGLETYPE_DEG
            {
                get;
            }

            public virtual ushort SVG_ANGLETYPE_GRAD
            {
                get;
            }

            public virtual ushort SVG_ANGLETYPE_RAD
            {
                get;
            }

            public virtual ushort SVG_ANGLETYPE_UNKNOWN
            {
                get;
            }

            public virtual ushort SVG_ANGLETYPE_UNSPECIFIED
            {
                get;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGAnimatedAngle : IObject
        {


            public static dom.SVGAnimatedAngle prototype
            {
                get;
                set;
            }

            public virtual dom.SVGAngle animVal
            {
                get;
            }

            public virtual dom.SVGAngle baseVal
            {
                get;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGAnimatedBoolean : IObject
        {


            public static dom.SVGAnimatedBoolean prototype
            {
                get;
                set;
            }

            public virtual bool animVal
            {
                get;
            }

            public virtual bool baseVal
            {
                get;
                set;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGAnimatedEnumeration : IObject
        {


            public static dom.SVGAnimatedEnumeration prototype
            {
                get;
                set;
            }

            public virtual ushort animVal
            {
                get;
            }

            public virtual ushort baseVal
            {
                get;
                set;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGAnimatedInteger : IObject
        {


            public static dom.SVGAnimatedInteger prototype
            {
                get;
                set;
            }

            public virtual int animVal
            {
                get;
            }

            public virtual int baseVal
            {
                get;
                set;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGAnimatedLength : IObject
        {


            public static dom.SVGAnimatedLength prototype
            {
                get;
                set;
            }

            public virtual dom.SVGLength animVal
            {
                get;
            }

            public virtual dom.SVGLength baseVal
            {
                get;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGAnimatedLengthList : IObject
        {


            public static dom.SVGAnimatedLengthList prototype
            {
                get;
                set;
            }

            public virtual dom.SVGLengthList animVal
            {
                get;
            }

            public virtual dom.SVGLengthList baseVal
            {
                get;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGAnimatedNumber : IObject
        {


            public static dom.SVGAnimatedNumber prototype
            {
                get;
                set;
            }

            public virtual float animVal
            {
                get;
            }

            public virtual float baseVal
            {
                get;
                set;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGAnimatedNumberList : IObject
        {


            public static dom.SVGAnimatedNumberList prototype
            {
                get;
                set;
            }

            public virtual dom.SVGNumberList animVal
            {
                get;
            }

            public virtual dom.SVGNumberList baseVal
            {
                get;
            }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [InterfaceWrapper]
        public class SVGAnimatedPoints : dom.SVGAnimatedPoints.Interface, IObject
        {

            public dom.SVGPointList animatedPoints
            {
                get;
            }

            public dom.SVGPointList points
            {
                get;
            }

            [Generated]
            [ObjectLiteral]
            [IgnoreCast]
            public interface Interface : IObject
            {
                dom.SVGPointList animatedPoints { get; }

                dom.SVGPointList points { get; }
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGAnimatedPreserveAspectRatio : IObject
        {


            public static dom.SVGAnimatedPreserveAspectRatio prototype
            {
                get;
                set;
            }

            public virtual dom.SVGPreserveAspectRatio animVal
            {
                get;
            }

            public virtual dom.SVGPreserveAspectRatio baseVal
            {
                get;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGAnimatedRect : IObject
        {


            public static dom.SVGAnimatedRect prototype
            {
                get;
                set;
            }

            public virtual dom.SVGRect animVal
            {
                get;
            }

            public virtual dom.SVGRect baseVal
            {
                get;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGAnimatedString : IObject
        {


            public static dom.SVGAnimatedString prototype
            {
                get;
                set;
            }

            public virtual string animVal
            {
                get;
            }

            public virtual string baseVal
            {
                get;
                set;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGAnimatedTransformList : IObject
        {


            public static dom.SVGAnimatedTransformList prototype
            {
                get;
                set;
            }

            public virtual dom.SVGTransformList animVal
            {
                get;
            }

            public virtual dom.SVGTransformList baseVal
            {
                get;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGCircleElement : dom.SVGGraphicsElement
        {

            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"circle\")")]
            public extern SVGCircleElement();

            public static dom.SVGCircleElement prototype
            {
                get;
                set;
            }

            public virtual dom.SVGAnimatedLength cx
            {
                get;
            }

            public virtual dom.SVGAnimatedLength cy
            {
                get;
            }

            public virtual dom.SVGAnimatedLength r
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGCircleElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGCircleElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGCircleElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGCircleElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGCircleElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGCircleElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGCircleElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGCircleElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGClipPathElement : dom.SVGGraphicsElement, dom.SVGUnitTypes.Interface, IObject
        {


            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"clipPath\")")]
            public extern SVGClipPathElement();

            public static dom.SVGClipPathElement prototype
            {
                get;
                set;
            }

            public virtual dom.SVGAnimatedEnumeration clipPathUnits
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGClipPathElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGClipPathElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGClipPathElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGClipPathElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGClipPathElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGClipPathElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGClipPathElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGClipPathElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual ushort SVG_UNIT_TYPE_OBJECTBOUNDINGBOX
            {
                get;
            }

            public virtual ushort SVG_UNIT_TYPE_UNKNOWN
            {
                get;
            }

            public virtual ushort SVG_UNIT_TYPE_USERSPACEONUSE
            {
                get;
            }

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGComponentTransferFunctionElement : dom.SVGElement
        {







            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"componentTransferFunction\")")]
            public extern SVGComponentTransferFunctionElement();

            public static dom.SVGComponentTransferFunctionElement prototype
            {
                get;
                set;
            }

            [Name("SVG_FECOMPONENTTRANSFER_TYPE_DISCRETE")]
            public static double SVG_FECOMPONENTTRANSFER_TYPE_DISCRETE_Static
            {
                get;
            }

            [Name("SVG_FECOMPONENTTRANSFER_TYPE_GAMMA")]
            public static double SVG_FECOMPONENTTRANSFER_TYPE_GAMMA_Static
            {
                get;
            }

            [Name("SVG_FECOMPONENTTRANSFER_TYPE_IDENTITY")]
            public static double SVG_FECOMPONENTTRANSFER_TYPE_IDENTITY_Static
            {
                get;
            }

            [Name("SVG_FECOMPONENTTRANSFER_TYPE_LINEAR")]
            public static double SVG_FECOMPONENTTRANSFER_TYPE_LINEAR_Static
            {
                get;
            }

            [Name("SVG_FECOMPONENTTRANSFER_TYPE_TABLE")]
            public static double SVG_FECOMPONENTTRANSFER_TYPE_TABLE_Static
            {
                get;
            }

            [Name("SVG_FECOMPONENTTRANSFER_TYPE_UNKNOWN")]
            public static double SVG_FECOMPONENTTRANSFER_TYPE_UNKNOWN_Static
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber amplitude
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber exponent
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber intercept
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber offset
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber slope
            {
                get;
            }

            public virtual dom.SVGAnimatedNumberList tableValues
            {
                get;
            }

            public virtual dom.SVGAnimatedEnumeration type
            {
                get;
            }

            public virtual double SVG_FECOMPONENTTRANSFER_TYPE_DISCRETE
            {
                get;
            }

            public virtual double SVG_FECOMPONENTTRANSFER_TYPE_GAMMA
            {
                get;
            }

            public virtual double SVG_FECOMPONENTTRANSFER_TYPE_IDENTITY
            {
                get;
            }

            public virtual double SVG_FECOMPONENTTRANSFER_TYPE_LINEAR
            {
                get;
            }

            public virtual double SVG_FECOMPONENTTRANSFER_TYPE_TABLE
            {
                get;
            }

            public virtual double SVG_FECOMPONENTTRANSFER_TYPE_UNKNOWN
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGComponentTransferFunctionElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGComponentTransferFunctionElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGComponentTransferFunctionElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGComponentTransferFunctionElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGComponentTransferFunctionElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGComponentTransferFunctionElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGComponentTransferFunctionElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGComponentTransferFunctionElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGDefsElement : dom.SVGGraphicsElement
        {
            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"defs\")")]
            public extern SVGDefsElement();

            public static dom.SVGDefsElement prototype
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGDefsElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGDefsElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGDefsElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGDefsElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGDefsElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGDefsElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGDefsElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGDefsElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGDescElement : dom.SVGElement
        {
            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"desc\")")]
            public extern SVGDescElement();

            public static dom.SVGDescElement prototype
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGDescElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGDescElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGDescElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGDescElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGDescElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGDescElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGDescElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGDescElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class SVGElementEventMap : dom.ElementEventMap
        {



            public dom.MouseEvent click
            {
                get;
                set;
            }

            public dom.MouseEvent dblclick
            {
                get;
                set;
            }

            public dom.FocusEvent focusin
            {
                get;
                set;
            }

            public dom.FocusEvent focusout
            {
                get;
                set;
            }

            public dom.Event load
            {
                get;
                set;
            }

            public dom.MouseEvent mousedown
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

            [Generated]
            public new static class KeyOf
            {
                [Template("\"click\"")]
                public static readonly KeyOf<dom.SVGElementEventMap> click;
                [Template("\"dblclick\"")]
                public static readonly KeyOf<dom.SVGElementEventMap> dblclick;
                [Template("\"focusin\"")]
                public static readonly KeyOf<dom.SVGElementEventMap> focusin;
                [Template("\"focusout\"")]
                public static readonly KeyOf<dom.SVGElementEventMap> focusout;
                [Template("\"load\"")]
                public static readonly KeyOf<dom.SVGElementEventMap> load;
                [Template("\"mousedown\"")]
                public static readonly KeyOf<dom.SVGElementEventMap> mousedown;
                [Template("\"mousemove\"")]
                public static readonly KeyOf<dom.SVGElementEventMap> mousemove;
                [Template("\"mouseout\"")]
                public static readonly KeyOf<dom.SVGElementEventMap> mouseout;
                [Template("\"mouseover\"")]
                public static readonly KeyOf<dom.SVGElementEventMap> mouseover;
                [Template("\"mouseup\"")]
                public static readonly KeyOf<dom.SVGElementEventMap> mouseup;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGElement : dom.Element, dom.ElementCSSInlineStyle.Interface, IObject
        {





            protected extern SVGElement();

            public static dom.SVGElement prototype
            {
                get;
                set;
            }

            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", {0})")]
            public extern SVGElement(string name);

            public virtual object className
            {
                get;
            }

            public virtual dom.SVGElement.onclickFn onclick
            {
                get;
                set;
            }

            public virtual dom.SVGElement.onclickFn ondblclick
            {
                get;
                set;
            }

            public virtual dom.SVGElement.onfocusinFn onfocusin
            {
                get;
                set;
            }

            public virtual dom.SVGElement.onfocusinFn onfocusout
            {
                get;
                set;
            }

            public virtual dom.SVGElement.onloadFn onload
            {
                get;
                set;
            }

            public virtual dom.SVGElement.onclickFn onmousedown
            {
                get;
                set;
            }

            public virtual dom.SVGElement.onclickFn onmousemove
            {
                get;
                set;
            }

            public virtual dom.SVGElement.onclickFn onmouseout
            {
                get;
                set;
            }

            public virtual dom.SVGElement.onclickFn onmouseover
            {
                get;
                set;
            }

            public virtual dom.SVGElement.onclickFn onmouseup
            {
                get;
                set;
            }

            public virtual dom.SVGSVGElement ownerSVGElement
            {
                get;
            }

            public virtual dom.SVGElement viewportElement
            {
                get;
            }

            public virtual string xmlbase
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual dom.CSSStyleDeclaration style
            {
                get;
            }

            [Generated]
            public delegate void onclickFn(dom.MouseEvent ev);

            [Generated]
            public delegate void onfocusinFn(dom.FocusEvent ev);

            [Generated]
            public delegate void onloadFn(dom.Event ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGElementInstance : dom.EventTarget
        {




            public static dom.SVGElementInstance prototype
            {
                get;
                set;
            }

            public virtual dom.SVGElementInstanceList childNodes
            {
                get;
            }

            public virtual dom.SVGElement correspondingElement
            {
                get;
            }

            public virtual dom.SVGUseElement correspondingUseElement
            {
                get;
            }

            public virtual dom.SVGElementInstance firstChild
            {
                get;
            }

            public virtual dom.SVGElementInstance lastChild
            {
                get;
            }

            public virtual dom.SVGElementInstance nextSibling
            {
                get;
            }

            public virtual dom.SVGElementInstance parentNode
            {
                get;
            }

            public virtual dom.SVGElementInstance previousSibling
            {
                get;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGElementInstanceList : IObject
        {

            public static dom.SVGElementInstanceList prototype
            {
                get;
                set;
            }

            public virtual double length
            {
                get;
            }

            public virtual extern dom.SVGElementInstance item(double index);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGEllipseElement : dom.SVGGraphicsElement
        {


            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"ellipse\")")]
            public extern SVGEllipseElement();

            public static dom.SVGEllipseElement prototype
            {
                get;
                set;
            }

            public virtual dom.SVGAnimatedLength cx
            {
                get;
            }

            public virtual dom.SVGAnimatedLength cy
            {
                get;
            }

            public virtual dom.SVGAnimatedLength rx
            {
                get;
            }

            public virtual dom.SVGAnimatedLength ry
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGEllipseElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGEllipseElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGEllipseElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGEllipseElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGEllipseElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGEllipseElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGEllipseElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGEllipseElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGFEBlendElement : dom.SVGElement, dom.SVGFilterPrimitiveStandardAttributes.Interface, IObject
        {














            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"feBlend\")")]
            public extern SVGFEBlendElement();

            public static dom.SVGFEBlendElement prototype
            {
                get;
                set;
            }

            [Name("SVG_FEBLEND_MODE_COLOR")]
            public static double SVG_FEBLEND_MODE_COLOR_Static
            {
                get;
            }

            [Name("SVG_FEBLEND_MODE_COLOR_BURN")]
            public static double SVG_FEBLEND_MODE_COLOR_BURN_Static
            {
                get;
            }

            [Name("SVG_FEBLEND_MODE_COLOR_DODGE")]
            public static double SVG_FEBLEND_MODE_COLOR_DODGE_Static
            {
                get;
            }

            [Name("SVG_FEBLEND_MODE_DARKEN")]
            public static double SVG_FEBLEND_MODE_DARKEN_Static
            {
                get;
            }

            [Name("SVG_FEBLEND_MODE_DIFFERENCE")]
            public static double SVG_FEBLEND_MODE_DIFFERENCE_Static
            {
                get;
            }

            [Name("SVG_FEBLEND_MODE_EXCLUSION")]
            public static double SVG_FEBLEND_MODE_EXCLUSION_Static
            {
                get;
            }

            [Name("SVG_FEBLEND_MODE_HARD_LIGHT")]
            public static double SVG_FEBLEND_MODE_HARD_LIGHT_Static
            {
                get;
            }

            [Name("SVG_FEBLEND_MODE_HUE")]
            public static double SVG_FEBLEND_MODE_HUE_Static
            {
                get;
            }

            [Name("SVG_FEBLEND_MODE_LIGHTEN")]
            public static double SVG_FEBLEND_MODE_LIGHTEN_Static
            {
                get;
            }

            [Name("SVG_FEBLEND_MODE_LUMINOSITY")]
            public static double SVG_FEBLEND_MODE_LUMINOSITY_Static
            {
                get;
            }

            [Name("SVG_FEBLEND_MODE_MULTIPLY")]
            public static double SVG_FEBLEND_MODE_MULTIPLY_Static
            {
                get;
            }

            [Name("SVG_FEBLEND_MODE_NORMAL")]
            public static double SVG_FEBLEND_MODE_NORMAL_Static
            {
                get;
            }

            [Name("SVG_FEBLEND_MODE_OVERLAY")]
            public static double SVG_FEBLEND_MODE_OVERLAY_Static
            {
                get;
            }

            [Name("SVG_FEBLEND_MODE_SATURATION")]
            public static double SVG_FEBLEND_MODE_SATURATION_Static
            {
                get;
            }

            [Name("SVG_FEBLEND_MODE_SCREEN")]
            public static double SVG_FEBLEND_MODE_SCREEN_Static
            {
                get;
            }

            [Name("SVG_FEBLEND_MODE_SOFT_LIGHT")]
            public static double SVG_FEBLEND_MODE_SOFT_LIGHT_Static
            {
                get;
            }

            [Name("SVG_FEBLEND_MODE_UNKNOWN")]
            public static double SVG_FEBLEND_MODE_UNKNOWN_Static
            {
                get;
            }

            public virtual dom.SVGAnimatedString in1
            {
                get;
            }

            public virtual dom.SVGAnimatedString in2
            {
                get;
            }

            public virtual dom.SVGAnimatedEnumeration mode
            {
                get;
            }

            public virtual double SVG_FEBLEND_MODE_COLOR
            {
                get;
            }

            public virtual double SVG_FEBLEND_MODE_COLOR_BURN
            {
                get;
            }

            public virtual double SVG_FEBLEND_MODE_COLOR_DODGE
            {
                get;
            }

            public virtual double SVG_FEBLEND_MODE_DARKEN
            {
                get;
            }

            public virtual double SVG_FEBLEND_MODE_DIFFERENCE
            {
                get;
            }

            public virtual double SVG_FEBLEND_MODE_EXCLUSION
            {
                get;
            }

            public virtual double SVG_FEBLEND_MODE_HARD_LIGHT
            {
                get;
            }

            public virtual double SVG_FEBLEND_MODE_HUE
            {
                get;
            }

            public virtual double SVG_FEBLEND_MODE_LIGHTEN
            {
                get;
            }

            public virtual double SVG_FEBLEND_MODE_LUMINOSITY
            {
                get;
            }

            public virtual double SVG_FEBLEND_MODE_MULTIPLY
            {
                get;
            }

            public virtual double SVG_FEBLEND_MODE_NORMAL
            {
                get;
            }

            public virtual double SVG_FEBLEND_MODE_OVERLAY
            {
                get;
            }

            public virtual double SVG_FEBLEND_MODE_SATURATION
            {
                get;
            }

            public virtual double SVG_FEBLEND_MODE_SCREEN
            {
                get;
            }

            public virtual double SVG_FEBLEND_MODE_SOFT_LIGHT
            {
                get;
            }

            public virtual double SVG_FEBLEND_MODE_UNKNOWN
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEBlendElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEBlendElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEBlendElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEBlendElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEBlendElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEBlendElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEBlendElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEBlendElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual dom.SVGAnimatedLength height
            {
                get;
            }

            public virtual dom.SVGAnimatedString result
            {
                get;
            }

            public virtual dom.SVGAnimatedLength width
            {
                get;
            }

            public virtual dom.SVGAnimatedLength x
            {
                get;
            }

            public virtual dom.SVGAnimatedLength y
            {
                get;
            }

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGFEColorMatrixElement : dom.SVGElement, dom.SVGFilterPrimitiveStandardAttributes.Interface, IObject
        {






            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"feColorMatrix\")")]
            public extern SVGFEColorMatrixElement();

            public static dom.SVGFEColorMatrixElement prototype
            {
                get;
                set;
            }

            [Name("SVG_FECOLORMATRIX_TYPE_HUEROTATE")]
            public static double SVG_FECOLORMATRIX_TYPE_HUEROTATE_Static
            {
                get;
            }

            [Name("SVG_FECOLORMATRIX_TYPE_LUMINANCETOALPHA")]
            public static double SVG_FECOLORMATRIX_TYPE_LUMINANCETOALPHA_Static
            {
                get;
            }

            [Name("SVG_FECOLORMATRIX_TYPE_MATRIX")]
            public static double SVG_FECOLORMATRIX_TYPE_MATRIX_Static
            {
                get;
            }

            [Name("SVG_FECOLORMATRIX_TYPE_SATURATE")]
            public static double SVG_FECOLORMATRIX_TYPE_SATURATE_Static
            {
                get;
            }

            [Name("SVG_FECOLORMATRIX_TYPE_UNKNOWN")]
            public static double SVG_FECOLORMATRIX_TYPE_UNKNOWN_Static
            {
                get;
            }

            public virtual dom.SVGAnimatedString in1
            {
                get;
            }

            public virtual dom.SVGAnimatedEnumeration type
            {
                get;
            }

            public virtual dom.SVGAnimatedNumberList values
            {
                get;
            }

            public virtual double SVG_FECOLORMATRIX_TYPE_HUEROTATE
            {
                get;
            }

            public virtual double SVG_FECOLORMATRIX_TYPE_LUMINANCETOALPHA
            {
                get;
            }

            public virtual double SVG_FECOLORMATRIX_TYPE_MATRIX
            {
                get;
            }

            public virtual double SVG_FECOLORMATRIX_TYPE_SATURATE
            {
                get;
            }

            public virtual double SVG_FECOLORMATRIX_TYPE_UNKNOWN
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEColorMatrixElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEColorMatrixElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEColorMatrixElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEColorMatrixElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEColorMatrixElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEColorMatrixElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEColorMatrixElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEColorMatrixElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual dom.SVGAnimatedLength height
            {
                get;
            }

            public virtual dom.SVGAnimatedString result
            {
                get;
            }

            public virtual dom.SVGAnimatedLength width
            {
                get;
            }

            public virtual dom.SVGAnimatedLength x
            {
                get;
            }

            public virtual dom.SVGAnimatedLength y
            {
                get;
            }

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGFEComponentTransferElement : dom.SVGElement, dom.SVGFilterPrimitiveStandardAttributes.Interface, IObject
        {


            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"feComponentTransfer\")")]
            public extern SVGFEComponentTransferElement();

            public static dom.SVGFEComponentTransferElement prototype
            {
                get;
                set;
            }

            public virtual dom.SVGAnimatedString in1
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEComponentTransferElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEComponentTransferElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEComponentTransferElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEComponentTransferElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEComponentTransferElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEComponentTransferElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEComponentTransferElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEComponentTransferElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual dom.SVGAnimatedLength height
            {
                get;
            }

            public virtual dom.SVGAnimatedString result
            {
                get;
            }

            public virtual dom.SVGAnimatedLength width
            {
                get;
            }

            public virtual dom.SVGAnimatedLength x
            {
                get;
            }

            public virtual dom.SVGAnimatedLength y
            {
                get;
            }

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGFECompositeElement : dom.SVGElement, dom.SVGFilterPrimitiveStandardAttributes.Interface, IObject
        {










            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"feComposite\")")]
            public extern SVGFECompositeElement();

            public static dom.SVGFECompositeElement prototype
            {
                get;
                set;
            }

            [Name("SVG_FECOMPOSITE_OPERATOR_ARITHMETIC")]
            public static double SVG_FECOMPOSITE_OPERATOR_ARITHMETIC_Static
            {
                get;
            }

            [Name("SVG_FECOMPOSITE_OPERATOR_ATOP")]
            public static double SVG_FECOMPOSITE_OPERATOR_ATOP_Static
            {
                get;
            }

            [Name("SVG_FECOMPOSITE_OPERATOR_IN")]
            public static double SVG_FECOMPOSITE_OPERATOR_IN_Static
            {
                get;
            }

            [Name("SVG_FECOMPOSITE_OPERATOR_OUT")]
            public static double SVG_FECOMPOSITE_OPERATOR_OUT_Static
            {
                get;
            }

            [Name("SVG_FECOMPOSITE_OPERATOR_OVER")]
            public static double SVG_FECOMPOSITE_OPERATOR_OVER_Static
            {
                get;
            }

            [Name("SVG_FECOMPOSITE_OPERATOR_UNKNOWN")]
            public static double SVG_FECOMPOSITE_OPERATOR_UNKNOWN_Static
            {
                get;
            }

            [Name("SVG_FECOMPOSITE_OPERATOR_XOR")]
            public static double SVG_FECOMPOSITE_OPERATOR_XOR_Static
            {
                get;
            }

            public virtual dom.SVGAnimatedString in1
            {
                get;
            }

            public virtual dom.SVGAnimatedString in2
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber k1
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber k2
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber k3
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber k4
            {
                get;
            }

            [Name("operator")]
            public virtual dom.SVGAnimatedEnumeration @operator
            {
                get;
            }

            public virtual double SVG_FECOMPOSITE_OPERATOR_ARITHMETIC
            {
                get;
            }

            public virtual double SVG_FECOMPOSITE_OPERATOR_ATOP
            {
                get;
            }

            public virtual double SVG_FECOMPOSITE_OPERATOR_IN
            {
                get;
            }

            public virtual double SVG_FECOMPOSITE_OPERATOR_OUT
            {
                get;
            }

            public virtual double SVG_FECOMPOSITE_OPERATOR_OVER
            {
                get;
            }

            public virtual double SVG_FECOMPOSITE_OPERATOR_UNKNOWN
            {
                get;
            }

            public virtual double SVG_FECOMPOSITE_OPERATOR_XOR
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFECompositeElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFECompositeElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFECompositeElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFECompositeElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFECompositeElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFECompositeElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFECompositeElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFECompositeElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual dom.SVGAnimatedLength height
            {
                get;
            }

            public virtual dom.SVGAnimatedString result
            {
                get;
            }

            public virtual dom.SVGAnimatedLength width
            {
                get;
            }

            public virtual dom.SVGAnimatedLength x
            {
                get;
            }

            public virtual dom.SVGAnimatedLength y
            {
                get;
            }

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGFEConvolveMatrixElement : dom.SVGElement, dom.SVGFilterPrimitiveStandardAttributes.Interface, IObject
        {









            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"feConvolveMatrix\")")]
            public extern SVGFEConvolveMatrixElement();

            public static dom.SVGFEConvolveMatrixElement prototype
            {
                get;
                set;
            }

            [Name("SVG_EDGEMODE_DUPLICATE")]
            public static double SVG_EDGEMODE_DUPLICATE_Static
            {
                get;
            }

            [Name("SVG_EDGEMODE_NONE")]
            public static double SVG_EDGEMODE_NONE_Static
            {
                get;
            }

            [Name("SVG_EDGEMODE_UNKNOWN")]
            public static double SVG_EDGEMODE_UNKNOWN_Static
            {
                get;
            }

            [Name("SVG_EDGEMODE_WRAP")]
            public static double SVG_EDGEMODE_WRAP_Static
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber bias
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber divisor
            {
                get;
            }

            public virtual dom.SVGAnimatedEnumeration edgeMode
            {
                get;
            }

            public virtual dom.SVGAnimatedString in1
            {
                get;
            }

            public virtual dom.SVGAnimatedNumberList kernelMatrix
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber kernelUnitLengthX
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber kernelUnitLengthY
            {
                get;
            }

            public virtual dom.SVGAnimatedInteger orderX
            {
                get;
            }

            public virtual dom.SVGAnimatedInteger orderY
            {
                get;
            }

            public virtual dom.SVGAnimatedBoolean preserveAlpha
            {
                get;
            }

            public virtual dom.SVGAnimatedInteger targetX
            {
                get;
            }

            public virtual dom.SVGAnimatedInteger targetY
            {
                get;
            }

            public virtual double SVG_EDGEMODE_DUPLICATE
            {
                get;
            }

            public virtual double SVG_EDGEMODE_NONE
            {
                get;
            }

            public virtual double SVG_EDGEMODE_UNKNOWN
            {
                get;
            }

            public virtual double SVG_EDGEMODE_WRAP
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEConvolveMatrixElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEConvolveMatrixElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEConvolveMatrixElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEConvolveMatrixElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEConvolveMatrixElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEConvolveMatrixElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEConvolveMatrixElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEConvolveMatrixElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual dom.SVGAnimatedLength height
            {
                get;
            }

            public virtual dom.SVGAnimatedString result
            {
                get;
            }

            public virtual dom.SVGAnimatedLength width
            {
                get;
            }

            public virtual dom.SVGAnimatedLength x
            {
                get;
            }

            public virtual dom.SVGAnimatedLength y
            {
                get;
            }

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGFEDiffuseLightingElement : dom.SVGElement, dom.SVGFilterPrimitiveStandardAttributes.Interface, IObject
        {




            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"feDiffuseLighting\")")]
            public extern SVGFEDiffuseLightingElement();

            public static dom.SVGFEDiffuseLightingElement prototype
            {
                get;
                set;
            }

            public virtual dom.SVGAnimatedNumber diffuseConstant
            {
                get;
            }

            public virtual dom.SVGAnimatedString in1
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber kernelUnitLengthX
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber kernelUnitLengthY
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber surfaceScale
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEDiffuseLightingElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEDiffuseLightingElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEDiffuseLightingElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEDiffuseLightingElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEDiffuseLightingElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEDiffuseLightingElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEDiffuseLightingElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEDiffuseLightingElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual dom.SVGAnimatedLength height
            {
                get;
            }

            public virtual dom.SVGAnimatedString result
            {
                get;
            }

            public virtual dom.SVGAnimatedLength width
            {
                get;
            }

            public virtual dom.SVGAnimatedLength x
            {
                get;
            }

            public virtual dom.SVGAnimatedLength y
            {
                get;
            }

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGFEDisplacementMapElement : dom.SVGElement, dom.SVGFilterPrimitiveStandardAttributes.Interface, IObject
        {








            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"feDisplacementMap\")")]
            public extern SVGFEDisplacementMapElement();

            public static dom.SVGFEDisplacementMapElement prototype
            {
                get;
                set;
            }

            [Name("SVG_CHANNEL_A")]
            public static double SVG_CHANNEL_A_Static
            {
                get;
            }

            [Name("SVG_CHANNEL_B")]
            public static double SVG_CHANNEL_B_Static
            {
                get;
            }

            [Name("SVG_CHANNEL_G")]
            public static double SVG_CHANNEL_G_Static
            {
                get;
            }

            [Name("SVG_CHANNEL_R")]
            public static double SVG_CHANNEL_R_Static
            {
                get;
            }

            [Name("SVG_CHANNEL_UNKNOWN")]
            public static double SVG_CHANNEL_UNKNOWN_Static
            {
                get;
            }

            public virtual dom.SVGAnimatedString in1
            {
                get;
            }

            public virtual dom.SVGAnimatedString in2
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber scale
            {
                get;
            }

            public virtual dom.SVGAnimatedEnumeration xChannelSelector
            {
                get;
            }

            public virtual dom.SVGAnimatedEnumeration yChannelSelector
            {
                get;
            }

            public virtual double SVG_CHANNEL_A
            {
                get;
            }

            public virtual double SVG_CHANNEL_B
            {
                get;
            }

            public virtual double SVG_CHANNEL_G
            {
                get;
            }

            public virtual double SVG_CHANNEL_R
            {
                get;
            }

            public virtual double SVG_CHANNEL_UNKNOWN
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEDisplacementMapElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEDisplacementMapElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEDisplacementMapElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEDisplacementMapElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEDisplacementMapElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEDisplacementMapElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEDisplacementMapElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEDisplacementMapElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual dom.SVGAnimatedLength height
            {
                get;
            }

            public virtual dom.SVGAnimatedString result
            {
                get;
            }

            public virtual dom.SVGAnimatedLength width
            {
                get;
            }

            public virtual dom.SVGAnimatedLength x
            {
                get;
            }

            public virtual dom.SVGAnimatedLength y
            {
                get;
            }

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGFEDistantLightElement : dom.SVGElement
        {


            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"feDistantLight\")")]
            public extern SVGFEDistantLightElement();

            public static dom.SVGFEDistantLightElement prototype
            {
                get;
                set;
            }

            public virtual dom.SVGAnimatedNumber azimuth
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber elevation
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEDistantLightElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEDistantLightElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEDistantLightElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEDistantLightElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEDistantLightElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEDistantLightElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEDistantLightElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEDistantLightElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGFEFloodElement : dom.SVGElement, dom.SVGFilterPrimitiveStandardAttributes.Interface, IObject
        {



            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"feFlood\")")]
            public extern SVGFEFloodElement();

            public static dom.SVGFEFloodElement prototype
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEFloodElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEFloodElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEFloodElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEFloodElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEFloodElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEFloodElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEFloodElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEFloodElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual dom.SVGAnimatedLength height
            {
                get;
            }

            public virtual dom.SVGAnimatedString result
            {
                get;
            }

            public virtual dom.SVGAnimatedLength width
            {
                get;
            }

            public virtual dom.SVGAnimatedLength x
            {
                get;
            }

            public virtual dom.SVGAnimatedLength y
            {
                get;
            }

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGFEFuncAElement : dom.SVGComponentTransferFunctionElement
        {
            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"feFuncA\")")]
            public extern SVGFEFuncAElement();

            public static dom.SVGFEFuncAElement prototype
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEFuncAElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEFuncAElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEFuncAElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEFuncAElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEFuncAElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEFuncAElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEFuncAElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEFuncAElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGFEFuncBElement : dom.SVGComponentTransferFunctionElement
        {
            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"feFuncB\")")]
            public extern SVGFEFuncBElement();

            public static dom.SVGFEFuncBElement prototype
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEFuncBElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEFuncBElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEFuncBElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEFuncBElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEFuncBElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEFuncBElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEFuncBElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEFuncBElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGFEFuncGElement : dom.SVGComponentTransferFunctionElement
        {
            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"feFuncG\")")]
            public extern SVGFEFuncGElement();

            public static dom.SVGFEFuncGElement prototype
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEFuncGElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEFuncGElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEFuncGElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEFuncGElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEFuncGElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEFuncGElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEFuncGElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEFuncGElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGFEFuncRElement : dom.SVGComponentTransferFunctionElement
        {
            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"feFuncR\")")]
            public extern SVGFEFuncRElement();

            public static dom.SVGFEFuncRElement prototype
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEFuncRElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEFuncRElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEFuncRElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEFuncRElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEFuncRElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEFuncRElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEFuncRElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEFuncRElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGFEGaussianBlurElement : dom.SVGElement, dom.SVGFilterPrimitiveStandardAttributes.Interface, IObject
        {




            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"feGaussianBlur\")")]
            public extern SVGFEGaussianBlurElement();

            public static dom.SVGFEGaussianBlurElement prototype
            {
                get;
                set;
            }

            public virtual dom.SVGAnimatedString in1
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber stdDeviationX
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber stdDeviationY
            {
                get;
            }

            public virtual extern void setStdDeviation(double stdDeviationX, double stdDeviationY);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEGaussianBlurElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEGaussianBlurElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEGaussianBlurElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEGaussianBlurElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEGaussianBlurElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEGaussianBlurElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEGaussianBlurElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEGaussianBlurElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual dom.SVGAnimatedLength height
            {
                get;
            }

            public virtual dom.SVGAnimatedString result
            {
                get;
            }

            public virtual dom.SVGAnimatedLength width
            {
                get;
            }

            public virtual dom.SVGAnimatedLength x
            {
                get;
            }

            public virtual dom.SVGAnimatedLength y
            {
                get;
            }

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGFEImageElement : dom.SVGElement, dom.SVGFilterPrimitiveStandardAttributes.Interface, IObject, dom.SVGURIReference.Interface
        {



            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"feImage\")")]
            public extern SVGFEImageElement();

            public static dom.SVGFEImageElement prototype
            {
                get;
                set;
            }

            public virtual dom.SVGAnimatedPreserveAspectRatio preserveAspectRatio
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEImageElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEImageElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEImageElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEImageElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEImageElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEImageElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEImageElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEImageElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual dom.SVGAnimatedLength height
            {
                get;
            }

            public virtual dom.SVGAnimatedString result
            {
                get;
            }

            public virtual dom.SVGAnimatedLength width
            {
                get;
            }

            public virtual dom.SVGAnimatedLength x
            {
                get;
            }

            public virtual dom.SVGAnimatedLength y
            {
                get;
            }

            public virtual dom.SVGAnimatedString href
            {
                get;
            }

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGFEMergeElement : dom.SVGElement, dom.SVGFilterPrimitiveStandardAttributes.Interface, IObject
        {



            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"feMerge\")")]
            public extern SVGFEMergeElement();

            public static dom.SVGFEMergeElement prototype
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEMergeElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEMergeElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEMergeElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEMergeElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEMergeElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEMergeElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEMergeElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEMergeElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual dom.SVGAnimatedLength height
            {
                get;
            }

            public virtual dom.SVGAnimatedString result
            {
                get;
            }

            public virtual dom.SVGAnimatedLength width
            {
                get;
            }

            public virtual dom.SVGAnimatedLength x
            {
                get;
            }

            public virtual dom.SVGAnimatedLength y
            {
                get;
            }

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGFEMergeNodeElement : dom.SVGElement
        {

            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"feMergeNode\")")]
            public extern SVGFEMergeNodeElement();

            public static dom.SVGFEMergeNodeElement prototype
            {
                get;
                set;
            }

            public virtual dom.SVGAnimatedString in1
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEMergeNodeElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEMergeNodeElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEMergeNodeElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEMergeNodeElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEMergeNodeElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEMergeNodeElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEMergeNodeElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEMergeNodeElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGFEMorphologyElement : dom.SVGElement, dom.SVGFilterPrimitiveStandardAttributes.Interface, IObject
        {





            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"feMorphology\")")]
            public extern SVGFEMorphologyElement();

            public static dom.SVGFEMorphologyElement prototype
            {
                get;
                set;
            }

            [Name("SVG_MORPHOLOGY_OPERATOR_DILATE")]
            public static double SVG_MORPHOLOGY_OPERATOR_DILATE_Static
            {
                get;
            }

            [Name("SVG_MORPHOLOGY_OPERATOR_ERODE")]
            public static double SVG_MORPHOLOGY_OPERATOR_ERODE_Static
            {
                get;
            }

            [Name("SVG_MORPHOLOGY_OPERATOR_UNKNOWN")]
            public static double SVG_MORPHOLOGY_OPERATOR_UNKNOWN_Static
            {
                get;
            }

            public virtual dom.SVGAnimatedString in1
            {
                get;
            }

            [Name("operator")]
            public virtual dom.SVGAnimatedEnumeration @operator
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber radiusX
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber radiusY
            {
                get;
            }

            public virtual double SVG_MORPHOLOGY_OPERATOR_DILATE
            {
                get;
            }

            public virtual double SVG_MORPHOLOGY_OPERATOR_ERODE
            {
                get;
            }

            public virtual double SVG_MORPHOLOGY_OPERATOR_UNKNOWN
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEMorphologyElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEMorphologyElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEMorphologyElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEMorphologyElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEMorphologyElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEMorphologyElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEMorphologyElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEMorphologyElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual dom.SVGAnimatedLength height
            {
                get;
            }

            public virtual dom.SVGAnimatedString result
            {
                get;
            }

            public virtual dom.SVGAnimatedLength width
            {
                get;
            }

            public virtual dom.SVGAnimatedLength x
            {
                get;
            }

            public virtual dom.SVGAnimatedLength y
            {
                get;
            }

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGFEOffsetElement : dom.SVGElement, dom.SVGFilterPrimitiveStandardAttributes.Interface, IObject
        {




            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"feOffset\")")]
            public extern SVGFEOffsetElement();

            public static dom.SVGFEOffsetElement prototype
            {
                get;
                set;
            }

            public virtual dom.SVGAnimatedNumber dx
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber dy
            {
                get;
            }

            public virtual dom.SVGAnimatedString in1
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEOffsetElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEOffsetElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEOffsetElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEOffsetElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEOffsetElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEOffsetElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEOffsetElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEOffsetElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual dom.SVGAnimatedLength height
            {
                get;
            }

            public virtual dom.SVGAnimatedString result
            {
                get;
            }

            public virtual dom.SVGAnimatedLength width
            {
                get;
            }

            public virtual dom.SVGAnimatedLength x
            {
                get;
            }

            public virtual dom.SVGAnimatedLength y
            {
                get;
            }

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGFEPointLightElement : dom.SVGElement
        {

            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"fePointLight\")")]
            public extern SVGFEPointLightElement();

            public static dom.SVGFEPointLightElement prototype
            {
                get;
                set;
            }

            public virtual dom.SVGAnimatedNumber x
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber y
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber z
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEPointLightElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEPointLightElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEPointLightElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFEPointLightElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEPointLightElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEPointLightElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEPointLightElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFEPointLightElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGFESpecularLightingElement : dom.SVGElement, dom.SVGFilterPrimitiveStandardAttributes.Interface, IObject
        {





            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"feSpecularLighting\")")]
            public extern SVGFESpecularLightingElement();

            public static dom.SVGFESpecularLightingElement prototype
            {
                get;
                set;
            }

            public virtual dom.SVGAnimatedString in1
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber kernelUnitLengthX
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber kernelUnitLengthY
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber specularConstant
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber specularExponent
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber surfaceScale
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFESpecularLightingElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFESpecularLightingElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFESpecularLightingElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFESpecularLightingElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFESpecularLightingElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFESpecularLightingElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFESpecularLightingElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFESpecularLightingElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual dom.SVGAnimatedLength height
            {
                get;
            }

            public virtual dom.SVGAnimatedString result
            {
                get;
            }

            public virtual dom.SVGAnimatedLength width
            {
                get;
            }

            public virtual dom.SVGAnimatedLength x
            {
                get;
            }

            public virtual dom.SVGAnimatedLength y
            {
                get;
            }

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGFESpotLightElement : dom.SVGElement
        {




            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"feSpotLight\")")]
            public extern SVGFESpotLightElement();

            public static dom.SVGFESpotLightElement prototype
            {
                get;
                set;
            }

            public virtual dom.SVGAnimatedNumber limitingConeAngle
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber pointsAtX
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber pointsAtY
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber pointsAtZ
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber specularExponent
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber x
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber y
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber z
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFESpotLightElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFESpotLightElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFESpotLightElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFESpotLightElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFESpotLightElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFESpotLightElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFESpotLightElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFESpotLightElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGFETileElement : dom.SVGElement, dom.SVGFilterPrimitiveStandardAttributes.Interface, IObject
        {


            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"feTile\")")]
            public extern SVGFETileElement();

            public static dom.SVGFETileElement prototype
            {
                get;
                set;
            }

            public virtual dom.SVGAnimatedString in1
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFETileElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFETileElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFETileElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFETileElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFETileElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFETileElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFETileElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFETileElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual dom.SVGAnimatedLength height
            {
                get;
            }

            public virtual dom.SVGAnimatedString result
            {
                get;
            }

            public virtual dom.SVGAnimatedLength width
            {
                get;
            }

            public virtual dom.SVGAnimatedLength x
            {
                get;
            }

            public virtual dom.SVGAnimatedLength y
            {
                get;
            }

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGFETurbulenceElement : dom.SVGElement, dom.SVGFilterPrimitiveStandardAttributes.Interface, IObject
        {









            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"feTurbulence\")")]
            public extern SVGFETurbulenceElement();

            public static dom.SVGFETurbulenceElement prototype
            {
                get;
                set;
            }

            [Name("SVG_STITCHTYPE_NOSTITCH")]
            public static double SVG_STITCHTYPE_NOSTITCH_Static
            {
                get;
            }

            [Name("SVG_STITCHTYPE_STITCH")]
            public static double SVG_STITCHTYPE_STITCH_Static
            {
                get;
            }

            [Name("SVG_STITCHTYPE_UNKNOWN")]
            public static double SVG_STITCHTYPE_UNKNOWN_Static
            {
                get;
            }

            [Name("SVG_TURBULENCE_TYPE_FRACTALNOISE")]
            public static double SVG_TURBULENCE_TYPE_FRACTALNOISE_Static
            {
                get;
            }

            [Name("SVG_TURBULENCE_TYPE_TURBULENCE")]
            public static double SVG_TURBULENCE_TYPE_TURBULENCE_Static
            {
                get;
            }

            [Name("SVG_TURBULENCE_TYPE_UNKNOWN")]
            public static double SVG_TURBULENCE_TYPE_UNKNOWN_Static
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber baseFrequencyX
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber baseFrequencyY
            {
                get;
            }

            public virtual dom.SVGAnimatedInteger numOctaves
            {
                get;
            }

            public virtual dom.SVGAnimatedNumber seed
            {
                get;
            }

            public virtual dom.SVGAnimatedEnumeration stitchTiles
            {
                get;
            }

            public virtual dom.SVGAnimatedEnumeration type
            {
                get;
            }

            public virtual double SVG_STITCHTYPE_NOSTITCH
            {
                get;
            }

            public virtual double SVG_STITCHTYPE_STITCH
            {
                get;
            }

            public virtual double SVG_STITCHTYPE_UNKNOWN
            {
                get;
            }

            public virtual double SVG_TURBULENCE_TYPE_FRACTALNOISE
            {
                get;
            }

            public virtual double SVG_TURBULENCE_TYPE_TURBULENCE
            {
                get;
            }

            public virtual double SVG_TURBULENCE_TYPE_UNKNOWN
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFETurbulenceElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFETurbulenceElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFETurbulenceElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFETurbulenceElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFETurbulenceElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFETurbulenceElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFETurbulenceElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFETurbulenceElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual dom.SVGAnimatedLength height
            {
                get;
            }

            public virtual dom.SVGAnimatedString result
            {
                get;
            }

            public virtual dom.SVGAnimatedLength width
            {
                get;
            }

            public virtual dom.SVGAnimatedLength x
            {
                get;
            }

            public virtual dom.SVGAnimatedLength y
            {
                get;
            }

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGFilterElement : dom.SVGElement, dom.SVGUnitTypes.Interface, IObject, dom.SVGURIReference.Interface
        {




            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"filter\")")]
            public extern SVGFilterElement();

            public static dom.SVGFilterElement prototype
            {
                get;
                set;
            }

            public virtual dom.SVGAnimatedInteger filterResX
            {
                get;
            }

            public virtual dom.SVGAnimatedInteger filterResY
            {
                get;
            }

            public virtual dom.SVGAnimatedEnumeration filterUnits
            {
                get;
            }

            public virtual dom.SVGAnimatedLength height
            {
                get;
            }

            public virtual dom.SVGAnimatedEnumeration primitiveUnits
            {
                get;
            }

            public virtual dom.SVGAnimatedLength width
            {
                get;
            }

            public virtual dom.SVGAnimatedLength x
            {
                get;
            }

            public virtual dom.SVGAnimatedLength y
            {
                get;
            }

            public virtual extern void setFilterRes(double filterResX, double filterResY);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFilterElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFilterElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFilterElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGFilterElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFilterElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFilterElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFilterElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGFilterElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual ushort SVG_UNIT_TYPE_OBJECTBOUNDINGBOX
            {
                get;
            }

            public virtual ushort SVG_UNIT_TYPE_UNKNOWN
            {
                get;
            }

            public virtual ushort SVG_UNIT_TYPE_USERSPACEONUSE
            {
                get;
            }

            public virtual dom.SVGAnimatedString href
            {
                get;
            }

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [IgnoreCast]
        [ObjectLiteral]
        [InterfaceWrapper]
        public class SVGFilterPrimitiveStandardAttributes : dom.SVGFilterPrimitiveStandardAttributes.Interface, IObject
        {


            public dom.SVGAnimatedLength height
            {
                get;
            }

            public dom.SVGAnimatedString result
            {
                get;
            }

            public dom.SVGAnimatedLength width
            {
                get;
            }

            public dom.SVGAnimatedLength x
            {
                get;
            }

            public dom.SVGAnimatedLength y
            {
                get;
            }

            [Generated]
            [ObjectLiteral]
            [IgnoreCast]
            public interface Interface : IObject
            {
                dom.SVGAnimatedLength height { get; }

                dom.SVGAnimatedString result { get; }

                dom.SVGAnimatedLength width { get; }

                dom.SVGAnimatedLength x { get; }

                dom.SVGAnimatedLength y { get; }
            }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [InterfaceWrapper]
        public class SVGFitToViewBox : dom.SVGFitToViewBox.Interface, IObject
        {

            public dom.SVGAnimatedPreserveAspectRatio preserveAspectRatio
            {
                get;
            }

            public dom.SVGAnimatedRect viewBox
            {
                get;
            }

            [Generated]
            [ObjectLiteral]
            [IgnoreCast]
            public interface Interface : IObject
            {
                dom.SVGAnimatedPreserveAspectRatio preserveAspectRatio { get; }

                dom.SVGAnimatedRect viewBox { get; }
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGForeignObjectElement : dom.SVGGraphicsElement
        {


            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"foreignObject\")")]
            public extern SVGForeignObjectElement();

            public static dom.SVGForeignObjectElement prototype
            {
                get;
                set;
            }

            public virtual dom.SVGAnimatedLength height
            {
                get;
            }

            public virtual dom.SVGAnimatedLength width
            {
                get;
            }

            public virtual dom.SVGAnimatedLength x
            {
                get;
            }

            public virtual dom.SVGAnimatedLength y
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGForeignObjectElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGForeignObjectElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGForeignObjectElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGForeignObjectElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGForeignObjectElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGForeignObjectElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGForeignObjectElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGForeignObjectElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGGElement : dom.SVGGraphicsElement
        {
            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"g\")")]
            public extern SVGGElement();

            public static dom.SVGGElement prototype
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGGElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGGElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGGElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGGElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGGElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGGElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGGElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGGElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGGradientElement : dom.SVGElement, dom.SVGUnitTypes.Interface, IObject, dom.SVGURIReference.Interface
        {





            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"gradient\")")]
            public extern SVGGradientElement();

            public static dom.SVGGradientElement prototype
            {
                get;
                set;
            }

            [Name("SVG_SPREADMETHOD_PAD")]
            public static ushort SVG_SPREADMETHOD_PAD_Static
            {
                get;
            }

            [Name("SVG_SPREADMETHOD_REFLECT")]
            public static ushort SVG_SPREADMETHOD_REFLECT_Static
            {
                get;
            }

            [Name("SVG_SPREADMETHOD_REPEAT")]
            public static ushort SVG_SPREADMETHOD_REPEAT_Static
            {
                get;
            }

            [Name("SVG_SPREADMETHOD_UNKNOWN")]
            public static ushort SVG_SPREADMETHOD_UNKNOWN_Static
            {
                get;
            }

            public virtual dom.SVGAnimatedTransformList gradientTransform
            {
                get;
            }

            public virtual dom.SVGAnimatedEnumeration gradientUnits
            {
                get;
            }

            public virtual dom.SVGAnimatedEnumeration spreadMethod
            {
                get;
            }

            public virtual ushort SVG_SPREADMETHOD_PAD
            {
                get;
            }

            public virtual ushort SVG_SPREADMETHOD_REFLECT
            {
                get;
            }

            public virtual ushort SVG_SPREADMETHOD_REPEAT
            {
                get;
            }

            public virtual ushort SVG_SPREADMETHOD_UNKNOWN
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGGradientElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGGradientElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGGradientElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGGradientElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGGradientElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGGradientElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGGradientElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGGradientElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual ushort SVG_UNIT_TYPE_OBJECTBOUNDINGBOX
            {
                get;
            }

            public virtual ushort SVG_UNIT_TYPE_UNKNOWN
            {
                get;
            }

            public virtual ushort SVG_UNIT_TYPE_USERSPACEONUSE
            {
                get;
            }

            public virtual dom.SVGAnimatedString href
            {
                get;
            }

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGGraphicsElement : dom.SVGElement, dom.SVGTests.Interface, IObject
        {


            public static dom.SVGGraphicsElement prototype
            {
                get;
                set;
            }

            public virtual dom.SVGElement farthestViewportElement
            {
                get;
            }

            public virtual dom.SVGElement nearestViewportElement
            {
                get;
            }

            public virtual dom.SVGAnimatedTransformList transform
            {
                get;
            }

            public virtual extern dom.SVGRect getBBox();

            public virtual extern dom.SVGMatrix getCTM();

            public virtual extern dom.SVGMatrix getScreenCTM();

            public virtual extern dom.SVGMatrix getTransformToElement(dom.SVGElement element);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGGraphicsElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGGraphicsElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGGraphicsElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGGraphicsElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGGraphicsElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGGraphicsElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGGraphicsElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGGraphicsElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual dom.SVGStringList requiredExtensions
            {
                get;
            }

            public virtual dom.SVGStringList requiredFeatures
            {
                get;
            }

            public virtual dom.SVGStringList systemLanguage
            {
                get;
            }

            public virtual extern bool hasExtension(string extension);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGImageElement : dom.SVGGraphicsElement, dom.SVGURIReference.Interface, IObject
        {


            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"image\")")]
            public extern SVGImageElement();

            public static dom.SVGImageElement prototype
            {
                get;
                set;
            }

            public virtual dom.SVGAnimatedLength height
            {
                get;
            }

            public virtual dom.SVGAnimatedPreserveAspectRatio preserveAspectRatio
            {
                get;
            }

            public virtual dom.SVGAnimatedLength width
            {
                get;
            }

            public virtual dom.SVGAnimatedLength x
            {
                get;
            }

            public virtual dom.SVGAnimatedLength y
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGImageElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGImageElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGImageElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGImageElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGImageElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGImageElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGImageElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGImageElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual dom.SVGAnimatedString href
            {
                get;
            }

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGLength : IObject
        {










            public static dom.SVGLength prototype
            {
                get;
                set;
            }

            [Name("SVG_LENGTHTYPE_CM")]
            public static ushort SVG_LENGTHTYPE_CM_Static
            {
                get;
            }

            [Name("SVG_LENGTHTYPE_EMS")]
            public static ushort SVG_LENGTHTYPE_EMS_Static
            {
                get;
            }

            [Name("SVG_LENGTHTYPE_EXS")]
            public static ushort SVG_LENGTHTYPE_EXS_Static
            {
                get;
            }

            [Name("SVG_LENGTHTYPE_IN")]
            public static ushort SVG_LENGTHTYPE_IN_Static
            {
                get;
            }

            [Name("SVG_LENGTHTYPE_MM")]
            public static ushort SVG_LENGTHTYPE_MM_Static
            {
                get;
            }

            [Name("SVG_LENGTHTYPE_NUMBER")]
            public static ushort SVG_LENGTHTYPE_NUMBER_Static
            {
                get;
            }

            [Name("SVG_LENGTHTYPE_PC")]
            public static ushort SVG_LENGTHTYPE_PC_Static
            {
                get;
            }

            [Name("SVG_LENGTHTYPE_PERCENTAGE")]
            public static ushort SVG_LENGTHTYPE_PERCENTAGE_Static
            {
                get;
            }

            [Name("SVG_LENGTHTYPE_PT")]
            public static ushort SVG_LENGTHTYPE_PT_Static
            {
                get;
            }

            [Name("SVG_LENGTHTYPE_PX")]
            public static ushort SVG_LENGTHTYPE_PX_Static
            {
                get;
            }

            [Name("SVG_LENGTHTYPE_UNKNOWN")]
            public static ushort SVG_LENGTHTYPE_UNKNOWN_Static
            {
                get;
            }

            public virtual ushort unitType
            {
                get;
            }

            public virtual float value
            {
                get;
                set;
            }

            public virtual string valueAsString
            {
                get;
                set;
            }

            public virtual float valueInSpecifiedUnits
            {
                get;
                set;
            }

            public virtual extern void convertToSpecifiedUnits(ushort unitType);

            public virtual extern void newValueSpecifiedUnits(
              ushort unitType,
              float valueInSpecifiedUnits);

            public virtual ushort SVG_LENGTHTYPE_CM
            {
                get;
            }

            public virtual ushort SVG_LENGTHTYPE_EMS
            {
                get;
            }

            public virtual ushort SVG_LENGTHTYPE_EXS
            {
                get;
            }

            public virtual ushort SVG_LENGTHTYPE_IN
            {
                get;
            }

            public virtual ushort SVG_LENGTHTYPE_MM
            {
                get;
            }

            public virtual ushort SVG_LENGTHTYPE_NUMBER
            {
                get;
            }

            public virtual ushort SVG_LENGTHTYPE_PC
            {
                get;
            }

            public virtual ushort SVG_LENGTHTYPE_PERCENTAGE
            {
                get;
            }

            public virtual ushort SVG_LENGTHTYPE_PT
            {
                get;
            }

            public virtual ushort SVG_LENGTHTYPE_PX
            {
                get;
            }

            public virtual ushort SVG_LENGTHTYPE_UNKNOWN
            {
                get;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGLengthList : IEnumerable<dom.SVGLength>, IEnumerable, IH5Class, IObject
        {

            public static dom.SVGLengthList prototype
            {
                get;
                set;
            }

            public virtual uint numberOfItems
            {
                get;
            }

            public virtual extern dom.SVGLength appendItem(dom.SVGLength newItem);

            public virtual extern void clear();

            public virtual extern dom.SVGLength getItem(uint index);

            public virtual extern dom.SVGLength initialize(dom.SVGLength newItem);

            public virtual extern dom.SVGLength insertItemBefore(dom.SVGLength newItem, uint index);

            public virtual extern dom.SVGLength removeItem(uint index);

            public virtual extern dom.SVGLength replaceItem(dom.SVGLength newItem, uint index);

            [Template("H5.getEnumerator({this}, {T})")]
            extern IEnumerator<dom.SVGLength> IEnumerable<dom.SVGLength>.GetEnumerator();

            [Template("H5.getEnumerator({this})")]
            extern IEnumerator IEnumerable.GetEnumerator();
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGLineElement : dom.SVGGraphicsElement
        {


            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"line\")")]
            public extern SVGLineElement();

            public static dom.SVGLineElement prototype
            {
                get;
                set;
            }

            public virtual dom.SVGAnimatedLength x1
            {
                get;
            }

            public virtual dom.SVGAnimatedLength x2
            {
                get;
            }

            public virtual dom.SVGAnimatedLength y1
            {
                get;
            }

            public virtual dom.SVGAnimatedLength y2
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGLineElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGLineElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGLineElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGLineElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGLineElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGLineElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGLineElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGLineElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGLinearGradientElement : dom.SVGGradientElement
        {


            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"linearGradient\")")]
            public extern SVGLinearGradientElement();

            public static dom.SVGLinearGradientElement prototype
            {
                get;
                set;
            }

            public virtual dom.SVGAnimatedLength x1
            {
                get;
            }

            public virtual dom.SVGAnimatedLength x2
            {
                get;
            }

            public virtual dom.SVGAnimatedLength y1
            {
                get;
            }

            public virtual dom.SVGAnimatedLength y2
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGLinearGradientElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGLinearGradientElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGLinearGradientElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGLinearGradientElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGLinearGradientElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGLinearGradientElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGLinearGradientElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGLinearGradientElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGMarkerElement : dom.SVGElement, dom.SVGFitToViewBox.Interface, IObject
        {







            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"marker\")")]
            public extern SVGMarkerElement();

            public static dom.SVGMarkerElement prototype
            {
                get;
                set;
            }

            [Name("SVG_MARKERUNITS_STROKEWIDTH")]
            public static ushort SVG_MARKERUNITS_STROKEWIDTH_Static
            {
                get;
            }

            [Name("SVG_MARKERUNITS_UNKNOWN")]
            public static ushort SVG_MARKERUNITS_UNKNOWN_Static
            {
                get;
            }

            [Name("SVG_MARKERUNITS_USERSPACEONUSE")]
            public static ushort SVG_MARKERUNITS_USERSPACEONUSE_Static
            {
                get;
            }

            [Name("SVG_MARKER_ORIENT_ANGLE")]
            public static ushort SVG_MARKER_ORIENT_ANGLE_Static
            {
                get;
            }

            [Name("SVG_MARKER_ORIENT_AUTO")]
            public static ushort SVG_MARKER_ORIENT_AUTO_Static
            {
                get;
            }

            [Name("SVG_MARKER_ORIENT_UNKNOWN")]
            public static ushort SVG_MARKER_ORIENT_UNKNOWN_Static
            {
                get;
            }

            public virtual dom.SVGAnimatedLength markerHeight
            {
                get;
            }

            public virtual dom.SVGAnimatedEnumeration markerUnits
            {
                get;
            }

            public virtual dom.SVGAnimatedLength markerWidth
            {
                get;
            }

            public virtual dom.SVGAnimatedAngle orientAngle
            {
                get;
            }

            public virtual dom.SVGAnimatedEnumeration orientType
            {
                get;
            }

            public virtual dom.SVGAnimatedLength refX
            {
                get;
            }

            public virtual dom.SVGAnimatedLength refY
            {
                get;
            }

            public virtual extern void setOrientToAngle(dom.SVGAngle angle);

            public virtual extern void setOrientToAuto();

            public virtual ushort SVG_MARKERUNITS_STROKEWIDTH
            {
                get;
            }

            public virtual ushort SVG_MARKERUNITS_UNKNOWN
            {
                get;
            }

            public virtual ushort SVG_MARKERUNITS_USERSPACEONUSE
            {
                get;
            }

            public virtual ushort SVG_MARKER_ORIENT_ANGLE
            {
                get;
            }

            public virtual ushort SVG_MARKER_ORIENT_AUTO
            {
                get;
            }

            public virtual ushort SVG_MARKER_ORIENT_UNKNOWN
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGMarkerElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGMarkerElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGMarkerElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGMarkerElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGMarkerElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGMarkerElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGMarkerElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGMarkerElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual dom.SVGAnimatedPreserveAspectRatio preserveAspectRatio
            {
                get;
            }

            public virtual dom.SVGAnimatedRect viewBox
            {
                get;
            }

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGMaskElement : dom.SVGElement, dom.SVGTests.Interface, IObject, dom.SVGUnitTypes.Interface
        {




            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"mask\")")]
            public extern SVGMaskElement();

            public static dom.SVGMaskElement prototype
            {
                get;
                set;
            }

            public virtual dom.SVGAnimatedLength height
            {
                get;
            }

            public virtual dom.SVGAnimatedEnumeration maskContentUnits
            {
                get;
            }

            public virtual dom.SVGAnimatedEnumeration maskUnits
            {
                get;
            }

            public virtual dom.SVGAnimatedLength width
            {
                get;
            }

            public virtual dom.SVGAnimatedLength x
            {
                get;
            }

            public virtual dom.SVGAnimatedLength y
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGMaskElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGMaskElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGMaskElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGMaskElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGMaskElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGMaskElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGMaskElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGMaskElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual dom.SVGStringList requiredExtensions
            {
                get;
            }

            public virtual dom.SVGStringList requiredFeatures
            {
                get;
            }

            public virtual dom.SVGStringList systemLanguage
            {
                get;
            }

            public virtual extern bool hasExtension(string extension);

            public virtual ushort SVG_UNIT_TYPE_OBJECTBOUNDINGBOX
            {
                get;
            }

            public virtual ushort SVG_UNIT_TYPE_UNKNOWN
            {
                get;
            }

            public virtual ushort SVG_UNIT_TYPE_USERSPACEONUSE
            {
                get;
            }

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGMatrix : IObject
        {


            public static dom.SVGMatrix prototype
            {
                get;
                set;
            }

            public virtual double a
            {
                get;
                set;
            }

            public virtual double b
            {
                get;
                set;
            }

            public virtual double c
            {
                get;
                set;
            }

            public virtual double d
            {
                get;
                set;
            }

            public virtual double e
            {
                get;
                set;
            }

            public virtual double f
            {
                get;
                set;
            }

            public virtual extern dom.SVGMatrix flipX();

            public virtual extern dom.SVGMatrix flipY();

            public virtual extern dom.SVGMatrix inverse();

            public virtual extern dom.SVGMatrix multiply(dom.SVGMatrix secondMatrix);

            public virtual extern dom.SVGMatrix rotate(double angle);

            public virtual extern dom.SVGMatrix rotateFromVector(double x, double y);

            public virtual extern dom.SVGMatrix scale(double scaleFactor);

            public virtual extern dom.SVGMatrix scaleNonUniform(
              double scaleFactorX,
              double scaleFactorY);

            public virtual extern dom.SVGMatrix skewX(double angle);

            public virtual extern dom.SVGMatrix skewY(double angle);

            public virtual extern dom.SVGMatrix translate(double x, double y);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGMetadataElement : dom.SVGElement
        {
            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"metadata\")")]
            public extern SVGMetadataElement();

            public static dom.SVGMetadataElement prototype
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGMetadataElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGMetadataElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGMetadataElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGMetadataElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGMetadataElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGMetadataElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGMetadataElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGMetadataElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGNumber : IObject
        {

            public static dom.SVGNumber prototype
            {
                get;
                set;
            }

            public virtual float value
            {
                get;
                set;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGNumberList : IEnumerable<dom.SVGNumber>, IEnumerable, IH5Class, IObject
        {

            public static dom.SVGNumberList prototype
            {
                get;
                set;
            }

            public virtual uint numberOfItems
            {
                get;
            }

            public virtual extern dom.SVGNumber appendItem(dom.SVGNumber newItem);

            public virtual extern void clear();

            public virtual extern dom.SVGNumber getItem(uint index);

            public virtual extern dom.SVGNumber initialize(dom.SVGNumber newItem);

            public virtual extern dom.SVGNumber insertItemBefore(dom.SVGNumber newItem, uint index);

            public virtual extern dom.SVGNumber removeItem(uint index);

            public virtual extern dom.SVGNumber replaceItem(dom.SVGNumber newItem, uint index);

            [Template("H5.getEnumerator({this}, {T})")]
            extern IEnumerator<dom.SVGNumber> IEnumerable<dom.SVGNumber>.GetEnumerator();

            [Template("H5.getEnumerator({this})")]
            extern IEnumerator IEnumerable.GetEnumerator();
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGPathElement : dom.SVGGraphicsElement
        {

            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"path\")")]
            public extern SVGPathElement();

            public static dom.SVGPathElement prototype
            {
                get;
                set;
            }

            public virtual dom.SVGPathSegList pathSegList
            {
                get;
            }

            public virtual extern dom.SVGPathSegArcAbs createSVGPathSegArcAbs(
              double x,
              double y,
              double r1,
              double r2,
              double angle,
              bool largeArcFlag,
              bool sweepFlag);

            public virtual extern dom.SVGPathSegArcRel createSVGPathSegArcRel(
              double x,
              double y,
              double r1,
              double r2,
              double angle,
              bool largeArcFlag,
              bool sweepFlag);

            public virtual extern dom.SVGPathSegClosePath createSVGPathSegClosePath();

            public virtual extern dom.SVGPathSegCurvetoCubicAbs createSVGPathSegCurvetoCubicAbs(
              double x,
              double y,
              double x1,
              double y1,
              double x2,
              double y2);

            public virtual extern dom.SVGPathSegCurvetoCubicRel createSVGPathSegCurvetoCubicRel(
              double x,
              double y,
              double x1,
              double y1,
              double x2,
              double y2);

            public virtual extern dom.SVGPathSegCurvetoCubicSmoothAbs createSVGPathSegCurvetoCubicSmoothAbs(
              double x,
              double y,
              double x2,
              double y2);

            public virtual extern dom.SVGPathSegCurvetoCubicSmoothRel createSVGPathSegCurvetoCubicSmoothRel(
              double x,
              double y,
              double x2,
              double y2);

            public virtual extern dom.SVGPathSegCurvetoQuadraticAbs createSVGPathSegCurvetoQuadraticAbs(
              double x,
              double y,
              double x1,
              double y1);

            public virtual extern dom.SVGPathSegCurvetoQuadraticRel createSVGPathSegCurvetoQuadraticRel(
              double x,
              double y,
              double x1,
              double y1);

            public virtual extern dom.SVGPathSegCurvetoQuadraticSmoothAbs createSVGPathSegCurvetoQuadraticSmoothAbs(
              double x,
              double y);

            public virtual extern dom.SVGPathSegCurvetoQuadraticSmoothRel createSVGPathSegCurvetoQuadraticSmoothRel(
              double x,
              double y);

            public virtual extern dom.SVGPathSegLinetoAbs createSVGPathSegLinetoAbs(double x, double y);

            public virtual extern dom.SVGPathSegLinetoHorizontalAbs createSVGPathSegLinetoHorizontalAbs(
              double x);

            public virtual extern dom.SVGPathSegLinetoHorizontalRel createSVGPathSegLinetoHorizontalRel(
              double x);

            public virtual extern dom.SVGPathSegLinetoRel createSVGPathSegLinetoRel(double x, double y);

            public virtual extern dom.SVGPathSegLinetoVerticalAbs createSVGPathSegLinetoVerticalAbs(
              double y);

            public virtual extern dom.SVGPathSegLinetoVerticalRel createSVGPathSegLinetoVerticalRel(
              double y);

            public virtual extern dom.SVGPathSegMovetoAbs createSVGPathSegMovetoAbs(double x, double y);

            public virtual extern dom.SVGPathSegMovetoRel createSVGPathSegMovetoRel(double x, double y);

            public virtual extern double getPathSegAtLength(double distance);

            public virtual extern dom.SVGPoint getPointAtLength(double distance);

            public virtual extern double getTotalLength();

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGPathElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGPathElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGPathElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGPathElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGPathElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGPathElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGPathElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGPathElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGPathSeg : IObject
        {














            public static dom.SVGPathSeg prototype
            {
                get;
                set;
            }

            [Name("PATHSEG_ARC_ABS")]
            public static double PATHSEG_ARC_ABS_Static
            {
                get;
            }

            [Name("PATHSEG_ARC_REL")]
            public static double PATHSEG_ARC_REL_Static
            {
                get;
            }

            [Name("PATHSEG_CLOSEPATH")]
            public static double PATHSEG_CLOSEPATH_Static
            {
                get;
            }

            [Name("PATHSEG_CURVETO_CUBIC_ABS")]
            public static double PATHSEG_CURVETO_CUBIC_ABS_Static
            {
                get;
            }

            [Name("PATHSEG_CURVETO_CUBIC_REL")]
            public static double PATHSEG_CURVETO_CUBIC_REL_Static
            {
                get;
            }

            [Name("PATHSEG_CURVETO_CUBIC_SMOOTH_ABS")]
            public static double PATHSEG_CURVETO_CUBIC_SMOOTH_ABS_Static
            {
                get;
            }

            [Name("PATHSEG_CURVETO_CUBIC_SMOOTH_REL")]
            public static double PATHSEG_CURVETO_CUBIC_SMOOTH_REL_Static
            {
                get;
            }

            [Name("PATHSEG_CURVETO_QUADRATIC_ABS")]
            public static double PATHSEG_CURVETO_QUADRATIC_ABS_Static
            {
                get;
            }

            [Name("PATHSEG_CURVETO_QUADRATIC_REL")]
            public static double PATHSEG_CURVETO_QUADRATIC_REL_Static
            {
                get;
            }

            [Name("PATHSEG_CURVETO_QUADRATIC_SMOOTH_ABS")]
            public static double PATHSEG_CURVETO_QUADRATIC_SMOOTH_ABS_Static
            {
                get;
            }

            [Name("PATHSEG_CURVETO_QUADRATIC_SMOOTH_REL")]
            public static double PATHSEG_CURVETO_QUADRATIC_SMOOTH_REL_Static
            {
                get;
            }

            [Name("PATHSEG_LINETO_ABS")]
            public static double PATHSEG_LINETO_ABS_Static
            {
                get;
            }

            [Name("PATHSEG_LINETO_HORIZONTAL_ABS")]
            public static double PATHSEG_LINETO_HORIZONTAL_ABS_Static
            {
                get;
            }

            [Name("PATHSEG_LINETO_HORIZONTAL_REL")]
            public static double PATHSEG_LINETO_HORIZONTAL_REL_Static
            {
                get;
            }

            [Name("PATHSEG_LINETO_REL")]
            public static double PATHSEG_LINETO_REL_Static
            {
                get;
            }

            [Name("PATHSEG_LINETO_VERTICAL_ABS")]
            public static double PATHSEG_LINETO_VERTICAL_ABS_Static
            {
                get;
            }

            [Name("PATHSEG_LINETO_VERTICAL_REL")]
            public static double PATHSEG_LINETO_VERTICAL_REL_Static
            {
                get;
            }

            [Name("PATHSEG_MOVETO_ABS")]
            public static double PATHSEG_MOVETO_ABS_Static
            {
                get;
            }

            [Name("PATHSEG_MOVETO_REL")]
            public static double PATHSEG_MOVETO_REL_Static
            {
                get;
            }

            [Name("PATHSEG_UNKNOWN")]
            public static double PATHSEG_UNKNOWN_Static
            {
                get;
            }

            public virtual double pathSegType
            {
                get;
            }

            public virtual string pathSegTypeAsLetter
            {
                get;
            }

            public virtual double PATHSEG_ARC_ABS
            {
                get;
            }

            public virtual double PATHSEG_ARC_REL
            {
                get;
            }

            public virtual double PATHSEG_CLOSEPATH
            {
                get;
            }

            public virtual double PATHSEG_CURVETO_CUBIC_ABS
            {
                get;
            }

            public virtual double PATHSEG_CURVETO_CUBIC_REL
            {
                get;
            }

            public virtual double PATHSEG_CURVETO_CUBIC_SMOOTH_ABS
            {
                get;
            }

            public virtual double PATHSEG_CURVETO_CUBIC_SMOOTH_REL
            {
                get;
            }

            public virtual double PATHSEG_CURVETO_QUADRATIC_ABS
            {
                get;
            }

            public virtual double PATHSEG_CURVETO_QUADRATIC_REL
            {
                get;
            }

            public virtual double PATHSEG_CURVETO_QUADRATIC_SMOOTH_ABS
            {
                get;
            }

            public virtual double PATHSEG_CURVETO_QUADRATIC_SMOOTH_REL
            {
                get;
            }

            public virtual double PATHSEG_LINETO_ABS
            {
                get;
            }

            public virtual double PATHSEG_LINETO_HORIZONTAL_ABS
            {
                get;
            }

            public virtual double PATHSEG_LINETO_HORIZONTAL_REL
            {
                get;
            }

            public virtual double PATHSEG_LINETO_REL
            {
                get;
            }

            public virtual double PATHSEG_LINETO_VERTICAL_ABS
            {
                get;
            }

            public virtual double PATHSEG_LINETO_VERTICAL_REL
            {
                get;
            }

            public virtual double PATHSEG_MOVETO_ABS
            {
                get;
            }

            public virtual double PATHSEG_MOVETO_REL
            {
                get;
            }

            public virtual double PATHSEG_UNKNOWN
            {
                get;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGPathSegArcAbs : dom.SVGPathSeg
        {



            public static dom.SVGPathSegArcAbs prototype
            {
                get;
                set;
            }

            public virtual double angle
            {
                get;
                set;
            }

            public virtual bool largeArcFlag
            {
                get;
                set;
            }

            public virtual double r1
            {
                get;
                set;
            }

            public virtual double r2
            {
                get;
                set;
            }

            public virtual bool sweepFlag
            {
                get;
                set;
            }

            public virtual double x
            {
                get;
                set;
            }

            public virtual double y
            {
                get;
                set;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGPathSegArcRel : dom.SVGPathSeg
        {



            public static dom.SVGPathSegArcRel prototype
            {
                get;
                set;
            }

            public virtual double angle
            {
                get;
                set;
            }

            public virtual bool largeArcFlag
            {
                get;
                set;
            }

            public virtual double r1
            {
                get;
                set;
            }

            public virtual double r2
            {
                get;
                set;
            }

            public virtual bool sweepFlag
            {
                get;
                set;
            }

            public virtual double x
            {
                get;
                set;
            }

            public virtual double y
            {
                get;
                set;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGPathSegClosePath : dom.SVGPathSeg
        {
            public static dom.SVGPathSegClosePath prototype
            {
                get;
                set;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGPathSegCurvetoCubicAbs : dom.SVGPathSeg
        {


            public static dom.SVGPathSegCurvetoCubicAbs prototype
            {
                get;
                set;
            }

            public virtual double x
            {
                get;
                set;
            }

            public virtual double x1
            {
                get;
                set;
            }

            public virtual double x2
            {
                get;
                set;
            }

            public virtual double y
            {
                get;
                set;
            }

            public virtual double y1
            {
                get;
                set;
            }

            public virtual double y2
            {
                get;
                set;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGPathSegCurvetoCubicRel : dom.SVGPathSeg
        {


            public static dom.SVGPathSegCurvetoCubicRel prototype
            {
                get;
                set;
            }

            public virtual double x
            {
                get;
                set;
            }

            public virtual double x1
            {
                get;
                set;
            }

            public virtual double x2
            {
                get;
                set;
            }

            public virtual double y
            {
                get;
                set;
            }

            public virtual double y1
            {
                get;
                set;
            }

            public virtual double y2
            {
                get;
                set;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGPathSegCurvetoCubicSmoothAbs : dom.SVGPathSeg
        {


            public static dom.SVGPathSegCurvetoCubicSmoothAbs prototype
            {
                get;
                set;
            }

            public virtual double x
            {
                get;
                set;
            }

            public virtual double x2
            {
                get;
                set;
            }

            public virtual double y
            {
                get;
                set;
            }

            public virtual double y2
            {
                get;
                set;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGPathSegCurvetoCubicSmoothRel : dom.SVGPathSeg
        {


            public static dom.SVGPathSegCurvetoCubicSmoothRel prototype
            {
                get;
                set;
            }

            public virtual double x
            {
                get;
                set;
            }

            public virtual double x2
            {
                get;
                set;
            }

            public virtual double y
            {
                get;
                set;
            }

            public virtual double y2
            {
                get;
                set;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGPathSegCurvetoQuadraticAbs : dom.SVGPathSeg
        {


            public static dom.SVGPathSegCurvetoQuadraticAbs prototype
            {
                get;
                set;
            }

            public virtual double x
            {
                get;
                set;
            }

            public virtual double x1
            {
                get;
                set;
            }

            public virtual double y
            {
                get;
                set;
            }

            public virtual double y1
            {
                get;
                set;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGPathSegCurvetoQuadraticRel : dom.SVGPathSeg
        {


            public static dom.SVGPathSegCurvetoQuadraticRel prototype
            {
                get;
                set;
            }

            public virtual double x
            {
                get;
                set;
            }

            public virtual double x1
            {
                get;
                set;
            }

            public virtual double y
            {
                get;
                set;
            }

            public virtual double y1
            {
                get;
                set;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGPathSegCurvetoQuadraticSmoothAbs : dom.SVGPathSeg
        {


            public static dom.SVGPathSegCurvetoQuadraticSmoothAbs prototype
            {
                get;
                set;
            }

            public virtual double x
            {
                get;
                set;
            }

            public virtual double y
            {
                get;
                set;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGPathSegCurvetoQuadraticSmoothRel : dom.SVGPathSeg
        {


            public static dom.SVGPathSegCurvetoQuadraticSmoothRel prototype
            {
                get;
                set;
            }

            public virtual double x
            {
                get;
                set;
            }

            public virtual double y
            {
                get;
                set;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGPathSegLinetoAbs : dom.SVGPathSeg
        {


            public static dom.SVGPathSegLinetoAbs prototype
            {
                get;
                set;
            }

            public virtual double x
            {
                get;
                set;
            }

            public virtual double y
            {
                get;
                set;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGPathSegLinetoHorizontalAbs : dom.SVGPathSeg
        {

            public static dom.SVGPathSegLinetoHorizontalAbs prototype
            {
                get;
                set;
            }

            public virtual double x
            {
                get;
                set;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGPathSegLinetoHorizontalRel : dom.SVGPathSeg
        {

            public static dom.SVGPathSegLinetoHorizontalRel prototype
            {
                get;
                set;
            }

            public virtual double x
            {
                get;
                set;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGPathSegLinetoRel : dom.SVGPathSeg
        {


            public static dom.SVGPathSegLinetoRel prototype
            {
                get;
                set;
            }

            public virtual double x
            {
                get;
                set;
            }

            public virtual double y
            {
                get;
                set;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGPathSegLinetoVerticalAbs : dom.SVGPathSeg
        {

            public static dom.SVGPathSegLinetoVerticalAbs prototype
            {
                get;
                set;
            }

            public virtual double y
            {
                get;
                set;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGPathSegLinetoVerticalRel : dom.SVGPathSeg
        {

            public static dom.SVGPathSegLinetoVerticalRel prototype
            {
                get;
                set;
            }

            public virtual double y
            {
                get;
                set;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGPathSegList : IEnumerable<dom.SVGPathSeg>, IEnumerable, IH5Class, IObject
        {

            public static dom.SVGPathSegList prototype
            {
                get;
                set;
            }

            public virtual double numberOfItems
            {
                get;
            }

            public virtual extern dom.SVGPathSeg appendItem(dom.SVGPathSeg newItem);

            public virtual extern void clear();

            public virtual extern dom.SVGPathSeg getItem(double index);

            public virtual extern dom.SVGPathSeg initialize(dom.SVGPathSeg newItem);

            public virtual extern dom.SVGPathSeg insertItemBefore(dom.SVGPathSeg newItem, double index);

            public virtual extern dom.SVGPathSeg removeItem(double index);

            public virtual extern dom.SVGPathSeg replaceItem(dom.SVGPathSeg newItem, double index);

            [Template("H5.getEnumerator({this}, {T})")]
            extern IEnumerator<dom.SVGPathSeg> IEnumerable<dom.SVGPathSeg>.GetEnumerator();

            [Template("H5.getEnumerator({this})")]
            extern IEnumerator IEnumerable.GetEnumerator();
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGPathSegMovetoAbs : dom.SVGPathSeg
        {


            public static dom.SVGPathSegMovetoAbs prototype
            {
                get;
                set;
            }

            public virtual double x
            {
                get;
                set;
            }

            public virtual double y
            {
                get;
                set;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGPathSegMovetoRel : dom.SVGPathSeg
        {


            public static dom.SVGPathSegMovetoRel prototype
            {
                get;
                set;
            }

            public virtual double x
            {
                get;
                set;
            }

            public virtual double y
            {
                get;
                set;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGPatternElement : dom.SVGElement, dom.SVGTests.Interface, IObject, dom.SVGUnitTypes.Interface, dom.SVGFitToViewBox.Interface, dom.SVGURIReference.Interface
        {






            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"pattern\")")]
            public extern SVGPatternElement();

            public static dom.SVGPatternElement prototype
            {
                get;
                set;
            }

            public virtual dom.SVGAnimatedLength height
            {
                get;
            }

            public virtual dom.SVGAnimatedEnumeration patternContentUnits
            {
                get;
            }

            public virtual dom.SVGAnimatedTransformList patternTransform
            {
                get;
            }

            public virtual dom.SVGAnimatedEnumeration patternUnits
            {
                get;
            }

            public virtual dom.SVGAnimatedLength width
            {
                get;
            }

            public virtual dom.SVGAnimatedLength x
            {
                get;
            }

            public virtual dom.SVGAnimatedLength y
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGPatternElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGPatternElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGPatternElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGPatternElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGPatternElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGPatternElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGPatternElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGPatternElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual dom.SVGStringList requiredExtensions
            {
                get;
            }

            public virtual dom.SVGStringList requiredFeatures
            {
                get;
            }

            public virtual dom.SVGStringList systemLanguage
            {
                get;
            }

            public virtual extern bool hasExtension(string extension);

            public virtual ushort SVG_UNIT_TYPE_OBJECTBOUNDINGBOX
            {
                get;
            }

            public virtual ushort SVG_UNIT_TYPE_UNKNOWN
            {
                get;
            }

            public virtual ushort SVG_UNIT_TYPE_USERSPACEONUSE
            {
                get;
            }

            public virtual dom.SVGAnimatedPreserveAspectRatio preserveAspectRatio
            {
                get;
            }

            public virtual dom.SVGAnimatedRect viewBox
            {
                get;
            }

            public virtual dom.SVGAnimatedString href
            {
                get;
            }

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGPoint : IObject
        {


            public static dom.SVGPoint prototype
            {
                get;
                set;
            }

            public virtual double x
            {
                get;
                set;
            }

            public virtual double y
            {
                get;
                set;
            }

            public virtual extern dom.SVGPoint matrixTransform(dom.SVGMatrix matrix);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGPointList : IEnumerable<dom.SVGPoint>, IEnumerable, IH5Class, IObject
        {

            public static dom.SVGPointList prototype
            {
                get;
                set;
            }

            public virtual uint numberOfItems
            {
                get;
            }

            public virtual extern dom.SVGPoint appendItem(dom.SVGPoint newItem);

            public virtual extern void clear();

            public virtual extern dom.SVGPoint getItem(uint index);

            public virtual extern dom.SVGPoint initialize(dom.SVGPoint newItem);

            public virtual extern dom.SVGPoint insertItemBefore(dom.SVGPoint newItem, uint index);

            public virtual extern dom.SVGPoint removeItem(uint index);

            public virtual extern dom.SVGPoint replaceItem(dom.SVGPoint newItem, uint index);

            [Template("H5.getEnumerator({this}, {T})")]
            extern IEnumerator<dom.SVGPoint> IEnumerable<dom.SVGPoint>.GetEnumerator();

            [Template("H5.getEnumerator({this})")]
            extern IEnumerator IEnumerable.GetEnumerator();
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGPolygonElement : dom.SVGGraphicsElement, dom.SVGAnimatedPoints.Interface, IObject
        {


            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"polygon\")")]
            public extern SVGPolygonElement();

            public static dom.SVGPolygonElement prototype
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGPolygonElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGPolygonElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGPolygonElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGPolygonElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGPolygonElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGPolygonElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGPolygonElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGPolygonElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual dom.SVGPointList animatedPoints
            {
                get;
            }

            public virtual dom.SVGPointList points
            {
                get;
            }

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGPolylineElement : dom.SVGGraphicsElement, dom.SVGAnimatedPoints.Interface, IObject
        {


            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"polyline\")")]
            public extern SVGPolylineElement();

            public static dom.SVGPolylineElement prototype
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGPolylineElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGPolylineElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGPolylineElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGPolylineElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGPolylineElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGPolylineElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGPolylineElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGPolylineElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual dom.SVGPointList animatedPoints
            {
                get;
            }

            public virtual dom.SVGPointList points
            {
                get;
            }

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGPreserveAspectRatio : IObject
        {










            public static dom.SVGPreserveAspectRatio prototype
            {
                get;
                set;
            }

            [Name("SVG_MEETORSLICE_MEET")]
            public static ushort SVG_MEETORSLICE_MEET_Static
            {
                get;
            }

            [Name("SVG_MEETORSLICE_SLICE")]
            public static ushort SVG_MEETORSLICE_SLICE_Static
            {
                get;
            }

            [Name("SVG_MEETORSLICE_UNKNOWN")]
            public static ushort SVG_MEETORSLICE_UNKNOWN_Static
            {
                get;
            }

            [Name("SVG_PRESERVEASPECTRATIO_NONE")]
            public static ushort SVG_PRESERVEASPECTRATIO_NONE_Static
            {
                get;
            }

            [Name("SVG_PRESERVEASPECTRATIO_UNKNOWN")]
            public static ushort SVG_PRESERVEASPECTRATIO_UNKNOWN_Static
            {
                get;
            }

            [Name("SVG_PRESERVEASPECTRATIO_XMAXYMAX")]
            public static ushort SVG_PRESERVEASPECTRATIO_XMAXYMAX_Static
            {
                get;
            }

            [Name("SVG_PRESERVEASPECTRATIO_XMAXYMID")]
            public static ushort SVG_PRESERVEASPECTRATIO_XMAXYMID_Static
            {
                get;
            }

            [Name("SVG_PRESERVEASPECTRATIO_XMAXYMIN")]
            public static ushort SVG_PRESERVEASPECTRATIO_XMAXYMIN_Static
            {
                get;
            }

            [Name("SVG_PRESERVEASPECTRATIO_XMIDYMAX")]
            public static ushort SVG_PRESERVEASPECTRATIO_XMIDYMAX_Static
            {
                get;
            }

            [Name("SVG_PRESERVEASPECTRATIO_XMIDYMID")]
            public static ushort SVG_PRESERVEASPECTRATIO_XMIDYMID_Static
            {
                get;
            }

            [Name("SVG_PRESERVEASPECTRATIO_XMIDYMIN")]
            public static ushort SVG_PRESERVEASPECTRATIO_XMIDYMIN_Static
            {
                get;
            }

            [Name("SVG_PRESERVEASPECTRATIO_XMINYMAX")]
            public static ushort SVG_PRESERVEASPECTRATIO_XMINYMAX_Static
            {
                get;
            }

            [Name("SVG_PRESERVEASPECTRATIO_XMINYMID")]
            public static ushort SVG_PRESERVEASPECTRATIO_XMINYMID_Static
            {
                get;
            }

            [Name("SVG_PRESERVEASPECTRATIO_XMINYMIN")]
            public static ushort SVG_PRESERVEASPECTRATIO_XMINYMIN_Static
            {
                get;
            }

            public virtual ushort align
            {
                get;
                set;
            }

            public virtual ushort meetOrSlice
            {
                get;
                set;
            }

            public virtual ushort SVG_MEETORSLICE_MEET
            {
                get;
            }

            public virtual ushort SVG_MEETORSLICE_SLICE
            {
                get;
            }

            public virtual ushort SVG_MEETORSLICE_UNKNOWN
            {
                get;
            }

            public virtual ushort SVG_PRESERVEASPECTRATIO_NONE
            {
                get;
            }

            public virtual ushort SVG_PRESERVEASPECTRATIO_UNKNOWN
            {
                get;
            }

            public virtual ushort SVG_PRESERVEASPECTRATIO_XMAXYMAX
            {
                get;
            }

            public virtual ushort SVG_PRESERVEASPECTRATIO_XMAXYMID
            {
                get;
            }

            public virtual ushort SVG_PRESERVEASPECTRATIO_XMAXYMIN
            {
                get;
            }

            public virtual ushort SVG_PRESERVEASPECTRATIO_XMIDYMAX
            {
                get;
            }

            public virtual ushort SVG_PRESERVEASPECTRATIO_XMIDYMID
            {
                get;
            }

            public virtual ushort SVG_PRESERVEASPECTRATIO_XMIDYMIN
            {
                get;
            }

            public virtual ushort SVG_PRESERVEASPECTRATIO_XMINYMAX
            {
                get;
            }

            public virtual ushort SVG_PRESERVEASPECTRATIO_XMINYMID
            {
                get;
            }

            public virtual ushort SVG_PRESERVEASPECTRATIO_XMINYMIN
            {
                get;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGRadialGradientElement : dom.SVGGradientElement
        {



            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"radialGradient\")")]
            public extern SVGRadialGradientElement();

            public static dom.SVGRadialGradientElement prototype
            {
                get;
                set;
            }

            public virtual dom.SVGAnimatedLength cx
            {
                get;
            }

            public virtual dom.SVGAnimatedLength cy
            {
                get;
            }

            public virtual dom.SVGAnimatedLength fx
            {
                get;
            }

            public virtual dom.SVGAnimatedLength fy
            {
                get;
            }

            public virtual dom.SVGAnimatedLength r
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGRadialGradientElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGRadialGradientElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGRadialGradientElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGRadialGradientElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGRadialGradientElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGRadialGradientElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGRadialGradientElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGRadialGradientElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGRect : IObject
        {


            public static dom.SVGRect prototype
            {
                get;
                set;
            }

            public virtual double height
            {
                get;
                set;
            }

            public virtual double width
            {
                get;
                set;
            }

            public virtual double x
            {
                get;
                set;
            }

            public virtual double y
            {
                get;
                set;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGRectElement : dom.SVGGraphicsElement
        {


            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"rect\")")]
            public extern SVGRectElement();

            public static dom.SVGRectElement prototype
            {
                get;
                set;
            }

            public virtual dom.SVGAnimatedLength height
            {
                get;
            }

            public virtual dom.SVGAnimatedLength rx
            {
                get;
            }

            public virtual dom.SVGAnimatedLength ry
            {
                get;
            }

            public virtual dom.SVGAnimatedLength width
            {
                get;
            }

            public virtual dom.SVGAnimatedLength x
            {
                get;
            }

            public virtual dom.SVGAnimatedLength y
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGRectElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGRectElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGRectElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGRectElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGRectElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGRectElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGRectElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGRectElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class SVGSVGElementEventMap : dom.SVGElementEventMap
        {



            public dom.Event SVGAbort
            {
                get;
                set;
            }

            public dom.Event SVGError
            {
                get;
                set;
            }

            public dom.UIEvent resize
            {
                get;
                set;
            }

            public dom.UIEvent scroll
            {
                get;
                set;
            }

            public dom.Event SVGUnload
            {
                get;
                set;
            }

            public dom.SVGZoomEvent SVGZoom
            {
                get;
                set;
            }

            [Generated]
            public new static class KeyOf
            {
                [Template("\"SVGAbort\"")]
                public static readonly KeyOf<dom.SVGSVGElementEventMap> SVGAbort;
                [Template("\"SVGError\"")]
                public static readonly KeyOf<dom.SVGSVGElementEventMap> SVGError;
                [Template("\"resize\"")]
                public static readonly KeyOf<dom.SVGSVGElementEventMap> resize;
                [Template("\"scroll\"")]
                public static readonly KeyOf<dom.SVGSVGElementEventMap> scroll;
                [Template("\"SVGUnload\"")]
                public static readonly KeyOf<dom.SVGSVGElementEventMap> SVGUnload;
                [Template("\"SVGZoom\"")]
                public static readonly KeyOf<dom.SVGSVGElementEventMap> SVGZoom;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGSVGElement : dom.SVGGraphicsElement, dom.DocumentEvent.Interface, IObject, dom.SVGFitToViewBox.Interface, dom.SVGZoomAndPan.Interface
        {








            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"svg\")")]
            public extern SVGSVGElement();

            public static dom.SVGSVGElement prototype
            {
                get;
                set;
            }

            public virtual string contentScriptType
            {
                get;
                set;
            }

            public virtual string contentStyleType
            {
                get;
                set;
            }

            public virtual float currentScale
            {
                get;
                set;
            }

            public virtual dom.SVGPoint currentTranslate
            {
                get;
            }

            public virtual dom.SVGAnimatedLength height
            {
                get;
            }

            public virtual dom.SVGSVGElement.onabortFn onabort
            {
                get;
                set;
            }

            public virtual dom.SVGSVGElement.onabortFn onerror
            {
                get;
                set;
            }

            public virtual dom.SVGSVGElement.onresizeFn onresize
            {
                get;
                set;
            }

            public virtual dom.SVGSVGElement.onresizeFn onscroll
            {
                get;
                set;
            }

            public virtual dom.SVGSVGElement.onabortFn onunload
            {
                get;
                set;
            }

            public virtual dom.SVGSVGElement.onzoomFn onzoom
            {
                get;
                set;
            }

            public virtual double pixelUnitToMillimeterX
            {
                get;
            }

            public virtual double pixelUnitToMillimeterY
            {
                get;
            }

            public virtual double screenPixelToMillimeterX
            {
                get;
            }

            public virtual double screenPixelToMillimeterY
            {
                get;
            }

            public virtual dom.SVGRect viewport
            {
                get;
            }

            public virtual dom.SVGAnimatedLength width
            {
                get;
            }

            public virtual dom.SVGAnimatedLength x
            {
                get;
            }

            public virtual dom.SVGAnimatedLength y
            {
                get;
            }

            public virtual extern bool checkEnclosure(dom.SVGElement element, dom.SVGRect rect);

            public virtual extern bool checkIntersection(dom.SVGElement element, dom.SVGRect rect);

            public virtual extern dom.SVGAngle createSVGAngle();

            public virtual extern dom.SVGLength createSVGLength();

            public virtual extern dom.SVGMatrix createSVGMatrix();

            public virtual extern dom.SVGNumber createSVGNumber();

            public virtual extern dom.SVGPoint createSVGPoint();

            public virtual extern dom.SVGRect createSVGRect();

            public virtual extern dom.SVGTransform createSVGTransform();

            public virtual extern dom.SVGTransform createSVGTransformFromMatrix(dom.SVGMatrix matrix);

            public virtual extern void deselectAll();

            public virtual extern void forceRedraw();

            public virtual extern dom.CSSStyleDeclaration getComputedStyle(dom.Element elt);

            public virtual extern dom.CSSStyleDeclaration getComputedStyle(
              dom.Element elt,
              string pseudoElt);

            public virtual extern double getCurrentTime();

            public virtual extern dom.Element getElementById(string elementId);

            public virtual extern dom.NodeListOf<dom.SVGGraphicsElement> getEnclosureList(
              dom.SVGRect rect,
              dom.SVGElement referenceElement);

            public virtual extern dom.NodeListOf<dom.SVGGraphicsElement> getIntersectionList(
              dom.SVGRect rect,
              dom.SVGElement referenceElement);

            public virtual extern void pauseAnimations();

            public virtual extern void setCurrentTime(double seconds);

            public virtual extern uint suspendRedraw(uint maxWaitMilliseconds);

            public virtual extern void unpauseAnimations();

            public virtual extern void unsuspendRedraw(uint suspendHandleID);

            public virtual extern void unsuspendRedrawAll();

            [Where("K", typeof(KeyOf<dom.SVGSVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGSVGElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGSVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGSVGElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGSVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGSVGElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGSVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGSVGElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGSVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGSVGElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGSVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGSVGElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGSVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGSVGElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGSVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGSVGElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

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

            public virtual dom.SVGAnimatedPreserveAspectRatio preserveAspectRatio
            {
                get;
            }

            public virtual dom.SVGAnimatedRect viewBox
            {
                get;
            }

            public virtual ushort zoomAndPan
            {
                get;
            }

            [Generated]
            public delegate void onabortFn(dom.Event ev);

            [Generated]
            public delegate void onresizeFn(dom.UIEvent ev);

            [Generated]
            public delegate void onzoomFn(dom.SVGZoomEvent ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGSVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGSVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGScriptElement : dom.SVGElement, dom.SVGURIReference.Interface, IObject
        {


            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"script\")")]
            public extern SVGScriptElement();

            public static dom.SVGScriptElement prototype
            {
                get;
                set;
            }

            public virtual string type
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGScriptElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGScriptElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGScriptElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGScriptElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGScriptElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGScriptElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGScriptElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGScriptElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual dom.SVGAnimatedString href
            {
                get;
            }

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGStopElement : dom.SVGElement
        {

            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"stop\")")]
            public extern SVGStopElement();

            public static dom.SVGStopElement prototype
            {
                get;
                set;
            }

            public virtual dom.SVGAnimatedNumber offset
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGStopElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGStopElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGStopElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGStopElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGStopElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGStopElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGStopElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGStopElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGStringList : IEnumerable<string>, IEnumerable, IH5Class, IObject
        {

            public static dom.SVGStringList prototype
            {
                get;
                set;
            }

            public virtual uint numberOfItems
            {
                get;
            }

            public virtual extern string appendItem(string newItem);

            public virtual extern void clear();

            public virtual extern string getItem(uint index);

            public virtual extern string initialize(string newItem);

            public virtual extern string insertItemBefore(string newItem, uint index);

            public virtual extern string removeItem(uint index);

            public virtual extern string replaceItem(string newItem, uint index);

            [Template("H5.getEnumerator({this}, {T})")]
            extern IEnumerator<string> IEnumerable<string>.GetEnumerator();

            [Template("H5.getEnumerator({this})")]
            extern IEnumerator IEnumerable.GetEnumerator();
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGStylable : IObject
        {

            public static dom.SVGStylable prototype
            {
                get;
                set;
            }

            public virtual object className
            {
                get;
                set;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGStyleElement : dom.SVGElement
        {


            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"style\")")]
            public extern SVGStyleElement();

            public static dom.SVGStyleElement prototype
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

            public virtual string title
            {
                get;
                set;
            }

            public virtual string type
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGStyleElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGStyleElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGStyleElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGStyleElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGStyleElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGStyleElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGStyleElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGStyleElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGSwitchElement : dom.SVGGraphicsElement
        {
            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"switch\")")]
            public extern SVGSwitchElement();

            public static dom.SVGSwitchElement prototype
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGSwitchElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGSwitchElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGSwitchElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGSwitchElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGSwitchElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGSwitchElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGSwitchElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGSwitchElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGSymbolElement : dom.SVGElement, dom.SVGFitToViewBox.Interface, IObject
        {


            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"symbol\")")]
            public extern SVGSymbolElement();

            public static dom.SVGSymbolElement prototype
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGSymbolElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGSymbolElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGSymbolElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGSymbolElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGSymbolElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGSymbolElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGSymbolElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGSymbolElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual dom.SVGAnimatedPreserveAspectRatio preserveAspectRatio
            {
                get;
            }

            public virtual dom.SVGAnimatedRect viewBox
            {
                get;
            }

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGTSpanElement : dom.SVGTextPositioningElement
        {
            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"tspan\")")]
            public extern SVGTSpanElement();

            public static dom.SVGTSpanElement prototype
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGTSpanElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGTSpanElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGTSpanElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGTSpanElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGTSpanElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGTSpanElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGTSpanElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGTSpanElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [IgnoreCast]
        [Virtual]
        [InterfaceWrapper]
        public abstract class SVGTests : dom.SVGTests.Interface, IObject
        {
            public abstract dom.SVGStringList requiredExtensions { get; }

            public abstract dom.SVGStringList requiredFeatures { get; }

            public abstract dom.SVGStringList systemLanguage { get; }

            public abstract bool hasExtension(string extension);

            [Generated]
            [IgnoreCast]
            public interface Interface : IObject
            {
                dom.SVGStringList requiredExtensions { get; }

                dom.SVGStringList requiredFeatures { get; }

                dom.SVGStringList systemLanguage { get; }

                bool hasExtension(string extension);
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGTextContentElement : dom.SVGGraphicsElement
        {




            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"textContent\")")]
            public extern SVGTextContentElement();

            public static dom.SVGTextContentElement prototype
            {
                get;
                set;
            }

            [Name("LENGTHADJUST_SPACING")]
            public static ushort LENGTHADJUST_SPACING_Static
            {
                get;
            }

            [Name("LENGTHADJUST_SPACINGANDGLYPHS")]
            public static ushort LENGTHADJUST_SPACINGANDGLYPHS_Static
            {
                get;
            }

            [Name("LENGTHADJUST_UNKNOWN")]
            public static ushort LENGTHADJUST_UNKNOWN_Static
            {
                get;
            }

            public virtual dom.SVGAnimatedEnumeration lengthAdjust
            {
                get;
            }

            public virtual dom.SVGAnimatedLength textLength
            {
                get;
            }

            public virtual extern int getCharNumAtPosition(dom.SVGPoint point);

            public virtual extern float getComputedTextLength();

            public virtual extern dom.SVGPoint getEndPositionOfChar(uint charnum);

            public virtual extern dom.SVGRect getExtentOfChar(uint charnum);

            public virtual extern int getNumberOfChars();

            public virtual extern float getRotationOfChar(uint charnum);

            public virtual extern dom.SVGPoint getStartPositionOfChar(uint charnum);

            public virtual extern float getSubStringLength(uint charnum, uint nchars);

            public virtual extern void selectSubString(uint charnum, uint nchars);

            public virtual ushort LENGTHADJUST_SPACING
            {
                get;
            }

            public virtual ushort LENGTHADJUST_SPACINGANDGLYPHS
            {
                get;
            }

            public virtual ushort LENGTHADJUST_UNKNOWN
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGTextContentElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGTextContentElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGTextContentElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGTextContentElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGTextContentElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGTextContentElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGTextContentElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGTextContentElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGTextElement : dom.SVGTextPositioningElement
        {
            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"text\")")]
            public extern SVGTextElement();

            public static dom.SVGTextElement prototype
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGTextElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGTextElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGTextElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGTextElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGTextElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGTextElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGTextElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGTextElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGTextPathElement : dom.SVGTextContentElement, dom.SVGURIReference.Interface, IObject
        {






            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"textPath\")")]
            public extern SVGTextPathElement();

            public static dom.SVGTextPathElement prototype
            {
                get;
                set;
            }

            [Name("TEXTPATH_METHODTYPE_ALIGN")]
            public static ushort TEXTPATH_METHODTYPE_ALIGN_Static
            {
                get;
            }

            [Name("TEXTPATH_METHODTYPE_STRETCH")]
            public static ushort TEXTPATH_METHODTYPE_STRETCH_Static
            {
                get;
            }

            [Name("TEXTPATH_METHODTYPE_UNKNOWN")]
            public static ushort TEXTPATH_METHODTYPE_UNKNOWN_Static
            {
                get;
            }

            [Name("TEXTPATH_SPACINGTYPE_AUTO")]
            public static ushort TEXTPATH_SPACINGTYPE_AUTO_Static
            {
                get;
            }

            [Name("TEXTPATH_SPACINGTYPE_EXACT")]
            public static ushort TEXTPATH_SPACINGTYPE_EXACT_Static
            {
                get;
            }

            [Name("TEXTPATH_SPACINGTYPE_UNKNOWN")]
            public static ushort TEXTPATH_SPACINGTYPE_UNKNOWN_Static
            {
                get;
            }

            public virtual dom.SVGAnimatedEnumeration method
            {
                get;
            }

            public virtual dom.SVGAnimatedEnumeration spacing
            {
                get;
            }

            public virtual dom.SVGAnimatedLength startOffset
            {
                get;
            }

            public virtual ushort TEXTPATH_METHODTYPE_ALIGN
            {
                get;
            }

            public virtual ushort TEXTPATH_METHODTYPE_STRETCH
            {
                get;
            }

            public virtual ushort TEXTPATH_METHODTYPE_UNKNOWN
            {
                get;
            }

            public virtual ushort TEXTPATH_SPACINGTYPE_AUTO
            {
                get;
            }

            public virtual ushort TEXTPATH_SPACINGTYPE_EXACT
            {
                get;
            }

            public virtual ushort TEXTPATH_SPACINGTYPE_UNKNOWN
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGTextPathElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGTextPathElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGTextPathElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGTextPathElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGTextPathElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGTextPathElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGTextPathElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGTextPathElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual dom.SVGAnimatedString href
            {
                get;
            }

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGTextPositioningElement : dom.SVGTextContentElement
        {



            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"textPositioning\")")]
            public extern SVGTextPositioningElement();

            public static dom.SVGTextPositioningElement prototype
            {
                get;
                set;
            }

            public virtual dom.SVGAnimatedLengthList dx
            {
                get;
            }

            public virtual dom.SVGAnimatedLengthList dy
            {
                get;
            }

            public virtual dom.SVGAnimatedNumberList rotate
            {
                get;
            }

            public virtual dom.SVGAnimatedLengthList x
            {
                get;
            }

            public virtual dom.SVGAnimatedLengthList y
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGTextPositioningElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGTextPositioningElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGTextPositioningElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGTextPositioningElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGTextPositioningElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGTextPositioningElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGTextPositioningElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGTextPositioningElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGTitleElement : dom.SVGElement
        {
            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"title\")")]
            public extern SVGTitleElement();

            public static dom.SVGTitleElement prototype
            {
                get;
                set;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGTitleElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGTitleElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGTitleElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGTitleElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGTitleElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGTitleElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGTitleElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGTitleElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGTransform : IObject
        {







            public static dom.SVGTransform prototype
            {
                get;
                set;
            }

            [Name("SVG_TRANSFORM_MATRIX")]
            public static ushort SVG_TRANSFORM_MATRIX_Static
            {
                get;
            }

            [Name("SVG_TRANSFORM_ROTATE")]
            public static ushort SVG_TRANSFORM_ROTATE_Static
            {
                get;
            }

            [Name("SVG_TRANSFORM_SCALE")]
            public static ushort SVG_TRANSFORM_SCALE_Static
            {
                get;
            }

            [Name("SVG_TRANSFORM_SKEWX")]
            public static ushort SVG_TRANSFORM_SKEWX_Static
            {
                get;
            }

            [Name("SVG_TRANSFORM_SKEWY")]
            public static ushort SVG_TRANSFORM_SKEWY_Static
            {
                get;
            }

            [Name("SVG_TRANSFORM_TRANSLATE")]
            public static ushort SVG_TRANSFORM_TRANSLATE_Static
            {
                get;
            }

            [Name("SVG_TRANSFORM_UNKNOWN")]
            public static ushort SVG_TRANSFORM_UNKNOWN_Static
            {
                get;
            }

            public virtual float angle
            {
                get;
            }

            public virtual dom.SVGMatrix matrix
            {
                get;
            }

            public virtual ushort type
            {
                get;
            }

            public virtual extern void setMatrix(dom.SVGMatrix matrix);

            public virtual extern void setRotate(float angle, float cx, float cy);

            public virtual extern void setScale(float sx, float sy);

            public virtual extern void setSkewX(float angle);

            public virtual extern void setSkewY(float angle);

            public virtual extern void setTranslate(float tx, float ty);

            public virtual ushort SVG_TRANSFORM_MATRIX
            {
                get;
            }

            public virtual ushort SVG_TRANSFORM_ROTATE
            {
                get;
            }

            public virtual ushort SVG_TRANSFORM_SCALE
            {
                get;
            }

            public virtual ushort SVG_TRANSFORM_SKEWX
            {
                get;
            }

            public virtual ushort SVG_TRANSFORM_SKEWY
            {
                get;
            }

            public virtual ushort SVG_TRANSFORM_TRANSLATE
            {
                get;
            }

            public virtual ushort SVG_TRANSFORM_UNKNOWN
            {
                get;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGTransformList : IEnumerable<dom.SVGTransform>, IEnumerable, IH5Class, IObject
        {

            public static dom.SVGTransformList prototype
            {
                get;
                set;
            }

            public virtual uint numberOfItems
            {
                get;
            }

            public virtual extern dom.SVGTransform appendItem(dom.SVGTransform newItem);

            public virtual extern void clear();

            public virtual extern dom.SVGTransform consolidate();

            public virtual extern dom.SVGTransform createSVGTransformFromMatrix(dom.SVGMatrix matrix);

            public virtual extern dom.SVGTransform getItem(uint index);

            public virtual extern dom.SVGTransform initialize(dom.SVGTransform newItem);

            public virtual extern dom.SVGTransform insertItemBefore(
              dom.SVGTransform newItem,
              uint index);

            public virtual extern dom.SVGTransform removeItem(uint index);

            public virtual extern dom.SVGTransform replaceItem(dom.SVGTransform newItem, uint index);

            [Template("H5.getEnumerator({this}, {T})")]
            extern IEnumerator<dom.SVGTransform> IEnumerable<dom.SVGTransform>.GetEnumerator();

            [Template("H5.getEnumerator({this})")]
            extern IEnumerator IEnumerable.GetEnumerator();
        }

        [IgnoreCast]
        [ObjectLiteral]
        [InterfaceWrapper]
        public class SVGURIReference : dom.SVGURIReference.Interface, IObject
        {
            public dom.SVGAnimatedString href
            {
                get;
            }

            [Generated]
            [ObjectLiteral]
            [IgnoreCast]
            public interface Interface : IObject
            {
                dom.SVGAnimatedString href { get; }
            }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [InterfaceWrapper]
        public class SVGUnitTypes : dom.SVGUnitTypes.Interface, IObject
        {


            public ushort SVG_UNIT_TYPE_OBJECTBOUNDINGBOX
            {
                get;
            }

            public ushort SVG_UNIT_TYPE_UNKNOWN
            {
                get;
            }

            public ushort SVG_UNIT_TYPE_USERSPACEONUSE
            {
                get;
            }

            [Generated]
            [ObjectLiteral]
            [IgnoreCast]
            public interface Interface : IObject
            {
                ushort SVG_UNIT_TYPE_OBJECTBOUNDINGBOX { get; }

                ushort SVG_UNIT_TYPE_UNKNOWN { get; }

                ushort SVG_UNIT_TYPE_USERSPACEONUSE { get; }
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGUseElement : dom.SVGGraphicsElement, dom.SVGURIReference.Interface, IObject
        {



            [Template("document.createElementNS(\"http://www.w3.org/2000/svg\", \"use\")")]
            public extern SVGUseElement();

            public static dom.SVGUseElement prototype
            {
                get;
                set;
            }

            public virtual dom.SVGElementInstance animatedInstanceRoot
            {
                get;
            }

            public virtual dom.SVGAnimatedLength height
            {
                get;
            }

            public virtual dom.SVGElementInstance instanceRoot
            {
                get;
            }

            public virtual dom.SVGAnimatedLength width
            {
                get;
            }

            public virtual dom.SVGAnimatedLength x
            {
                get;
            }

            public virtual dom.SVGAnimatedLength y
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGUseElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGUseElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGUseElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGUseElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGUseElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGUseElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGUseElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGUseElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual dom.SVGAnimatedString href
            {
                get;
            }

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGViewElement : dom.SVGElement, dom.SVGFitToViewBox.Interface, IObject, dom.SVGZoomAndPan.Interface
        {


            public static dom.SVGViewElement prototype
            {
                get;
                set;
            }

            public virtual dom.SVGStringList viewTarget
            {
                get;
            }

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGViewElement.addEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGViewElement.addEventListenerFn<K> listener,
              Union<bool, dom.AddEventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGViewElement.addEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void addEventListener<K>(
              K type,
              dom.SVGViewElement.addEventListenerFn<K> listener,
              dom.AddEventListenerOptions options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGViewElement.removeEventListenerFn<K> listener);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGViewElement.removeEventListenerFn<K> listener,
              Union<bool, dom.EventListenerOptions> options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGViewElement.removeEventListenerFn<K> listener,
              bool options);

            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public virtual extern void removeEventListener<K>(
              K type,
              dom.SVGViewElement.removeEventListenerFn<K> listener,
              dom.EventListenerOptions options);

            public virtual dom.SVGAnimatedPreserveAspectRatio preserveAspectRatio
            {
                get;
            }

            public virtual dom.SVGAnimatedRect viewBox
            {
                get;
            }

            public virtual ushort zoomAndPan
            {
                get;
            }

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void addEventListenerFn<K>(object ev);

            [Generated]
            [Where("K", typeof(KeyOf<dom.SVGElementEventMap>), EnableImplicitConversion = true)]
            public new delegate void removeEventListenerFn<K>(object ev);
        }

        [IgnoreCast]
        [ObjectLiteral]
        [InterfaceWrapper]
        public class SVGZoomAndPan : dom.SVGZoomAndPan.Interface, IObject
        {
            public ushort zoomAndPan
            {
                get;
            }

            [Generated]
            [ObjectLiteral]
            [IgnoreCast]
            public interface Interface : IObject
            {
                ushort zoomAndPan { get; }
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class SVGZoomEvent : dom.UIEvent
        {



            public static dom.SVGZoomEvent prototype
            {
                get;
                set;
            }

            public virtual double newScale
            {
                get;
            }

            public virtual dom.SVGPoint newTranslate
            {
                get;
            }

            public virtual double previousScale
            {
                get;
            }

            public virtual dom.SVGPoint previousTranslate
            {
                get;
            }

            public virtual dom.SVGRect zoomRectScreen
            {
                get;
            }
        }

    }
}
