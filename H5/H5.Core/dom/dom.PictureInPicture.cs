using H5;
using H5.Core;

namespace H5.Core
{
    public static partial class dom
    {
        [CombinedClass]
        [FormerInterface]
        public class PictureInPictureWindow : dom.EventTarget
        {
            public static dom.PictureInPictureWindow prototype { get; set; }

            public virtual double width { get; }
            public virtual double height { get; }

            public virtual dom.PictureInPictureWindow.onresizeFn onresize { get; set; }

            [Generated]
            public delegate void onresizeFn(dom.Event ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class PictureInPictureEvent : dom.Event
        {
            public extern PictureInPictureEvent(string type, dom.PictureInPictureEventInit eventInitDict);
            public static dom.PictureInPictureEvent prototype { get; set; }

            public virtual dom.PictureInPictureWindow pictureInPictureWindow { get; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class PictureInPictureEventInit : dom.EventInit
        {
            public dom.PictureInPictureWindow pictureInPictureWindow { get; set; }
        }

        [Virtual]
        public abstract class PictureInPictureWindowTypeConfig : IObject
        {
             public virtual dom.PictureInPictureWindow prototype { get; set; }
        }
    }
}
