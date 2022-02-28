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
        [Virtual]
        [FormerInterface]
        public abstract class Float32ArrayConstructor : IObject
        {
            public abstract es5.Float32Array prototype { get; }

            [Template("new {this}({0})")]
            public abstract es5.Float32Array New(uint length);

            [Template("new {this}({0})")]
            public abstract es5.Float32Array New(
              Union<es5.ArrayLike<float>, es5.ArrayBufferLike> arrayOrArrayBuffer);

            [Template("new {this}({0})")]
            public abstract es5.Float32Array New(es5.ArrayLike<float> arrayOrArrayBuffer);

            [Template("new {this}({0})")]
            public abstract es5.Float32Array New(es5.ArrayBufferLike arrayOrArrayBuffer);

            [Template("new {this}({0})")]
            public abstract es5.Float32Array New(es5.ArrayBuffer arrayOrArrayBuffer);

            [Template("new {this}({0}, {1})")]
            public abstract es5.Float32Array New(es5.ArrayBufferLike buffer, uint byteOffset);

            [Template("new {this}({0}, {1})")]
            public abstract es5.Float32Array New(es5.ArrayBuffer buffer, uint byteOffset);

            [Template("new {this}({0}, {1}, {2})")]
            public abstract es5.Float32Array New(
              es5.ArrayBufferLike buffer,
              uint byteOffset,
              uint length);

            [Template("new {this}({0}, {1}, {2})")]
            public abstract es5.Float32Array New(es5.ArrayBuffer buffer, uint byteOffset, uint length);

            public abstract double BYTES_PER_ELEMENT { get; }

            [ExpandParams]
            public abstract es5.Float32Array of(params float[] items);

            public abstract es5.Float32Array from(es5.ArrayLike<float> arrayLike);

            public abstract es5.Float32Array from(
              es5.ArrayLike<float> arrayLike,
              es5.Float32ArrayConstructor.fromFn mapfn);

            public abstract es5.Float32Array from(
              es5.ArrayLike<float> arrayLike,
              es5.Float32ArrayConstructor.fromFn mapfn,
              object thisArg);

            [Generated]
            public delegate double fromFn(double v, double k);
        }
    }
}
