// Decompiled with JetBrains decompiler
// Type: Object.Net.Utilities.ObjectUtils
// Assembly: Object.Net.Utilities, Version=2.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0B5927F8-8BAA-4C4A-84B2-E1600C4FD3FC
// Assembly location: C:\bridge\2020-04-30\bridge\packages\Object.NET.Utilities.2.5.0\lib\Object.Net.Utilities.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
//using System.Web.UI.WebControls;

namespace Object.Net.Utilities
{
  /// <summary>
  /// 
  /// </summary>
  public static class ObjectUtils
  {
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="valueIfNull"></param>
    /// <returns></returns>
    public static T IfNull<T>(this T value, T valueIfNull)
    {
      return value.If<T>((Func<bool>) (() => ((object) (T) value).IsNull()), valueIfNull, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="test"></param>
    /// <param name="valueIfTrue"></param>
    /// <param name="valueIfFalse"></param>
    /// <returns></returns>
    public static T If<T>(this T value, Func<bool> test, T valueIfTrue, T valueIfFalse)
    {
      return !test() ? valueIfFalse : valueIfTrue;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="test"></param>
    /// <param name="valueIfTrue"></param>
    /// <param name="valueIfFalse"></param>
    /// <returns></returns>
    public static T IfNot<T>(this T value, Func<bool> test, T valueIfTrue, T valueIfFalse)
    {
      return test() ? valueIfFalse : valueIfTrue;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    public static bool IsNull(this object instance)
    {
      return instance == null;
    }

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    ///// <param name="to"></param>
    ///// <param name="from"></param>
    ///// <returns></returns>
    //public static T Apply<T>(object to, object from) where T : IComponent
    //{
    //  return (T) ObjectUtils.Apply(to, from);
    //}

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="to"></param>
    ///// <param name="from"></param>
    ///// <returns></returns>
    //public static object Apply(object to, object from)
    //{
    //  return ObjectUtils.Apply(to, from, true);
    //}

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="to"></param>
    ///// <param name="from"></param>
    ///// <param name="ignoreDefaultValues"></param>
    ///// <returns></returns>
    //public static object Apply(object to, object from, bool ignoreDefaultValues)
    //{
    //  foreach (PropertyInfo property1 in from.GetType().GetProperties())
    //  {
    //    if (property1.CanRead)
    //    {
    //      object obj1 = property1.GetValue(from, (object[]) null);
    //      if (ignoreDefaultValues)
    //      {
    //        object defaultValue = ReflectionUtils.GetDefaultValue(property1);
    //        if (obj1 != null && obj1.Equals(defaultValue))
    //          continue;
    //      }
    //      if (obj1 != null)
    //      {
    //        PropertyInfo property2 = to.GetType().GetProperty(property1.Name, BindingFlags.Instance | BindingFlags.Public);
    //        if (property2 != null && property2.GetType().Equals(property1.GetType()))
    //        {
    //          Type propertyType = property2.PropertyType;
    //          if (property2.CanWrite)
    //          {
    //            if (propertyType == typeof (Unit) && obj1 is int)
    //              obj1 = (object) Unit.Pixel((int) obj1);
    //            property2.SetValue(to, obj1, (object[]) null);
    //          }
    //          else if (property2.PropertyType.GetInterface("IList") != null)
    //          {
    //            IList list1 = property2.GetValue(to, (object[]) null) as IList;
    //            IList list2 = property1.GetValue(from, (object[]) null) as IList;
    //            if (list2.Count != 0)
    //            {
    //              Type genericItemType = typeof (object);
    //              if (propertyType.IsGenericType)
    //              {
    //                Type[] genericArguments = propertyType.GetGenericArguments();
    //                if (genericArguments != null && genericArguments.Length == 1)
    //                  genericItemType = genericArguments[0];
    //              }
    //              else
    //              {
    //                foreach (Type type in propertyType.GetInterfaces())
    //                {
    //                  if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof (IEnumerable<>))
    //                  {
    //                    genericItemType = type.GetGenericArguments()[0];
    //                    break;
    //                  }
    //                }
    //              }
    //              MethodInfo method = (MethodInfo) null;
    //              ((IEnumerable<MethodInfo>) propertyType.GetMethods()).Each<MethodInfo>((Action<MethodInfo>) (m =>
    //              {
    //                if (!(m.Name == "Add"))
    //                  return;
    //                ParameterInfo[] parameters = m.GetParameters();
    //                if (parameters == null || parameters.Length != 1 || !parameters[0].ParameterType.IsAssignableFrom(genericItemType) || method != null && !m.DeclaringType.IsSubclassOf(method.DeclaringType))
    //                  return;
    //                method = m;
    //              }));
    //              foreach (object obj2 in (IEnumerable) list2)
    //              {
    //                if (method != null)
    //                  method.Invoke((object) list1, new object[1]
    //                  {
    //                    obj2
    //                  });
    //                else
    //                  list1.Add(obj2);
    //              }
    //            }
    //          }
    //          else if (property2.PropertyType.GetInterface("IDictionary") != null)
    //          {
    //            IDictionary dictionary1 = property2.GetValue(to, (object[]) null) as IDictionary;
    //            IDictionary dictionary2 = property1.GetValue(from, (object[]) null) as IDictionary;
    //            if (dictionary2.Count != 0)
    //            {
    //              foreach (DictionaryEntry dictionaryEntry in dictionary2)
    //                dictionary1.Add(dictionaryEntry.Key, dictionaryEntry.Value);
    //            }
    //          }
    //        }
    //      }
    //    }
    //  }
    //  return to;
    //}
  }
}
