namespace System.Globalization
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Reflectable]
    public sealed class NumberFormatInfo : IFormatProvider, ICloneable, H5.IH5Class
    {
        public extern NumberFormatInfo();

        [H5.Convention(H5.Notation.CamelCase)]
        public static extern NumberFormatInfo InvariantInfo
        {
            get;
        }

        [H5.Name("nanSymbol")]
        public extern string NaNSymbol
        {
            get;
            set;
        }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern string NegativeSign
        {
            get;
            set;
        }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern string PositiveSign
        {
            get;
            set;
        }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern string NegativeInfinitySymbol
        {
            get;
            set;
        }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern string PositiveInfinitySymbol
        {
            get;
            set;
        }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern string PercentSymbol
        {
            get;
            set;
        }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern int[] PercentGroupSizes
        {
            get;
            set;
        }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern int PercentDecimalDigits
        {
            get;
            set;
        }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern string PercentDecimalSeparator
        {
            get;
            set;
        }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern string PercentGroupSeparator
        {
            get;
            set;
        }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern int PercentPositivePattern
        {
            get;
            set;
        }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern int PercentNegativePattern
        {
            get;
            set;
        }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern string CurrencySymbol
        {
            get;
            set;
        }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern int[] CurrencyGroupSizes
        {
            get;
            set;
        }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern int CurrencyDecimalDigits
        {
            get;
            set;
        }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern string CurrencyDecimalSeparator
        {
            get;
            set;
        }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern string CurrencyGroupSeparator
        {
            get;
            set;
        }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern int CurrencyPositivePattern
        {
            get;
            set;
        }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern int CurrencyNegativePattern
        {
            get;
            set;
        }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern int[] NumberGroupSizes
        {
            get;
            set;
        }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern int NumberDecimalDigits
        {
            get;
            set;
        }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern string NumberDecimalSeparator
        {
            get;
            set;
        }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern string NumberGroupSeparator
        {
            get;
            set;
        }

        public extern object GetFormat(Type formatType);

        public extern object Clone();

        [H5.Convention(H5.Notation.CamelCase)]
        public static extern NumberFormatInfo CurrentInfo
        {
            get;
        }
    }
}