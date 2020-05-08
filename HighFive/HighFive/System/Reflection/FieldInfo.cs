using System.ComponentModel;

namespace System.Reflection
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Unbox(true)]
    public abstract partial class FieldInfo : MemberInfo
    {
        [H5.Name("rt")]
        public extern Type FieldType
        {
            get;
            private set;
        }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern bool IsInitOnly
        {
            [H5.Template("({this}.ro || false)")]
            get;
        }

        [H5.Template("H5.Reflection.fieldAccess({this}, {obj})")]
        public extern object GetValue(object obj);

        [H5.Template("H5.Reflection.fieldAccess({this}, {obj}, {value})")]
        public extern void SetValue(object obj, object value);

        /// <summary>
        /// Script name of the field
        /// </summary>
        [H5.Name("sn")]
        public extern string ScriptName
        {
            get;
            private set;
        }

        [H5.NonScriptable, EditorBrowsable(EditorBrowsableState.Never)]
        public static extern FieldInfo GetFieldFromHandle(RuntimeFieldHandle h);

        [H5.NonScriptable, EditorBrowsable(EditorBrowsableState.Never)]
        public static extern FieldInfo GetFieldFromHandle(RuntimeFieldHandle h, RuntimeTypeHandle x);

        internal extern FieldInfo();
    }
}