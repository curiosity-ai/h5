using System;

namespace HighFive
{
    [NonScriptable]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate | AttributeTargets.Interface, AllowMultiple = true)]
    public sealed class IgnoreCastAttribute : Attribute
    {
    }
}