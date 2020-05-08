using System;

namespace HighFive
{
    [NonScriptable]
    [AttributeUsage( AttributeTargets.Delegate | AttributeTargets.Method, AllowMultiple = false)]
    public sealed class ToAwaitAttribute : Attribute
    {
    }
}