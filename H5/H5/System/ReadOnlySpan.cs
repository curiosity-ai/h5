using System.Runtime.CompilerServices;

namespace System
{
    public readonly ref struct ReadOnlySpan<T>
    {
        internal readonly T[] _array;
        internal readonly int _offset;
        internal readonly int _length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan(T[] array)
        {
            _array = array;
            _offset = 0;
            _length = array != null ? array.Length : 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan(T[] array, int start, int length)
        {
             _array = array;
             _offset = start;
             _length = length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan(Span<T> span)
        {
             _array = span._array;
             _offset = span._offset;
             _length = span._length;
        }

        public int Length => _length;

        public ref readonly T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if ((uint)index >= (uint)_length)
                     throw new IndexOutOfRangeException();
                return ref _array[_offset + index];
            }
        }

        public static implicit operator ReadOnlySpan<T>(T[] array) => new ReadOnlySpan<T>(array);
        public static implicit operator ReadOnlySpan<T>(Span<T> span) => new ReadOnlySpan<T>(span);

        public T[] ToArray()
        {
            if (_length == 0) return Array.Empty<T>();
            var destination = new T[_length];
            Array.Copy(_array, _offset, destination, 0, _length);
            return destination;
        }
    }
}
