using System;
using System.Text;

namespace H5.Contract
{
    public class TranslatorOutputItemContent
    {
        public StringBuilder Builder
        {
            get; private set;
        }

        public string String
        {
            get; private set;
        }

        public byte[] Buffer
        {
            get; private set;
        }

        private Encoding OutputEncoding = new UTF8Encoding(false);

        public TranslatorOutputItemContent(StringBuilder content)
        {
            this.Builder = content;
        }

        public TranslatorOutputItemContent(string content)
        {
            this.String = content;
        }

        public TranslatorOutputItemContent(byte[] content)
        {
            this.Buffer = content;
        }

        public void SetContent(string s)
        {
            Builder = null;
            Buffer = null;
            String = s;
        }

        public object GetContent(bool stringableOnly = false)
        {
            if (Builder != null)
            {
                return Builder;
            }

            if (String != null)
            {
                return String;
            }

            if (Buffer != null)
            {
                if (stringableOnly)
                {
                    throw new InvalidOperationException("Cannot get stringable content as underlying data is Buffer.");
                }

                return Buffer;
            }

            return null;
        }

        public string GetContentAsString()
        {
            if (Builder != null)
            {
                return Builder.ToString();
            }

            if (String != null)
            {
                return String;
            }

            if (Buffer != null)
            {
                return OutputEncoding.GetString(Buffer);
            }

            return null;
        }

        public byte[] GetContentAsBytes()
        {
            if (Builder != null)
            {
                return GetBytesFromString(Builder.ToString());
            }

            if (String != null)
            {
                return GetBytesFromString(String);
            }

            if (Buffer != null)
            {
                return Buffer;
            }

            return new byte[0];
        }

        private byte[] GetBytesFromString(string s)
        {
            if (s == null)
            {
                return new byte[0];
            }

            return OutputEncoding.GetBytes(s);
        }

        public static implicit operator TranslatorOutputItemContent(StringBuilder content)
        {
            return new TranslatorOutputItemContent(content);
        }
        public static implicit operator TranslatorOutputItemContent(string content)
        {
            return new TranslatorOutputItemContent(content);
        }
        public static implicit operator TranslatorOutputItemContent(byte[] content)
        {
            return new TranslatorOutputItemContent(content);
        }
    }
}