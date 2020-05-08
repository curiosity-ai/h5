using System.Collections.Generic;

namespace System.Threading.Tasks
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.IgnoreGeneric]
    [HighFive.Name("System.Threading.Tasks.TaskCompletionSource")]
    [HighFive.Reflectable]
    public class TaskCompletionSource<TResult>
    {
        public extern TaskCompletionSource();
        public extern TaskCompletionSource(object state);

        [HighFive.Convention(HighFive.Notation.CamelCase)]
        public extern Task<TResult> Task
        {
            get;
        }

        public extern void SetCanceled();

        public extern void SetException(IEnumerable<Exception> exceptions);

        public extern void SetException(Exception exception);

        public extern void SetResult(TResult result);

        public extern bool TrySetCanceled();

        public extern bool TrySetException(IEnumerable<Exception> exceptions);

        public extern bool TrySetException(Exception exception);

        public extern bool TrySetResult(TResult result);
    }
}