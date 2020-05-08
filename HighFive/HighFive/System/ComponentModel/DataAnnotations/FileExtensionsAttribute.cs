namespace System.ComponentModel.DataAnnotations
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)]
    [HighFive.External]
    [HighFive.NonScriptable]
    public sealed class FileExtensionsAttribute : DataTypeAttribute
    {
        public extern FileExtensionsAttribute();

        public extern string Extensions { get; set; }
    }
}
