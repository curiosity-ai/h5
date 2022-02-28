// Decompiled with JetBrains decompiler
// Type: H5.es5
// Assembly: H5.es5, Version=2.8.2.0, Culture=neutral, PublicKeyToken=null
// MVID: EC57AC2B-0E02-4A1C-B567-F790F377783B
// Assembly location: C:\work\curiosity\tesserae\Tesserae\bin\Debug\net461\H5.es5.dll

namespace H5.Core
{
    public  static partial class es5
    {
        [Scope]
        public static class Intl
        {
            [Name("Collator")]
            public static es5.Intl.CollatorTypeConfig CollatorType
            {
                get;
                set;
            }

            [Name("NumberFormat")]
            public static es5.Intl.NumberFormatTypeConfig NumberFormatType
            {
                get;
                set;
            }

            [Name("DateTimeFormat")]
            public static es5.Intl.DateTimeFormatTypeConfig DateTimeFormatType
            {
                get;
                set;
            }

            [IgnoreCast]
            [ObjectLiteral]
            [FormerInterface]
            public class CollatorOptions : IObject
            {
                public string usage
                {
                    get;
                    set;
                }

                public string localeMatcher
                {
                    get;
                    set;
                }

                public bool? numeric
                {
                    get;
                    set;
                }

                public string caseFirst
                {
                    get;
                    set;
                }

                public string sensitivity
                {
                    get;
                    set;
                }

                public bool? ignorePunctuation
                {
                    get;
                    set;
                }
            }

            [IgnoreCast]
            [ObjectLiteral]
            [FormerInterface]
            public class ResolvedCollatorOptions : IObject
            {

                public string locale
                {
                    get; set;
                }

                public string usage
                {
                    get; set;
                }

                public string sensitivity
                {
                    get; set;
                }

                public bool ignorePunctuation
                {
                    get; set;
                }

                public string collation
                {
                    get; set;
                }

                public string caseFirst
                {
                    get; set;
                }

                public bool numeric
                {
                    get; set;
                }
            }

            [CombinedClass]
            [FormerInterface]
            public abstract class Collator : IObject
            {
                public extern Collator();

                public extern Collator(Union<string, string[]> locales);

                public extern Collator(string locales);

                public extern Collator(string[] locales);

                public extern Collator(Union<string, string[]> locales, es5.Intl.CollatorOptions options);

                public extern Collator(string locales, es5.Intl.CollatorOptions options);

                public extern Collator(string[] locales, es5.Intl.CollatorOptions options);

                public static extern es5.Intl.Collator Self();

                public static extern es5.Intl.Collator Self(Union<string, string[]> locales);

                public static extern es5.Intl.Collator Self(string locales);

                public static extern es5.Intl.Collator Self(string[] locales);

                public static extern es5.Intl.Collator Self(
                  Union<string, string[]> locales,
                  es5.Intl.CollatorOptions options);

                public static extern es5.Intl.Collator Self(
                  string locales,
                  es5.Intl.CollatorOptions options);

                public static extern es5.Intl.Collator Self(
                  string[] locales,
                  es5.Intl.CollatorOptions options);

                public static extern string[] supportedLocalesOf(Union<string, string[]> locales);

                public static extern string[] supportedLocalesOf(string locales);

                public static extern string[] supportedLocalesOf(string[] locales);

                public static extern string[] supportedLocalesOf(
                  Union<string, string[]> locales,
                  es5.Intl.CollatorOptions options);

                public static extern string[] supportedLocalesOf(
                  string locales,
                  es5.Intl.CollatorOptions options);

                public static extern string[] supportedLocalesOf(
                  string[] locales,
                  es5.Intl.CollatorOptions options);

                public virtual extern double compare(string x, string y);

                public virtual extern es5.Intl.ResolvedCollatorOptions resolvedOptions();
            }

            [IgnoreCast]
            [ObjectLiteral]
            [FormerInterface]
            public class NumberFormatOptions : IObject
            {

                public string localeMatcher
                {
                    get; set;
                }

                public string style
                {
                    get; set;
                }

                public string currency
                {
                    get; set;
                }

                public string currencyDisplay
                {
                    get; set;
                }

                public bool? useGrouping
                {
                    get; set;
                }

                public double? minimumIntegerDigits
                {
                    get; set;
                }

                public double? minimumFractionDigits
                {
                    get; set;
                }

                public double? maximumFractionDigits
                {
                    get; set;
                }

                public double? minimumSignificantDigits
                {
                    get; set;
                }

                public double? maximumSignificantDigits
                {
                    get; set;
                }
            }

            [IgnoreCast]
            [ObjectLiteral]
            [FormerInterface]
            public class ResolvedNumberFormatOptions : IObject
            {

