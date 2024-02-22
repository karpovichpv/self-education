public class Program
{
    // [ThreadStatic]
    private static int _counter;

    public static void Main()
    {
        Thread thread = new(Method);
        thread.Start();
        thread.Join();

        Console.WriteLine("Primary thread was finished");

        Console.ReadKey();
    }

    public static void Method()
    {
        if (_counter < 100)
        {
            _counter++;
            Console.WriteLine(
                $"{_counter} - start --- {Thread.CurrentThread.GetHashCode()}");

            Thread thread = new(Method);
            thread.Start();
            thread.Join();
        }

        Console.WriteLine($"Thread {Thread.CurrentThread.GetHashCode()} was finished");
    }
}