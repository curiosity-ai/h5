// Decompiled with JetBrains decompiler
// Type: Retyped.KeyOf`1
// Assembly: Retyped.Core, Version=1.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E855DC6-9E83-4420-9E6F-8D2B7A117BBD
// Assembly location: C:\work\curiosity\tesserae\Tesserae\bin\Debug\net461\Retyped.Core.dll

using Bridge;

namespace Retyped
{
  [IgnoreGeneric(AllowInTypeScript = true)]
  [IgnoreCast]
  [Bridge.Name("String")]
  [ExportedAs("KeyOf")]
  public class KeyOf<T>
  {
    [Template("{this}")]
    public static readonly string Name;

    private extern KeyOf();
  }
}
