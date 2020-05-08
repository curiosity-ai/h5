namespace System
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Reflectable]
    [HighFive.Constructor("Number")]
    public struct Single : IComparable, IComparable<Single>, IEquatable<Single>, IFormattable
    {
        private extern Single(int i);

        [HighFive.InlineConst]
        public const float MaxValue = (float)3.40282346638528859e+38;

        [HighFive.InlineConst]
        public const float MinValue = (float)-3.40282346638528859e+38;

        [HighFive.InlineConst]
        public const float Epsilon = (float)1.4e-45;

        [HighFive.Template("Number.NaN")]
        public const float NaN = 0f / 0f;

        [HighFive.Template("Number.NEGATIVE_INFINITY")]
        public const float NegativeInfinity = -1f / 0f;

        [HighFive.Template("Number.POSITIVE_INFINITY")]
        public const float PositiveInfinity = 1f / 0f;

        [HighFive.Template("System.Single.format({this}, {format})")]
        public extern string Format(string format);

        [HighFive.Template("System.Single.format({this}, {format}, {provider})")]
        public extern string Format(string format, IFormatProvider provider);

        public extern string ToString(int radix);

        [HighFive.Template("System.Single.format({this}, {format})")]
        public extern string ToString(string format);

        [HighFive.Template("System.Single.format({this}, {format}, {provider})")]
        public extern string ToString(string format, IFormatProvider provider);

        [HighFive.Template(Fn = "System.Single.format")]
        public override extern string ToString();

        [HighFive.Template("System.Single.format({this}, \"G\", {provider})")]
        public extern string ToString(IFormatProvider provider);

        [HighFive.Template("System.Single.parse({s})")]
        public static extern float Parse(string s);

        [HighFive.Template("System.Single.parse({s}, {provider})")]
        public static extern float Parse(string s, IFormatProvider provider);

        [HighFive.Template("System.Single.tryParse({s}, null, {result})")]
        public static extern bool TryParse(string s, out float result);

        [HighFive.Template("System.Single.tryParse({s}, {provider}, {result})")]
        public static extern bool TryParse(string s, IFormatProvider provider, out float result);

        public extern string ToExponential();

        public extern string ToExponential(int fractionDigits);

        public extern string ToFixed();

        public extern string ToFixed(int fractionDigits);

        public extern string ToPrecision();

        public extern string ToPrecision(int precision);

        [HighFive.Template("({d} === Number.POSITIVE_INFINITY)")]
        public static extern bool IsPositiveInfinity(float d);

        [HighFive.Template("({d} === Number.NEGATIVE_INFINITY)")]
        public static extern bool IsNegativeInfinity(float d);

        [HighFive.Template("(Math.abs({d}) === Number.POSITIVE_INFINITY)")]
        public static extern bool IsInfinity(float d);

        [HighFive.Template("isFinite({d})")]
        public static extern bool IsFinite(float d);

        [HighFive.Template("isNaN({d})")]
        public static extern bool IsNaN(float d);

        [HighFive.Template("HighFive.compare({this}, {other})")]
        public extern int CompareTo(float other);

        [HighFive.Template("HighFive.compare({this}, {obj})")]
        public extern int CompareTo(object obj);

        [HighFive.Template("{this} === {other}")]
        public extern bool Equals(float other);

        [HighFive.Template("System.Single.equals({this}, {other})")]
        public override extern bool Equals(object other);

        [HighFive.Template(Fn = "System.Single.getHashCode")]
        public override extern int GetHashCode();
    }
}