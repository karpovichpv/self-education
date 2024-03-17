using System;
using System.Threading;
using System.Threading.Tasks;

namespace TPLCancellationToken
{
    internal class Program
    {//53:26
        static void Main(string[] args)
        {
            Console.WriteLine("The main thread is started");

            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            Task task = new Task(MyTask, token);
            task.Start();

            Thread.Sleep(2000);

            try
            {
                source.Cancel();
                task.Wait();
            }
            catch (AggregateException e)
            {
                Console.WriteLine($"Exception : " + e.GetType());
                Console.WriteLine($"Message : " + e.Message);

                if (e.InnerException != null)
                    Console.WriteLine("Inner Exception : " + e.InnerException.ToString());
            }

            Console.Read();
        }

        private static void MyTask(object arg)
        {
            CancellationToken token = (CancellationToken)arg;

            token.ThrowIfCancellationRequested();

            Console.WriteLine("MyTask is started");

            for (int i = 0; i < 80; i++)
            {
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("\nIs got the request for the cancellation of the task.");
                    token.ThrowIfCancellationRequested();
                }

                Thread.Sleep(100);
                Console.Write(".");
            }
        }
    }
}
