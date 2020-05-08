namespace System.Reflection
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Unbox(true)]
    public abstract partial class ConstructorInfo : MethodBase
    {
        [H5.Template("H5.Reflection.invokeCI({this}, {arguments:array})")]
        public extern object Invoke(params object[] arguments);

        /// <summary>
        /// Script name of the constructor. Null for the unnamed constructor and for constructors with special implementations
        /// </summary>
        [H5.Name("sn")]
        public extern string ScriptName
        {
            get;
            private set;
        }

        /// <summary>
        /// True if the constructor is a normal method that returns the created instance and should be invoked without the 'new' operator
        /// </summary>
        public extern bool IsStaticMethod
        {
            [H5.Template("({this}.sm || false)")]
            get;
            [H5.Template("{this}.sm = {value}")]
            private set;
        }

        /// <summary>
        /// For constructors with a special implementation (eg. [H5.Template]), contains a delegate that can be invoked to create an instance.
        /// </summary>
        [H5.Name("def")]
        public extern Delegate SpecialImplementation
        {
            get;
            private set;
        }

        /// <summary>
        /// Whether the [ExpandParams] attribute was specified on the constructor.
        /// </summary>
        public extern bool IsExpandParams {[H5.Template("{this}.exp || false")] get;[H5.Template("{this}.exp = {value}")] private set; }

        internal extern ConstructorInfo();
    }
}