using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tesserae
{
    public class SingleSemaphoreSlim
    {
        public SingleSemaphoreSlim()
        {

        }

        private readonly Queue<TaskCompletionSource<bool>> _queue = new Queue<TaskCompletionSource<bool>>();

        public Task WaitAsync()
        {
            var completion = new TaskCompletionSource<bool>();

            _queue.Enqueue(completion);

            if (_queue.Count == 1)
            {
                completion.SetResult(true);
            }

            return completion.Task;
        }

        public void Release()
        {
            if(_queue.Count == 0)
            {
                throw new InvalidOperationException("Nothing to release");
            }

            var completion = _queue.Dequeue();

            if (!completion.Task.IsCompleted && !completion.Task.IsCanceled && !completion.Task.IsFaulted)
            {
                throw new InvalidOperationException("Released wrong semaphore");
            }

            if (_queue.Count > 0)
            {
                _queue.Peek().SetResult(true);
            }
        }
    }
}
