using System;
using System.Threading;

namespace ThreadsSemaphoreSlim
{
    internal class Program
    {
        private static SemaphoreSlim _pool;

        static void Main(string[] args)
        {
            Thread.Sleep(5000);

            _pool = new SemaphoreSlim(2, 7);
            // _pool.Release(2);

            for (int i = 1; i <= 40; i++)
            {
                new Thread(Function).Start(i);
                Thread.Sleep(500);
            }

            Console.ReadLine();
        }

        private static void Function(object number)
        {
            _pool.Wait();

            Console.WriteLine($"Thread {number} occupy a semaphore place");
            Thread.Sleep(2000);
            Console.WriteLine($"Thread {number} ==========>> realese the place");

            _pool.Release();
        }
    }
}
