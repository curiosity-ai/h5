using System;

namespace H5
{
    [NonScriptable]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class AccessorsIndexerAttribute : Attribute
    {
    }
}