// Decompiled with JetBrains decompiler
// Type: Retyped.TypeAlias`1
// Assembly: Retyped.Core, Version=1.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E855DC6-9E83-4420-9E6F-8D2B7A117BBD
// Assembly location: C:\work\curiosity\tesserae\Tesserae\bin\Debug\net461\Retyped.Core.dll

using Bridge;

namespace Retyped
{
  [IgnoreCast]
  [IgnoreGeneric(AllowInTypeScript = true)]
  [Virtual]
  public abstract class TypeAlias<TOriginal>
  {
    [Template("{this}")]
    public readonly TOriginal Value;

    public static extern implicit operator TOriginal(TypeAlias<TOriginal> value);
  }
}
