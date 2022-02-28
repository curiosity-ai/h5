// Decompiled with JetBrains decompiler
// Type: H5.es5
// Assembly: H5.es5, Version=2.8.2.0, Culture=neutral, PublicKeyToken=null
// MVID: EC57AC2B-0E02-4A1C-B567-F790F377783B
// Assembly location: C:\work\curiosity\tesserae\Tesserae\bin\Debug\net461\H5.es5.dll

using System;

namespace H5.Core
{
    public  static partial class es5
    {
        [IgnoreCast]
        [IgnoreGeneric(AllowInTypeScript = true)]
        [ObjectLiteral]
        [FormerInterface]
        public class TypedPropertyDescriptor<T> : IObject
        {
            public bool? enumerable
            {
                get; set;
            }

            public bool? configurable
            {
                get; set;
            }

            public bool? writable
            {
                get; set;
            }

            public T value
            {
                get; set;
            }

            public Func<T> get
            {
                get; set;
            }

            public es5.TypedPropertyDescriptor<T>.setFn set
            {
                get; set;
            }

            [Generated]
            public delegate void setFn(T value);
        }
    }
}
