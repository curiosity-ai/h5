namespace System
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Reflectable]
    public struct Nullable<T> where T : struct
    {
        [HighFive.Template("{0}")]
        public extern Nullable(T value);

        public extern bool HasValue
        {
            [HighFive.Template("System.Nullable.hasValue({this})")]
            get;
        }

        public extern T Value
        {
            [HighFive.Template("System.Nullable.getValue({this})")]
            get;
        }

        [HighFive.Template("System.Nullable.getValueOrDefault({this}, {T:default})")]
        public extern T GetValueOrDefault();

        [HighFive.Template("System.Nullable.getValueOrDefault({this}, {0})")]
        public extern T GetValueOrDefault(T defaultValue);

        public static extern implicit operator T? (T value);

        [HighFive.Template("System.Nullable.getValue({this})")]
        public static extern explicit operator T(T? value);

        [HighFive.Template("System.Nullable.equalsT({this}, {other})")]
        public override extern bool Equals(object other);

        [HighFive.Template("System.Nullable.getHashCode({this}, {T:GetHashCode})", Fn = "System.Nullable.getHashCodeFn({T:GetHashCode})")]
        public override extern int GetHashCode();

        [HighFive.Template("System.Nullable.toString({this}, {T:ToString})", Fn = "System.Nullable.toStringFn({T:ToString})")]
        public override extern string ToString();
    }

    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    public static class Nullable
    {
        public static extern int Compare<T>(Nullable<T> n1, Nullable<T> n2) where T : struct;

        public static extern bool Equals<T>(Nullable<T> n1, Nullable<T> n2) where T : struct;

        public static extern Type GetUnderlyingType(Type nullableType);
    }
}