namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Used to mark one or more entity properties that provide the entity's unique identity
    /// </summary>
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    [HighFive.External]
    [HighFive.NonScriptable]
    public sealed class KeyAttribute : Attribute { }
}
