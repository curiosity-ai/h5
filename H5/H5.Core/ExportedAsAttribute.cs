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
    private bool _IsExportAssign_BackingField;
    private bool _IsExportDefault_BackingField;
    private bool _IsStandalone_BackingField;
    private bool _AsNamespace_BackingField;

    public bool IsExportAssign
    {
      get
      {
        return this._IsExportAssign_BackingField;
      }
      set
      {
        this._IsExportAssign_BackingField = value;
      }
    }

    public bool IsExportDefault
    {
      get
      {
        return this._IsExportDefault_BackingField;
      }
      set
      {
        this._IsExportDefault_BackingField = value;
      }
    }

    public bool IsStandalone
    {
      get
      {
        return this._IsStandalone_BackingField;
      }
      set
      {
        this._IsStandalone_BackingField = value;
      }
    }

    public bool AsNamespace
    {
      get
      {
        return this._AsNamespace_BackingField;
      }
      set
      {
        this._AsNamespace_BackingField = value;
      }
    }

    public extern ExportedAsAttribute(string fullName);
  }
}
