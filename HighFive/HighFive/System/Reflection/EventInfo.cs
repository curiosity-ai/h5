namespace System.Reflection
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    public abstract class EventInfo : MemberInfo
    {
        [H5.Name("ad")]
        public extern MethodInfo AddMethod
        {
            get;
            private set;
        }

        [H5.Name("r")]
        public extern MethodInfo RemoveMethod
        {
            get;
            private set;
        }

        [H5.Template("H5.Reflection.midel({this}.ad, {target})({handler})")]
        public extern void AddEventHandler(object target, Delegate handler);

        [H5.Template("H5.Reflection.midel({this}.r, {target})({handler})")]
        public extern void RemoveEventHandler(object target, Delegate handler);

        internal extern EventInfo();
    }
}