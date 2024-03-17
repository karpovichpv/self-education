using System;
using System.Threading;
using System.Threading.Tasks;

namespace TPLContinuation
{//40:44
    internal class Program
    {
        static void Main(string[] args)
        {
            Action action = new Action(MyTask);
            Task task = new Task(action);

            Action<Task> continuation = new Action<Task>(ContinuationTask);
            Task taskContinuation = task.ContinueWith(continuation);

            task.Start();

            Console.ReadKey();
        }

        private static void MyTask()
        {
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(200);
                Console.Write("+");
            }
        }

        private static void ContinuationTask(Task task)
        {
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(200);
                Console.Write("-");
            }
        }
    }
}
