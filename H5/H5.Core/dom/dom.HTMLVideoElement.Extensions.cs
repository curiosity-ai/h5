using H5;
using H5.Core;
using System;

namespace H5.Core
{
    public static partial class dom
    {
        public partial class HTMLVideoElement
        {
            public virtual extern es5.Promise<dom.PictureInPictureWindow> requestPictureInPicture();

            public virtual bool disablePictureInPicture
            {
                get;
                set;
            }

            public virtual dom.HTMLVideoElement.onenterpictureinpictureFn onenterpictureinpicture
            {
                get;
                set;
            }

            public virtual dom.HTMLVideoElement.onleavepictureinpictureFn onleavepictureinpicture
            {
                get;
                set;
            }

            [Generated]
            public delegate void onenterpictureinpictureFn(dom.PictureInPictureEvent ev);

            [Generated]
            public delegate void onleavepictureinpictureFn(dom.PictureInPictureEvent ev);
        }
    }
}
