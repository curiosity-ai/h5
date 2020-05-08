namespace System.ComponentModel.DataAnnotations.Schema
{
    /// <summary>
    /// Denotes that the class is a complex type.
    /// Complex types are non-scalar properties of entity types that enable scalar properties to be organized within
    /// entities.
    /// Complex types do not have keys and cannot be managed by the Entity Framework apart from the parent object.
    /// </summary>
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    [HighFive.External]
    [HighFive.NonScriptable]
    public class ComplexTypeAttribute : Attribute { }
}
