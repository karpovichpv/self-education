using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwaitSendArgumentReturnValue
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MyClass my = new MyClass();
            Task<double> task = my.OperationAsync(3);
            Console.WriteLine($"result: {task.Result}");

            Console.Read();
        }
    }

    internal class MyClass
    {
        private double Operation(object argument)
        {
            Thread.Sleep(2000);
            return (double)argument * (double)argument;
        }

        public async Task<double> OperationAsync(double arg)
        {
            return await Task<double>.Factory.StartNew(Operation, arg);
        }
    }
}
