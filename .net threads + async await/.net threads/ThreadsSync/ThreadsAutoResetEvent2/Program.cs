using System;
using System.Threading;

namespace ThreadsAutoResetEvent
{
    internal class Program
    {
        private static AutoResetEvent _auto = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            new Thread(Function1).Start();
            new Thread(Function2).Start();

            Thread.Sleep(500);

            Console.WriteLine("Push any button for setting of AutoResetEvent into signal state");
            Console.ReadKey();
            _auto.Set();
            //_auto.Set();

            Console.ReadKey();
        }

        private static void Function1()
        {
            Console.WriteLine("Thread 1 is started and waiting a signal.");
            _auto.WaitOne();
            Console.WriteLine("Thread 1 is finishing.");
        }

        private static void Function2()
        {
            Console.WriteLine("Thread 2 is started and waiting a signal.");
            _auto.WaitOne();
            Console.WriteLine("Thread 2 is finishing.");
        }
    }
}
