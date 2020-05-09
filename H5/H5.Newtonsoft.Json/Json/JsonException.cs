using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Newtonsoft.Json
{
    /// <summary>
    /// The exception thrown when an error occurs during JSON serialization or deserialization.
    /// </summary>
    public class JsonException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonException"/> class.
        /// </summary>
        public JsonException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonException"/> class
        /// with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public JsonException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonException"/> class
        /// with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or <c>null</c> if no inner exception is specified.</param>
        public JsonException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}