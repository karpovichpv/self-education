using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace TPLDelayTaskSchedual
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Main ThreadId {Thread.CurrentThread.ManagedThreadId}");

            TaskScheduler scheduler = new DelayTaskScheduler();
            TaskFactory factory = new TaskFactory(scheduler);
            Task task = factory.StartNew(MyTask);

            TaskAwaiter awaiter = task.GetAwaiter();

            while (!awaiter.IsCompleted)
            {
                Console.Write(".");
                Thread.Sleep(100);
            }

            Console.Read();
        }

        private static void MyTask()
        {
            Console.WriteLine($"MyTask ThreadID {Thread.CurrentThread.ManagedThreadId}");

            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(200);
                Console.Write("+ ");
            }
        }
    }
}
