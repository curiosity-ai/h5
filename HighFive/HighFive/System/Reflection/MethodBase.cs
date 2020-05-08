using System.ComponentModel;

namespace System.Reflection
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Name("System.Object")]
    public class MethodBase : MemberInfo
    {
        public extern Type[] ParameterTypes
        {
            [HighFive.Template("({this}.p || [])")]
            get;
        }

        public extern bool IsConstructor
        {
            [HighFive.Template("({this}.t === 1)")]
            get;
        }

        [HighFive.NonScriptable, EditorBrowsable(EditorBrowsableState.Never)]
        public static extern MethodBase GetMethodFromHandle(RuntimeMethodHandle h);

        [HighFive.NonScriptable, EditorBrowsable(EditorBrowsableState.Never)]
        public static extern MethodBase GetMethodFromHandle(RuntimeMethodHandle h, RuntimeTypeHandle x);

        [HighFive.Template("({this}.pi || [])")]
        public extern ParameterInfo[] GetParameters();

        internal extern MethodBase();
    }
}