// Decompiled with JetBrains decompiler
// Type: H5.Primitive.Boolean
// Assembly: H5.es5, Version=2.8.2.0, Culture=neutral, PublicKeyToken=null
// MVID: EC57AC2B-0E02-4A1C-B567-F790F377783B
// Assembly location: C:\work\curiosity\tesserae\Tesserae\bin\Debug\net461\H5.es5.dll

using HighFive;

namespace H5.Primitive
{
  [CombinedClass]
  [StaticInterface("BooleanConstructor")]
  [Namespace(false)]
  public class Boolean : Object, Boolean.Interface, IObject
  {
    private static readonly Boolean _prototype_BackingField;

    public extern Boolean();

    public extern Boolean(object value);

    public static Boolean prototype
    {
      get
      {
        return Boolean._prototype_BackingField;
      }
    }

    public static extern bool Self();

    public static extern bool Self(object value);

    public virtual extern bool valueOf();

    public static extern implicit operator Boolean(bool value);

    [Template("{this} != null ? {this}.valueOf() : {this}")]
    public static extern implicit operator bool(Boolean value);

    [Generated]
    [IgnoreCast]
    [ClassInterface]
    [Name("Boolean")]
    public new interface Interface : IObject
    {
      bool valueOf();
    }
  }
}
