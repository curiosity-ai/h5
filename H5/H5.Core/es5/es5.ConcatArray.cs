// Decompiled with JetBrains decompiler
// Type: H5.es5
// Assembly: H5.es5, Version=2.8.2.0, Culture=neutral, PublicKeyToken=null
// MVID: EC57AC2B-0E02-4A1C-B567-F790F377783B
// Assembly location: C:\work\curiosity\tesserae\Tesserae\bin\Debug\net461\H5.es5.dll

namespace H5.Core
{
    public  static partial class es5
    {
        [IgnoreCast]
        [IgnoreGeneric(AllowInTypeScript = true)]
        [Virtual]
        [FormerInterface]
        public abstract class ConcatArray<T> : IObject
        {
            public abstract double length { get; }

            public abstract T this[double n] { get; }

            public abstract string join();

            public abstract string join(string separator);

            public abstract T[] slice();

            public abstract T[] slice(double start);

            public abstract T[] slice(double start, double end);
        }
    }
}
