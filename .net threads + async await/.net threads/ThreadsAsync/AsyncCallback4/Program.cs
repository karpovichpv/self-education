using System;
using System.Threading;

namespace AsyncCallback4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"The primary thread: Id {Thread.CurrentThread.ManagedThreadId}");

            var func = new Func<int, int, int>(Sum);
            func.BeginInvoke(1, 2, CallBack, func);

            Console.WriteLine("The primary thread finished its work");

            Console.Read();
        }

        private static void CallBack(IAsyncResult ar)
        {
            Func<int, int, int> caller = ar.AsyncState as Func<int, int, int>;
            int sum = caller.EndInvoke(ar);
            Console.WriteLine($"a + b = {sum}");
        }

        private static int Sum(int arg1, int arg2)
        {
            Console.WriteLine($"The secondary thread: Id {Thread.CurrentThread.ManagedThreadId}");

            Thread.Sleep(3000);
            return arg1 + arg2;
        }
    }
}
