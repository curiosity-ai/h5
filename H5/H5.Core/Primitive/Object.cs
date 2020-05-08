// Decompiled with JetBrains decompiler
// Type: H5.Core.Object
// Assembly: H5.es5, Version=2.8.2.0, Culture=neutral, PublicKeyToken=null
// MVID: EC57AC2B-0E02-4A1C-B567-F790F377783B
// Assembly location: C:\work\curiosity\tesserae\Tesserae\bin\Debug\net461\H5.es5.dll

using HighFive;
using System.ComponentModel;

namespace H5.Core
{
    [CombinedClass]
    [StaticInterface("ObjectConstructor")]
    [Namespace(false)]
    [Virtual]
    public class Object : Object.Interface, IObject
    {
        public extern Object();

        public extern Object(object value);

        public static Object prototype
        {
            get;
        }

        public static extern object Self();

        public static extern object Self(object value);

        public static extern object getPrototypeOf(object o);

        public static extern Union<es5.PropertyDescriptor, Undefined> getOwnPropertyDescriptor(
          object o,
          string p);

        public static extern string[] getOwnPropertyNames(object o);

        public static extern object create(IObject o);

        public static extern object create(
          IObject o,
          Intersection<es5.PropertyDescriptorMap, es5.ThisType<object>> properties);

        public static extern object defineProperty(
          object o,
          string p,
          Intersection<es5.PropertyDescriptor, es5.ThisType<object>> attributes);

        public static extern object defineProperties(
          object o,
          Intersection<es5.PropertyDescriptorMap, es5.ThisType<object>> properties);

        public static extern T seal<T>(T o);

        public static extern es5.ReadonlyArray<T> freeze<T>(T[] a);

        [Where("T", typeof(es5.Function), EnableImplicitConversion = true)]
        public static extern T freeze<T>(T f);

        [Name("freeze")]
        public static extern es5.Readonly<T> freeze2<T>(T o);

        public static extern T preventExtensions<T>(T o);

        public static extern bool isSealed(object o);

        public static extern bool isFrozen(object o);

        public static extern bool isExtensible(object o);

        public static extern string[] keys(object o);

        public virtual es5.Function constructor
        {
            get; set;
        }

        public virtual extern string toString();

        public virtual extern string toLocaleString();

        public virtual extern Object valueOf();

        public virtual extern bool hasOwnProperty(string v);

        public virtual extern bool isPrototypeOf(Object v);

        public virtual extern bool propertyIsEnumerable(string v);

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override extern string ToString();

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override extern bool Equals(object obj);

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override extern int GetHashCode();

        [Generated]
        [IgnoreCast]
        [ClassInterface]
        [Name("Object")]
        public interface Interface : IObject
        {
            es5.Function constructor { get; set; }

            string toString();

            string toLocaleString();

            Object valueOf();

            bool hasOwnProperty(string v);

            bool isPrototypeOf(Object v);

            bool propertyIsEnumerable(string v);
        }
    }
}
