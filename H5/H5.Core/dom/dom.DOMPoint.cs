using H5;
using H5.Core;

namespace H5.Core
{
    public static partial class dom
    {
        [CombinedClass]
        [FormerInterface]
        public class DOMPoint : dom.DOMPointReadOnly
        {
            public extern DOMPoint();
            public extern DOMPoint(double x);
            public extern DOMPoint(double x, double y);
            public extern DOMPoint(double x, double y, double z);
            public extern DOMPoint(double x, double y, double z, double w);

            public static dom.DOMPoint prototype { get; set; }

            public static extern dom.DOMPoint fromPoint(dom.DOMPointInit other);

            public new double x { get; set; }
            public new double y { get; set; }
            public new double z { get; set; }
            public new double w { get; set; }
        }

        [CombinedClass]
        [FormerInterface]
        public class DOMPointReadOnly : IObject
        {
            public extern DOMPointReadOnly();
            public extern DOMPointReadOnly(double x);
            public extern DOMPointReadOnly(double x, double y);
            public extern DOMPointReadOnly(double x, double y, double z);
            public extern DOMPointReadOnly(double x, double y, double z, double w);

            public static dom.DOMPointReadOnly prototype { get; set; }

            public static extern dom.DOMPointReadOnly fromPoint(dom.DOMPointInit other);

            public virtual double x { get; }
            public virtual double y { get; }
            public virtual double z { get; }
            public virtual double w { get; }

            public virtual extern dom.DOMPoint matrixTransform(dom.DOMMatrixInit matrix);
            public virtual extern object toJSON();
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class DOMPointInit : IObject
        {
            public double? x { get; set; }
            public double? y { get; set; }
            public double? z { get; set; }
            public double? w { get; set; }
        }

        // Stub for DOMMatrixInit if it doesn't exist, though it likely does if DOMMatrix is used.
        // Checking if DOMMatrix exists. If not, I should probably add it too or stub it.
        // For now, I'll assume DOMMatrixInit might not exist if DOMPoint didn't.

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class DOMMatrixInit : IObject
        {
            public double? a { get; set; }
            public double? b { get; set; }
            public double? c { get; set; }
            public double? d { get; set; }
            public double? e { get; set; }
            public double? f { get; set; }
            public double? m11 { get; set; }
            public double? m12 { get; set; }
            public double? m13 { get; set; }
            public double? m14 { get; set; }
            public double? m21 { get; set; }
            public double? m22 { get; set; }
            public double? m23 { get; set; }
            public double? m24 { get; set; }
            public double? m31 { get; set; }
            public double? m32 { get; set; }
            public double? m33 { get; set; }
            public double? m34 { get; set; }
            public double? m41 { get; set; }
            public double? m42 { get; set; }
            public double? m43 { get; set; }
            public double? m44 { get; set; }
            public bool? is2D { get; set; }
        }

        [Virtual]
        public abstract class DOMPointTypeConfig : IObject
        {
             public virtual dom.DOMPoint prototype { get; set; }

             [Template("new {this}()")]
             public abstract dom.DOMPoint New();

             [Template("new {this}({0})")]
             public abstract dom.DOMPoint New(double x);

             [Template("new {this}({0}, {1})")]
             public abstract dom.DOMPoint New(double x, double y);

             [Template("new {this}({0}, {1}, {2})")]
             public abstract dom.DOMPoint New(double x, double y, double z);

             [Template("new {this}({0}, {1}, {2}, {3})")]
             public abstract dom.DOMPoint New(double x, double y, double z, double w);
        }

        [Virtual]
        public abstract class DOMPointReadOnlyTypeConfig : IObject
        {
             public virtual dom.DOMPointReadOnly prototype { get; set; }

             [Template("new {this}()")]
             public abstract dom.DOMPointReadOnly New();

             [Template("new {this}({0})")]
             public abstract dom.DOMPointReadOnly New(double x);

             [Template("new {this}({0}, {1})")]
             public abstract dom.DOMPointReadOnly New(double x, double y);

             [Template("new {this}({0}, {1}, {2})")]
             public abstract dom.DOMPointReadOnly New(double x, double y, double z);

             [Template("new {this}({0}, {1}, {2}, {3})")]
             public abstract dom.DOMPointReadOnly New(double x, double y, double z, double w);
        }
    }
}
