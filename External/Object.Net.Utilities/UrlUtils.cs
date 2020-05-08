// Decompiled with JetBrains decompiler
// Type: Object.Net.Utilities.UrlUtils
// Assembly: Object.Net.Utilities, Version=2.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0B5927F8-8BAA-4C4A-84B2-E1600C4FD3FC
// Assembly location: C:\h5\2020-04-30\h5\packages\Object.NET.Utilities.2.5.0\lib\Object.Net.Utilities.dll

namespace Object.Net.Utilities
{
  public class UrlUtils
  {
    public static bool IsUrl(string url)
    {
      return !string.IsNullOrEmpty(url) && url.IndexOf("://") >= 0;
    }
  }
}
