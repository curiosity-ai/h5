namespace System.Threading
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Reflectable]
    public class CancellationTokenSource : IDisposable
    {
        public extern CancellationTokenSource();

        public extern CancellationTokenSource(int millisecondsDelay);

        [HighFive.Template("new System.Threading.CancellationTokenSource({delay}.ticks / 10000)")]
        public extern CancellationTokenSource(TimeSpan delay);

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern bool IsCancellationRequested
        {
            get;
            private set;
        }

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern CancellationToken Token
        {
            get;
            private set;
        }

        public extern void Cancel();

        public extern void Cancel(bool throwOnFirstException);

        public extern void CancelAfter(int millisecondsDelay);

        [HighFive.Template("{this}.cancelAfter({delay}.ticks / 10000)")]
        public extern void CancelAfter(TimeSpan delay);

        public extern void Dispose();

        [HighFive.Name("createLinked")]
        public static extern CancellationTokenSource CreateLinkedTokenSource(CancellationToken token1, CancellationToken token2);

        [HighFive.Template("System.Threading.CancellationTokenSource.createLinked({*tokens})")]
        public static extern CancellationTokenSource CreateLinkedTokenSource(params CancellationToken[] tokens);
    }
}