                public string locale
                {
                    get; set;
                }

                public string numberingSystem
                {
                    get; set;
                }

                public string style
                {
                    get; set;
                }

                public string currency
                {
                    get; set;
                }

                public string currencyDisplay
                {
                    get; set;
                }

                public double minimumIntegerDigits
                {
                    get; set;
                }

                public double minimumFractionDigits
                {
                    get; set;
                }

                public double maximumFractionDigits
                {
                    get; set;
                }

                public double? minimumSignificantDigits
                {
                    get; set;
                }

                public double? maximumSignificantDigits
                {
                    get; set;
                }

                public bool useGrouping
                {
                    get; set;
                }
            }

            [CombinedClass]
            [FormerInterface]
            public abstract class NumberFormat : IObject
            {
                public extern NumberFormat();

                public extern NumberFormat(Union<string, string[]> locales);

                public extern NumberFormat(string locales);

                public extern NumberFormat(string[] locales);

                public extern NumberFormat(
                  Union<string, string[]> locales,
                  es5.Intl.NumberFormatOptions options);

                public extern NumberFormat(string locales, es5.Intl.NumberFormatOptions options);

                public extern NumberFormat(string[] locales, es5.Intl.NumberFormatOptions options);

                public static extern es5.Intl.NumberFormat Self();

                public static extern es5.Intl.NumberFormat Self(Union<string, string[]> locales);

                public static extern es5.Intl.NumberFormat Self(string locales);

                public static extern es5.Intl.NumberFormat Self(string[] locales);

                public static extern es5.Intl.NumberFormat Self(
                  Union<string, string[]> locales,
                  es5.Intl.NumberFormatOptions options);

                public static extern es5.Intl.NumberFormat Self(
                  string locales,
                  es5.Intl.NumberFormatOptions options);

                public static extern es5.Intl.NumberFormat Self(
                  string[] locales,
                  es5.Intl.NumberFormatOptions options);

                public static extern string[] supportedLocalesOf(Union<string, string[]> locales);

                public static extern string[] supportedLocalesOf(string locales);

                public static extern string[] supportedLocalesOf(string[] locales);

                public static extern string[] supportedLocalesOf(
                  Union<string, string[]> locales,
                  es5.Intl.NumberFormatOptions options);

                public static extern string[] supportedLocalesOf(
                  string locales,
                  es5.Intl.NumberFormatOptions options);

                public static extern string[] supportedLocalesOf(
                  string[] locales,
                  es5.Intl.NumberFormatOptions options);

                public virtual extern string format(double value);

                public virtual extern es5.Intl.ResolvedNumberFormatOptions resolvedOptions();
            }

            [IgnoreCast]
            [ObjectLiteral]
            [FormerInterface]
            public class DateTimeFormatOptions : IObject
            {

                public string localeMatcher
                {
                    get; set;
                }

                public string weekday
                {
                    get; set;
                }

                public string era
                {
                    get; set;
                }

                public string year
                {
                    get; set;
                }

                public string month
                {
                    get; set;
                }

                public string day
                {
                    get; set;
                }

                public string hour
                {
                    get; set;
                }

                public string minute
                {
                    get; set;
                }

                public string second
                {
                    get; set;
                }

                public string timeZoneName
                {
                    get; set;
                }

                public string formatMatcher
                {
                    get; set;
                }

                public bool? hour12
                {
                    get; set;
                }

                public string timeZone
                {
                    get; set;
                }
            }

            [IgnoreCast]
            [ObjectLiteral]
            [FormerInterface]
            public class ResolvedDateTimeFormatOptions : IObject
            {
                public string locale
                {
                    get; set;
                }

                public string calendar
                {
                    get; set;
                }

                public string numberingSystem
                {
                    get; set;
                }

                public string timeZone
                {
                    get; set;
                }

                public bool? hour12
                {
                    get; set;
                }

                public string weekday
                {
                    get; set;
                }

                public string era
                {
                    get; set;
                }

                public string year
                {
                    get; set;
                }

                public string month
                {
                    get; set;
                }

                public string day
                {
                    get; set;
                }

                public string hour
                {
                    get; set;
                }

                public string minute
                {
                    get; set;
                }

                public string second
                {
                    get; set;
                }

                public string timeZoneName
                {
                    get; set;
                }
            }

            [CombinedClass]
            [FormerInterface]
            public abstract class DateTimeFormat : IObject
            {
                public extern DateTimeFormat();

                public extern DateTimeFormat(Union<string, string[]> locales);

                public extern DateTimeFormat(string locales);

                public extern DateTimeFormat(string[] locales);

                public extern DateTimeFormat(
                  Union<string, string[]> locales,
                  es5.Intl.DateTimeFormatOptions options);

