using System;
using System.Threading;

namespace AsyncAwaitHandle
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Func<int, int, int> myDelegate = new Func<int, int, int>(Sum);
            IAsyncResult asyncResult = myDelegate.BeginInvoke(1, 2, null, null);

            Console.WriteLine("The asyncronous method is started. The main method is beginning its work");
            Console.WriteLine("The main method is waiting when the asyncronous task will be finished");

            Console.WriteLine(asyncResult.AsyncWaitHandle.GetType()); // What type of the syncronization object
            asyncResult.AsyncWaitHandle.WaitOne(); // Stopping of the primary thread

            Console.WriteLine("Asyncronous method is finished. The main method is continuing its work");

            int result = myDelegate.EndInvoke(asyncResult);
            Console.WriteLine("Result = " + result);

            Console.ReadKey();
        }

        private static int Sum(int a, int b)
        {
            Thread.Sleep(3000);
            return a + b;
        }
    }
}
