using System;
using System.Threading;

namespace ThreadsAutoResetEvent
{
    internal class Program
    {
        private static AutoResetEvent _auto = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            Thread thread = new Thread(Function);
            thread.Start();
            Thread.Sleep(500);

            Console.WriteLine("Push any button for setting of AutoResetEvent into signal state");
            Console.ReadKey();
            _auto.Set();

            Console.WriteLine("Push any buttom for setting of AutoResetEvent into signal state");
            Console.ReadKey();
            _auto.Set();

            Console.ReadKey();
        }

        private static void Function()
        {
            Console.WriteLine("Red color");
            _auto.WaitOne(); // Stopping of the secondary thread execution

            Console.WriteLine("Yellow color");
            _auto.WaitOne(); // Stopping of the secondary thread execution

            Console.WriteLine("Green color");
        }
    }
}
