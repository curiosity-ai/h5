// Decompiled with JetBrains decompiler
// Type: H5.This
// Assembly: H5.Core, Version=1.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E855DC6-9E83-4420-9E6F-8D2B7A117BBD
// Assembly location: C:\work\curiosity\tesserae\Tesserae\bin\Debug\net461\H5.Core.dll

using HighFive;

namespace H5
{
    [Name("this")]
    [Virtual]
    public static class This
    {
        [Template("this")]
        public static readonly object Instance;

        [Template("{this}[{name}].call(null, {args})")]
        public static extern void Call(string name, params object[] args);

        [Template("{this}[{name}].call(null, {args})")]
        public static extern T Call<T>(string name, params object[] args);

        [Template("{this}[{name}]")]
        public static extern object Get(string name);

        [Template("{this}[{name}]")]
        public static extern T Get<T>(string name);

        [Template("{this}[{name}] = {value}")]
        public static extern void Set(string name, object value);
    }
}
