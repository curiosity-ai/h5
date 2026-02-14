using H5;
using H5.Core;

namespace H5.Core
{
    public static partial class dom
    {
        [CombinedClass]
        [FormerInterface]
        public class XRSystem : dom.EventTarget
        {
            public static dom.XRSystem prototype { get; set; }

            public virtual extern es5.Promise<bool> isSessionSupported(dom.XRSessionMode mode);

            public virtual extern es5.Promise<dom.XRSession> requestSession(dom.XRSessionMode mode);

            public virtual extern es5.Promise<dom.XRSession> requestSession(dom.XRSessionMode mode, dom.XRSessionInit options);
        }

        [CombinedClass]
        [FormerInterface]
        public class XRSession : dom.EventTarget
        {
            public static dom.XRSession prototype { get; set; }

            public virtual dom.XRRenderState renderState { get; }

            public virtual dom.XRInputSource[] inputSources { get; }

            public virtual extern void updateRenderState(dom.XRRenderStateInit state);

            public virtual extern es5.Promise<dom.XRReferenceSpace> requestReferenceSpace(dom.XRReferenceSpaceType type);

            public virtual extern double requestAnimationFrame(dom.XRFrameRequestCallback callback);

            public virtual extern void cancelAnimationFrame(double handle);

            public virtual extern es5.Promise<H5.Core.Void> end();
        }

        [CombinedClass]
        [FormerInterface]
        public class XRFrame : IObject
        {
            public static dom.XRFrame prototype { get; set; }

            public virtual dom.XRSession session { get; }

            public virtual extern dom.XRViewerPose getViewerPose(dom.XRReferenceSpace referenceSpace);

            public virtual extern dom.XRPose getPose(dom.XRSpace space, dom.XRSpace baseSpace);
        }

        [CombinedClass]
        [FormerInterface]
        public class XRSpace : dom.EventTarget
        {
            public static dom.XRSpace prototype { get; set; }
        }

        [CombinedClass]
        [FormerInterface]
        public class XRReferenceSpace : dom.XRSpace
        {
            public static dom.XRReferenceSpace prototype { get; set; }

            public virtual extern dom.XRReferenceSpace getOffsetReferenceSpace(dom.XRRigidTransform originOffset);
        }

        [CombinedClass]
        [FormerInterface]
        public class XRRenderState : IObject
        {
            public static dom.XRRenderState prototype { get; set; }

            public virtual double depthNear { get; }
            public virtual double depthFar { get; }
            public virtual double inlineVerticalFieldOfView { get; }
            public virtual dom.XRWebGLLayer baseLayer { get; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class XRRenderStateInit : IObject
        {
            public double? depthNear { get; set; }
            public double? depthFar { get; set; }
            public double? inlineVerticalFieldOfView { get; set; }
            public dom.XRWebGLLayer baseLayer { get; set; }
            public dom.XRLayer[] layers { get; set; }
        }

        [CombinedClass]
        [FormerInterface]
        public class XRInputSource : IObject
        {
            public static dom.XRInputSource prototype { get; set; }
            public virtual string handedness { get; }
            public virtual string targetRayMode { get; }
            public virtual dom.XRSpace targetRaySpace { get; }
            public virtual dom.XRSpace gripSpace { get; }
            public virtual string[] profiles { get; }
        }

        [CombinedClass]
        [FormerInterface]
        public class XRPose : IObject
        {
            public static dom.XRPose prototype { get; set; }
            public virtual dom.XRRigidTransform transform { get; }
            public virtual bool emulatedPosition { get; }
        }

        [CombinedClass]
        [FormerInterface]
        public class XRViewerPose : dom.XRPose
        {
            public static dom.XRViewerPose prototype { get; set; }
            public virtual dom.XRView[] views { get; }
        }

        [CombinedClass]
        [FormerInterface]
        public class XRView : IObject
        {
            public static dom.XRView prototype { get; set; }
            public virtual dom.XREye eye { get; }
            public virtual es5.Float32Array projectionMatrix { get; }
            public virtual dom.XRRigidTransform transform { get; }
        }

        [CombinedClass]
        [FormerInterface]
        public class XRRigidTransform : IObject
        {
            public extern XRRigidTransform(dom.DOMPointInit position, dom.DOMPointInit orientation);
            public static dom.XRRigidTransform prototype { get; set; }
            public virtual dom.DOMPointReadOnly position { get; }
            public virtual dom.DOMPointReadOnly orientation { get; }
            public virtual es5.Float32Array matrix { get; }
            public virtual extern dom.XRRigidTransform inverse { get; }
        }

        [CombinedClass]
        [FormerInterface]
        public class XRWebGLLayer : dom.XRLayer
        {
            public extern XRWebGLLayer(dom.XRSession session, dom.WebGLRenderingContext context);
            public static dom.XRWebGLLayer prototype { get; set; }
        }

        [CombinedClass]
        [FormerInterface]
        public class XRLayer : dom.EventTarget
        {
            public static dom.XRLayer prototype { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class XRSessionInit : IObject
        {
            public string[] requiredFeatures { get; set; }
            public string[] optionalFeatures { get; set; }
        }

        [Generated]
        public delegate void XRFrameRequestCallback(double time, dom.XRFrame frame);

        [Name("System.String")]
        public class XRSessionMode : LiteralType<string>
        {
            [Template("<self>\"inline\"")]
            public static readonly dom.XRSessionMode @inline;

            [Template("<self>\"immersive-vr\"")]
            public static readonly dom.XRSessionMode immersiveVr;

            [Template("<self>\"immersive-ar\"")]
            public static readonly dom.XRSessionMode immersiveAr;

            private extern XRSessionMode();
            public static extern implicit operator dom.XRSessionMode(string value);
        }

        [Name("System.String")]
        public class XRReferenceSpaceType : LiteralType<string>
        {
            [Template("<self>\"viewer\"")]
            public static readonly dom.XRReferenceSpaceType viewer;

            [Template("<self>\"local\"")]
            public static readonly dom.XRReferenceSpaceType local;

            [Template("<self>\"local-floor\"")]
            public static readonly dom.XRReferenceSpaceType localFloor;

            [Template("<self>\"bounded-floor\"")]
            public static readonly dom.XRReferenceSpaceType boundedFloor;

            [Template("<self>\"unbounded\"")]
            public static readonly dom.XRReferenceSpaceType unbounded;

            private extern XRReferenceSpaceType();
            public static extern implicit operator dom.XRReferenceSpaceType(string value);
        }

        [Name("System.String")]
        public class XREye : LiteralType<string>
        {
            [Template("<self>\"none\"")]
            public static readonly dom.XREye none;

            [Template("<self>\"left\"")]
            public static readonly dom.XREye left;

            [Template("<self>\"right\"")]
            public static readonly dom.XREye right;

            private extern XREye();
            public static extern implicit operator dom.XREye(string value);
        }

        [Virtual]
        public abstract class XRSystemTypeConfig : IObject
        {
             public virtual dom.XRSystem prototype { get; set; }
        }
    }
}
