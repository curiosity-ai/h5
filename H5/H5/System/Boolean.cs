using System.Runtime.CompilerServices;

namespace System
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Reflectable]
    public struct Boolean : IComparable, IComparable<bool>, IEquatable<bool>
    {
        [H5.InlineConst]
        internal const int True = 1;

        [H5.InlineConst]
        internal const int False = 0;

        [H5.Template("System.Boolean.trueString")]
        public static readonly string TrueString = "True";

        [H5.Template("System.Boolean.falseString")]
        public static readonly string FalseString = "False";

        [H5.Template("false")]
        private extern Boolean(DummyTypeUsedToAddAttributeToDefaultValueTypeConstructor _);

        [H5.Template("H5.compare({this}, {other})")]
        public extern int CompareTo(bool other);

        [H5.Template("{this} === {other}")]
        public extern bool Equals(bool other);

        [H5.Template("System.Boolean.parse({value})")]
        public static extern bool Parse(string value);

        [H5.Template("System.Boolean.tryParse({value}, {result})")]
        public static extern bool TryParse(string value, out bool result);

        [H5.Template("H5.compare({this}, {other})")]
        public extern int CompareTo(object obj);

        [H5.Template(Fn = "System.Boolean.toString")]
        public override extern string ToString();
    }
}