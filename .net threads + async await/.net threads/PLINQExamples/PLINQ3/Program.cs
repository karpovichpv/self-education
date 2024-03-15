using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PLINQ3
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

            CancellationTokenSource source = new CancellationTokenSource();
            System.Threading.CancellationToken token = source.Token;
            ParallelQuery<int> negatives = array
                .AsParallel<int>()
                .WithCancellation(token)
                .Where(e => e < 0)
                .Select(e => e);

            source.CancelAfter(10000);


            try
            {
                foreach (int element in negatives)
                    Console.Write(element + " ");

                Console.WriteLine("Enumeration is successfully finished!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message);
            }
            finally
            {
                source.Dispose();
            }

            Console.ReadLine();
        }
    }
}
