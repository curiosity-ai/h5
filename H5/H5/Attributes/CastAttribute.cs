using System;

namespace H5
{
    [NonScriptable]
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
    public sealed class CastAttribute : Attribute
    {
        public CastAttribute(string value)
        {
        }
    }
}