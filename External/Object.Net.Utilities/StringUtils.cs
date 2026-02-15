// Decompiled with JetBrains decompiler
// Type: Object.Net.Utilities.StringUtils
// Assembly: Object.Net.Utilities, Version=2.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0B5927F8-8BAA-4C4A-84B2-E1600C4FD3FC
// Assembly location: C:\h5\2020-04-30\h5\packages\Object.NET.Utilities.2.5.0\lib\Object.Net.Utilities.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
//using System.Web.UI;

namespace Object.Net.Utilities
{
  /// <summary>
  /// 
  /// </summary>
  public static class StringUtils
  {
    private static readonly Random random = new Random();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="test"></param>
    /// <param name="valueIfTrue"></param>
    /// <returns></returns>
    public static string If(this string text, string test, string valueIfTrue)
    {
      return text.If<string>((Func<bool>) (() => text == test), valueIfTrue, text);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="test"></param>
    /// <param name="valueIfTrue"></param>
    /// <returns></returns>
    public static string IfNot(this string text, string test, string valueIfTrue)
    {
      return text.IfNot<string>((Func<bool>) (() => text == test), valueIfTrue, text);
    }

    /// <summary>
    /// Replaces all occurrences of System.String in the oldValues String Array, with
    /// another specified System.String of newValue.
    /// </summary>
    /// <param name="instance">this string</param>
    /// <param name="oldValues">An Array of String objects to be replaced.</param>
    /// <param name="newValue">The new value to replace old values.</param>
    /// <returns>The modified original string.</returns>
    [Description("Replaces all occurrences of System.String in the oldValues String Array, with another specified System.String of newValue.")]
    public static string Replace(this string instance, string[] oldValues, string newValue)
    {
      if (oldValues == null || oldValues.Length < 1)
        return instance;
      ((IEnumerable<string>) oldValues).Each<string>((Action<string>) (value => instance = instance.Replace(value, newValue)));
      return instance;
    }

    /// <summary>
    /// Replaces all occurrences of System.String in the oldValue String Array, with the
    /// return String value of the specified Function convert.
    /// </summary>
    /// <param name="instance">this string</param>
    /// <param name="oldValues">An Array of String objects to be replaced.</param>
    /// <param name="convert">The Function to convert the found old value.</param>
    /// <returns>The modified original string.</returns>
    [Description("Replaces all occurrences of System.String in the oldValue String Array, with the return String value of the specified Function convert.")]
    public static string Replace(
      this string instance,
      string[] oldValues,
      Func<string, string> convert)
    {
      if (oldValues == null || oldValues.Length < 1)
        return instance;
      ((IEnumerable<string>) oldValues).Each<string>((Action<string>) (value => instance = instance.Replace(value, convert(value))));
      return instance;
    }

    /// <summary>Format the string with the args.</summary>
    public static string FormatWith(this string format, params object[] args)
    {
      if (args == null)
        throw new ArgumentNullException($"The args parameter can not be null when calling {format}.FormatWith().");
      return string.Format(format, args);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="format"></param>
    /// <param name="provider"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static string FormatWith(
      this string format,
      IFormatProvider provider,
      params object[] args)
    {
      Verify.IsNotNull((object) format, nameof (format));
      return string.Format(provider, format, args);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="format"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string FormatWith(this string format, object source)
    {
      return format.FormatWith((IFormatProvider) null, source);
    }

    ///// http://james.newtonking.com/archive/2008/03/29/formatwith-2-0-string-formatting-with-named-variables.aspx
    /////             <summary>
    ///// 
    ///// </summary>
    ///// <param name="format"></param>
    ///// <param name="provider"></param>
    ///// <param name="source"></param>
    ///// <returns></returns>
    //[Description("")]
    //public static string FormatWith(this string format, IFormatProvider provider, object source)
    //{
    //  if (format == null)
    //    throw new ArgumentNullException(nameof (format));
    //  List<object> values = new List<object>();
    //  string format1 = Regex.Replace(format, "(?<start>\\{)+(?<property>[\\w\\.\\[\\]]+)(?<format>:[^}]+)?(?<end>\\})+", (MatchEvaluator) (m =>
    //  {
    //    Group group1 = m.Groups["start"];
    //    Group group2 = m.Groups["property"];
    //    Group group3 = m.Groups[nameof (format)];
    //    Group group4 = m.Groups["end"];
    //    values.Add(group2.Value == "0" ? source : StringUtils.Eval(source, group2.Value));
    //    int count1 = group1.Captures.Count;
    //    int count2 = group4.Captures.Count;
    //    if (count1 > count2 || count1 % 2 == 0)
    //      return m.Value;
    //    return new string('{', count1) + (object) (values.Count - 1) + group3.Value + new string('}', count2);
    //  }), RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);
    //  return string.Format(provider, format1, values.ToArray());
    //}

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="source"></param>
    ///// <param name="expression"></param>
    ///// <returns></returns>
    //private static object Eval(object source, string expression)
    //{
    //  try
    //  {
    //    return DataBinder.Eval(source, expression);
    //  }
    //  catch (HttpException ex)
    //  {
    //    throw new FormatException((string) null, (Exception) ex);
    //  }
    //}

    /// <summary>Add the text string to the source string.</summary>
    public static string ConcatWith(this string instance, string text)
    {
      return instance + text;
    }

    /// <summary>Add the args strings the source text string.</summary>
    public static string ConcatWith(this string instance, params object[] args)
    {
      if (args == null)
        throw new ArgumentNullException($"The args parameter can not be null when calling {instance}.Format().");
      return instance + string.Concat(args);
    }

    /// <summary>
    /// Determines if the string contains any of the args. If yes, returns true, otherwise returns false.
    /// </summary>
    /// <param name="instance">The instance of the string</param>
    /// <param name="args">The string to check if contained within the string instance.</param>
    /// <returns>boolean</returns>
    public static bool Contains(this string instance, params string[] args)
    {
      foreach (string str in args)
      {
        if (instance.Contains(str))
          return true;
      }
      return false;
    }

    /// <summary>Determine is the string is null or empty.</summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static bool IsEmpty(this string text)
    {
      return string.IsNullOrEmpty(text);
    }

    /// <summary>Determine is the string is NOT null or empty.</summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static bool IsNotEmpty(this string text)
    {
      return !text.IsEmpty();
    }

    /// <summary>
    /// Return a string from between the start and end positions.
    /// </summary>
    [Description("Return a sub array of this string array.")]
    public static string Between(this string text, string start, string end)
    {
      return text.IsEmpty() ? text : text.RightOf(start).LeftOfRightmostOf(end);
    }

    /// <summary>Return a sub array of this string array.</summary>
    [Description("Return a sub array of this string array.")]
    public static string[] Subarray(this string[] items, int start)
    {
      return items.Subarray(start, items.Length - start);
    }

    /// <summary>Return a sub array of this string array.</summary>
    [Description("Return a sub array of this string array.")]
    public static string[] Subarray(this string[] items, int start, int length)
    {
      if (start > items.Length)
        throw new ArgumentException($"The start index [{start}] is greater than the length [{items.Length}] of the array.");
      if (start + length > items.Length)
        throw new ArgumentException($"The length [{length}] to return is greater than the length [{items.Length}] of the array.");
      string[] strArray = new string[length];
      int index1 = 0;
      for (int index2 = start; index2 < start + length; ++index2)
      {
        strArray[index1] = items[index2];
        ++index1;
      }
      return strArray;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    public static string Join(this IEnumerable items)
    {
      return items.Join(",", "{0}");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="items"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static string Join(this IEnumerable items, string separator)
    {
      return items.Join(separator, "{0}");
    }

    /// <summary>Join the items together</summary>
    /// <param name="items">The items to join.</param>
    /// <param name="separator">The separator.</param>
    /// <param name="template">The template to format the items with.</param>
    /// <returns></returns>
    public static string Join(this IEnumerable items, string separator, string template)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (object obj in items)
      {
        if (obj != null)
        {
          stringBuilder.Append(separator);
          stringBuilder.Append(string.Format(template, (object) obj.ToString()));
        }
      }
      return stringBuilder.ToString().RightOf(separator);
    }

    /// <summary>Chops one character from each end of string.</summary>
    public static string Chop(this string text)
    {
      return text.Chop(1);
    }

    /// <summary>
    /// Chops the specified number of characters from each end of string.
    /// </summary>
    public static string Chop(this string text, int characters)
    {
      return text.IsEmpty() ? text : text.Substring(characters, text.Length - characters - 1);
    }

    /// <summary>
    /// Chops the specified string from each end of the string. If the character does not exist on both ends
    /// of the string, the characters are not chopped.
    /// </summary>
    public static string Chop(this string text, string character)
    {
      if (text.IsEmpty() || !text.StartsWith(character) || !text.EndsWith(character))
        return text;
      int length = character.Length;
      return text.Substring(length, text.Length - (length + 1));
    }

    /// <summary>MD5Hash's a string.</summary>
    public static string ToMD5Hash(this string text)
    {
      if (text.IsEmpty())
        return text;
      byte[] hash = MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(text.Trim()));
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < hash.Length; ++index)
        stringBuilder.Append(hash[index].ToString("x2"));
      return stringBuilder.ToString();
    }

    /// <summary>
    /// Converts the first character of each word to Uppercase. Example: "the lazy dog" returns "The Lazy Dog"
    /// </summary>
    /// <param name="text">The text to convert to sentence case</param>
    public static string ToTitleCase(this string text)
    {
      if (text.IsEmpty())
        return text;
      return text.Split(' ').ToTitleCase();
    }

    /// <summary>
    /// Converts the first character of each word to Uppercase. Example: "the lazy dog" returns "The Lazy Dog"
    /// </summary>
    /// <param name="ci"></param>
    /// <param name="text">The text to convert to sentence case</param>
    public static string ToTitleCase(this string text, CultureInfo ci)
    {
      if (text.IsEmpty())
        return text;
      return text.Split(' ').ToTitleCase(ci);
    }

    /// <summary>
    /// Converts the first character of each word to Uppercase. Example: "the lazy dog" returns "The Lazy Dog"
    /// </summary>
    public static string ToTitleCase(this string[] words)
    {
      return words.ToTitleCase((CultureInfo) null);
    }

    /// <summary>
    /// Converts the first character of each word to Uppercase. Example: "the lazy dog" returns "The Lazy Dog"
    /// </summary>
    public static string ToTitleCase(this string[] words, CultureInfo ci)
    {
      if (words == null || words.Length == 0)
        return "";
      for (int index = 0; index < words.Length; ++index)
        words[index] = ((char) (ci != null ? (int) char.ToUpper(words[index][0], ci) : (int) char.ToUpper(words[index][0]))).ToString() + words[index].Substring(1);
      return string.Join(" ", words);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static bool IsLowerCamelCase(this string text)
    {
      return !text.IsEmpty() && text.Substring(0, 1).ToLowerInvariant().Equals(text.Substring(0, 1));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string ToLowerCamelCase(this string text)
    {
      return text.IsEmpty() ? text : text.Substring(0, 1).ToLower(CultureInfo.InvariantCulture) + text.Substring(1);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static string ToLowerCamelCase(this string[] values)
    {
      return values == null || values.Length == 0 ? "" : values.ToLowerCamelCase();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string ToCamelCase(this string text)
    {
      return text.IsEmpty() ? text : text.Substring(0, 1).ToUpper(CultureInfo.CurrentCulture) + text.Substring(1);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="values"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static string ToCamelCase(this string[] values, string separator)
    {
      string str = "";
      foreach (string text in values)
        str = $"{str}{separator}{text.ToCamelCase()}";
      return str;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static string ToCamelCase(this string[] values)
    {
      return values.ToCamelCase("");
    }

    /// <summary>
    /// Pad the left side of a string with characters to make the total length.
    /// </summary>
    public static string PadLeft(this string text, char c, int totalLength)
    {
      return text.IsEmpty() || totalLength < text.Length ? text : new string(c, totalLength - text.Length) + text;
    }

    /// <summary>
    /// Pad the right side of a string with a '0' if a single character.
    /// </summary>
    public static string PadRight(this string text)
    {
      return text.PadRight('0', 2);
    }

    /// <summary>
    /// Pad the right side of a string with characters to make the total length.
    /// </summary>
    public static string PadRight(this string text, char c, int totalLength)
    {
      return text.IsEmpty() || totalLength < text.Length ? text : text + new string(c, totalLength - text.Length);
    }

    /// <summary>Left of the first occurance of c</summary>
    public static string LeftOf(this string text, char c)
    {
      if (text.IsEmpty())
        return text;
      int length = text.IndexOf(c);
      return length == -1 ? text : text.Substring(0, length);
    }

    /// <summary>Left of the first occurance of text</summary>
    public static string LeftOf(this string text, string value)
    {
      if (text.IsEmpty())
        return text;
      int length = text.IndexOf(value);
      return length == -1 ? text : text.Substring(0, length);
    }

    /// <summary>Left of the n'th occurance of c</summary>
    public static string LeftOf(this string text, char c, int n)
    {
      if (text.IsEmpty())
        return text;
      int length = -1;
      for (; n != 0; --n)
      {
        length = text.IndexOf(c, length + 1);
        if (length == -1)
          return text;
      }
      return text.Substring(0, length);
    }

    /// <summary>Right of the first occurance of c</summary>
    public static string RightOf(this string text, char c)
    {
      if (text.IsEmpty())
        return text;
      int num = text.IndexOf(c);
      return num == -1 ? "" : text.Substring(num + 1);
    }

    /// <summary>Right of the first occurance of text</summary>
    public static string RightOf(this string text, string value)
    {
      if (text.IsEmpty())
        return text;
      int num = text.IndexOf(value);
      return num == -1 ? "" : text.Substring(num + value.Length);
    }

    /// <summary>Right of the n'th occurance of c</summary>
    public static string RightOf(this string text, char c, int n)
    {
      if (text.IsEmpty())
        return text;
      int num = -1;
      for (; n != 0; --n)
      {
        num = text.IndexOf(c, num + 1);
        if (num == -1)
          return "";
      }
      return text.Substring(num + 1);
    }

    /// <summary>Right of the n'th occurance of c</summary>
    public static string RightOf(this string text, string c, int n)
    {
      if (text.IsEmpty())
        return text;
      int num = -1;
      for (; n != 0; --n)
      {
        num = text.IndexOf(c, num + 1);
        if (num == -1)
          return "";
      }
      return text.Substring(num + 1);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="c"></param>
    /// <returns></returns>
    public static string LeftOfRightmostOf(this string text, char c)
    {
      if (text.IsEmpty())
        return text;
      int length = text.LastIndexOf(c);
      return length == -1 ? text : text.Substring(0, length);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string LeftOfRightmostOf(this string text, string value)
    {
      if (text.IsEmpty())
        return text;
      int length = text.LastIndexOf(value);
      return length == -1 ? text : text.Substring(0, length);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="c"></param>
    /// <returns></returns>
    public static string RightOfRightmostOf(this string text, char c)
    {
      if (text.IsEmpty())
        return text;
      int num = text.LastIndexOf(c);
      return num == -1 ? text : text.Substring(num + 1);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string RightOfRightmostOf(this string text, string value)
    {
      if (text.IsEmpty())
        return text;
      int num = text.LastIndexOf(value);
      return num == -1 ? text : text.Substring(num + value.Length);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    /// <returns></returns>
    public static string ReplaceLastInstanceOf(this string text, string oldValue, string newValue)
    {
      return text.IsEmpty() ? text : $"{text.LeftOfRightmostOf(oldValue)}{newValue}{text.RightOfRightmostOf(oldValue)}";
    }

    /// <summary>
    /// Accepts a string like "ArrowRotateClockwise" and returns "arrow_rotate_clockwise.png".
    /// </summary>
    public static string ToCharacterSeparatedFileName(
      this string name,
      char separator,
      string extension)
    {
      if (name.IsEmpty())
        return name;
      MatchCollection matchCollection = Regex.Matches(name, "([A-Z]+)[a-z]*|\\d{1,}[a-z]{0,}");
      string str = "";
      for (int index = 0; index < matchCollection.Count; ++index)
      {
        if (index != 0)
          str += (string) (object) separator;
        str += matchCollection[index].ToString().ToLowerInvariant();
      }
      return string.IsNullOrEmpty(extension) ? $"{str}{extension}" : $"{str}.{extension}";
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string Enquote(this string text)
    {
      if (text.IsEmpty())
        return text;
      if (string.IsNullOrEmpty(text))
        return string.Empty;
      int length = text.Length;
      StringBuilder stringBuilder = new StringBuilder(length + 4);
      for (int index = 0; index < length; ++index)
      {
        char c = text[index];
        switch (c)
        {
          case '\b':
            stringBuilder.Append("\\b");
            break;
          case '\t':
            stringBuilder.Append("\\t");
            break;
          case '\n':
            stringBuilder.Append("\\n");
            break;
          case '\f':
            stringBuilder.Append("\\f");
            break;
          case '\r':
            stringBuilder.Append("\\r");
            break;
          case '"':
          case '>':
          case '\\':
            stringBuilder.Append('\\');
            stringBuilder.Append(c);
            break;
          default:
            if (c < ' ')
            {
              string str = "000" + (object) int.Parse(new string(c, 1), NumberStyles.HexNumber);
              stringBuilder.Append($"\\u{str.Substring(str.Length - 4)}");
              break;
            }
            stringBuilder.Append(c);
            break;
        }
      }
      return stringBuilder.ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string EnsureSemiColon(this string text)
    {
      return text.IsEmpty() || string.IsNullOrEmpty(text) || text.EndsWith(";") ? text : text + ";";
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="wrapByText"></param>
    /// <returns></returns>
    public static string Wrap(this string text, string wrapByText)
    {
      if (text == null)
        text = "";
      return wrapByText.ConcatWith((object) text, (object) wrapByText);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="wrapStart"></param>
    /// <param name="wrapEnd"></param>
    /// <returns></returns>
    public static string Wrap(this string text, string wrapStart, string wrapEnd)
    {
      if (text == null)
        text = "";
      return wrapStart.ConcatWith((object) text, (object) wrapEnd);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="pattern"></param>
    /// <returns></returns>
    public static bool Test(this string text, string pattern)
    {
      return Regex.IsMatch(text, pattern);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="pattern"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static bool Test(this string text, string pattern, RegexOptions options)
    {
      return Regex.IsMatch(text, pattern, options);
    }

    /// <summary>
    /// Truncate a string and add an ellipsis ('...') to the end if it exceeds the specified length
    /// </summary>
    /// <param name="text">The string to truncate</param>
    /// <param name="length">The maximum length to allow before truncating</param>
    /// <returns>The converted text</returns>
    public static string Ellipsis(this string text, int length)
    {
      return text.Ellipsis(length, false);
    }

    /// <summary>
    /// Truncate a string and add an ellipsis ('...') to the end if it exceeds the specified length
    /// </summary>
    /// <param name="text">The string to truncate</param>
    /// <param name="length">The maximum length to allow before truncating</param>
    /// <param name="word">True to try to find a common work break</param>
    /// <returns>The converted text</returns>
    public static string Ellipsis(this string text, int length, bool word)
    {
      if (text == null || text.Length <= length)
        return text;
      if (!word)
        return text.Substring(0, length - 3) + "...";
      string str = text.Substring(0, length - 2);
      int length1 = Math.Max(str.LastIndexOf(' '), Math.Max(str.LastIndexOf('.'), Math.Max(str.LastIndexOf('!'), str.LastIndexOf('?'))));
      return length1 == -1 || length1 < length - 15 ? text.Substring(0, length - 3) + "..." : str.Substring(0, length1) + "...";
    }

    /// <summary>Base64 string decoder</summary>
    /// <param name="text">The text string to decode</param>
    /// <returns>The decoded string</returns>
    public static string Base64Decode(this string text)
    {
      Decoder decoder = new UTF8Encoding().GetDecoder();
      byte[] bytes = Convert.FromBase64String(text);
      char[] chars = new char[decoder.GetCharCount(bytes, 0, bytes.Length)];
      decoder.GetChars(bytes, 0, bytes.Length, chars, 0);
      return new string(chars);
    }

    /// <summary>Base64 string encoder</summary>
    /// <param name="text">The text string to encode</param>
    /// <returns>The encoded string</returns>
    public static string Base64Encode(this string text)
    {
      byte[] numArray = new byte[text.Length];
      return Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="regex"></param>
    /// <returns></returns>
    public static string FormatRegexPattern(this string regex)
    {
      bool flag = !regex.StartsWith("new RegExp");
      if (!regex.StartsWith("/", StringComparison.InvariantCulture) && flag)
        regex = $"/{regex}";
      if (!regex.EndsWith("/", StringComparison.InvariantCulture) && flag)
        regex = $"{regex}/";
      return regex;
    }

    /// <summary>
    /// Generate a random string of character at a certain length
    /// </summary>
    /// <param name="chars">The Characters to use in the random string</param>
    /// <param name="length">The length of the random string</param>
    /// <returns>A string of random characters</returns>
    public static string Randomize(this string chars, int length)
    {
      char[] chArray = new char[length];
      for (int index = 0; index < length; ++index)
        chArray[index] = chars[StringUtils.random.Next(chars.Length)];
      return new string(chArray);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="chars"></param>
    /// <returns></returns>
    public static string Randomize(this string chars)
    {
      return chars.Randomize(chars.Length);
    }
  }
}
