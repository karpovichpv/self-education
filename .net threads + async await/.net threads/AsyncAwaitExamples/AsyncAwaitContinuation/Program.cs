using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwaitContinuation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MyClass my = new MyClass();
            Task task = my.OperationAsync();

            task.ContinueWith(t => Console.WriteLine("\nContinuation of the task"));

            Console.ReadKey();
        }
    }

    internal class MyClass
    {
        public void Operation()
        {
            Thread.Sleep(2000);
            Console.WriteLine("The main thread");
        }

        public async Task OperationAsync()
        {
            await Task.Factory.StartNew(Operation);
        }
    }
}
