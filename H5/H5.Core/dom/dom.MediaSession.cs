using H5;
using H5.Core;

namespace H5.Core
{
    public static partial class dom
    {
        [CombinedClass]
        [FormerInterface]
        public class MediaSession : IObject
        {
            public static dom.MediaSession prototype { get; set; }

            public virtual dom.MediaMetadata metadata { get; set; }
            public virtual dom.MediaSessionPlaybackState playbackState { get; set; }

            public virtual extern void setActionHandler(dom.MediaSessionAction action, dom.MediaSessionActionHandler handler);
            public virtual extern void setPositionState(dom.MediaPositionState state);
        }

        [CombinedClass]
        [FormerInterface]
        public class MediaMetadata : IObject
        {
            public extern MediaMetadata(dom.MediaMetadataInit init);
            public static dom.MediaMetadata prototype { get; set; }

            public virtual string title { get; set; }
            public virtual string artist { get; set; }
            public virtual string album { get; set; }
            public virtual dom.MediaImage[] artwork { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class MediaMetadataInit : IObject
        {
            public string title { get; set; }
            public string artist { get; set; }
            public string album { get; set; }
            public dom.MediaImage[] artwork { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class MediaImage : IObject
        {
            public string src { get; set; }
            public string sizes { get; set; }
            public string type { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class MediaPositionState : IObject
        {
            public double? duration { get; set; }
            public double? playbackRate { get; set; }
            public double? position { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class MediaSessionActionDetails : IObject
        {
            public dom.MediaSessionAction action { get; set; }
            public double? seekOffset { get; set; }
            public double? seekTime { get; set; }
            public bool? fastSeek { get; set; }
        }

        [Generated]
        public delegate void MediaSessionActionHandler(dom.MediaSessionActionDetails details);

        [Name("System.String")]
        public class MediaSessionPlaybackState : LiteralType<string>
        {
            [Template("<self>\"none\"")] public static readonly dom.MediaSessionPlaybackState none;
            [Template("<self>\"paused\"")] public static readonly dom.MediaSessionPlaybackState paused;
            [Template("<self>\"playing\"")] public static readonly dom.MediaSessionPlaybackState playing;
            private extern MediaSessionPlaybackState();
            public static extern implicit operator dom.MediaSessionPlaybackState(string value);
        }

        [Name("System.String")]
        public class MediaSessionAction : LiteralType<string>
        {
            [Template("<self>\"play\"")] public static readonly dom.MediaSessionAction play;
            [Template("<self>\"pause\"")] public static readonly dom.MediaSessionAction pause;
            [Template("<self>\"seekbackward\"")] public static readonly dom.MediaSessionAction seekbackward;
            [Template("<self>\"seekforward\"")] public static readonly dom.MediaSessionAction seekforward;
            [Template("<self>\"previoustrack\"")] public static readonly dom.MediaSessionAction previoustrack;
            [Template("<self>\"nexttrack\"")] public static readonly dom.MediaSessionAction nexttrack;
            [Template("<self>\"skipad\"")] public static readonly dom.MediaSessionAction skipad;
            [Template("<self>\"stop\"")] public static readonly dom.MediaSessionAction stop;
            [Template("<self>\"seekto\"")] public static readonly dom.MediaSessionAction seekto;
            [Template("<self>\"togglemicrophone\"")] public static readonly dom.MediaSessionAction togglemicrophone;
            [Template("<self>\"togglecamera\"")] public static readonly dom.MediaSessionAction togglecamera;
            [Template("<self>\"hangup\"")] public static readonly dom.MediaSessionAction hangup;
            private extern MediaSessionAction();
            public static extern implicit operator dom.MediaSessionAction(string value);
        }

        [Virtual]
        public abstract class MediaSessionTypeConfig : IObject
        {
             public virtual dom.MediaSession prototype { get; set; }
        }

        [Virtual]
        public abstract class MediaMetadataTypeConfig : IObject
        {
             public virtual dom.MediaMetadata prototype { get; set; }
             [Template("new {this}({0})")]
             public abstract dom.MediaMetadata New(dom.MediaMetadataInit init);
        }
    }
}
