using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwaitReturnValue
{
    internal class Program
    {//52:43
        static void Main(string[] args)
        {
            MyClass my = new MyClass();
            Task<int> task = my.OperationAsync();

            task.ContinueWith(t => Console.WriteLine($"Result : {t.Result}"));

            Console.ReadLine();
        }
    }

    public class MyClass
    {
        private int Operation()
        {
            Thread.Sleep(2000);
            return 2 + 2;
        }

        public async Task<int> OperationAsync()
        {
            return await Task<int>.Factory.StartNew(Operation);
        }
    }
}
