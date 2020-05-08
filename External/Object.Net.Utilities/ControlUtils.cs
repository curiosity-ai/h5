// Decompiled with JetBrains decompiler
// Type: Object.Net.Utilities.ControlUtils
// Assembly: Object.Net.Utilities, Version=2.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0B5927F8-8BAA-4C4A-84B2-E1600C4FD3FC
// Assembly location: C:\highfive\2020-04-30\highfive\packages\Object.NET.Utilities.2.5.0\lib\Object.Net.Utilities.dll

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

namespace Object.Net.Utilities
{
  public static class ControlUtils
  {
    public static bool HasControls(Control control)
    {
      if (control == null)
        return false;
      return control.HasControls() || control.Controls.Count > 0;
    }

    public static Control FindControl(Control seed, string id)
    {
      return ControlUtils.FindControl(seed, id, true, (Control) null);
    }

    public static Control FindControl(Control seed, string id, bool traverse)
    {
      return ControlUtils.FindControl(seed, id, traverse, (Control) null);
    }

    private static Control FindControl(
      Control seed,
      string id,
      bool traverse,
      Control branch)
    {
      if (seed == null || string.IsNullOrEmpty(id))
        return (Control) null;
      Control control1 = (Control) null;
      try
      {
        control1 = seed.FindControl(id);
        if (control1 != null)
          return control1;
      }
      catch (HttpException ex)
      {
      }
      Control branch1 = seed is INamingContainer ? seed : seed.NamingContainer;
      string str = branch != null ? branch.ID ?? "" : "";
      foreach (Control control2 in branch1.Controls)
      {
        if (!str.Equals(control2.ID) && ControlUtils.HasControls(control2))
          control1 = ControlUtils.FindChildControl(control2, id);
        if (control1 != null)
          break;
      }
      if (traverse && control1 == null)
        control1 = ControlUtils.FindControl(branch1.NamingContainer, id, traverse, branch1);
      return control1;
    }

    public static T FindControl<T>(Control seed, string id) where T : Control
    {
      return ControlUtils.FindControl<T>(seed, id, true, (Control) null);
    }

    public static T FindControl<T>(Control seed, string id, bool traverse) where T : Control
    {
      return ControlUtils.FindControl<T>(seed, id, traverse, (Control) null);
    }

    public static T FindControl<T>(Control seed, string id, bool traverse, Control branch) where T : Control
    {
      Control control = ControlUtils.FindControl(seed, id, traverse, branch);
      if (control != null && !ReflectionUtils.IsTypeOf((object) control, typeof (T)))
        throw new InvalidCastException(string.Format("The Control ID ('{0}') was found, but it was not a type of {1}. The found Control was a type of {2}.", (object) id, (object) typeof (T).ToString(), (object) control.GetType().ToString()));
      return control as T;
    }

    public static T FindControl<T>(Control seed) where T : Control
    {
      return ControlUtils.FindControl(seed, typeof (T)) as T;
    }

    public static T FindControl<T>(Control seed, bool shallow) where T : Control
    {
      return ControlUtils.FindControl(seed, typeof (T), shallow) as T;
    }

    public static Control FindControl(Control seed, Type type)
    {
      return ControlUtils.FindControl(seed, type, false);
    }

    public static Control FindControl(Control seed, Type type, bool shallow)
    {
      return ControlUtils.FindControlByTypeName(seed, type.FullName, shallow, true, (Control) null);
    }

    public static Control FindControlByTypeName(Control seed, string typeFullName)
    {
      return ControlUtils.FindControlByTypeName(seed, typeFullName, false, true, (Control) null);
    }

