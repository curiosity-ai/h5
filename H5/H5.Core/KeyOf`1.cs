// Decompiled with JetBrains decompiler
// Type: H5.KeyOf`1
// Assembly: H5.Core, Version=1.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E855DC6-9E83-4420-9E6F-8D2B7A117BBD
// Assembly location: C:\work\curiosity\tesserae\Tesserae\bin\Debug\net461\HTML.dll

using H5;

namespace HTML
{
    [IgnoreGeneric(AllowInTypeScript = true)]
    [IgnoreCast]
    [H5.Name("String")]
    [ExportedAs("KeyOf")]
    public class KeyOf<T>
    {
        [Template("{this}")]
        public static readonly string Name;

        private extern KeyOf();
    }
}
