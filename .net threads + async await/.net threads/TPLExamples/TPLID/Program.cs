using System;
using System.Threading;
using System.Threading.Tasks;

namespace TPLID
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string taskId = Task.CurrentId == null
                ? "null"
                : Task.CurrentId.ToString();

            Console.WriteLine($"Main: Task.CurrentId" +
                $" {taskId} ");

            Task task1 = new Task(MyTask);
            Task task2 = new Task(MyTask);

            task1.Start();
            task2.Start();

            Console.ReadLine();
        }

        private static void MyTask()
        {
            Console.WriteLine($"MyTask: CurrentId {Task.CurrentId} with the ManagedThreadId {Thread.CurrentThread.ManagedThreadId}");

            Thread.Sleep(2000);

            Console.WriteLine($"MyTask: CurrentId {Task.CurrentId} is finished");
        }
    }
}
