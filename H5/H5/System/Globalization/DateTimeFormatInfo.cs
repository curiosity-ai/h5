namespace System.Globalization
{
    /// <summary>
    /// Provides culture-specific information about the format of date and time values.
    /// </summary>
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Reflectable]
    public sealed class DateTimeFormatInfo : IFormatProvider, ICloneable, H5.IH5Class
    {
        /// <summary>
        /// Initializes a new writable instance of the DateTimeFormatInfo class that is culture-independent (invariant).
        /// </summary>
        public extern DateTimeFormatInfo();

        /// <summary>
        /// Gets the default read-only DateTimeFormatInfo object that is culture-independent (invariant).
        /// </summary>
        [H5.Convention(H5.Notation.CamelCase)]
        public static extern DateTimeFormatInfo InvariantInfo
        {
            get;
        }

        /// <summary>
        /// Gets or sets the string designator for hours that are "ante meridiem" (before noon).
        /// </summary>
        [H5.Name("amDesignator")]
        public extern string AMDesignator
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the string designator for hours that are "post meridiem" (after noon).
        /// </summary>
        [H5.Name("pmDesignator")]
        public extern string PMDesignator
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the string that separates the components of a date, that is, the year, month, and day.
        /// </summary>
        [H5.Convention(H5.Notation.CamelCase)]
        public extern string DateSeparator
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the string that separates the components of time, that is, the hour, minutes, and seconds.
        /// </summary>
        [H5.Convention(H5.Notation.CamelCase)]
        public extern string TimeSeparator
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the custom format string for a universal, sortable date and time string.
        /// </summary>
        [H5.Convention(H5.Notation.CamelCase)]
        public extern string UniversalSortableDateTimePattern
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the custom format string for a sortable date and time value.
        /// </summary>
        [H5.Convention(H5.Notation.CamelCase)]
        public extern string SortableDateTimePattern
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the custom format string for a long date and long time value.
        /// </summary>
        [H5.Convention(H5.Notation.CamelCase)]
        public extern string FullDateTimePattern
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the custom format string for a long date value.
        /// </summary>
        [H5.Convention(H5.Notation.CamelCase)]
        public extern string LongDatePattern
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the custom format string for a short date value.
        /// </summary>
        [H5.Convention(H5.Notation.CamelCase)]
        public extern string ShortDatePattern
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the custom format string for a long time value.
        /// </summary>
        [H5.Convention(H5.Notation.CamelCase)]
        public extern string LongTimePattern
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the custom format string for a short time value.
        /// </summary>
        [H5.Convention(H5.Notation.CamelCase)]
        public extern string ShortTimePattern
        {
            get;
            set;
        }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern DayOfWeek FirstDayOfWeek
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a one-dimensional string array that contains the culture-specific full names of the days of the week.
        /// </summary>
        [H5.Convention(H5.Notation.CamelCase)]
        public extern string[] DayNames
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a one-dimensional array of type String containing the culture-specific abbreviated names of the days of the week.
        /// </summary>
        [H5.Convention(H5.Notation.CamelCase)]
        public extern string[] AbbreviatedDayNames
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a string array of the shortest unique abbreviated day names associated with the current DateTimeFormatInfo object.
        /// </summary>
        [H5.Convention(H5.Notation.CamelCase)]
        public extern string[] ShortestDayNames
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a one-dimensional array of type String containing the culture-specific full names of the months.
        /// </summary>
        [H5.Convention(H5.Notation.CamelCase)]
        public extern string[] MonthNames
        {
            get;
            set;
        }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern string[] MonthGenitiveNames
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a one-dimensional string array that contains the culture-specific abbreviated names of the months.
        /// </summary>
        [H5.Convention(H5.Notation.CamelCase)]
        public extern string[] AbbreviatedMonthNames
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a string array of abbreviated month names associated with the current DateTimeFormatInfo object.
        /// </summary>
        [H5.Convention(H5.Notation.CamelCase)]
        public extern string[] AbbreviatedMonthGenitiveNames
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the custom format string for a month and day value.
        /// </summary>
        [H5.Convention(H5.Notation.CamelCase)]
        public extern string MonthDayPattern
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the custom format string for a time value that is based on the Internet Engineering Task Force (IETF) Request for Comments (RFC) 1123 specification.
        /// </summary>
        [H5.Name("rfc1123Pattern")]
        public extern string RFC1123Pattern
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the custom format string for a year and month value.
        /// </summary>
        [H5.Convention(H5.Notation.CamelCase)]
        public extern string YearMonthPattern
        {
            get;
            set;
        }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern string RoundtripFormat
        {
            get;
            set;
        }

        /// <summary>
        /// Returns an object of the specified type that provides a date and time formatting service.
        /// </summary>
        /// <param name="formatType">The type of the required formatting service.</param>
        /// <returns>The current object, if formatType is the same as the type of the current DateTimeFormatInfo; otherwise, null.</returns>
        public extern object GetFormat(Type formatType);

        /// <summary>
        /// Creates a shallow copy of the DateTimeFormatInfo.
        /// </summary>
        /// <returns>A new DateTimeFormatInfo object copied from the original DateTimeFormatInfo.</returns>
        public extern object Clone();

        /// <summary>
        /// Gets a read-only DateTimeFormatInfo object that formats values based on the current culture.
        /// </summary>
        [H5.Convention(H5.Notation.CamelCase)]
        public static extern DateTimeFormatInfo CurrentInfo
        {
            get;
        }

        /// <summary>
        /// Returns the culture-specific abbreviated name of the specified day of the week based on the culture associated with the current DateTimeFormatInfo object.
        /// </summary>
        /// <param name="dayofweek">A System.DayOfWeek value.</param>
        /// <returns>The culture-specific abbreviated name of the day of the week represented by dayofweek.</returns>
        public extern string GetAbbreviatedDayName(DayOfWeek dayofweek);

        /// <summary>
        /// Returns the culture-specific abbreviated name of the specified month based on the culture associated with the current DateTimeFormatInfo object.
        /// </summary>
        /// <param name="month">An integer from 1 through 13 representing the name of the month to retrieve.</param>
        /// <returns>The culture-specific abbreviated name of the month represented by month.</returns>
        public extern string GetAbbreviatedMonthName(int month);

        /// <summary>
        /// Returns all the standard patterns in which date and time values can be formatted.
        /// </summary>
        /// <returns>An array that contains the standard patterns in which date and time values can be formatted.</returns>
        public extern string[] GetAllDateTimePatterns();

        /// <summary>
        /// Returns all the patterns in which date and time values can be formatted using the specified standard format string.
        /// </summary>
        /// <param name="format">A standard format string.</param>
        /// <returns>An array containing the standard patterns in which date and time values can be formatted using the specified format string.</returns>
        public extern string[] GetAllDateTimePatterns(char format);

        /// <summary>
        /// Returns the culture-specific full name of the specified day of the week based on the culture associated with the current DateTimeFormatInfo object.
        /// </summary>
        /// <param name="dayofweek">A System.DayOfWeek value.</param>
        /// <returns>The culture-specific full name of the day of the week represented by dayofweek.</returns>
        public extern string GetDayName(DayOfWeek dayofweek);

        /// <summary>
        /// Returns the culture-specific full name of the specified month based on the culture associated with the current DateTimeFormatInfo object.
        /// </summary>
        /// <param name="month">An integer from 1 through 13 representing the name of the month to retrieve.</param>
        /// <returns>The culture-specific full name of the month represented by month.</returns>
        public extern string GetMonthName(int month);

        /// <summary>
        /// Obtains the shortest abbreviated day name for a specified day of the week associated with the current DateTimeFormatInfo object.
        /// </summary>
        /// <param name="dayOfWeek">One of the DayOfWeek values.</param>
        /// <returns>The abbreviated name of the week that corresponds to the dayOfWeek parameter.</returns>
        public extern string GetShortestDayName(DayOfWeek dayOfWeek);
    }
}