namespace System.ComponentModel.DataAnnotations.Schema
{
    /// <summary>
    /// Denotes that a property or class should be excluded from database mapping.
    /// </summary>
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class, AllowMultiple = false)]
    [HighFive.External]
    [HighFive.NonScriptable]
    public class NotMappedAttribute : Attribute { }
}
