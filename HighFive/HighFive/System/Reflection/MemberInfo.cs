namespace System.Reflection
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Name("System.Object")]
    public class MemberInfo
    {
        [H5.Name("t")]
        public extern MemberTypes MemberType
        {
            get;
        }

        [H5.Name("n")]
        public extern string Name
        {
            get;
        }

        [H5.Name("td")]
        public extern Type DeclaringType
        {
            get;
        }

        public extern bool IsStatic
        {
            [H5.Template("({this}.is || false)")]
            get;
        }

        public extern bool IsOverride
        {
            [H5.Template("({this}.ov || false)")]
            get;
        }

        public extern bool IsVirtual
        {
            [H5.Template("({this}.v || false)")]
            get;
        }

        public extern bool IsAbstract
        {
            [H5.Template("({this}.ab || false)")]
            get;
        }

        public extern bool IsSealed
        {
            [H5.Template("({this}.sl || false)")]
            get;
        }

        public extern bool IsSpecialName
        {
            [H5.Template("({this}.sy || false)")]
            get;
        }

        public extern bool IsFamily
        {
            [H5.Template("({this}.a === 3)")]
            get;
        }

        public extern bool IsFamilyOrAssembly
        {
            [H5.Template("({this}.a === 5)")]
            get;
        }

        public extern bool IsFamilyAndAssembly
        {
            [H5.Template("({this}.a === 6)")]
            get;
        }

        public extern bool IsPrivate
        {
            [H5.Template("({this}.a === 1)")]
            get;
        }

        public extern bool IsPublic
        {
            [H5.Template("({this}.a === 2)")]
            get;
        }

        public extern bool IsAssembly
        {
            [H5.Template("({this}.a === 4)")]
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

        [H5.Template("System.Attribute.isDefined({this}, {attributeType}, {inherit})")]
        public extern bool IsDefined(Type attributeType, bool inherit);

        public extern bool ContainsGenericParameters
        {
            [H5.Template("H5.Reflection.containsGenericParameters({this})")]
            get;
        }

        internal extern MemberInfo();
    }
}