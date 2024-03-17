using System;
using System.Threading;

namespace ThreadsVolatile
{
    internal class Program
    {
        private static volatile bool _stop;

        static void Main(string[] args)
        {
            Thread thread = new Thread(Function);
            thread.Start();

            Thread.Sleep(2000);

            _stop = true;
            Console.WriteLine($"Main: waiting while the secondary thread will be finished");
            thread.Join();

            Console.ReadLine();
        }

        private static void Function()
        {
            int x = 0;
            while (!_stop)
            {
                x++;
            }
            Console.WriteLine($"Thread is finished when {x}");
        }
    }
}
