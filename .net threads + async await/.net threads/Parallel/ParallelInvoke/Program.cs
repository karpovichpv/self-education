using System;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelInvoke
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("The main thread is finished.");
            ParallelOptions options = new ParallelOptions();

            options.MaxDegreeOfParallelism = 2;
            Console.WriteLine($"Quantity of logical threads of CPU: {Environment.ProcessorCount}");

            Console.ReadKey();

            Parallel.Invoke(options, MyTask1, MyTask2);
            Parallel.Invoke(options, MyTask1, MyTask2, MyTask1, MyTask2);

            Console.WriteLine("The main thread is finished");

            Console.ReadKey();
        }

        private static void MyTask1()
        {
            Console.WriteLine($"MyTask1: is started");
            for (int i = 0; i < 80; i++)
            {
                Thread.Sleep(10);
                Console.Write("+");
            }
            Console.WriteLine("MyTask1: is finished");
        }

        private static void MyTask2()
        {
            Console.WriteLine($"MyTask2: is started");
            for (int i = 0; i < 80; i++)
            {
                Thread.Sleep(10);
                Console.Write("-");
            }
            Console.WriteLine("MyTask2: is finished");
        }
    }
}
