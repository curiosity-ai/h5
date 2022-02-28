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
        [StaticInterface("RegExpConstructor")]
        [FormerInterface]
        public class RegExp : IObject
        {

            public extern RegExp(Union<es5.RegExp, string> pattern);

            public extern RegExp(es5.RegExp pattern);

            public extern RegExp(string pattern);

            public extern RegExp(string pattern, string flags);

            public static es5.RegExp prototype
            {
                get;
            }

            [Name("$1")]
            public static string Dollar1
            {
                get; set;
            }

            [Name("$2")]
            public static string Dollar2
            {
                get; set;
            }

            [Name("$3")]
            public static string Dollar3
            {
                get; set;
            }

            [Name("$4")]
            public static string Dollar4
            {
                get; set;
            }

            [Name("$5")]
            public static string Dollar5
            {
                get; set;
            }

            [Name("$6")]
            public static string Dollar6
            {
                get; set;
            }

            [Name("$7")]
            public static string Dollar7
            {
                get; set;
            }

            [Name("$8")]
            public static string Dollar8
            {
                get; set;
            }

            [Name("$9")]
            public static string Dollar9
            {
                get; set;
            }

            public static string lastMatch
            {
                get; set;
            }

            public static extern es5.RegExp Self(Union<es5.RegExp, string> pattern);

            public static extern es5.RegExp Self(es5.RegExp pattern);

            public static extern es5.RegExp Self(string pattern);

            public static extern es5.RegExp Self(string pattern, string flags);

            public virtual extern es5.RegExpExecArray exec(string @string);

            public virtual extern bool test(string @string);

            public virtual string source
            {
                get;
            }

            public virtual bool global
            {
                get;
            }

            public virtual bool ignoreCase
            {
                get;
            }

            public virtual bool multiline
            {
                get;
            }

            public virtual double lastIndex
            {
                get; set;
            }

            public virtual extern es5.RegExp compile();
        }
    }
}
