namespace System.ComponentModel.DataAnnotations.Schema
{
    /// <summary>
    /// Specifies the inverse of a navigation property that represents the other end of the same relationship.
    /// </summary>
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    [H5.External]
    [H5.NonScriptable]
    public class InversePropertyAttribute : Attribute
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="InversePropertyAttribute" /> class.
        /// </summary>
        /// <param name="property">The navigation property representing the other end of the same relationship.</param>
        public extern InversePropertyAttribute(string property);

        /// <summary>
        /// The navigation property representing the other end of the same relationship.
        /// </summary>
        public extern string Property { get; }
    }
}
