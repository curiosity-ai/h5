// Decompiled with JetBrains decompiler
// Type: H5.LiteralType`1
// Assembly: H5.Core, Version=1.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E855DC6-9E83-4420-9E6F-8D2B7A117BBD
// Assembly location: C:\work\curiosity\tesserae\Tesserae\bin\Debug\net461\H5.Core.dll

using HighFive;

namespace H5.Core
{
  [IgnoreCast]
  [IgnoreGeneric(AllowInTypeScript = true)]
  [Virtual]
  public abstract class LiteralType<T> : TypeAlias<T>
  {
    [Template("{arg1} === {arg2}")]
    public static extern bool operator ==(LiteralType<T> arg1, LiteralType<T> arg2);

    [Template("{arg1} !== {arg2}")]
    public static extern bool operator !=(LiteralType<T> arg1, LiteralType<T> arg2);

    [Template("{arg1} === {arg2}")]
    public static extern bool operator ==(LiteralType<T> arg1, T arg2);

    [Template("{arg1} !== {arg2}")]
    public static extern bool operator !=(LiteralType<T> arg1, T arg2);

    [Template("{arg1} === {arg2}")]
    public static extern bool operator ==(T arg1, LiteralType<T> arg2);

    [Template("{arg1} !== {arg2}")]
    public static extern bool operator !=(T arg1, LiteralType<T> arg2);
  }
}
