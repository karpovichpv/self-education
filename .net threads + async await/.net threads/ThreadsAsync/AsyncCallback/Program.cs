using System;
using System.Threading;

namespace AsyncCallbackExample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"The primary thread {Thread.CurrentThread.ManagedThreadId}");

            Action action = new Action(Method);

            // Delegate that will be invoked when the asynchronous operation will finished
            AsyncCallback callback = new AsyncCallback(CallBack);

            action.BeginInvoke(callback, null);

            Console.WriteLine("The primary thread is continuing the work");

            Console.Read();
        }

        private static void Method()
        {
            Console.WriteLine($"\nThe secondary thread: Id {Thread.CurrentThread.ManagedThreadId}");

            for (int i = 0; i < 80; i++)
            {
                Thread.Sleep(20);
                Console.Write(".");
            }

            Console.WriteLine("The secondary thread is finished. \n");
        }

        private static void CallBack(IAsyncResult asyncResult)
        {
            Console.WriteLine($"Callback method. Thread Id {Thread.CurrentThread.Name}");
        }
    }
}
