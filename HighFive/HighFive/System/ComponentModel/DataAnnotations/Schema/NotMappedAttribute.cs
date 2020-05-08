namespace System.ComponentModel.DataAnnotations.Schema
{
    /// <summary>
    /// Denotes that a property or class should be excluded from database mapping.
    /// </summary>
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class, AllowMultiple = false)]
    [H5.External]
    [H5.NonScriptable]
    public class NotMappedAttribute : Attribute { }
}
