using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    internal class Program
    {//1:23:49
        static void Main(string[] args)
        {
            Console.WriteLine($"Main ThreadId {Thread.CurrentThread.ManagedThreadId}");

            List<Task> tasks = new List<Task>();
            TaskScheduler scheduler = new DelayTaskScheduler();
            TaskFactory factory = new TaskFactory(scheduler);
            tasks.Add(factory.StartNew(MyTask1));
            tasks.Add(factory.StartNew(MyTask2));

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine("\nAll tasks finished");
        }

        private static void MyTask1()
        {
            Console.WriteLine($"MyTask1 ThreadId {Thread.CurrentThread.ManagedThreadId}");

            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(200);
                Console.Write("+ ");
            }
        }

        private static void MyTask2()
        {
            Console.WriteLine($"MyTask2 ThreadId {Thread.CurrentThread.ManagedThreadId}");

            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(200);
                Console.Write("- ");
            }
        }
    }
}
