namespace System.Reflection
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Unbox(true)]
    public class MethodInfo : MethodBase
    {
        [H5.Name("rt")]
        public extern Type ReturnType
        {
            get;
            private set;
        }

        /// <summary>
        /// Returns an array of all custom attributes applied to this member.
        /// </summary>
        /// <param name="inherit">Ignored for members. Base members will never be considered.</param>
        /// <returns>An array that contains all the custom attributes applied to this member, or an array with zero elements if no attributes are defined. </returns>
        [H5.Template("({this}.rta || [])")]
        public extern object[] GetReturnTypeCustomAttributes(bool inherit);

        /// <summary>
        /// Returns an array of custom attributes applied to this member and identified by <see cref="T:System.Type"/>.
        /// </summary>
        /// <param name="attributeType">The type of attribute to search for. Only attributes that are assignable to this type are returned. </param>
        /// <param name="inherit">Ignored for members. Base members will never be considered.</param>
        /// <returns>An array that contains all the custom attributes applied to this member, or an array with zero elements if no attributes are defined.</returns>
        [H5.Template("({this}.rta || []).filter(function (a) { return H5.is(a, {attributeType}); })")]
        public extern object[] GetReturnTypeCustomAttributes(Type attributeType, bool inherit);

        /// <summary>
        /// Returns an array of all custom attributes applied to this member.
        /// </summary>
        /// <returns>An array that contains all the custom attributes applied to this member, or an array with zero elements if no attributes are defined. </returns>
        [H5.Template("({this}.rta || [])")]
        public extern object[] GetReturnTypeCustomAttributes();

        /// <summary>
        /// Returns an array of custom attributes applied to this member and identified by <see cref="T:System.Type"/>.
        /// </summary>
        /// <param name="attributeType">The type of attribute to search for. Only attributes that are assignable to this type are returned. </param>
        /// <returns>An array that contains all the custom attributes applied to this member, or an array with zero elements if no attributes are defined.</returns>
        [H5.Template("({this}.rta || []).filter(function (a) { return H5.is(a, {attributeType}); })")]
        public extern object[] GetReturnTypeCustomAttributes(Type attributeType);

        [H5.Template("H5.Reflection.midel({this})")]
        public extern Delegate CreateDelegate(Type delegateType);

        [H5.Template("H5.Reflection.midel({this}, {target})")]
        public extern Delegate CreateDelegate(Type delegateType, object target);

        [H5.Template("H5.Reflection.midel({this})")]
        public extern Delegate CreateDelegate();

        [H5.Template("H5.Reflection.midel({this}, {target})")]
        public extern Delegate CreateDelegate(object target);

        [H5.Template("H5.Reflection.midel({this}, null, {typeArguments})")]
        public extern Delegate CreateDelegate(Type[] typeArguments);

        [H5.Template("H5.Reflection.midel({this}, {target}, {typeArguments})")]
        public extern Delegate CreateDelegate(object target, Type[] typeArguments);

        public extern int TypeParameterCount
        {
            [H5.Template("({this}.tpc || 0)")]
            get;
            [H5.Template("X")]
            private set;
        }

        public extern bool IsGenericMethodDefinition
        {
            [H5.Template("H5.Reflection.isGenericMethodDefinition({this})")]
            get;
            [H5.Template("X")]
            private set;
        }

        public extern bool IsGenericMethod
        {
            [H5.Template("H5.Reflection.isGenericMethod({this})")]
            get;
            [H5.Template("X")]
            private set;
        }

        [H5.Template("H5.Reflection.midel({this}, {obj})({*arguments})", "H5.Reflection.midel({this}, {obj}).apply(null, {arguments:array})")]
        public extern object Invoke(object obj, params object[] arguments);

        [H5.Template("H5.Reflection.midel({this}, {obj}, {typeArguments})({*arguments})", "H5.Reflection.midel({this}, {obj}, {typeArguments}).apply(null, {arguments:array})")]
        public extern object Invoke(object obj, Type[] typeArguments, params object[] arguments);

        /// <summary>
        /// Script name of the method. Null if the method has a special implementation.
        /// </summary>
        [H5.Name("sn")]
        public extern string ScriptName
        {
            get;
            private set;
        }

        /// <summary>
        /// For methods with a special implementation (eg. [H5.Template]), contains a delegate that represents the method. Null for normal methods.
        /// </summary>
        [H5.Name("def")]
        public extern Delegate SpecialImplementation
        {
            get;
            private set;
        }

        /// <summary>
        /// Whether the [ExpandParams] attribute was specified on the method.
        /// </summary>
        public extern bool IsExpandParams
        {
            [H5.Template("{this}.exp || false")]
            get;

            [H5.Template("{this}.exp = {value}")]
            private set;
        }

        /// <summary>
        /// Returns an array of Type objects that represent the type arguments of a generic method or the type parameters of a generic method definition.
        /// </summary>
        /// <returns>An array of Type objects that represent the type arguments of a generic method or the type parameters of a generic method definition. Returns an empty array if the current method is not a generic method.</returns>
        [H5.Template("H5.Reflection.getMethodGenericArguments({this})")]
        public extern Type[] GetGenericArguments();

        [H5.Template("H5.Reflection.makeGenericMethod({this}, {typeArguments:array})")]
        public extern MethodInfo MakeGenericMethod(params Type[] typeArguments);

        [H5.Template("H5.Reflection.getGenericMethodDefinition({this})")]
        public extern System.Reflection.MethodInfo GetGenericMethodDefinition();

        internal extern MethodInfo();
    }
}