// Decompiled with JetBrains decompiler
// Type: Object.Net.Utilities.EnumerableUtils
// Assembly: Object.Net.Utilities, Version=2.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0B5927F8-8BAA-4C4A-84B2-E1600C4FD3FC
// Assembly location: C:\bridge\2020-04-30\bridge\packages\Object.NET.Utilities.2.5.0\lib\Object.Net.Utilities.dll

using System;
using System.Collections.Generic;

namespace Object.Net.Utilities
{
  /// <summary>
  /// 
  /// </summary>
  public static class EnumerableUtils
  {
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="instance"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IEnumerable<T> Each<T>(this IEnumerable<T> instance, Action<T> action)
    {
      foreach (T obj in instance)
        action(obj);
      return instance;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="instance"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static int Each<T>(this int instance, Action<int> action)
    {
      for (int index = 0; index < instance; ++index)
        action(index);
      return instance;
    }
  }
}
