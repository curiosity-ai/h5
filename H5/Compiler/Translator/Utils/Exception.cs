using H5.Contract;
using System;

namespace H5.Translator
{
    public class TranslatorException : Exception, IVisitorException
    {
        public TranslatorException()
        {
        }

        public TranslatorException(string message)
            : base(message)
        {
        }

        public TranslatorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public static IVisitorException Create(string format, params object[] args)
        {
            return new TranslatorException(String.Format(format, args));
        }

        public static void Throw(string format, params object[] args)
        {
            throw (TranslatorException)Create(format, args);
        }
    }
}