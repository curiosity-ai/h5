using H5;
using H5.Core;

namespace H5.Core
{
    public static partial class dom
    {
        [CombinedClass]
        [FormerInterface]
        public class MediaCapabilities : IObject
        {
            public static dom.MediaCapabilities prototype { get; set; }

            public virtual extern es5.Promise<dom.MediaCapabilitiesDecodingInfo> decodingInfo(dom.MediaDecodingConfiguration configuration);
            public virtual extern es5.Promise<dom.MediaCapabilitiesEncodingInfo> encodingInfo(dom.MediaEncodingConfiguration configuration);
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class MediaConfiguration : IObject
        {
            public dom.VideoConfiguration video { get; set; }
            public dom.AudioConfiguration audio { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class MediaDecodingConfiguration : dom.MediaConfiguration
        {
            public dom.MediaDecodingType type { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class MediaEncodingConfiguration : dom.MediaConfiguration
        {
            public dom.MediaEncodingType type { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class VideoConfiguration : IObject
        {
            public string contentType { get; set; }
            public uint width { get; set; }
            public uint height { get; set; }
            public uint? bitrate { get; set; }
            public double? framerate { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class AudioConfiguration : IObject
        {
            public string contentType { get; set; }
            public string channels { get; set; }
            public uint? bitrate { get; set; }
            public uint? samplerate { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class MediaCapabilitiesInfo : IObject
        {
            public bool supported { get; set; }
            public bool smooth { get; set; }
            public bool powerEfficient { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class MediaCapabilitiesDecodingInfo : dom.MediaCapabilitiesInfo
        {
            public dom.KeySystemAccess keySystemAccess { get; set; }
            public dom.MediaConfiguration configuration { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class MediaCapabilitiesEncodingInfo : dom.MediaCapabilitiesInfo
        {
            public dom.MediaConfiguration configuration { get; set; }
        }

        [CombinedClass]
        [FormerInterface]
        public class KeySystemAccess : IObject
        {
             // Placeholder for EME KeySystemAccess if not already defined
             public virtual string keySystem { get; }
             public virtual extern dom.MediaKeySystemConfiguration getConfiguration();
             public virtual extern es5.Promise<dom.MediaKeys> createMediaKeys();
        }

        [Name("System.String")]
        public class MediaDecodingType : LiteralType<string>
        {
            [Template("<self>\"file\"")] public static readonly dom.MediaDecodingType file;
            [Template("<self>\"media-source\"")] public static readonly dom.MediaDecodingType mediaSource;
            [Template("<self>\"webrtc\"")] public static readonly dom.MediaDecodingType webrtc;
            private extern MediaDecodingType();
            public static extern implicit operator dom.MediaDecodingType(string value);
        }

        [Name("System.String")]
        public class MediaEncodingType : LiteralType<string>
        {
            [Template("<self>\"record\"")] public static readonly dom.MediaEncodingType record;
            [Template("<self>\"transmission\"")] public static readonly dom.MediaEncodingType transmission;
            [Template("<self>\"webrtc\"")] public static readonly dom.MediaEncodingType webrtc;
            private extern MediaEncodingType();
            public static extern implicit operator dom.MediaEncodingType(string value);
        }

        [Virtual]
        public abstract class MediaCapabilitiesTypeConfig : IObject
        {
             public virtual dom.MediaCapabilities prototype { get; set; }
        }
    }
}
