using System;
using System.Threading;
using System.Threading.Tasks;

namespace TPLWailtAllWaitAny
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task task1 = new Task(() => { MyTask1(2000); });
            Task task2 = new Task(() => { MyTask1(4000); });

            task1.Start();
            task2.Start();

            Console.WriteLine($"Id task1: {task1.Id}");
            Console.WriteLine($"Id task2: {task2.Id}");

            //Task.WaitAll(task1, task2);
            Task.WaitAny(task1, task2);

            Console.WriteLine("The main thread is stopped");

            Console.ReadLine();
        }

        private static void MyTask1(int delay)
        {
            Console.WriteLine($"MyTask: CurrentId {Task.CurrentId} is started");
            Thread.Sleep(delay);
            Console.WriteLine($"MyTask: CurrentId " + Task.CurrentId + " is finished");
        }
    }
}
