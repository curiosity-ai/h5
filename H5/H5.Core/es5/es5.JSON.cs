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
        [StaticInterface("JSON.Interface")]
        public static class JSON
        {
            public static extern object parse(string text);

            public static extern object parse(string text, es5.JSON.parseFn reviver);

            public static extern string stringify(object value);

            public static extern string stringify(object value, es5.JSON.stringifyFn replacer);

            public static extern string stringify(
              object value,
              es5.JSON.stringifyFn replacer,
              Union<string, double> space);

            public static extern string stringify(
              object value,
              es5.JSON.stringifyFn replacer,
              string space);

            public static extern string stringify(
              object value,
              es5.JSON.stringifyFn replacer,
              double space);

            public static extern string stringify(object value, Union<double, string>[] replacer);

            public static extern string stringify(object value, double[] replacer);

            public static extern string stringify(object value, string[] replacer);

            public static extern string stringify(
              object value,
              Union<double, string>[] replacer,
              Union<string, double> space);

            public static extern string stringify(object value, double[] replacer, string space);

            public static extern string stringify(object value, double[] replacer, double space);

            public static extern string stringify(object value, string[] replacer, string space);

            public static extern string stringify(object value, string[] replacer, double space);

            [Generated]
            [IgnoreCast]
            [ClassInterface]
            [Name("JSON")]
            public interface Interface : IObject
            {
                object parse(string text);

                object parse(string text, es5.JSON.parseFn reviver);

                string stringify(object value);

                string stringify(object value, es5.JSON.stringifyFn replacer);

                string stringify(object value, es5.JSON.stringifyFn replacer, Union<string, double> space);

                string stringify(object value, es5.JSON.stringifyFn replacer, string space);

                string stringify(object value, es5.JSON.stringifyFn replacer, double space);

                string stringify(object value, Union<double, string>[] replacer);

                string stringify(object value, double[] replacer);

                string stringify(object value, string[] replacer);

                string stringify(
                  object value,
                  Union<double, string>[] replacer,
                  Union<string, double> space);

                string stringify(object value, double[] replacer, string space);

                string stringify(object value, double[] replacer, double space);

                string stringify(object value, string[] replacer, string space);

                string stringify(object value, string[] replacer, double space);
            }

            [Generated]
            public delegate object parseFn(object key, object value);

            [Generated]
            public delegate object stringifyFn(string key, object value);
        }
    }
}
