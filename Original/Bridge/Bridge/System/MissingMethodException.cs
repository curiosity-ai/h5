using System.Collections.Generic;

namespace System
{
    public class MissingMethodException : Exception
    {
        public MissingMethodException()
            : base("Attempted to access a missing method.")
        {
        }

        public MissingMethodException(String message)
            : base(message)
        {
        }

        public MissingMethodException(String message, Exception inner)
            : base(message, inner)
        {
        }

        public MissingMethodException(String className, String methodName) : base (className + "." + methodName + " Due to: Attempted to access a missing member.")
        {
        }
    }
}