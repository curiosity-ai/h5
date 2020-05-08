namespace System.ComponentModel.DataAnnotations
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Parameter,
        AllowMultiple = false)]
    [HighFive.External]
    [HighFive.NonScriptable]
    public sealed class EnumDataTypeAttribute : DataTypeAttribute
    {
        public extern EnumDataTypeAttribute(Type enumType);

        public extern Type EnumType { get; }
    }
}
