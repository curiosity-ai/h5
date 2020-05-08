namespace System
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Reflectable]
    public struct Nullable<T> where T : struct
    {
        [H5.Template("{0}")]
        public extern Nullable(T value);

        public extern bool HasValue
        {
            [H5.Template("System.Nullable.hasValue({this})")]
            get;
        }

        public extern T Value
        {
            [H5.Template("System.Nullable.getValue({this})")]
            get;
        }

        [H5.Template("System.Nullable.getValueOrDefault({this}, {T:default})")]
        public extern T GetValueOrDefault();

        [H5.Template("System.Nullable.getValueOrDefault({this}, {0})")]
        public extern T GetValueOrDefault(T defaultValue);

        public static extern implicit operator T? (T value);

        [H5.Template("System.Nullable.getValue({this})")]
        public static extern explicit operator T(T? value);

        [H5.Template("System.Nullable.equalsT({this}, {other})")]
        public override extern bool Equals(object other);

        [H5.Template("System.Nullable.getHashCode({this}, {T:GetHashCode})", Fn = "System.Nullable.getHashCodeFn({T:GetHashCode})")]
        public override extern int GetHashCode();

        [H5.Template("System.Nullable.toString({this}, {T:ToString})", Fn = "System.Nullable.toStringFn({T:ToString})")]
        public override extern string ToString();
    }

    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    public static class Nullable
    {
        public static extern int Compare<T>(Nullable<T> n1, Nullable<T> n2) where T : struct;

        public static extern bool Equals<T>(Nullable<T> n1, Nullable<T> n2) where T : struct;

        public static extern Type GetUnderlyingType(Type nullableType);
    }
}