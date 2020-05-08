namespace System.ComponentModel.DataAnnotations
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)]
    [H5.External]
    [H5.NonScriptable]
    public sealed class FileExtensionsAttribute : DataTypeAttribute
    {
        public extern FileExtensionsAttribute();

        public extern string Extensions { get; set; }
    }
}
