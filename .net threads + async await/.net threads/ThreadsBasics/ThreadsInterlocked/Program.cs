class Program
{
    private static long _counter;
    private static object _block = new();

    private static void Procedure()
    {
        for (int i = 0; i < 1000000; i++)
        {
            Interlocked.Increment(ref _counter);

            //lock (_block)
            //{
            //    _counter++;
            //}
        }
    }

    public static void Main()
    {
        Console.WriteLine("Expected counter value equals 10000000");

        Thread[] threads = new Thread[10];

        for (int i = 0; i < 10; ++i)
        {
            (threads[i] = new Thread(Procedure)).Start();
        }

        for (int i = 0; i < 10; ++i)
            threads[i].Join();

        Console.WriteLine($"Real counter value equals {_counter}");

        Console.ReadKey();
    }
}