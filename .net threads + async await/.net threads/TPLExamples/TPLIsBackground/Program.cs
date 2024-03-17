using System;
using System.Threading;
using System.Threading.Tasks;

namespace TPLIsBackground
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task task = new Task(MyTask);
            task.Start();

            Thread.Sleep(500);

            Console.WriteLine("The main method is finished");
            Console.ReadLine();
        }

        private static void MyTask()
        {
            Thread.CurrentThread.IsBackground = false;
            for (int i = 0; i < 80; i++)
            {
                Thread.Sleep(100);
                Console.Write(".");
            }
        }
    }
}
