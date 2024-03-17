using System;
using System.Threading;

namespace ThreadsTimer
{
    internal class Program
    {
        private static int _maxCount = 10;
        private static int _counter;


        private static void Function(Object state)
        {
            Console.WriteLine($"Invoking of the method" +
                $" {++_counter}");

            if (_counter == _maxCount)
            {
                _counter = 0;
                (state as AutoResetEvent).Set();
            }
        }

        public static void Main()
        {
            AutoResetEvent auto = new AutoResetEvent(false);
            TimerCallback callback = new TimerCallback(Function);

            Console.WriteLine("Timer timespan is 1/10 of the second");

            Timer timer = new Timer(callback, auto, 1000, 100);

            auto.WaitOne();

            Console.WriteLine("\nWorking timespan of the timer 1/2 of the second");

            timer.Change(0, 500);
            auto.WaitOne();

            Console.WriteLine("\nFinalization of the timer.");
            timer.Dispose();

            // Delay
            Console.ReadKey();
        }
    }
}
