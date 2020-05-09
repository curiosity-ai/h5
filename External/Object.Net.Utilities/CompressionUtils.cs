// Decompiled with JetBrains decompiler
// Type: Object.Net.Utilities.CompressionUtils
// Assembly: Object.Net.Utilities, Version=2.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0B5927F8-8BAA-4C4A-84B2-E1600C4FD3FC
// Assembly location: C:\h5\2020-04-30\h5\packages\Object.NET.Utilities.2.5.0\lib\Object.Net.Utilities.dll

using System.IO;
using System.IO.Compression;
using System.Text;
using System.Web;

namespace Object.Net.Utilities
{
  public class CompressionUtils
  {
    public static void GZipAndSend(object instance)
    {
      CompressionUtils.GZipAndSend(instance != null ? instance.ToString() : "");
    }

    public static void GZipAndSend(string instance)
    {
      CompressionUtils.GZipAndSend(instance, "application/json");
    }

    public static void GZipAndSend(string instance, string responseType)
    {
      CompressionUtils.GZipAndSend(Encoding.UTF8.GetBytes(instance), responseType);
    }

    //public static void GZipAndSend(byte[] instance, string responseType)
    //{
    //  if (CompressionUtils.IsGZipSupported)
    //    CompressionUtils.Send(CompressionUtils.GZip(instance), responseType, true);
    //  else
    //    CompressionUtils.Send(instance, responseType);
    //}

    //public static void Send(byte[] instance, string responseType)
    //{
    //  CompressionUtils.Send(instance, responseType, false);
    //}

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="instance"></param>
    ///// <param name="responseType"></param>
    //public static void Send(byte[] instance, string responseType, bool isGZip)
    //{
    //  HttpResponse response = HttpContext.Current.Response;
    //  if (isGZip)
    //  {
    //    response.AppendHeader("Content-Encoding", "gzip");
    //    response.Charset = "utf-8";
    //  }
    //  response.ContentType = responseType;
    //  response.BinaryWrite(instance);
    //}

    public static byte[] GZip(string instance)
    {
      return CompressionUtils.GZip(Encoding.UTF8.GetBytes(instance));
    }

    public static byte[] GZip(byte[] instance)
    {
      MemoryStream memoryStream = new MemoryStream();
      GZipStream gzipStream = new GZipStream((Stream) memoryStream, CompressionMode.Compress);
      gzipStream.Write(instance, 0, instance.Length);
      gzipStream.Close();
      return memoryStream.ToArray();
    }

    //public static bool IsGZipSupported
    //{
    //  get
    //  {
    //    HttpRequest request = HttpContext.Current.Request;
    //    bool flag = request.Browser.IsBrowser("IE") && request.Browser.MajorVersion <= 6;
    //    string lowerInvariant = (request.Headers["Accept-Encoding"] ?? "").ToLowerInvariant();
    //    if (flag)
    //      return false;
    //    return lowerInvariant.Contains("gzip", "deflate");
    //  }
    //}
  }
}
