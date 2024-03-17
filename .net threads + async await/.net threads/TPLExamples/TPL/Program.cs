using System;
using System.Threading;
using System.Threading.Tasks;

namespace TPL
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;

            Console.WriteLine($"Main: is started in the thread # {threadId}");

            Action action = new Action(MyTask);

            Task task = new Task(action);
            task.Start();

            for (int i = 0; i < 10; i++)
            {
                Console.Write(". ");
                Thread.Sleep(200);
            }

            Console.WriteLine($"\nThe main thread is finished in the thread # {threadId}");

            Console.ReadKey();
        }

        private static void MyTask()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;

            Console.WriteLine($"\nMyTask: is started in the thread # {threadId}");

            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(200);
                Console.Write("+ ");
            }

            Console.WriteLine($"\nMyTask: is finished in the thread # {threadId}");
        }
    }
}
