using System;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace AsyncCallback3
{
    internal class Program
    {
        public static void Main()
        {
            Console.WriteLine($"The primary thread: ID {Thread.CurrentThread.ManagedThreadId}");
            Func<int, int, int> func = new Func<int, int, int>(Sum);

            func.BeginInvoke(1, 2, CallBack, "a+b={0}");
            Console.WriteLine("The primary thread continues working");

            Console.ReadLine();
        }

        private static int Sum(int a, int b)
        {
            Console.WriteLine($"\nThe secondary thread: Id {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(4000);

            return a + b;
        }

        private static void CallBack(IAsyncResult asyncResult)
        {
            AsyncResult result = asyncResult as AsyncResult;
            Func<int, int, int> caller = (Func<int, int, int>)result.AsyncDelegate;

            int sum = caller.EndInvoke(asyncResult);

            string resultString = string.Format(asyncResult.AsyncState.ToString(), sum);
            Console.WriteLine($"Result of the asynchronous operation is {resultString}");
        }
    }
}
