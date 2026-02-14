using H5;
using H5.Core;

namespace H5.Core
{
    public static partial class dom
    {
        [CombinedClass]
        [FormerInterface]
        public class DOMMatrix : dom.DOMMatrixReadOnly
        {
            public extern DOMMatrix();
            public extern DOMMatrix(string init);
            public extern DOMMatrix(double[] init);
            public static dom.DOMMatrix prototype { get; set; }

            public static extern dom.DOMMatrix fromMatrix(dom.DOMMatrixInit other);
            public static extern dom.DOMMatrix fromFloat32Array(es5.Float32Array array32);
            public static extern dom.DOMMatrix fromFloat64Array(es5.Float64Array array64);

            public new double a { get; set; }
            public new double b { get; set; }
            public new double c { get; set; }
            public new double d { get; set; }
            public new double e { get; set; }
            public new double f { get; set; }
            public new double m11 { get; set; }
            public new double m12 { get; set; }
            public new double m13 { get; set; }
            public new double m14 { get; set; }
            public new double m21 { get; set; }
            public new double m22 { get; set; }
            public new double m23 { get; set; }
            public new double m24 { get; set; }
            public new double m31 { get; set; }
            public new double m32 { get; set; }
            public new double m33 { get; set; }
            public new double m34 { get; set; }
            public new double m41 { get; set; }
            public new double m42 { get; set; }
            public new double m43 { get; set; }
            public new double m44 { get; set; }

            public virtual extern dom.DOMMatrix multiply(dom.DOMMatrixInit other);
            public virtual extern dom.DOMMatrix preMultiply(dom.DOMMatrixInit other);
            public virtual extern dom.DOMMatrix translate(double tx, double ty, double tz);
            public virtual extern dom.DOMMatrix scale(double scaleX, double scaleY, double scaleZ, double originX, double originY, double originZ);
            public virtual extern dom.DOMMatrix scale3d(double scale, double originX, double originY, double originZ);
            public virtual extern dom.DOMMatrix rotate(double rotX, double rotY, double rotZ);
            public virtual extern dom.DOMMatrix rotateFromVector(double x, double y);
            public virtual extern dom.DOMMatrix rotateAxisAngle(double x, double y, double z, double angle);
            public virtual extern dom.DOMMatrix skewX(double sx);
            public virtual extern dom.DOMMatrix skewY(double sy);
            public virtual extern dom.DOMMatrix invertSelf();
            public virtual extern dom.DOMMatrix setMatrixValue(string transformList);
        }

        [CombinedClass]
        [FormerInterface]
        public class DOMMatrixReadOnly : IObject
        {
            public extern DOMMatrixReadOnly();
            public extern DOMMatrixReadOnly(string init);
            public extern DOMMatrixReadOnly(double[] init);
            public static dom.DOMMatrixReadOnly prototype { get; set; }

            public static extern dom.DOMMatrixReadOnly fromMatrix(dom.DOMMatrixInit other);
            public static extern dom.DOMMatrixReadOnly fromFloat32Array(es5.Float32Array array32);
            public static extern dom.DOMMatrixReadOnly fromFloat64Array(es5.Float64Array array64);

            public virtual double a { get; }
            public virtual double b { get; }
            public virtual double c { get; }
            public virtual double d { get; }
            public virtual double e { get; }
            public virtual double f { get; }
            public virtual double m11 { get; }
            public virtual double m12 { get; }
            public virtual double m13 { get; }
            public virtual double m14 { get; }
            public virtual double m21 { get; }
            public virtual double m22 { get; }
            public virtual double m23 { get; }
            public virtual double m24 { get; }
            public virtual double m31 { get; }
            public virtual double m32 { get; }
            public virtual double m33 { get; }
            public virtual double m34 { get; }
            public virtual double m41 { get; }
            public virtual double m42 { get; }
            public virtual double m43 { get; }
            public virtual double m44 { get; }
            public virtual bool is2D { get; }
            public virtual bool isIdentity { get; }

            public virtual extern dom.DOMMatrix translate(double tx, double ty, double tz);
            public virtual extern dom.DOMMatrix scale(double scaleX, double scaleY, double scaleZ, double originX, double originY, double originZ);
            public virtual extern dom.DOMMatrix scale3d(double scale, double originX, double originY, double originZ);
            public virtual extern dom.DOMMatrix rotate(double rotX, double rotY, double rotZ);
            public virtual extern dom.DOMMatrix rotateFromVector(double x, double y);
            public virtual extern dom.DOMMatrix rotateAxisAngle(double x, double y, double z, double angle);
            public virtual extern dom.DOMMatrix skewX(double sx);
            public virtual extern dom.DOMMatrix skewY(double sy);
            public virtual extern dom.DOMMatrix multiply(dom.DOMMatrixInit other);
            public virtual extern dom.DOMMatrix flipX();
            public virtual extern dom.DOMMatrix flipY();
            public virtual extern dom.DOMMatrix inverse();
            public virtual extern dom.DOMPoint transformPoint(dom.DOMPointInit point);
            public virtual extern es5.Float32Array toFloat32Array();
            public virtual extern es5.Float64Array toFloat64Array();
            public virtual extern string toString();
            public virtual extern object toJSON();
        }

        [Virtual]
        public abstract class DOMMatrixTypeConfig : IObject
        {
             public virtual dom.DOMMatrix prototype { get; set; }

             [Template("new {this}()")]
             public abstract dom.DOMMatrix New();

             [Template("new {this}({0})")]
             public abstract dom.DOMMatrix New(string init);

             [Template("new {this}({0})")]
             public abstract dom.DOMMatrix New(double[] init);
        }

        [Virtual]
        public abstract class DOMMatrixReadOnlyTypeConfig : IObject
        {
             public virtual dom.DOMMatrixReadOnly prototype { get; set; }

             [Template("new {this}()")]
             public abstract dom.DOMMatrixReadOnly New();

             [Template("new {this}({0})")]
             public abstract dom.DOMMatrixReadOnly New(string init);

             [Template("new {this}({0})")]
             public abstract dom.DOMMatrixReadOnly New(double[] init);
        }
    }
}
