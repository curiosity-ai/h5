namespace System
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Reflectable]
    public struct Int64 : IComparable, IComparable<long>, IEquatable<long>, IFormattable
    {
        private extern Int64(int i);

        [H5.Convention]
        public const long MinValue = -9223372036854775808;

        [H5.Convention]
        public const long MaxValue = 9223372036854775807;

        [H5.Template("System.Int64.parse({s})")]
        public static extern long Parse(string s);

        [H5.Template("System.Int64.tryParse({s}, {result})")]
        public static extern bool TryParse(string s, out long result);

        public extern string ToString(int radix);

        public extern string Format(string format);

        public extern string Format(string format, IFormatProvider provider);

        public extern string ToString(string format);

        public extern string ToString(string format, IFormatProvider provider);

        public extern int CompareTo(long other);

        public extern int CompareTo(object obj);

        public extern bool Equals(long other);

        //[H5.Template("System.Int64.lift({value})")]
        public static extern implicit operator long (byte value);

        //[H5.Template("System.Int64.lift({value})")]
        
        public static extern implicit operator long (sbyte value);

        //[H5.Template("System.Int64.lift({value})")]
        public static extern implicit operator long (short value);

        //[H5.Template("System.Int64.lift({value})")]
        
        public static extern implicit operator long (ushort value);

        //[H5.Template("System.Int64.lift({value})")]
        public static extern implicit operator long (char value);

        //[H5.Template("System.Int64.lift({value})")]
        public static extern implicit operator long (int value);

        //[H5.Template("System.Int64.lift({value})")]
        
        public static extern implicit operator long (uint value);

        //[H5.Template("System.Int64.lift(H5.Int.clip64({value}))")]
        public static extern explicit operator long (float value);

        //[H5.Template("System.Int64.lift(H5.Int.clip64({value}))")]
        public static extern explicit operator long (double value);

        //[H5.Template("System.Int64.lift({value})")]
        
        public static extern explicit operator long (ulong value);

        //[H5.Template("System.Int64.clip8({value})")]
        public static extern explicit operator byte (long value);

        //[H5.Template("System.Int64.clipu8({value})")]
        
        public static extern explicit operator sbyte (long value);

        //[H5.Template("System.Int64.clipu16({value})")]
        public static extern explicit operator char (long value);

        //[H5.Template("System.Int64.clip16({value})")]
        public static extern explicit operator short (long value);

        //[H5.Template("System.Int64.clipu16({value})")]
        
        public static extern explicit operator ushort (long value);

        //[H5.Template("System.Int64.clip32({value})")]
        public static extern explicit operator int (long value);

        //[H5.Template("System.Int64.clipu32({value})")]
        
        public static extern explicit operator uint (long value);

        //[H5.Template("System.UInt64.lift({value})")]
        
        public static extern explicit operator ulong (long value);

        //[H5.Template("System.Int64.toNumber({value})")]
        public static extern explicit operator float (long value);

        //[H5.Template("System.Int64.toNumber({value})")]
        public static extern explicit operator double (long value);
    }
}