namespace System.Diagnostics
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    public class Stopwatch
    {
        public static readonly long Frequency = 0;
        public static readonly bool IsHighResolution = false;

        public static extern Stopwatch StartNew();

        public extern TimeSpan Elapsed
        {
            [HighFive.Template("timeSpan()")]
            get;
        }

        public extern long ElapsedMilliseconds
        {
            [HighFive.Template("milliseconds()")]
            get;
        }

        public extern long ElapsedTicks
        {
            [HighFive.Template("ticks()")]
            get;
        }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern bool IsRunning
        {
            get;
        }

        public extern void Reset();

        public extern void Start();

        public extern void Stop();

        public extern void Restart();

        public extern static long GetTimestamp();
    }
}