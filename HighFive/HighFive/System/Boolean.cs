using System.Runtime.CompilerServices;

namespace System
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Reflectable]
    public struct Boolean : IComparable, IComparable<bool>, IEquatable<bool>
    {
        [HighFive.InlineConst]
        internal const int True = 1;

        [HighFive.InlineConst]
        internal const int False = 0;

        [HighFive.Template("System.Boolean.trueString")]
        public static readonly string TrueString = "True";

        [HighFive.Template("System.Boolean.falseString")]
        public static readonly string FalseString = "False";

        [HighFive.Template("false")]
        private extern Boolean(DummyTypeUsedToAddAttributeToDefaultValueTypeConstructor _);

        [HighFive.Template("HighFive.compare({this}, {other})")]
        public extern int CompareTo(bool other);

        [HighFive.Template("{this} === {other}")]
        public extern bool Equals(bool other);

        [HighFive.Template("System.Boolean.parse({value})")]
        public static extern bool Parse(string value);

        [HighFive.Template("System.Boolean.tryParse({value}, {result})")]
        public static extern bool TryParse(string value, out bool result);

        [HighFive.Template("HighFive.compare({this}, {other})")]
        public extern int CompareTo(object obj);

        [HighFive.Template(Fn = "System.Boolean.toString")]
        public override extern string ToString();
    }
}