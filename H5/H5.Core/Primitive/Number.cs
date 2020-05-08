// Decompiled with JetBrains decompiler
// Type: H5.Core.Number
// Assembly: H5.es5, Version=2.8.2.0, Culture=neutral, PublicKeyToken=null
// MVID: EC57AC2B-0E02-4A1C-B567-F790F377783B
// Assembly location: C:\work\curiosity\tesserae\Tesserae\bin\Debug\net461\H5.es5.dll

using HighFive;

namespace H5.Core
{
    [CombinedClass]
    [StaticInterface("NumberConstructor")]
    [Namespace(false)]
    [Virtual]
    public class Number : Object, Number.Interface, IObject
    {
        public extern Number();

        public extern Number(object value);

        public static Number prototype
        {
            get;
        }

        public static double MAX_VALUE
        {
            get;
        }

        public static double MIN_VALUE
        {
            get;
        }

        public static double NaN
        {
            get;
        }

        public static double NEGATIVE_INFINITY
        {
            get;
        }

        public static double POSITIVE_INFINITY
        {
            get;
        }

        public static extern double Self();

        public static extern double Self(object value);

        public new virtual extern string toString();

        public virtual extern string toString(double radix);

        public virtual extern string toFixed();

        public virtual extern string toFixed(double fractionDigits);

        public virtual extern string toExponential();

        public virtual extern string toExponential(double fractionDigits);

        public virtual extern string toPrecision();

        public virtual extern string toPrecision(double precision);

        public virtual extern double valueOf();

        public new virtual extern string toLocaleString();

        public virtual extern string toLocaleString(Union<string, string[]> locales);

        public virtual extern string toLocaleString(string locales);

        public virtual extern string toLocaleString(string[] locales);

        public virtual extern string toLocaleString(
          Union<string, string[]> locales,
          es5.Intl.NumberFormatOptions options);

        public virtual extern string toLocaleString(
          string locales,
          es5.Intl.NumberFormatOptions options);

        public virtual extern string toLocaleString(
          string[] locales,
          es5.Intl.NumberFormatOptions options);

        public static extern implicit operator Number(double value);

        [Template("{this} != null ? {this}.valueOf() : {this}")]
        public static extern implicit operator double(Number value);

        [Generated]
        [IgnoreCast]
        [ClassInterface]
        [Name("Number")]
        public new interface Interface : IObject
        {
            string toString();

            string toString(double radix);

            string toFixed();

            string toFixed(double fractionDigits);

            string toExponential();

            string toExponential(double fractionDigits);

            string toPrecision();

            string toPrecision(double precision);

            double valueOf();

            string toLocaleString();

            string toLocaleString(Union<string, string[]> locales);

            string toLocaleString(string locales);

            string toLocaleString(string[] locales);

            string toLocaleString(Union<string, string[]> locales, es5.Intl.NumberFormatOptions options);

            string toLocaleString(string locales, es5.Intl.NumberFormatOptions options);

            string toLocaleString(string[] locales, es5.Intl.NumberFormatOptions options);
        }
    }
}
