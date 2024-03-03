using System;
using System.Threading;

namespace ThreadPoolExample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start");
            Report();

            ThreadPool.QueueUserWorkItem(new WaitCallback(Task1));
            Report();

            ThreadPool.QueueUserWorkItem(Task2);
            Report();

            Thread.Sleep(3000);
            Console.WriteLine("End");
            Console.Read();
        }

        private static void Task1(Object state)
        {
            Thread.CurrentThread.Name = "1";
            Console.WriteLine($"Thread #{Thread.CurrentThread.Name}");
            Thread.Sleep(2000);
            Console.WriteLine($"Thread finished it'w work {Thread.CurrentThread.Name}");
        }

        private static void Task2(Object state)
        {
            Thread.CurrentThread.Name = "2";
            Console.WriteLine($"Thread #{Thread.CurrentThread.Name}");
            Thread.Sleep(2000);
            Console.WriteLine($"Thread finished it's work {Thread.CurrentThread.Name}");
        }

        private static void Report()
        {
            Thread.Sleep(200);
            int availableWorkThreads;
            int availableIOThreads;
            int maxWorkThreads;
            int maxIOThreads;

            ThreadPool.GetAvailableThreads(out availableWorkThreads, out availableIOThreads);
            ThreadPool.GetMaxThreads(out maxWorkThreads, out maxIOThreads);

            Console.WriteLine($"Available working threads in pool : {availableWorkThreads}, {maxWorkThreads}");
            Console.WriteLine($"Available io threads in pool : {availableIOThreads}, {maxIOThreads}");
        }
    }
}
