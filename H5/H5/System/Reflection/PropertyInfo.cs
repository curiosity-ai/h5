namespace System.Reflection
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Unbox(true)]
    public abstract class PropertyInfo : MemberInfo
    {
        [H5.Name("rt")]
        public extern Type PropertyType
        {
            get;
        }

        public extern Type[] IndexParameterTypes
        {
            [H5.Template("({this}.p || [])")]
            get;
        }

        [H5.Template("({this}.ipi || [])")]
        public extern ParameterInfo[] GetIndexParameters();

        public extern bool CanRead
        {
            [H5.Template("(!!{this}.g)")]
            get;
        }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern bool IsIndexer
        {
            [H5.Template("({this}.i || false)")]
            get;
        }

        public extern bool CanWrite
        {
            [H5.Template("(!!{this}.s)")]
            get;
        }

        [H5.Name("g")]
        public extern MethodInfo GetMethod
        {
            get;
        }

        [H5.Name("s")]
        public extern MethodInfo SetMethod
        {
            get;
        }

        [H5.Template("H5.Reflection.midel({this}.g, {obj})()")]
        public extern object GetValue(object obj);

        [H5.Template("H5.Reflection.midel({this}.g, {obj}).apply(null, {index})")]
        public extern object GetValue(object obj, object[] index);

        [H5.Template("H5.Reflection.midel({this}.s, {obj:nobox})({value:nobox})")]
        public extern void SetValue(object obj, object value);

        [H5.Template("H5.Reflection.midel({this}.s, {obj:nobox}).apply(null, ({index:nobox} || []).concat({value:nobox}))")]
        public extern void SetValue(object obj, object value, object[] index);

        /// <summary>
        /// For properties implemented as fields, contains the name of the field. Null for properties implemented as get and set methods.
        /// </summary>
        [H5.Name("fn")]
        public extern string ScriptFieldName
        {
            get;
        }

        internal extern PropertyInfo();
    }
}