namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// This attribute is used to mark the members of a Type that participate in optimistic concurrency checks.
    /// </summary>
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    [H5.External]
    [H5.NonScriptable]
    public sealed class ConcurrencyCheckAttribute : Attribute { }
}
