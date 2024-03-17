using System;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"The main thread is started");
            Task task = new Task(MyTask);

            try
            {
                task.Start();
                task.Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception : " + ex.GetType());
                Console.WriteLine($"Message : " + ex.Message);

                if (ex.InnerException != null)
                    Console.WriteLine("Inner Exception : " + ex.InnerException.ToString());
            }
            finally
            {
                Console.WriteLine($"Task status: {task.Status}");
            }

            Console.WriteLine("The main thread is finished");

            Console.ReadKey();
        }

        private static void MyTask()
        {
            Console.WriteLine("Task is started");

            throw new Exception();

            Console.WriteLine("Task is finished");
        }
    }
}
