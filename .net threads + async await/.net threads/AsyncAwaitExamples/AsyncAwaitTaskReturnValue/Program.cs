using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwaitTaskReturnValue
{
    internal class Program
    {//38:02
        static void Main(string[] args)
        {
            MyClass my = new MyClass();
            my.OperationAsync();

            Console.ReadLine();
        }
    }

    internal class MyClass
    {
        int Operation()
        {
            Console.WriteLine($"The operation is executed in the Thread {Thread.CurrentThread.ManagedThreadId}");

            Thread.Sleep(2000);
            return 2 + 2;
        }

        public void OperationAsync()
        {
            Task<int> task = Task<int>.Factory.StartNew(Operation);
            TaskAwaiter<int> awaiter = task.GetAwaiter();
            Action continuation = () => Console.WriteLine($"\nThe operation result is {task.Result}");
            awaiter.OnCompleted(continuation);
        }
    }
}
