namespace System
{
    /// <summary>
    /// Provides an object representation of a uniform resource identifier (URI) and easy access to the parts of the URI.
    /// </summary>
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Reflectable]
    public class Uri
    {
        public extern Uri(string uriString);

        public extern string AbsoluteUri
        {
            [H5.Template("getAbsoluteUri()")]
            get;
        }

        [H5.Template("System.Uri.equals({uri1}, {uri2})")]
        public static extern bool operator ==(Uri uri1, Uri uri2);

        [H5.Template("System.Uri.notEquals({uri1}, {uri2})")]
        public static extern bool operator !=(Uri uri1, Uri uri2);
    }
}