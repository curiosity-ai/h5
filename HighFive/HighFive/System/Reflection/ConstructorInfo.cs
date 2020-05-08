namespace System.Reflection
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Unbox(true)]
    public abstract partial class ConstructorInfo : MethodBase
    {
        [HighFive.Template("HighFive.Reflection.invokeCI({this}, {arguments:array})")]
        public extern object Invoke(params object[] arguments);

        /// <summary>
        /// Script name of the constructor. Null for the unnamed constructor and for constructors with special implementations
        /// </summary>
        [HighFive.Name("sn")]
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
            [HighFive.Template("({this}.sm || false)")]
            get;
            [HighFive.Template("{this}.sm = {value}")]
            private set;
        }

        /// <summary>
        /// For constructors with a special implementation (eg. [HighFive.Template]), contains a delegate that can be invoked to create an instance.
        /// </summary>
        [HighFive.Name("def")]
        public extern Delegate SpecialImplementation
        {
            get;
            private set;
        }

        /// <summary>
        /// Whether the [ExpandParams] attribute was specified on the constructor.
        /// </summary>
        public extern bool IsExpandParams {[HighFive.Template("{this}.exp || false")] get;[HighFive.Template("{this}.exp = {value}")] private set; }

        internal extern ConstructorInfo();
    }
}