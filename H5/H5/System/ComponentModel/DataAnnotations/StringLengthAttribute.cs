namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Validation attribute to assert a string property, field or parameter does not exceed a maximum length
    /// </summary>
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)]
    [H5.External]
    [H5.NonScriptable]
    public class StringLengthAttribute : ValidationAttribute
    {
        /// <summary>
        /// Constructor that accepts the maximum length of the string.
        /// </summary>
        /// <param name="maximumLength">The maximum length, inclusive.  It may not be negative.</param>
        public extern StringLengthAttribute(int maximumLength);

        /// <summary>
        /// Gets the maximum acceptable length of the string
        /// </summary>
        public extern int MaximumLength { get; }

        /// <summary>
        /// Gets or sets the minimum acceptable length of the string
        /// </summary>
        public extern int MinimumLength { get; set; }

        /// <summary>
        /// Override of <see cref="ValidationAttribute.IsValid(object)" />
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <exception cref="InvalidOperationException"> is thrown if the current attribute is ill-formed.</exception>
        public override extern bool IsValid(object value);

        /// <summary>
        /// Override of <see cref="ValidationAttribute.FormatErrorMessage" />
        /// </summary>
        /// <param name="name">The name to include in the formatted string</param>
        /// <exception cref="InvalidOperationException"> is thrown if the current attribute is ill-formed.</exception>
        public override extern string FormatErrorMessage(string name);
    }
}
