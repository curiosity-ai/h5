namespace System
{
    /// <summary>
    /// Delimits a section of a one-dimensional array.
    /// </summary>
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.IgnoreGeneric]
    [H5.Name("System.ArraySegment")]
    [H5.Reflectable]
    public struct ArraySegment<T>
    {
        public extern ArraySegment(T[] array);

        public extern ArraySegment(T[] array, int offset, int count);

        /// <summary>
        /// Gets the original array containing the range of elements that the array segment delimits.
        /// </summary>
        public extern T[] Array
        {
            [H5.Template("getArray()")]
            get;
        }

        /// <summary>
        /// Gets the number of elements in the range delimited by the array segment.
        /// </summary>
        public extern int Count
        {
            [H5.Template("getCount()")]
            get;
        }

        /// <summary>
        /// Gets the position of the first element in the range delimited by the array segment,
        /// relative to the start of the original array.
        /// </summary>
        public extern int Offset
        {
            [H5.Template("getOffset()")]
            get;
        }
    }
}