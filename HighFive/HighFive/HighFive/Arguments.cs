namespace HighFive
{
    /// <summary>
    /// Provides access to the arguments of the current function.
    /// </summary>
    [External]
    public static class Arguments
    {
        /// <summary>
        /// Retrieves the number of actual arguments passed to the function.
        /// </summary>
        /// <returns>The count of arguments.</returns>
        public static extern int Length
        {
            [Template("arguments.length")]
            get;
        }

        /// <summary>
        /// Retrieves the specified actual argument value passed to the
        /// function by index.
        /// </summary>
        /// <param name="index">The index of the argument to retrieve.</param>
        /// <returns>The value of the specified argument.</returns>
        [Template("arguments[{index}]")]
        public static extern object GetArgument(int index);

        [Template("Array.prototype.slice.call(arguments)")]
        public static extern object[] ToArray();

        [Template("Array.prototype.slice.call(arguments, {start})")]
        public static extern object[] ToArray(int start);

        [Template("Array.prototype.slice.call(arguments, {start}, {end})")]
        public static extern object[] ToArray(int start, int end);

        [Template("Array.prototype.slice.call(arguments)")]
        public static extern T[] ToArray<T>();

        [Template("Array.prototype.slice.call(arguments, {start})")]
        public static extern T[] ToArray<T>(int start);

        [Template("Array.prototype.slice.call(arguments, {start}, {end})")]
        public static extern T[] ToArray<T>(int start, int end);
    }
}