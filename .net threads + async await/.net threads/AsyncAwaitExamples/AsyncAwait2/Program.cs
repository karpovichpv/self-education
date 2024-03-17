using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait2
{
    internal class Program
    {//20:20
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

            Console.WriteLine($"OperationAsync (Part I) ThreadId {Thread.CurrentThread.ManagedThreadId}")
            ;
            Task task = new Task(Operation);
            task.Start();
            await task;

            Console.WriteLine($"OperationAsync (Part II) ThreadId {Thread.CurrentThread.ManagedThreadId}");
        }
    }
}
