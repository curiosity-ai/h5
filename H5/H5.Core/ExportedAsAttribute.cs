// Decompiled with JetBrains decompiler
// Type: H5.ExportedAsAttribute
// Assembly: H5.Core, Version=1.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E855DC6-9E83-4420-9E6F-8D2B7A117BBD
// Assembly location: C:\work\curiosity\tesserae\Tesserae\bin\Debug\net461\HTML.dll

using H5;
using System;

namespace HTML
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public sealed class ExportedAsAttribute : Attribute
    {
        public bool IsExportAssign
        {
            get;set;
        }

        public bool IsExportDefault
        {
            get; set;
        }

        public bool IsStandalone
        {
            get; set;
        }

        public bool AsNamespace
        {
            get; set;
        }

        public extern ExportedAsAttribute(string fullName);
    }
}