                public extern DateTimeFormat(string locales, es5.Intl.DateTimeFormatOptions options);

                public extern DateTimeFormat(string[] locales, es5.Intl.DateTimeFormatOptions options);

                public static extern es5.Intl.DateTimeFormat Self();

                public static extern es5.Intl.DateTimeFormat Self(Union<string, string[]> locales);

                public static extern es5.Intl.DateTimeFormat Self(string locales);

                public static extern es5.Intl.DateTimeFormat Self(string[] locales);

                public static extern es5.Intl.DateTimeFormat Self(
                  Union<string, string[]> locales,
                  es5.Intl.DateTimeFormatOptions options);

                public static extern es5.Intl.DateTimeFormat Self(
                  string locales,
                  es5.Intl.DateTimeFormatOptions options);

                public static extern es5.Intl.DateTimeFormat Self(
                  string[] locales,
                  es5.Intl.DateTimeFormatOptions options);

                public static extern string[] supportedLocalesOf(Union<string, string[]> locales);

                public static extern string[] supportedLocalesOf(string locales);

                public static extern string[] supportedLocalesOf(string[] locales);

                public static extern string[] supportedLocalesOf(
                  Union<string, string[]> locales,
                  es5.Intl.DateTimeFormatOptions options);

                public static extern string[] supportedLocalesOf(
                  string locales,
                  es5.Intl.DateTimeFormatOptions options);

                public static extern string[] supportedLocalesOf(
                  string[] locales,
                  es5.Intl.DateTimeFormatOptions options);

                public virtual extern string format();

                public virtual extern string format(Union<es5.Date, double> date);

                public virtual extern string format(es5.Date date);

                public virtual extern string format(double date);

                public virtual extern es5.Intl.ResolvedDateTimeFormatOptions resolvedOptions();
            }

            [Virtual]
            public abstract class CollatorTypeConfig : IObject
            {
                [Template("new {this}()")]
                public abstract es5.Intl.Collator New();

                [Template("new {this}({0})")]
                public abstract es5.Intl.Collator New(Union<string, string[]> locales);

                [Template("new {this}({0})")]
                public abstract es5.Intl.Collator New(string locales);

                [Template("new {this}({0})")]
                public abstract es5.Intl.Collator New(string[] locales);

                [Template("new {this}({0}, {1})")]
                public abstract es5.Intl.Collator New(
                  Union<string, string[]> locales,
                  es5.Intl.CollatorOptions options);

                [Template("new {this}({0}, {1})")]
                public abstract es5.Intl.Collator New(string locales, es5.Intl.CollatorOptions options);

                [Template("new {this}({0}, {1})")]
                public abstract es5.Intl.Collator New(string[] locales, es5.Intl.CollatorOptions options);

                [Template("{this}()")]
                public abstract es5.Intl.Collator Self();

                [Template("{this}({0})")]
                public abstract es5.Intl.Collator Self(Union<string, string[]> locales);

                [Template("{this}({0})")]
                public abstract es5.Intl.Collator Self(string locales);

                [Template("{this}({0})")]
                public abstract es5.Intl.Collator Self(string[] locales);

                [Template("{this}({0}, {1})")]
                public abstract es5.Intl.Collator Self(
                  Union<string, string[]> locales,
                  es5.Intl.CollatorOptions options);

                [Template("{this}({0}, {1})")]
                public abstract es5.Intl.Collator Self(string locales, es5.Intl.CollatorOptions options);

                [Template("{this}({0}, {1})")]
                public abstract es5.Intl.Collator Self(
                  string[] locales,
                  es5.Intl.CollatorOptions options);

                public abstract string[] supportedLocalesOf(Union<string, string[]> locales);

                public abstract string[] supportedLocalesOf(string locales);

                public abstract string[] supportedLocalesOf(string[] locales);

                public abstract string[] supportedLocalesOf(
                  Union<string, string[]> locales,
                  es5.Intl.CollatorOptions options);

                public abstract string[] supportedLocalesOf(
                  string locales,
                  es5.Intl.CollatorOptions options);

                public abstract string[] supportedLocalesOf(
                  string[] locales,
                  es5.Intl.CollatorOptions options);
            }

            [Virtual]
            public abstract class NumberFormatTypeConfig : IObject
            {
                [Template("new {this}()")]
                public abstract es5.Intl.NumberFormat New();

                [Template("new {this}({0})")]
                public abstract es5.Intl.NumberFormat New(Union<string, string[]> locales);

                [Template("new {this}({0})")]
                public abstract es5.Intl.NumberFormat New(string locales);

                [Template("new {this}({0})")]
                public abstract es5.Intl.NumberFormat New(string[] locales);

