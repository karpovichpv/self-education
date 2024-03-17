using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelForEach2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IList<Element> elements = new List<Element>();

            Action<int> initialize = (i) => elements.Add(new Element() { A = i });
            Parallel.For(0, 10000, initialize);
            elements[300].A = -1;

            Action<Element, ParallelLoopState> transform = (Element el, ParallelLoopState state) =>
            {
                if (el.A < 0)
                    state.Break();

                Thread.Sleep(10);

                el.A = 111 * 222 * 333 / 444;
            };

            ParallelLoopResult result = Parallel.ForEach(elements, transform);

            if (!result.IsCompleted)
            {
                Console.WriteLine($"Loop is broken when i was {result.LowestBreakIteration}");
            }

            Console.ReadLine();
        }
    }

    internal class Element
    {
        public int A { get; set; }
    }
}
