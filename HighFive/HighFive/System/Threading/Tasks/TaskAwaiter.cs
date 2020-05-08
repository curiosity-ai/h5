using System.Runtime.CompilerServices;

namespace System.Threading.Tasks
{
    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    public class TaskAwaiter : INotifyCompletion
    {
        internal extern TaskAwaiter();

        public extern bool IsCompleted
        {
            [HighFive.Template("isCompleted()")]
            get;
        }

        [HighFive.Name("continueWith")]
        public extern void OnCompleted(Action continuation);

        [HighFive.Name("getAwaitedResult")]
        public extern void GetResult();
    }

    [HighFive.Convention(Member = HighFive.ConventionMember.Field | HighFive.ConventionMember.Method, Notation = HighFive.Notation.CamelCase)]
    [HighFive.External]
    [HighFive.Name("System.Threading.Tasks.Task")]
    public class TaskAwaiter<TResult> : INotifyCompletion
    {
        internal extern TaskAwaiter();

        public extern bool IsCompleted
        {
            [HighFive.Template("isCompleted()")]
            get;
        }

        [HighFive.Name("continueWith")]
        public extern void OnCompleted(Action continuation);

        [HighFive.Name("getAwaitedResult")]
        public extern TResult GetResult();
    }
}