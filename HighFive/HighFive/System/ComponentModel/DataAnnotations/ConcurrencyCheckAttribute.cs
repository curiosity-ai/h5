namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// This attribute is used to mark the members of a Type that participate in optimistic concurrency checks.
    /// </summary>
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    [HighFive.External]
    [HighFive.NonScriptable]
    public sealed class ConcurrencyCheckAttribute : Attribute { }
}
