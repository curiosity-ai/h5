using System;

namespace HighFive
{
    [NonScriptable]
    [AttributeUsage(AttributeTargets.Struct, AllowMultiple = false)]
    public sealed class ImmutableAttribute : Attribute
    {
    }
}