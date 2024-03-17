using System;
using System.Threading;

namespace ThreadsAutoResetEvent
{
    internal class Program
    {
        private static ManualResetEvent _manual = new ManualResetEvent(false);

        private static void Function()
        {
            Console.WriteLine($"Thread is started {Thread.CurrentThread.Name}");

            for (int i = 0; i < 80; i++)
            {
                Console.Write(".");
                Thread.Sleep(100);
            }

            Console.WriteLine($"\r\nThread is finished {Thread.CurrentThread.Name}");

            _manual.Set();
        }

        public static void Main()
        {
            Thread thread = new Thread(Function) { Name = "1" };
            thread.Start();

            Console.WriteLine("Stopping of the execution of the primary thread.");
            _manual.WaitOne();

            Console.WriteLine("Primary thread returns to work");
            Thread thread2 = new Thread(Function) { Name = "2" };
            thread2.Start();

            _manual.Reset();

            Console.WriteLine("Stopping of the execution of the primary thread");
            _manual.WaitOne();

            Console.WriteLine("Primary thread returns to work");
            Console.ReadLine();
        }
    }
}
