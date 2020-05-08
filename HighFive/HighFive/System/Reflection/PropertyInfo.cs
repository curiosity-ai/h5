namespace System.Reflection
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Unbox(true)]
    public abstract class PropertyInfo : MemberInfo
    {
        [HighFive.Name("rt")]
        public extern Type PropertyType
        {
            get;
        }

        public extern Type[] IndexParameterTypes
        {
            [HighFive.Template("({this}.p || [])")]
            get;
        }

        [HighFive.Template("({this}.ipi || [])")]
        public extern ParameterInfo[] GetIndexParameters();

        public extern bool CanRead
        {
            [HighFive.Template("(!!{this}.g)")]
            get;
        }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern bool IsIndexer
        {
            [HighFive.Template("({this}.i || false)")]
            get;
        }

        public extern bool CanWrite
        {
            [HighFive.Template("(!!{this}.s)")]
            get;
        }

        [HighFive.Name("g")]
        public extern MethodInfo GetMethod
        {
            get;
        }

        [HighFive.Name("s")]
        public extern MethodInfo SetMethod
        {
            get;
        }

        [HighFive.Template("HighFive.Reflection.midel({this}.g, {obj})()")]
        public extern object GetValue(object obj);

        [HighFive.Template("HighFive.Reflection.midel({this}.g, {obj}).apply(null, {index})")]
        public extern object GetValue(object obj, object[] index);

        [HighFive.Template("HighFive.Reflection.midel({this}.s, {obj:nobox})({value:nobox})")]
        public extern void SetValue(object obj, object value);

        [HighFive.Template("HighFive.Reflection.midel({this}.s, {obj:nobox}).apply(null, ({index:nobox} || []).concat({value:nobox}))")]
        public extern void SetValue(object obj, object value, object[] index);

        /// <summary>
        /// For properties implemented as fields, contains the name of the field. Null for properties implemented as get and set methods.
        /// </summary>
        [HighFive.Name("fn")]
        public extern string ScriptFieldName
        {
            get;
        }

        internal extern PropertyInfo();
    }
}