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
        [StaticInterface("ArrayBufferConstructor")]
        [FormerInterface]
        public class ArrayBuffer : IObject
        {
            public extern ArrayBuffer(double byteLength);

            public static es5.ArrayBuffer prototype
            {
                get;
            }

            public static extern bool isView(object arg);

            public virtual uint byteLength
            {
                get;
            }

            public virtual extern es5.ArrayBuffer slice(int begin);

            public virtual extern es5.ArrayBuffer slice(int begin, int end);
        }
    }
}
