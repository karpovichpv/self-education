using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Main ThreadID {Thread.CurrentThread.ManagedThreadId}");
            MyClass my = new MyClass();
            my.OperationAsync();

            Console.WriteLine("The main thread finished its work");
            Console.ReadKey();
        }
    }

    internal class MyClass
    {
        public void Operation()
        {
            Console.WriteLine($"Operation ThreadID{Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine("Begin");
            for (int i = 0; i < 80; i++)
            {
                Console.Write("+");
                Thread.Sleep(100);
            }
            Console.WriteLine("End");
            Console.WriteLine("The secondary thread finished its work");
        }

        public async void OperationAsync()
        {
            Task task = new Task(Operation);
            task.Start();
            await task;
        }
    }
}
