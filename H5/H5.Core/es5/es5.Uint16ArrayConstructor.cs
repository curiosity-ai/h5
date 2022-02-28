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
        public abstract class Uint16ArrayConstructor : IObject
        {
            public abstract es5.Uint16Array prototype { get; }

            [Template("new {this}({0})")]
            public abstract es5.Uint16Array New(uint length);

            [Template("new {this}({0})")]
            public abstract es5.Uint16Array New(
              Union<es5.ArrayLike<ushort>, es5.ArrayBufferLike> arrayOrArrayBuffer);

            [Template("new {this}({0})")]
            public abstract es5.Uint16Array New(es5.ArrayLike<ushort> arrayOrArrayBuffer);

            [Template("new {this}({0})")]
            public abstract es5.Uint16Array New(es5.ArrayBufferLike arrayOrArrayBuffer);

            [Template("new {this}({0})")]
            public abstract es5.Uint16Array New(es5.ArrayBuffer arrayOrArrayBuffer);

            [Template("new {this}({0}, {1})")]
            public abstract es5.Uint16Array New(es5.ArrayBufferLike buffer, uint byteOffset);

            [Template("new {this}({0}, {1})")]
            public abstract es5.Uint16Array New(es5.ArrayBuffer buffer, uint byteOffset);

            [Template("new {this}({0}, {1}, {2})")]
            public abstract es5.Uint16Array New(
              es5.ArrayBufferLike buffer,
              uint byteOffset,
              uint length);

            [Template("new {this}({0}, {1}, {2})")]
            public abstract es5.Uint16Array New(es5.ArrayBuffer buffer, uint byteOffset, uint length);

            public abstract double BYTES_PER_ELEMENT { get; }

            [ExpandParams]
            public abstract es5.Uint16Array of(params ushort[] items);

            public abstract es5.Uint16Array from(es5.ArrayLike<ushort> arrayLike);

            public abstract es5.Uint16Array from(
              es5.ArrayLike<ushort> arrayLike,
              es5.Uint16ArrayConstructor.fromFn mapfn);

            public abstract es5.Uint16Array from(
              es5.ArrayLike<ushort> arrayLike,
              es5.Uint16ArrayConstructor.fromFn mapfn,
              object thisArg);

            [Generated]
            public delegate double fromFn(double v, double k);
        }
    }
}
