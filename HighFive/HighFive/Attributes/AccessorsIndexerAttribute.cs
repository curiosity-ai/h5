using System;

namespace HighFive
{
    [NonScriptable]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class AccessorsIndexerAttribute : Attribute
    {
    }
}