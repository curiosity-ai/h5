// Decompiled with JetBrains decompiler
// Type: H5.Core.ObjectConstructor
// Assembly: H5.es5, Version=2.8.2.0, Culture=neutral, PublicKeyToken=null
// MVID: EC57AC2B-0E02-4A1C-B567-F790F377783B
// Assembly location: C:\work\curiosity\tesserae\Tesserae\bin\Debug\net461\H5.es5.dll

using HighFive;

namespace H5.Core
{
    [IgnoreCast]
    [Namespace(false)]
    [Virtual]
    public interface ObjectConstructor : IObject
    {
        [Template("new {this}()")]
        Object New();

        [Template("new {this}({0})")]
        Object New(object value);

        [Template("{this}()")]
        object Self();

        [Template("{this}({0})")]
        object Self(object value);

        Object prototype { get; }

        object getPrototypeOf(object o);

        Union<es5.PropertyDescriptor, Undefined> getOwnPropertyDescriptor(
          object o,
          string p);

        string[] getOwnPropertyNames(object o);

        object create(IObject o);

        object create(
          IObject o,
          Intersection<es5.PropertyDescriptorMap, es5.ThisType<object>> properties);

        object defineProperty(
          object o,
          string p,
          Intersection<es5.PropertyDescriptor, es5.ThisType<object>> attributes);

        object defineProperties(
          object o,
          Intersection<es5.PropertyDescriptorMap, es5.ThisType<object>> properties);

        T seal<T>(T o);

        es5.ReadonlyArray<T> freeze<T>(T[] a);

        [Where("T", typeof(es5.Function), EnableImplicitConversion = true)]
        T freeze<T>(T f);

        [Name("freeze")]
        es5.Readonly<T> freeze2<T>(T o);

        T preventExtensions<T>(T o);

        bool isSealed(object o);

        bool isFrozen(object o);

        bool isExtensible(object o);

        string[] keys(object o);
    }
}
