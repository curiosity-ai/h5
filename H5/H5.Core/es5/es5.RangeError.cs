﻿// Decompiled with JetBrains decompiler
// Type: H5.es5
// Assembly: H5.es5, Version=2.8.2.0, Culture=neutral, PublicKeyToken=null
// MVID: EC57AC2B-0E02-4A1C-B567-F790F377783B
// Assembly location: C:\work\curiosity\tesserae\Tesserae\bin\Debug\net461\H5.es5.dll

namespace H5.Core
{
    public  static partial class es5
    {
        [CombinedClass]
        [StaticInterface("RangeErrorConstructor")]
        [FormerInterface]
        public class RangeError : es5.Error
        {
            public extern RangeError();

            public extern RangeError(string message);

            public static es5.RangeError prototype
            {
                get;
            }

            public static extern es5.RangeError Self();

            public static extern es5.RangeError Self(string message);
        }
    }
}
