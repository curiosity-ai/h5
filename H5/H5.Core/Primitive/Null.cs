// Decompiled with JetBrains decompiler
// Type: H5.Primitive.Null
// Assembly: H5.Core, Version=1.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E855DC6-9E83-4420-9E6F-8D2B7A117BBD
// Assembly location: C:\work\curiosity\tesserae\Tesserae\bin\Debug\net461\H5.Core.dll

using HighFive;
using System;

namespace H5.Primitive
{
  [IgnoreCast]
  [Virtual]
  [ExportedAs("null")]
  public class Null
  {
    [Template("null")]
    public static readonly Null Value;

    private extern Null();

    [Template("null")]
    public static extern T As<T>();

    public static extern implicit operator Null(Undefined u);

    public static extern implicit operator Null(Never n);

    public static extern implicit operator byte(Null n);

    public static extern implicit operator sbyte(Null n);

    public static extern implicit operator short(Null n);

    public static extern implicit operator ushort(Null n);

    public static extern implicit operator int(Null n);

    public static extern implicit operator uint(Null n);

    public static extern implicit operator long(Null n);

    public static extern implicit operator ulong(Null n);

    public static extern implicit operator float(Null n);

    public static extern implicit operator double(Null n);

    public static extern implicit operator Decimal(Null n);

    public static extern implicit operator bool(Null n);

    public static extern implicit operator char(Null n);

    public static extern implicit operator string(Null n);

    public static extern implicit operator DateTime(Null n);
  }
}
