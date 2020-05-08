// Decompiled with JetBrains decompiler
// Type: Retyped.Primitive.Void
// Assembly: Retyped.Core, Version=1.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E855DC6-9E83-4420-9E6F-8D2B7A117BBD
// Assembly location: C:\work\curiosity\tesserae\Tesserae\bin\Debug\net461\Retyped.Core.dll

using Bridge;

namespace Retyped.Primitive
{
  [IgnoreCast]
  [Virtual]
  [ExportedAs("void")]
  public class Void
  {
    private extern Void();

    public static extern implicit operator Void(Undefined u);

    public static extern implicit operator Void(Null n);

    public static extern implicit operator Void(Never n);
  }
}
