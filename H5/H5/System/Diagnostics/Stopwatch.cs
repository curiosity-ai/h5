namespace System.Diagnostics
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    public class Stopwatch
    {
        public static readonly long Frequency = 0;
        public static readonly bool IsHighResolution = false;

        public static extern Stopwatch StartNew();

        public extern TimeSpan Elapsed
        {
            [H5.Template("timeSpan()")]
            get;
        }

        public extern long ElapsedMilliseconds
        {
            [H5.Template("milliseconds()")]
            get;
        }

        public extern long ElapsedTicks
        {
            [H5.Template("ticks()")]
            get;
        }

        [H5.Convention(H5.Notation.CamelCase)]
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