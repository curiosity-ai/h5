namespace System.Threading
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Reflectable]
    public struct CancellationToken
    {
        public extern CancellationToken(bool canceled);

        [H5.Convention(H5.Notation.CamelCase)]
        public static extern CancellationToken None
        {
            get;
        }

        public extern bool CanBeCanceled
        {
            [H5.Template("getCanBeCanceled()")]
            get;
        }

        public extern bool IsCancellationRequested
        {
            [H5.Template("getIsCancellationRequested()")]
            get;
        }

        public extern void ThrowIfCancellationRequested();

        public extern CancellationTokenRegistration Register(Action callback);

        [H5.Template("{this}.register({callback})")]
        public extern CancellationTokenRegistration Register(Action callback, bool useSynchronizationContext);

        public extern CancellationTokenRegistration Register(Action<object> callback, object state);

        [H5.Template("{this}.register({callback}, {state})")]
        public extern CancellationTokenRegistration Register(Action<object> callback, object state, bool useSynchronizationContext);
    }
}