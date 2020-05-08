using System;

namespace H5
{
    [NonScriptable]
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class MixinAttribute : Attribute
    {
        public MixinAttribute(string expression)
        {
            Expression = expression;
        }

        public string Expression { get; private set; }
    }
}