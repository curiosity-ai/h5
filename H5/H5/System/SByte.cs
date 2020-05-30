namespace System
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Reflectable]
    [H5.Constructor("Number")]
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public struct SByte : IComparable, IComparable<sbyte>, IEquatable<sbyte>, IFormattable
    {
        private extern SByte(int i);

        [H5.InlineConst]
        [CLSCompliant(false)]
        public const sbyte MinValue = -128;

        [H5.InlineConst]
        [CLSCompliant(false)]
        public const sbyte MaxValue = 127;

        [H5.Template("System.SByte.parse({s})")]
        [CLSCompliant(false)]
        public static extern sbyte Parse(string s);

        [H5.Template("System.SByte.parse({s}, {radix})")]
        [CLSCompliant(false)]
        public static extern sbyte Parse(string s, int radix);

        [H5.Template("System.SByte.tryParse({s}, {result})")]
        [CLSCompliant(false)]
        public static extern bool TryParse(string s, out sbyte result);

        [H5.Template("System.SByte.tryParse({s}, {result}, {radix})")]
        [CLSCompliant(false)]
        public static extern bool TryParse(string s, out sbyte result, int radix);

        public extern string ToString(int radix);

        [H5.Template("System.SByte.format({this}, {format})")]
        public extern string Format(string format);

        [H5.Template("System.SByte.format({this}, {format}, {provider})")]
        public extern string Format(string format, IFormatProvider provider);

        [H5.Template("System.SByte.format({this}, {format})")]
        public extern string ToString(string format);

        [H5.Template("System.SByte.format({this}, {format}, {provider})")]
        public extern string ToString(string format, IFormatProvider provider);

        [H5.Template("H5.compare({this}, {other})")]
        [CLSCompliant(false)]
        public extern int CompareTo(sbyte other);

        [H5.Template("H5.compare({this}, {obj})")]
        public extern int CompareTo(object obj);

        [H5.Template("{this} === {other}")]
        [CLSCompliant(false)]
        public extern bool Equals(sbyte other);

        [H5.Template("System.SByte.equals({this}, {other})")]
        public override extern bool Equals(object other);
    }
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
}