using System.Collections.Generic;

namespace System.Runtime.CompilerServices
{
    public static class RuntimeHelpers
    {
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
