// Decompiled with JetBrains decompiler
// Type: Retyped.Primitive.String
// Assembly: Retyped.es5, Version=2.8.2.0, Culture=neutral, PublicKeyToken=null
// MVID: EC57AC2B-0E02-4A1C-B567-F790F377783B
// Assembly location: C:\work\curiosity\tesserae\Tesserae\bin\Debug\net461\Retyped.es5.dll

using Bridge;

namespace Retyped.Primitive
{
  [CombinedClass]
  [StaticInterface("StringConstructor")]
  [Namespace(false)]
  public class String : Object, String.Interface, IObject
  {
    private static readonly String \u003Cprototype\u003Ek__BackingField;
    private readonly double \u003Clength\u003Ek__BackingField;

    public extern String();

    public extern String(object value);

    public static String prototype
    {
      get
      {
        return String.\u003Cprototype\u003Ek__BackingField;
      }
    }

    public static extern string Self();

    public static extern string Self(object value);

    [ExpandParams]
    public static extern string fromCharCode(params double[] codes);

    public new virtual extern string toString();

    public virtual extern string charAt(double pos);

    public virtual extern double charCodeAt(double index);

    [ExpandParams]
    public virtual extern string concat(params string[] strings);

    public virtual extern double indexOf(string searchString);

    public virtual extern double indexOf(string searchString, double position);

    public virtual extern double lastIndexOf(string searchString);

    public virtual extern double lastIndexOf(string searchString, double position);

    public virtual extern double localeCompare(string that);

    public virtual extern es5.RegExpMatchArray match(Union<string, es5.RegExp> regexp);

    public virtual extern es5.RegExpMatchArray match(string regexp);

    public virtual extern es5.RegExpMatchArray match(es5.RegExp regexp);

    public virtual extern string replace(Union<string, es5.RegExp> searchValue, string replaceValue);

    public virtual extern string replace(string searchValue, string replaceValue);

    public virtual extern string replace(es5.RegExp searchValue, string replaceValue);

    public virtual extern string replace(
      Union<string, es5.RegExp> searchValue,
      String.replaceFn replacer);

    public virtual extern string replace(string searchValue, String.replaceFn replacer);

    public virtual extern string replace(es5.RegExp searchValue, String.replaceFn replacer);

    public virtual extern double search(Union<string, es5.RegExp> regexp);

    public virtual extern double search(string regexp);

    public virtual extern double search(es5.RegExp regexp);

    public virtual extern string slice();

    public virtual extern string slice(double start);

    public virtual extern string slice(double start, double end);

    public virtual extern string[] split(Union<string, es5.RegExp> separator);

    public virtual extern string[] split(string separator);

    public virtual extern string[] split(es5.RegExp separator);

    public virtual extern string[] split(Union<string, es5.RegExp> separator, double limit);

    public virtual extern string[] split(string separator, double limit);

    public virtual extern string[] split(es5.RegExp separator, double limit);

    public virtual extern string substring(double start);

    public virtual extern string substring(double start, double end);

    public virtual extern string toLowerCase();

    public virtual extern string toLocaleLowerCase();

    public virtual extern string toUpperCase();

    public virtual extern string toLocaleUpperCase();

    public virtual extern string trim();

    public virtual double length
    {
      get
      {
        return this.\u003Clength\u003Ek__BackingField;
      }
    }

    public virtual extern string substr(double from);

    public virtual extern string substr(double from, double length);

    public virtual extern string valueOf();

    public virtual extern string this[double index] { get; }

    public virtual extern double localeCompare(string that, Union<string, string[]> locales);

    public virtual extern double localeCompare(string that, string locales);

    public virtual extern double localeCompare(string that, string[] locales);

    public virtual extern double localeCompare(
      string that,
      Union<string, string[]> locales,
      es5.Intl.CollatorOptions options);

    public virtual extern double localeCompare(
      string that,
      string locales,
      es5.Intl.CollatorOptions options);

    public virtual extern double localeCompare(
      string that,
      string[] locales,
      es5.Intl.CollatorOptions options);

    public static extern implicit operator String(string value);

    [Template("{this} != null ? {this}.valueOf() : {this}")]
    public static extern implicit operator string(String value);

    [Generated]
    [IgnoreCast]
    [ClassInterface]
    [Name("String")]
    public new interface Interface : IObject
    {
      string toString();

      string charAt(double pos);

      double charCodeAt(double index);

      [ExpandParams]
      string concat(params string[] strings);

      double indexOf(string searchString);

      double indexOf(string searchString, double position);

      double lastIndexOf(string searchString);

      double lastIndexOf(string searchString, double position);

      double localeCompare(string that);

      es5.RegExpMatchArray match(Union<string, es5.RegExp> regexp);

      es5.RegExpMatchArray match(string regexp);

      es5.RegExpMatchArray match(es5.RegExp regexp);

      string replace(Union<string, es5.RegExp> searchValue, string replaceValue);

      string replace(string searchValue, string replaceValue);

      string replace(es5.RegExp searchValue, string replaceValue);

      string replace(Union<string, es5.RegExp> searchValue, String.replaceFn replacer);

      string replace(string searchValue, String.replaceFn replacer);

      string replace(es5.RegExp searchValue, String.replaceFn replacer);

      double search(Union<string, es5.RegExp> regexp);

      double search(string regexp);

      double search(es5.RegExp regexp);

      string slice();

      string slice(double start);

      string slice(double start, double end);

      string[] split(Union<string, es5.RegExp> separator);

      string[] split(string separator);

      string[] split(es5.RegExp separator);

      string[] split(Union<string, es5.RegExp> separator, double limit);

      string[] split(string separator, double limit);

      string[] split(es5.RegExp separator, double limit);

      string substring(double start);

      string substring(double start, double end);

      string toLowerCase();

      string toLocaleLowerCase();

      string toUpperCase();

      string toLocaleUpperCase();

      string trim();

      double length { get; }

      string substr(double from);

      string substr(double from, double length);

      string valueOf();

      string this[double index] { get; }

      double localeCompare(string that, Union<string, string[]> locales);

      double localeCompare(string that, string locales);

      double localeCompare(string that, string[] locales);

      double localeCompare(
        string that,
        Union<string, string[]> locales,
        es5.Intl.CollatorOptions options);

      double localeCompare(string that, string locales, es5.Intl.CollatorOptions options);

      double localeCompare(string that, string[] locales, es5.Intl.CollatorOptions options);
    }

    [Generated]
    public delegate string replaceFn(string substring, params object[] args);
  }
}
