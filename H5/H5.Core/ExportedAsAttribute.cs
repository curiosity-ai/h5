// Decompiled with JetBrains decompiler
// Type: H5.ExportedAsAttribute
// Assembly: H5.Core, Version=1.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E855DC6-9E83-4420-9E6F-8D2B7A117BBD
// Assembly location: C:\work\curiosity\tesserae\Tesserae\bin\Debug\net461\H5.Core.dll

using System;

namespace H5
{
  [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
  public sealed class ExportedAsAttribute : Attribute
  {
    private bool \u003CIsExportAssign\u003Ek__BackingField;
    private bool \u003CIsExportDefault\u003Ek__BackingField;
    private bool \u003CIsStandalone\u003Ek__BackingField;
    private bool \u003CAsNamespace\u003Ek__BackingField;

    public bool IsExportAssign
    {
      get
      {
        return this.\u003CIsExportAssign\u003Ek__BackingField;
      }
      set
      {
        this.\u003CIsExportAssign\u003Ek__BackingField = value;
      }
    }

    public bool IsExportDefault
    {
      get
      {
        return this.\u003CIsExportDefault\u003Ek__BackingField;
      }
      set
      {
        this.\u003CIsExportDefault\u003Ek__BackingField = value;
      }
    }

    public bool IsStandalone
    {
      get
      {
        return this.\u003CIsStandalone\u003Ek__BackingField;
      }
      set
      {
        this.\u003CIsStandalone\u003Ek__BackingField = value;
      }
    }

    public bool AsNamespace
    {
      get
      {
        return this.\u003CAsNamespace\u003Ek__BackingField;
      }
      set
      {
        this.\u003CAsNamespace\u003Ek__BackingField = value;
      }
    }

    public extern ExportedAsAttribute(string fullName);
  }
}
