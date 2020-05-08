namespace System
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Reflectable]
    [H5.Constructor("Number")]
    public struct Single : IComparable, IComparable<Single>, IEquatable<Single>, IFormattable
    {
        private extern Single(int i);

        [H5.InlineConst]
        public const float MaxValue = (float)3.40282346638528859e+38;

        [H5.InlineConst]
        public const float MinValue = (float)-3.40282346638528859e+38;

        [H5.InlineConst]
        public const float Epsilon = (float)1.4e-45;

        [H5.Template("Number.NaN")]
        public const float NaN = 0f / 0f;

        [H5.Template("Number.NEGATIVE_INFINITY")]
        public const float NegativeInfinity = -1f / 0f;

        [H5.Template("Number.POSITIVE_INFINITY")]
        public const float PositiveInfinity = 1f / 0f;

        [H5.Template("System.Single.format({this}, {format})")]
        public extern string Format(string format);

        [H5.Template("System.Single.format({this}, {format}, {provider})")]
        public extern string Format(string format, IFormatProvider provider);

        public extern string ToString(int radix);

        [H5.Template("System.Single.format({this}, {format})")]
        public extern string ToString(string format);

        [H5.Template("System.Single.format({this}, {format}, {provider})")]
        public extern string ToString(string format, IFormatProvider provider);

        [H5.Template(Fn = "System.Single.format")]
        public override extern string ToString();

        [H5.Template("System.Single.format({this}, \"G\", {provider})")]
        public extern string ToString(IFormatProvider provider);

        [H5.Template("System.Single.parse({s})")]
        public static extern float Parse(string s);

        [H5.Template("System.Single.parse({s}, {provider})")]
        public static extern float Parse(string s, IFormatProvider provider);

        [H5.Template("System.Single.tryParse({s}, null, {result})")]
        public static extern bool TryParse(string s, out float result);

        [H5.Template("System.Single.tryParse({s}, {provider}, {result})")]
        public static extern bool TryParse(string s, IFormatProvider provider, out float result);

        public extern string ToExponential();

        public extern string ToExponential(int fractionDigits);

        public extern string ToFixed();

        public extern string ToFixed(int fractionDigits);

        public extern string ToPrecision();

        public extern string ToPrecision(int precision);

        [H5.Template("({d} === Number.POSITIVE_INFINITY)")]
        public static extern bool IsPositiveInfinity(float d);

        [H5.Template("({d} === Number.NEGATIVE_INFINITY)")]
        public static extern bool IsNegativeInfinity(float d);

        [H5.Template("(Math.abs({d}) === Number.POSITIVE_INFINITY)")]
        public static extern bool IsInfinity(float d);

        [H5.Template("isFinite({d})")]
        public static extern bool IsFinite(float d);

        [H5.Template("isNaN({d})")]
        public static extern bool IsNaN(float d);

        [H5.Template("H5.compare({this}, {other})")]
        public extern int CompareTo(float other);

        [H5.Template("H5.compare({this}, {obj})")]
        public extern int CompareTo(object obj);

        [H5.Template("{this} === {other}")]
        public extern bool Equals(float other);

        [H5.Template("System.Single.equals({this}, {other})")]
        public override extern bool Equals(object other);

        [H5.Template(Fn = "System.Single.getHashCode")]
        public override extern int GetHashCode();
    }
}