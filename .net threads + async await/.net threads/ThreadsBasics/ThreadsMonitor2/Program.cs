class Program
{
    // Object for blocking
    private static object _block = new object();

    // Counter for threads
    private static int _counter;
    private static Random _random = new();

    private static void Function()
    {
        lock (_block)
        {
            _counter++;
        }

        int time = _random.Next(1000, 12000);
        Thread.Sleep(time);

        lock (_block)
        {
            _counter--;
        }
    }

    private static void Report()
    {
        while (true)
        {
            int count;

            try
            {
                Monitor.Enter(_block);
                count = _counter;
            }
            finally
            {
                Monitor.Exit(_block);
            }

            Console.WriteLine($"{count} threads are active");
            Thread.Sleep(100);
        }
    }

    public static void Main()
    {
        var reporter = new Thread(Report) { IsBackground = true };
        reporter.Start();

        var threads = new Thread[150];

        for (uint i = 0; i < 150; ++i)
        {
            threads[i] = new Thread(Function);
            threads[i].Start();
        }

        Thread.Sleep(15000);
    }
}




