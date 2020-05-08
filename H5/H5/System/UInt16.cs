namespace System
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Reflectable]
    [H5.Constructor("Number")]
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public struct UInt16 : IComparable, IComparable<UInt16>, IEquatable<UInt16>, IFormattable
    {
        private extern UInt16(int i);

        [H5.InlineConst]
        [CLSCompliant(false)]
        public const ushort MinValue = 0;

        [H5.InlineConst]
        [CLSCompliant(false)]
        public const ushort MaxValue = 65535;

        [H5.Template("System.UInt16.parse({s})")]
        [CLSCompliant(false)]
        public static extern ushort Parse(string s);

        [H5.Template("System.UInt16.parse({s}, {radix})")]
        [CLSCompliant(false)]
        public static extern ushort Parse(string s, int radix);

        [H5.Template("System.UInt16.tryParse({s}, {result})")]
        [CLSCompliant(false)]
        public static extern bool TryParse(string s, out ushort result);

        [H5.Template("System.UInt16.tryParse({s}, {result}, {radix})")]
        [CLSCompliant(false)]
        public static extern bool TryParse(string s, out ushort result, int radix);

        public extern string ToString(int radix);

        [H5.Template("System.UInt16.format({this}, {format})")]
        public extern string Format(string format);

        [H5.Template("System.UInt16.format({this}, {format}, {provider})")]
        public extern string Format(string format, IFormatProvider provider);

        [H5.Template("System.UInt16.format({this}, {format})")]
        public extern string ToString(string format);

        [H5.Template("System.UInt16.format({this}, {format}, {provider})")]
        public extern string ToString(string format, IFormatProvider provider);

        [H5.Template("H5.compare({this}, {other})")]
        [CLSCompliant(false)]
        public extern int CompareTo(ushort other);

        [H5.Template("H5.compare({this}, {obj})")]
        public extern int CompareTo(object obj);

        [H5.Template("{this} === {other}")]
        [CLSCompliant(false)]
        public extern bool Equals(ushort other);

        [H5.Template("System.UInt16.equals({this}, {other})")]
        public override extern bool Equals(object other);
    }
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
}