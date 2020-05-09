// Decompiled with JetBrains decompiler
// Type: Object.Net.Utilities.FileUtils
// Assembly: Object.Net.Utilities, Version=2.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0B5927F8-8BAA-4C4A-84B2-E1600C4FD3FC
// Assembly location: C:\h5\2020-04-30\h5\packages\Object.NET.Utilities.2.5.0\lib\Object.Net.Utilities.dll

using System.IO;

namespace Object.Net.Utilities
{
  /// <summary>
  /// 
  /// </summary>
  public class FileUtils
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string ReadFile(string path)
    {
      using (StreamReader streamReader = new StreamReader(path))
      {
        string end = streamReader.ReadToEnd();
        streamReader.Close();
        return end;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="value"></param>
    public static void WriteFile(string path, string value)
    {
      StreamWriter streamWriter = new StreamWriter(path);
      streamWriter.Write(value);
      streamWriter.Close();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="value"></param>
    public static void WriteToStart(string path, string value)
    {
      string str = FileUtils.ReadFile(path);
      FileUtils.WriteFile(path, value.Trim() + str.Trim());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="value"></param>
    public static void WriteToEnd(string path, string value)
    {
      string str = FileUtils.ReadFile(path);
      FileUtils.WriteFile(path, str.Trim() + value.Trim());
    }
  }
}
