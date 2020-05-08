// Decompiled with JetBrains decompiler
// Type: Object.Net.Utilities.Inflatr.Base
// Assembly: Object.Net.Utilities, Version=2.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0B5927F8-8BAA-4C4A-84B2-E1600C4FD3FC
// Assembly location: C:\highfive\2020-04-30\highfive\packages\Object.NET.Utilities.2.5.0\lib\Object.Net.Utilities.dll

using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Object.Net.Utilities.Inflatr
{
  /// <summary>
  /// 
  /// </summary>
  public abstract class Base
  {
    /// <summary>
    /// 
    /// </summary>
    protected string input = "";
    private Regex escapeRe = new Regex("([.*+?^${}()|[\\]\\/\\\\])", RegexOptions.Compiled);
    private Regex newLineRe1 = new Regex("\\Z", RegexOptions.Compiled);
    private Regex newLineRe2 = new Regex("\\S", RegexOptions.Compiled);
    private Options options;
    /// <summary>
    /// 
    /// </summary>
    protected int index;
    /// <summary>
    /// 
    /// </summary>
    protected int c;
    /// <summary>
    /// 
    /// </summary>
    protected StringBuilder r;

    /// <summary>
    /// 
    /// </summary>
    public Base()
    {
      this.options = new Options();
      this.r = this.Indent();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    public Base(Options options)
      : this()
    {
      if (options == null)
        return;
      this.options = options.Clone();
      this.r = this.Indent();
    }

    /// <summary>
    /// 
    /// </summary>
    public Options Options
    {
      get
      {
        return this.options;
      }
    }

    /// <summary>Inflate the string</summary>
    /// <param name="input">compressed string</param>
    /// <returns>Inflated string</returns>
    public abstract string Inflate(string input);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pattern"></param>
    /// <returns></returns>
    protected string Escape(string pattern)
    {
      return this.escapeRe.Replace(pattern, "\\$1");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected StringBuilder Indent()
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < this.options.Level; ++index)
        stringBuilder.Append(this.options.Indent);
      return stringBuilder;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pattern"></param>
    /// <returns></returns>
    protected int LastIndexOf(Regex pattern)
    {
      return this.LastIndexOf(pattern, (string) null);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pattern"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    protected int LastIndexOf(Regex pattern, string input)
    {
      if (input.IsEmpty())
        input = this.input;
      for (int index = this.index; index >= 0; --index)
      {
        if (pattern.IsMatch(input, index))
          return index;
      }
      return -1;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pattern"></param>
    /// <param name="start"></param>
    /// <returns></returns>
    protected bool After(Regex pattern, Regex start)
    {
      int startat = this.LastIndexOf(start);
      return startat > 0 && pattern.IsMatch(this.input, startat);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strings"></param>
    protected void Append(params string[] strings)
    {
      foreach (string str in strings)
      {
        if (str != null)
        {
          this.r.Append(str);
          this.c += str.Length;
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pat"></param>
    /// <returns></returns>
    protected string Peek(string pat)
    {
      Match match = new Regex("^" + pat, RegexOptions.Multiline).Match(this.input, this.index);
      return !match.Success ? (string) null : match.Groups[0].Value;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pat"></param>
    /// <returns></returns>
    protected string Scan(string pat)
    {
      string text = this.Peek(pat);
      if (!text.IsNotEmpty())
        return (string) null;
      this.index += text.Length;
      return text;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected bool NextChar()
    {
      this.Append(this.Scan("(\n|.)"));
      return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected bool WhiteSpace()
    {
      if (!this.Scan("\\s+").IsNotEmpty())
        return false;
      this.Append(" ");
      return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected string NewLine()
    {
      int num1 = this.LastIndexOf(this.newLineRe1, this.r.ToString());
      int num2 = this.LastIndexOf(this.newLineRe2, this.r.ToString());
      if (num1 >= 0 && num2 >= 0 && num1 >= num2)
        return "";
      this.c = 0;
      return "\n" + this.Indent().ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pat"></param>
    /// <returns></returns>
    protected string ScanUntil(string pat)
    {
      Match match = new Regex("^((?:(?!" + pat + ").)*)" + pat, RegexOptions.Multiline).Match(this.input, this.index);
      if (!match.Success)
        return (string) null;
      this.index += match.Groups[0].Length;
      return match.Groups[1].Value;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="start"></param>
    /// <returns></returns>
    protected string Between(string start)
    {
      return this.Between(start, start);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="start"></param>
    /// <param name="finish"></param>
    /// <returns></returns>
    protected string Between(string start, string finish)
    {
      if (!this.Scan(start).IsNotEmpty())
        return (string) null;
      string text = this.ScanUntil(finish);
      if (text.IsNotEmpty())
        return text;
      throw new Exception("Between: unmatched " + finish + " after " + start + ".");
    }
  }
}
