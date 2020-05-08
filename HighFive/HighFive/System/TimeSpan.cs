namespace System
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Reflectable]
    public struct TimeSpan : IComparable, IComparable<TimeSpan>, IEquatable<TimeSpan>, IFormattable, HighFive.IHighFiveClass
    {
        [HighFive.InlineConst]
        public const long TicksPerDay = 864000000000;

        [HighFive.InlineConst]
        public const long TicksPerHour = 36000000000;

        [HighFive.InlineConst]
        public const long TicksPerMillisecond = 10000;

        [HighFive.InlineConst]
        public const long TicksPerMinute = 600000000;

        [HighFive.InlineConst]
        public const long TicksPerSecond = 10000000;

        public static readonly TimeSpan MaxValue;
        public static readonly TimeSpan MinValue;
        public static readonly TimeSpan Zero;

        public extern TimeSpan(long ticks);

        public extern TimeSpan(int hours, int minutes, int seconds);

        public extern TimeSpan(int days, int hours, int minutes, int seconds);

        public extern TimeSpan(int days, int hours, int minutes, int seconds, int milliseconds);

        [HighFive.Template("System.TimeSpan.neg({t})")]
        public static extern TimeSpan operator -(TimeSpan t);

        [HighFive.Template("System.TimeSpan.sub({t1}, {t2})")]
        public static extern TimeSpan operator -(TimeSpan t1, TimeSpan t2);

        [HighFive.Template("System.TimeSpan.neq({t1}, {t2})")]
        public static extern bool operator !=(TimeSpan t1, TimeSpan t2);

        [HighFive.Template("System.TimeSpan.plus({t})")]
        public static extern TimeSpan operator +(TimeSpan t);

        [HighFive.Template("System.TimeSpan.add({t1}, {t2})")]
        public static extern TimeSpan operator +(TimeSpan t1, TimeSpan t2);

        [HighFive.Template("System.TimeSpan.lt({t1}, {t2})")]
        public static extern bool operator <(TimeSpan t1, TimeSpan t2);

        [HighFive.Template("System.TimeSpan.lte({t1}, {t2})")]
        public static extern bool operator <=(TimeSpan t1, TimeSpan t2);

        [HighFive.Template("System.TimeSpan.eq({t1}, {t2})")]
        public static extern bool operator ==(TimeSpan t1, TimeSpan t2);

        [HighFive.Template("System.TimeSpan.gt({t1}, {t2})")]
        public static extern bool operator >(TimeSpan t1, TimeSpan t2);

        [HighFive.Template("System.TimeSpan.gte({t1}, {t2})")]
        public static extern bool operator >=(TimeSpan t1, TimeSpan t2);

        public extern int Days
        {
            [HighFive.Template("getDays()")]
            get;
        }

        public extern int Hours
        {
            [HighFive.Template("getHours()")]
            get;
        }

        public extern int Milliseconds
        {
            [HighFive.Template("getMilliseconds()")]
            get;
        }

        public extern int Minutes
        {
            [HighFive.Template("getMinutes()")]
            get;
        }

        public extern int Seconds
        {
            [HighFive.Template("getSeconds()")]
            get;
        }

        [HighFive.Template("TimeToTicks({0}, {1}, {2})")]
        internal extern static long TimeToTicks(int hour, int minute, int second);

        // internal so that DateTime doesn't have to call an extra get
        // method for some arithmetic operations.
        [HighFive.Template("getTicks()")]
        internal long _ticks;

        public extern long Ticks
        {
            [HighFive.Template("getTicks()")]
            get;
        }

        public extern double TotalDays
        {
            [HighFive.Template("getTotalDays()")]
            get;
        }

        public extern double TotalHours
        {
            [HighFive.Template("getTotalHours()")]
            get;
        }

        public extern double TotalMilliseconds
        {
            [HighFive.Template("getTotalMilliseconds()")]
            get;
        }

        public extern double TotalMinutes
        {
            [HighFive.Template("getTotalMinutes()")]
            get;
        }

        public extern double TotalSeconds
        {
            [HighFive.Template("getTotalSeconds()")]
            get;
        }

        public extern TimeSpan Add(TimeSpan ts);

        [HighFive.Template("{t1}.compareTo({t2})")]
        public static extern int Compare(TimeSpan t1, TimeSpan t2);

        public extern int CompareTo(object value);

        public extern int CompareTo(TimeSpan value);

        public extern TimeSpan Duration();

        public extern bool Equals(TimeSpan obj);

        [HighFive.Template("({t1}).ticks.eq(({t2}).ticks)")]
        public static extern bool Equals(TimeSpan t1, TimeSpan t2);

        public static extern TimeSpan FromDays(double value);

        public static extern TimeSpan FromHours(double value);

        public static extern TimeSpan FromMilliseconds(double value);

        public static extern TimeSpan FromMinutes(double value);

        public static extern TimeSpan FromSeconds(double value);

        public static extern TimeSpan FromTicks(long value);

        public extern TimeSpan Negate();

        public extern TimeSpan Subtract(TimeSpan ts);

        public extern string ToString(string format);

        public extern string ToString(string format, IFormatProvider provider);

        [HighFive.Name("toString")]
        public extern string Format(string format);

        [HighFive.Name("toString")]
        public extern string Format(string format, IFormatProvider provider);

        public static extern TimeSpan Parse(string s);
        public static extern TimeSpan Parse(string s, IFormatProvider provider);

        [HighFive.Template("System.TimeSpan.tryParse({s}, null, {result})")]
        public static extern bool TryParse(string s, out TimeSpan result);

        [HighFive.Template("System.TimeSpan.tryParse({s}, {provider}, {result})")]
        public static extern bool TryParse(string s, IFormatProvider provider, out TimeSpan result);
    }
}