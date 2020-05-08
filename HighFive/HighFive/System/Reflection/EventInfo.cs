namespace System.Reflection
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    public abstract class EventInfo : MemberInfo
    {
        [HighFive.Name("ad")]
        public extern MethodInfo AddMethod
        {
            get;
            private set;
        }

        [HighFive.Name("r")]
        public extern MethodInfo RemoveMethod
        {
            get;
            private set;
        }

        [HighFive.Template("HighFive.Reflection.midel({this}.ad, {target})({handler})")]
        public extern void AddEventHandler(object target, Delegate handler);

        [HighFive.Template("HighFive.Reflection.midel({this}.r, {target})({handler})")]
        public extern void RemoveEventHandler(object target, Delegate handler);

        internal extern EventInfo();
    }
}