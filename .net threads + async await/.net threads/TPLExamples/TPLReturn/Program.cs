using System;
using System.Threading;
using System.Threading.Tasks;

namespace TPLReturn
{
    struct Context
    {
        public int a;
        public int b;
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("The main thread is started");
            Context context;
            context.a = 2;
            context.b = 3;

            Task<int> task;

            // Example 1
            //task = new Task<int>(Sum, context);
            //task.Start();

            // Example 2
            //TaskFactory<int> factory = new TaskFactory<int>();
            //task = factory.StartNew(Sum, context);

            // Example 3
            task = Task<int>.Factory.StartNew(Sum, context);

            Console.WriteLine($"Result {task.Result}");
            Console.WriteLine("The main thread is finished");

            Console.ReadLine();
        }

        private static int Sum(object arg)
        {
            int a = ((Context)arg).a;
            int b = ((Context)arg).b;

            Thread.Sleep(2000);

            return a + b;
        }
    }
}
