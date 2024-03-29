namespace System
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Constructor("Number")]
    [H5.Reflectable]
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public struct Int16 : IComparable, IComparable<short>, IEquatable<short>, IFormattable
    {
        private extern Int16(int i);

        [H5.InlineConst]
        public const short MinValue = -32768;

        [H5.InlineConst]
        public const short MaxValue = 32767;

        [H5.Template("System.Int16.parse({s})")]
        public static extern short Parse(string s);

        [H5.Template("System.Int16.parse({s}, {radix})")]
        public static extern short Parse(string s, int radix);

        [H5.Template("System.Int16.tryParse({s}, {result})")]
        public static extern bool TryParse(string s, out short result);

        [H5.Template("System.Int16.tryParse({s}, {result}, {radix})")]
        public static extern bool TryParse(string s, out short result, int radix);

        public extern string ToString(int radix);

        [H5.Template("System.Int16.format({this}, {format})")]
        public extern string Format(string format);

        [H5.Template("System.Int16.format({this}, {format}, {provider})")]
        public extern string Format(string format, IFormatProvider provider);

        [H5.Template("System.Int16.format({this}, {format})")]
        public extern string ToString(string format);

        [H5.Template("System.Int16.format({this}, {format}, {provider})")]
        public extern string ToString(string format, IFormatProvider provider);

        [H5.Template("H5.compare({this}, {other})")]
        public extern int CompareTo(short other);

        [H5.Template("H5.compare({this}, {obj})")]
        public extern int CompareTo(object obj);

        [H5.Template("{this} === {other}")]
        public extern bool Equals(short other);

        [H5.Template("System.Int16.equals({this}, {other})")]
        public override extern bool Equals(object other);
    }
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
}