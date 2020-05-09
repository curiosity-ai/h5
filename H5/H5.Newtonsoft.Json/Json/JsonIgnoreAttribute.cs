using System;

namespace Newtonsoft.Json
{
    /// <summary>
    /// Instructs the JsonSerializer not to serialize the public field or public read/write property value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class JsonIgnoreAttribute : Attribute
    {
    }
}