// Decompiled with JetBrains decompiler
// Type: Object.Net.Utilities.Inflatr.Javascript
// Assembly: Object.Net.Utilities, Version=2.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0B5927F8-8BAA-4C4A-84B2-E1600C4FD3FC
// Assembly location: C:\h5\2020-04-30\h5\packages\Object.NET.Utilities.2.5.0\lib\Object.Net.Utilities.dll

using System.Text.RegularExpressions;

namespace Object.Net.Utilities.Inflatr
{
  /// <summary>
  /// 
  /// </summary>
  public class Javascript : Base
  {
    private System.Text.RegularExpressions.Regex afterPatternRegex = new System.Text.RegularExpressions.Regex("^(\\d|\\w|\\$|_\\))", RegexOptions.Compiled);
    private System.Text.RegularExpressions.Regex afterStartRegex = new System.Text.RegularExpressions.Regex("(\\:|\\S)", RegexOptions.Compiled);
    /// <summary>
    /// 
    /// </summary>
    public const string KEYWORDS = "(function|continue|default|finally|export|return|switch|import|delete|throw|const|while|catch|break|void|case|with|this|else|for|try|var|new|do|in|if)\\W";
    /// <summary>
    /// 
    /// </summary>
    public const string OPERATORS = "(instanceof|typeof|>>>=|===|<<=|>>=|>>>|!==|!=|>=|\\*=|<=|\\+\\+|\\-=|==|&&|>>|<<|/=|\\|\\||\\+=|\\^=|\\|=|&=|\\-\\-|\\||&|\\^|>|<|!|=|\\?|:|%|/|\\*|\\-|\\+)";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public override string Inflate(string input)
    {
      this.input = input;
      while (this.index < this.input.Length)
      {
        if (!this.Comment() && !this.String() && (!this.Regex() && !this.Operator()) && (!this.Keyword() && !this.OpenBlock() && (!this.CloseBlock() && !this.Comma())) && (!this.Parens() && !this.Eos() && !this.Eol()))
          this.NextChar();
      }
      return new System.Text.RegularExpressions.Regex("\\s*$").Replace(this.r.ToString(), "");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected virtual bool Comment()
    {
      string text1 = this.Between("//", "\n");
      if (text1.IsNotEmpty())
      {
        this.Append("//", text1, this.NewLine());
        return false;
      }
      string text2 = this.Between(this.Escape("/*"), this.Escape("*/"));
      if (!text2.IsNotEmpty())
        return false;
      this.Append("/*", text2, "*/");
      return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected virtual bool String()
    {
      string text1 = this.Between("'");
      if (text1.IsNotEmpty())
      {
        this.Append("'" + text1 + "'");
        return true;
      }
      string text2 = this.Between("\"");
      if (!text2.IsNotEmpty())
        return false;
      this.Append("\"" + text2 + "\"");
      return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected virtual bool Regex()
    {
      if (!this.After(this.afterPatternRegex, this.afterStartRegex))
      {
        string text = this.Between("/");
        if (text.IsNotEmpty())
        {
          this.Append("/" + text + "/");
          return true;
        }
      }
      return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected virtual bool Operator()
    {
      string text = this.Scan("(instanceof|typeof|>>>=|===|<<=|>>=|>>>|!==|!=|>=|\\*=|<=|\\+\\+|\\-=|==|&&|>>|<<|/=|\\|\\||\\+=|\\^=|\\|=|&=|\\-\\-|\\||&|\\^|>|<|!|=|\\?|:|%|/|\\*|\\-|\\+)");
      if (!text.IsNotEmpty())
        return false;
      this.Append(" " + text + " ");
      return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected virtual bool Keyword()
    {
      return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected virtual bool OpenBlock()
    {
      if (!this.Scan("\\{").IsNotEmpty())
        return false;
      ++this.Options.Level;
      this.Append(" {", this.NewLine());
      return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected virtual bool CloseBlock()
    {
      if (!this.Scan("\\}").IsNotEmpty())
        return false;
      --this.Options.Level;
      this.Append(this.NewLine(), "}");
      if (this.Peek("[;,\\}]").IsNotEmpty())
        this.Append(this.NewLine());
      return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected virtual bool Comma()
    {
      if (!this.Scan(",").IsNotEmpty())
        return false;
      if (this.Peek("(?!\\{\\})*;").IsNotEmpty() || this.c < this.Options.Wrap)
        this.Append(", ");
      else
        this.Append(",", this.NewLine());
      return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected virtual bool Parens()
    {
      if (this.Scan("\\(\\s*\\)").IsNotEmpty())
      {
        this.Append("()");
        return true;
      }
      if (this.Scan("\\(").IsNotEmpty())
      {
        this.Append(" ( ");
        return true;
      }
      if (!this.Scan("\\)").IsNotEmpty())
        return false;
      this.Append(" ) ");
      return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected virtual bool Eos()
    {
      if (!this.Scan(";").IsNotEmpty())
        return false;
      this.Append(";");
      if (this.Peek("\\s*\\}").IsNotEmpty())
        this.Append(this.NewLine());
      return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected virtual bool Eol()
    {
      return this.Scan("\n").IsNotEmpty();
    }
  }
}
