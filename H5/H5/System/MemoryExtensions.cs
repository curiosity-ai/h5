using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace System
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class MemoryExtensions
    {
        public static ReadOnlySpan<char> AsSpan(this string text)
        {
            if (text == null) return default;
            return new ReadOnlySpan<char>(text.ToCharArray());
        }

        public static ReadOnlySpan<char> AsSpan(this string text, int start)
        {
            if (text == null)
            {
                if (start != 0) throw new ArgumentOutOfRangeException();
                return default;
            }

            return AsSpan(text, start, text.Length - start);
        }

        public static ReadOnlySpan<char> AsSpan(this string text, int start, int length)
        {
             if (text == null)
             {
                 if (start != 0 || length != 0) throw new ArgumentOutOfRangeException();
                 return default;
             }

             return new ReadOnlySpan<char>(text.ToCharArray(), start, length);
        }

        public static bool SequenceEqual<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> other) where T : IEquatable<T>
        {
            if (span.Length != other.Length) return false;
            for (int i = 0; i < span.Length; i++)
            {
                if (!EqualityComparer<T>.Default.Equals(span[i], other[i])) return false;
            }
            return true;
        }
    }
}
