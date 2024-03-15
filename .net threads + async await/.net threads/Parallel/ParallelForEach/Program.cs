using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ParallelForEach
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IList<Element> elements = new List<Element>();

            for (int i = 0; i < 10000000; i++)
                elements.Add(new Element() { A = i });

            Stopwatch timer = new Stopwatch();
            timer.Start();

            foreach (Element element in elements)
                element.A = 111 * 222 * 333 / 444;

            timer.Stop();
            Console.WriteLine($"The ordinary loop foreach {timer.ElapsedTicks}");
            timer.Reset();

            timer.Start();

            Parallel.ForEach(elements, element => element.A = 111 * 222 * 333 / 444);

            timer.Stop();
            Console.WriteLine($"The parallel loop foreach {timer.ElapsedTicks}");
            timer.Reset();

            Console.ReadLine();
        }
    }

    internal class Element
    {
        public int A { get; set; }
    }
}
