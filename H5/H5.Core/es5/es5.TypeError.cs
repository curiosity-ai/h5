// Decompiled with JetBrains decompiler
// Type: H5.es5
// Assembly: H5.es5, Version=2.8.2.0, Culture=neutral, PublicKeyToken=null
// MVID: EC57AC2B-0E02-4A1C-B567-F790F377783B
// Assembly location: C:\work\curiosity\tesserae\Tesserae\bin\Debug\net461\H5.es5.dll

namespace H5.Core
{
    public  static partial class es5
    {
        [CombinedClass]
        [StaticInterface("TypeErrorConstructor")]
        [FormerInterface]
        public class TypeError : es5.Error
        {
            public extern TypeError();

            public extern TypeError(string message);

            public static es5.TypeError prototype
            {
                get;
            }

            public static extern es5.TypeError Self();

            public static extern es5.TypeError Self(string message);
        }
    }
}
