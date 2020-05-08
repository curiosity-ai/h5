namespace System
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Constructor("Number")]
    [HighFive.Reflectable]
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public struct Byte : IComparable, IComparable<Byte>, IEquatable<Byte>, IFormattable
    {
        private extern Byte(int i);

        [HighFive.InlineConst]
        public const byte MinValue = 0;

        [HighFive.InlineConst]
        public const byte MaxValue = 255;

        [HighFive.Template("System.Byte.parse({s})")]
        public static extern byte Parse(string s);

        [HighFive.Template("System.Byte.parse({s}, {radix})")]
        public static extern byte Parse(string s, int radix);

        [HighFive.Template("System.Byte.tryParse({s}, {result})")]
        public static extern bool TryParse(string s, out byte result);

        [HighFive.Template("System.Byte.tryParse({s}, {result}, {radix})")]
        public static extern bool TryParse(string s, out byte result, int radix);

        public extern string ToString(int radix);

        [HighFive.Template("System.Byte.format({this}, {format})")]
        public extern string Format(string format);

        [HighFive.Template("System.Byte.format({this}, {format}, {provider})")]
        public extern string Format(string format, IFormatProvider provider);

        [HighFive.Template("System.Byte.format({this}, {format})")]
        public extern string ToString(string format);

        [HighFive.Template("System.Byte.format({this}, {format}, {provider})")]
        public extern string ToString(string format, IFormatProvider provider);

        [HighFive.Template("HighFive.compare({this}, {other})")]
        public extern int CompareTo(byte other);

        [HighFive.Template("HighFive.compare({this}, {obj})")]
        public extern int CompareTo(object obj);

        [HighFive.Template("{this} === {other}")]
        public extern bool Equals(byte other);

        [HighFive.Template("System.Byte.equals({this}, {other})")]
        public override extern bool Equals(object other);
    }
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
}