using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ParallelFor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] data = new int[100000000];

            Stopwatch timer = new Stopwatch();

            timer.Start();

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = i * i * i / 123;
            }

            timer.Stop();
            Console.WriteLine($"The ordinary loop for: {timer.ElapsedTicks}");
            timer.Reset();

            Action<int> transform = (int i) => { data[i] = i * i * i / 123; };

            timer.Start();

            Parallel.For(0, data.Length, transform);
            timer.Stop();
            Console.WriteLine($"The parallel loop for: {timer.ElapsedTicks}");

            Console.Read();
        }
    }
}
