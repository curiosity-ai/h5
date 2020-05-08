namespace System.Threading
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Reflectable]
    public struct CancellationToken
    {
        public extern CancellationToken(bool canceled);

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public static extern CancellationToken None
        {
            get;
        }

        public extern bool CanBeCanceled
        {
            [HighFive.Template("getCanBeCanceled()")]
            get;
        }

        public extern bool IsCancellationRequested
        {
            [HighFive.Template("getIsCancellationRequested()")]
            get;
        }

        public extern void ThrowIfCancellationRequested();

        public extern CancellationTokenRegistration Register(Action callback);

        [HighFive.Template("{this}.register({callback})")]
        public extern CancellationTokenRegistration Register(Action callback, bool useSynchronizationContext);

        public extern CancellationTokenRegistration Register(Action<object> callback, object state);

        [HighFive.Template("{this}.register({callback}, {state})")]
        public extern CancellationTokenRegistration Register(Action<object> callback, object state, bool useSynchronizationContext);
    }
}