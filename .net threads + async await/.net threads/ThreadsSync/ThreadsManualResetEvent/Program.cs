using System;
using System.Threading;

namespace ThreadsAutoResetEvent
{
    internal class Program
    {
        private static ManualResetEvent _manual = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            new Thread(Function1).Start();
            new Thread(Function2).Start();

            Thread.Sleep(500);

            Console.WriteLine("Push any button for setting of ManualResetEvent into signal state");
            Console.ReadKey();
            _manual.Set();
            //_auto.Set();

            Console.ReadKey();
        }

        private static void Function1()
        {
            Console.WriteLine("Thread 1 is started and waiting a signal.");
            _manual.WaitOne();
            Console.WriteLine("Thread 1 is finishing.");
        }

        private static void Function2()
        {
            Console.WriteLine("Thread 2 is started and waiting a signal.");
            _manual.WaitOne();
            Console.WriteLine("Thread 2 is finishing.");
        }
    }
}
