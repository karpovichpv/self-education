using System;
using System.Threading;

namespace AsyncDelegateEndInvoke2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Func<int, int, int> myDelegate = new Func<int, int, int>(Sum);

            IAsyncResult asyncResult = myDelegate.BeginInvoke(1, 2, null, null);

            Console.WriteLine("The primary thread is running");
            int result = myDelegate.EndInvoke(asyncResult);

            Console.WriteLine($"Result {result}");

            Console.ReadKey();
        }

        static int Sum(int a, int b)
        {
            Thread.Sleep(5000);
            return a + b;
        }
    }
}