                [Template("new {this}({0}, {1})")]
                public abstract es5.Intl.NumberFormat New(
                  Union<string, string[]> locales,
                  es5.Intl.NumberFormatOptions options);

                [Template("new {this}({0}, {1})")]
                public abstract es5.Intl.NumberFormat New(
                  string locales,
                  es5.Intl.NumberFormatOptions options);

                [Template("new {this}({0}, {1})")]
                public abstract es5.Intl.NumberFormat New(
                  string[] locales,
                  es5.Intl.NumberFormatOptions options);

                [Template("{this}()")]
                public abstract es5.Intl.NumberFormat Self();

                [Template("{this}({0})")]
                public abstract es5.Intl.NumberFormat Self(Union<string, string[]> locales);

                [Template("{this}({0})")]
                public abstract es5.Intl.NumberFormat Self(string locales);

                [Template("{this}({0})")]
                public abstract es5.Intl.NumberFormat Self(string[] locales);

                [Template("{this}({0}, {1})")]
                public abstract es5.Intl.NumberFormat Self(
                  Union<string, string[]> locales,
                  es5.Intl.NumberFormatOptions options);

                [Template("{this}({0}, {1})")]
                public abstract es5.Intl.NumberFormat Self(
                  string locales,
                  es5.Intl.NumberFormatOptions options);

                [Template("{this}({0}, {1})")]
                public abstract es5.Intl.NumberFormat Self(
                  string[] locales,
                  es5.Intl.NumberFormatOptions options);

                public abstract string[] supportedLocalesOf(Union<string, string[]> locales);

                public abstract string[] supportedLocalesOf(string locales);

                public abstract string[] supportedLocalesOf(string[] locales);

                public abstract string[] supportedLocalesOf(
                  Union<string, string[]> locales,
                  es5.Intl.NumberFormatOptions options);

                public abstract string[] supportedLocalesOf(
                  string locales,
                  es5.Intl.NumberFormatOptions options);

                public abstract string[] supportedLocalesOf(
                  string[] locales,
                  es5.Intl.NumberFormatOptions options);
            }

            [Virtual]
            public abstract class DateTimeFormatTypeConfig : IObject
            {
                [Template("new {this}()")]
                public abstract es5.Intl.DateTimeFormat New();

                [Template("new {this}({0})")]
                public abstract es5.Intl.DateTimeFormat New(Union<string, string[]> locales);

                [Template("new {this}({0})")]
                public abstract es5.Intl.DateTimeFormat New(string locales);

                [Template("new {this}({0})")]
                public abstract es5.Intl.DateTimeFormat New(string[] locales);

                [Template("new {this}({0}, {1})")]
                public abstract es5.Intl.DateTimeFormat New(
                  Union<string, string[]> locales,
                  es5.Intl.DateTimeFormatOptions options);

                [Template("new {this}({0}, {1})")]
                public abstract es5.Intl.DateTimeFormat New(
                  string locales,
                  es5.Intl.DateTimeFormatOptions options);

                [Template("new {this}({0}, {1})")]
                public abstract es5.Intl.DateTimeFormat New(
                  string[] locales,
                  es5.Intl.DateTimeFormatOptions options);

                [Template("{this}()")]
                public abstract es5.Intl.DateTimeFormat Self();

                [Template("{this}({0})")]
                public abstract es5.Intl.DateTimeFormat Self(Union<string, string[]> locales);

                [Template("{this}({0})")]
                public abstract es5.Intl.DateTimeFormat Self(string locales);

                [Template("{this}({0})")]
                public abstract es5.Intl.DateTimeFormat Self(string[] locales);

                [Template("{this}({0}, {1})")]
                public abstract es5.Intl.DateTimeFormat Self(
                  Union<string, string[]> locales,
                  es5.Intl.DateTimeFormatOptions options);

                [Template("{this}({0}, {1})")]
                public abstract es5.Intl.DateTimeFormat Self(
                  string locales,
                  es5.Intl.DateTimeFormatOptions options);

                [Template("{this}({0}, {1})")]
                public abstract es5.Intl.DateTimeFormat Self(
                  string[] locales,
                  es5.Intl.DateTimeFormatOptions options);

                public abstract string[] supportedLocalesOf(Union<string, string[]> locales);

                public abstract string[] supportedLocalesOf(string locales);

                public abstract string[] supportedLocalesOf(string[] locales);

                public abstract string[] supportedLocalesOf(
                  Union<string, string[]> locales,
                  es5.Intl.DateTimeFormatOptions options);

                public abstract string[] supportedLocalesOf(
                  string locales,
                  es5.Intl.DateTimeFormatOptions options);

                public abstract string[] supportedLocalesOf(
                  string[] locales,
                  es5.Intl.DateTimeFormatOptions options);
            }
        }
    }
}
