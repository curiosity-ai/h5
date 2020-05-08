namespace System.Reflection
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Name("System.Object")]
    public class ParameterInfo
    {
        [HighFive.Name("sn")]
        public extern string ScriptName
        {
            get;
        }

        [HighFive.Name("n")]
        public extern string Name
        {
            get;
        }

        [HighFive.Name("dv")]
        public extern string DefaultValue
        {
            get;
        }

        public extern bool HasDefaultValue
        {
            [HighFive.Template("({this}.isOptional || false)")]
            get;
        }

        public extern bool IsOptional
        {
            [HighFive.Template("({this}.o || false)")]
            get;
        }

        public extern bool IsOut
        {
            [HighFive.Template("({this}.out || false)")]
            get;
        }

        public extern bool IsRef
        {
            [HighFive.Template("({this}.ref || false)")]
            get;
        }

        public extern bool IsParams
        {
            [HighFive.Template("({this}.ip || false)")]
            get;
        }

        [HighFive.Name("pt")]
        public extern Type ParameterType
        {
            get;
        }

        [HighFive.Name("ps")]
        public extern int Position
        {
            get;
        }

        /// <summary>
        /// Returns an array of all custom attributes applied to this member.
        /// </summary>
        /// <param name="inherit">Ignored for members. Base members will never be considered.</param>
        /// <returns>An array that contains all the custom attributes applied to this member, or an array with zero elements if no attributes are defined. </returns>
        [HighFive.Template("System.Attribute.getCustomAttributes({this}, false, {inherit})")]
        public extern object[] GetCustomAttributes(bool inherit);

        /// <summary>
        /// Returns an array of custom attributes applied to this member and identified by <see cref="T:System.Type"/>.
        /// </summary>
        /// <param name="attributeType">The type of attribute to search for. Only attributes that are assignable to this type are returned. </param>
        /// <param name="inherit">Ignored for members. Base members will never be considered.</param>
        /// <returns>An array that contains all the custom attributes applied to this member, or an array with zero elements if no attributes are defined.</returns>
        [HighFive.Template("System.Attribute.getCustomAttributes({this}, {attributeType}, {inherit})")]
        public extern object[] GetCustomAttributes(Type attributeType, bool inherit);

        /// <summary>
        /// Returns an array of all custom attributes applied to this member.
        /// </summary>
        /// <returns>An array that contains all the custom attributes applied to this member, or an array with zero elements if no attributes are defined. </returns>
        [HighFive.Template("System.Attribute.getCustomAttributes({this}, false)")]
        public extern object[] GetCustomAttributes();

        /// <summary>
        /// Returns an array of custom attributes applied to this member and identified by <see cref="T:System.Type"/>.
        /// </summary>
        /// <param name="attributeType">The type of attribute to search for. Only attributes that are assignable to this type are returned. </param>
        /// <returns>An array that contains all the custom attributes applied to this member, or an array with zero elements if no attributes are defined.</returns>
        [HighFive.Template("System.Attribute.getCustomAttributes({this}, {attributeType})")]
        public extern object[] GetCustomAttributes(Type attributeType);
    }
}