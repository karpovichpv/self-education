
public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine($"Primary thread # {Thread.CurrentThread.GetHashCode()}");

        // Creating of secondary thread
        Thread thread = new(Method2);
        thread.Start();
        thread.Join();

        WriteChar('1', 80, ConsoleColor.Green);

        Console.WriteLine($"Primary thread was finished");

        // Delay
        Console.ReadKey();
    }

    private static void Method3()
    {
        Console.WriteLine($"Tertiary thread #{Thread.CurrentThread.GetHashCode()}");
        WriteChar('3', 80, ConsoleColor.Yellow);
        Console.WriteLine("Tertiary thread was finished");
    }

    private static void Method2()
    {
        Console.WriteLine($"Secondary thread #{Thread.CurrentThread.GetHashCode()}");
        WriteChar('2', 80, ConsoleColor.Blue);

        var thread = new Thread(Method3);
        thread.Start();
        thread.Join();

        Console.WriteLine($"Secondary thread was finished");
    }

    private static void WriteChar(char @char, int count, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        for (int i = 0; i < count; i++)
        {
            Thread.Sleep(20);
            Console.Write(@char);
        }

        Console.ForegroundColor = ConsoleColor.Gray;
    }
}