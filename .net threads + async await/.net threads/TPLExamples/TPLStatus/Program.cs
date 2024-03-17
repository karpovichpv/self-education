using System;
using System.Threading;
using System.Threading.Tasks;

namespace TPLStatus
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task task = new Task(MyTask);
            Console.WriteLine("1. " + task.Status);

            task.Start();
            Console.WriteLine("2. " + task.Status);

            Thread.Sleep(1000);
            Console.WriteLine("3. " + task.Status);

            Thread.Sleep(3000);
            Console.WriteLine("4. " + task.Status);

            Console.ReadKey();
        }

        private static void MyTask()
        {
            Thread.Sleep(3000);
        }
    }
}
