using System;

namespace Bridge
{
    [NonScriptable]
    [AttributeUsage( AttributeTargets.Delegate | AttributeTargets.Method, AllowMultiple = false)]
    public sealed class ToAwaitAttribute : Attribute
    {
    }
}