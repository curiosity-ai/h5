using H5;
using System.Collections.Generic;

namespace System.Threading.Tasks
{
    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Reflectable]
    public class Task : IDisposable, H5.IH5Class, IAsyncResult
    {
        public extern Task(Action action);

        public extern Task(Action<object> action, object state);

        public extern AggregateException Exception
        {
            [Template("getException()")]
            get;
        }

        public extern bool IsCanceled
        {
            [H5.Template("isCanceled()")]
            get;
        }

        public extern bool IsCompleted
        {
            [H5.Template("isCompleted()")]
            get;
        }

        public extern bool IsFaulted
        {
            [H5.Template("isFaulted()")]
            get;
        }

        [H5.Convention(H5.Notation.CamelCase)]
        public extern TaskStatus Status
        {
            get;
        }

        public object AsyncState
        {
            get;
        }

        bool IAsyncResult.CompletedSynchronously
        {
            get;
        }

        public extern Task ContinueWith(Action<Task> continuationAction);

        public extern Task<TResult> ContinueWith<TResult>(Func<Task, TResult> continuationFunction);

        public extern void Start();

        public extern TaskAwaiter GetAwaiter();

        public extern void Dispose();

        public extern void Complete(object result = null);

        [ToAwait]
        public extern void Wait();

        [Name("wait")]
        public extern Task WaitTask();

        [ToAwait]
        public extern void Wait(CancellationToken cancellationToken);

        [Name("wait")]
        public extern Task WaitTask(CancellationToken cancellationToken);

        [ToAwait]
        public extern bool Wait(int millisecondsTimeout);

        [Name("waitt")]
        public extern Task<bool> WaitTask(int millisecondsTimeout);

        [ToAwait]
        public extern bool Wait(int millisecondsTimeout, CancellationToken cancellationToken);

        [Name("waitt")]
        public extern Task<bool> WaitTask(int millisecondsTimeout, CancellationToken cancellationToken);

        [ToAwait]
        public extern bool Wait(TimeSpan timeout);

        [Name("waitt")]
        public extern Task<bool> WaitTask(TimeSpan timeout);

        public static extern Task Delay(int millisecondDelay);

        public static extern Task Delay(int millisecondsDelay, CancellationToken cancellationToken);

        public static extern Task Delay(TimeSpan delay);

        public static extern Task Delay(TimeSpan delay, CancellationToken cancellationToken);

        
        public static extern Task CompletedTask 
        {
            [H5.Template("System.Threading.Tasks.Task.fromResult({}, null)")]
            get; 
        }

        [H5.Template("System.Threading.Tasks.Task.fromResult({result}, {TResult})")]
        public static extern Task<TResult> FromResult<TResult>(TResult result);

        [H5.Template("System.Threading.Tasks.Task.fromException({exception}, null)")]
        public static extern Task FromException(Exception exception);        

        [H5.Template("System.Threading.Tasks.Task.fromException({exception}, {TResult})")]
        public static extern Task<TResult> FromException<TResult>(Exception exception);        

        public static extern Task Run(Action action);

        public static extern Task<TResult> Run<TResult>(Func<TResult> function);

        public static extern Task WhenAll(params Task[] tasks);

        public static extern Task WhenAll(IEnumerable<Task> tasks);

        public static extern Task<TResult[]> WhenAll<TResult>(params Task<TResult>[] tasks);

        public static extern Task<TResult[]> WhenAll<TResult>(IEnumerable<Task<TResult>> tasks);

        public static extern Task<Task> WhenAny(params Task[] tasks);

        public static extern Task<Task> WhenAny(IEnumerable<Task> tasks);

        public static extern Task<Task<TResult>> WhenAny<TResult>(params Task<TResult>[] tasks);

        public static extern Task<Task<TResult>> WhenAny<TResult>(IEnumerable<Task<TResult>> tasks);

        public static extern Task FromCallback(object target, string method, params object[] otherArguments);

        public static extern Task FromCallbackResult(object target, string method, Delegate resultHandler, params object[] otherArguments);

        public static extern Task<TResult> FromCallback<TResult>(object target, string method, params object[] otherArguments);

        public static extern Task<TResult> FromCallbackResult<TResult>(object target, string method, Delegate resultHandler, params object[] otherArguments);

        public static extern Task<object[]> FromPromise(IPromise promise);

        public static extern Task<TResult> FromPromise<TResult>(IPromise promise, Delegate resultHandler);

        public static extern Task<TResult> FromPromise<TResult>(IPromise promise, Delegate resultHandler, Delegate errorHandler);

        public static extern Task<TResult> FromPromise<TResult>(IPromise promise, Delegate resultHandler, Delegate errorHandler, Delegate progressHandler);
    }

    [H5.Convention(Member = H5.ConventionMember.Field | H5.ConventionMember.Method, Notation = H5.Notation.CamelCase)]
    [H5.External]
    [H5.Reflectable]
    public class Task<TResult> : Task
    {
        public extern Task(Func<TResult> function);

        public extern Task(Func<object, TResult> function, object state);

        public extern TResult Result
        {
            [H5.Template("getResult()")]
            get;
        }

        public extern Task ContinueWith(Action<Task<TResult>> continuationAction);

        [H5.IgnoreGeneric]
        public extern Task<TNewResult> ContinueWith<TNewResult>(Func<Task<TResult>, TNewResult> continuationFunction);

        public new extern TaskAwaiter<TResult> GetAwaiter();

        public extern void SetResult(TResult result);
    }
}