// Decompiled with JetBrains decompiler
// Type: HTML.This`1
// Assembly: H5.Core, Version=1.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E855DC6-9E83-4420-9E6F-8D2B7A117BBD
// Assembly location: C:\work\curiosity\tesserae\Tesserae\bin\Debug\net461\HTML.dll

using H5;

namespace HTML
{
  [IgnoreGeneric(AllowInTypeScript = true)]
  [IgnoreCast]
  [Virtual]
  public abstract class This<T>
  {
    private extern This();

    public T Value
    {
      [Template("{this}")] get;
    }

    [Template("{this}")]
    public extern TSuccessor As<TSuccessor>() where TSuccessor : T;

    public static extern implicit operator T(This<T> th);

    public static extern implicit operator This<T>(T value);
  }
}
