namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Allows overriding various display-related options for a given field. The options have the same meaning as in
    /// BoundField.
    /// </summary>
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    [H5.External]
    [H5.NonScriptable]
    public class DisplayFormatAttribute : Attribute
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public extern DisplayFormatAttribute();

        /// <summary>
        /// Gets or sets the format string
        /// </summary>
        public extern string DataFormatString { get; set; }

        /// <summary>
        /// Gets or sets the string to display when the value is null
        /// </summary>
        public extern string NullDisplayText { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether empty strings should be set to null
        /// </summary>
        public extern bool ConvertEmptyStringToNull { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the format string should be used in edit mode
        /// </summary>
        public extern bool ApplyFormatInEditMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the field should be html encoded
        /// </summary>
        public extern bool HtmlEncode { get; set; }
    }
}
