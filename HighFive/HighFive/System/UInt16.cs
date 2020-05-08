namespace System
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Reflectable]
    [HighFive.Constructor("Number")]
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public struct UInt16 : IComparable, IComparable<UInt16>, IEquatable<UInt16>, IFormattable
    {
        private extern UInt16(int i);

        [HighFive.InlineConst]
        [CLSCompliant(false)]
        public const ushort MinValue = 0;

        [HighFive.InlineConst]
        [CLSCompliant(false)]
        public const ushort MaxValue = 65535;

        [HighFive.Template("System.UInt16.parse({s})")]
        [CLSCompliant(false)]
        public static extern ushort Parse(string s);

        [HighFive.Template("System.UInt16.parse({s}, {radix})")]
        [CLSCompliant(false)]
        public static extern ushort Parse(string s, int radix);

        [HighFive.Template("System.UInt16.tryParse({s}, {result})")]
        [CLSCompliant(false)]
        public static extern bool TryParse(string s, out ushort result);

        [HighFive.Template("System.UInt16.tryParse({s}, {result}, {radix})")]
        [CLSCompliant(false)]
        public static extern bool TryParse(string s, out ushort result, int radix);

        public extern string ToString(int radix);

        [HighFive.Template("System.UInt16.format({this}, {format})")]
        public extern string Format(string format);

        [HighFive.Template("System.UInt16.format({this}, {format}, {provider})")]
        public extern string Format(string format, IFormatProvider provider);

        [HighFive.Template("System.UInt16.format({this}, {format})")]
        public extern string ToString(string format);

        [HighFive.Template("System.UInt16.format({this}, {format}, {provider})")]
        public extern string ToString(string format, IFormatProvider provider);

        [HighFive.Template("HighFive.compare({this}, {other})")]
        [CLSCompliant(false)]
        public extern int CompareTo(ushort other);

        [HighFive.Template("HighFive.compare({this}, {obj})")]
        public extern int CompareTo(object obj);

        [HighFive.Template("{this} === {other}")]
        [CLSCompliant(false)]
        public extern bool Equals(ushort other);

        [HighFive.Template("System.UInt16.equals({this}, {other})")]
        public override extern bool Equals(object other);
    }
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
}