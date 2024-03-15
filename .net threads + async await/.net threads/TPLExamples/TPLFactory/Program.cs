using System;
using System.Threading;
using System.Threading.Tasks;

namespace TPLFactory
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task task = Task.Factory.StartNew(MyTask);

            Console.ReadKey();
        }

        private static void MyTask()
        {
            for (int i = 0; i < 80; i++)
            {
                Thread.Sleep(20);
                Console.Write(".");
            }
        }
    }
}
