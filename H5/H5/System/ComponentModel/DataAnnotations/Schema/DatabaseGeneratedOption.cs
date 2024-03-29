namespace System.ComponentModel.DataAnnotations.Schema
{
    /// <summary>
    /// The pattern used to generate values for a property in the database.
    /// </summary>
    [H5.External]
    public enum DatabaseGeneratedOption
    {
        /// <summary>
        /// The database does not generate values.
        /// </summary>
        None = 0,

        /// <summary>
        /// The database generates a value when a row is inserted.
        /// </summary>
        Identity = 1,

        /// <summary>
        /// The database generates a value when a row is inserted or updated.
        /// </summary>
        Computed = 2
    }
}
