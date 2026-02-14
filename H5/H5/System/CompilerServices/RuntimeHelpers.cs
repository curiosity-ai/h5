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

        public static T[] GetSubArray<T>(T[] array, Range range)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            (int offset, int length) = range.GetOffsetAndLength(array.Length);

            if (length == 0)
            {
                return Array.Empty<T>();
            }

            var dest = new T[length];
            Array.Copy(array, offset, dest, 0, length);
            return dest;
        }
    }
}