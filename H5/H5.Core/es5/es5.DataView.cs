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
        [StaticInterface("DataViewConstructor")]
        [FormerInterface]
        public class DataView : IObject
        {
            public extern DataView(es5.ArrayBufferLike buffer);

            public extern DataView(es5.ArrayBuffer buffer);

            public extern DataView(es5.ArrayBufferLike buffer, double byteOffset);

            public extern DataView(es5.ArrayBuffer buffer, double byteOffset);

            public extern DataView(es5.ArrayBufferLike buffer, double byteOffset, double byteLength);

            public extern DataView(es5.ArrayBuffer buffer, double byteOffset, double byteLength);

            public virtual es5.ArrayBuffer buffer
            {
                get;
            }

            public virtual uint byteLength
            {
                get;
            }

            public virtual uint byteOffset
            {
                get;
            }

            public virtual extern float getFloat32(uint byteOffset);

            public virtual extern float getFloat32(uint byteOffset, bool littleEndian);

            public virtual extern double getFloat64(uint byteOffset);

            public virtual extern double getFloat64(uint byteOffset, bool littleEndian);

            public virtual extern sbyte getInt8(uint byteOffset);

            public virtual extern short getInt16(uint byteOffset);

            public virtual extern short getInt16(uint byteOffset, bool littleEndian);

            public virtual extern int getInt32(uint byteOffset);

            public virtual extern int getInt32(uint byteOffset, bool littleEndian);

            public virtual extern byte getUint8(uint byteOffset);

            public virtual extern ushort getUint16(uint byteOffset);

            public virtual extern ushort getUint16(uint byteOffset, bool littleEndian);

            public virtual extern uint getUint32(uint byteOffset);

            public virtual extern uint getUint32(uint byteOffset, bool littleEndian);

            public virtual extern void setFloat32(uint byteOffset, float value);

            public virtual extern void setFloat32(uint byteOffset, float value, bool littleEndian);

            public virtual extern void setFloat64(uint byteOffset, double value);

            public virtual extern void setFloat64(uint byteOffset, double value, bool littleEndian);

            public virtual extern void setInt8(uint byteOffset, sbyte value);

            public virtual extern void setInt16(uint byteOffset, short value);

            public virtual extern void setInt16(uint byteOffset, short value, bool littleEndian);

            public virtual extern void setInt32(uint byteOffset, int value);

            public virtual extern void setInt32(uint byteOffset, int value, bool littleEndian);

            public virtual extern void setUint8(uint byteOffset, byte value);

            public virtual extern void setUint16(uint byteOffset, ushort value);

            public virtual extern void setUint16(uint byteOffset, ushort value, bool littleEndian);

            public virtual extern void setUint32(uint byteOffset, uint value);

            public virtual extern void setUint32(uint byteOffset, uint value, bool littleEndian);
        }
    }
}
