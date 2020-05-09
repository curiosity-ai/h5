// Decompiled with JetBrains decompiler
// Type: Object.Net.Utilities.JsonUtils
// Assembly: Object.Net.Utilities, Version=2.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0B5927F8-8BAA-4C4A-84B2-E1600C4FD3FC
// Assembly location: C:\h5\2020-04-30\h5\packages\Object.NET.Utilities.2.5.0\lib\Object.Net.Utilities.dll

using System.Globalization;
using System.Text;

namespace Object.Net.Utilities
{
  /// <summary>
  /// 
  /// </summary>
  public class JsonUtils
  {
    /// <summary>
    /// Produce a string in double quotes with backslash sequences in all the right places.
    /// </summary>
    /// <param name="s">A String</param>
    /// <returns>A String correctly formatted for insertion in a JSON message.</returns>
    public static string Enquote(string s)
    {
      switch (s)
      {
        case "":
        case null:
          return "\"\"";
        default:
          int length = s.Length;
          StringBuilder stringBuilder = new StringBuilder(length + 4);
          stringBuilder.Append('"');
          for (int index = 0; index < length; ++index)
          {
            char c = s[index];
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
                  stringBuilder.Append("\\u" + str.Substring(str.Length - 4));
                  break;
                }
                stringBuilder.Append(c);
                break;
            }
          }
          stringBuilder.Append('"');
          return stringBuilder.ToString();
      }
    }
  }
}
