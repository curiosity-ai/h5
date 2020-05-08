using System.Runtime.CompilerServices;

namespace System.Threading.Tasks
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    public class TaskAwaiter : INotifyCompletion
    {
        internal extern TaskAwaiter();

        public extern bool IsCompleted
        {
            [H5.Template("isCompleted()")]
            get;
        }

        [H5.Name("continueWith")]
        public extern void OnCompleted(Action continuation);

        [H5.Name("getAwaitedResult")]
        public extern void GetResult();
    }

    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Name("System.Threading.Tasks.Task")]
    public class TaskAwaiter<TResult> : INotifyCompletion
    {
        internal extern TaskAwaiter();

        public extern bool IsCompleted
        {
            [H5.Template("isCompleted()")]
            get;
        }

        [H5.Name("continueWith")]
        public extern void OnCompleted(Action continuation);

        [H5.Name("getAwaitedResult")]
        public extern TResult GetResult();
    }
}