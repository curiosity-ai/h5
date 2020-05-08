using System.ComponentModel;

namespace System.Reflection
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Name("System.Object")]
    public class MethodBase : MemberInfo
    {
        public extern Type[] ParameterTypes
        {
            [H5.Template("({this}.p || [])")]
            get;
        }

        public extern bool IsConstructor
        {
            [H5.Template("({this}.t === 1)")]
            get;
        }

        [H5.NonScriptable, EditorBrowsable(EditorBrowsableState.Never)]
        public static extern MethodBase GetMethodFromHandle(RuntimeMethodHandle h);

        [H5.NonScriptable, EditorBrowsable(EditorBrowsableState.Never)]
        public static extern MethodBase GetMethodFromHandle(RuntimeMethodHandle h, RuntimeTypeHandle x);

        [H5.Template("({this}.pi || [])")]
        public extern ParameterInfo[] GetParameters();

        internal extern MethodBase();
    }
}