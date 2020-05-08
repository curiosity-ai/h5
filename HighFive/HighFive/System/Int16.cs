namespace System
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Constructor("Number")]
    [HighFive.Reflectable]
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public struct Int16 : IComparable, IComparable<Int16>, IEquatable<Int16>, IFormattable
    {
        private extern Int16(int i);

        [HighFive.InlineConst]
        public const short MinValue = -32768;

        [HighFive.InlineConst]
        public const short MaxValue = 32767;

        [HighFive.Template("System.Int16.parse({s})")]
        public static extern short Parse(string s);

        [HighFive.Template("System.Int16.parse({s}, {radix})")]
        public static extern short Parse(string s, int radix);

        [HighFive.Template("System.Int16.tryParse({s}, {result})")]
        public static extern bool TryParse(string s, out short result);

        [HighFive.Template("System.Int16.tryParse({s}, {result}, {radix})")]
        public static extern bool TryParse(string s, out short result, int radix);

        public extern string ToString(int radix);

        [HighFive.Template("System.Int16.format({this}, {format})")]
        public extern string Format(string format);

        [HighFive.Template("System.Int16.format({this}, {format}, {provider})")]
        public extern string Format(string format, IFormatProvider provider);

        [HighFive.Template("System.Int16.format({this}, {format})")]
        public extern string ToString(string format);

        [HighFive.Template("System.Int16.format({this}, {format}, {provider})")]
        public extern string ToString(string format, IFormatProvider provider);

        [HighFive.Template("HighFive.compare({this}, {other})")]
        public extern int CompareTo(short other);

        [HighFive.Template("HighFive.compare({this}, {obj})")]
        public extern int CompareTo(object obj);

        [HighFive.Template("{this} === {other}")]
        public extern bool Equals(short other);

        [HighFive.Template("System.Int16.equals({this}, {other})")]
        public override extern bool Equals(object other);
    }
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
}