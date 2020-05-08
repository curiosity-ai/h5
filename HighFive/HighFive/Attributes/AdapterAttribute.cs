using System;

namespace H5
{
    [NonScriptable]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public abstract class AdapterAttribute : Attribute
    {
    }
}