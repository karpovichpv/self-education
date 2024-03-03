using System;
using System.Threading;

namespace ThreadsMutex2
{
    internal class Program
    {
        private static Mutex _mutex = new Mutex();

        static void Main(string[] args)
        {
            Thread thread = new Thread(Method1);
            thread.Start();
            thread.Join();

            Console.ReadKey();
        }

        private static void Method1()
        {
            _mutex.WaitOne();
            Console.WriteLine($"Method1 Start {Thread.CurrentThread.ManagedThreadId}");
            Method2();
            _mutex.ReleaseMutex();
            Console.WriteLine("Method1 Finish " + Thread.CurrentThread.ManagedThreadId);
        }

        private static void Method2()
        {
            _mutex.WaitOne();
            Console.WriteLine($"Method2 Start {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(1000);
            _mutex.ReleaseMutex();
            Console.WriteLine($"Method2 Finish {Thread.CurrentThread.ManagedThreadId}");
        }
    }
}
