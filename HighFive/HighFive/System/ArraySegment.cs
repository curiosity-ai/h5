namespace System
{
    /// <summary>
    /// Delimits a section of a one-dimensional array.
    /// </summary>
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.IgnoreGeneric]
    [HighFive.Name("System.ArraySegment")]
    [HighFive.Reflectable]
    public struct ArraySegment<T>
    {
        public extern ArraySegment(T[] array);

        public extern ArraySegment(T[] array, int offset, int count);

        /// <summary>
        /// Gets the original array containing the range of elements that the array segment delimits.
        /// </summary>
        public extern T[] Array
        {
            [HighFive.Template("getArray()")]
            get;
        }

        /// <summary>
        /// Gets the number of elements in the range delimited by the array segment.
        /// </summary>
        public extern int Count
        {
            [HighFive.Template("getCount()")]
            get;
        }

        /// <summary>
        /// Gets the position of the first element in the range delimited by the array segment,
        /// relative to the start of the original array.
        /// </summary>
        public extern int Offset
        {
            [HighFive.Template("getOffset()")]
            get;
        }
    }
}