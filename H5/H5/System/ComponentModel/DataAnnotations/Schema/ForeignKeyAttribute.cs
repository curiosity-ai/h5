namespace System.ComponentModel.DataAnnotations.Schema
{
    /// <summary>
    /// Denotes a property used as a foreign key in a relationship.
    /// The annotation may be placed on the foreign key property and specify the associated navigation property name,
    /// or placed on a navigation property and specify the associated foreign key name.
    /// </summary>
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    [H5.External]
    [H5.NonScriptable]
    public class ForeignKeyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ForeignKeyAttribute" /> class.
        /// </summary>
        /// <param name="name">
        /// If placed on a foreign key property, the name of the associated navigation property.
        /// If placed on a navigation property, the name of the associated foreign key(s).
        /// If a navigation property has multiple foreign keys, a comma separated list should be supplied.
        /// </param>
        public extern ForeignKeyAttribute(string name);

        /// <summary>
        /// If placed on a foreign key property, the name of the associated navigation property.
        /// If placed on a navigation property, the name of the associated foreign key(s).
        /// </summary>
        public extern string Name { get; }
    }
}
