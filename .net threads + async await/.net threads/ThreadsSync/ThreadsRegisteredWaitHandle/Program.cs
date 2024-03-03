using System;
using System.Threading;

namespace ThreadsRegisteredWaitHandle
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AutoResetEvent auto = new AutoResetEvent(false);

            RegisteredWaitHandle handle = ThreadPool.RegisterWaitForSingleObject(auto, Function, null, 2000, false);

            Console.WriteLine("S - sygnal, Q - exit");

            while (true)
            {
                string operation = Console.ReadKey(true).KeyChar.ToString().ToUpper();

                if (operation == "S")
                    auto.Set();

                if (operation == "Q")
                {
                    handle.Unregister(auto);
                    return;
                }
            }
        }

        private static void Function(object state, bool timeOut)
        {
            Console.WriteLine("Signal");
        }
    }
}
