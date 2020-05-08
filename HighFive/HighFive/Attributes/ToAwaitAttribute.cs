using System;

namespace H5
{
    [NonScriptable]
    [AttributeUsage( AttributeTargets.Delegate | AttributeTargets.Method, AllowMultiple = false)]
    public sealed class ToAwaitAttribute : Attribute
    {
    }
}