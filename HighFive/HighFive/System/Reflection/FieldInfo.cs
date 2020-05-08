using System.ComponentModel;

namespace System.Reflection
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Unbox(true)]
    public abstract partial class FieldInfo : MemberInfo
    {
        [HighFive.Name("rt")]
        public extern Type FieldType
        {
            get;
            private set;
        }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern bool IsInitOnly
        {
            [HighFive.Template("({this}.ro || false)")]
            get;
        }

        [HighFive.Template("HighFive.Reflection.fieldAccess({this}, {obj})")]
        public extern object GetValue(object obj);

        [HighFive.Template("HighFive.Reflection.fieldAccess({this}, {obj}, {value})")]
        public extern void SetValue(object obj, object value);

        /// <summary>
        /// Script name of the field
        /// </summary>
        [HighFive.Name("sn")]
        public extern string ScriptName
        {
            get;
            private set;
        }

        [HighFive.NonScriptable, EditorBrowsable(EditorBrowsableState.Never)]
        public static extern FieldInfo GetFieldFromHandle(RuntimeFieldHandle h);

        [HighFive.NonScriptable, EditorBrowsable(EditorBrowsableState.Never)]
        public static extern FieldInfo GetFieldFromHandle(RuntimeFieldHandle h, RuntimeTypeHandle x);

        internal extern FieldInfo();
    }
}