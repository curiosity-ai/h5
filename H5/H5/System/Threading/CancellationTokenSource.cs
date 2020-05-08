namespace System.Threading
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Reflectable]
    public class CancellationTokenSource : IDisposable
    {
        public extern CancellationTokenSource();

        public extern CancellationTokenSource(int millisecondsDelay);

        [H5.Template("new System.Threading.CancellationTokenSource({delay}.ticks / 10000)")]
        public extern CancellationTokenSource(TimeSpan delay);

        [H5.Convention(H5.Notation.CamelCase)]
        public extern bool IsCancellationRequested
        {
            get;
            private set;
        }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern CancellationToken Token
        {
            get;
            private set;
        }

        public extern void Cancel();

        public extern void Cancel(bool throwOnFirstException);

        public extern void CancelAfter(int millisecondsDelay);

        [H5.Template("{this}.cancelAfter({delay}.ticks / 10000)")]
        public extern void CancelAfter(TimeSpan delay);

        public extern void Dispose();

        [H5.Name("createLinked")]
        public static extern CancellationTokenSource CreateLinkedTokenSource(CancellationToken token1, CancellationToken token2);

        [H5.Template("System.Threading.CancellationTokenSource.createLinked({*tokens})")]
        public static extern CancellationTokenSource CreateLinkedTokenSource(params CancellationToken[] tokens);
    }
}