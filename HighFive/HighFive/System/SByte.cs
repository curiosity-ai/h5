namespace System
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Reflectable]
    [HighFive.Constructor("Number")]
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public struct SByte : IComparable, IComparable<SByte>, IEquatable<SByte>, IFormattable
    {
        private extern SByte(int i);

        [HighFive.InlineConst]
        [CLSCompliant(false)]
        public const sbyte MinValue = -128;

        [HighFive.InlineConst]
        [CLSCompliant(false)]
        public const sbyte MaxValue = 127;

        [HighFive.Template("System.SByte.parse({s})")]
        [CLSCompliant(false)]
        public static extern sbyte Parse(string s);

        [HighFive.Template("System.SByte.parse({s}, {radix})")]
        [CLSCompliant(false)]
        public static extern sbyte Parse(string s, int radix);

        [HighFive.Template("System.SByte.tryParse({s}, {result})")]
        [CLSCompliant(false)]
        public static extern bool TryParse(string s, out sbyte result);

        [HighFive.Template("System.SByte.tryParse({s}, {result}, {radix})")]
        [CLSCompliant(false)]
        public static extern bool TryParse(string s, out sbyte result, int radix);

        public extern string ToString(int radix);

        [HighFive.Template("System.SByte.format({this}, {format})")]
        public extern string Format(string format);

        [HighFive.Template("System.SByte.format({this}, {format}, {provider})")]
        public extern string Format(string format, IFormatProvider provider);

        [HighFive.Template("System.SByte.format({this}, {format})")]
        public extern string ToString(string format);

        [HighFive.Template("System.SByte.format({this}, {format}, {provider})")]
        public extern string ToString(string format, IFormatProvider provider);

        [HighFive.Template("HighFive.compare({this}, {other})")]
        [CLSCompliant(false)]
        public extern int CompareTo(sbyte other);

        [HighFive.Template("HighFive.compare({this}, {obj})")]
        public extern int CompareTo(object obj);

        [HighFive.Template("{this} === {other}")]
        [CLSCompliant(false)]
        public extern bool Equals(sbyte other);

        [HighFive.Template("System.SByte.equals({this}, {other})")]
        public override extern bool Equals(object other);
    }
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
}