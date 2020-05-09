namespace System
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Constructor("Number")]
    [H5.Reflectable]
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public struct Byte : IComparable, IComparable<Byte>, IEquatable<Byte>, IFormattable
    {
        private extern Byte(int i);

        [H5.InlineConst]
        public const byte MinValue = 0;

        [H5.InlineConst]
        public const byte MaxValue = 255;

        [H5.Template("System.Byte.parse({s})")]
        public static extern byte Parse(string s);

        [H5.Template("System.Byte.parse({s}, {radix})")]
        public static extern byte Parse(string s, int radix);

        [H5.Template("System.Byte.tryParse({s}, {result})")]
        public static extern bool TryParse(string s, out byte result);

        [H5.Template("System.Byte.tryParse({s}, {result}, {radix})")]
        public static extern bool TryParse(string s, out byte result, int radix);

        public extern string ToString(int radix);

        [H5.Template("System.Byte.format({this}, {format})")]
        public extern string Format(string format);

        [H5.Template("System.Byte.format({this}, {format}, {provider})")]
        public extern string Format(string format, IFormatProvider provider);

        [H5.Template("System.Byte.format({this}, {format})")]
        public extern string ToString(string format);

        [H5.Template("System.Byte.format({this}, {format}, {provider})")]
        public extern string ToString(string format, IFormatProvider provider);

        [H5.Template("H5.compare({this}, {other})")]
        public extern int CompareTo(byte other);

        [H5.Template("H5.compare({this}, {obj})")]
        public extern int CompareTo(object obj);

        [H5.Template("{this} === {other}")]
        public extern bool Equals(byte other);

        [H5.Template("System.Byte.equals({this}, {other})")]
        public override extern bool Equals(object other);
    }
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
}