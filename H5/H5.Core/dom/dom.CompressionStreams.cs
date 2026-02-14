using H5;
using H5.Core;

namespace H5.Core
{
    public static partial class dom
    {
        [CombinedClass]
        [FormerInterface]
        public class CompressionStream : IObject
        {
            public extern CompressionStream(dom.CompressionFormat format);

            public static dom.CompressionStream prototype { get; set; }

            public virtual dom.ReadableStream readable { get; }

            public virtual dom.WritableStream writable { get; }
        }

        [CombinedClass]
        [FormerInterface]
        public class DecompressionStream : IObject
        {
            public extern DecompressionStream(dom.CompressionFormat format);

            public static dom.DecompressionStream prototype { get; set; }

            public virtual dom.ReadableStream readable { get; }

            public virtual dom.WritableStream writable { get; }
        }

        [Name("System.String")]
        public class CompressionFormat : LiteralType<string>
        {
            [Template("<self>\"gzip\"")]
            public static readonly dom.CompressionFormat gzip;

            [Template("<self>\"deflate\"")]
            public static readonly dom.CompressionFormat deflate;

            [Template("<self>\"deflate-raw\"")]
            public static readonly dom.CompressionFormat deflateRaw;

            private extern CompressionFormat();

            public static extern implicit operator dom.CompressionFormat(string value);
        }

        [Virtual]
        public abstract class CompressionStreamTypeConfig : IObject
        {
             public virtual dom.CompressionStream prototype { get; set; }

             [Template("new {this}({0})")]
             public abstract dom.CompressionStream New(dom.CompressionFormat format);
        }

        [Virtual]
        public abstract class DecompressionStreamTypeConfig : IObject
        {
             public virtual dom.DecompressionStream prototype { get; set; }

             [Template("new {this}({0})")]
             public abstract dom.DecompressionStream New(dom.CompressionFormat format);
        }
    }
}
