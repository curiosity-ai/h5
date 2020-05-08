// Decompiled with JetBrains decompiler
// Type: Object.Net.Utilities.Inflatr.Options
// Assembly: Object.Net.Utilities, Version=2.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0B5927F8-8BAA-4C4A-84B2-E1600C4FD3FC
// Assembly location: C:\bridge\2020-04-30\bridge\packages\Object.NET.Utilities.2.5.0\lib\Object.Net.Utilities.dll

namespace Object.Net.Utilities.Inflatr
{
  /// <summary>
  /// 
  /// </summary>
  public class Options
  {
    private int wrap = 80;
    private string indent = "  ";
    private int level;

    /// <summary>
    /// 
    /// </summary>
    public int Wrap
    {
      get
      {
        return this.wrap;
      }
      set
      {
        this.wrap = value;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public string Indent
    {
      get
      {
        return this.indent;
      }
      set
      {
        this.indent = value;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public int Level
    {
      get
      {
        return this.level;
      }
      set
      {
        this.level = value;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Options Clone()
    {
      return new Options()
      {
        Indent = this.Indent,
        Wrap = this.Wrap,
        Level = this.Level
      };
    }
  }
}
