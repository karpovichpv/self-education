using System;
using System.Threading;
using System.Threading.Tasks;

namespace TPLAsyncState
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Action<object> myTask = MyTask;
            Task task = new Task(myTask, ".");
            task.Start();

            Thread.Sleep(500);

            Console.WriteLine($"\n[{task.AsyncState as string}]");

            Console.ReadKey();
        }

        private static void MyTask(object arg)
        {
            for (int i = 0; i < 80; i++)
            {
                Thread.Sleep(25);
                Console.Write(arg as string);
            }
        }
    }
}
