namespace System
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Constructor("Number")]
    [HighFive.Reflectable]
    public struct Double : IComparable, IComparable<Double>, IEquatable<Double>, IFormattable
    {
        private extern Double(int i);

        [HighFive.Template("System.Double.max")]
        public const double MaxValue = 1.7976931348623157E+308;

        [HighFive.Template("System.Double.min")]
        public const double MinValue = -1.7976931348623157E+308;

        [HighFive.InlineConst]
        public const double Epsilon = 4.94065645841247E-324;

        [HighFive.Template("Number.NEGATIVE_INFINITY")]
        public const double NegativeInfinity = -1D / 0D;

        [HighFive.Template("Number.POSITIVE_INFINITY")]
        public const double PositiveInfinity = 1D / 0D;

        [HighFive.Template("Number.NaN")]
        public const double NaN = 0D / 0D;

        [HighFive.Template("System.Double.format({this}, {format})")]
        public extern string Format(string format);

        [HighFive.Template("System.Double.format({this}, {format}, {provider})")]
        public extern string Format(string format, IFormatProvider provider);

        public extern string ToString(int radix);

        [HighFive.Template("System.Double.format({this}, {format})")]
        public extern string ToString(string format);

        [HighFive.Template("System.Double.format({this}, {format}, {provider})")]
        public extern string ToString(string format, IFormatProvider provider);

        [HighFive.Template(Fn = "System.Double.format")]
        public override extern string ToString();

        [HighFive.Template("System.Double.format({this}, \"G\", {provider})")]
        public extern string ToString(IFormatProvider provider);

        [HighFive.Template("System.Double.parse({s})")]
        public static extern double Parse(string s);

        [HighFive.Template("HighFive.Int.parseFloat({s}, {provider})")]
        public static extern double Parse(string s, IFormatProvider provider);

        [HighFive.Template("System.Double.tryParse({s}, null, {result})")]
        public static extern bool TryParse(string s, out double result);

        [HighFive.Template("System.Double.tryParse({s}, {provider}, {result})")]
        public static extern bool TryParse(string s, IFormatProvider provider, out double result);

        public extern string ToExponential();

        public extern string ToExponential(int fractionDigits);

        public extern string ToFixed();

        public extern string ToFixed(int fractionDigits);

        public extern string ToPrecision();

        public extern string ToPrecision(int precision);

        [HighFive.Template("({d} === Number.POSITIVE_INFINITY)")]
        public static extern bool IsPositiveInfinity(double d);

        [HighFive.Template("({d} === Number.NEGATIVE_INFINITY)")]
        public static extern bool IsNegativeInfinity(double d);

        [HighFive.Template("(Math.abs({d}) === Number.POSITIVE_INFINITY)")]
        public static extern bool IsInfinity(double d);

        [HighFive.Template("isFinite({d})")]
        public static extern bool IsFinite(double d);

        [HighFive.Template("isNaN({d})")]
        public static extern bool IsNaN(double d);

        [HighFive.Template("HighFive.compare({this}, {other})")]
        public extern int CompareTo(double other);

        [HighFive.Template("HighFive.compare({this}, {obj})")]
        public extern int CompareTo(object obj);

        [HighFive.Template("{this} === {other}")]
        public extern bool Equals(double other);

        [HighFive.Template("System.Double.equals({this}, {other})")]
        public override extern bool Equals(object other);

        [HighFive.Template(Fn = "System.Double.getHashCode")]
        public override extern int GetHashCode();
    }
}