using H5;
using H5.Core;
using System;

namespace H5.Core
{
    public static partial class dom
    {
        [CombinedClass]
        [FormerInterface]
        public class FontFaceSet : dom.EventTarget
        {
            public virtual extern es5.Promise<dom.FontFace[]> load(string font, string text);
            public virtual extern bool check(string font, string text);
            public virtual extern void add(object font);
            public virtual extern void delete(object font);
            public virtual extern void clear();
            public virtual dom.FontFaceSet.onloadingFn onloading { get; set; }
            public virtual dom.FontFaceSet.onloadingdoneFn onloadingdone { get; set; }
            public virtual dom.FontFaceSet.onloadingerrorFn onloadingerror { get; set; }
            public virtual string status { get; }
            public virtual es5.Promise<dom.FontFaceSet> ready { get; }

            [Generated]
            public delegate void onloadingFn(dom.Event ev);
            [Generated]
            public delegate void onloadingdoneFn(dom.Event ev);
            [Generated]
            public delegate void onloadingerrorFn(dom.Event ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class FontFace : IObject
        {
            public extern FontFace(string family, string source);
            public extern FontFace(string family, string source, object descriptors);
            public virtual string family { get; set; }
            public virtual string style { get; set; }
            public virtual string weight { get; set; }
            public virtual string stretch { get; set; }
            public virtual string unicodeRange { get; set; }
            public virtual string variant { get; set; }
            public virtual string featureSettings { get; set; }
            public virtual string display { get; set; }
            public virtual string status { get; }
            public virtual extern es5.Promise<dom.FontFace> load();
        }

        public partial class Document
        {
            public virtual dom.Element pictureInPictureElement
            {
                get;
            }

            public virtual bool pictureInPictureEnabled
            {
                get;
            }

            public virtual extern es5.Promise<H5.Core.Void> exitPictureInPicture();

            public virtual dom.FontFaceSet fonts
            {
                get;
            }
        }
    }
}
