namespace System
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Constructor("Number")]
    [H5.Reflectable]
    public struct Char : IComparable, IComparable<Char>, IEquatable<Char>, IFormattable
    {
        private extern Char(int i);

        [H5.InlineConst]
        public const char MinValue = '\0';

        [H5.InlineConst]
        public const char MaxValue = '\xFFFF';

        [H5.Template("System.Char.format({this}, {0})")]
        public extern string Format(string format);

        [H5.Template("System.Char.format({this}, {0}, {1})")]
        public extern string Format(string format, IFormatProvider provider);

        [H5.Template("System.Char.charCodeAt({0}, 0)")]
        public static extern char Parse(string s);

        [H5.Template(Fn = "String.fromCharCode")]
        public override extern string ToString();

        [H5.Template("System.Char.format({this}, {format})")]
        public extern string ToString(string format);

        [H5.Template("System.Char.format({this}, {format}, {provider})")]
        public extern string ToString(string format, IFormatProvider provider);

        [H5.Template("H5.compare({this}, {0})")]
        public extern int CompareTo(char value);

        [H5.Template("H5.compare({this}, {0})")]
        public extern int CompareTo(object value);

        [H5.Template("{this} === {0}")]
        public extern bool Equals(char obj);

        [H5.Template("H5.isLower({0})")]
        public static extern bool IsLower(char s);

        [H5.Template("H5.isUpper({0})")]
        public static extern bool IsUpper(char s);

        /// <summary>
        /// Indicates whether the character at the specified position in a specified string is categorized as an uppercase letter.
        /// </summary>
        /// <param name="s">A string.</param>
        /// <param name="index">The position of the character to evaluate in s.</param>
        /// <returns>true if the character at position index in s is an uppercase letter; otherwise, false.</returns>
        public extern static bool IsUpper(String s, int index);

        [H5.Template("String.fromCharCode({0}).toLowerCase().charCodeAt(0)")]
        public static extern char ToLower(char c);

        [H5.Template("String.fromCharCode({0}).toUpperCase().charCodeAt(0)")]
        public static extern char ToUpper(char c);

        public static extern bool IsLetter(char c);

        [H5.Template("System.Char.isLetter(({0}).charCodeAt({1}))")]
        public static extern bool IsLetter(string s, int index);

        public static extern bool IsDigit(char c);

        [H5.Template("System.Char.isDigit(({0}).charCodeAt({1}))")]
        public static extern bool IsDigit(string s, int index);

        [H5.Template("(System.Char.isDigit({0}) || System.Char.isLetter({0}))")]
        public static extern bool IsLetterOrDigit(char c);

        [H5.Template("(System.Char.isDigit(({0}).charCodeAt({1})) || System.Char.isLetter(({0}).charCodeAt({1})))")]
        public static extern bool IsLetterOrDigit(string s, int index);

        [H5.Template("System.Char.isWhiteSpace(String.fromCharCode({0}))")]
        public static extern bool IsWhiteSpace(char c);

        [H5.Template("System.Char.isWhiteSpace(({0}).charAt({1}))")]
        public static extern bool IsWhiteSpace(string s, int index);

        public static extern bool IsHighSurrogate(char c);

        [H5.Template("System.Char.isHighSurrogate(({0}).charCodeAt({1}))")]
        public static extern bool IsHighSurrogate(string s, int index);

        public static extern bool IsLowSurrogate(char c);

        [H5.Template("System.Char.isLowSurrogate(({0}).charCodeAt({1}))")]
        public static extern bool IsLowSurrogate(string s, int index);

        public static extern bool IsSurrogate(char c);

        [H5.Template("System.Char.isSurrogate(({0}).charCodeAt({1}))")]
        public static extern bool IsSurrogate(string s, int index);

        [H5.Template("(System.Char.isHighSurrogate({0}) && System.Char.isLowSurrogate({1}))")]
        public static extern bool IsSurrogatePair(char highSurrogate, char lowSurrogate);

        [H5.Template("(System.Char.isHighSurrogate(({0}).charCodeAt({1})) && System.Char.isLowSurrogate(({0}).charCodeAt({1} + 1)))")]
        public static extern bool IsSurrogatePair(string s, int index);

        public static extern bool IsSymbol(char c);

        [H5.Template("System.Char.isSymbol(({0}).charCodeAt({1}))")]
        public static extern bool IsSymbol(string s, int index);

        public static extern bool IsSeparator(char c);

        [H5.Template("System.Char.isSeparator(({0}).charCodeAt({1}))")]
        public static extern bool IsSeparator(string s, int index);

        public static extern bool IsPunctuation(char c);

        [H5.Template("System.Char.isPunctuation(({0}).charCodeAt({1}))")]
        public static extern bool IsPunctuation(string s, int index);

        public static extern bool IsNumber(char c);

        [H5.Template("System.Char.isNumber(({0}).charCodeAt({1}))")]
        public static extern bool IsNumber(string s, int index);

        public static extern bool IsControl(char c);

        [H5.Template("System.Char.isControl(({0}).charCodeAt({1}))")]
        public static extern bool IsControl(string s, int index);

        [H5.Template("System.Char.equals({this}, {0})")]
        public override extern bool Equals(object obj);

        [H5.Template(Fn = "System.Char.getHashCode")]
        public override extern int GetHashCode();

        [H5.Template("String.fromCharCode({c})")]
        public static extern string ToString(Char c);
    }
}