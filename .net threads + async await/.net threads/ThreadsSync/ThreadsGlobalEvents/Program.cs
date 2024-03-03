using System;
using System.Threading;

namespace ConsoleApp1
{
    internal class Program
    {
        private static EventWaitHandle _handle = null;

        static void Main(string[] args)
        {
            _handle = new EventWaitHandle(false, EventResetMode.ManualReset, "GlobalEvent::GUID");

            Thread thread = new Thread(Function) { IsBackground = true };
            thread.Start();

            Console.WriteLine("Push any button for starting the application");
            Console.ReadKey();
            _handle.Set();

            Console.ReadKey();
        }

        private static void Function()
        {
            _handle.WaitOne();

            while (true)
            {
                Console.WriteLine("Hello world!");
                Thread.Sleep(300);
            }
        }
    }
}
