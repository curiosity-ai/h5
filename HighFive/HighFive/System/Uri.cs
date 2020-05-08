namespace System
{
    /// <summary>
    /// Provides an object representation of a uniform resource identifier (URI) and easy access to the parts of the URI.
    /// </summary>
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Reflectable]
    public class Uri
    {
        public extern Uri(string uriString);

        public extern string AbsoluteUri
        {
            [HighFive.Template("getAbsoluteUri()")]
            get;
        }

        [HighFive.Template("System.Uri.equals({uri1}, {uri2})")]
        public static extern bool operator ==(Uri uri1, Uri uri2);

        [HighFive.Template("System.Uri.notEquals({uri1}, {uri2})")]
        public static extern bool operator !=(Uri uri1, Uri uri2);
    }
}