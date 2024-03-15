using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    internal class DelayTaskScheduler : TaskScheduler
    {
        private readonly Queue<Task> _queue = new Queue<Task>();
        private readonly AutoResetEvent _auto = new AutoResetEvent(false);

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            throw new System.NotImplementedException();
        }

        protected override void QueueTask(Task task)
        {
            Console.WriteLine($"QueueTask ThreadId {Thread.CurrentThread.ManagedThreadId}");
            _queue.Enqueue(task);

            WaitOrTimerCallback callback = (object state, bool timedOut)
                => base.TryExecuteTask(_queue.Dequeue());

            ThreadPool.RegisterWaitForSingleObject(_auto, callback, null, 2000, true);
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return false;
        }
    }
}
