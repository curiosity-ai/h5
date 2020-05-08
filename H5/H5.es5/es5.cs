// Decompiled with JetBrains decompiler
// Type: H5.es5
// Assembly: H5.es5, Version=2.8.2.0, Culture=neutral, PublicKeyToken=null
// MVID: EC57AC2B-0E02-4A1C-B567-F790F377783B
// Assembly location: C:\work\curiosity\tesserae\Tesserae\bin\Debug\net461\H5.es5.dll

using HighFive;
using H5.Primitive;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace H5
{
  [Scope]
  [GlobalMethods]
  public static class es5
  {
    public static readonly double NaN;
    public static readonly double Infinity;

    public static extern object eval(string x);

    public static extern double parseInt(string s);

    public static extern double parseInt(string s, double radix);

    public static extern double parseFloat(string @string);

    public static extern bool isNaN(double number);

    public static extern bool isFinite(double number);

    public static extern string decodeURI(string encodedURI);

    public static extern string decodeURIComponent(string encodedURIComponent);

    public static extern string encodeURI(string uri);

    public static extern string encodeURIComponent(string uriComponent);

    public static extern string escape(string @string);

    public static extern string unescape(string @string);

    [IgnoreCast]
    [ObjectLiteral]
    [FormerInterface]
    public class PropertyDescriptor : IObject
    {
      private bool? _configurable_BackingField;
      private bool? _enumerable_BackingField;
      private object _value_BackingField;
      private bool? _writable_BackingField;
      private Func<object> _get_BackingField;
      private es5.PropertyDescriptor.setFn _set_BackingField;

      public bool? configurable
      {
        get
        {
          return this._configurable_BackingField;
        }
        set
        {
          this._configurable_BackingField = value;
        }
      }

      public bool? enumerable
      {
        get
        {
          return this._enumerable_BackingField;
        }
        set
        {
          this._enumerable_BackingField = value;
        }
      }

      public object value
      {
        get
        {
          return this._value_BackingField;
        }
        set
        {
          this._value_BackingField = value;
        }
      }

      public bool? writable
      {
        get
        {
          return this._writable_BackingField;
        }
        set
        {
          this._writable_BackingField = value;
        }
      }

      public Func<object> get
      {
        get
        {
          return this._get_BackingField;
        }
        set
        {
          this._get_BackingField = value;
        }
      }

      public es5.PropertyDescriptor.setFn set
      {
        get
        {
          return this._set_BackingField;
        }
        set
        {
          this._set_BackingField = value;
        }
      }

      [Generated]
      public delegate void setFn(object v);
    }

    [IgnoreCast]
    [Virtual]
    [FormerInterface]
    public class PropertyDescriptorMap : IObject
    {
      public virtual extern es5.PropertyDescriptor this[string s] { get; set; }
    }

    [CombinedClass]
    [StaticInterface("FunctionConstructor")]
    [FormerInterface]
    public class Function : IObject
    {
      private static readonly es5.Function _prototype_Static_BackingField;
      private object _prototype_BackingField;
      private readonly double _length_BackingField;
      private object _arguments_BackingField;
      private es5.Function _caller_BackingField;

      [ExpandParams]
      public extern Function(params string[] args);

      [Name("prototype")]
      public static es5.Function prototype_Static
      {
        get
        {
          return es5.Function._prototype_Static_BackingField;
        }
      }

      [ExpandParams]
      public static extern es5.Function Self(params string[] args);

      [Template("({this}.$method || {this}).apply({thisArg})")]
      public virtual extern object apply(object thisArg);

      [Template("({this}.$method || {this}).apply({thisArg}, {argArray})")]
      public virtual extern object apply(object thisArg, object argArray);

      [Template("({this}.$method || {this}).call({thisArg}, {*argArray})")]
      [ExpandParams]
      public virtual extern object call(object thisArg, params object[] argArray);

      [Template("({this}.$method || {this}).bind({thisArg}, {*argArray})")]
      [ExpandParams]
      public virtual extern object bind(object thisArg, params object[] argArray);

      public virtual extern string toString();

      public virtual object prototype
      {
        get
        {
          return this._prototype_BackingField;
        }
        set
        {
          this._prototype_BackingField = value;
        }
      }

      public virtual double length
      {
        get
        {
          return this._length_BackingField;
        }
      }

      public virtual object arguments
      {
        get
        {
          return this._arguments_BackingField;
        }
        set
        {
          this._arguments_BackingField = value;
        }
      }

      public virtual es5.Function caller
      {
        get
        {
          return this._caller_BackingField;
        }
        set
        {
          this._caller_BackingField = value;
        }
      }

      public static extern implicit operator es5.Function(Delegate d);

      public static extern implicit operator Delegate(es5.Function f);
    }

    [IgnoreCast]
    public interface FunctionConstructor : IObject
    {
      [Template("new {this}({0})")]
      es5.Function New(params string[] args);

      [Template("{this}({0})")]
      es5.Function Self(params string[] args);

      es5.Function prototype { get; }
    }

    [IgnoreCast]
    [Virtual]
    [FormerInterface]
    public class IArguments : IObject
    {
      private double _length_BackingField;
      private es5.Function _callee_BackingField;

      public virtual extern object this[double index] { get; set; }

      public virtual double length
      {
        get
        {
          return this._length_BackingField;
        }
        set
        {
          this._length_BackingField = value;
        }
      }

      public virtual es5.Function callee
      {
        get
        {
          return this._callee_BackingField;
        }
        set
        {
          this._callee_BackingField = value;
        }
      }
    }

    [IgnoreCast]
    [Virtual]
    [FormerInterface]
    public abstract class TemplateStringsArray : es5.ReadonlyArray<string>
    {
      public abstract es5.ReadonlyArray<string> raw { get; }
    }

    [CombinedClass]
    [StaticInterface("Math.Interface")]
    public static class Math
    {
      private static readonly double _E_BackingField;
      private static readonly double _LN10_BackingField;
      private static readonly double _LN2_BackingField;
      private static readonly double _LOG2E_BackingField;
      private static readonly double _LOG10E_BackingField;
      private static readonly double _PI_BackingField;
      private static readonly double _SQRT1_2_BackingField;
      private static readonly double _SQRT2_BackingField;

      public static double E
      {
        get
        {
          return es5.Math._E_BackingField;
        }
      }

      public static double LN10
      {
        get
        {
          return es5.Math._LN10_BackingField;
        }
      }

      public static double LN2
      {
        get
        {
          return es5.Math._LN2_BackingField;
        }
      }

      public static double LOG2E
      {
        get
        {
          return es5.Math._LOG2E_BackingField;
        }
      }

      public static double LOG10E
      {
        get
        {
          return es5.Math._LOG10E_BackingField;
        }
      }

      public static double PI
      {
        get
        {
          return es5.Math._PI_BackingField;
        }
      }

      public static double SQRT1_2
      {
        get
        {
          return es5.Math._SQRT1_2_BackingField;
        }
      }

      public static double SQRT2
      {
        get
        {
          return es5.Math._SQRT2_BackingField;
        }
      }

      public static extern double abs(double x);

      public static extern double acos(double x);

      public static extern double asin(double x);

      public static extern double atan(double x);

      public static extern double atan2(double y, double x);

      public static extern double ceil(double x);

      public static extern double cos(double x);

      public static extern double exp(double x);

      public static extern double floor(double x);

      public static extern double log(double x);

      [ExpandParams]
      public static extern double max(params double[] values);

      [ExpandParams]
      public static extern double min(params double[] values);

      public static extern double pow(double x, double y);

      public static extern double random();

      public static extern double round(double x);

      public static extern double sin(double x);

      public static extern double sqrt(double x);

      public static extern double tan(double x);

      [Generated]
      [IgnoreCast]
      [ClassInterface]
      [Name("Math")]
      public interface Interface : IObject
      {
        double E { get; }

        double LN10 { get; }

        double LN2 { get; }

        double LOG2E { get; }

        double LOG10E { get; }

        double PI { get; }

        double SQRT1_2 { get; }

        double SQRT2 { get; }

        double abs(double x);

        double acos(double x);

        double asin(double x);

        double atan(double x);

        double atan2(double y, double x);

        double ceil(double x);

        double cos(double x);

        double exp(double x);

        double floor(double x);

        double log(double x);

        [ExpandParams]
        double max(params double[] values);

        [ExpandParams]
        double min(params double[] values);

        double pow(double x, double y);

        double random();

        double round(double x);

        double sin(double x);

        double sqrt(double x);

        double tan(double x);
      }
    }

    [CombinedClass]
    [StaticInterface("DateConstructor")]
    [FormerInterface]
    public class Date : IObject
    {
      private static readonly es5.Date _prototype_BackingField;

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
        get
        {
          return es5.Date._prototype_BackingField;
        }
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

    [IgnoreCast]
    public interface DateConstructor : IObject
    {
      [Template("new {this}()")]
      es5.Date New();

      [Template("new {this}({0})")]
      es5.Date New(double value);

      [Template("new {this}({0})")]
      es5.Date New(string value);

      [Template("new {this}({0}, {1})")]
      es5.Date New(double year, double month);

      [Template("new {this}({0}, {1}, {2})")]
      es5.Date New(double year, double month, double date);

      [Template("new {this}({0}, {1}, {2}, {3})")]
      es5.Date New(double year, double month, double date, double hours);

      [Template("new {this}({0}, {1}, {2}, {3}, {4})")]
      es5.Date New(double year, double month, double date, double hours, double minutes);

      [Template("new {this}({0}, {1}, {2}, {3}, {4}, {5})")]
      es5.Date New(
        double year,
        double month,
        double date,
        double hours,
        double minutes,
        double seconds);

      [Template("new {this}({0}, {1}, {2}, {3}, {4}, {5}, {6})")]
      es5.Date New(
        double year,
        double month,
        double date,
        double hours,
        double minutes,
        double seconds,
        double ms);

      [Template("{this}()")]
      string Self();

      es5.Date prototype { get; }

      double parse(string s);

      double UTC(double year, double month);

      double UTC(double year, double month, double date);

      double UTC(double year, double month, double date, double hours);

      double UTC(double year, double month, double date, double hours, double minutes);

      double UTC(
        double year,
        double month,
        double date,
        double hours,
        double minutes,
        double seconds);

      double UTC(
        double year,
        double month,
        double date,
        double hours,
        double minutes,
        double seconds,
        double ms);

      double now();
    }

    [IgnoreCast]
    [Virtual]
    [FormerInterface]
    public class RegExpMatchArray : es5.Array<string>
    {
      private double? _index_BackingField;
      private string _input_BackingField;

      public virtual double? index
      {
        get
        {
          return this._index_BackingField;
        }
        set
        {
          this._index_BackingField = value;
        }
      }

      public virtual string input
      {
        get
        {
          return this._input_BackingField;
        }
        set
        {
          this._input_BackingField = value;
        }
      }
    }

    [IgnoreCast]
    [Virtual]
    [FormerInterface]
    public class RegExpExecArray : es5.Array<string>
    {
      private double _index_BackingField;
      private string _input_BackingField;

      public virtual double index
      {
        get
        {
          return this._index_BackingField;
        }
        set
        {
          this._index_BackingField = value;
        }
      }

      public virtual string input
      {
        get
        {
          return this._input_BackingField;
        }
        set
        {
          this._input_BackingField = value;
        }
      }
    }

    [CombinedClass]
    [StaticInterface("RegExpConstructor")]
    [FormerInterface]
    public class RegExp : IObject
    {
      private static readonly es5.RegExp _prototype_BackingField;
      private static string _Dollar1_BackingField;
      private static string _Dollar2_BackingField;
      private static string _Dollar3_BackingField;
      private static string _Dollar4_BackingField;
      private static string _Dollar5_BackingField;
      private static string _Dollar6_BackingField;
      private static string _Dollar7_BackingField;
      private static string _Dollar8_BackingField;
      private static string _Dollar9_BackingField;
      private static string _lastMatch_BackingField;
      private readonly string _source_BackingField;
      private readonly bool _global_BackingField;
      private readonly bool _ignoreCase_BackingField;
      private readonly bool _multiline_BackingField;
      private double _lastIndex_BackingField;

      public extern RegExp(Union<es5.RegExp, string> pattern);

      public extern RegExp(es5.RegExp pattern);

      public extern RegExp(string pattern);

      public extern RegExp(string pattern, string flags);

      public static es5.RegExp prototype
      {
        get
        {
          return es5.RegExp._prototype_BackingField;
        }
      }

      [Name("$1")]
      public static string Dollar1
      {
        get
        {
          return es5.RegExp._Dollar1_BackingField;
        }
        set
        {
          es5.RegExp._Dollar1_BackingField = value;
        }
      }

      [Name("$2")]
      public static string Dollar2
      {
        get
        {
          return es5.RegExp._Dollar2_BackingField;
        }
        set
        {
          es5.RegExp._Dollar2_BackingField = value;
        }
      }

      [Name("$3")]
      public static string Dollar3
      {
        get
        {
          return es5.RegExp._Dollar3_BackingField;
        }
        set
        {
          es5.RegExp._Dollar3_BackingField = value;
        }
      }

      [Name("$4")]
      public static string Dollar4
      {
        get
        {
          return es5.RegExp._Dollar4_BackingField;
        }
        set
        {
          es5.RegExp._Dollar4_BackingField = value;
        }
      }

      [Name("$5")]
      public static string Dollar5
      {
        get
        {
          return es5.RegExp._Dollar5_BackingField;
        }
        set
        {
          es5.RegExp._Dollar5_BackingField = value;
        }
      }

      [Name("$6")]
      public static string Dollar6
      {
        get
        {
          return es5.RegExp._Dollar6_BackingField;
        }
        set
        {
          es5.RegExp._Dollar6_BackingField = value;
        }
      }

      [Name("$7")]
      public static string Dollar7
      {
        get
        {
          return es5.RegExp._Dollar7_BackingField;
        }
        set
        {
          es5.RegExp._Dollar7_BackingField = value;
        }
      }

      [Name("$8")]
      public static string Dollar8
      {
        get
        {
          return es5.RegExp._Dollar8_BackingField;
        }
        set
        {
          es5.RegExp._Dollar8_BackingField = value;
        }
      }

      [Name("$9")]
      public static string Dollar9
      {
        get
        {
          return es5.RegExp._Dollar9_BackingField;
        }
        set
        {
          es5.RegExp._Dollar9_BackingField = value;
        }
      }

      public static string lastMatch
      {
        get
        {
          return es5.RegExp._lastMatch_BackingField;
        }
        set
        {
          es5.RegExp._lastMatch_BackingField = value;
        }
      }

      public static extern es5.RegExp Self(Union<es5.RegExp, string> pattern);

      public static extern es5.RegExp Self(es5.RegExp pattern);

      public static extern es5.RegExp Self(string pattern);

      public static extern es5.RegExp Self(string pattern, string flags);

      public virtual extern es5.RegExpExecArray exec(string @string);

      public virtual extern bool test(string @string);

      public virtual string source
      {
        get
        {
          return this._source_BackingField;
        }
      }

      public virtual bool global
      {
        get
        {
          return this._global_BackingField;
        }
      }

      public virtual bool ignoreCase
      {
        get
        {
          return this._ignoreCase_BackingField;
        }
      }

      public virtual bool multiline
      {
        get
        {
          return this._multiline_BackingField;
        }
      }

      public virtual double lastIndex
      {
        get
        {
          return this._lastIndex_BackingField;
        }
        set
        {
          this._lastIndex_BackingField = value;
        }
      }

      public virtual extern es5.RegExp compile();
    }

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

    [CombinedClass]
    [StaticInterface("ErrorConstructor")]
    [FormerInterface]
    public class Error : IObject
    {
      private static readonly es5.Error _prototype_BackingField;
      private string _name_BackingField;
      private string _message_BackingField;
      private string _stack_BackingField;

      public extern Error();

      public extern Error(string message);

      public static es5.Error prototype
      {
        get
        {
          return es5.Error._prototype_BackingField;
        }
      }

      public static extern es5.Error Self();

      public static extern es5.Error Self(string message);

      public virtual string name
      {
        get
        {
          return this._name_BackingField;
        }
        set
        {
          this._name_BackingField = value;
        }
      }

      public virtual string message
      {
        get
        {
          return this._message_BackingField;
        }
        set
        {
          this._message_BackingField = value;
        }
      }

      public virtual string stack
      {
        get
        {
          return this._stack_BackingField;
        }
        set
        {
          this._stack_BackingField = value;
        }
      }
    }

    [IgnoreCast]
    public interface ErrorConstructor : IObject
    {
      [Template("new {this}()")]
      es5.Error New();

      [Template("new {this}({0})")]
      es5.Error New(string message);

      [Template("{this}()")]
      es5.Error Self();

      [Template("{this}({0})")]
      es5.Error Self(string message);

      es5.Error prototype { get; }
    }

    [CombinedClass]
    [StaticInterface("EvalErrorConstructor")]
    [FormerInterface]
    public class EvalError : es5.Error
    {
      private static readonly es5.EvalError _prototype_BackingField;

      public extern EvalError();

      public extern EvalError(string message);

      public static es5.EvalError prototype
      {
        get
        {
          return es5.EvalError._prototype_BackingField;
        }
      }

      public static extern es5.EvalError Self();

      public static extern es5.EvalError Self(string message);
    }

    [IgnoreCast]
    public interface EvalErrorConstructor : IObject
    {
      [Template("new {this}()")]
      es5.EvalError New();

      [Template("new {this}({0})")]
      es5.EvalError New(string message);

      [Template("{this}()")]
      es5.EvalError Self();

      [Template("{this}({0})")]
      es5.EvalError Self(string message);

      es5.EvalError prototype { get; }
    }

    [CombinedClass]
    [StaticInterface("RangeErrorConstructor")]
    [FormerInterface]
    public class RangeError : es5.Error
    {
      private static readonly es5.RangeError _prototype_BackingField;

      public extern RangeError();

      public extern RangeError(string message);

      public static es5.RangeError prototype
      {
        get
        {
          return es5.RangeError._prototype_BackingField;
        }
      }

      public static extern es5.RangeError Self();

      public static extern es5.RangeError Self(string message);
    }

    [IgnoreCast]
    public interface RangeErrorConstructor : IObject
    {
      [Template("new {this}()")]
      es5.RangeError New();

      [Template("new {this}({0})")]
      es5.RangeError New(string message);

      [Template("{this}()")]
      es5.RangeError Self();

      [Template("{this}({0})")]
      es5.RangeError Self(string message);

      es5.RangeError prototype { get; }
    }

    [CombinedClass]
    [StaticInterface("ReferenceErrorConstructor")]
    [FormerInterface]
    public class ReferenceError : es5.Error
    {
      private static readonly es5.ReferenceError _prototype_BackingField;

      public extern ReferenceError();

      public extern ReferenceError(string message);

      public static es5.ReferenceError prototype
      {
        get
        {
          return es5.ReferenceError._prototype_BackingField;
        }
      }

      public static extern es5.ReferenceError Self();

      public static extern es5.ReferenceError Self(string message);
    }

    [IgnoreCast]
    public interface ReferenceErrorConstructor : IObject
    {
      [Template("new {this}()")]
      es5.ReferenceError New();

      [Template("new {this}({0})")]
      es5.ReferenceError New(string message);

      [Template("{this}()")]
      es5.ReferenceError Self();

      [Template("{this}({0})")]
      es5.ReferenceError Self(string message);

      es5.ReferenceError prototype { get; }
    }

    [CombinedClass]
    [StaticInterface("SyntaxErrorConstructor")]
    [FormerInterface]
    public class SyntaxError : es5.Error
    {
      private static readonly es5.SyntaxError _prototype_BackingField;

      public extern SyntaxError();

      public extern SyntaxError(string message);

      public static es5.SyntaxError prototype
      {
        get
        {
          return es5.SyntaxError._prototype_BackingField;
        }
      }

      public static extern es5.SyntaxError Self();

      public static extern es5.SyntaxError Self(string message);
    }

    [IgnoreCast]
    public interface SyntaxErrorConstructor : IObject
    {
      [Template("new {this}()")]
      es5.SyntaxError New();

      [Template("new {this}({0})")]
      es5.SyntaxError New(string message);

      [Template("{this}()")]
      es5.SyntaxError Self();

      [Template("{this}({0})")]
      es5.SyntaxError Self(string message);

      es5.SyntaxError prototype { get; }
    }

    [CombinedClass]
    [StaticInterface("TypeErrorConstructor")]
    [FormerInterface]
    public class TypeError : es5.Error
    {
      private static readonly es5.TypeError _prototype_BackingField;

      public extern TypeError();

      public extern TypeError(string message);

      public static es5.TypeError prototype
      {
        get
        {
          return es5.TypeError._prototype_BackingField;
        }
      }

      public static extern es5.TypeError Self();

      public static extern es5.TypeError Self(string message);
    }

    [IgnoreCast]
    public interface TypeErrorConstructor : IObject
    {
      [Template("new {this}()")]
      es5.TypeError New();

      [Template("new {this}({0})")]
      es5.TypeError New(string message);

      [Template("{this}()")]
      es5.TypeError Self();

      [Template("{this}({0})")]
      es5.TypeError Self(string message);

      es5.TypeError prototype { get; }
    }

    [CombinedClass]
    [StaticInterface("URIErrorConstructor")]
    [FormerInterface]
    public class URIError : es5.Error
    {
      private static readonly es5.URIError _prototype_BackingField;

      public extern URIError();

      public extern URIError(string message);

      public static es5.URIError prototype
      {
        get
        {
          return es5.URIError._prototype_BackingField;
        }
      }

      public static extern es5.URIError Self();

      public static extern es5.URIError Self(string message);
    }

    [IgnoreCast]
    public interface URIErrorConstructor : IObject
    {
      [Template("new {this}()")]
      es5.URIError New();

      [Template("new {this}({0})")]
      es5.URIError New(string message);

      [Template("{this}()")]
      es5.URIError Self();

      [Template("{this}({0})")]
      es5.URIError Self(string message);

      es5.URIError prototype { get; }
    }

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

    [IgnoreCast]
    [IgnoreGeneric(AllowInTypeScript = true)]
    [Virtual]
    [FormerInterface]
    public abstract class ReadonlyArray<T> : IObject
    {
      public abstract double length { get; }

      public abstract string toString();

      public abstract string toLocaleString();

      [ExpandParams]
      public abstract T[] concat(params es5.ConcatArray<T>[] items);

      [ExpandParams]
      public abstract T[] concat(params Union<T, es5.ConcatArray<T>>[] items);

      [ExpandParams]
      public abstract T[] concat(params T[] items);

      public abstract string join();

      public abstract string join(string separator);

      public abstract T[] slice();

      public abstract T[] slice(double start);

      public abstract T[] slice(double start, double end);

      public abstract double indexOf(T searchElement);

      public abstract double indexOf(T searchElement, double fromIndex);

      public abstract double lastIndexOf(T searchElement);

      public abstract double lastIndexOf(T searchElement, double fromIndex);

      public abstract bool every(es5.ReadonlyArray<T>.everyFn callbackfn);

      public abstract bool every(es5.ReadonlyArray<T>.everyFn callbackfn, object thisArg);

      public abstract bool some(es5.ReadonlyArray<T>.someFn callbackfn);

      public abstract bool some(es5.ReadonlyArray<T>.someFn callbackfn, object thisArg);

      public abstract void forEach(es5.ReadonlyArray<T>.forEachFn callbackfn);

      public abstract void forEach(es5.ReadonlyArray<T>.forEachFn callbackfn, object thisArg);

      public abstract U[] map<U>(es5.ReadonlyArray<T>.mapFn<U> callbackfn);

      public abstract U[] map<U>(es5.ReadonlyArray<T>.mapFn<U> callbackfn, object thisArg);

      [Where("S", new string[] {"T"}, EnableImplicitConversion = true)]
      public abstract S[] filter<S>(es5.ReadonlyArray<T>.filterFn<S> callbackfn);

      [Where("S", new string[] {"T"}, EnableImplicitConversion = true)]
      public abstract S[] filter<S>(es5.ReadonlyArray<T>.filterFn<S> callbackfn, object thisArg);

      public abstract T[] filter(es5.ReadonlyArray<T>.filterFn2 callbackfn);

      public abstract T[] filter(es5.ReadonlyArray<T>.filterFn2 callbackfn, object thisArg);

      public abstract T reduce(es5.ReadonlyArray<T>.reduceFn callbackfn);

      public abstract T reduce(es5.ReadonlyArray<T>.reduceFn callbackfn, T initialValue);

      public abstract U reduce<U>(es5.ReadonlyArray<T>.reduceFn2<U> callbackfn, U initialValue);

      public abstract T reduceRight(es5.ReadonlyArray<T>.reduceRightFn callbackfn);

      public abstract T reduceRight(es5.ReadonlyArray<T>.reduceRightFn callbackfn, T initialValue);

      public abstract U reduceRight<U>(
        es5.ReadonlyArray<T>.reduceRightFn2<U> callbackfn,
        U initialValue);

      public abstract T this[double n] { get; }

      [Generated]
      public delegate bool everyFn(T value, double index, es5.ReadonlyArray<T> array);

      [Generated]
      public delegate bool someFn(T value, double index, es5.ReadonlyArray<T> array);

      [Generated]
      public delegate void forEachFn(T value, double index, es5.ReadonlyArray<T> array);

      [Generated]
      public delegate U mapFn<U>(T value, double index, es5.ReadonlyArray<T> array);

      [Where("S", new string[] {"T"}, EnableImplicitConversion = true)]
      [Generated]
      public delegate bool filterFn<S>(T value, double index, es5.ReadonlyArray<T> array);

      [Generated]
      public delegate object filterFn2(T value, double index, es5.ReadonlyArray<T> array);

      [Generated]
      public delegate T reduceFn(
        T previousValue,
        T currentValue,
        double currentIndex,
        es5.ReadonlyArray<T> array);

      [Generated]
      public delegate U reduceFn2<U>(
        U previousValue,
        T currentValue,
        double currentIndex,
        es5.ReadonlyArray<T> array);

      [Generated]
      public delegate T reduceRightFn(
        T previousValue,
        T currentValue,
        double currentIndex,
        es5.ReadonlyArray<T> array);

      [Generated]
      public delegate U reduceRightFn2<U>(
        U previousValue,
        T currentValue,
        double currentIndex,
        es5.ReadonlyArray<T> array);
    }

    [IgnoreCast]
    [IgnoreGeneric(AllowInTypeScript = true)]
    [Virtual]
    [FormerInterface]
    public abstract class ConcatArray<T> : IObject
    {
      public abstract double length { get; }

      public abstract T this[double n] { get; }

      public abstract string join();

      public abstract string join(string separator);

      public abstract T[] slice();

      public abstract T[] slice(double start);

      public abstract T[] slice(double start, double end);
    }

    [IgnoreGeneric(AllowInTypeScript = true)]
    [CombinedClass]
    [StaticInterface("ArrayConstructor")]
    [FormerInterface]
    public class Array<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IBridgeClass, IReadOnlyList<T>, IReadOnlyCollection<T>, ICollection, IObject
    {
      private static readonly es5.Array<object> _prototype_BackingField;
      private double _length_BackingField;
      private readonly int _System\u002ECollections\u002EGeneric\u002EICollection_T\u003E\u002ECount_BackingField;
      private readonly bool _System\u002ECollections\u002EGeneric\u002EICollection_T\u003E\u002EIsReadOnly_BackingField;
      private readonly int _System\u002ECollections\u002EGeneric\u002EIReadOnlyCollection_T\u003E\u002ECount_BackingField;
      private readonly int _System\u002ECollections\u002EICollection\u002ECount_BackingField;
      private readonly object _System\u002ECollections\u002EICollection\u002ESyncRoot_BackingField;
      private readonly bool _System\u002ECollections\u002EICollection\u002EIsSynchronized_BackingField;

      public extern Array();

      public extern Array(double arrayLength);

      [ExpandParams]
      public extern Array(params T[] items);

      public static es5.Array<object> prototype
      {
        get
        {
          return es5.Array<T>._prototype_BackingField;
        }
      }

      public static extern object[] Self();

      public static extern object[] Self(double arrayLength);

      public static extern T[] Self<T>(double arrayLength);

      [ExpandParams]
      public static extern T[] Self<T>(params T[] items);

      public static extern bool isArray(object arg);

      public virtual double length
      {
        get
        {
          return this._length_BackingField;
        }
        set
        {
          this._length_BackingField = value;
        }
      }

      public virtual extern string toString();

      public virtual extern string toLocaleString();

      [ExpandParams]
      public virtual extern double push(params T[] items);

      public virtual extern Union<T, Undefined> pop();

      [ExpandParams]
      public virtual extern T[] concat(params es5.ConcatArray<T>[] items);

      [ExpandParams]
      public virtual extern T[] concat(params Union<T, es5.ConcatArray<T>>[] items);

      [ExpandParams]
      public virtual extern T[] concat(params T[] items);

      public virtual extern string join();

      public virtual extern string join(string separator);

      public virtual extern T[] reverse();

      public virtual extern Union<T, Undefined> shift();

      public virtual extern T[] slice();

      public virtual extern T[] slice(double start);

      public virtual extern T[] slice(double start, double end);

      public virtual extern es5.Array<T> sort();

      public virtual extern es5.Array<T> sort(es5.Array<T>.sortFn compareFn);

      public virtual extern T[] splice(double start);

      public virtual extern T[] splice(double start, double deleteCount);

      [ExpandParams]
      public virtual extern T[] splice(double start, double deleteCount, params T[] items);

      [ExpandParams]
      public virtual extern double unshift(params T[] items);

      public virtual extern double indexOf(T searchElement);

      public virtual extern double indexOf(T searchElement, double fromIndex);

      public virtual extern double lastIndexOf(T searchElement);

      public virtual extern double lastIndexOf(T searchElement, double fromIndex);

      public virtual extern bool every(es5.Array<T>.everyFn callbackfn);

      public virtual extern bool every(es5.Array<T>.everyFn callbackfn, object thisArg);

      public virtual extern bool some(es5.Array<T>.someFn callbackfn);

      public virtual extern bool some(es5.Array<T>.someFn callbackfn, object thisArg);

      public virtual extern void forEach(es5.Array<T>.forEachFn callbackfn);

      public virtual extern void forEach(es5.Array<T>.forEachFn callbackfn, object thisArg);

      public virtual extern U[] map<U>(es5.Array<T>.mapFn<U> callbackfn);

      public virtual extern U[] map<U>(es5.Array<T>.mapFn<U> callbackfn, object thisArg);

      [Where("S", new string[] {"T"}, EnableImplicitConversion = true)]
      public virtual extern S[] filter<S>(es5.Array<T>.filterFn<S> callbackfn);

      [Where("S", new string[] {"T"}, EnableImplicitConversion = true)]
      public virtual extern S[] filter<S>(es5.Array<T>.filterFn<S> callbackfn, object thisArg);

      public virtual extern T[] filter(es5.Array<T>.filterFn2 callbackfn);

      public virtual extern T[] filter(es5.Array<T>.filterFn2 callbackfn, object thisArg);

      public virtual extern T reduce(es5.Array<T>.reduceFn callbackfn);

      public virtual extern T reduce(es5.Array<T>.reduceFn callbackfn, T initialValue);

      public virtual extern U reduce<U>(es5.Array<T>.reduceFn2<U> callbackfn, U initialValue);

      public virtual extern T reduceRight(es5.Array<T>.reduceRightFn callbackfn);

      public virtual extern T reduceRight(es5.Array<T>.reduceRightFn callbackfn, T initialValue);

      public virtual extern U reduceRight<U>(
        es5.Array<T>.reduceRightFn2<U> callbackfn,
        U initialValue);

      public virtual extern T this[double n] { get; set; }

      [Template("System.Array.indexOf({this}, {item}, 0, null, {T})")]
      extern int IList<T>.IndexOf(T item);

      [Template("System.Array.insert({this}, {index}, {item}, {T})")]
      extern void IList<T>.Insert(int index, T item);

      [Template("System.Array.removeAt({this}, {index}, {T})")]
      extern void IList<T>.RemoveAt(int index);

      extern T IList<T>.this[int index] { [Template("System.Array.getItem({this}, {0}, {T})")] get; [Template("System.Array.setItem({this}, {0}, {T})")] set; }

      [Template("System.Array.add({this}, {item}, {T})")]
      extern void ICollection<T>.Add(T item);

      [Template("System.Array.copyTo({this}, {array}, {arrayIndex}, {T})")]
      extern void ICollection<T>.CopyTo(T[] array, int arrayIndex);

      [Template("System.Array.clear({this}, {T})")]
      extern void ICollection<T>.Clear();

      [Template("System.Array.contains({this}, {item}, {T})")]
      extern bool ICollection<T>.Contains(T item);

      [Template("System.Array.remove({this}, {item}, {T})")]
      extern bool ICollection<T>.Remove(T item);

      int ICollection<T>.Count
      {
        [Template("System.Array.getCount({this}, {T})")] get
        {
          return this._System\u002ECollections\u002EGeneric\u002EICollection_T\u003E\u002ECount_BackingField;
        }
      }

      bool ICollection<T>.IsReadOnly
      {
        [Template("System.Array.getIsReadOnly({this}, {T})")] get
        {
          return this._System\u002ECollections\u002EGeneric\u002EICollection_T\u003E\u002EIsReadOnly_BackingField;
        }
      }

      [Template("Bridge.getEnumerator({this}, {T})")]
      extern IEnumerator<T> IEnumerable<T>.GetEnumerator();

      [Template("Bridge.getEnumerator({this})")]
      extern IEnumerator IEnumerable.GetEnumerator();

      extern T IReadOnlyList<T>.this[int index] { [Template("System.Array.getItem({this}, {0}, {T})")] get; }

      int IReadOnlyCollection<T>.Count
      {
        [Template("System.Array.getCount({this}, {T})")] get
        {
          return this._System\u002ECollections\u002EGeneric\u002EIReadOnlyCollection_T\u003E\u002ECount_BackingField;
        }
      }

      [Template("System.Array.copyTo({this}, {array}, {arrayIndex})")]
      extern void ICollection.CopyTo(Array array, int arrayIndex);

      int ICollection.Count
      {
        [Template("System.Array.getCount({this})")] get
        {
          return this._System\u002ECollections\u002EICollection\u002ECount_BackingField;
        }
      }

      object ICollection.SyncRoot
      {
        get
        {
          return this._System\u002ECollections\u002EICollection\u002ESyncRoot_BackingField;
        }
      }

      bool ICollection.IsSynchronized
      {
        get
        {
          return this._System\u002ECollections\u002EICollection\u002EIsSynchronized_BackingField;
        }
      }

      [Generated]
      public delegate double sortFn(T a, T b);

      [Generated]
      public delegate bool everyFn(T value, double index, T[] array);

      [Generated]
      public delegate bool someFn(T value, double index, T[] array);

      [Generated]
      public delegate void forEachFn(T value, double index, T[] array);

      [Generated]
      public delegate U mapFn<U>(T value, double index, T[] array);

      [Generated]
      [Where("S", new string[] {"T"}, EnableImplicitConversion = true)]
      public delegate bool filterFn<S>(T value, double index, T[] array);

      [Generated]
      public delegate object filterFn2(T value, double index, T[] array);

      [Generated]
      public delegate T reduceFn(T previousValue, T currentValue, double currentIndex, T[] array);

      [Generated]
      public delegate U reduceFn2<U>(
        U previousValue,
        T currentValue,
        double currentIndex,
        T[] array);

      [Generated]
      public delegate T reduceRightFn(
        T previousValue,
        T currentValue,
        double currentIndex,
        T[] array);

      [Generated]
      public delegate U reduceRightFn2<U>(
        U previousValue,
        T currentValue,
        double currentIndex,
        T[] array);
    }

    [IgnoreCast]
    public interface ArrayConstructor : IObject
    {
      [Template("new {this}()")]
      object[] New();

      [Template("new {this}({0})")]
      object[] New(double arrayLength);

      [Template("new {this}({0})")]
      T[] New<T>(double arrayLength);

      [Template("new {this}({0})")]
      T[] New<T>(params T[] items);

      [Template("{this}()")]
      object[] Self();

      [Template("{this}({0})")]
      object[] Self(double arrayLength);

      [Template("{this}({0})")]
      T[] Self<T>(double arrayLength);

      [Template("{this}({0})")]
      T[] Self<T>(params T[] items);

      bool isArray(object arg);

      es5.Array<object> prototype { get; }
    }

    [IgnoreCast]
    [IgnoreGeneric(AllowInTypeScript = true)]
    [ObjectLiteral]
    [FormerInterface]
    public class TypedPropertyDescriptor<T> : IObject
    {
      private bool? _enumerable_BackingField;
      private bool? _configurable_BackingField;
      private bool? _writable_BackingField;
      private T _value_BackingField;
      private Func<T> _get_BackingField;
      private es5.TypedPropertyDescriptor<T>.setFn _set_BackingField;

      public bool? enumerable
      {
        get
        {
          return this._enumerable_BackingField;
        }
        set
        {
          this._enumerable_BackingField = value;
        }
      }

      public bool? configurable
      {
        get
        {
          return this._configurable_BackingField;
        }
        set
        {
          this._configurable_BackingField = value;
        }
      }

      public bool? writable
      {
        get
        {
          return this._writable_BackingField;
        }
        set
        {
          this._writable_BackingField = value;
        }
      }

      public T value
      {
        get
        {
          return this._value_BackingField;
        }
        set
        {
          this._value_BackingField = value;
        }
      }

      public Func<T> get
      {
        get
        {
          return this._get_BackingField;
        }
        set
        {
          this._get_BackingField = value;
        }
      }

      public es5.TypedPropertyDescriptor<T>.setFn set
      {
        get
        {
          return this._set_BackingField;
        }
        set
        {
          this._set_BackingField = value;
        }
      }

      [Generated]
      public delegate void setFn(T value);
    }

    [IgnoreCast]
    [IgnoreGeneric(AllowInTypeScript = true)]
    [Virtual]
    [FormerInterface]
    public class PromiseLike<T>
    {
      public extern es5.Promise<T> then();

      public extern es5.Promise<H5.Primitive.Void> then(Action<T> onfulfilled);

      public extern es5.Promise<TResult> then<TResult>(Func<T, TResult> onfulfilled);

      public extern es5.Promise<TResult> then<TResult>(Func<T, es5.Promise<TResult>> onfulfilled);

      public extern es5.Promise<TResult> then<TResult>(
        Func<T, es5.PromiseLike<TResult>> onfulfilled);

      public extern es5.Promise<Union<TResult1, TResult2>> then<TResult1, TResult2>(
        Func<T, TResult1> onfulfilled,
        Func<object, TResult2> onrejected);

      public extern es5.Promise<Union<TResult1, TResult2>> then<TResult1, TResult2>(
        Func<T, es5.Promise<TResult1>> onfulfilled,
        Func<object, es5.Promise<TResult2>> onrejected);

      public extern es5.Promise<Union<TResult1, TResult2>> then<TResult1, TResult2>(
        Func<T, es5.PromiseLike<TResult1>> onfulfilled,
        Func<object, es5.PromiseLike<TResult2>> onrejected);

      [Template("System.Threading.Tasks.Task.fromPromise({this})")]
      public extern Task<T> ToTask();
    }

    [IgnoreCast]
    [IgnoreGeneric(AllowInTypeScript = true)]
    [Virtual]
    [FormerInterface]
    public class Promise<T>
    {
      public extern es5.Promise<T> then();

      public extern es5.Promise<H5.Primitive.Void> then(Action<T> onfulfilled);

      public extern es5.Promise<TResult> then<TResult>(Func<T, TResult> onfulfilled);

      public extern es5.Promise<TResult> then<TResult>(Func<T, es5.Promise<TResult>> onfulfilled);

      public extern es5.Promise<TResult> then<TResult>(
        Func<T, es5.PromiseLike<TResult>> onfulfilled);

      public extern es5.Promise<Union<TResult1, TResult2>> then<TResult1, TResult2>(
        Func<T, TResult1> onfulfilled,
        Func<object, TResult2> onrejected);

      public extern es5.Promise<Union<TResult1, TResult2>> then<TResult1, TResult2>(
        Func<T, es5.Promise<TResult1>> onfulfilled,
        Func<object, es5.Promise<TResult2>> onrejected);

      public extern es5.Promise<Union<TResult1, TResult2>> then<TResult1, TResult2>(
        Func<T, es5.PromiseLike<TResult1>> onfulfilled,
        Func<object, es5.PromiseLike<TResult2>> onrejected);

      [Name("catch")]
      public extern es5.Promise<T> @catch();

      [Name("catch")]
      public extern es5.Promise<Union<T, H5.Primitive.Void>> @catch(
        Action<object> onrejected);

      [Name("catch")]
      public extern es5.Promise<T> @catch(Func<object, T> onrejected);

      [Name("catch")]
      public extern es5.Promise<T> @catch(Func<object, es5.Promise<T>> onrejected);

      [Name("catch")]
      public extern es5.Promise<T> @catch(Func<object, es5.PromiseLike<T>> onrejected);

      [Name("catch")]
      public extern es5.Promise<Union<T, TResult>> @catch<TResult>(
        Func<object, TResult> onrejected);

      [Name("catch")]
      public extern es5.Promise<Union<T, TResult>> @catch<TResult>(
        Func<object, es5.Promise<TResult>> onrejected);

      [Name("catch")]
      public extern es5.Promise<Union<T, TResult>> @catch<TResult>(
        Func<object, es5.PromiseLike<TResult>> onrejected);

      [Template("System.Threading.Tasks.Task.fromPromise({this})")]
      public extern Task<T> ToTask();

      [Template("{0}")]
      public static extern implicit operator es5.PromiseLike<T>(es5.Promise<T> promise);
    }

    [IgnoreCast]
    [IgnoreGeneric(AllowInTypeScript = true)]
    [Virtual]
    [FormerInterface]
    public abstract class ArrayLike<T> : IObject
    {
      public abstract double length { get; }

      public abstract T this[double n] { get; }

      public static extern implicit operator es5.ArrayLike<T>(T[] arr);

      public static extern implicit operator es5.ArrayLike<T>(es5.Array<T> arr);

      [Template("{0}")]
      public static extern es5.ArrayLike<T> From(T[] arr);

      [Template("{0}")]
      public static extern es5.ArrayLike<T> From(es5.Array<T> arr);
    }

    [IgnoreCast]
    [IgnoreGeneric(AllowInTypeScript = true)]
    [ObjectLiteral]
    [FormerInterface]
    public class ThisType<T> : IObject
    {
    }

    [CombinedClass]
    [StaticInterface("ArrayBufferConstructor")]
    [FormerInterface]
    public class ArrayBuffer : IObject
    {
      private static readonly es5.ArrayBuffer _prototype_BackingField;
      private readonly uint _byteLength_BackingField;

      public extern ArrayBuffer(double byteLength);

      public static es5.ArrayBuffer prototype
      {
        get
        {
          return es5.ArrayBuffer._prototype_BackingField;
        }
      }

      public static extern bool isView(object arg);

      public virtual uint byteLength
      {
        get
        {
          return this._byteLength_BackingField;
        }
      }

      public virtual extern es5.ArrayBuffer slice(int begin);

      public virtual extern es5.ArrayBuffer slice(int begin, int end);
    }

    [IgnoreCast]
    [ObjectLiteral]
    [FormerInterface]
    public class ArrayBufferTypes : IObject
    {
      private es5.ArrayBuffer _ArrayBuffer_BackingField;

      public es5.ArrayBuffer ArrayBuffer
      {
        get
        {
          return this._ArrayBuffer_BackingField;
        }
        set
        {
          this._ArrayBuffer_BackingField = value;
        }
      }
    }

    [IgnoreCast]
    public interface ArrayBufferConstructor : IObject
    {
      es5.ArrayBuffer prototype { get; }

      [Template("new {this}({0})")]
      es5.ArrayBuffer New(double byteLength);

      bool isView(object arg);
    }

    [IgnoreCast]
    [ObjectLiteral]
    [FormerInterface]
    public class ArrayBufferView : IObject
    {
      private es5.ArrayBufferLike _buffer_BackingField;
      private uint _byteLength_BackingField;
      private uint _byteOffset_BackingField;

      public es5.ArrayBufferLike buffer
      {
        get
        {
          return this._buffer_BackingField;
        }
        set
        {
          this._buffer_BackingField = value;
        }
      }

      public uint byteLength
      {
        get
        {
          return this._byteLength_BackingField;
        }
        set
        {
          this._byteLength_BackingField = value;
        }
      }

      public uint byteOffset
      {
        get
        {
          return this._byteOffset_BackingField;
        }
        set
        {
          this._byteOffset_BackingField = value;
        }
      }
    }

    [CombinedClass]
    [StaticInterface("DataViewConstructor")]
    [FormerInterface]
    public class DataView : IObject
    {
      private readonly es5.ArrayBuffer _buffer_BackingField;
      private readonly uint _byteLength_BackingField;
      private readonly uint _byteOffset_BackingField;

      public extern DataView(es5.ArrayBufferLike buffer);

      public extern DataView(es5.ArrayBuffer buffer);

      public extern DataView(es5.ArrayBufferLike buffer, double byteOffset);

      public extern DataView(es5.ArrayBuffer buffer, double byteOffset);

      public extern DataView(es5.ArrayBufferLike buffer, double byteOffset, double byteLength);

      public extern DataView(es5.ArrayBuffer buffer, double byteOffset, double byteLength);

      public virtual es5.ArrayBuffer buffer
      {
        get
        {
          return this._buffer_BackingField;
        }
      }

      public virtual uint byteLength
      {
        get
        {
          return this._byteLength_BackingField;
        }
      }

      public virtual uint byteOffset
      {
        get
        {
          return this._byteOffset_BackingField;
        }
      }

      public virtual extern float getFloat32(uint byteOffset);

      public virtual extern float getFloat32(uint byteOffset, bool littleEndian);

      public virtual extern double getFloat64(uint byteOffset);

      public virtual extern double getFloat64(uint byteOffset, bool littleEndian);

      public virtual extern sbyte getInt8(uint byteOffset);

      public virtual extern short getInt16(uint byteOffset);

      public virtual extern short getInt16(uint byteOffset, bool littleEndian);

      public virtual extern int getInt32(uint byteOffset);

      public virtual extern int getInt32(uint byteOffset, bool littleEndian);

      public virtual extern byte getUint8(uint byteOffset);

      public virtual extern ushort getUint16(uint byteOffset);

      public virtual extern ushort getUint16(uint byteOffset, bool littleEndian);

      public virtual extern uint getUint32(uint byteOffset);

      public virtual extern uint getUint32(uint byteOffset, bool littleEndian);

      public virtual extern void setFloat32(uint byteOffset, float value);

      public virtual extern void setFloat32(uint byteOffset, float value, bool littleEndian);

      public virtual extern void setFloat64(uint byteOffset, double value);

      public virtual extern void setFloat64(uint byteOffset, double value, bool littleEndian);

      public virtual extern void setInt8(uint byteOffset, sbyte value);

      public virtual extern void setInt16(uint byteOffset, short value);

      public virtual extern void setInt16(uint byteOffset, short value, bool littleEndian);

      public virtual extern void setInt32(uint byteOffset, int value);

      public virtual extern void setInt32(uint byteOffset, int value, bool littleEndian);

      public virtual extern void setUint8(uint byteOffset, byte value);

      public virtual extern void setUint16(uint byteOffset, ushort value);

      public virtual extern void setUint16(uint byteOffset, ushort value, bool littleEndian);

      public virtual extern void setUint32(uint byteOffset, uint value);

      public virtual extern void setUint32(uint byteOffset, uint value, bool littleEndian);
    }

    [IgnoreCast]
    public interface DataViewConstructor : IObject
    {
      [Template("new {this}({0})")]
      es5.DataView New(es5.ArrayBufferLike buffer);

      [Template("new {this}({0})")]
      es5.DataView New(es5.ArrayBuffer buffer);

      [Template("new {this}({0}, {1})")]
      es5.DataView New(es5.ArrayBufferLike buffer, double byteOffset);

      [Template("new {this}({0}, {1})")]
      es5.DataView New(es5.ArrayBuffer buffer, double byteOffset);

      [Template("new {this}({0}, {1}, {2})")]
      es5.DataView New(es5.ArrayBufferLike buffer, double byteOffset, double byteLength);

      [Template("new {this}({0}, {1}, {2})")]
      es5.DataView New(es5.ArrayBuffer buffer, double byteOffset, double byteLength);
    }

    [CombinedClass]
    [StaticInterface("Int8ArrayConstructor")]
    [FormerInterface]
    public class Int8Array : IList<sbyte>, ICollection<sbyte>, IEnumerable<sbyte>, IEnumerable, IBridgeClass, IReadOnlyList<sbyte>, IReadOnlyCollection<sbyte>, ICollection, IObject
    {
      private static readonly es5.Int8Array _prototype_BackingField;
      private static readonly double _BYTES_PER_ELEMENT_Static_BackingField;
      private readonly int _BYTES_PER_ELEMENT_BackingField;
      private readonly es5.ArrayBufferLike _buffer_BackingField;
      private readonly uint _byteLength_BackingField;
      private readonly uint _byteOffset_BackingField;
      private readonly uint _length_BackingField;
      private readonly int _System\u002ECollections\u002EGeneric\u002EICollection_System\u002ESByte\u003E\u002ECount_BackingField;
      private readonly bool _System\u002ECollections\u002EGeneric\u002EICollection_System\u002ESByte\u003E\u002EIsReadOnly_BackingField;
      private readonly int _System\u002ECollections\u002EGeneric\u002EIReadOnlyCollection_System\u002ESByte\u003E\u002ECount_BackingField;
      private readonly int _System\u002ECollections\u002EICollection\u002ECount_BackingField;
      private readonly object _System\u002ECollections\u002EICollection\u002ESyncRoot_BackingField;
      private readonly bool _System\u002ECollections\u002EICollection\u002EIsSynchronized_BackingField;

      public extern Int8Array(uint length);

      public extern Int8Array(
        Union<es5.ArrayLike<sbyte>, es5.ArrayBufferLike> arrayOrArrayBuffer);

      public extern Int8Array(es5.ArrayLike<sbyte> arrayOrArrayBuffer);

      public extern Int8Array(es5.ArrayBufferLike arrayOrArrayBuffer);

      public extern Int8Array(es5.ArrayBuffer arrayOrArrayBuffer);

      public extern Int8Array(es5.ArrayBufferLike buffer, uint byteOffset);

      public extern Int8Array(es5.ArrayBuffer buffer, uint byteOffset);

      public extern Int8Array(es5.ArrayBufferLike buffer, uint byteOffset, uint length);

      public extern Int8Array(es5.ArrayBuffer buffer, uint byteOffset, uint length);

      public static es5.Int8Array prototype
      {
        get
        {
          return es5.Int8Array._prototype_BackingField;
        }
      }

      [Name("BYTES_PER_ELEMENT")]
      public static double BYTES_PER_ELEMENT_Static
      {
        get
        {
          return es5.Int8Array._BYTES_PER_ELEMENT_Static_BackingField;
        }
      }

      [ExpandParams]
      public static extern es5.Int8Array of(params sbyte[] items);

      public static extern es5.Int8Array from(es5.ArrayLike<sbyte> arrayLike);

      public static extern es5.Int8Array from(
        es5.ArrayLike<sbyte> arrayLike,
        es5.Int8Array.fromFn mapfn);

      public static extern es5.Int8Array from(
        es5.ArrayLike<sbyte> arrayLike,
        es5.Int8Array.fromFn mapfn,
        object thisArg);

      public virtual int BYTES_PER_ELEMENT
      {
        get
        {
          return this._BYTES_PER_ELEMENT_BackingField;
        }
      }

      public virtual es5.ArrayBufferLike buffer
      {
        get
        {
          return this._buffer_BackingField;
        }
      }

      public virtual uint byteLength
      {
        get
        {
          return this._byteLength_BackingField;
        }
      }

      public virtual uint byteOffset
      {
        get
        {
          return this._byteOffset_BackingField;
        }
      }

      public virtual extern es5.Int8Array copyWithin(long target, long start);

      public virtual extern es5.Int8Array copyWithin(long target, long start, long end);

      public virtual extern bool every(es5.Int8Array.everyFn callbackfn);

      public virtual extern bool every(es5.Int8Array.everyFn callbackfn, object thisArg);

      public virtual extern es5.Int8Array fill(sbyte value);

      public virtual extern es5.Int8Array fill(sbyte value, uint start);

      public virtual extern es5.Int8Array fill(sbyte value, uint start, uint end);

      public virtual extern es5.Int8Array filter(es5.Int8Array.filterFn callbackfn);

      public virtual extern es5.Int8Array filter(
        es5.Int8Array.filterFn callbackfn,
        object thisArg);

      public virtual extern Union<sbyte, Undefined> find(
        es5.Int8Array.findFn predicate);

      public virtual extern Union<sbyte, Undefined> find(
        es5.Int8Array.findFn predicate,
        object thisArg);

      public virtual extern uint findIndex(es5.Int8Array.findIndexFn predicate);

      public virtual extern uint findIndex(es5.Int8Array.findIndexFn predicate, object thisArg);

      public virtual extern void forEach(es5.Int8Array.forEachFn callbackfn);

      public virtual extern void forEach(es5.Int8Array.forEachFn callbackfn, object thisArg);

      public virtual extern uint indexOf(sbyte searchElement);

      public virtual extern uint indexOf(sbyte searchElement, uint fromIndex);

      public virtual extern string join();

      public virtual extern string join(string separator);

      public virtual extern uint lastIndexOf(sbyte searchElement);

      public virtual extern uint lastIndexOf(sbyte searchElement, uint fromIndex);

      public virtual uint length
      {
        get
        {
          return this._length_BackingField;
        }
      }

      public virtual extern es5.Int8Array map(es5.Int8Array.mapFn callbackfn);

      public virtual extern es5.Int8Array map(es5.Int8Array.mapFn callbackfn, object thisArg);

      public virtual extern long reduce(es5.Int8Array.reduceFn callbackfn);

      public virtual extern long reduce(es5.Int8Array.reduceFn callbackfn, long initialValue);

      public virtual extern U reduce<U>(es5.Int8Array.reduceFn2<U> callbackfn, U initialValue);

      public virtual extern long reduceRight(es5.Int8Array.reduceRightFn callbackfn);

      public virtual extern long reduceRight(
        es5.Int8Array.reduceRightFn callbackfn,
        long initialValue);

      public virtual extern U reduceRight<U>(
        es5.Int8Array.reduceRightFn2<U> callbackfn,
        U initialValue);

      public virtual extern es5.Int8Array reverse();

      public virtual extern void set(es5.ArrayLike<sbyte> array);

      public virtual extern void set(es5.ArrayLike<sbyte> array, uint offset);

      public virtual extern es5.Int8Array slice();

      public virtual extern es5.Int8Array slice(uint start);

      public virtual extern es5.Int8Array slice(uint start, uint end);

      public virtual extern bool some(es5.Int8Array.someFn callbackfn);

      public virtual extern bool some(es5.Int8Array.someFn callbackfn, object thisArg);

      public virtual extern es5.Int8Array sort();

      public virtual extern es5.Int8Array sort(es5.Int8Array.sortFn compareFn);

      public virtual extern es5.Int8Array subarray(uint begin);

      public virtual extern es5.Int8Array subarray(uint begin, int end);

      public virtual extern string toLocaleString();

      public virtual extern string toString();

      public virtual extern sbyte this[uint index] { get; set; }

      [Template("System.Array.indexOf({this}, {item}, 0, null, {T})")]
      extern int IList<sbyte>.IndexOf(sbyte item);

      [Template("System.Array.insert({this}, {index}, {item}, {T})")]
      extern void IList<sbyte>.Insert(int index, sbyte item);

      [Template("System.Array.removeAt({this}, {index}, {T})")]
      extern void IList<sbyte>.RemoveAt(int index);

      extern sbyte IList<sbyte>.this[int index] { [Template("System.Array.getItem({this}, {0}, {T})")] get; [Template("System.Array.setItem({this}, {0}, {T})")] set; }

      [Template("System.Array.add({this}, {item}, {T})")]
      extern void ICollection<sbyte>.Add(sbyte item);

      [Template("System.Array.copyTo({this}, {array}, {arrayIndex}, {T})")]
      extern void ICollection<sbyte>.CopyTo(sbyte[] array, int arrayIndex);

      [Template("System.Array.clear({this}, {T})")]
      extern void ICollection<sbyte>.Clear();

      [Template("System.Array.contains({this}, {item}, {T})")]
      extern bool ICollection<sbyte>.Contains(sbyte item);

      [Template("System.Array.remove({this}, {item}, {T})")]
      extern bool ICollection<sbyte>.Remove(sbyte item);

      int ICollection<sbyte>.Count
      {
        [Template("System.Array.getCount({this}, {T})")] get
        {
          return this._System\u002ECollections\u002EGeneric\u002EICollection_System\u002ESByte\u003E\u002ECount_BackingField;
        }
      }

      bool ICollection<sbyte>.IsReadOnly
      {
        [Template("System.Array.getIsReadOnly({this}, {T})")] get
        {
          return this._System\u002ECollections\u002EGeneric\u002EICollection_System\u002ESByte\u003E\u002EIsReadOnly_BackingField;
        }
      }

      [Template("Bridge.getEnumerator({this}, {T})")]
      extern IEnumerator<sbyte> IEnumerable<sbyte>.GetEnumerator();

      [Template("Bridge.getEnumerator({this})")]
      extern IEnumerator IEnumerable.GetEnumerator();

      extern sbyte IReadOnlyList<sbyte>.this[int index] { [Template("System.Array.getItem({this}, {0}, {T})")] get; }

      int IReadOnlyCollection<sbyte>.Count
      {
        [Template("System.Array.getCount({this}, {T})")] get
        {
          return this._System\u002ECollections\u002EGeneric\u002EIReadOnlyCollection_System\u002ESByte\u003E\u002ECount_BackingField;
        }
      }

      [Template("System.Array.copyTo({this}, {array}, {arrayIndex})")]
      extern void ICollection.CopyTo(Array array, int arrayIndex);

      int ICollection.Count
      {
        [Template("System.Array.getCount({this})")] get
        {
          return this._System\u002ECollections\u002EICollection\u002ECount_BackingField;
        }
      }

      object ICollection.SyncRoot
      {
        get
        {
          return this._System\u002ECollections\u002EICollection\u002ESyncRoot_BackingField;
        }
      }

      bool ICollection.IsSynchronized
      {
        get
        {
          return this._System\u002ECollections\u002EICollection\u002EIsSynchronized_BackingField;
        }
      }

      [Generated]
      public delegate bool everyFn(sbyte value, uint index, es5.Int8Array array);

      [Generated]
      public delegate object filterFn(sbyte value, uint index, es5.Int8Array array);

      [Generated]
      public delegate bool findFn(sbyte value, uint index, es5.Int8Array obj);

      [Generated]
      public delegate bool findIndexFn(sbyte value, uint index, es5.Int8Array obj);

      [Generated]
      public delegate void forEachFn(sbyte value, uint index, es5.Int8Array array);

      [Generated]
      public delegate double mapFn(sbyte value, uint index, es5.Int8Array array);

      [Generated]
      public delegate double reduceFn(
        long previousValue,
        sbyte currentValue,
        uint currentIndex,
        es5.Int8Array array);

      [Generated]
      public delegate U reduceFn2<U>(
        U previousValue,
        sbyte currentValue,
        uint currentIndex,
        es5.Int8Array array);

      [Generated]
      public delegate double reduceRightFn(
        long previousValue,
        sbyte currentValue,
        uint currentIndex,
        es5.Int8Array array);

      [Generated]
      public delegate U reduceRightFn2<U>(
        U previousValue,
        sbyte currentValue,
        uint currentIndex,
        es5.Int8Array array);

      [Generated]
      public delegate bool someFn(sbyte value, uint index, es5.Int8Array array);

      [Generated]
      public delegate double sortFn(sbyte a, sbyte b);

      [Generated]
      public delegate double fromFn(double v, double k);
    }

    [IgnoreCast]
    [Virtual]
    [FormerInterface]
    public abstract class Int8ArrayConstructor : IObject
    {
      public abstract es5.Int8Array prototype { get; }

      [Template("new {this}({0})")]
      public abstract es5.Int8Array New(uint length);

      [Template("new {this}({0})")]
      public abstract es5.Int8Array New(
        Union<es5.ArrayLike<sbyte>, es5.ArrayBufferLike> arrayOrArrayBuffer);

      [Template("new {this}({0})")]
      public abstract es5.Int8Array New(es5.ArrayLike<sbyte> arrayOrArrayBuffer);

      [Template("new {this}({0})")]
      public abstract es5.Int8Array New(es5.ArrayBufferLike arrayOrArrayBuffer);

      [Template("new {this}({0})")]
      public abstract es5.Int8Array New(es5.ArrayBuffer arrayOrArrayBuffer);

      [Template("new {this}({0}, {1})")]
      public abstract es5.Int8Array New(es5.ArrayBufferLike buffer, uint byteOffset);

      [Template("new {this}({0}, {1})")]
      public abstract es5.Int8Array New(es5.ArrayBuffer buffer, uint byteOffset);

      [Template("new {this}({0}, {1}, {2})")]
      public abstract es5.Int8Array New(
        es5.ArrayBufferLike buffer,
        uint byteOffset,
        uint length);

      [Template("new {this}({0}, {1}, {2})")]
      public abstract es5.Int8Array New(es5.ArrayBuffer buffer, uint byteOffset, uint length);

      public abstract double BYTES_PER_ELEMENT { get; }

      [ExpandParams]
      public abstract es5.Int8Array of(params sbyte[] items);

      public abstract es5.Int8Array from(es5.ArrayLike<sbyte> arrayLike);

      public abstract es5.Int8Array from(
        es5.ArrayLike<sbyte> arrayLike,
        es5.Int8ArrayConstructor.fromFn mapfn);

      public abstract es5.Int8Array from(
        es5.ArrayLike<sbyte> arrayLike,
        es5.Int8ArrayConstructor.fromFn mapfn,
        object thisArg);

      [Generated]
      public delegate double fromFn(double v, double k);
    }

    [CombinedClass]
    [StaticInterface("Uint8ArrayConstructor")]
    [FormerInterface]
    public class Uint8Array : IList<byte>, ICollection<byte>, IEnumerable<byte>, IEnumerable, IBridgeClass, IReadOnlyList<byte>, IReadOnlyCollection<byte>, ICollection, IObject
    {
      private static readonly es5.Uint8Array _prototype_BackingField;
      private static readonly double _BYTES_PER_ELEMENT_Static_BackingField;
      private readonly int _BYTES_PER_ELEMENT_BackingField;
      private readonly es5.ArrayBufferLike _buffer_BackingField;
      private readonly uint _byteLength_BackingField;
      private readonly uint _byteOffset_BackingField;
      private readonly uint _length_BackingField;
      private readonly int _System\u002ECollections\u002EGeneric\u002EICollection_System\u002EByte\u003E\u002ECount_BackingField;
      private readonly bool _System\u002ECollections\u002EGeneric\u002EICollection_System\u002EByte\u003E\u002EIsReadOnly_BackingField;
      private readonly int _System\u002ECollections\u002EGeneric\u002EIReadOnlyCollection_System\u002EByte\u003E\u002ECount_BackingField;
      private readonly int _System\u002ECollections\u002EICollection\u002ECount_BackingField;
      private readonly object _System\u002ECollections\u002EICollection\u002ESyncRoot_BackingField;
      private readonly bool _System\u002ECollections\u002EICollection\u002EIsSynchronized_BackingField;

      public extern Uint8Array(uint length);

      public extern Uint8Array(
        Union<es5.ArrayLike<byte>, es5.ArrayBufferLike> arrayOrArrayBuffer);

      public extern Uint8Array(es5.ArrayLike<byte> arrayOrArrayBuffer);

      public extern Uint8Array(es5.ArrayBufferLike arrayOrArrayBuffer);

      public extern Uint8Array(es5.ArrayBuffer arrayOrArrayBuffer);

      public extern Uint8Array(es5.ArrayBufferLike buffer, uint byteOffset);

      public extern Uint8Array(es5.ArrayBuffer buffer, uint byteOffset);

      public extern Uint8Array(es5.ArrayBufferLike buffer, uint byteOffset, uint length);

      public extern Uint8Array(es5.ArrayBuffer buffer, uint byteOffset, uint length);

      public static es5.Uint8Array prototype
      {
        get
        {
          return es5.Uint8Array._prototype_BackingField;
        }
      }

      [Name("BYTES_PER_ELEMENT")]
      public static double BYTES_PER_ELEMENT_Static
      {
        get
        {
          return es5.Uint8Array._BYTES_PER_ELEMENT_Static_BackingField;
        }
      }

      [ExpandParams]
      public static extern es5.Uint8Array of(params byte[] items);

      public static extern es5.Uint8Array from(es5.ArrayLike<byte> arrayLike);

      public static extern es5.Uint8Array from(
        es5.ArrayLike<byte> arrayLike,
        es5.Uint8Array.fromFn mapfn);

      public static extern es5.Uint8Array from(
        es5.ArrayLike<byte> arrayLike,
        es5.Uint8Array.fromFn mapfn,
        object thisArg);

      public virtual int BYTES_PER_ELEMENT
      {
        get
        {
          return this._BYTES_PER_ELEMENT_BackingField;
        }
      }

      public virtual es5.ArrayBufferLike buffer
      {
        get
        {
          return this._buffer_BackingField;
        }
      }

      public virtual uint byteLength
      {
        get
        {
          return this._byteLength_BackingField;
        }
      }

      public virtual uint byteOffset
      {
        get
        {
          return this._byteOffset_BackingField;
        }
      }

      public virtual extern es5.Uint8Array copyWithin(long target, long start);

      public virtual extern es5.Uint8Array copyWithin(long target, long start, long end);

      public virtual extern bool every(es5.Uint8Array.everyFn callbackfn);

      public virtual extern bool every(es5.Uint8Array.everyFn callbackfn, object thisArg);

      public virtual extern es5.Uint8Array fill(byte value);

      public virtual extern es5.Uint8Array fill(byte value, uint start);

      public virtual extern es5.Uint8Array fill(byte value, uint start, uint end);

      public virtual extern es5.Uint8Array filter(es5.Uint8Array.filterFn callbackfn);

      public virtual extern es5.Uint8Array filter(
        es5.Uint8Array.filterFn callbackfn,
        object thisArg);

      public virtual extern Union<byte, Undefined> find(
        es5.Uint8Array.findFn predicate);

      public virtual extern Union<byte, Undefined> find(
        es5.Uint8Array.findFn predicate,
        object thisArg);

      public virtual extern uint findIndex(es5.Uint8Array.findIndexFn predicate);

      public virtual extern uint findIndex(es5.Uint8Array.findIndexFn predicate, object thisArg);

      public virtual extern void forEach(es5.Uint8Array.forEachFn callbackfn);

      public virtual extern void forEach(es5.Uint8Array.forEachFn callbackfn, object thisArg);

      public virtual extern uint indexOf(byte searchElement);

      public virtual extern uint indexOf(byte searchElement, uint fromIndex);

      public virtual extern string join();

      public virtual extern string join(string separator);

      public virtual extern uint lastIndexOf(byte searchElement);

      public virtual extern uint lastIndexOf(byte searchElement, uint fromIndex);

      public virtual uint length
      {
        get
        {
          return this._length_BackingField;
        }
      }

      public virtual extern es5.Uint8Array map(es5.Uint8Array.mapFn callbackfn);

      public virtual extern es5.Uint8Array map(es5.Uint8Array.mapFn callbackfn, object thisArg);

      public virtual extern long reduce(es5.Uint8Array.reduceFn callbackfn);

      public virtual extern long reduce(es5.Uint8Array.reduceFn callbackfn, long initialValue);

      public virtual extern U reduce<U>(es5.Uint8Array.reduceFn2<U> callbackfn, U initialValue);

      public virtual extern long reduceRight(es5.Uint8Array.reduceRightFn callbackfn);

      public virtual extern long reduceRight(
        es5.Uint8Array.reduceRightFn callbackfn,
        long initialValue);

      public virtual extern U reduceRight<U>(
        es5.Uint8Array.reduceRightFn2<U> callbackfn,
        U initialValue);

      public virtual extern es5.Uint8Array reverse();

      public virtual extern void set(es5.ArrayLike<byte> array);

      public virtual extern void set(es5.ArrayLike<byte> array, uint offset);

      public virtual extern es5.Uint8Array slice();

      public virtual extern es5.Uint8Array slice(uint start);

      public virtual extern es5.Uint8Array slice(uint start, uint end);

      public virtual extern bool some(es5.Uint8Array.someFn callbackfn);

      public virtual extern bool some(es5.Uint8Array.someFn callbackfn, object thisArg);

      public virtual extern es5.Uint8Array sort();

      public virtual extern es5.Uint8Array sort(es5.Uint8Array.sortFn compareFn);

      public virtual extern es5.Uint8Array subarray(uint begin);

      public virtual extern es5.Uint8Array subarray(uint begin, int end);

      public virtual extern string toLocaleString();

      public virtual extern string toString();

      public virtual extern byte this[uint index] { get; set; }

      [Template("System.Array.indexOf({this}, {item}, 0, null, {T})")]
      extern int IList<byte>.IndexOf(byte item);

      [Template("System.Array.insert({this}, {index}, {item}, {T})")]
      extern void IList<byte>.Insert(int index, byte item);

      [Template("System.Array.removeAt({this}, {index}, {T})")]
      extern void IList<byte>.RemoveAt(int index);

      extern byte IList<byte>.this[int index] { [Template("System.Array.getItem({this}, {0}, {T})")] get; [Template("System.Array.setItem({this}, {0}, {T})")] set; }

      [Template("System.Array.add({this}, {item}, {T})")]
      extern void ICollection<byte>.Add(byte item);

      [Template("System.Array.copyTo({this}, {array}, {arrayIndex}, {T})")]
      extern void ICollection<byte>.CopyTo(byte[] array, int arrayIndex);

      [Template("System.Array.clear({this}, {T})")]
      extern void ICollection<byte>.Clear();

      [Template("System.Array.contains({this}, {item}, {T})")]
      extern bool ICollection<byte>.Contains(byte item);

      [Template("System.Array.remove({this}, {item}, {T})")]
      extern bool ICollection<byte>.Remove(byte item);

      int ICollection<byte>.Count
      {
        [Template("System.Array.getCount({this}, {T})")] get
        {
          return this._System\u002ECollections\u002EGeneric\u002EICollection_System\u002EByte\u003E\u002ECount_BackingField;
        }
      }

      bool ICollection<byte>.IsReadOnly
      {
        [Template("System.Array.getIsReadOnly({this}, {T})")] get
        {
          return this._System\u002ECollections\u002EGeneric\u002EICollection_System\u002EByte\u003E\u002EIsReadOnly_BackingField;
        }
      }

      [Template("Bridge.getEnumerator({this}, {T})")]
      extern IEnumerator<byte> IEnumerable<byte>.GetEnumerator();

      [Template("Bridge.getEnumerator({this})")]
      extern IEnumerator IEnumerable.GetEnumerator();

      extern byte IReadOnlyList<byte>.this[int index] { [Template("System.Array.getItem({this}, {0}, {T})")] get; }

      int IReadOnlyCollection<byte>.Count
      {
        [Template("System.Array.getCount({this}, {T})")] get
        {
          return this._System\u002ECollections\u002EGeneric\u002EIReadOnlyCollection_System\u002EByte\u003E\u002ECount_BackingField;
        }
      }

      [Template("System.Array.copyTo({this}, {array}, {arrayIndex})")]
      extern void ICollection.CopyTo(Array array, int arrayIndex);

      int ICollection.Count
      {
        [Template("System.Array.getCount({this})")] get
        {
          return this._System\u002ECollections\u002EICollection\u002ECount_BackingField;
        }
      }

      object ICollection.SyncRoot
      {
        get
        {
          return this._System\u002ECollections\u002EICollection\u002ESyncRoot_BackingField;
        }
      }

      bool ICollection.IsSynchronized
      {
        get
        {
          return this._System\u002ECollections\u002EICollection\u002EIsSynchronized_BackingField;
        }
      }

      [Generated]
      public delegate bool everyFn(byte value, uint index, es5.Uint8Array array);

      [Generated]
      public delegate object filterFn(byte value, uint index, es5.Uint8Array array);

      [Generated]
      public delegate bool findFn(byte value, uint index, es5.Uint8Array obj);

      [Generated]
      public delegate bool findIndexFn(byte value, uint index, es5.Uint8Array obj);

      [Generated]
      public delegate void forEachFn(byte value, uint index, es5.Uint8Array array);

      [Generated]
      public delegate double mapFn(byte value, uint index, es5.Uint8Array array);

      [Generated]
      public delegate double reduceFn(
        long previousValue,
        byte currentValue,
        uint currentIndex,
        es5.Uint8Array array);

      [Generated]
      public delegate U reduceFn2<U>(
        U previousValue,
        byte currentValue,
        uint currentIndex,
        es5.Uint8Array array);

      [Generated]
      public delegate double reduceRightFn(
        long previousValue,
        byte currentValue,
        uint currentIndex,
        es5.Uint8Array array);

      [Generated]
      public delegate U reduceRightFn2<U>(
        U previousValue,
        byte currentValue,
        uint currentIndex,
        es5.Uint8Array array);

      [Generated]
      public delegate bool someFn(byte value, uint index, es5.Uint8Array array);

      [Generated]
      public delegate double sortFn(byte a, byte b);

      [Generated]
      public delegate double fromFn(double v, double k);
    }

    [IgnoreCast]
    [Virtual]
    [FormerInterface]
    public abstract class Uint8ArrayConstructor : IObject
    {
      public abstract es5.Uint8Array prototype { get; }

      [Template("new {this}({0})")]
      public abstract es5.Uint8Array New(uint length);

      [Template("new {this}({0})")]
      public abstract es5.Uint8Array New(
        Union<es5.ArrayLike<byte>, es5.ArrayBufferLike> arrayOrArrayBuffer);

      [Template("new {this}({0})")]
      public abstract es5.Uint8Array New(es5.ArrayLike<byte> arrayOrArrayBuffer);

      [Template("new {this}({0})")]
      public abstract es5.Uint8Array New(es5.ArrayBufferLike arrayOrArrayBuffer);

      [Template("new {this}({0})")]
      public abstract es5.Uint8Array New(es5.ArrayBuffer arrayOrArrayBuffer);

      [Template("new {this}({0}, {1})")]
      public abstract es5.Uint8Array New(es5.ArrayBufferLike buffer, uint byteOffset);

      [Template("new {this}({0}, {1})")]
      public abstract es5.Uint8Array New(es5.ArrayBuffer buffer, uint byteOffset);

      [Template("new {this}({0}, {1}, {2})")]
      public abstract es5.Uint8Array New(
        es5.ArrayBufferLike buffer,
        uint byteOffset,
        uint length);

      [Template("new {this}({0}, {1}, {2})")]
      public abstract es5.Uint8Array New(es5.ArrayBuffer buffer, uint byteOffset, uint length);

      public abstract double BYTES_PER_ELEMENT { get; }

      [ExpandParams]
      public abstract es5.Uint8Array of(params byte[] items);

      public abstract es5.Uint8Array from(es5.ArrayLike<byte> arrayLike);

      public abstract es5.Uint8Array from(
        es5.ArrayLike<byte> arrayLike,
        es5.Uint8ArrayConstructor.fromFn mapfn);

      public abstract es5.Uint8Array from(
        es5.ArrayLike<byte> arrayLike,
        es5.Uint8ArrayConstructor.fromFn mapfn,
        object thisArg);

      [Generated]
      public delegate double fromFn(double v, double k);
    }

    [CombinedClass]
    [StaticInterface("Uint8ClampedArrayConstructor")]
    [FormerInterface]
    public class Uint8ClampedArray : IList<byte>, ICollection<byte>, IEnumerable<byte>, IEnumerable, IBridgeClass, IReadOnlyList<byte>, IReadOnlyCollection<byte>, ICollection, IObject
    {
      private static readonly es5.Uint8ClampedArray _prototype_BackingField;
      private static readonly double _BYTES_PER_ELEMENT_Static_BackingField;
      private readonly int _BYTES_PER_ELEMENT_BackingField;
      private readonly es5.ArrayBufferLike _buffer_BackingField;
      private readonly uint _byteLength_BackingField;
      private readonly uint _byteOffset_BackingField;
      private readonly uint _length_BackingField;
      private readonly int _System\u002ECollections\u002EGeneric\u002EICollection_System\u002EByte\u003E\u002ECount_BackingField;
      private readonly bool _System\u002ECollections\u002EGeneric\u002EICollection_System\u002EByte\u003E\u002EIsReadOnly_BackingField;
      private readonly int _System\u002ECollections\u002EGeneric\u002EIReadOnlyCollection_System\u002EByte\u003E\u002ECount_BackingField;
      private readonly int _System\u002ECollections\u002EICollection\u002ECount_BackingField;
      private readonly object _System\u002ECollections\u002EICollection\u002ESyncRoot_BackingField;
      private readonly bool _System\u002ECollections\u002EICollection\u002EIsSynchronized_BackingField;

      public extern Uint8ClampedArray(uint length);

      public extern Uint8ClampedArray(
        Union<es5.ArrayLike<byte>, es5.ArrayBufferLike> arrayOrArrayBuffer);

      public extern Uint8ClampedArray(es5.ArrayLike<byte> arrayOrArrayBuffer);

      public extern Uint8ClampedArray(es5.ArrayBufferLike arrayOrArrayBuffer);

      public extern Uint8ClampedArray(es5.ArrayBuffer arrayOrArrayBuffer);

      public extern Uint8ClampedArray(es5.ArrayBufferLike buffer, uint byteOffset);

      public extern Uint8ClampedArray(es5.ArrayBuffer buffer, uint byteOffset);

      public extern Uint8ClampedArray(es5.ArrayBufferLike buffer, uint byteOffset, uint length);

      public extern Uint8ClampedArray(es5.ArrayBuffer buffer, uint byteOffset, uint length);

      public static es5.Uint8ClampedArray prototype
      {
        get
        {
          return es5.Uint8ClampedArray._prototype_BackingField;
        }
      }

      [Name("BYTES_PER_ELEMENT")]
      public static double BYTES_PER_ELEMENT_Static
      {
        get
        {
          return es5.Uint8ClampedArray._BYTES_PER_ELEMENT_Static_BackingField;
        }
      }

      [ExpandParams]
      public static extern es5.Uint8ClampedArray of(params byte[] items);

      public static extern es5.Uint8ClampedArray from(es5.ArrayLike<byte> arrayLike);

      public static extern es5.Uint8ClampedArray from(
        es5.ArrayLike<byte> arrayLike,
        es5.Uint8ClampedArray.fromFn mapfn);

      public static extern es5.Uint8ClampedArray from(
        es5.ArrayLike<byte> arrayLike,
        es5.Uint8ClampedArray.fromFn mapfn,
        object thisArg);

      public virtual int BYTES_PER_ELEMENT
      {
        get
        {
          return this._BYTES_PER_ELEMENT_BackingField;
        }
      }

      public virtual es5.ArrayBufferLike buffer
      {
        get
        {
          return this._buffer_BackingField;
        }
      }

      public virtual uint byteLength
      {
        get
        {
          return this._byteLength_BackingField;
        }
      }

      public virtual uint byteOffset
      {
        get
        {
          return this._byteOffset_BackingField;
        }
      }

      public virtual extern es5.Uint8ClampedArray copyWithin(long target, long start);

      public virtual extern es5.Uint8ClampedArray copyWithin(long target, long start, long end);

      public virtual extern bool every(es5.Uint8ClampedArray.everyFn callbackfn);

      public virtual extern bool every(es5.Uint8ClampedArray.everyFn callbackfn, object thisArg);

      public virtual extern es5.Uint8ClampedArray fill(byte value);

      public virtual extern es5.Uint8ClampedArray fill(byte value, uint start);

      public virtual extern es5.Uint8ClampedArray fill(byte value, uint start, uint end);

      public virtual extern es5.Uint8ClampedArray filter(
        es5.Uint8ClampedArray.filterFn callbackfn);

      public virtual extern es5.Uint8ClampedArray filter(
        es5.Uint8ClampedArray.filterFn callbackfn,
        object thisArg);

      public virtual extern Union<byte, Undefined> find(
        es5.Uint8ClampedArray.findFn predicate);

      public virtual extern Union<byte, Undefined> find(
        es5.Uint8ClampedArray.findFn predicate,
        object thisArg);

      public virtual extern uint findIndex(es5.Uint8ClampedArray.findIndexFn predicate);

      public virtual extern uint findIndex(
        es5.Uint8ClampedArray.findIndexFn predicate,
        object thisArg);

      public virtual extern void forEach(es5.Uint8ClampedArray.forEachFn callbackfn);

      public virtual extern void forEach(es5.Uint8ClampedArray.forEachFn callbackfn, object thisArg);

      public virtual extern uint indexOf(byte searchElement);

      public virtual extern uint indexOf(byte searchElement, uint fromIndex);

      public virtual extern string join();

      public virtual extern string join(string separator);

      public virtual extern uint lastIndexOf(byte searchElement);

      public virtual extern uint lastIndexOf(byte searchElement, uint fromIndex);

      public virtual uint length
      {
        get
        {
          return this._length_BackingField;
        }
      }

      public virtual extern es5.Uint8ClampedArray map(es5.Uint8ClampedArray.mapFn callbackfn);

      public virtual extern es5.Uint8ClampedArray map(
        es5.Uint8ClampedArray.mapFn callbackfn,
        object thisArg);

      public virtual extern long reduce(es5.Uint8ClampedArray.reduceFn callbackfn);

      public virtual extern long reduce(
        es5.Uint8ClampedArray.reduceFn callbackfn,
        long initialValue);

      public virtual extern U reduce<U>(
        es5.Uint8ClampedArray.reduceFn2<U> callbackfn,
        U initialValue);

      public virtual extern long reduceRight(es5.Uint8ClampedArray.reduceRightFn callbackfn);

      public virtual extern long reduceRight(
        es5.Uint8ClampedArray.reduceRightFn callbackfn,
        long initialValue);

      public virtual extern U reduceRight<U>(
        es5.Uint8ClampedArray.reduceRightFn2<U> callbackfn,
        U initialValue);

      public virtual extern es5.Uint8ClampedArray reverse();

      public virtual extern void set(es5.ArrayLike<byte> array);

      public virtual extern void set(es5.ArrayLike<byte> array, uint offset);

      public virtual extern es5.Uint8ClampedArray slice();

      public virtual extern es5.Uint8ClampedArray slice(uint start);

      public virtual extern es5.Uint8ClampedArray slice(uint start, uint end);

      public virtual extern bool some(es5.Uint8ClampedArray.someFn callbackfn);

      public virtual extern bool some(es5.Uint8ClampedArray.someFn callbackfn, object thisArg);

      public virtual extern es5.Uint8ClampedArray sort();

      public virtual extern es5.Uint8ClampedArray sort(es5.Uint8ClampedArray.sortFn compareFn);

      public virtual extern es5.Uint8ClampedArray subarray(uint begin);

      public virtual extern es5.Uint8ClampedArray subarray(uint begin, int end);

      public virtual extern string toLocaleString();

      public virtual extern string toString();

      public virtual extern byte this[uint index] { get; set; }

      [Template("System.Array.indexOf({this}, {item}, 0, null, {T})")]
      extern int IList<byte>.IndexOf(byte item);

      [Template("System.Array.insert({this}, {index}, {item}, {T})")]
      extern void IList<byte>.Insert(int index, byte item);

      [Template("System.Array.removeAt({this}, {index}, {T})")]
      extern void IList<byte>.RemoveAt(int index);

      extern byte IList<byte>.this[int index] { [Template("System.Array.getItem({this}, {0}, {T})")] get; [Template("System.Array.setItem({this}, {0}, {T})")] set; }

      [Template("System.Array.add({this}, {item}, {T})")]
      extern void ICollection<byte>.Add(byte item);

      [Template("System.Array.copyTo({this}, {array}, {arrayIndex}, {T})")]
      extern void ICollection<byte>.CopyTo(byte[] array, int arrayIndex);

      [Template("System.Array.clear({this}, {T})")]
      extern void ICollection<byte>.Clear();

      [Template("System.Array.contains({this}, {item}, {T})")]
      extern bool ICollection<byte>.Contains(byte item);

      [Template("System.Array.remove({this}, {item}, {T})")]
      extern bool ICollection<byte>.Remove(byte item);

      int ICollection<byte>.Count
      {
        [Template("System.Array.getCount({this}, {T})")] get
        {
          return this._System\u002ECollections\u002EGeneric\u002EICollection_System\u002EByte\u003E\u002ECount_BackingField;
        }
      }

      bool ICollection<byte>.IsReadOnly
      {
        [Template("System.Array.getIsReadOnly({this}, {T})")] get
        {
          return this._System\u002ECollections\u002EGeneric\u002EICollection_System\u002EByte\u003E\u002EIsReadOnly_BackingField;
        }
      }

      [Template("Bridge.getEnumerator({this}, {T})")]
      extern IEnumerator<byte> IEnumerable<byte>.GetEnumerator();

      [Template("Bridge.getEnumerator({this})")]
      extern IEnumerator IEnumerable.GetEnumerator();

      extern byte IReadOnlyList<byte>.this[int index] { [Template("System.Array.getItem({this}, {0}, {T})")] get; }

      int IReadOnlyCollection<byte>.Count
      {
        [Template("System.Array.getCount({this}, {T})")] get
        {
          return this._System\u002ECollections\u002EGeneric\u002EIReadOnlyCollection_System\u002EByte\u003E\u002ECount_BackingField;
        }
      }

      [Template("System.Array.copyTo({this}, {array}, {arrayIndex})")]
      extern void ICollection.CopyTo(Array array, int arrayIndex);

      int ICollection.Count
      {
        [Template("System.Array.getCount({this})")] get
        {
          return this._System\u002ECollections\u002EICollection\u002ECount_BackingField;
        }
      }

      object ICollection.SyncRoot
      {
        get
        {
          return this._System\u002ECollections\u002EICollection\u002ESyncRoot_BackingField;
        }
      }

      bool ICollection.IsSynchronized
      {
        get
        {
          return this._System\u002ECollections\u002EICollection\u002EIsSynchronized_BackingField;
        }
      }

      [Generated]
      public delegate bool everyFn(byte value, uint index, es5.Uint8ClampedArray array);

      [Generated]
      public delegate object filterFn(byte value, uint index, es5.Uint8ClampedArray array);

      [Generated]
      public delegate bool findFn(byte value, uint index, es5.Uint8ClampedArray obj);

      [Generated]
      public delegate bool findIndexFn(byte value, uint index, es5.Uint8ClampedArray obj);

      [Generated]
      public delegate void forEachFn(byte value, uint index, es5.Uint8ClampedArray array);

      [Generated]
      public delegate double mapFn(byte value, uint index, es5.Uint8ClampedArray array);

      [Generated]
      public delegate double reduceFn(
        long previousValue,
        byte currentValue,
        uint currentIndex,
        es5.Uint8ClampedArray array);

      [Generated]
      public delegate U reduceFn2<U>(
        U previousValue,
        byte currentValue,
        uint currentIndex,
        es5.Uint8ClampedArray array);

      [Generated]
      public delegate double reduceRightFn(
        long previousValue,
        byte currentValue,
        uint currentIndex,
        es5.Uint8ClampedArray array);

      [Generated]
      public delegate U reduceRightFn2<U>(
        U previousValue,
        byte currentValue,
        uint currentIndex,
        es5.Uint8ClampedArray array);

      [Generated]
      public delegate bool someFn(byte value, uint index, es5.Uint8ClampedArray array);

      [Generated]
      public delegate double sortFn(byte a, byte b);

      [Generated]
      public delegate double fromFn(double v, double k);
    }

    [IgnoreCast]
    [Virtual]
    [FormerInterface]
    public abstract class Uint8ClampedArrayConstructor : IObject
    {
      public abstract es5.Uint8ClampedArray prototype { get; }

      [Template("new {this}({0})")]
      public abstract es5.Uint8ClampedArray New(uint length);

      [Template("new {this}({0})")]
      public abstract es5.Uint8ClampedArray New(
        Union<es5.ArrayLike<byte>, es5.ArrayBufferLike> arrayOrArrayBuffer);

      [Template("new {this}({0})")]
      public abstract es5.Uint8ClampedArray New(es5.ArrayLike<byte> arrayOrArrayBuffer);

      [Template("new {this}({0})")]
      public abstract es5.Uint8ClampedArray New(es5.ArrayBufferLike arrayOrArrayBuffer);

      [Template("new {this}({0})")]
      public abstract es5.Uint8ClampedArray New(es5.ArrayBuffer arrayOrArrayBuffer);

      [Template("new {this}({0}, {1})")]
      public abstract es5.Uint8ClampedArray New(es5.ArrayBufferLike buffer, uint byteOffset);

      [Template("new {this}({0}, {1})")]
      public abstract es5.Uint8ClampedArray New(es5.ArrayBuffer buffer, uint byteOffset);

      [Template("new {this}({0}, {1}, {2})")]
      public abstract es5.Uint8ClampedArray New(
        es5.ArrayBufferLike buffer,
        uint byteOffset,
        uint length);

      [Template("new {this}({0}, {1}, {2})")]
      public abstract es5.Uint8ClampedArray New(
        es5.ArrayBuffer buffer,
        uint byteOffset,
        uint length);

      public abstract double BYTES_PER_ELEMENT { get; }

      [ExpandParams]
      public abstract es5.Uint8ClampedArray of(params byte[] items);

      public abstract es5.Uint8ClampedArray from(es5.ArrayLike<byte> arrayLike);

      public abstract es5.Uint8ClampedArray from(
        es5.ArrayLike<byte> arrayLike,
        es5.Uint8ClampedArrayConstructor.fromFn mapfn);

      public abstract es5.Uint8ClampedArray from(
        es5.ArrayLike<byte> arrayLike,
        es5.Uint8ClampedArrayConstructor.fromFn mapfn,
        object thisArg);

      [Generated]
      public delegate double fromFn(double v, double k);
    }

    [CombinedClass]
    [StaticInterface("Int16ArrayConstructor")]
    [FormerInterface]
    public class Int16Array : IList<short>, ICollection<short>, IEnumerable<short>, IEnumerable, IBridgeClass, IReadOnlyList<short>, IReadOnlyCollection<short>, ICollection, IObject
    {
      private static readonly es5.Int16Array _prototype_BackingField;
      private static readonly double _BYTES_PER_ELEMENT_Static_BackingField;
      private readonly int _BYTES_PER_ELEMENT_BackingField;
      private readonly es5.ArrayBufferLike _buffer_BackingField;
      private readonly uint _byteLength_BackingField;
      private readonly uint _byteOffset_BackingField;
      private readonly uint _length_BackingField;
      private readonly int _System\u002ECollections\u002EGeneric\u002EICollection_System\u002EInt16\u003E\u002ECount_BackingField;
      private readonly bool _System\u002ECollections\u002EGeneric\u002EICollection_System\u002EInt16\u003E\u002EIsReadOnly_BackingField;
      private readonly int _System\u002ECollections\u002EGeneric\u002EIReadOnlyCollection_System\u002EInt16\u003E\u002ECount_BackingField;
      private readonly int _System\u002ECollections\u002EICollection\u002ECount_BackingField;
      private readonly object _System\u002ECollections\u002EICollection\u002ESyncRoot_BackingField;
      private readonly bool _System\u002ECollections\u002EICollection\u002EIsSynchronized_BackingField;

      public extern Int16Array(uint length);

      public extern Int16Array(
        Union<es5.ArrayLike<short>, es5.ArrayBufferLike> arrayOrArrayBuffer);

      public extern Int16Array(es5.ArrayLike<short> arrayOrArrayBuffer);

      public extern Int16Array(es5.ArrayBufferLike arrayOrArrayBuffer);

      public extern Int16Array(es5.ArrayBuffer arrayOrArrayBuffer);

      public extern Int16Array(es5.ArrayBufferLike buffer, uint byteOffset);

      public extern Int16Array(es5.ArrayBuffer buffer, uint byteOffset);

      public extern Int16Array(es5.ArrayBufferLike buffer, uint byteOffset, uint length);

      public extern Int16Array(es5.ArrayBuffer buffer, uint byteOffset, uint length);

      public static es5.Int16Array prototype
      {
        get
        {
          return es5.Int16Array._prototype_BackingField;
        }
      }

      [Name("BYTES_PER_ELEMENT")]
      public static double BYTES_PER_ELEMENT_Static
      {
        get
        {
          return es5.Int16Array._BYTES_PER_ELEMENT_Static_BackingField;
        }
      }

      [ExpandParams]
      public static extern es5.Int16Array of(params short[] items);

      public static extern es5.Int16Array from(es5.ArrayLike<short> arrayLike);

      public static extern es5.Int16Array from(
        es5.ArrayLike<short> arrayLike,
        es5.Int16Array.fromFn mapfn);

      public static extern es5.Int16Array from(
        es5.ArrayLike<short> arrayLike,
        es5.Int16Array.fromFn mapfn,
        object thisArg);

      public virtual int BYTES_PER_ELEMENT
      {
        get
        {
          return this._BYTES_PER_ELEMENT_BackingField;
        }
      }

      public virtual es5.ArrayBufferLike buffer
      {
        get
        {
          return this._buffer_BackingField;
        }
      }

      public virtual uint byteLength
      {
        get
        {
          return this._byteLength_BackingField;
        }
      }

      public virtual uint byteOffset
      {
        get
        {
          return this._byteOffset_BackingField;
        }
      }

      public virtual extern es5.Int16Array copyWithin(long target, long start);

      public virtual extern es5.Int16Array copyWithin(long target, long start, long end);

      public virtual extern bool every(es5.Int16Array.everyFn callbackfn);

      public virtual extern bool every(es5.Int16Array.everyFn callbackfn, object thisArg);

      public virtual extern es5.Int16Array fill(short value);

      public virtual extern es5.Int16Array fill(short value, uint start);

      public virtual extern es5.Int16Array fill(short value, uint start, uint end);

      public virtual extern es5.Int16Array filter(es5.Int16Array.filterFn callbackfn);

      public virtual extern es5.Int16Array filter(
        es5.Int16Array.filterFn callbackfn,
        object thisArg);

      public virtual extern Union<short, Undefined> find(
        es5.Int16Array.findFn predicate);

      public virtual extern Union<short, Undefined> find(
        es5.Int16Array.findFn predicate,
        object thisArg);

      public virtual extern uint findIndex(es5.Int16Array.findIndexFn predicate);

      public virtual extern uint findIndex(es5.Int16Array.findIndexFn predicate, object thisArg);

      public virtual extern void forEach(es5.Int16Array.forEachFn callbackfn);

      public virtual extern void forEach(es5.Int16Array.forEachFn callbackfn, object thisArg);

      public virtual extern uint indexOf(short searchElement);

      public virtual extern uint indexOf(short searchElement, uint fromIndex);

      public virtual extern string join();

      public virtual extern string join(string separator);

      public virtual extern uint lastIndexOf(short searchElement);

      public virtual extern uint lastIndexOf(short searchElement, uint fromIndex);

      public virtual uint length
      {
        get
        {
          return this._length_BackingField;
        }
      }

      public virtual extern es5.Int16Array map(es5.Int16Array.mapFn callbackfn);

      public virtual extern es5.Int16Array map(es5.Int16Array.mapFn callbackfn, object thisArg);

      public virtual extern long reduce(es5.Int16Array.reduceFn callbackfn);

      public virtual extern long reduce(es5.Int16Array.reduceFn callbackfn, long initialValue);

      public virtual extern U reduce<U>(es5.Int16Array.reduceFn2<U> callbackfn, U initialValue);

      public virtual extern long reduceRight(es5.Int16Array.reduceRightFn callbackfn);

      public virtual extern long reduceRight(
        es5.Int16Array.reduceRightFn callbackfn,
        long initialValue);

      public virtual extern U reduceRight<U>(
        es5.Int16Array.reduceRightFn2<U> callbackfn,
        U initialValue);

      public virtual extern es5.Int16Array reverse();

      public virtual extern void set(es5.ArrayLike<short> array);

      public virtual extern void set(es5.ArrayLike<short> array, uint offset);

      public virtual extern es5.Int16Array slice();

      public virtual extern es5.Int16Array slice(uint start);

      public virtual extern es5.Int16Array slice(uint start, uint end);

      public virtual extern bool some(es5.Int16Array.someFn callbackfn);

      public virtual extern bool some(es5.Int16Array.someFn callbackfn, object thisArg);

      public virtual extern es5.Int16Array sort();

      public virtual extern es5.Int16Array sort(es5.Int16Array.sortFn compareFn);

      public virtual extern es5.Int16Array subarray(uint begin);

      public virtual extern es5.Int16Array subarray(uint begin, int end);

      public virtual extern string toLocaleString();

      public virtual extern string toString();

      public virtual extern short this[uint index] { get; set; }

      [Template("System.Array.indexOf({this}, {item}, 0, null, {T})")]
      extern int IList<short>.IndexOf(short item);

      [Template("System.Array.insert({this}, {index}, {item}, {T})")]
      extern void IList<short>.Insert(int index, short item);

      [Template("System.Array.removeAt({this}, {index}, {T})")]
      extern void IList<short>.RemoveAt(int index);

      extern short IList<short>.this[int index] { [Template("System.Array.getItem({this}, {0}, {T})")] get; [Template("System.Array.setItem({this}, {0}, {T})")] set; }

      [Template("System.Array.add({this}, {item}, {T})")]
      extern void ICollection<short>.Add(short item);

      [Template("System.Array.copyTo({this}, {array}, {arrayIndex}, {T})")]
      extern void ICollection<short>.CopyTo(short[] array, int arrayIndex);

      [Template("System.Array.clear({this}, {T})")]
      extern void ICollection<short>.Clear();

      [Template("System.Array.contains({this}, {item}, {T})")]
      extern bool ICollection<short>.Contains(short item);

      [Template("System.Array.remove({this}, {item}, {T})")]
      extern bool ICollection<short>.Remove(short item);

      int ICollection<short>.Count
      {
        [Template("System.Array.getCount({this}, {T})")] get
        {
          return this._System\u002ECollections\u002EGeneric\u002EICollection_System\u002EInt16\u003E\u002ECount_BackingField;
        }
      }

      bool ICollection<short>.IsReadOnly
      {
        [Template("System.Array.getIsReadOnly({this}, {T})")] get
        {
          return this._System\u002ECollections\u002EGeneric\u002EICollection_System\u002EInt16\u003E\u002EIsReadOnly_BackingField;
        }
      }

      [Template("Bridge.getEnumerator({this}, {T})")]
      extern IEnumerator<short> IEnumerable<short>.GetEnumerator();

      [Template("Bridge.getEnumerator({this})")]
      extern IEnumerator IEnumerable.GetEnumerator();

      extern short IReadOnlyList<short>.this[int index] { [Template("System.Array.getItem({this}, {0}, {T})")] get; }

      int IReadOnlyCollection<short>.Count
      {
        [Template("System.Array.getCount({this}, {T})")] get
        {
          return this._System\u002ECollections\u002EGeneric\u002EIReadOnlyCollection_System\u002EInt16\u003E\u002ECount_BackingField;
        }
      }

      [Template("System.Array.copyTo({this}, {array}, {arrayIndex})")]
      extern void ICollection.CopyTo(Array array, int arrayIndex);

      int ICollection.Count
      {
        [Template("System.Array.getCount({this})")] get
        {
          return this._System\u002ECollections\u002EICollection\u002ECount_BackingField;
        }
      }

      object ICollection.SyncRoot
      {
        get
        {
          return this._System\u002ECollections\u002EICollection\u002ESyncRoot_BackingField;
        }
      }

      bool ICollection.IsSynchronized
      {
        get
        {
          return this._System\u002ECollections\u002EICollection\u002EIsSynchronized_BackingField;
        }
      }

      [Generated]
      public delegate bool everyFn(short value, uint index, es5.Int16Array array);

      [Generated]
      public delegate object filterFn(short value, uint index, es5.Int16Array array);

      [Generated]
      public delegate bool findFn(short value, uint index, es5.Int16Array obj);

      [Generated]
      public delegate bool findIndexFn(short value, uint index, es5.Int16Array obj);

      [Generated]
      public delegate void forEachFn(short value, uint index, es5.Int16Array array);

      [Generated]
      public delegate double mapFn(short value, uint index, es5.Int16Array array);

      [Generated]
      public delegate double reduceFn(
        long previousValue,
        short currentValue,
        uint currentIndex,
        es5.Int16Array array);

      [Generated]
      public delegate U reduceFn2<U>(
        U previousValue,
        short currentValue,
        uint currentIndex,
        es5.Int16Array array);

      [Generated]
      public delegate double reduceRightFn(
        long previousValue,
        short currentValue,
        uint currentIndex,
        es5.Int16Array array);

      [Generated]
      public delegate U reduceRightFn2<U>(
        U previousValue,
        short currentValue,
        uint currentIndex,
        es5.Int16Array array);

      [Generated]
      public delegate bool someFn(short value, uint index, es5.Int16Array array);

      [Generated]
      public delegate double sortFn(short a, short b);

      [Generated]
      public delegate double fromFn(double v, double k);
    }

    [IgnoreCast]
    [Virtual]
    [FormerInterface]
    public abstract class Int16ArrayConstructor : IObject
    {
      public abstract es5.Int16Array prototype { get; }

      [Template("new {this}({0})")]
      public abstract es5.Int16Array New(uint length);

      [Template("new {this}({0})")]
      public abstract es5.Int16Array New(
        Union<es5.ArrayLike<short>, es5.ArrayBufferLike> arrayOrArrayBuffer);

      [Template("new {this}({0})")]
      public abstract es5.Int16Array New(es5.ArrayLike<short> arrayOrArrayBuffer);

      [Template("new {this}({0})")]
      public abstract es5.Int16Array New(es5.ArrayBufferLike arrayOrArrayBuffer);

      [Template("new {this}({0})")]
      public abstract es5.Int16Array New(es5.ArrayBuffer arrayOrArrayBuffer);

      [Template("new {this}({0}, {1})")]
      public abstract es5.Int16Array New(es5.ArrayBufferLike buffer, uint byteOffset);

      [Template("new {this}({0}, {1})")]
      public abstract es5.Int16Array New(es5.ArrayBuffer buffer, uint byteOffset);

      [Template("new {this}({0}, {1}, {2})")]
      public abstract es5.Int16Array New(
        es5.ArrayBufferLike buffer,
        uint byteOffset,
        uint length);

      [Template("new {this}({0}, {1}, {2})")]
      public abstract es5.Int16Array New(es5.ArrayBuffer buffer, uint byteOffset, uint length);

      public abstract double BYTES_PER_ELEMENT { get; }

      [ExpandParams]
      public abstract es5.Int16Array of(params short[] items);

      public abstract es5.Int16Array from(es5.ArrayLike<short> arrayLike);

      public abstract es5.Int16Array from(
        es5.ArrayLike<short> arrayLike,
        es5.Int16ArrayConstructor.fromFn mapfn);

      public abstract es5.Int16Array from(
        es5.ArrayLike<short> arrayLike,
        es5.Int16ArrayConstructor.fromFn mapfn,
        object thisArg);

      [Generated]
      public delegate double fromFn(double v, double k);
    }

    [CombinedClass]
    [StaticInterface("Uint16ArrayConstructor")]
    [FormerInterface]
    public class Uint16Array : IList<ushort>, ICollection<ushort>, IEnumerable<ushort>, IEnumerable, IBridgeClass, IReadOnlyList<ushort>, IReadOnlyCollection<ushort>, ICollection, IObject
    {
      private static readonly es5.Uint16Array _prototype_BackingField;
      private static readonly double _BYTES_PER_ELEMENT_Static_BackingField;
      private readonly int _BYTES_PER_ELEMENT_BackingField;
      private readonly es5.ArrayBufferLike _buffer_BackingField;
      private readonly uint _byteLength_BackingField;
      private readonly uint _byteOffset_BackingField;
      private readonly uint _length_BackingField;
      private readonly int _System\u002ECollections\u002EGeneric\u002EICollection_System\u002EUInt16\u003E\u002ECount_BackingField;
      private readonly bool _System\u002ECollections\u002EGeneric\u002EICollection_System\u002EUInt16\u003E\u002EIsReadOnly_BackingField;
      private readonly int _System\u002ECollections\u002EGeneric\u002EIReadOnlyCollection_System\u002EUInt16\u003E\u002ECount_BackingField;
      private readonly int _System\u002ECollections\u002EICollection\u002ECount_BackingField;
      private readonly object _System\u002ECollections\u002EICollection\u002ESyncRoot_BackingField;
      private readonly bool _System\u002ECollections\u002EICollection\u002EIsSynchronized_BackingField;

      public extern Uint16Array(uint length);

      public extern Uint16Array(
        Union<es5.ArrayLike<ushort>, es5.ArrayBufferLike> arrayOrArrayBuffer);

      public extern Uint16Array(es5.ArrayLike<ushort> arrayOrArrayBuffer);

      public extern Uint16Array(es5.ArrayBufferLike arrayOrArrayBuffer);

      public extern Uint16Array(es5.ArrayBuffer arrayOrArrayBuffer);

      public extern Uint16Array(es5.ArrayBufferLike buffer, uint byteOffset);

      public extern Uint16Array(es5.ArrayBuffer buffer, uint byteOffset);

      public extern Uint16Array(es5.ArrayBufferLike buffer, uint byteOffset, uint length);

      public extern Uint16Array(es5.ArrayBuffer buffer, uint byteOffset, uint length);

      public static es5.Uint16Array prototype
      {
        get
        {
          return es5.Uint16Array._prototype_BackingField;
        }
      }

      [Name("BYTES_PER_ELEMENT")]
      public static double BYTES_PER_ELEMENT_Static
      {
        get
        {
          return es5.Uint16Array._BYTES_PER_ELEMENT_Static_BackingField;
        }
      }

      [ExpandParams]
      public static extern es5.Uint16Array of(params ushort[] items);

      public static extern es5.Uint16Array from(es5.ArrayLike<ushort> arrayLike);

      public static extern es5.Uint16Array from(
        es5.ArrayLike<ushort> arrayLike,
        es5.Uint16Array.fromFn mapfn);

      public static extern es5.Uint16Array from(
        es5.ArrayLike<ushort> arrayLike,
        es5.Uint16Array.fromFn mapfn,
        object thisArg);

      public virtual int BYTES_PER_ELEMENT
      {
        get
        {
          return this._BYTES_PER_ELEMENT_BackingField;
        }
      }

      public virtual es5.ArrayBufferLike buffer
      {
        get
        {
          return this._buffer_BackingField;
        }
      }

      public virtual uint byteLength
      {
        get
        {
          return this._byteLength_BackingField;
        }
      }

      public virtual uint byteOffset
      {
        get
        {
          return this._byteOffset_BackingField;
        }
      }

      public virtual extern es5.Uint16Array copyWithin(long target, long start);

      public virtual extern es5.Uint16Array copyWithin(long target, long start, long end);

      public virtual extern bool every(es5.Uint16Array.everyFn callbackfn);

      public virtual extern bool every(es5.Uint16Array.everyFn callbackfn, object thisArg);

      public virtual extern es5.Uint16Array fill(ushort value);

      public virtual extern es5.Uint16Array fill(ushort value, uint start);

      public virtual extern es5.Uint16Array fill(ushort value, uint start, uint end);

      public virtual extern es5.Uint16Array filter(es5.Uint16Array.filterFn callbackfn);

      public virtual extern es5.Uint16Array filter(
        es5.Uint16Array.filterFn callbackfn,
        object thisArg);

      public virtual extern Union<ushort, Undefined> find(
        es5.Uint16Array.findFn predicate);

      public virtual extern Union<ushort, Undefined> find(
        es5.Uint16Array.findFn predicate,
        object thisArg);

      public virtual extern uint findIndex(es5.Uint16Array.findIndexFn predicate);

      public virtual extern uint findIndex(es5.Uint16Array.findIndexFn predicate, object thisArg);

      public virtual extern void forEach(es5.Uint16Array.forEachFn callbackfn);

      public virtual extern void forEach(es5.Uint16Array.forEachFn callbackfn, object thisArg);

      public virtual extern uint indexOf(ushort searchElement);

      public virtual extern uint indexOf(ushort searchElement, uint fromIndex);

      public virtual extern string join();

      public virtual extern string join(string separator);

      public virtual extern uint lastIndexOf(ushort searchElement);

      public virtual extern uint lastIndexOf(ushort searchElement, uint fromIndex);

      public virtual uint length
      {
        get
        {
          return this._length_BackingField;
        }
      }

      public virtual extern es5.Uint16Array map(es5.Uint16Array.mapFn callbackfn);

      public virtual extern es5.Uint16Array map(es5.Uint16Array.mapFn callbackfn, object thisArg);

      public virtual extern long reduce(es5.Uint16Array.reduceFn callbackfn);

      public virtual extern long reduce(es5.Uint16Array.reduceFn callbackfn, long initialValue);

      public virtual extern U reduce<U>(es5.Uint16Array.reduceFn2<U> callbackfn, U initialValue);

      public virtual extern long reduceRight(es5.Uint16Array.reduceRightFn callbackfn);

      public virtual extern long reduceRight(
        es5.Uint16Array.reduceRightFn callbackfn,
        long initialValue);

      public virtual extern U reduceRight<U>(
        es5.Uint16Array.reduceRightFn2<U> callbackfn,
        U initialValue);

      public virtual extern es5.Uint16Array reverse();

      public virtual extern void set(es5.ArrayLike<ushort> array);

      public virtual extern void set(es5.ArrayLike<ushort> array, uint offset);

      public virtual extern es5.Uint16Array slice();

      public virtual extern es5.Uint16Array slice(uint start);

      public virtual extern es5.Uint16Array slice(uint start, uint end);

      public virtual extern bool some(es5.Uint16Array.someFn callbackfn);

      public virtual extern bool some(es5.Uint16Array.someFn callbackfn, object thisArg);

      public virtual extern es5.Uint16Array sort();

      public virtual extern es5.Uint16Array sort(es5.Uint16Array.sortFn compareFn);

      public virtual extern es5.Uint16Array subarray(uint begin);

      public virtual extern es5.Uint16Array subarray(uint begin, int end);

      public virtual extern string toLocaleString();

      public virtual extern string toString();

      public virtual extern ushort this[uint index] { get; set; }

      [Template("System.Array.indexOf({this}, {item}, 0, null, {T})")]
      extern int IList<ushort>.IndexOf(ushort item);

      [Template("System.Array.insert({this}, {index}, {item}, {T})")]
      extern void IList<ushort>.Insert(int index, ushort item);

      [Template("System.Array.removeAt({this}, {index}, {T})")]
      extern void IList<ushort>.RemoveAt(int index);

      extern ushort IList<ushort>.this[int index] { [Template("System.Array.getItem({this}, {0}, {T})")] get; [Template("System.Array.setItem({this}, {0}, {T})")] set; }

      [Template("System.Array.add({this}, {item}, {T})")]
      extern void ICollection<ushort>.Add(ushort item);

      [Template("System.Array.copyTo({this}, {array}, {arrayIndex}, {T})")]
      extern void ICollection<ushort>.CopyTo(ushort[] array, int arrayIndex);

      [Template("System.Array.clear({this}, {T})")]
      extern void ICollection<ushort>.Clear();

      [Template("System.Array.contains({this}, {item}, {T})")]
      extern bool ICollection<ushort>.Contains(ushort item);

      [Template("System.Array.remove({this}, {item}, {T})")]
      extern bool ICollection<ushort>.Remove(ushort item);

      int ICollection<ushort>.Count
      {
        [Template("System.Array.getCount({this}, {T})")] get
        {
          return this._System\u002ECollections\u002EGeneric\u002EICollection_System\u002EUInt16\u003E\u002ECount_BackingField;
        }
      }

      bool ICollection<ushort>.IsReadOnly
      {
        [Template("System.Array.getIsReadOnly({this}, {T})")] get
        {
          return this._System\u002ECollections\u002EGeneric\u002EICollection_System\u002EUInt16\u003E\u002EIsReadOnly_BackingField;
        }
      }

      [Template("Bridge.getEnumerator({this}, {T})")]
      extern IEnumerator<ushort> IEnumerable<ushort>.GetEnumerator();

      [Template("Bridge.getEnumerator({this})")]
      extern IEnumerator IEnumerable.GetEnumerator();

      extern ushort IReadOnlyList<ushort>.this[int index] { [Template("System.Array.getItem({this}, {0}, {T})")] get; }

      int IReadOnlyCollection<ushort>.Count
      {
        [Template("System.Array.getCount({this}, {T})")] get
        {
          return this._System\u002ECollections\u002EGeneric\u002EIReadOnlyCollection_System\u002EUInt16\u003E\u002ECount_BackingField;
        }
      }

      [Template("System.Array.copyTo({this}, {array}, {arrayIndex})")]
      extern void ICollection.CopyTo(Array array, int arrayIndex);

      int ICollection.Count
      {
        [Template("System.Array.getCount({this})")] get
        {
          return this._System\u002ECollections\u002EICollection\u002ECount_BackingField;
        }
      }

      object ICollection.SyncRoot
      {
        get
        {
          return this._System\u002ECollections\u002EICollection\u002ESyncRoot_BackingField;
        }
      }

      bool ICollection.IsSynchronized
      {
        get
        {
          return this._System\u002ECollections\u002EICollection\u002EIsSynchronized_BackingField;
        }
      }

      [Generated]
      public delegate bool everyFn(ushort value, uint index, es5.Uint16Array array);

      [Generated]
      public delegate object filterFn(ushort value, uint index, es5.Uint16Array array);

      [Generated]
      public delegate bool findFn(ushort value, uint index, es5.Uint16Array obj);

      [Generated]
      public delegate bool findIndexFn(ushort value, uint index, es5.Uint16Array obj);

      [Generated]
      public delegate void forEachFn(ushort value, uint index, es5.Uint16Array array);

      [Generated]
      public delegate double mapFn(ushort value, uint index, es5.Uint16Array array);

      [Generated]
      public delegate double reduceFn(
        long previousValue,
        ushort currentValue,
        uint currentIndex,
        es5.Uint16Array array);

      [Generated]
      public delegate U reduceFn2<U>(
        U previousValue,
        ushort currentValue,
        uint currentIndex,
        es5.Uint16Array array);

      [Generated]
      public delegate double reduceRightFn(
        long previousValue,
        ushort currentValue,
        uint currentIndex,
        es5.Uint16Array array);

      [Generated]
      public delegate U reduceRightFn2<U>(
        U previousValue,
        ushort currentValue,
        uint currentIndex,
        es5.Uint16Array array);

      [Generated]
      public delegate bool someFn(ushort value, uint index, es5.Uint16Array array);

      [Generated]
      public delegate double sortFn(ushort a, ushort b);

      [Generated]
      public delegate double fromFn(double v, double k);
    }

    [IgnoreCast]
    [Virtual]
    [FormerInterface]
    public abstract class Uint16ArrayConstructor : IObject
    {
      public abstract es5.Uint16Array prototype { get; }

      [Template("new {this}({0})")]
      public abstract es5.Uint16Array New(uint length);

      [Template("new {this}({0})")]
      public abstract es5.Uint16Array New(
        Union<es5.ArrayLike<ushort>, es5.ArrayBufferLike> arrayOrArrayBuffer);

      [Template("new {this}({0})")]
      public abstract es5.Uint16Array New(es5.ArrayLike<ushort> arrayOrArrayBuffer);

      [Template("new {this}({0})")]
      public abstract es5.Uint16Array New(es5.ArrayBufferLike arrayOrArrayBuffer);

      [Template("new {this}({0})")]
      public abstract es5.Uint16Array New(es5.ArrayBuffer arrayOrArrayBuffer);

      [Template("new {this}({0}, {1})")]
      public abstract es5.Uint16Array New(es5.ArrayBufferLike buffer, uint byteOffset);

      [Template("new {this}({0}, {1})")]
      public abstract es5.Uint16Array New(es5.ArrayBuffer buffer, uint byteOffset);

      [Template("new {this}({0}, {1}, {2})")]
      public abstract es5.Uint16Array New(
        es5.ArrayBufferLike buffer,
        uint byteOffset,
        uint length);

      [Template("new {this}({0}, {1}, {2})")]
      public abstract es5.Uint16Array New(es5.ArrayBuffer buffer, uint byteOffset, uint length);

      public abstract double BYTES_PER_ELEMENT { get; }

      [ExpandParams]
      public abstract es5.Uint16Array of(params ushort[] items);

      public abstract es5.Uint16Array from(es5.ArrayLike<ushort> arrayLike);

      public abstract es5.Uint16Array from(
        es5.ArrayLike<ushort> arrayLike,
        es5.Uint16ArrayConstructor.fromFn mapfn);

      public abstract es5.Uint16Array from(
        es5.ArrayLike<ushort> arrayLike,
        es5.Uint16ArrayConstructor.fromFn mapfn,
        object thisArg);

      [Generated]
      public delegate double fromFn(double v, double k);
    }

    [CombinedClass]
    [StaticInterface("Int32ArrayConstructor")]
    [FormerInterface]
    public class Int32Array : IList<int>, ICollection<int>, IEnumerable<int>, IEnumerable, IBridgeClass, IReadOnlyList<int>, IReadOnlyCollection<int>, ICollection, IObject
    {
      private static readonly es5.Int32Array _prototype_BackingField;
      private static readonly double _BYTES_PER_ELEMENT_Static_BackingField;
      private readonly int _BYTES_PER_ELEMENT_BackingField;
      private readonly es5.ArrayBufferLike _buffer_BackingField;
      private readonly uint _byteLength_BackingField;
      private readonly uint _byteOffset_BackingField;
      private readonly uint _length_BackingField;
      private readonly int _System\u002ECollections\u002EGeneric\u002EICollection_System\u002EInt32\u003E\u002ECount_BackingField;
      private readonly bool _System\u002ECollections\u002EGeneric\u002EICollection_System\u002EInt32\u003E\u002EIsReadOnly_BackingField;
      private readonly int _System\u002ECollections\u002EGeneric\u002EIReadOnlyCollection_System\u002EInt32\u003E\u002ECount_BackingField;
      private readonly int _System\u002ECollections\u002EICollection\u002ECount_BackingField;
      private readonly object _System\u002ECollections\u002EICollection\u002ESyncRoot_BackingField;
      private readonly bool _System\u002ECollections\u002EICollection\u002EIsSynchronized_BackingField;

      public extern Int32Array(uint length);

      public extern Int32Array(
        Union<es5.ArrayLike<int>, es5.ArrayBufferLike> arrayOrArrayBuffer);

      public extern Int32Array(es5.ArrayLike<int> arrayOrArrayBuffer);

      public extern Int32Array(es5.ArrayBufferLike arrayOrArrayBuffer);

      public extern Int32Array(es5.ArrayBuffer arrayOrArrayBuffer);

      public extern Int32Array(es5.ArrayBufferLike buffer, uint byteOffset);

      public extern Int32Array(es5.ArrayBuffer buffer, uint byteOffset);

      public extern Int32Array(es5.ArrayBufferLike buffer, uint byteOffset, uint length);

      public extern Int32Array(es5.ArrayBuffer buffer, uint byteOffset, uint length);

      public static es5.Int32Array prototype
      {
        get
        {
          return es5.Int32Array._prototype_BackingField;
        }
      }

      [Name("BYTES_PER_ELEMENT")]
      public static double BYTES_PER_ELEMENT_Static
      {
        get
        {
          return es5.Int32Array._BYTES_PER_ELEMENT_Static_BackingField;
        }
      }

      [ExpandParams]
      public static extern es5.Int32Array of(params int[] items);

      public static extern es5.Int32Array from(es5.ArrayLike<int> arrayLike);

      public static extern es5.Int32Array from(
        es5.ArrayLike<int> arrayLike,
        es5.Int32Array.fromFn mapfn);

      public static extern es5.Int32Array from(
        es5.ArrayLike<int> arrayLike,
        es5.Int32Array.fromFn mapfn,
        object thisArg);

      public virtual int BYTES_PER_ELEMENT
      {
        get
        {
          return this._BYTES_PER_ELEMENT_BackingField;
        }
      }

      public virtual es5.ArrayBufferLike buffer
      {
        get
        {
          return this._buffer_BackingField;
        }
      }

      public virtual uint byteLength
      {
        get
        {
          return this._byteLength_BackingField;
        }
      }

      public virtual uint byteOffset
      {
        get
        {
          return this._byteOffset_BackingField;
        }
      }

      public virtual extern es5.Int32Array copyWithin(long target, long start);

      public virtual extern es5.Int32Array copyWithin(long target, long start, long end);

      public virtual extern bool every(es5.Int32Array.everyFn callbackfn);

      public virtual extern bool every(es5.Int32Array.everyFn callbackfn, object thisArg);

      public virtual extern es5.Int32Array fill(int value);

      public virtual extern es5.Int32Array fill(int value, uint start);

      public virtual extern es5.Int32Array fill(int value, uint start, uint end);

      public virtual extern es5.Int32Array filter(es5.Int32Array.filterFn callbackfn);

      public virtual extern es5.Int32Array filter(
        es5.Int32Array.filterFn callbackfn,
        object thisArg);

      public virtual extern Union<int, Undefined> find(es5.Int32Array.findFn predicate);

      public virtual extern Union<int, Undefined> find(
        es5.Int32Array.findFn predicate,
        object thisArg);

      public virtual extern uint findIndex(es5.Int32Array.findIndexFn predicate);

      public virtual extern uint findIndex(es5.Int32Array.findIndexFn predicate, object thisArg);

      public virtual extern void forEach(es5.Int32Array.forEachFn callbackfn);

      public virtual extern void forEach(es5.Int32Array.forEachFn callbackfn, object thisArg);

      public virtual extern uint indexOf(int searchElement);

      public virtual extern uint indexOf(int searchElement, uint fromIndex);

      public virtual extern string join();

      public virtual extern string join(string separator);

      public virtual extern uint lastIndexOf(int searchElement);

      public virtual extern uint lastIndexOf(int searchElement, uint fromIndex);

      public virtual uint length
      {
        get
        {
          return this._length_BackingField;
        }
      }

      public virtual extern es5.Int32Array map(es5.Int32Array.mapFn callbackfn);

      public virtual extern es5.Int32Array map(es5.Int32Array.mapFn callbackfn, object thisArg);

      public virtual extern long reduce(es5.Int32Array.reduceFn callbackfn);

      public virtual extern long reduce(es5.Int32Array.reduceFn callbackfn, long initialValue);

      public virtual extern U reduce<U>(es5.Int32Array.reduceFn2<U> callbackfn, U initialValue);

      public virtual extern long reduceRight(es5.Int32Array.reduceRightFn callbackfn);

      public virtual extern long reduceRight(
        es5.Int32Array.reduceRightFn callbackfn,
        long initialValue);

      public virtual extern U reduceRight<U>(
        es5.Int32Array.reduceRightFn2<U> callbackfn,
        U initialValue);

      public virtual extern es5.Int32Array reverse();

      public virtual extern void set(es5.ArrayLike<int> array);

      public virtual extern void set(es5.ArrayLike<int> array, uint offset);

      public virtual extern es5.Int32Array slice();

      public virtual extern es5.Int32Array slice(uint start);

      public virtual extern es5.Int32Array slice(uint start, uint end);

      public virtual extern bool some(es5.Int32Array.someFn callbackfn);

      public virtual extern bool some(es5.Int32Array.someFn callbackfn, object thisArg);

      public virtual extern es5.Int32Array sort();

      public virtual extern es5.Int32Array sort(es5.Int32Array.sortFn compareFn);

      public virtual extern es5.Int32Array subarray(uint begin);

      public virtual extern es5.Int32Array subarray(uint begin, int end);

      public virtual extern string toLocaleString();

      public virtual extern string toString();

      public virtual extern int this[uint index] { get; set; }

      [Template("System.Array.indexOf({this}, {item}, 0, null, {T})")]
      extern int IList<int>.IndexOf(int item);

      [Template("System.Array.insert({this}, {index}, {item}, {T})")]
      extern void IList<int>.Insert(int index, int item);

      [Template("System.Array.removeAt({this}, {index}, {T})")]
      extern void IList<int>.RemoveAt(int index);

      extern int IList<int>.this[int index] { [Template("System.Array.getItem({this}, {0}, {T})")] get; [Template("System.Array.setItem({this}, {0}, {T})")] set; }

      [Template("System.Array.add({this}, {item}, {T})")]
      extern void ICollection<int>.Add(int item);

      [Template("System.Array.copyTo({this}, {array}, {arrayIndex}, {T})")]
      extern void ICollection<int>.CopyTo(int[] array, int arrayIndex);

      [Template("System.Array.clear({this}, {T})")]
      extern void ICollection<int>.Clear();

      [Template("System.Array.contains({this}, {item}, {T})")]
      extern bool ICollection<int>.Contains(int item);

      [Template("System.Array.remove({this}, {item}, {T})")]
      extern bool ICollection<int>.Remove(int item);

      int ICollection<int>.Count
      {
        [Template("System.Array.getCount({this}, {T})")] get
        {
          return this._System\u002ECollections\u002EGeneric\u002EICollection_System\u002EInt32\u003E\u002ECount_BackingField;
        }
      }

      bool ICollection<int>.IsReadOnly
      {
        [Template("System.Array.getIsReadOnly({this}, {T})")] get
        {
          return this._System\u002ECollections\u002EGeneric\u002EICollection_System\u002EInt32\u003E\u002EIsReadOnly_BackingField;
        }
      }

      [Template("Bridge.getEnumerator({this}, {T})")]
      extern IEnumerator<int> IEnumerable<int>.GetEnumerator();

      [Template("Bridge.getEnumerator({this})")]
      extern IEnumerator IEnumerable.GetEnumerator();

      extern int IReadOnlyList<int>.this[int index] { [Template("System.Array.getItem({this}, {0}, {T})")] get; }

      int IReadOnlyCollection<int>.Count
      {
        [Template("System.Array.getCount({this}, {T})")] get
        {
          return this._System\u002ECollections\u002EGeneric\u002EIReadOnlyCollection_System\u002EInt32\u003E\u002ECount_BackingField;
        }
      }

      [Template("System.Array.copyTo({this}, {array}, {arrayIndex})")]
      extern void ICollection.CopyTo(Array array, int arrayIndex);

      int ICollection.Count
      {
        [Template("System.Array.getCount({this})")] get
        {
          return this._System\u002ECollections\u002EICollection\u002ECount_BackingField;
        }
      }

      object ICollection.SyncRoot
      {
        get
        {
          return this._System\u002ECollections\u002EICollection\u002ESyncRoot_BackingField;
        }
      }

      bool ICollection.IsSynchronized
      {
        get
        {
          return this._System\u002ECollections\u002EICollection\u002EIsSynchronized_BackingField;
        }
      }

      [Generated]
      public delegate bool everyFn(int value, uint index, es5.Int32Array array);

      [Generated]
      public delegate object filterFn(int value, uint index, es5.Int32Array array);

      [Generated]
      public delegate bool findFn(int value, uint index, es5.Int32Array obj);

      [Generated]
      public delegate bool findIndexFn(int value, uint index, es5.Int32Array obj);

      [Generated]
      public delegate void forEachFn(int value, uint index, es5.Int32Array array);

      [Generated]
      public delegate double mapFn(int value, uint index, es5.Int32Array array);

      [Generated]
      public delegate double reduceFn(
        long previousValue,
        int currentValue,
        uint currentIndex,
        es5.Int32Array array);

      [Generated]
      public delegate U reduceFn2<U>(
        U previousValue,
        int currentValue,
        uint currentIndex,
        es5.Int32Array array);

      [Generated]
      public delegate double reduceRightFn(
        long previousValue,
        int currentValue,
        uint currentIndex,
        es5.Int32Array array);

      [Generated]
      public delegate U reduceRightFn2<U>(
        U previousValue,
        int currentValue,
        uint currentIndex,
        es5.Int32Array array);

      [Generated]
      public delegate bool someFn(int value, uint index, es5.Int32Array array);

      [Generated]
      public delegate double sortFn(int a, int b);

      [Generated]
      public delegate double fromFn(double v, double k);
    }

    [IgnoreCast]
    [Virtual]
    [FormerInterface]
    public abstract class Int32ArrayConstructor : IObject
    {
      public abstract es5.Int32Array prototype { get; }

      [Template("new {this}({0})")]
      public abstract es5.Int32Array New(uint length);

      [Template("new {this}({0})")]
      public abstract es5.Int32Array New(
        Union<es5.ArrayLike<int>, es5.ArrayBufferLike> arrayOrArrayBuffer);

      [Template("new {this}({0})")]
      public abstract es5.Int32Array New(es5.ArrayLike<int> arrayOrArrayBuffer);

      [Template("new {this}({0})")]
      public abstract es5.Int32Array New(es5.ArrayBufferLike arrayOrArrayBuffer);

      [Template("new {this}({0})")]
      public abstract es5.Int32Array New(es5.ArrayBuffer arrayOrArrayBuffer);

      [Template("new {this}({0}, {1})")]
      public abstract es5.Int32Array New(es5.ArrayBufferLike buffer, uint byteOffset);

      [Template("new {this}({0}, {1})")]
      public abstract es5.Int32Array New(es5.ArrayBuffer buffer, uint byteOffset);

      [Template("new {this}({0}, {1}, {2})")]
      public abstract es5.Int32Array New(
        es5.ArrayBufferLike buffer,
        uint byteOffset,
        uint length);

      [Template("new {this}({0}, {1}, {2})")]
      public abstract es5.Int32Array New(es5.ArrayBuffer buffer, uint byteOffset, uint length);

      public abstract double BYTES_PER_ELEMENT { get; }

      [ExpandParams]
      public abstract es5.Int32Array of(params int[] items);

      public abstract es5.Int32Array from(es5.ArrayLike<int> arrayLike);

      public abstract es5.Int32Array from(
        es5.ArrayLike<int> arrayLike,
        es5.Int32ArrayConstructor.fromFn mapfn);

      public abstract es5.Int32Array from(
        es5.ArrayLike<int> arrayLike,
        es5.Int32ArrayConstructor.fromFn mapfn,
        object thisArg);

      [Generated]
      public delegate double fromFn(double v, double k);
    }

    [CombinedClass]
    [StaticInterface("Uint32ArrayConstructor")]
    [FormerInterface]
    public class Uint32Array : IList<uint>, ICollection<uint>, IEnumerable<uint>, IEnumerable, IBridgeClass, IReadOnlyList<uint>, IReadOnlyCollection<uint>, ICollection, IObject
    {
      private static readonly es5.Uint32Array _prototype_BackingField;
      private static readonly double _BYTES_PER_ELEMENT_Static_BackingField;
      private readonly int _BYTES_PER_ELEMENT_BackingField;
      private readonly es5.ArrayBufferLike _buffer_BackingField;
      private readonly uint _byteLength_BackingField;
      private readonly uint _byteOffset_BackingField;
      private readonly uint _length_BackingField;
      private readonly int _System\u002ECollections\u002EGeneric\u002EICollection_System\u002EUInt32\u003E\u002ECount_BackingField;
      private readonly bool _System\u002ECollections\u002EGeneric\u002EICollection_System\u002EUInt32\u003E\u002EIsReadOnly_BackingField;
      private readonly int _System\u002ECollections\u002EGeneric\u002EIReadOnlyCollection_System\u002EUInt32\u003E\u002ECount_BackingField;
      private readonly int _System\u002ECollections\u002EICollection\u002ECount_BackingField;
      private readonly object _System\u002ECollections\u002EICollection\u002ESyncRoot_BackingField;
      private readonly bool _System\u002ECollections\u002EICollection\u002EIsSynchronized_BackingField;

      public extern Uint32Array(uint length);

      public extern Uint32Array(
        Union<es5.ArrayLike<uint>, es5.ArrayBufferLike> arrayOrArrayBuffer);

      public extern Uint32Array(es5.ArrayLike<uint> arrayOrArrayBuffer);

      public extern Uint32Array(es5.ArrayBufferLike arrayOrArrayBuffer);

      public extern Uint32Array(es5.ArrayBuffer arrayOrArrayBuffer);

      public extern Uint32Array(es5.ArrayBufferLike buffer, uint byteOffset);

      public extern Uint32Array(es5.ArrayBuffer buffer, uint byteOffset);

      public extern Uint32Array(es5.ArrayBufferLike buffer, uint byteOffset, uint length);

      public extern Uint32Array(es5.ArrayBuffer buffer, uint byteOffset, uint length);

      public static es5.Uint32Array prototype
      {
        get
        {
          return es5.Uint32Array._prototype_BackingField;
        }
      }

      [Name("BYTES_PER_ELEMENT")]
      public static double BYTES_PER_ELEMENT_Static
      {
        get
        {
          return es5.Uint32Array._BYTES_PER_ELEMENT_Static_BackingField;
        }
      }

      [ExpandParams]
      public static extern es5.Uint32Array of(params uint[] items);

      public static extern es5.Uint32Array from(es5.ArrayLike<uint> arrayLike);

      public static extern es5.Uint32Array from(
        es5.ArrayLike<uint> arrayLike,
        es5.Uint32Array.fromFn mapfn);

      public static extern es5.Uint32Array from(
        es5.ArrayLike<uint> arrayLike,
        es5.Uint32Array.fromFn mapfn,
        object thisArg);

      public virtual int BYTES_PER_ELEMENT
      {
        get
        {
          return this._BYTES_PER_ELEMENT_BackingField;
        }
      }

      public virtual es5.ArrayBufferLike buffer
      {
        get
        {
          return this._buffer_BackingField;
        }
      }

      public virtual uint byteLength
      {
        get
        {
          return this._byteLength_BackingField;
        }
      }

      public virtual uint byteOffset
      {
        get
        {
          return this._byteOffset_BackingField;
        }
      }

      public virtual extern es5.Uint32Array copyWithin(long target, long start);

      public virtual extern es5.Uint32Array copyWithin(long target, long start, long end);

      public virtual extern bool every(es5.Uint32Array.everyFn callbackfn);

      public virtual extern bool every(es5.Uint32Array.everyFn callbackfn, object thisArg);

      public virtual extern es5.Uint32Array fill(uint value);

      public virtual extern es5.Uint32Array fill(uint value, uint start);

      public virtual extern es5.Uint32Array fill(uint value, uint start, uint end);

      public virtual extern es5.Uint32Array filter(es5.Uint32Array.filterFn callbackfn);

      public virtual extern es5.Uint32Array filter(
        es5.Uint32Array.filterFn callbackfn,
        object thisArg);

      public virtual extern Union<uint, Undefined> find(
        es5.Uint32Array.findFn predicate);

      public virtual extern Union<uint, Undefined> find(
        es5.Uint32Array.findFn predicate,
        object thisArg);

      public virtual extern uint findIndex(es5.Uint32Array.findIndexFn predicate);

      public virtual extern uint findIndex(es5.Uint32Array.findIndexFn predicate, object thisArg);

      public virtual extern void forEach(es5.Uint32Array.forEachFn callbackfn);

      public virtual extern void forEach(es5.Uint32Array.forEachFn callbackfn, object thisArg);

      public virtual extern uint indexOf(uint searchElement);

      public virtual extern uint indexOf(uint searchElement, uint fromIndex);

      public virtual extern string join();

      public virtual extern string join(string separator);

      public virtual extern uint lastIndexOf(uint searchElement);

      public virtual extern uint lastIndexOf(uint searchElement, uint fromIndex);

      public virtual uint length
      {
        get
        {
          return this._length_BackingField;
        }
      }

      public virtual extern es5.Uint32Array map(es5.Uint32Array.mapFn callbackfn);

      public virtual extern es5.Uint32Array map(es5.Uint32Array.mapFn callbackfn, object thisArg);

      public virtual extern long reduce(es5.Uint32Array.reduceFn callbackfn);

      public virtual extern long reduce(es5.Uint32Array.reduceFn callbackfn, long initialValue);

      public virtual extern U reduce<U>(es5.Uint32Array.reduceFn2<U> callbackfn, U initialValue);

      public virtual extern long reduceRight(es5.Uint32Array.reduceRightFn callbackfn);

      public virtual extern long reduceRight(
        es5.Uint32Array.reduceRightFn callbackfn,
        long initialValue);

      public virtual extern U reduceRight<U>(
        es5.Uint32Array.reduceRightFn2<U> callbackfn,
        U initialValue);

      public virtual extern es5.Uint32Array reverse();

      public virtual extern void set(es5.ArrayLike<uint> array);

      public virtual extern void set(es5.ArrayLike<uint> array, uint offset);

      public virtual extern es5.Uint32Array slice();

      public virtual extern es5.Uint32Array slice(uint start);

      public virtual extern es5.Uint32Array slice(uint start, uint end);

      public virtual extern bool some(es5.Uint32Array.someFn callbackfn);

      public virtual extern bool some(es5.Uint32Array.someFn callbackfn, object thisArg);

      public virtual extern es5.Uint32Array sort();

      public virtual extern es5.Uint32Array sort(es5.Uint32Array.sortFn compareFn);

      public virtual extern es5.Uint32Array subarray(uint begin);

      public virtual extern es5.Uint32Array subarray(uint begin, int end);

      public virtual extern string toLocaleString();

      public virtual extern string toString();

      public virtual extern uint this[uint index] { get; set; }

      [Template("System.Array.indexOf({this}, {item}, 0, null, {T})")]
      extern int IList<uint>.IndexOf(uint item);

      [Template("System.Array.insert({this}, {index}, {item}, {T})")]
      extern void IList<uint>.Insert(int index, uint item);

      [Template("System.Array.removeAt({this}, {index}, {T})")]
      extern void IList<uint>.RemoveAt(int index);

      extern uint IList<uint>.this[int index] { [Template("System.Array.getItem({this}, {0}, {T})")] get; [Template("System.Array.setItem({this}, {0}, {T})")] set; }

      [Template("System.Array.add({this}, {item}, {T})")]
      extern void ICollection<uint>.Add(uint item);

      [Template("System.Array.copyTo({this}, {array}, {arrayIndex}, {T})")]
      extern void ICollection<uint>.CopyTo(uint[] array, int arrayIndex);

      [Template("System.Array.clear({this}, {T})")]
      extern void ICollection<uint>.Clear();

      [Template("System.Array.contains({this}, {item}, {T})")]
      extern bool ICollection<uint>.Contains(uint item);

      [Template("System.Array.remove({this}, {item}, {T})")]
      extern bool ICollection<uint>.Remove(uint item);

      int ICollection<uint>.Count
      {
        [Template("System.Array.getCount({this}, {T})")] get
        {
          return this._System\u002ECollections\u002EGeneric\u002EICollection_System\u002EUInt32\u003E\u002ECount_BackingField;
        }
      }

      bool ICollection<uint>.IsReadOnly
      {
        [Template("System.Array.getIsReadOnly({this}, {T})")] get
        {
          return this._System\u002ECollections\u002EGeneric\u002EICollection_System\u002EUInt32\u003E\u002EIsReadOnly_BackingField;
        }
      }

      [Template("Bridge.getEnumerator({this}, {T})")]
      extern IEnumerator<uint> IEnumerable<uint>.GetEnumerator();

      [Template("Bridge.getEnumerator({this})")]
      extern IEnumerator IEnumerable.GetEnumerator();

      extern uint IReadOnlyList<uint>.this[int index] { [Template("System.Array.getItem({this}, {0}, {T})")] get; }

      int IReadOnlyCollection<uint>.Count
      {
        [Template("System.Array.getCount({this}, {T})")] get
        {
          return this._System\u002ECollections\u002EGeneric\u002EIReadOnlyCollection_System\u002EUInt32\u003E\u002ECount_BackingField;
        }
      }

      [Template("System.Array.copyTo({this}, {array}, {arrayIndex})")]
      extern void ICollection.CopyTo(Array array, int arrayIndex);

      int ICollection.Count
      {
        [Template("System.Array.getCount({this})")] get
        {
          return this._System\u002ECollections\u002EICollection\u002ECount_BackingField;
        }
      }

      object ICollection.SyncRoot
      {
        get
        {
          return this._System\u002ECollections\u002EICollection\u002ESyncRoot_BackingField;
        }
      }

      bool ICollection.IsSynchronized
      {
        get
        {
          return this._System\u002ECollections\u002EICollection\u002EIsSynchronized_BackingField;
        }
      }

      [Generated]
      public delegate bool everyFn(uint value, uint index, es5.Uint32Array array);

      [Generated]
      public delegate object filterFn(uint value, uint index, es5.Uint32Array array);

      [Generated]
      public delegate bool findFn(uint value, uint index, es5.Uint32Array obj);

      [Generated]
      public delegate bool findIndexFn(uint value, uint index, es5.Uint32Array obj);

      [Generated]
      public delegate void forEachFn(uint value, uint index, es5.Uint32Array array);

      [Generated]
      public delegate double mapFn(uint value, uint index, es5.Uint32Array array);

      [Generated]
      public delegate double reduceFn(
        long previousValue,
        uint currentValue,
        uint currentIndex,
        es5.Uint32Array array);

      [Generated]
      public delegate U reduceFn2<U>(
        U previousValue,
        uint currentValue,
        uint currentIndex,
        es5.Uint32Array array);

      [Generated]
      public delegate double reduceRightFn(
        long previousValue,
        uint currentValue,
        uint currentIndex,
        es5.Uint32Array array);

      [Generated]
      public delegate U reduceRightFn2<U>(
        U previousValue,
        uint currentValue,
        uint currentIndex,
        es5.Uint32Array array);

      [Generated]
      public delegate bool someFn(uint value, uint index, es5.Uint32Array array);

      [Generated]
      public delegate double sortFn(uint a, uint b);

      [Generated]
      public delegate double fromFn(double v, double k);
    }

    [IgnoreCast]
    [Virtual]
    [FormerInterface]
    public abstract class Uint32ArrayConstructor : IObject
    {
      public abstract es5.Uint32Array prototype { get; }

      [Template("new {this}({0})")]
      public abstract es5.Uint32Array New(uint length);

      [Template("new {this}({0})")]
      public abstract es5.Uint32Array New(
        Union<es5.ArrayLike<uint>, es5.ArrayBufferLike> arrayOrArrayBuffer);

      [Template("new {this}({0})")]
      public abstract es5.Uint32Array New(es5.ArrayLike<uint> arrayOrArrayBuffer);

      [Template("new {this}({0})")]
      public abstract es5.Uint32Array New(es5.ArrayBufferLike arrayOrArrayBuffer);

      [Template("new {this}({0})")]
      public abstract es5.Uint32Array New(es5.ArrayBuffer arrayOrArrayBuffer);

      [Template("new {this}({0}, {1})")]
      public abstract es5.Uint32Array New(es5.ArrayBufferLike buffer, uint byteOffset);

      [Template("new {this}({0}, {1})")]
      public abstract es5.Uint32Array New(es5.ArrayBuffer buffer, uint byteOffset);

      [Template("new {this}({0}, {1}, {2})")]
      public abstract es5.Uint32Array New(
        es5.ArrayBufferLike buffer,
        uint byteOffset,
        uint length);

      [Template("new {this}({0}, {1}, {2})")]
      public abstract es5.Uint32Array New(es5.ArrayBuffer buffer, uint byteOffset, uint length);

      public abstract double BYTES_PER_ELEMENT { get; }

      [ExpandParams]
      public abstract es5.Uint32Array of(params uint[] items);

      public abstract es5.Uint32Array from(es5.ArrayLike<uint> arrayLike);

      public abstract es5.Uint32Array from(
        es5.ArrayLike<uint> arrayLike,
        es5.Uint32ArrayConstructor.fromFn mapfn);

      public abstract es5.Uint32Array from(
        es5.ArrayLike<uint> arrayLike,
        es5.Uint32ArrayConstructor.fromFn mapfn,
        object thisArg);

      [Generated]
      public delegate double fromFn(double v, double k);
    }

    [CombinedClass]
    [StaticInterface("Float32ArrayConstructor")]
    [FormerInterface]
    public class Float32Array : IList<float>, ICollection<float>, IEnumerable<float>, IEnumerable, IBridgeClass, IReadOnlyList<float>, IReadOnlyCollection<float>, ICollection, IObject
    {
      private static readonly es5.Float32Array _prototype_BackingField;
      private static readonly double _BYTES_PER_ELEMENT_Static_BackingField;
      private readonly int _BYTES_PER_ELEMENT_BackingField;
      private readonly es5.ArrayBufferLike _buffer_BackingField;
      private readonly uint _byteLength_BackingField;
      private readonly uint _byteOffset_BackingField;
      private readonly uint _length_BackingField;
      private readonly int _System\u002ECollections\u002EGeneric\u002EICollection_System\u002ESingle\u003E\u002ECount_BackingField;
      private readonly bool _System\u002ECollections\u002EGeneric\u002EICollection_System\u002ESingle\u003E\u002EIsReadOnly_BackingField;
      private readonly int _System\u002ECollections\u002EGeneric\u002EIReadOnlyCollection_System\u002ESingle\u003E\u002ECount_BackingField;
      private readonly int _System\u002ECollections\u002EICollection\u002ECount_BackingField;
      private readonly object _System\u002ECollections\u002EICollection\u002ESyncRoot_BackingField;
      private readonly bool _System\u002ECollections\u002EICollection\u002EIsSynchronized_BackingField;

      public extern Float32Array(uint length);

      public extern Float32Array(
        Union<es5.ArrayLike<float>, es5.ArrayBufferLike> arrayOrArrayBuffer);

      public extern Float32Array(es5.ArrayLike<float> arrayOrArrayBuffer);

      public extern Float32Array(es5.ArrayBufferLike arrayOrArrayBuffer);

      public extern Float32Array(es5.ArrayBuffer arrayOrArrayBuffer);

      public extern Float32Array(es5.ArrayBufferLike buffer, uint byteOffset);

      public extern Float32Array(es5.ArrayBuffer buffer, uint byteOffset);

      public extern Float32Array(es5.ArrayBufferLike buffer, uint byteOffset, uint length);

      public extern Float32Array(es5.ArrayBuffer buffer, uint byteOffset, uint length);

      public static es5.Float32Array prototype
      {
        get
        {
          return es5.Float32Array._prototype_BackingField;
        }
      }

      [Name("BYTES_PER_ELEMENT")]
      public static double BYTES_PER_ELEMENT_Static
      {
        get
        {
          return es5.Float32Array._BYTES_PER_ELEMENT_Static_BackingField;
        }
      }

      [ExpandParams]
      public static extern es5.Float32Array of(params float[] items);

      public static extern es5.Float32Array from(es5.ArrayLike<float> arrayLike);

      public static extern es5.Float32Array from(
        es5.ArrayLike<float> arrayLike,
        es5.Float32Array.fromFn mapfn);

      public static extern es5.Float32Array from(
        es5.ArrayLike<float> arrayLike,
        es5.Float32Array.fromFn mapfn,
        object thisArg);

      public virtual int BYTES_PER_ELEMENT
      {
        get
        {
          return this._BYTES_PER_ELEMENT_BackingField;
        }
      }

      public virtual es5.ArrayBufferLike buffer
      {
        get
        {
          return this._buffer_BackingField;
        }
      }

      public virtual uint byteLength
      {
        get
        {
          return this._byteLength_BackingField;
        }
      }

      public virtual uint byteOffset
      {
        get
        {
          return this._byteOffset_BackingField;
        }
      }

      public virtual extern es5.Float32Array copyWithin(long target, long start);

      public virtual extern es5.Float32Array copyWithin(long target, long start, long end);

      public virtual extern bool every(es5.Float32Array.everyFn callbackfn);

      public virtual extern bool every(es5.Float32Array.everyFn callbackfn, object thisArg);

      public virtual extern es5.Float32Array fill(float value);

      public virtual extern es5.Float32Array fill(float value, uint start);

      public virtual extern es5.Float32Array fill(float value, uint start, uint end);

      public virtual extern es5.Float32Array filter(es5.Float32Array.filterFn callbackfn);

      public virtual extern es5.Float32Array filter(
        es5.Float32Array.filterFn callbackfn,
        object thisArg);

      public virtual extern Union<float, Undefined> find(
        es5.Float32Array.findFn predicate);

      public virtual extern Union<float, Undefined> find(
        es5.Float32Array.findFn predicate,
        object thisArg);

      public virtual extern uint findIndex(es5.Float32Array.findIndexFn predicate);

      public virtual extern uint findIndex(es5.Float32Array.findIndexFn predicate, object thisArg);

      public virtual extern void forEach(es5.Float32Array.forEachFn callbackfn);

      public virtual extern void forEach(es5.Float32Array.forEachFn callbackfn, object thisArg);

      public virtual extern uint indexOf(float searchElement);

      public virtual extern uint indexOf(float searchElement, uint fromIndex);

      public virtual extern string join();

      public virtual extern string join(string separator);

      public virtual extern uint lastIndexOf(float searchElement);

      public virtual extern uint lastIndexOf(float searchElement, uint fromIndex);

      public virtual uint length
      {
        get
        {
          return this._length_BackingField;
        }
      }

      public virtual extern es5.Float32Array map(es5.Float32Array.mapFn callbackfn);

      public virtual extern es5.Float32Array map(
        es5.Float32Array.mapFn callbackfn,
        object thisArg);

      public virtual extern long reduce(es5.Float32Array.reduceFn callbackfn);

      public virtual extern long reduce(es5.Float32Array.reduceFn callbackfn, long initialValue);

      public virtual extern U reduce<U>(es5.Float32Array.reduceFn2<U> callbackfn, U initialValue);

      public virtual extern long reduceRight(es5.Float32Array.reduceRightFn callbackfn);

      public virtual extern long reduceRight(
        es5.Float32Array.reduceRightFn callbackfn,
        long initialValue);

      public virtual extern U reduceRight<U>(
        es5.Float32Array.reduceRightFn2<U> callbackfn,
        U initialValue);

      public virtual extern es5.Float32Array reverse();

      public virtual extern void set(es5.ArrayLike<float> array);

      public virtual extern void set(es5.ArrayLike<float> array, uint offset);

      public virtual extern es5.Float32Array slice();

      public virtual extern es5.Float32Array slice(uint start);

      public virtual extern es5.Float32Array slice(uint start, uint end);

      public virtual extern bool some(es5.Float32Array.someFn callbackfn);

      public virtual extern bool some(es5.Float32Array.someFn callbackfn, object thisArg);

      public virtual extern es5.Float32Array sort();

      public virtual extern es5.Float32Array sort(es5.Float32Array.sortFn compareFn);

      public virtual extern es5.Float32Array subarray(uint begin);

      public virtual extern es5.Float32Array subarray(uint begin, int end);

      public virtual extern string toLocaleString();

      public virtual extern string toString();

      public virtual extern float this[uint index] { get; set; }

      [Template("System.Array.indexOf({this}, {item}, 0, null, {T})")]
      extern int IList<float>.IndexOf(float item);

      [Template("System.Array.insert({this}, {index}, {item}, {T})")]
      extern void IList<float>.Insert(int index, float item);

      [Template("System.Array.removeAt({this}, {index}, {T})")]
      extern void IList<float>.RemoveAt(int index);

      extern float IList<float>.this[int index] { [Template("System.Array.getItem({this}, {0}, {T})")] get; [Template("System.Array.setItem({this}, {0}, {T})")] set; }

      [Template("System.Array.add({this}, {item}, {T})")]
      extern void ICollection<float>.Add(float item);

      [Template("System.Array.copyTo({this}, {array}, {arrayIndex}, {T})")]
      extern void ICollection<float>.CopyTo(float[] array, int arrayIndex);

      [Template("System.Array.clear({this}, {T})")]
      extern void ICollection<float>.Clear();

      [Template("System.Array.contains({this}, {item}, {T})")]
      extern bool ICollection<float>.Contains(float item);

      [Template("System.Array.remove({this}, {item}, {T})")]
      extern bool ICollection<float>.Remove(float item);

      int ICollection<float>.Count
      {
        [Template("System.Array.getCount({this}, {T})")] get
        {
          return this._System\u002ECollections\u002EGeneric\u002EICollection_System\u002ESingle\u003E\u002ECount_BackingField;
        }
      }

      bool ICollection<float>.IsReadOnly
      {
        [Template("System.Array.getIsReadOnly({this}, {T})")] get
        {
          return this._System\u002ECollections\u002EGeneric\u002EICollection_System\u002ESingle\u003E\u002EIsReadOnly_BackingField;
        }
      }

      [Template("Bridge.getEnumerator({this}, {T})")]
      extern IEnumerator<float> IEnumerable<float>.GetEnumerator();

      [Template("Bridge.getEnumerator({this})")]
      extern IEnumerator IEnumerable.GetEnumerator();

      extern float IReadOnlyList<float>.this[int index] { [Template("System.Array.getItem({this}, {0}, {T})")] get; }

      int IReadOnlyCollection<float>.Count
      {
        [Template("System.Array.getCount({this}, {T})")] get
        {
          return this._System\u002ECollections\u002EGeneric\u002EIReadOnlyCollection_System\u002ESingle\u003E\u002ECount_BackingField;
        }
      }

      [Template("System.Array.copyTo({this}, {array}, {arrayIndex})")]
      extern void ICollection.CopyTo(Array array, int arrayIndex);

      int ICollection.Count
      {
        [Template("System.Array.getCount({this})")] get
        {
          return this._System\u002ECollections\u002EICollection\u002ECount_BackingField;
        }
      }

      object ICollection.SyncRoot
      {
        get
        {
          return this._System\u002ECollections\u002EICollection\u002ESyncRoot_BackingField;
        }
      }

      bool ICollection.IsSynchronized
      {
        get
        {
          return this._System\u002ECollections\u002EICollection\u002EIsSynchronized_BackingField;
        }
      }

      [Generated]
      public delegate bool everyFn(float value, uint index, es5.Float32Array array);

      [Generated]
      public delegate object filterFn(float value, uint index, es5.Float32Array array);

      [Generated]
      public delegate bool findFn(float value, uint index, es5.Float32Array obj);

      [Generated]
      public delegate bool findIndexFn(float value, uint index, es5.Float32Array obj);

      [Generated]
      public delegate void forEachFn(float value, uint index, es5.Float32Array array);

      [Generated]
      public delegate double mapFn(float value, uint index, es5.Float32Array array);

      [Generated]
      public delegate double reduceFn(
        long previousValue,
        float currentValue,
        uint currentIndex,
        es5.Float32Array array);

      [Generated]
      public delegate U reduceFn2<U>(
        U previousValue,
        float currentValue,
        uint currentIndex,
        es5.Float32Array array);

      [Generated]
      public delegate double reduceRightFn(
        long previousValue,
        float currentValue,
        uint currentIndex,
        es5.Float32Array array);

      [Generated]
      public delegate U reduceRightFn2<U>(
        U previousValue,
        float currentValue,
        uint currentIndex,
        es5.Float32Array array);

      [Generated]
      public delegate bool someFn(float value, uint index, es5.Float32Array array);

      [Generated]
      public delegate double sortFn(float a, float b);

      [Generated]
      public delegate double fromFn(double v, double k);
    }

    [IgnoreCast]
    [Virtual]
    [FormerInterface]
    public abstract class Float32ArrayConstructor : IObject
    {
      public abstract es5.Float32Array prototype { get; }

      [Template("new {this}({0})")]
      public abstract es5.Float32Array New(uint length);

      [Template("new {this}({0})")]
      public abstract es5.Float32Array New(
        Union<es5.ArrayLike<float>, es5.ArrayBufferLike> arrayOrArrayBuffer);

      [Template("new {this}({0})")]
      public abstract es5.Float32Array New(es5.ArrayLike<float> arrayOrArrayBuffer);

      [Template("new {this}({0})")]
      public abstract es5.Float32Array New(es5.ArrayBufferLike arrayOrArrayBuffer);

      [Template("new {this}({0})")]
      public abstract es5.Float32Array New(es5.ArrayBuffer arrayOrArrayBuffer);

      [Template("new {this}({0}, {1})")]
      public abstract es5.Float32Array New(es5.ArrayBufferLike buffer, uint byteOffset);

      [Template("new {this}({0}, {1})")]
      public abstract es5.Float32Array New(es5.ArrayBuffer buffer, uint byteOffset);

      [Template("new {this}({0}, {1}, {2})")]
      public abstract es5.Float32Array New(
        es5.ArrayBufferLike buffer,
        uint byteOffset,
        uint length);

      [Template("new {this}({0}, {1}, {2})")]
      public abstract es5.Float32Array New(es5.ArrayBuffer buffer, uint byteOffset, uint length);

      public abstract double BYTES_PER_ELEMENT { get; }

      [ExpandParams]
      public abstract es5.Float32Array of(params float[] items);

      public abstract es5.Float32Array from(es5.ArrayLike<float> arrayLike);

      public abstract es5.Float32Array from(
        es5.ArrayLike<float> arrayLike,
        es5.Float32ArrayConstructor.fromFn mapfn);

      public abstract es5.Float32Array from(
        es5.ArrayLike<float> arrayLike,
        es5.Float32ArrayConstructor.fromFn mapfn,
        object thisArg);

      [Generated]
      public delegate double fromFn(double v, double k);
    }

    [CombinedClass]
    [StaticInterface("Float64ArrayConstructor")]
    [FormerInterface]
    public class Float64Array : IList<double>, ICollection<double>, IEnumerable<double>, IEnumerable, IBridgeClass, IReadOnlyList<double>, IReadOnlyCollection<double>, ICollection, IObject
    {
      private static readonly es5.Float64Array _prototype_BackingField;
      private static readonly double _BYTES_PER_ELEMENT_Static_BackingField;
      private readonly int _BYTES_PER_ELEMENT_BackingField;
      private readonly es5.ArrayBufferLike _buffer_BackingField;
      private readonly uint _byteLength_BackingField;
      private readonly uint _byteOffset_BackingField;
      private readonly uint _length_BackingField;
      private readonly int _System\u002ECollections\u002EGeneric\u002EICollection_System\u002EDouble\u003E\u002ECount_BackingField;
      private readonly bool _System\u002ECollections\u002EGeneric\u002EICollection_System\u002EDouble\u003E\u002EIsReadOnly_BackingField;
      private readonly int _System\u002ECollections\u002EGeneric\u002EIReadOnlyCollection_System\u002EDouble\u003E\u002ECount_BackingField;
      private readonly int _System\u002ECollections\u002EICollection\u002ECount_BackingField;
      private readonly object _System\u002ECollections\u002EICollection\u002ESyncRoot_BackingField;
      private readonly bool _System\u002ECollections\u002EICollection\u002EIsSynchronized_BackingField;

      public extern Float64Array(uint length);

      public extern Float64Array(
        Union<es5.ArrayLike<double>, es5.ArrayBufferLike> arrayOrArrayBuffer);

      public extern Float64Array(es5.ArrayLike<double> arrayOrArrayBuffer);

      public extern Float64Array(es5.ArrayBufferLike arrayOrArrayBuffer);

      public extern Float64Array(es5.ArrayBuffer arrayOrArrayBuffer);

      public extern Float64Array(es5.ArrayBufferLike buffer, uint byteOffset);

      public extern Float64Array(es5.ArrayBuffer buffer, uint byteOffset);

      public extern Float64Array(es5.ArrayBufferLike buffer, uint byteOffset, uint length);

      public extern Float64Array(es5.ArrayBuffer buffer, uint byteOffset, uint length);

      public static es5.Float64Array prototype
      {
        get
        {
          return es5.Float64Array._prototype_BackingField;
        }
      }

      [Name("BYTES_PER_ELEMENT")]
      public static double BYTES_PER_ELEMENT_Static
      {
        get
        {
          return es5.Float64Array._BYTES_PER_ELEMENT_Static_BackingField;
        }
      }

      [ExpandParams]
      public static extern es5.Float64Array of(params double[] items);

      public static extern es5.Float64Array from(es5.ArrayLike<double> arrayLike);

      public static extern es5.Float64Array from(
        es5.ArrayLike<double> arrayLike,
        es5.Float64Array.fromFn mapfn);

      public static extern es5.Float64Array from(
        es5.ArrayLike<double> arrayLike,
        es5.Float64Array.fromFn mapfn,
        object thisArg);

      public virtual int BYTES_PER_ELEMENT
      {
        get
        {
          return this._BYTES_PER_ELEMENT_BackingField;
        }
      }

      public virtual es5.ArrayBufferLike buffer
      {
        get
        {
          return this._buffer_BackingField;
        }
      }

      public virtual uint byteLength
      {
        get
        {
          return this._byteLength_BackingField;
        }
      }

      public virtual uint byteOffset
      {
        get
        {
          return this._byteOffset_BackingField;
        }
      }

      public virtual extern es5.Float64Array copyWithin(long target, long start);

      public virtual extern es5.Float64Array copyWithin(long target, long start, long end);

      public virtual extern bool every(es5.Float64Array.everyFn callbackfn);

      public virtual extern bool every(es5.Float64Array.everyFn callbackfn, object thisArg);

      public virtual extern es5.Float64Array fill(double value);

      public virtual extern es5.Float64Array fill(double value, uint start);

      public virtual extern es5.Float64Array fill(double value, uint start, uint end);

      public virtual extern es5.Float64Array filter(es5.Float64Array.filterFn callbackfn);

      public virtual extern es5.Float64Array filter(
        es5.Float64Array.filterFn callbackfn,
        object thisArg);

      public virtual extern Union<double, Undefined> find(
        es5.Float64Array.findFn predicate);

      public virtual extern Union<double, Undefined> find(
        es5.Float64Array.findFn predicate,
        object thisArg);

      public virtual extern uint findIndex(es5.Float64Array.findIndexFn predicate);

      public virtual extern uint findIndex(es5.Float64Array.findIndexFn predicate, object thisArg);

      public virtual extern void forEach(es5.Float64Array.forEachFn callbackfn);

      public virtual extern void forEach(es5.Float64Array.forEachFn callbackfn, object thisArg);

      public virtual extern uint indexOf(double searchElement);

      public virtual extern uint indexOf(double searchElement, uint fromIndex);

      public virtual extern string join();

      public virtual extern string join(string separator);

      public virtual extern uint lastIndexOf(double searchElement);

      public virtual extern uint lastIndexOf(double searchElement, uint fromIndex);

      public virtual uint length
      {
        get
        {
          return this._length_BackingField;
        }
      }

      public virtual extern es5.Float64Array map(es5.Float64Array.mapFn callbackfn);

      public virtual extern es5.Float64Array map(
        es5.Float64Array.mapFn callbackfn,
        object thisArg);

      public virtual extern long reduce(es5.Float64Array.reduceFn callbackfn);

      public virtual extern long reduce(es5.Float64Array.reduceFn callbackfn, long initialValue);

      public virtual extern U reduce<U>(es5.Float64Array.reduceFn2<U> callbackfn, U initialValue);

      public virtual extern long reduceRight(es5.Float64Array.reduceRightFn callbackfn);

      public virtual extern long reduceRight(
        es5.Float64Array.reduceRightFn callbackfn,
        long initialValue);

      public virtual extern U reduceRight<U>(
        es5.Float64Array.reduceRightFn2<U> callbackfn,
        U initialValue);

      public virtual extern es5.Float64Array reverse();

      public virtual extern void set(es5.ArrayLike<double> array);

      public virtual extern void set(es5.ArrayLike<double> array, uint offset);

      public virtual extern es5.Float64Array slice();

      public virtual extern es5.Float64Array slice(uint start);

      public virtual extern es5.Float64Array slice(uint start, uint end);

      public virtual extern bool some(es5.Float64Array.someFn callbackfn);

      public virtual extern bool some(es5.Float64Array.someFn callbackfn, object thisArg);

      public virtual extern es5.Float64Array sort();

      public virtual extern es5.Float64Array sort(es5.Float64Array.sortFn compareFn);

      public virtual extern es5.Float64Array subarray(uint begin);

      public virtual extern es5.Float64Array subarray(uint begin, int end);

      public virtual extern string toLocaleString();

      public virtual extern string toString();

      public virtual extern double this[uint index] { get; set; }

      [Template("System.Array.indexOf({this}, {item}, 0, null, {T})")]
      extern int IList<double>.IndexOf(double item);

      [Template("System.Array.insert({this}, {index}, {item}, {T})")]
      extern void IList<double>.Insert(int index, double item);

      [Template("System.Array.removeAt({this}, {index}, {T})")]
      extern void IList<double>.RemoveAt(int index);

      extern double IList<double>.this[int index] { [Template("System.Array.getItem({this}, {0}, {T})")] get; [Template("System.Array.setItem({this}, {0}, {T})")] set; }

      [Template("System.Array.add({this}, {item}, {T})")]
      extern void ICollection<double>.Add(double item);

      [Template("System.Array.copyTo({this}, {array}, {arrayIndex}, {T})")]
      extern void ICollection<double>.CopyTo(double[] array, int arrayIndex);

      [Template("System.Array.clear({this}, {T})")]
      extern void ICollection<double>.Clear();

      [Template("System.Array.contains({this}, {item}, {T})")]
      extern bool ICollection<double>.Contains(double item);

      [Template("System.Array.remove({this}, {item}, {T})")]
      extern bool ICollection<double>.Remove(double item);

      int ICollection<double>.Count
      {
        [Template("System.Array.getCount({this}, {T})")] get
        {
          return this._System\u002ECollections\u002EGeneric\u002EICollection_System\u002EDouble\u003E\u002ECount_BackingField;
        }
      }

      bool ICollection<double>.IsReadOnly
      {
        [Template("System.Array.getIsReadOnly({this}, {T})")] get
        {
          return this._System\u002ECollections\u002EGeneric\u002EICollection_System\u002EDouble\u003E\u002EIsReadOnly_BackingField;
        }
      }

      [Template("Bridge.getEnumerator({this}, {T})")]
      extern IEnumerator<double> IEnumerable<double>.GetEnumerator();

      [Template("Bridge.getEnumerator({this})")]
      extern IEnumerator IEnumerable.GetEnumerator();

      extern double IReadOnlyList<double>.this[int index] { [Template("System.Array.getItem({this}, {0}, {T})")] get; }

      int IReadOnlyCollection<double>.Count
      {
        [Template("System.Array.getCount({this}, {T})")] get
        {
          return this._System\u002ECollections\u002EGeneric\u002EIReadOnlyCollection_System\u002EDouble\u003E\u002ECount_BackingField;
        }
      }

      [Template("System.Array.copyTo({this}, {array}, {arrayIndex})")]
      extern void ICollection.CopyTo(Array array, int arrayIndex);

      int ICollection.Count
      {
        [Template("System.Array.getCount({this})")] get
        {
          return this._System\u002ECollections\u002EICollection\u002ECount_BackingField;
        }
      }

      object ICollection.SyncRoot
      {
        get
        {
          return this._System\u002ECollections\u002EICollection\u002ESyncRoot_BackingField;
        }
      }

      bool ICollection.IsSynchronized
      {
        get
        {
          return this._System\u002ECollections\u002EICollection\u002EIsSynchronized_BackingField;
        }
      }

      [Generated]
      public delegate bool everyFn(double value, uint index, es5.Float64Array array);

      [Generated]
      public delegate object filterFn(double value, uint index, es5.Float64Array array);

      [Generated]
      public delegate bool findFn(double value, uint index, es5.Float64Array obj);

      [Generated]
      public delegate bool findIndexFn(double value, uint index, es5.Float64Array obj);

      [Generated]
      public delegate void forEachFn(double value, uint index, es5.Float64Array array);

      [Generated]
      public delegate double mapFn(double value, uint index, es5.Float64Array array);

      [Generated]
      public delegate double reduceFn(
        long previousValue,
        double currentValue,
        uint currentIndex,
        es5.Float64Array array);

      [Generated]
      public delegate U reduceFn2<U>(
        U previousValue,
        double currentValue,
        uint currentIndex,
        es5.Float64Array array);

      [Generated]
      public delegate double reduceRightFn(
        long previousValue,
        double currentValue,
        uint currentIndex,
        es5.Float64Array array);

      [Generated]
      public delegate U reduceRightFn2<U>(
        U previousValue,
        double currentValue,
        uint currentIndex,
        es5.Float64Array array);

      [Generated]
      public delegate bool someFn(double value, uint index, es5.Float64Array array);

      [Generated]
      public delegate double sortFn(double a, double b);

      [Generated]
      public delegate double fromFn(double v, double k);
    }

    [IgnoreCast]
    [Virtual]
    [FormerInterface]
    public abstract class Float64ArrayConstructor : IObject
    {
      public abstract es5.Float64Array prototype { get; }

      [Template("new {this}({0})")]
      public abstract es5.Float64Array New(uint length);

      [Template("new {this}({0})")]
      public abstract es5.Float64Array New(
        Union<es5.ArrayLike<double>, es5.ArrayBufferLike> arrayOrArrayBuffer);

      [Template("new {this}({0})")]
      public abstract es5.Float64Array New(es5.ArrayLike<double> arrayOrArrayBuffer);

      [Template("new {this}({0})")]
      public abstract es5.Float64Array New(es5.ArrayBufferLike arrayOrArrayBuffer);

      [Template("new {this}({0})")]
      public abstract es5.Float64Array New(es5.ArrayBuffer arrayOrArrayBuffer);

      [Template("new {this}({0}, {1})")]
      public abstract es5.Float64Array New(es5.ArrayBufferLike buffer, uint byteOffset);

      [Template("new {this}({0}, {1})")]
      public abstract es5.Float64Array New(es5.ArrayBuffer buffer, uint byteOffset);

      [Template("new {this}({0}, {1}, {2})")]
      public abstract es5.Float64Array New(
        es5.ArrayBufferLike buffer,
        uint byteOffset,
        uint length);

      [Template("new {this}({0}, {1}, {2})")]
      public abstract es5.Float64Array New(es5.ArrayBuffer buffer, uint byteOffset, uint length);

      public abstract double BYTES_PER_ELEMENT { get; }

      [ExpandParams]
      public abstract es5.Float64Array of(params double[] items);

      public abstract es5.Float64Array from(es5.ArrayLike<double> arrayLike);

      public abstract es5.Float64Array from(
        es5.ArrayLike<double> arrayLike,
        es5.Float64ArrayConstructor.fromFn mapfn);

      public abstract es5.Float64Array from(
        es5.ArrayLike<double> arrayLike,
        es5.Float64ArrayConstructor.fromFn mapfn,
        object thisArg);

      [Generated]
      public delegate double fromFn(double v, double k);
    }

    [Scope]
    public static class Intl
    {
      private static es5.Intl.CollatorTypeConfig _CollatorType_BackingField;
      private static es5.Intl.NumberFormatTypeConfig _NumberFormatType_BackingField;
      private static es5.Intl.DateTimeFormatTypeConfig _DateTimeFormatType_BackingField;

      [Name("Collator")]
      public static es5.Intl.CollatorTypeConfig CollatorType
      {
        get
        {
          return es5.Intl._CollatorType_BackingField;
        }
        set
        {
          es5.Intl._CollatorType_BackingField = value;
        }
      }

      [Name("NumberFormat")]
      public static es5.Intl.NumberFormatTypeConfig NumberFormatType
      {
        get
        {
          return es5.Intl._NumberFormatType_BackingField;
        }
        set
        {
          es5.Intl._NumberFormatType_BackingField = value;
        }
      }

      [Name("DateTimeFormat")]
      public static es5.Intl.DateTimeFormatTypeConfig DateTimeFormatType
      {
        get
        {
          return es5.Intl._DateTimeFormatType_BackingField;
        }
        set
        {
          es5.Intl._DateTimeFormatType_BackingField = value;
        }
      }

      [IgnoreCast]
      [ObjectLiteral]
      [FormerInterface]
      public class CollatorOptions : IObject
      {
        private string _usage_BackingField;
        private string _localeMatcher_BackingField;
        private bool? _numeric_BackingField;
        private string _caseFirst_BackingField;
        private string _sensitivity_BackingField;
        private bool? _ignorePunctuation_BackingField;

        public string usage
        {
          get
          {
            return this._usage_BackingField;
          }
          set
          {
            this._usage_BackingField = value;
          }
        }

        public string localeMatcher
        {
          get
          {
            return this._localeMatcher_BackingField;
          }
          set
          {
            this._localeMatcher_BackingField = value;
          }
        }

        public bool? numeric
        {
          get
          {
            return this._numeric_BackingField;
          }
          set
          {
            this._numeric_BackingField = value;
          }
        }

        public string caseFirst
        {
          get
          {
            return this._caseFirst_BackingField;
          }
          set
          {
            this._caseFirst_BackingField = value;
          }
        }

        public string sensitivity
        {
          get
          {
            return this._sensitivity_BackingField;
          }
          set
          {
            this._sensitivity_BackingField = value;
          }
        }

        public bool? ignorePunctuation
        {
          get
          {
            return this._ignorePunctuation_BackingField;
          }
          set
          {
            this._ignorePunctuation_BackingField = value;
          }
        }
      }

      [IgnoreCast]
      [ObjectLiteral]
      [FormerInterface]
      public class ResolvedCollatorOptions : IObject
      {
        private string _locale_BackingField;
        private string _usage_BackingField;
        private string _sensitivity_BackingField;
        private bool _ignorePunctuation_BackingField;
        private string _collation_BackingField;
        private string _caseFirst_BackingField;
        private bool _numeric_BackingField;

        public string locale
        {
          get
          {
            return this._locale_BackingField;
          }
          set
          {
            this._locale_BackingField = value;
          }
        }

        public string usage
        {
          get
          {
            return this._usage_BackingField;
          }
          set
          {
            this._usage_BackingField = value;
          }
        }

        public string sensitivity
        {
          get
          {
            return this._sensitivity_BackingField;
          }
          set
          {
            this._sensitivity_BackingField = value;
          }
        }

        public bool ignorePunctuation
        {
          get
          {
            return this._ignorePunctuation_BackingField;
          }
          set
          {
            this._ignorePunctuation_BackingField = value;
          }
        }

        public string collation
        {
          get
          {
            return this._collation_BackingField;
          }
          set
          {
            this._collation_BackingField = value;
          }
        }

        public string caseFirst
        {
          get
          {
            return this._caseFirst_BackingField;
          }
          set
          {
            this._caseFirst_BackingField = value;
          }
        }

        public bool numeric
        {
          get
          {
            return this._numeric_BackingField;
          }
          set
          {
            this._numeric_BackingField = value;
          }
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
        private string _localeMatcher_BackingField;
        private string _style_BackingField;
        private string _currency_BackingField;
        private string _currencyDisplay_BackingField;
        private bool? _useGrouping_BackingField;
        private double? _minimumIntegerDigits_BackingField;
        private double? _minimumFractionDigits_BackingField;
        private double? _maximumFractionDigits_BackingField;
        private double? _minimumSignificantDigits_BackingField;
        private double? _maximumSignificantDigits_BackingField;

        public string localeMatcher
        {
          get
          {
            return this._localeMatcher_BackingField;
          }
          set
          {
            this._localeMatcher_BackingField = value;
          }
        }

        public string style
        {
          get
          {
            return this._style_BackingField;
          }
          set
          {
            this._style_BackingField = value;
          }
        }

        public string currency
        {
          get
          {
            return this._currency_BackingField;
          }
          set
          {
            this._currency_BackingField = value;
          }
        }

        public string currencyDisplay
        {
          get
          {
            return this._currencyDisplay_BackingField;
          }
          set
          {
            this._currencyDisplay_BackingField = value;
          }
        }

        public bool? useGrouping
        {
          get
          {
            return this._useGrouping_BackingField;
          }
          set
          {
            this._useGrouping_BackingField = value;
          }
        }

        public double? minimumIntegerDigits
        {
          get
          {
            return this._minimumIntegerDigits_BackingField;
          }
          set
          {
            this._minimumIntegerDigits_BackingField = value;
          }
        }

        public double? minimumFractionDigits
        {
          get
          {
            return this._minimumFractionDigits_BackingField;
          }
          set
          {
            this._minimumFractionDigits_BackingField = value;
          }
        }

        public double? maximumFractionDigits
        {
          get
          {
            return this._maximumFractionDigits_BackingField;
          }
          set
          {
            this._maximumFractionDigits_BackingField = value;
          }
        }

        public double? minimumSignificantDigits
        {
          get
          {
            return this._minimumSignificantDigits_BackingField;
          }
          set
          {
            this._minimumSignificantDigits_BackingField = value;
          }
        }

        public double? maximumSignificantDigits
        {
          get
          {
            return this._maximumSignificantDigits_BackingField;
          }
          set
          {
            this._maximumSignificantDigits_BackingField = value;
          }
        }
      }

      [IgnoreCast]
      [ObjectLiteral]
      [FormerInterface]
      public class ResolvedNumberFormatOptions : IObject
      {
        private string _locale_BackingField;
        private string _numberingSystem_BackingField;
        private string _style_BackingField;
        private string _currency_BackingField;
        private string _currencyDisplay_BackingField;
        private double _minimumIntegerDigits_BackingField;
        private double _minimumFractionDigits_BackingField;
        private double _maximumFractionDigits_BackingField;
        private double? _minimumSignificantDigits_BackingField;
        private double? _maximumSignificantDigits_BackingField;
        private bool _useGrouping_BackingField;

        public string locale
        {
          get
          {
            return this._locale_BackingField;
          }
          set
          {
            this._locale_BackingField = value;
          }
        }

        public string numberingSystem
        {
          get
          {
            return this._numberingSystem_BackingField;
          }
          set
          {
            this._numberingSystem_BackingField = value;
          }
        }

        public string style
        {
          get
          {
            return this._style_BackingField;
          }
          set
          {
            this._style_BackingField = value;
          }
        }

        public string currency
        {
          get
          {
            return this._currency_BackingField;
          }
          set
          {
            this._currency_BackingField = value;
          }
        }

        public string currencyDisplay
        {
          get
          {
            return this._currencyDisplay_BackingField;
          }
          set
          {
            this._currencyDisplay_BackingField = value;
          }
        }

        public double minimumIntegerDigits
        {
          get
          {
            return this._minimumIntegerDigits_BackingField;
          }
          set
          {
            this._minimumIntegerDigits_BackingField = value;
          }
        }

        public double minimumFractionDigits
        {
          get
          {
            return this._minimumFractionDigits_BackingField;
          }
          set
          {
            this._minimumFractionDigits_BackingField = value;
          }
        }

        public double maximumFractionDigits
        {
          get
          {
            return this._maximumFractionDigits_BackingField;
          }
          set
          {
            this._maximumFractionDigits_BackingField = value;
          }
        }

        public double? minimumSignificantDigits
        {
          get
          {
            return this._minimumSignificantDigits_BackingField;
          }
          set
          {
            this._minimumSignificantDigits_BackingField = value;
          }
        }

        public double? maximumSignificantDigits
        {
          get
          {
            return this._maximumSignificantDigits_BackingField;
          }
          set
          {
            this._maximumSignificantDigits_BackingField = value;
          }
        }

        public bool useGrouping
        {
          get
          {
            return this._useGrouping_BackingField;
          }
          set
          {
            this._useGrouping_BackingField = value;
          }
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
        private string _localeMatcher_BackingField;
        private string _weekday_BackingField;
        private string _era_BackingField;
        private string _year_BackingField;
        private string _month_BackingField;
        private string _day_BackingField;
        private string _hour_BackingField;
        private string _minute_BackingField;
        private string _second_BackingField;
        private string _timeZoneName_BackingField;
        private string _formatMatcher_BackingField;
        private bool? _hour12_BackingField;
        private string _timeZone_BackingField;

        public string localeMatcher
        {
          get
          {
            return this._localeMatcher_BackingField;
          }
          set
          {
            this._localeMatcher_BackingField = value;
          }
        }

        public string weekday
        {
          get
          {
            return this._weekday_BackingField;
          }
          set
          {
            this._weekday_BackingField = value;
          }
        }

        public string era
        {
          get
          {
            return this._era_BackingField;
          }
          set
          {
            this._era_BackingField = value;
          }
        }

        public string year
        {
          get
          {
            return this._year_BackingField;
          }
          set
          {
            this._year_BackingField = value;
          }
        }

        public string month
        {
          get
          {
            return this._month_BackingField;
          }
          set
          {
            this._month_BackingField = value;
          }
        }

        public string day
        {
          get
          {
            return this._day_BackingField;
          }
          set
          {
            this._day_BackingField = value;
          }
        }

        public string hour
        {
          get
          {
            return this._hour_BackingField;
          }
          set
          {
            this._hour_BackingField = value;
          }
        }

        public string minute
        {
          get
          {
            return this._minute_BackingField;
          }
          set
          {
            this._minute_BackingField = value;
          }
        }

        public string second
        {
          get
          {
            return this._second_BackingField;
          }
          set
          {
            this._second_BackingField = value;
          }
        }

        public string timeZoneName
        {
          get
          {
            return this._timeZoneName_BackingField;
          }
          set
          {
            this._timeZoneName_BackingField = value;
          }
        }

        public string formatMatcher
        {
          get
          {
            return this._formatMatcher_BackingField;
          }
          set
          {
            this._formatMatcher_BackingField = value;
          }
        }

        public bool? hour12
        {
          get
          {
            return this._hour12_BackingField;
          }
          set
          {
            this._hour12_BackingField = value;
          }
        }

        public string timeZone
        {
          get
          {
            return this._timeZone_BackingField;
          }
          set
          {
            this._timeZone_BackingField = value;
          }
        }
      }

      [IgnoreCast]
      [ObjectLiteral]
      [FormerInterface]
      public class ResolvedDateTimeFormatOptions : IObject
      {
        private string _locale_BackingField;
        private string _calendar_BackingField;
        private string _numberingSystem_BackingField;
        private string _timeZone_BackingField;
        private bool? _hour12_BackingField;
        private string _weekday_BackingField;
        private string _era_BackingField;
        private string _year_BackingField;
        private string _month_BackingField;
        private string _day_BackingField;
        private string _hour_BackingField;
        private string _minute_BackingField;
        private string _second_BackingField;
        private string _timeZoneName_BackingField;

        public string locale
        {
          get
          {
            return this._locale_BackingField;
          }
          set
          {
            this._locale_BackingField = value;
          }
        }

        public string calendar
        {
          get
          {
            return this._calendar_BackingField;
          }
          set
          {
            this._calendar_BackingField = value;
          }
        }

        public string numberingSystem
        {
          get
          {
            return this._numberingSystem_BackingField;
          }
          set
          {
            this._numberingSystem_BackingField = value;
          }
        }

        public string timeZone
        {
          get
          {
            return this._timeZone_BackingField;
          }
          set
          {
            this._timeZone_BackingField = value;
          }
        }

        public bool? hour12
        {
          get
          {
            return this._hour12_BackingField;
          }
          set
          {
            this._hour12_BackingField = value;
          }
        }

        public string weekday
        {
          get
          {
            return this._weekday_BackingField;
          }
          set
          {
            this._weekday_BackingField = value;
          }
        }

        public string era
        {
          get
          {
            return this._era_BackingField;
          }
          set
          {
            this._era_BackingField = value;
          }
        }

        public string year
        {
          get
          {
            return this._year_BackingField;
          }
          set
          {
            this._year_BackingField = value;
          }
        }

        public string month
        {
          get
          {
            return this._month_BackingField;
          }
          set
          {
            this._month_BackingField = value;
          }
        }

        public string day
        {
          get
          {
            return this._day_BackingField;
          }
          set
          {
            this._day_BackingField = value;
          }
        }

        public string hour
        {
          get
          {
            return this._hour_BackingField;
          }
          set
          {
            this._hour_BackingField = value;
          }
        }

        public string minute
        {
          get
          {
            return this._minute_BackingField;
          }
          set
          {
            this._minute_BackingField = value;
          }
        }

        public string second
        {
          get
          {
            return this._second_BackingField;
          }
          set
          {
            this._second_BackingField = value;
          }
        }

        public string timeZoneName
        {
          get
          {
            return this._timeZoneName_BackingField;
          }
          set
          {
            this._timeZoneName_BackingField = value;
          }
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

    public abstract class ClassDecorator : IObject
    {
      [Template("{this}({0})")]
      [Where("TFunction", typeof (es5.Function), EnableImplicitConversion = true)]
      public extern Union<TFunction, H5.Primitive.Void> Invoke<TFunction>(
        TFunction target);
    }

    public delegate void PropertyDecorator(H5.Primitive.Object target, Union<string, symbol> propertyKey);

    public abstract class MethodDecorator : IObject
    {
      [Template("{this}({0}, {1}, {2})")]
      public extern Union<es5.TypedPropertyDescriptor<T>, H5.Primitive.Void> Invoke<T>(
        H5.Primitive.Object target,
        Union<string, symbol> propertyKey,
        es5.TypedPropertyDescriptor<T> descriptor);

      [Template("{this}({0}, {1}, {2})")]
      public extern Union<es5.TypedPropertyDescriptor<T>, H5.Primitive.Void> Invoke<T>(
        H5.Primitive.Object target,
        string propertyKey,
        es5.TypedPropertyDescriptor<T> descriptor);

      [Template("{this}({0}, {1}, {2})")]
      public extern Union<es5.TypedPropertyDescriptor<T>, H5.Primitive.Void> Invoke<T>(
        H5.Primitive.Object target,
        symbol propertyKey,
        es5.TypedPropertyDescriptor<T> descriptor);
    }

    public delegate void ParameterDecorator(
      H5.Primitive.Object target,
      Union<string, symbol> propertyKey,
      double parameterIndex);

    public delegate es5.PromiseLike<T> PromiseConstructorLike<T>(
      es5.PromiseConstructorLikeFn<T> executor);

    [IgnoreGeneric(AllowInTypeScript = true)]
    [ObjectLiteral]
    public class Partial<T> : IObject
    {
      public extern object this[KeyOf<T> P] { get; set; }
    }

    [IgnoreGeneric(AllowInTypeScript = true)]
    [ObjectLiteral]
    public class Required<T> : IObject
    {
      private object _Minus_BackingField;

      public extern object this[KeyOf<T> P] { get; set; }

      [Name("-")]
      public object Minus
      {
        get
        {
          return this._Minus_BackingField;
        }
        set
        {
          this._Minus_BackingField = value;
        }
      }
    }

    [IgnoreGeneric(AllowInTypeScript = true)]
    [Virtual]
    public abstract class Readonly<T> : IObject
    {
      public abstract object this[KeyOf<T> P] { get; }
    }

    [IgnoreGeneric(AllowInTypeScript = true)]
    [ObjectLiteral]
    [Where("K", new string[] {"KeyOf"}, EnableImplicitConversion = true)]
    public class Pick<T, K> : IObject
    {
      public new extern object this[string P] { get; set; }
    }

    [IgnoreGeneric(AllowInTypeScript = true)]
    [ObjectLiteral]
    [Where("K", typeof (H5.Primitive.String.Interface), EnableImplicitConversion = true)]
    public class Record<K, T> : IObject
    {
      public extern T this[string P] { get; set; }
    }

    [IgnoreGeneric(AllowInTypeScript = true)]
    [IgnoreCast]
    [Virtual]
    public abstract class Exclude<T, U> : TypeAlias<Union<Never, T>>
    {
      public static extern implicit operator es5.Exclude<T, U>(Union<Never, T> value);

      [Template("{0}")]
      public static extern es5.Exclude<T, U> Create(Never value);

      [Template("{0}")]
      public static extern es5.Exclude<T, U> Create(T value);

      public static extern implicit operator es5.Exclude<T, U>(Never value);

      public static extern implicit operator es5.Exclude<T, U>(T value);

      public static extern explicit operator Never(es5.Exclude<T, U> value);

      public static extern explicit operator T(es5.Exclude<T, U> value);
    }

    [IgnoreGeneric(AllowInTypeScript = true)]
    [IgnoreCast]
    [Virtual]
    public abstract class Extract<T, U> : TypeAlias<Union<T, Never>>
    {
      public static extern implicit operator es5.Extract<T, U>(Union<T, Never> value);

      [Template("{0}")]
      public static extern es5.Extract<T, U> Create(T value);

      [Template("{0}")]
      public static extern es5.Extract<T, U> Create(Never value);

      public static extern implicit operator es5.Extract<T, U>(T value);

      public static extern implicit operator es5.Extract<T, U>(Never value);

      public static extern explicit operator T(es5.Extract<T, U> value);

      public static extern explicit operator Never(es5.Extract<T, U> value);
    }

    [IgnoreGeneric(AllowInTypeScript = true)]
    [IgnoreCast]
    [Virtual]
    public abstract class NonNullable<T> : TypeAlias<Union<Never, T>>
    {
      public static extern implicit operator es5.NonNullable<T>(Union<Never, T> value);

      [Template("{0}")]
      public static extern es5.NonNullable<T> Create(Never value);

      [Template("{0}")]
      public static extern es5.NonNullable<T> Create(T value);

      public static extern implicit operator es5.NonNullable<T>(Never value);

      public static extern implicit operator es5.NonNullable<T>(T value);

      public static extern explicit operator Never(es5.NonNullable<T> value);

      public static extern explicit operator T(es5.NonNullable<T> value);
    }

    [IgnoreGeneric(AllowInTypeScript = true)]
    [IgnoreCast]
    [Virtual]
    [Where("T", new string[] {"ReturnTypeFnAlias"}, EnableImplicitConversion = true)]
    public abstract class ReturnType<T> : TypeAlias<Union<object, object>>
    {
      public static extern implicit operator es5.ReturnType<T>(Union<object, object> value);

      [Template("{0}")]
      public static extern es5.ReturnType<T> Create(object value);

      [Generated]
      public delegate object ReturnTypeFn<T>(params object[] args);

      [IgnoreGeneric(AllowInTypeScript = true)]
      [IgnoreCast]
      [Virtual]
      public abstract class ReturnTypeFnAlias<T> : TypeAlias<es5.ReturnType<T>.ReturnTypeFn<T>>
      {
        public static extern implicit operator es5.ReturnType<T>.ReturnTypeFnAlias<T>(
          es5.ReturnType<T>.ReturnTypeFn<T> value);
      }
    }

    [IgnoreGeneric(AllowInTypeScript = true)]
    [IgnoreCast]
    [Virtual]
    [Where("T", new string[] {"InstanceTypeCtorFnAlias"}, EnableImplicitConversion = true)]
    public abstract class InstanceType<T> : TypeAlias<Union<object, object>>
    {
      public static extern implicit operator es5.InstanceType<T>(Union<object, object> value);

      [Template("{0}")]
      public static extern es5.InstanceType<T> Create(object value);

      [Generated]
      [IgnoreGeneric(AllowInTypeScript = true)]
      public class InstanceTypeCtorFn<T> : CtorDelegate
      {
        [Template("({expr}.body.t[{expr}.body.constructor.sn] || {expr}.body.t)")]
        public extern InstanceTypeCtorFn(
          Expression<es5.InstanceType<T>.InstanceTypeCtorFn<T>.InstanceTypeCtorFnDelegate> expr);

        [Template("new {this}({0})")]
        public extern object Invoke(params object[] args);

        [Generated]
        public delegate object InstanceTypeCtorFnDelegate(params object[] args);
      }

      [IgnoreGeneric(AllowInTypeScript = true)]
      [IgnoreCast]
      [Virtual]
      public abstract class InstanceTypeCtorFnAlias<T> : TypeAlias<es5.InstanceType<T>.InstanceTypeCtorFn<T>>
      {
        public static extern implicit operator es5.InstanceType<T>.InstanceTypeCtorFnAlias<T>(
          es5.InstanceType<T>.InstanceTypeCtorFn<T> value);
      }
    }

    [IgnoreCast]
    [Virtual]
    public abstract class ArrayBufferLike : TypeAlias<es5.ArrayBuffer>
    {
      public static extern implicit operator es5.ArrayBufferLike(es5.ArrayBuffer arg);
    }

    [Generated]
    public delegate void PromiseConstructorLikeFn2<T>(Union<T, es5.PromiseLike<T>> value);

    [Generated]
    public delegate void PromiseConstructorLikeFn3<T>(object reason);

    [Generated]
    public delegate void PromiseConstructorLikeFn<T>(
      es5.PromiseConstructorLikeFn2<T> resolve,
      es5.PromiseConstructorLikeFn3<T> reject);
  }
}
