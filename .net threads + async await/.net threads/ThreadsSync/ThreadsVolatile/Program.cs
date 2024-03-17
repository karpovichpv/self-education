using System;
using System.Threading;

namespace ThreadsVolatile
{
    internal class Program
    {
        private static volatile byte _stop;

        static void Main(string[] args)
        {
            Thread thread = new Thread(Function);
            thread.Start();

            Thread.Sleep(2000);

            Thread.VolatileWrite(ref _stop, 1);
            Console.WriteLine($"Main: waiting while the secondary thread will be finished");
            thread.Join();

            Console.ReadLine();
        }

        private static void Function()
        {
            int x = 0;
            while (Thread.VolatileRead(ref _stop) != 1)
            {
                x++;
            }
            Console.WriteLine($"Thread is finished when {x}");
        }
    }
}
