using System;
using System.Linq;
using System.Threading.Tasks;

namespace PLINQ2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] array = new int[100000000];

            Parallel.For(0, 10000000, (i) => array[i] = i);

            array[1000] = -1;
            array[14000] = -2;
            array[15000] = -3;
            array[1335000] = -4;
            array[12335000] = -5;
            array[88888880] = -5;

            ParallelQuery<int> negatives = array.AsParallel<int>().AsOrdered().Where(e => e < 0).Select(e => e);

            foreach (int element in negatives)
                Console.Write(element + " ");

            Console.ReadLine();
        }
    }
}
