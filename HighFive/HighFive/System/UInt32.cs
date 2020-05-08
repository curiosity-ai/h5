namespace System
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Reflectable]
    [H5.Constructor("Number")]
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public struct UInt32 : IComparable, IComparable<UInt32>, IEquatable<UInt32>, IFormattable
    {
        private extern UInt32(int i);

        [H5.InlineConst]
        [CLSCompliant(false)]
        public const uint MinValue = 0;

        [H5.InlineConst]
        [CLSCompliant(false)]
        public const uint MaxValue = 4294967295;

        [H5.Template("System.UInt32.parse({s})")]
        [CLSCompliant(false)]
        public static extern uint Parse(string s);

        [H5.Template("System.UInt32.parse({s}, {radix})")]
        [CLSCompliant(false)]
        public static extern uint Parse(string s, int radix);

        [H5.Template("System.UInt32.tryParse({s}, {result})")]
        [CLSCompliant(false)]
        public static extern bool TryParse(string s, out uint result);

        [H5.Template("System.UInt32.tryParse({s}, {result}, {radix})")]
        [CLSCompliant(false)]
        public static extern bool TryParse(string s, out uint result, int radix);

        public extern string ToString(int radix);

        [H5.Template("System.UInt32.format({this}, {format})")]
        public extern string Format(string format);

        [H5.Template("System.UInt32.format({this}, {format}, {provider})")]
        public extern string Format(string format, IFormatProvider provider);

        [H5.Template("System.UInt32.format({this}, {format})")]
        public extern string ToString(string format);

        [H5.Template("System.UInt32.format({this}, {format}, {provider})")]
        public extern string ToString(string format, IFormatProvider provider);

        [H5.Template("H5.compare({this}, {other})")]
        [CLSCompliant(false)]
        public extern int CompareTo(uint other);

        [H5.Template("H5.compare({this}, {obj})")]
        public extern int CompareTo(object obj);

        [H5.Template("{this} === {other}")]
        [CLSCompliant(false)]
        public extern bool Equals(uint other);

        [H5.Template("System.UInt32.equals({this}, {other})")]
        public override extern bool Equals(object other);
    }
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
}