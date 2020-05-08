// Decompiled with JetBrains decompiler
// Type: Object.Net.Utilities.DateTimeUtils
// Assembly: Object.Net.Utilities, Version=2.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0B5927F8-8BAA-4C4A-84B2-E1600C4FD3FC
// Assembly location: C:\h5\2020-04-30\h5\packages\Object.NET.Utilities.2.5.0\lib\Object.Net.Utilities.dll

using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Object.Net.Utilities
{
  /// <summary>
  /// 
  /// </summary>
  public static class DateTimeUtils
  {
    /// <summary>Returns an ISO 8601 formatted DateTime string</summary>
    /// <param name="instance">The DateTime object to format</param>
    /// <returns>The ISO 8601 formatted string</returns>
    public static string ToISOString(this DateTime instance)
    {
      return instance.ToString("yyyy-MM-ddTHH:mm:ss");
    }

    /// <summary>
    /// Returns the Date portion only of an ISO 8601 formatted DateTime string
    /// </summary>
    /// <param name="instance">The DateTime object to format</param>
    /// <returns>The ISO 8601 formatted string</returns>
    public static string ToISODateString(this DateTime instance)
    {
      return instance.ToString("yyyy-MM-dd");
    }

    /// <summary>
    /// Returns the Time portion only of an ISO 8601 formatted DateTime string
    /// </summary>
    /// <param name="instance">The DateTime object to format</param>
    /// <returns>The ISO 8601 formatted string</returns>
    public static string ToISOTimeString(this DateTime instance)
    {
      return instance.ToString("HH:mm:ss");
    }

    /// <summary>
    /// Accepts a Unix/PHP date string format and returns a valid .NET date format
    /// </summary>
    /// <param name="format">The PHP format string</param>
    /// <returns>The format string converted to .NET DateTime format specifiers</returns>
    public static string ConvertPHPToNet(string format)
    {
      if (string.IsNullOrEmpty(format))
        return "";
      StringBuilder stringBuilder = new StringBuilder(128);
      for (Match match = Regex.Match(format, "(%|\\\\)?.|%%", RegexOptions.IgnoreCase); match.Success; match = match.NextMatch())
      {
        string str = match.Value;
        if (str.StartsWith("\\") || str.StartsWith("%%"))
          stringBuilder.Append(str.Replace("\\", "").Replace("%%", "%"));
        switch (str)
        {
          case "d":
            stringBuilder.Append("dd");
            break;
          case "D":
            stringBuilder.Append("ddd");
            break;
          case "j":
            stringBuilder.Append("d");
            break;
          case "l":
            stringBuilder.Append("dddd");
            break;
          case "F":
            stringBuilder.Append("MMMM");
            break;
          case "m":
            stringBuilder.Append("MM");
            break;
          case "M":
            stringBuilder.Append("MMM");
            break;
          case "n":
            stringBuilder.Append("M");
            break;
          case "Y":
            stringBuilder.Append("yyyy");
            break;
          case "y":
            stringBuilder.Append("yy");
            break;
          case "a":
          case "A":
            stringBuilder.Append("tt");
            break;
          case "g":
            stringBuilder.Append("h");
            break;
          case "G":
            stringBuilder.Append("H");
            break;
          case "h":
            stringBuilder.Append("hh");
            break;
          case "H":
            stringBuilder.Append("HH");
            break;
          case "i":
            stringBuilder.Append("mm");
            break;
          case "s":
            stringBuilder.Append("ss");
            break;
          default:
            stringBuilder.Append(str);
            break;
        }
      }
      return stringBuilder.ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    public static string ConvertNetToPHP(string format)
    {
      return DateTimeUtils.ConvertNetToPHP(format, CultureInfo.CurrentUICulture);
    }

    /// <summary>
    /// Accepts a Unix/PHP date string format and returns a valid .NET date format
    /// </summary>
    /// <param name="format">The .NET format string</param>
    /// <param name="culture"></param>
    /// <returns>The format string converted to PHP format specifiers</returns>
    public static string ConvertNetToPHP(string format, CultureInfo culture)
    {
      if (string.IsNullOrEmpty(format))
        return "";
      StringBuilder stringBuilder = new StringBuilder(128);
      switch (format.Trim())
      {
        case "d":
          format = culture.DateTimeFormat.ShortDatePattern;
          break;
        case "D":
          format = culture.DateTimeFormat.LongDatePattern;
          break;
        case "f":
          format = culture.DateTimeFormat.LongDatePattern + " " + culture.DateTimeFormat.ShortTimePattern;
          break;
        case "F":
          format = culture.DateTimeFormat.FullDateTimePattern;
          break;
        case "g":
          format = culture.DateTimeFormat.ShortDatePattern + " " + culture.DateTimeFormat.ShortTimePattern;
          break;
        case "G":
          format = culture.DateTimeFormat.ShortDatePattern + " " + culture.DateTimeFormat.LongTimePattern;
          break;
        case "t":
          format = culture.DateTimeFormat.ShortTimePattern;
          break;
        case "T":
          format = culture.DateTimeFormat.LongTimePattern;
          break;
      }
      for (Match match = Regex.Match(format, "(\\\\)?(dd?d?d?|M\\$|MS|MM?M?M?|yy?y?y?|hh?|HH?|mm?|ss?|tt?|S)|.", RegexOptions.IgnoreCase); match.Success; match = match.NextMatch())
      {
        string str = match.Value;
        switch (str)
        {
          case "dd":
            stringBuilder.Append("d");
            break;
          case "ddd":
            stringBuilder.Append("D");
            break;
          case "d":
            stringBuilder.Append("j");
            break;
          case "dddd":
            stringBuilder.Append("l");
            break;
          case "M$":
          case "MS":
            stringBuilder.Append("MS");
            break;
          case "MMMM":
            stringBuilder.Append("F");
            break;
          case "MM":
            stringBuilder.Append("m");
            break;
          case "MMM":
            stringBuilder.Append("M");
            break;
          case "M":
            stringBuilder.Append("n");
            break;
          case "yyyy":
            stringBuilder.Append("Y");
            break;
          case "yy":
            stringBuilder.Append("y");
            break;
          case "tt":
            stringBuilder.Append("a");
            break;
          case "h":
            stringBuilder.Append("g");
            break;
          case "H":
            stringBuilder.Append("G");
            break;
          case "hh":
            stringBuilder.Append("h");
            break;
          case "HH":
            stringBuilder.Append("H");
            break;
          case "mm":
            stringBuilder.Append("i");
            break;
          case "ss":
            stringBuilder.Append("s");
            break;
          default:
            stringBuilder.Append(str);
            break;
        }
      }
      return stringBuilder.ToString();
    }

    /// <summary>Convert .NET DateTime to JavaScript object</summary>
    /// <param name="date">.NET DateTime</param>
    /// <returns>JavaScript Date as string</returns>
    public static string DateNetToJs(DateTime date)
    {
      if (date.Equals(DateTime.MinValue))
        return "null";
      return "new Date(" + string.Format(date.TimeOfDay == new TimeSpan(0, 0, 0) ? "{0},{1},{2}" : "{0},{1},{2},{3},{4},{5},{6}", (object) date.Year, (object) (date.Month - 1), (object) date.Day, (object) date.Hour, (object) date.Minute, (object) date.Second, (object) date.Millisecond) + ")";
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static DateTimeFormatInfo ClientDateTimeFormatInfo()
    {
      try
      {
        return HttpContext.Current == null || HttpContext.Current.Request.UserLanguages == null || HttpContext.Current.Request.UserLanguages.Length == 0 ? CultureInfo.InvariantCulture.DateTimeFormat : CultureInfo.CreateSpecificCulture(HttpContext.Current.Request.UserLanguages[0]).DateTimeFormat;
      }
      catch
      {
        return CultureInfo.InvariantCulture.DateTimeFormat;
      }
    }
  }
}
