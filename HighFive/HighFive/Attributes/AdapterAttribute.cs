using System;

namespace HighFive
{
    [NonScriptable]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public abstract class AdapterAttribute : Attribute
    {
    }
}