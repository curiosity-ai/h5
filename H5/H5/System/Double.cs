namespace System
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Constructor("Number")]
    [H5.Reflectable]
    public struct Double : IComparable, IComparable<double>, IEquatable<double>, IFormattable
    {
        private extern Double(int i);

        [H5.Template("System.Double.max")]
        public const double MaxValue = 1.7976931348623157E+308;

        [H5.Template("System.Double.min")]
        public const double MinValue = -1.7976931348623157E+308;

        [H5.InlineConst]
        public const double Epsilon = 4.94065645841247E-324;

        [H5.Template("Number.NEGATIVE_INFINITY")]
        public const double NegativeInfinity = -1D / 0D;

        [H5.Template("Number.POSITIVE_INFINITY")]
        public const double PositiveInfinity = 1D / 0D;

        [H5.Template("Number.NaN")]
        public const double NaN = 0D / 0D;

        [H5.Template("System.Double.format({this}, {format})")]
        public extern string Format(string format);

        [H5.Template("System.Double.format({this}, {format}, {provider})")]
        public extern string Format(string format, IFormatProvider provider);

        public extern string ToString(int radix);

        [H5.Template("System.Double.format({this}, {format})")]
        public extern string ToString(string format);

        [H5.Template("System.Double.format({this}, {format}, {provider})")]
        public extern string ToString(string format, IFormatProvider provider);

        [H5.Template(Fn = "System.Double.format")]
        public override extern string ToString();

        [H5.Template("System.Double.format({this}, \"G\", {provider})")]
        public extern string ToString(IFormatProvider provider);

        [H5.Template("System.Double.parse({s})")]
        public static extern double Parse(string s);

        [H5.Template("H5.Int.parseFloat({s}, {provider})")]
        public static extern double Parse(string s, IFormatProvider provider);

        [H5.Template("System.Double.tryParse({s}, null, {result})")]
        public static extern bool TryParse(string s, out double result);

        [H5.Template("System.Double.tryParse({s}, {provider}, {result})")]
        public static extern bool TryParse(string s, IFormatProvider provider, out double result);

        public extern string ToExponential();

        public extern string ToExponential(int fractionDigits);

        public extern string ToFixed();

        public extern string ToFixed(int fractionDigits);

        public extern string ToPrecision();

        public extern string ToPrecision(int precision);

        [H5.Template("({d} === Number.POSITIVE_INFINITY)")]
        public static extern bool IsPositiveInfinity(double d);

        [H5.Template("({d} === Number.NEGATIVE_INFINITY)")]
        public static extern bool IsNegativeInfinity(double d);

        [H5.Template("(Math.abs({d}) === Number.POSITIVE_INFINITY)")]
        public static extern bool IsInfinity(double d);

        [H5.Template("isFinite({d})")]
        public static extern bool IsFinite(double d);

        [H5.Template("isNaN({d})")]
        public static extern bool IsNaN(double d);

        [H5.Template("H5.compare({this}, {other})")]
        public extern int CompareTo(double other);

        [H5.Template("H5.compare({this}, {obj})")]
        public extern int CompareTo(object obj);

        [H5.Template("{this} === {other}")]
        public extern bool Equals(double other);

        [H5.Template("System.Double.equals({this}, {other})")]
        public override extern bool Equals(object other);

        [H5.Template(Fn = "System.Double.getHashCode")]
        public override extern int GetHashCode();
    }
}