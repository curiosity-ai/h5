using System;

namespace H5
{
    [NonScriptable]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate | AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class IgnoreGenericAttribute : Attribute
    {
        public bool AllowInTypeScript
        {
            get;
            set;
        }
    }
}