using System;
using System.Threading;

namespace ThreadsWaitHandle
{
    internal class Program
    {
        private static WaitHandle[] events = new WaitHandle[]
        {
            new AutoResetEvent(false),
            new AutoResetEvent(false)
        };

        static void Main(string[] args)
        {
            ThreadPool.QueueUserWorkItem(Task1, events[0]);
            ThreadPool.QueueUserWorkItem(Task2, events[1]);

            Console.WriteLine("Waiting for finishing of the both tasks.");
            WaitHandle.WaitAll(events);

            ThreadPool.QueueUserWorkItem(Task1, events[0]);
            ThreadPool.QueueUserWorkItem(Task2, events[1]);

            Console.WriteLine("\nWaiting for finishing of a one task.");
            int index = WaitHandle.WaitAny(events);
            Console.WriteLine($"\nTask {index + 1} finished the first");

            Console.ReadLine();
        }

        private static void Task1(Object state)
        {
            for (int i = 0; i < 5; i++)
            {
                Console.Write($"1 ");
                Thread.Sleep(500);
            }
            (state as AutoResetEvent).Set();
        }

        private static void Task2(Object state)
        {
            for (int i = 0; i < 10; i++)
            {
                Console.Write("2 ");
                Thread.Sleep(500);
            }
            (state as AutoResetEvent).Set();
        }
    }
}
