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
        [StaticInterface("DateConstructor")]
        [FormerInterface]
        public class Date : IObject
        {
            public extern Date();

            public extern Date(double value);

            public extern Date(string value);

            public extern Date(double year, double month);

            public extern Date(double year, double month, double date);

            public extern Date(double year, double month, double date, double hours);

            public extern Date(double year, double month, double date, double hours, double minutes);

            public extern Date(
              double year,
              double month,
              double date,
              double hours,
              double minutes,
              double seconds);

            public extern Date(
              double year,
              double month,
              double date,
              double hours,
              double minutes,
              double seconds,
              double ms);

            public static es5.Date prototype
            {
                get;
            }

            public static extern string Self();

            public static extern double parse(string s);

            public static extern double UTC(double year, double month);

            public static extern double UTC(double year, double month, double date);

            public static extern double UTC(double year, double month, double date, double hours);

            public static extern double UTC(
              double year,
              double month,
              double date,
              double hours,
              double minutes);

            public static extern double UTC(
              double year,
              double month,
              double date,
              double hours,
              double minutes,
              double seconds);

            public static extern double UTC(
              double year,
              double month,
              double date,
              double hours,
              double minutes,
              double seconds,
              double ms);

            public static extern double now();

            public virtual extern string toString();

            public virtual extern string toDateString();

            public virtual extern string toTimeString();

            public virtual extern string toLocaleString();

            public virtual extern string toLocaleDateString();

            public virtual extern string toLocaleTimeString();

            public virtual extern double valueOf();

            public virtual extern double getTime();

            public virtual extern double getFullYear();

            public virtual extern double getUTCFullYear();

            public virtual extern double getMonth();

            public virtual extern double getUTCMonth();

            public virtual extern double getDate();

            public virtual extern double getUTCDate();

            public virtual extern double getDay();

            public virtual extern double getUTCDay();

            public virtual extern double getHours();

            public virtual extern double getUTCHours();

            public virtual extern double getMinutes();

            public virtual extern double getUTCMinutes();

            public virtual extern double getSeconds();

            public virtual extern double getUTCSeconds();

            public virtual extern double getMilliseconds();

            public virtual extern double getUTCMilliseconds();

            public virtual extern double getTimezoneOffset();

            public virtual extern double setTime(double time);

            public virtual extern double setMilliseconds(double ms);

            public virtual extern double setUTCMilliseconds(double ms);

            public virtual extern double setSeconds(double sec);

            public virtual extern double setSeconds(double sec, double ms);

            public virtual extern double setUTCSeconds(double sec);

            public virtual extern double setUTCSeconds(double sec, double ms);

            public virtual extern double setMinutes(double min);

            public virtual extern double setMinutes(double min, double sec);

            public virtual extern double setMinutes(double min, double sec, double ms);

            public virtual extern double setUTCMinutes(double min);

            public virtual extern double setUTCMinutes(double min, double sec);

            public virtual extern double setUTCMinutes(double min, double sec, double ms);

            public virtual extern double setHours(double hours);

            public virtual extern double setHours(double hours, double min);

            public virtual extern double setHours(double hours, double min, double sec);

            public virtual extern double setHours(double hours, double min, double sec, double ms);

            public virtual extern double setUTCHours(double hours);

            public virtual extern double setUTCHours(double hours, double min);

            public virtual extern double setUTCHours(double hours, double min, double sec);

            public virtual extern double setUTCHours(double hours, double min, double sec, double ms);

            public virtual extern double setDate(double date);

            public virtual extern double setUTCDate(double date);

            public virtual extern double setMonth(double month);

            public virtual extern double setMonth(double month, double date);

            public virtual extern double setUTCMonth(double month);

            public virtual extern double setUTCMonth(double month, double date);

            public virtual extern double setFullYear(double year);

            public virtual extern double setFullYear(double year, double month);

            public virtual extern double setFullYear(double year, double month, double date);

            public virtual extern double setUTCFullYear(double year);

            public virtual extern double setUTCFullYear(double year, double month);

            public virtual extern double setUTCFullYear(double year, double month, double date);

            public virtual extern string toUTCString();

            public virtual extern string toISOString();

            public virtual extern string toJSON();

            public virtual extern string toJSON(object key);

            public virtual extern string toLocaleString(Union<string, string[]> locales);

            public virtual extern string toLocaleString(string locales);

            public virtual extern string toLocaleString(string[] locales);

            public virtual extern string toLocaleString(
              Union<string, string[]> locales,
              es5.Intl.DateTimeFormatOptions options);

            public virtual extern string toLocaleString(
              string locales,
              es5.Intl.DateTimeFormatOptions options);

            public virtual extern string toLocaleString(
              string[] locales,
              es5.Intl.DateTimeFormatOptions options);

            public virtual extern string toLocaleDateString(Union<string, string[]> locales);

            public virtual extern string toLocaleDateString(string locales);

            public virtual extern string toLocaleDateString(string[] locales);

            public virtual extern string toLocaleDateString(
              Union<string, string[]> locales,
              es5.Intl.DateTimeFormatOptions options);

            public virtual extern string toLocaleDateString(
              string locales,
              es5.Intl.DateTimeFormatOptions options);

            public virtual extern string toLocaleDateString(
              string[] locales,
              es5.Intl.DateTimeFormatOptions options);

            public virtual extern string toLocaleTimeString(Union<string, string[]> locales);

            public virtual extern string toLocaleTimeString(string locales);

            public virtual extern string toLocaleTimeString(string[] locales);

            public virtual extern string toLocaleTimeString(
              Union<string, string[]> locales,
              es5.Intl.DateTimeFormatOptions options);

            public virtual extern string toLocaleTimeString(
              string locales,
              es5.Intl.DateTimeFormatOptions options);

            public virtual extern string toLocaleTimeString(
              string[] locales,
              es5.Intl.DateTimeFormatOptions options);
        }
    }
}
