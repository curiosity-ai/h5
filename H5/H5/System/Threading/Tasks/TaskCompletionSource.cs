using System.Collections.Generic;

namespace System.Threading.Tasks
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.IgnoreGeneric]
    [H5.Name("System.Threading.Tasks.TaskCompletionSource")]
    [H5.Reflectable]
    public class TaskCompletionSource<TResult>
    {
        public extern TaskCompletionSource();
        public extern TaskCompletionSource(object state);

        [H5.Convention(H5.Notation.CamelCase)]
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