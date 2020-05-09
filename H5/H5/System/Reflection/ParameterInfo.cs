namespace System.Reflection
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Name("System.Object")]
    public class ParameterInfo
    {
        [H5.Name("sn")]
        public extern string ScriptName
        {
            get;
        }

        [H5.Name("n")]
        public extern string Name
        {
            get;
        }

        [H5.Name("dv")]
        public extern string DefaultValue
        {
            get;
        }

        public extern bool HasDefaultValue
        {
            [H5.Template("({this}.isOptional || false)")]
            get;
        }

        public extern bool IsOptional
        {
            [H5.Template("({this}.o || false)")]
            get;
        }

        public extern bool IsOut
        {
            [H5.Template("({this}.out || false)")]
            get;
        }

        public extern bool IsRef
        {
            [H5.Template("({this}.ref || false)")]
            get;
        }

        public extern bool IsParams
        {
            [H5.Template("({this}.ip || false)")]
            get;
        }

        [H5.Name("pt")]
        public extern Type ParameterType
        {
            get;
        }

        [H5.Name("ps")]
        public extern int Position
        {
            get;
        }

        /// <summary>
        /// Returns an array of all custom attributes applied to this member.
        /// </summary>
        /// <param name="inherit">Ignored for members. Base members will never be considered.</param>
        /// <returns>An array that contains all the custom attributes applied to this member, or an array with zero elements if no attributes are defined. </returns>
        [H5.Template("System.Attribute.getCustomAttributes({this}, false, {inherit})")]
        public extern object[] GetCustomAttributes(bool inherit);

        /// <summary>
        /// Returns an array of custom attributes applied to this member and identified by <see cref="T:System.Type"/>.
        /// </summary>
        /// <param name="attributeType">The type of attribute to search for. Only attributes that are assignable to this type are returned. </param>
        /// <param name="inherit">Ignored for members. Base members will never be considered.</param>
        /// <returns>An array that contains all the custom attributes applied to this member, or an array with zero elements if no attributes are defined.</returns>
        [H5.Template("System.Attribute.getCustomAttributes({this}, {attributeType}, {inherit})")]
        public extern object[] GetCustomAttributes(Type attributeType, bool inherit);

        /// <summary>
        /// Returns an array of all custom attributes applied to this member.
        /// </summary>
        /// <returns>An array that contains all the custom attributes applied to this member, or an array with zero elements if no attributes are defined. </returns>
        [H5.Template("System.Attribute.getCustomAttributes({this}, false)")]
        public extern object[] GetCustomAttributes();

        /// <summary>
        /// Returns an array of custom attributes applied to this member and identified by <see cref="T:System.Type"/>.
        /// </summary>
        /// <param name="attributeType">The type of attribute to search for. Only attributes that are assignable to this type are returned. </param>
        /// <returns>An array that contains all the custom attributes applied to this member, or an array with zero elements if no attributes are defined.</returns>
        [H5.Template("System.Attribute.getCustomAttributes({this}, {attributeType})")]
        public extern object[] GetCustomAttributes(Type attributeType);
    }
}