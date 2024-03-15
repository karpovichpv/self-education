using System;
using System.Threading;
using System.Threading.Tasks;

namespace TPLLambda
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("The main thread is started");

            Task task = Task.Factory.StartNew(new Action(() =>
            {
                for (int i = 0; i < 80; i++)
                {
                    Thread.Sleep(20);
                    Console.Write(".");
                }
            }));

            task.Wait();

            task.Dispose();

            Console.WriteLine("The main thread is finished");

            Console.ReadKey();
        }
    }
}
