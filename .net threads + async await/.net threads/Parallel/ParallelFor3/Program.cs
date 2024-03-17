using System;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelFor3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] data = new int[10000000];

            Parallel.For(0, data.Length, i => data[i] = i);
            data[300] = -1;

            Action<int, ParallelLoopState> transform = (int i, ParallelLoopState state) =>
            {
                if (data[i] < 0)
                    state.Break();

                Thread.Sleep(1);

                data[i] = i * i * i / 123;
            };

            ParallelLoopResult loopResult = Parallel.For(0, data.Length, transform);

            if (!loopResult.IsCompleted)
            {
                Console.WriteLine($"\nThe loop is finished. Element {loopResult.LowestBreakIteration}");
            }

            Console.WriteLine("The main thread is finished");
            Console.ReadLine();
        }
    }
}
