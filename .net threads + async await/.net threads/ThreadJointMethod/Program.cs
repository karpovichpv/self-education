class Program
{
    public static void Main(string[] args)
    {
        // Create of a new thread
        Thread thread = new(new ThreadStart(Function));
        thread.Start();

        // The primary thread is waiting while the secondary will be finished
        thread.Join();

        Console.ForegroundColor = ConsoleColor.Green;

        for (int i = 0; i < 160; i++)
        {
            Thread.Sleep(20);
            Console.Write("-");
        }

        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("\nPrimary thread was finished");
    }

    private static void Function()
    {
        Console.WriteLine($"ID of the secondary thread: {Thread.CurrentThread.ManagedThreadId}");
        Console.ForegroundColor = ConsoleColor.DarkYellow;

        for (int i = 0; i < 160; i++)
        {
            Thread.Sleep(20);
            Console.Write(".");
        }

        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("The secondary thread was finished");
    }
}