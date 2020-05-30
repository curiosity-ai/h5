namespace System
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Constructor("Number")]
    [H5.Reflectable]
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public struct Int32 : IComparable, IComparable<int>, IEquatable<int>, IFormattable
    {
        private extern Int32(int i);

        [H5.InlineConst]
        public const int MinValue = -2147483648;

        [H5.InlineConst]
        public const int MaxValue = 2147483647;

        [H5.Template("System.Int32.parse({s})")]
        public static extern int Parse(string s);

        [H5.Template("System.Int32.parse({s}, {radix})")]
        public static extern int Parse(string s, int radix);

        [H5.Template("System.Int32.tryParse({s}, {result})")]
        public static extern bool TryParse(string s, out int result);

        [H5.Template("System.Int32.tryParse({s}, {result}, {radix})")]
        public static extern bool TryParse(string s, out int result, int radix);

        public extern string ToString(int radix);

        [H5.Template("System.Int32.format({this}, {format})")]
        public extern string Format(string format);

        [H5.Template("System.Int32.format({this}, {format}, {provider})")]
        public extern string Format(string format, IFormatProvider provider);

        [H5.Template("System.Int32.format({this}, {format})")]
        public extern string ToString(string format);

        [H5.Template("System.Int32.format({this}, {format}, {provider})")]
        public extern string ToString(string format, IFormatProvider provider);

        [H5.Template("H5.compare({this}, {other})")]
        public extern int CompareTo(int other);

        [H5.Template("H5.compare({this}, {obj})")]
        public extern int CompareTo(object obj);

        [H5.Template("{this} === {other}")]
        public extern bool Equals(int other);

        [H5.Template("System.Int32.equals({this}, {other})")]
        public override extern bool Equals(object other);
    }
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
}