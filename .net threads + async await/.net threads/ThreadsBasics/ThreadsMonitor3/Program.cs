class Program
{
    private static int _counter;
    private static int _block = 0;

    private static void Function()
    {
        for (int i = 0; i < 50; i++)
        {
            Monitor.Enter((object)_block);

            Console.WriteLine(_counter++);

            Monitor.Exit((object)_block);
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




