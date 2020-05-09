namespace System
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Reflectable]
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public abstract class Enum : ValueType, IComparable, IFormattable
    {
        public static extern Object Parse(Type enumType, string value);

        public static extern Object Parse(Type enumType, string value, bool ignoreCase);

        public static extern string ToString(Type enumType, Enum value);

        public static extern Array GetValues(Type enumType);

        [H5.Template("H5.compare({this}, {target})")]
        public extern int CompareTo(object target);

        public static extern string Format(Type enumType, object value, string format);

        public static extern string GetName(Type enumType, object value);

        public static extern string[] GetNames(Type enumType);

        [H5.Template("System.Enum.hasFlag({this}, {flag})")]
        public extern bool HasFlag(Enum flag);

        public static extern bool IsDefined(Type enumType, object value);

        [H5.Template("System.Enum.tryParse({TEnum}, {value}, {result})")]
        public static extern bool TryParse<TEnum>(string value, out TEnum result) where TEnum : struct;

        [H5.Template("System.Enum.tryParse({TEnum}, {value}, {result}, {ignoreCase})")]
        public static extern bool TryParse<TEnum>(string value, bool ignoreCase, out TEnum result) where TEnum : struct;

        [H5.Template("System.Enum.toString({this:type}, {this})", Fn = "System.Enum.toStringFn({this:type})")]
        public override extern string ToString();

        [H5.Template("System.Enum.format({this:type}, {this}, {format})")]
        public extern string ToString(string format);

        [H5.Template("System.Enum.equals({this}, {other}, {this:type})")]
        public override extern bool Equals(object other);

        [H5.Template("System.Enum.format({this:type}, {this}, {format})")]
        public extern string ToString(string format, IFormatProvider formatProvider);

        [H5.Template("System.Enum.toObject({enumType}, {value})")]
        public static extern object ToObject(Type enumType, object value);
    }
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
}