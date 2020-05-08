using System;

namespace H5
{
    [NonScriptable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Interface)]
    public sealed class FieldAttribute : Attribute
    {
    }
}