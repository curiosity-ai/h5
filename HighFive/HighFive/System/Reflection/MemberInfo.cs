namespace System.Reflection
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Name("System.Object")]
    public class MemberInfo
    {
        [HighFive.Name("t")]
        public extern MemberTypes MemberType
        {
            get;
        }

        [HighFive.Name("n")]
        public extern string Name
        {
            get;
        }

        [HighFive.Name("td")]
        public extern Type DeclaringType
        {
            get;
        }

        public extern bool IsStatic
        {
            [HighFive.Template("({this}.is || false)")]
            get;
        }

        public extern bool IsOverride
        {
            [HighFive.Template("({this}.ov || false)")]
            get;
        }

        public extern bool IsVirtual
        {
            [HighFive.Template("({this}.v || false)")]
            get;
        }

        public extern bool IsAbstract
        {
            [HighFive.Template("({this}.ab || false)")]
            get;
        }

        public extern bool IsSealed
        {
            [HighFive.Template("({this}.sl || false)")]
            get;
        }

        public extern bool IsSpecialName
        {
            [HighFive.Template("({this}.sy || false)")]
            get;
        }

        public extern bool IsFamily
        {
            [HighFive.Template("({this}.a === 3)")]
            get;
        }

        public extern bool IsFamilyOrAssembly
        {
            [HighFive.Template("({this}.a === 5)")]
            get;
        }

        public extern bool IsFamilyAndAssembly
        {
            [HighFive.Template("({this}.a === 6)")]
            get;
        }

        public extern bool IsPrivate
        {
            [HighFive.Template("({this}.a === 1)")]
            get;
        }

        public extern bool IsPublic
        {
            [HighFive.Template("({this}.a === 2)")]
            get;
        }

        public extern bool IsAssembly
        {
            [HighFive.Template("({this}.a === 4)")]
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

        [HighFive.Template("System.Attribute.isDefined({this}, {attributeType}, {inherit})")]
        public extern bool IsDefined(Type attributeType, bool inherit);

        public extern bool ContainsGenericParameters
        {
            [HighFive.Template("HighFive.Reflection.containsGenericParameters({this})")]
            get;
        }

        internal extern MemberInfo();
    }
}