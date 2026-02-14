using System.Runtime.CompilerServices;

namespace System
{
    public readonly ref struct Span<T>
    {
        internal readonly T[] _array;
        internal readonly int _offset;
        internal readonly int _length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span(T[] array)
        {
            _array = array;
            _offset = 0;
            _length = array != null ? array.Length : 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span(T[] array, int start, int length)
        {
             _array = array;
             _offset = start;
             _length = length;
        }

        public int Length => _length;

        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if ((uint)index >= (uint)_length)
                     throw new IndexOutOfRangeException();
                return ref _array[_offset + index];
            }
        }

        public static implicit operator Span<T>(T[] array) => new Span<T>(array);

        public T[] ToArray()
        {
            if (_length == 0) return Array.Empty<T>();
            var destination = new T[_length];
            Array.Copy(_array, _offset, destination, 0, _length);
            return destination;
        }
    }
}
