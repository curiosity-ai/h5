using System;

namespace HighFive
{
    /// <summary>
    /// Applies to interface if it's implementation is done outside HighFive type system (class implementation doesn't provide aliases for interface members implementations)
    /// </summary>
    [NonScriptable]
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface)]
    public sealed class ExternalInterfaceAttribute : Attribute
    {
        public extern ExternalInterfaceAttribute();
        public extern ExternalInterfaceAttribute(bool nativeImplementation);

        public bool IsVirtual
        {
            get; set;
        }
    }
}