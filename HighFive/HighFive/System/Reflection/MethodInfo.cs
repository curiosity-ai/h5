namespace System.Reflection
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Unbox(true)]
    public class MethodInfo : MethodBase
    {
        [HighFive.Name("rt")]
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
        [HighFive.Template("({this}.rta || [])")]
        public extern object[] GetReturnTypeCustomAttributes(bool inherit);

        /// <summary>
        /// Returns an array of custom attributes applied to this member and identified by <see cref="T:System.Type"/>.
        /// </summary>
        /// <param name="attributeType">The type of attribute to search for. Only attributes that are assignable to this type are returned. </param>
        /// <param name="inherit">Ignored for members. Base members will never be considered.</param>
        /// <returns>An array that contains all the custom attributes applied to this member, or an array with zero elements if no attributes are defined.</returns>
        [HighFive.Template("({this}.rta || []).filter(function (a) { return HighFive.is(a, {attributeType}); })")]
        public extern object[] GetReturnTypeCustomAttributes(Type attributeType, bool inherit);

        /// <summary>
        /// Returns an array of all custom attributes applied to this member.
        /// </summary>
        /// <returns>An array that contains all the custom attributes applied to this member, or an array with zero elements if no attributes are defined. </returns>
        [HighFive.Template("({this}.rta || [])")]
        public extern object[] GetReturnTypeCustomAttributes();

        /// <summary>
        /// Returns an array of custom attributes applied to this member and identified by <see cref="T:System.Type"/>.
        /// </summary>
        /// <param name="attributeType">The type of attribute to search for. Only attributes that are assignable to this type are returned. </param>
        /// <returns>An array that contains all the custom attributes applied to this member, or an array with zero elements if no attributes are defined.</returns>
        [HighFive.Template("({this}.rta || []).filter(function (a) { return HighFive.is(a, {attributeType}); })")]
        public extern object[] GetReturnTypeCustomAttributes(Type attributeType);

        [HighFive.Template("HighFive.Reflection.midel({this})")]
        public extern Delegate CreateDelegate(Type delegateType);

        [HighFive.Template("HighFive.Reflection.midel({this}, {target})")]
        public extern Delegate CreateDelegate(Type delegateType, object target);

        [HighFive.Template("HighFive.Reflection.midel({this})")]
        public extern Delegate CreateDelegate();

        [HighFive.Template("HighFive.Reflection.midel({this}, {target})")]
        public extern Delegate CreateDelegate(object target);

        [HighFive.Template("HighFive.Reflection.midel({this}, null, {typeArguments})")]
        public extern Delegate CreateDelegate(Type[] typeArguments);

        [HighFive.Template("HighFive.Reflection.midel({this}, {target}, {typeArguments})")]
        public extern Delegate CreateDelegate(object target, Type[] typeArguments);

        public extern int TypeParameterCount
        {
            [HighFive.Template("({this}.tpc || 0)")]
            get;
            [HighFive.Template("X")]
            private set;
        }

        public extern bool IsGenericMethodDefinition
        {
            [HighFive.Template("HighFive.Reflection.isGenericMethodDefinition({this})")]
            get;
            [HighFive.Template("X")]
            private set;
        }

        public extern bool IsGenericMethod
        {
            [HighFive.Template("HighFive.Reflection.isGenericMethod({this})")]
            get;
            [HighFive.Template("X")]
            private set;
        }

        [HighFive.Template("HighFive.Reflection.midel({this}, {obj})({*arguments})", "HighFive.Reflection.midel({this}, {obj}).apply(null, {arguments:array})")]
        public extern object Invoke(object obj, params object[] arguments);

        [HighFive.Template("HighFive.Reflection.midel({this}, {obj}, {typeArguments})({*arguments})", "HighFive.Reflection.midel({this}, {obj}, {typeArguments}).apply(null, {arguments:array})")]
        public extern object Invoke(object obj, Type[] typeArguments, params object[] arguments);

        /// <summary>
        /// Script name of the method. Null if the method has a special implementation.
        /// </summary>
        [HighFive.Name("sn")]
        public extern string ScriptName
        {
            get;
            private set;
        }

        /// <summary>
        /// For methods with a special implementation (eg. [HighFive.Template]), contains a delegate that represents the method. Null for normal methods.
        /// </summary>
        [HighFive.Name("def")]
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
            [HighFive.Template("{this}.exp || false")]
            get;

            [HighFive.Template("{this}.exp = {value}")]
            private set;
        }

        /// <summary>
        /// Returns an array of Type objects that represent the type arguments of a generic method or the type parameters of a generic method definition.
        /// </summary>
        /// <returns>An array of Type objects that represent the type arguments of a generic method or the type parameters of a generic method definition. Returns an empty array if the current method is not a generic method.</returns>
        [HighFive.Template("HighFive.Reflection.getMethodGenericArguments({this})")]
        public extern Type[] GetGenericArguments();

        [HighFive.Template("HighFive.Reflection.makeGenericMethod({this}, {typeArguments:array})")]
        public extern MethodInfo MakeGenericMethod(params Type[] typeArguments);

        [HighFive.Template("HighFive.Reflection.getGenericMethodDefinition({this})")]
        public extern System.Reflection.MethodInfo GetGenericMethodDefinition();

        internal extern MethodInfo();
    }
}