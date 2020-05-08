namespace System.ComponentModel.DataAnnotations.Schema
{
    /// <summary>
    /// Specifies the database table that a class is mapped to.
    /// </summary>
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    [H5.External]
    [H5.NonScriptable]
    public class TableAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TableAttribute" /> class.
        /// </summary>
        /// <param name="name">The name of the table the class is mapped to.</param>
        public extern TableAttribute(string name);

        /// <summary>
        /// The name of the table the class is mapped to.
        /// </summary>
        public extern string Name { get; }

        /// <summary>
        /// The schema of the table the class is mapped to.
        /// </summary>
        public extern string Schema { get; set; }
    }
}