    private static Control FindControlByTypeName(
      Control seed,
      string typeFullName,
      bool shallow,
      bool traverse,
      Control branch)
    {
      if (seed == null || string.IsNullOrEmpty(typeFullName))
        return (Control) null;
      Control branch1 = seed is INamingContainer ? seed : seed.NamingContainer;
      if (ReflectionUtils.IsTypeOf((object) branch1, typeFullName, shallow))
        return branch1;
      Control control1 = (Control) null;
      string str = branch != null ? branch.ID ?? "" : "";
      foreach (Control control2 in branch1.Controls)
      {
        if (!str.Equals(control2.ID))
        {
          if (ReflectionUtils.IsTypeOf((object) control2, typeFullName, shallow))
            control1 = control2;
          else if (ControlUtils.HasControls(control2))
            control1 = ControlUtils.FindChildControl(control2, typeFullName, shallow);
          if (control1 != null)
            break;
        }
      }
      if (traverse && control1 == null)
        control1 = ControlUtils.FindControlByTypeName(branch1.NamingContainer, typeFullName, shallow, traverse, branch1);
      return control1;
    }

    public static Control FindChildControl(Control seed, string id)
    {
      if (seed == null || string.IsNullOrEmpty(id))
        return (Control) null;
      Control control1 = (Control) null;
      try
      {
        control1 = seed.FindControl(id);
        if (control1 != null)
          return control1;
      }
      catch (HttpException ex)
      {
      }
      foreach (Control control2 in seed.Controls)
      {
        if (ControlUtils.HasControls(control2))
          control1 = ControlUtils.FindChildControl(control2, id);
        if (control1 != null)
          break;
      }
      return control1;
    }

    public static T FindChildControl<T>(Control seed, string id) where T : Control
    {
      Control childControl = ControlUtils.FindChildControl(seed, id);
      if (childControl != null && !ReflectionUtils.IsTypeOf((object) childControl, typeof (T)))
        throw new InvalidCastException(string.Format("The Control ID ('{0}') was found, but it was not a type of {1}. The found Control was a type of {2}.", (object) id, (object) typeof (T).ToString(), (object) childControl.GetType().ToString()));
      return childControl as T;
    }

    public static Control FindChildControl(Control seed, string typeFullName, bool shallow)
    {
      if (seed == null || string.IsNullOrEmpty(typeFullName))
        return (Control) null;
      Control control1 = (Control) null;
      foreach (Control control2 in seed.Controls)
      {
        if (ReflectionUtils.IsTypeOf((object) control2, typeFullName, shallow))
          control1 = control2;
        else if (ControlUtils.HasControls(control2))
          control1 = ControlUtils.FindChildControl(control2, typeFullName, shallow);
        if (control1 != null)
          break;
      }
      return control1;
    }

    public static T FindChildControl<T>(Control seed) where T : Control
    {
      return ControlUtils.FindChildControl(seed, typeof (T), false) as T;
    }

    public static T FindChildControl<T>(Control seed, bool shallow) where T : Control
    {
      return ControlUtils.FindChildControl(seed, typeof (T), shallow) as T;
    }

    public static Control FindChildControl(Control seed, Type type)
    {
      return ControlUtils.FindChildControl(seed, type, false);
    }

    public static Control FindChildControl(Control seed, Type type, bool shallow)
    {
      return ControlUtils.FindChildControl(seed, type.FullName, shallow);
    }

    public static List<T> FindControls<T>(Control seed) where T : Control
    {
      return ControlUtils.FindControls<T>(seed, false);
    }

    public static List<T> FindControls<T>(Control seed, bool shallow) where T : Control
    {
      if (seed == null)
        return (List<T>) null;
      seed = seed is INamingContainer ? seed : seed.NamingContainer;
      List<T> objList = new List<T>();
      foreach (Control control in seed.Controls)
      {
        if (ReflectionUtils.IsTypeOf((object) control, typeof (T), shallow))
          objList.Add(control as T);
        if (ControlUtils.HasControls(control))
          objList.AddRange((IEnumerable<T>) ControlUtils.FindChildControls<T>(control, shallow));
      }
      return objList;
    }

