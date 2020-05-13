// Decompiled with JetBrains decompiler
// Type: H5.Core.Undefined
// Assembly: H5.Core, Version=1.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E855DC6-9E83-4420-9E6F-8D2B7A117BBD
// Assembly location: C:\work\curiosity\tesserae\Tesserae\bin\Debug\net461\H5.Core.dll

using H5;
using System;

namespace H5.Core
{
  [IgnoreCast]
  [ExportedAs("undefined")]
  public class Undefined
  {
    [Template("undefined")]
    public static readonly Undefined Value;

    private extern Undefined();

    [Template("undefined")]
    public static extern T As<T>();

    public static extern implicit operator Undefined(Null n);

    public static extern implicit operator Undefined(Never n);

    public static extern implicit operator byte(Undefined u);

    public static extern implicit operator sbyte(Undefined u);

    public static extern implicit operator short(Undefined u);

    public static extern implicit operator ushort(Undefined u);

    public static extern implicit operator int(Undefined u);

    public static extern implicit operator uint(Undefined u);

    public static extern implicit operator long(Undefined u);

    public static extern implicit operator ulong(Undefined u);

    public static extern implicit operator float(Undefined u);

    public static extern implicit operator double(Undefined u);

    public static extern implicit operator Decimal(Undefined u);

    public static extern implicit operator bool(Undefined u);

    public static extern implicit operator char(Undefined u);

    public static extern implicit operator string(Undefined u);

    public static extern implicit operator DateTime(Undefined u);
  }
}
