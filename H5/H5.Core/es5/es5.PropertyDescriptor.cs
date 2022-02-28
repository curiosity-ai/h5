// Decompiled with JetBrains decompiler
// Type: H5.es5
// Assembly: H5.es5, Version=2.8.2.0, Culture=neutral, PublicKeyToken=null
// MVID: EC57AC2B-0E02-4A1C-B567-F790F377783B
// Assembly location: C:\work\curiosity\tesserae\Tesserae\bin\Debug\net461\H5.es5.dll

using System;

namespace H5.Core
{
    public static partial class es5
    {
        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class PropertyDescriptor : IObject
        {
            public bool? configurable
            {
                get; set;
            }

            public bool? enumerable
            {
                get; set;
            }

            public object value
            {
                get; set;
            }

            public bool? writable
            {
                get; set;
            }

            public Func<object> get
            {
                get; set;
            }

            public es5.PropertyDescriptor.setFn set
            {
                get; set;
            }

            [Generated]
            public delegate void setFn(object v);
        }
    }
}
