using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ParallelFor2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] data = new int[100000000];

            Stopwatch timer = new Stopwatch();

            timer.Start();

            Parallel.For(0, data.Length, i => data[i] = i);
            timer.Stop();
            Console.WriteLine($"The parallel initialization for: {timer.ElapsedTicks}");
            timer.Reset();

            timer.Start();

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = i;
            }

            Console.WriteLine($"The ordynary initialization for: {timer.ElapsedTicks}");
            timer.Reset();

            timer.Start();

            Parallel.For(0, data.Length, i => data[i] = i * i * i / 123);
            timer.Stop();
            Console.WriteLine($"The parallel modifying for: {timer.ElapsedTicks}");
            timer.Reset();

            timer.Start();

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = i * i * i / 123;
            }

            Console.WriteLine($"The ordynary modyfying for: {timer.ElapsedTicks}");
            timer.Reset();
            Console.Read();
        }
    }
}
