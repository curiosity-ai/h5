// Decompiled with JetBrains decompiler
// Type: Object.Net.Utilities.Verify
// Assembly: Object.Net.Utilities, Version=2.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0B5927F8-8BAA-4C4A-84B2-E1600C4FD3FC
// Assembly location: C:\h5\2020-04-30\h5\packages\Object.NET.Utilities.2.5.0\lib\Object.Net.Utilities.dll

using System;

namespace Object.Net.Utilities
{
  /// <summary>
  /// 
  /// </summary>
  public abstract class Verify
  {
    /// <summary>
    /// Checks if parameter is not null. Throws ArgumentNullException with name of parameter if null
    /// </summary>
    /// <param name="parameter">The parameter value to check.</param>
    /// <param name="parameterName">The name of the parameter.</param>
    public static void IsNotNull(object parameter, string parameterName)
    {
      if (parameter == null)
        throw new ArgumentNullException(parameterName, parameterName);
    }

    /// <summary>
    /// Checks if the value is a type of String object. Throws ArgumentException if value is not a String type object.
    /// </summary>
    /// <param name="value">The object to check.</param>
    /// <param name="paramterName">The name of the parameter.</param>
    public static void IsString(object value, string paramterName)
    {
      if (!(value is string))
        throw new ArgumentException(paramterName, paramterName);
    }
  }
}
