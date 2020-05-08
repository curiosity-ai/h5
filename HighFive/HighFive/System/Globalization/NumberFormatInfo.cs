namespace System.Globalization
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Reflectable]
    public sealed class NumberFormatInfo : IFormatProvider, ICloneable, HighFive.IHighFiveClass
    {
        public extern NumberFormatInfo();

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public static extern NumberFormatInfo InvariantInfo
        {
            get;
        }

        [HighFive.Name("nanSymbol")]
        public extern string NaNSymbol
        {
            get;
            set;
        }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern string NegativeSign
        {
            get;
            set;
        }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern string PositiveSign
        {
            get;
            set;
        }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern string NegativeInfinitySymbol
        {
            get;
            set;
        }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern string PositiveInfinitySymbol
        {
            get;
            set;
        }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern string PercentSymbol
        {
            get;
            set;
        }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern int[] PercentGroupSizes
        {
            get;
            set;
        }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern int PercentDecimalDigits
        {
            get;
            set;
        }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern string PercentDecimalSeparator
        {
            get;
            set;
        }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern string PercentGroupSeparator
        {
            get;
            set;
        }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern int PercentPositivePattern
        {
            get;
            set;
        }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern int PercentNegativePattern
        {
            get;
            set;
        }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern string CurrencySymbol
        {
            get;
            set;
        }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern int[] CurrencyGroupSizes
        {
            get;
            set;
        }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern int CurrencyDecimalDigits
        {
            get;
            set;
        }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern string CurrencyDecimalSeparator
        {
            get;
            set;
        }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern string CurrencyGroupSeparator
        {
            get;
            set;
        }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern int CurrencyPositivePattern
        {
            get;
            set;
        }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern int CurrencyNegativePattern
        {
            get;
            set;
        }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern int[] NumberGroupSizes
        {
            get;
            set;
        }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern int NumberDecimalDigits
        {
            get;
            set;
        }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern string NumberDecimalSeparator
        {
            get;
            set;
        }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern string NumberGroupSeparator
        {
            get;
            set;
        }

        public extern object GetFormat(Type formatType);

        public extern object Clone();

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public static extern NumberFormatInfo CurrentInfo
        {
            get;
        }
    }
}