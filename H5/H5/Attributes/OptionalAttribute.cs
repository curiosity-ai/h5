using System;

namespace H5
{
    /// <summary>
    /// Attribute modifies the generated .d.ts to include the TypeScript ? optional modifier.
    /// </summary>
    [NonScriptable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class OptionalAttribute : Attribute
    {
    }
}