using System;

namespace HighFive
{
    [NonScriptable]
    [AttributeUsage(AttributeTargets.All)]
    public sealed class PrivateProtectedAttribute : Attribute
    {
    }
}