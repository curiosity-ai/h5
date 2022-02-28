// Decompiled with JetBrains decompiler
// Type: H5.es5
// Assembly: H5.es5, Version=2.8.2.0, Culture=neutral, PublicKeyToken=null
// MVID: EC57AC2B-0E02-4A1C-B567-F790F377783B
// Assembly location: C:\work\curiosity\tesserae\Tesserae\bin\Debug\net461\H5.es5.dll

namespace H5.Core
{
    public  static partial class es5
    {
        [IgnoreGeneric(AllowInTypeScript = true)]
        [IgnoreCast]
        [Virtual]
        public abstract class NonNullable<T> : TypeAlias<Union<Never, T>>
        {
            public static extern implicit operator es5.NonNullable<T>(Union<Never, T> value);

            [Template("{0}")]
            public static extern es5.NonNullable<T> Create(Never value);

            [Template("{0}")]
            public static extern es5.NonNullable<T> Create(T value);

            public static extern implicit operator es5.NonNullable<T>(Never value);

            public static extern implicit operator es5.NonNullable<T>(T value);

            public static extern explicit operator Never(es5.NonNullable<T> value);

            public static extern explicit operator T(es5.NonNullable<T> value);
        }
    }
}