    public static List<T> FindControls<T>(Control seed, string typeFullName, bool shallow) where T : Control
    {
      if (seed == null || string.IsNullOrEmpty(typeFullName))
        return (List<T>) null;
      seed = seed is INamingContainer ? seed : seed.NamingContainer;
      List<T> objList = new List<T>();
      foreach (Control control in seed.Controls)
      {
        if (ReflectionUtils.IsTypeOf((object) control, typeFullName, shallow))
          objList.Add(control as T);
        if (ControlUtils.HasControls(control))
          objList.AddRange((IEnumerable<T>) ControlUtils.FindChildControls<T>(control, typeFullName, shallow));
      }
      return objList;
    }

    public static List<T> FindChildControls<T>(Control seed) where T : Control
    {
      return ControlUtils.FindChildControls<T>(seed, false);
    }

    public static List<T> FindChildControls<T>(Control seed, bool shallow) where T : Control
    {
      if (seed == null)
        return (List<T>) null;
      List<T> objList = new List<T>();
      foreach (Control control in seed.Controls)
      {
        if (ReflectionUtils.IsTypeOf((object) control, typeof (T), shallow))
          objList.Add(control as T);
        if (ControlUtils.HasControls(control))
          objList.AddRange((IEnumerable<T>) ControlUtils.FindChildControls<T>(control, shallow));
      }
      return objList;
    }

    public static List<T> FindChildControls<T>(Control seed, string typeFullName, bool shallow) where T : Control
    {
      if (seed == null || string.IsNullOrEmpty(typeFullName))
        return (List<T>) null;
      List<T> objList = new List<T>();
      foreach (Control control in seed.Controls)
      {
        if (ReflectionUtils.IsTypeOf((object) control, typeFullName, shallow))
          objList.Add(control as T);
        if (ControlUtils.HasControls(control))
          objList.AddRange((IEnumerable<T>) ControlUtils.FindChildControls<T>(control, typeFullName, shallow));
      }
      return objList;
    }

    public static bool IsChildOfParent(Control parent, Control child)
    {
      if (parent != null && child != null && !parent.UniqueID.Equals(child.UniqueID))
      {
        for (Control parent1 = child.Parent; parent1 != null; parent1 = parent1.Parent)
        {
          if (parent1.UniqueID == parent.UniqueID)
            return true;
        }
      }
      return false;
    }

    public static Control FindControlByClientID(
      Control seed,
      string clientID,
      bool traverse,
      Control branch)
    {
      if (seed == null || string.IsNullOrEmpty(clientID))
        return (Control) null;
      Control branch1 = seed is INamingContainer ? seed : seed.NamingContainer;
      if (clientID.Equals(branch1.ClientID ?? ""))
        return branch1;
      Control control1 = (Control) null;
      string text = branch != null ? branch.ClientID ?? "" : "";
      List<Control> controlList = new List<Control>();
      foreach (Control control2 in branch1.Controls)
      {
        string str1 = control2.ID ?? "";
        string str2 = control2.ClientID ?? "";
        if (clientID.Equals(str1) || clientID.Equals(str2))
          control1 = control2;
        else if (ControlUtils.HasControls(control2) && (text.IsEmpty() || !text.Equals(str2)))
          control1 = ControlUtils.FindChildControlByClientID(control2, clientID);
        if (control1 != null)
          break;
      }
      if (traverse && control1 == null)
        control1 = ControlUtils.FindControlByClientID(branch1.NamingContainer, clientID, true, branch1);
      return control1;
    }

    public static Control FindChildControlByClientID(Control seed, string clientID)
    {
      if (seed == null || string.IsNullOrEmpty(clientID))
        return (Control) null;
      Control control1 = (Control) null;
      foreach (Control control2 in seed.Controls)
      {
        string str1 = control2.ID ?? "";
        string str2 = control2.ClientID ?? "";
        if (clientID.Equals(str1) || clientID.Equals(str2))
          control1 = control2;
        else if (ControlUtils.HasControls(control2))
          control1 = ControlUtils.FindChildControlByClientID(control2, clientID);
        if (control1 != null)
          break;
      }
      return control1;
    }
  }
}
