// Decompiled with JetBrains decompiler
// Type: Object.Net.Utilities.ReflectionUtils
// Assembly: Object.Net.Utilities, Version=2.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0B5927F8-8BAA-4C4A-84B2-E1600C4FD3FC
// Assembly location: C:\highfive\2020-04-30\highfive\packages\Object.NET.Utilities.2.5.0\lib\Object.Net.Utilities.dll

using System;
using System.ComponentModel;
using System.Reflection;
using System.Web.UI;

namespace Object.Net.Utilities
{
  /// <summary>
  /// 
  /// </summary>
  public class ReflectionUtils
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    public static object GetDefaultValue(PropertyDescriptor property)
    {
      return !(property.Attributes[typeof (DefaultValueAttribute)] is DefaultValueAttribute attribute) ? (object) null : attribute.Value;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    public static object GetDefaultValue(PropertyInfo property)
    {
      object[] customAttributes = property.GetCustomAttributes(typeof (DefaultValueAttribute), false);
      return customAttributes.Length <= 0 ? (object) null : ((DefaultValueAttribute) customAttributes[0]).Value;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsTypeOf(object obj, Type type)
    {
      return ReflectionUtils.IsTypeOf(obj, type.FullName, false);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="type"></param>
    /// <param name="shallow"></param>
    /// <returns></returns>
    public static bool IsTypeOf(object obj, Type type, bool shallow)
    {
      return ReflectionUtils.IsTypeOf(obj, type.FullName, shallow);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="typeFullName"></param>
    /// <returns></returns>
    public static bool IsTypeOf(object obj, string typeFullName)
    {
      return ReflectionUtils.IsTypeOf(obj, typeFullName, false);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="typeFullName"></param>
    /// <param name="shallow"></param>
    /// <returns></returns>
    public static bool IsTypeOf(object obj, string typeFullName, bool shallow)
    {
      if (obj != null)
      {
        if (shallow)
          return obj.GetType().FullName.Equals(typeFullName);
        Type type = obj.GetType();
        for (string fullName = type.FullName; !fullName.Equals("System.Object"); fullName = type.FullName)
        {
          if (fullName.Equals(typeFullName))
            return true;
          type = type.BaseType;
        }
      }
      return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="control"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsInTypeOf(Control control, Type type)
    {
      return ReflectionUtils.IsInTypeOf(control, type.FullName);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="control"></param>
    /// <param name="typeFullName"></param>
    /// <returns></returns>
    public static bool IsInTypeOf(Control control, string typeFullName)
    {
      return ReflectionUtils.GetTypeOfParent(control, typeFullName) != null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="control"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static Control GetTypeOfParent(Control control, Type type)
    {
      return ReflectionUtils.GetTypeOfParent(control, type.FullName);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="control"></param>
    /// <param name="typeFullName"></param>
    /// <returns></returns>
    public static Control GetTypeOfParent(Control control, string typeFullName)
    {
      for (Control parent = control.Parent; parent != null; parent = parent.Parent)
      {
        if (ReflectionUtils.IsTypeOf((object) parent, typeFullName))
          return parent;
      }
      return (Control) null;
    }
  }
}
