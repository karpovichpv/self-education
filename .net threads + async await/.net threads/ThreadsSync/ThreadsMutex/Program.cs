using System;
using System.Threading;

namespace ThreadsMutex
{
    internal class Program
    {
        private static Mutex _mutex = new Mutex(false, "MyMutex");

        static void Main(string[] args)
        {
            Thread[] threads = new Thread[5];

            for (int i = 0; i < 5; i++)
            {
                threads[i] = new Thread(Function);
                threads[i].Name = i.ToString();
                Thread.Sleep(5000);
                threads[i].Start();
            }

            Console.ReadKey();
        }

        private static void Function()
        {
            _mutex.WaitOne();

            Console.WriteLine($"Thread {Thread.CurrentThread.Name} went into a protected area");
            Thread.Sleep(2000);
            Console.WriteLine($"Thread {Thread.CurrentThread.Name} left the protected area");

            _mutex.ReleaseMutex();
        }
    }
}
