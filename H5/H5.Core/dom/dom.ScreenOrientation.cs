using H5;
using H5.Core;

namespace H5.Core
{
    public static partial class dom
    {
        [CombinedClass]
        [FormerInterface]
        public class ScreenOrientation : dom.EventTarget
        {
            public static dom.ScreenOrientation prototype { get; set; }

            public virtual double angle { get; }

            public virtual string type { get; }

            public virtual dom.ScreenOrientation.onchangeFn onchange { get; set; }

            public virtual extern es5.Promise<H5.Core.Void> @lock(dom.ScreenOrientationLockType orientation);

            public virtual extern void unlock();

            [Generated]
            public delegate void onchangeFn(dom.Event ev);
        }

        [Name("System.String")]
        public class ScreenOrientationLockType : LiteralType<string>
        {
            [Template("<self>\"any\"")]
            public static readonly dom.ScreenOrientationLockType any;

            [Template("<self>\"natural\"")]
            public static readonly dom.ScreenOrientationLockType natural;

            [Template("<self>\"landscape\"")]
            public static readonly dom.ScreenOrientationLockType landscape;

            [Template("<self>\"portrait\"")]
            public static readonly dom.ScreenOrientationLockType portrait;

            [Template("<self>\"portrait-primary\"")]
            public static readonly dom.ScreenOrientationLockType portraitPrimary;

            [Template("<self>\"portrait-secondary\"")]
            public static readonly dom.ScreenOrientationLockType portraitSecondary;

            [Template("<self>\"landscape-primary\"")]
            public static readonly dom.ScreenOrientationLockType landscapePrimary;

            [Template("<self>\"landscape-secondary\"")]
            public static readonly dom.ScreenOrientationLockType landscapeSecondary;

            private extern ScreenOrientationLockType();

            public static extern implicit operator dom.ScreenOrientationLockType(string value);
        }

        [Name("System.String")]
        public class ScreenOrientationType : LiteralType<string>
        {
            [Template("<self>\"portrait-primary\"")]
            public static readonly dom.ScreenOrientationType portraitPrimary;

            [Template("<self>\"portrait-secondary\"")]
            public static readonly dom.ScreenOrientationType portraitSecondary;

            [Template("<self>\"landscape-primary\"")]
            public static readonly dom.ScreenOrientationType landscapePrimary;

            [Template("<self>\"landscape-secondary\"")]
            public static readonly dom.ScreenOrientationType landscapeSecondary;

            private extern ScreenOrientationType();

            public static extern implicit operator dom.ScreenOrientationType(string value);
        }
    }
}
