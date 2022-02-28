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
        public interface RegExpConstructor : IObject
        {
            [Template("new {this}({0})")]
            es5.RegExp New(Union<es5.RegExp, string> pattern);

            [Template("new {this}({0})")]
            es5.RegExp New(es5.RegExp pattern);

            [Template("new {this}({0})")]
            es5.RegExp New(string pattern);

            [Template("new {this}({0}, {1})")]
            es5.RegExp New(string pattern, string flags);

            [Template("{this}({0})")]
            es5.RegExp Self(Union<es5.RegExp, string> pattern);

            [Template("{this}({0})")]
            es5.RegExp Self(es5.RegExp pattern);

            [Template("{this}({0})")]
            es5.RegExp Self(string pattern);

            [Template("{this}({0}, {1})")]
            es5.RegExp Self(string pattern, string flags);

            es5.RegExp prototype { get; }

            [Name("$1")]
            string Dollar1 { get; set; }

            [Name("$2")]
            string Dollar2 { get; set; }

            [Name("$3")]
            string Dollar3 { get; set; }

            [Name("$4")]
            string Dollar4 { get; set; }

            [Name("$5")]
            string Dollar5 { get; set; }

            [Name("$6")]
            string Dollar6 { get; set; }

            [Name("$7")]
            string Dollar7 { get; set; }

            [Name("$8")]
            string Dollar8 { get; set; }

            [Name("$9")]
            string Dollar9 { get; set; }

            string lastMatch { get; set; }
        }
    }
}
