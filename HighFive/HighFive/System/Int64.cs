namespace System
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Reflectable]
    public struct Int64 : IComparable, IComparable<Int64>, IEquatable<Int64>, IFormattable
    {
        private extern Int64(int i);

        [HighFive.Convention]
        public const long MinValue = -9223372036854775808;

        [HighFive.Convention]
        public const long MaxValue = 9223372036854775807;

        [HighFive.Template("System.Int64.parse({s})")]
        public static extern long Parse(string s);

        [HighFive.Template("System.Int64.tryParse({s}, {result})")]
        public static extern bool TryParse(string s, out long result);

        public extern string ToString(int radix);

        public extern string Format(string format);

        public extern string Format(string format, IFormatProvider provider);

        public extern string ToString(string format);

        public extern string ToString(string format, IFormatProvider provider);

        public extern int CompareTo(long other);

        public extern int CompareTo(object obj);

        public extern bool Equals(long other);

        //[HighFive.Template("System.Int64.lift({value})")]
        public static extern implicit operator long (byte value);

        //[HighFive.Template("System.Int64.lift({value})")]
        [CLSCompliant(false)]
        public static extern implicit operator long (sbyte value);

        //[HighFive.Template("System.Int64.lift({value})")]
        public static extern implicit operator long (short value);

        //[HighFive.Template("System.Int64.lift({value})")]
        [CLSCompliant(false)]
        public static extern implicit operator long (ushort value);

        //[HighFive.Template("System.Int64.lift({value})")]
        public static extern implicit operator long (char value);

        //[HighFive.Template("System.Int64.lift({value})")]
        public static extern implicit operator long (int value);

        //[HighFive.Template("System.Int64.lift({value})")]
        [CLSCompliant(false)]
        public static extern implicit operator long (uint value);

        //[HighFive.Template("System.Int64.lift(HighFive.Int.clip64({value}))")]
        public static extern explicit operator long (float value);

        //[HighFive.Template("System.Int64.lift(HighFive.Int.clip64({value}))")]
        public static extern explicit operator long (double value);

        //[HighFive.Template("System.Int64.lift({value})")]
        [CLSCompliant(false)]
        public static extern explicit operator long (ulong value);

        //[HighFive.Template("System.Int64.clip8({value})")]
        public static extern explicit operator byte (long value);

        //[HighFive.Template("System.Int64.clipu8({value})")]
        [CLSCompliant(false)]
        public static extern explicit operator sbyte (long value);

        //[HighFive.Template("System.Int64.clipu16({value})")]
        public static extern explicit operator char (long value);

        //[HighFive.Template("System.Int64.clip16({value})")]
        public static extern explicit operator short (long value);

        //[HighFive.Template("System.Int64.clipu16({value})")]
        [CLSCompliant(false)]
        public static extern explicit operator ushort (long value);

        //[HighFive.Template("System.Int64.clip32({value})")]
        public static extern explicit operator int (long value);

        //[HighFive.Template("System.Int64.clipu32({value})")]
        [CLSCompliant(false)]
        public static extern explicit operator uint (long value);

        //[HighFive.Template("System.UInt64.lift({value})")]
        [CLSCompliant(false)]
        public static extern explicit operator ulong (long value);

        //[HighFive.Template("System.Int64.toNumber({value})")]
        public static extern explicit operator float (long value);

        //[HighFive.Template("System.Int64.toNumber({value})")]
        public static extern explicit operator double (long value);
    }
}