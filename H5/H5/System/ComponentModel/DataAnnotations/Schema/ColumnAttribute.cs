namespace System.ComponentModel.DataAnnotations.Schema
{
    /// <summary>
    /// Specifies the database column that a property is mapped to.
    /// </summary>
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    [H5.External]
    [H5.NonScriptable]
    public class ColumnAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnAttribute" /> class.
        /// </summary>
        public extern ColumnAttribute();

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnAttribute" /> class.
        /// </summary>
        /// <param name="name">The name of the column the property is mapped to.</param>
        public extern ColumnAttribute(string name);

        /// <summary>
        /// The name of the column the property is mapped to.
        /// </summary>
        public extern string Name { get; }

        /// <summary>
        /// The zero-based order of the column the property is mapped to.
        /// </summary>
        public extern int Order { get; set; }

        /// <summary>
        /// The database provider specific data type of the column the property is mapped to.
        /// </summary>
        public extern string TypeName { get; set; }
    }
}
