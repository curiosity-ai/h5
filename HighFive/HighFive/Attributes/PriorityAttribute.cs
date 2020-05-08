using System;

namespace HighFive
{
    [NonScriptable]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate | AttributeTargets.Interface, AllowMultiple = true)]
    public class PriorityAttribute : Attribute
    {
        public PriorityAttribute(int priority)
        {
        }
    }
}