namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// This attribute is used to mark a Timestamp member of a Type.
    /// </summary>
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    [HighFive.External]
    [HighFive.NonScriptable]
    public sealed class TimestampAttribute : Attribute { }
}
