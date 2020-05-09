using System.ComponentModel;

namespace System.Runtime.CompilerServices
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    public static class RuntimeHelpers
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static extern void InitializeArray(Array array, RuntimeFieldHandle handle);

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static extern int OffsetToStringData
        {
            get;
        }

        [H5.Template("H5.getHashCode({obj})")]
        public static extern int GetHashCode(object obj);

        [H5.Template("{type}.$staticInit && {type}.$staticInit()")]
        public static extern void RunClassConstructor(Type type);
    }
}