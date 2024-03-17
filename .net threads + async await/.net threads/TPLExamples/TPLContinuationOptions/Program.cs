using System;
using System.Threading.Tasks;

namespace ContinuationOptions
{
    internal class Program
    {//1:03:09
        static void Main(string[] args)
        {
            Task<int> task = new Task<int>(MyTask);
            Action<Task<int>> continuation;

            continuation = t => Console.WriteLine($"Result : {task.Result}");
            task.ContinueWith(continuation, TaskContinuationOptions.OnlyOnRanToCompletion);

            continuation = t => Console.WriteLine($"Inner Exception : {task.Exception.InnerException.Message}");
            task.ContinueWith(continuation, TaskContinuationOptions.OnlyOnFaulted);

            task.Start();

            Console.ReadLine();
        }

        private static int MyTask()
        {
            byte result = 255;

            //checked
            {
                result += 1;
            }

            return result;
        }
    }
}
