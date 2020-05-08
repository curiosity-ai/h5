// Decompiled with JetBrains decompiler
// Type: Object.Net.Utilities.HtmlUtils
// Assembly: Object.Net.Utilities, Version=2.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0B5927F8-8BAA-4C4A-84B2-E1600C4FD3FC
// Assembly location: C:\bridge\2020-04-30\bridge\packages\Object.NET.Utilities.2.5.0\lib\Object.Net.Utilities.dll

using System.Text.RegularExpressions;

namespace Object.Net.Utilities
{
  /// <summary>
  /// 
  /// </summary>
  public static class HtmlUtils
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="html"></param>
    /// <returns></returns>
    public static string StripWhitespaceChars(this string html)
    {
      return Regex.Replace(html, "[\n\r\t]", "");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="html"></param>
    /// <returns></returns>
    public static string StripExtraSpaces(this string html)
    {
      return Regex.Replace(html, "\\s+", " ");
    }
  }
}
