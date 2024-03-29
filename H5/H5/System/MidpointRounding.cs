namespace System
{
    /// <summary>
    /// Specifies how mathematical rounding methods should process a number that is midway between two numbers.
    /// </summary>
    [H5.External]
    [H5.Enum(H5.Emit.Value)]
    [H5.Name("Number")]
    public enum MidpointRounding
    {
        /// <summary>
        /// Rounds away from zero
        /// </summary>
        Up = 0,

        /// <summary>
        /// Rounds towards zero
        /// </summary>
        Down = 1,

        /// <summary>
        /// Rounds towards Infinity
        /// </summary>
        InfinityPos = 2,

        /// <summary>
        /// Rounds towards -Infinity
        /// </summary>
        InfinityNeg = 3,

        /// <summary>
        /// Rounds towards nearest neighbour. If equidistant, rounds away from zero
        /// </summary>
        AwayFromZero = 4,

        /// <summary>
        /// Rounds towards nearest neighbour. If equidistant, rounds towards zero
        /// </summary>
        TowardsZero = 5,

        /// <summary>
        /// Rounds towards nearest neighbour. If equidistant, rounds towards even neighbour
        /// </summary>
        ToEven = 6,

        /// <summary>
        /// Rounds towards nearest neighbour. If equidistant, rounds towards Infinity
        /// </summary>
        Ceil = 7,

        /// <summary>
        /// Rounds towards nearest neighbour. If equidistant, rounds towards -Infinity
        /// </summary>
        Floor = 8
    }
}