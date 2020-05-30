using System.Collections.Generic;

namespace System
{
    public class MissingMethodException : Exception
    {
        public MissingMethodException()
            : base("Attempted to access a missing method.")
        {
        }

        public MissingMethodException(string message)
            : base(message)
        {
        }

        public MissingMethodException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public MissingMethodException(string className, string methodName) : base (className + "." + methodName + " Due to: Attempted to access a missing member.")
        {
        }
    }
}