using static H5.Core.dom;

namespace System.Net.Http
{
    public class StringContent : HttpContent
    {
        private const string DefaultMediaType = "text/plain";

        public StringContent(string content) : this(content, DefaultMediaType)
        {
        }

        public StringContent(string content, string mediaType)
        {
            MediaType = mediaType;
            Content = content;
        }

        public string Content { get; }
        public string MediaType { get; }
    }

    public class FormContent : HttpContent
    {
        public FormContent(FormData content)
        {
            Content = content;
        }

        public FormData Content { get; }
    }

    public class EmptyContent  : HttpContent
    {

    }
}