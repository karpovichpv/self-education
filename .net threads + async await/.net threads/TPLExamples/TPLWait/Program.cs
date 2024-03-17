using System;
using System.Threading;
using System.Threading.Tasks;

namespace TPLWait
{
    internal class Program
    {//28.37
        static void Main(string[] args)
        {
            Task task = new Task(MyTask);
            task.Start();

            Thread.Sleep(500);

            Console.WriteLine($"\ntask.IsCompleted = {task.IsCompleted}");

            // Example 1
            // task.Wait();

            // Example 2
            // while (!task.IsCompleted)
            // Thread.Sleep(100);

            // Example 3
            IAsyncResult asyncResult = task as IAsyncResult;
            ManualResetEvent waitHandle = asyncResult.AsyncWaitHandle as ManualResetEvent;
            waitHandle.WaitOne();

            Console.WriteLine($"\ntask.IsCompleted = {task.IsCompleted}");

            // Delay
            Console.ReadKey();
        }

        private static void MyTask()
        {
            for (int i = 0; i < 80; i++)
            {
                Thread.Sleep(25);
                Console.Write(".");
            }
        }
    }
}
