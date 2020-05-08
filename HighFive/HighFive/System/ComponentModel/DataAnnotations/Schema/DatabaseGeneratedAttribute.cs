namespace System.ComponentModel.DataAnnotations.Schema
{
    /// <summary>
    /// Specifies how the database generates values for a property.
    /// </summary>
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    [H5.External]
    [H5.NonScriptable]
    public class DatabaseGeneratedAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseGeneratedAttribute" /> class.
        /// </summary>
        /// <param name="databaseGeneratedOption">The pattern used to generate values for the property in the database.</param>
        public extern DatabaseGeneratedAttribute(DatabaseGeneratedOption databaseGeneratedOption);

        /// <summary>
        /// The pattern used to generate values for the property in the database.
        /// </summary>
        public extern DatabaseGeneratedOption DatabaseGeneratedOption { get; }
    }
}
