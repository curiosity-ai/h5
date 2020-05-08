namespace System
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Constructor("Number")]
    [HighFive.Reflectable]
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public struct Int32 : IComparable, IComparable<Int32>, IEquatable<Int32>, IFormattable
    {
        private extern Int32(int i);

        [HighFive.InlineConst]
        public const int MinValue = -2147483648;

        [HighFive.InlineConst]
        public const int MaxValue = 2147483647;

        [HighFive.Template("System.Int32.parse({s})")]
        public static extern int Parse(string s);

        [HighFive.Template("System.Int32.parse({s}, {radix})")]
        public static extern int Parse(string s, int radix);

        [HighFive.Template("System.Int32.tryParse({s}, {result})")]
        public static extern bool TryParse(string s, out int result);

        [HighFive.Template("System.Int32.tryParse({s}, {result}, {radix})")]
        public static extern bool TryParse(string s, out int result, int radix);

        public extern string ToString(int radix);

        [HighFive.Template("System.Int32.format({this}, {format})")]
        public extern string Format(string format);

        [HighFive.Template("System.Int32.format({this}, {format}, {provider})")]
        public extern string Format(string format, IFormatProvider provider);

        [HighFive.Template("System.Int32.format({this}, {format})")]
        public extern string ToString(string format);

        [HighFive.Template("System.Int32.format({this}, {format}, {provider})")]
        public extern string ToString(string format, IFormatProvider provider);

        [HighFive.Template("HighFive.compare({this}, {other})")]
        public extern int CompareTo(int other);

        [HighFive.Template("HighFive.compare({this}, {obj})")]
        public extern int CompareTo(object obj);

        [HighFive.Template("{this} === {other}")]
        public extern bool Equals(int other);

        [HighFive.Template("System.Int32.equals({this}, {other})")]
        public override extern bool Equals(object other);
    }
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
}